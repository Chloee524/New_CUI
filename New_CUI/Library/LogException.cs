using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    class LogException : ApplicationException
    {
        public LogException(string msg)
            : base(msg)
        {
            
        }
    }
}
