using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCountUtilities
{
    public class InputStatementValidationException : Exception
    {
        public InputStatementValidationException(string msg)
            : base(msg)
        { }
    }
}
