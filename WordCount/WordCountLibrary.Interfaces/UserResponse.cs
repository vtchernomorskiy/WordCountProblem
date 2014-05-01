using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.ComponentModel;

namespace WordCountLibrary.Interfaces
{
    [DataContract]
    public class UserResponse : INotifyPropertyChanged
    {
        private string _id;
        private string _statement;
        private DateTime _requestReceivedAt;
        private DateTime _requestCompleteAt;
        private bool _hasError;
        private string _error;
        private KeyValuePair<string, string>[] _snapshot;

        [DataMember]
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        [DataMember]
        public string Statement
        {
            get
            {
                return _statement;
            }
            set
            {
                _statement = value;
                OnPropertyChanged("Statement");
            }
        }

        [DataMember]
        public DateTime RequestReceivedAt
        {
            get
            {
                return _requestReceivedAt;
            }
            set
            {
                _requestReceivedAt = value;
                OnPropertyChanged("RequestReceivedAt");
            }
        }

        [DataMember]
        public DateTime RequestCompleteAt
        {
            get
            {
                return _requestCompleteAt;
            }
            set
            {
                _requestCompleteAt = value;
                OnPropertyChanged("RequestCompleteAt");
            }
        }

        [DataMember]
        public bool HasError
        {
            get
            {
                return _hasError;
            }
            set
            {
                _hasError = value;
                OnPropertyChanged("HasError");
            }
        }

        [DataMember]
        public string Error
        {
            get
            {
                return _error;
            }
            set
            {
                _error = value;
                OnPropertyChanged("Error");
            }
        }

        [DataMember]
        public KeyValuePair<string, string>[] Snapshot
        {
            get
            {
                return _snapshot;
            }
            set
            {
                _snapshot = value;
                OnPropertyChanged("Snapshot");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
