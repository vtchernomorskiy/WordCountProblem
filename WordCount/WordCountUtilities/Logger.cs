using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using WordCountLibrary.Interfaces;

namespace WordCountUtilities
{
    public class Logger : ILogger
    {
        private readonly ILog _log;

        public Logger(Type tp)
        {
            _log = log4net.LogManager.GetLogger(tp);
        }

        public void Debug(string record)
        {
            _log.Debug(record);
        }

        public void Info(string record)
        {
            _log.Info(record);
        }

        public void Error(string record)
        {
            _log.Error(record);
        }

        public void Warning(string record)
        {
            _log.Warn(record);
        }
    }
}
