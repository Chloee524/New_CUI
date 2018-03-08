using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStructure
{
    public class DS
    {
        private static DS _instance = null;
        private string ipaddr = string.Empty;
        private string port = string.Empty;
        private string dir = string.Empty;
        private Dictionary<int, string> ShortCmd = new Dictionary<int, string>();
        private List<string> symbolList = new List<string>();

        public List<string> Symbol
        {
            set { symbolList = value; }
            get { return symbolList; }
        }
        public String DIR
        {
            set { dir = value; }
            get { return dir; }
        }
        public String IPAddr
        {
            set { ipaddr = value; }
            get { return ipaddr; }
        }
        public String Port
        {
            set { port = value; }
            get { return port; }
        }
        public Dictionary<int, string> _Cmd
        {
            get { return ShortCmd; }
            set { ShortCmd = value; }
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
    }

    public class Command
    {
        //public const string cmd_Start = "START";
        public const string cmd_Start_Normal = "START0";
        public const string cmd_Start_Fault = "START1";
        public const string cmd_Stop = "STOP";
        public const string cmd_Init = "INIT";
        public const string IP = "IP";
        public const string Port = "PORT";
        public const string Dir = "DIR";
        public const string cmd_1 = "1";
        public const string cmd_2 = "2";
        public const string cmd_3 = "3";
        public const string cmd_4 = "4";
        public const string cmd_5 = "5";
        public const string cmd_tm_Normal = "TestMode[0]";
        public const string cmd_tm_Fault = "TestMode[1]";
    }
}
