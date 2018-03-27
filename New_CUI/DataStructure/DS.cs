using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStructure
{
    public class DS
    {
        #region Singleton
        private static DS _instance = null;
        #endregion Singleton

        #region Private Member Variables
        private string                  _ipaddr     = string.Empty;
        private string                  _port       = string.Empty;
        private string                  _dir        = string.Empty;
        private List<string>            _symbolList = new List<string>();
        private Dictionary<int, string> _shortCmd   = new Dictionary<int, string>();
        #endregion Private Member Variables

        #region Properties
        public List<string> Symbol
        {
            set { _symbolList = value; }
            get { return _symbolList; }
        }
        public string Dir
        {
            set { _dir = value; }
            get { return _dir; }
        }
        public string IpAddr
        {
            set { _ipaddr = value; }
            get { return _ipaddr; }
        }
        public string Port
        {
            set { _port = value; }
            get { return _port; }
        }
        public Dictionary<int, string> Cmd
        {
            get { return _shortCmd; }
            set { _shortCmd = value; }
        }
        public static DS Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DS();

                return _instance;
            }
        }
        #endregion Properties
    }

    public class Command
    {
        #region Constants
        public const string CmdStartNormal = "START0";
        public const string CmdStartFault  = "START1";
        public const string CmdStop        = "STOP";
        public const string CmdInit        = "INIT";
        public const string IP             = "IP";
        public const string Port           = "PORT";
        public const string Dir            = "DIR";
        public const string Cmd1           = "1";
        public const string Cmd2           = "2";
        public const string Cmd3           = "3";
        public const string Cmd4           = "4";
        public const string Cmd5           = "5";
        public const string CmdTmNormal    = "TestMode[0]";
        public const string CmdTmFault     = "TestMode[1]";
        #endregion Constants
    }
}
