using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using WordCountLibrary.Interfaces;

namespace WordCountClient
{
    public class WordCountMVVM : INotifyPropertyChanged
    {
        private static WordCountMVVM _instance = new WordCountMVVM();

        private UserResponse _firstClientResponse;
        private UserResponse _secondClientResponse;

        private bool _removePunctuation1;
        public bool RemovePunctuation1
        {
            get
            {
                return _removePunctuation1;
            }
            set
            {
                _removePunctuation1 = value;
                OnPropertyChanged("RemovePunctuation1");
            }
        }

        private bool _removePunctuation2;
        public bool RemovePunctuation2
        {
            get
            {
                return _removePunctuation2;
            }
            set
            {
                _removePunctuation2 = value;
                OnPropertyChanged("RemovePunctuation2");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static WordCountMVVM Instance
        {
            get
            {
                return _instance;
            }
        }
       

        private WordCountMVVM()
        { }

        public UserResponse FirstUserResponse
        {
            get
            {
                return _firstClientResponse;
            }
            set
            {
                _firstClientResponse = value;
                OnPropertyChanged("FirstUserResponse");
            }
        }

        public UserResponse SecondUserResponse
        {
            get
            {
                return _secondClientResponse;
            }
            set
            {
                _secondClientResponse = value;
                OnPropertyChanged("SecondUserResponse");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
