using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WordCountLibrary.Interfaces;
using WordCountUtilities;

namespace WordCountLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class WordCounter : IDisposable, IWordCount
    {
        private const int MAX_ACTIVE_USER_REQUESTS = 10; // TODO PUT IN CONFIG

        private readonly Dictionary<string, Task> _activeUserRequests;
        private readonly ILogger _logger;
        private readonly IConfig _config;

        private Semaphore _servedUserRequests;

        public WordCounter()
        {
            _activeUserRequests = new Dictionary<string, Task>();
            _servedUserRequests = new Semaphore(MAX_ACTIVE_USER_REQUESTS, MAX_ACTIVE_USER_REQUESTS);
            _logger = new Logger(typeof(WordCounter));
        }

        public WordCounter(ILogger logger, IConfig config)
        {
            _activeUserRequests = new Dictionary<string, Task>();
            _servedUserRequests = new Semaphore(MAX_ACTIVE_USER_REQUESTS, MAX_ACTIVE_USER_REQUESTS);
            _logger = logger;
            _config = config;
        }

        public void Dispose()
        {
            DestroySemaphore();

            try
            {
                lock (_activeUserRequests)
                {
                    Task.WaitAll(_activeUserRequests.Values.ToArray());
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
        }

        public UserResponse CountWordsInStatement(string statement, bool removePunctuation)
        {
            var request = new UserRequestWrapper { Statement = statement, RequestReceivedAt = DateTime.Now, RemovePunctuation = removePunctuation };

            try
            {
                AddUserRequest(request);

                if (!request.WaitForRequestToComplete.WaitOne(-1))
                {
                    throw new InvalidOperationException(
                        string.Format(
                            "Timed out waiting for request id {1} being processed",
                            request.Id));
                }

                return request.ToUserResonse();
            }
            catch (Exception ex)
            {
                return new UserResponse { Id = request.Id, HasError = true, Error = ex.ToString() };
            }
        }

        internal void AddUserRequest(UserRequestWrapper userRequest)
        {
            // If we're in shutdown mode and destroy semaphore at this point
            // the requests being served just return an error, 
            // but the service still will exit gracefully
            _servedUserRequests.WaitOne();

            lock (_activeUserRequests)
            {
                if (_activeUserRequests.ContainsKey(userRequest.Id)) // this can never happen, but just in case
                {
                    throw new InvalidOperationException(string.Format("Request id {0} already exists", userRequest.Id));
                }

                _activeUserRequests.Add(userRequest.Id, Task.Factory.StartNew(new Action<object>(o =>
                {

                    RunUserRequest(o);

                }), userRequest));
            }
            
        }

        private void DestroySemaphore()
        {
            try
            {
                _servedUserRequests.Dispose();
            }
            catch // We are shutting down, no need to handle exception here
            {
            }
        }

        private void RemoveUserRequest(string requestId)
        {
            lock (_activeUserRequests)
            {
                try
                {
                    _activeUserRequests.Remove(requestId);
                }
                catch (Exception ex)
                {
                    _logger.Error(string.Format("Error processing request id {0}: {1}", requestId, ex.ToString()));
                }
            }
        }

        private void RunUserRequest(object args)
        {
            var requestId = "N/A";
            try
            {
                var userRequest = (UserRequestWrapper)args;

                requestId = userRequest.Id;

                try
                {
                    // Heart of the process
                    userRequest.Statement // 1. Take a statement
                        .ToWordArray(userRequest.RemovePunctuation) // 2. Split into words with/without removal of punctuation
                        .CountWordOccurences() //3. Transform into word-counter pairs
                        .ToList()               
                        .ForEach(
                            countedWord =>  // and add each one to dictionary
                                {
                                    userRequest.Snapshot = SharedDictionary.CheckIncrement(countedWord.Word, countedWord.Count);
                                });
                }
                catch (Exception ex)
                {
                    _logger.Error(string.Format("Error processing request id {0}: {1}", requestId, ex.ToString()));
                    userRequest.HasError = true;
                    userRequest.Error = ex.ToString();
                }
                finally
                {
                    RemoveUserRequest(requestId);
                    try
                    {
                        _servedUserRequests.Release();
                    }
                    catch
                    {
                    }
                    userRequest.WaitForRequestToComplete.Set();
                    userRequest.RequestCompleteAt = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Error processing request id {0}: {1}", requestId, ex.ToString()));
            }
        }
    }
}
