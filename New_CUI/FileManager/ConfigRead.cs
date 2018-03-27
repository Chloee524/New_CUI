using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DataStructure;

namespace FileManager
{
    class ConfigRead : FileRead
    {
        #region Privage Member Variables
        private DataStructure.DS _ds = null;
        private string           _filepath;
        #endregion Private Member Variables

        #region Constants
        private const string FileName = "CUI_Config.cfg";
        #endregion Constants

        #region Properties
        public String FilePath
        {
            get { return _filepath; }
            set { _filepath = value; }
        }
        #endregion Properties

        #region Constructor
        public ConfigRead()
        {
            _ds = DS.Instance;
        }
        #endregion Constructor

        #region Private Member Functions
        /// <summary>
        /// Configuration file을 읽는 Method
        /// </summary>
        /// <returns>return 값으로 Comment가 제대로 Parse 되었는지 확인</returns>
        private bool ReadCfg()
        {
            bool IsTrue = false;

            foreach (string line in System.IO.File.ReadLines(Path.Combine(_filepath, FileName), Encoding.UTF8))
            {
                if (string.IsNullOrWhiteSpace(line)) 
                    continue;

                string title = GiveTitle(line);
                title = title.ToUpper();

                switch (title)
                {
                    case Command.IP:
                        _ds.IpAddr = GiveValue(line);
                        break;
                    case Command.Port:
                        _ds.Port = GiveValue(line);
                        break;
                    case Command.Dir:
                        _ds.Dir = GiveValue(line);
                        break;
                    case Command.Cmd1:
                    case Command.Cmd2:
                    case Command.Cmd3:
                    case Command.Cmd4:
                    case Command.Cmd5:
                        _ds.Cmd[Int32.Parse(title)] = GiveValue(line);
                        break;
                    default:
                        IsTrue = CheckComment(line);
                        break;
                }
            }

            return IsTrue;
        }
        #endregion Private Member Functions

        #region Public Member Functions
        /// <summary>
        /// Config 파일을 확인하고 Read 하는 Method
        /// </summary>
        /// <returns>-1은 filepath가 null인 경우
        /// -2는 Config 파일이 경로에 없을 경우
        /// 0은 정상적으로 Read 한 경우</returns>
        public int StartConfig()
        {
            int cvalue = -1;

            if (string.IsNullOrEmpty(_filepath)) 
                return cvalue;
            else if (!File.Exists(Path.Combine(_filepath, FileName))) 
                return cvalue = -2;

            if (ReadCfg()) 
                cvalue = 0;

            return cvalue;
        }
        #endregion Public Member Functions
    }
}
