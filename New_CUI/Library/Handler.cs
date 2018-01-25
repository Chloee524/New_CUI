using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library
{
    public class Handler
    {
        public static Interface.IErrorLog LogMsg = Library.ErrorReport.Instance as Interface.IErrorLog;
    }
}