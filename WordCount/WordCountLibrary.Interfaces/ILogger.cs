using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCountLibrary.Interfaces
{
    public interface ILogger
    {
        void Debug(string record);
        void Info(string record);
        void Error(string record);
        void Warning(string record);
    }
}
