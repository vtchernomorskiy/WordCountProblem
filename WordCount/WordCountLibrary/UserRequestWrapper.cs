using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using WordCountUtilities;
using WordCountLibrary.Interfaces;

namespace WordCountLibrary
{
    internal class UserRequestWrapper
    {
        public string Statement { get; set; }
        public KeyValuePair<string, string>[] Snapshot { get; set; }
        public readonly AutoResetEvent WaitForRequestToComplete;
        public readonly string Id;
        public bool HasError { get; set; }
        public string Error { get; set; }
        public DateTime RequestReceivedAt { get; set; }
        public DateTime RequestCompleteAt { get; set; }
        public bool RemovePunctuation { get; set; }

        public UserRequestWrapper()
        {
            Id = Guid.NewGuid().ToString();
            WaitForRequestToComplete = new AutoResetEvent(false);
        }

        public UserResponse ToUserResonse()
        {
            return new UserResponse 
            { 
                Error = Error, 
                HasError = HasError, 
                RequestCompleteAt = RequestCompleteAt, 
                RequestReceivedAt = RequestReceivedAt,
                Statement = Statement,
                Id = Id,
                Snapshot = Snapshot
            };
        }
    }
}
