using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;


namespace WordCountLibrary.Interfaces
{
    [ServiceContract]
    public interface IWordCount
    {
        [OperationContract]
        UserResponse CountWordsInStatement(string statement, bool removePunctuation);
    }
}
