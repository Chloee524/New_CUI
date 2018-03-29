using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using DataStructure;

namespace New_CUI.FileManager
{
    class LogFileWrite
    {
        #region Private Member Variables
        private bool             _isSuccess = false;
        private string           _filepath  = string.Empty;
        private string           _buffer;
        private StreamWriter     _sw        = null;
        private DataStructure.DS _ds        = null;
        #endregion Private Member Variables

        #region Constants
        private readonly Int32 Buffersize = 64000;
        #endregion Constants

        #region Properties
        public String FilePath
        {
            get { return _filepath; }
            set { _filepath = value; }
        }
        #endregion Properties

        #region Constants
        public LogFileWrite()
        {
            _ds = DS.Instance;
        }
        #endregion Constants

        #region Public Member Functions
        public bool OpenLogFile()
        {
            _sw        = new StreamWriter(_filepath, true, System.Text.UTF8Encoding.UTF8);
            _buffer    = string.Empty;
            _isSuccess = true;
            
            return _isSuccess;
        }

        public void WriteLogFile(string msg)
        {
            if(!_isSuccess) 
                return;

            if (_buffer.Length + msg.Length+1 >= Buffersize)
            {
                _sw.Write(_buffer);
                _buffer = string.Empty;
                _buffer = msg;
            }
            else
            {
                _buffer = _buffer + msg;
            }
        }

        public void CloseLogFile()
        {
            // 남아있는 buffer 사이즈 확인 후, length가 0보다 이상이면
            // 오픈한 로그 파일에 저장을 하고,
            // 파일을 클로즈 한다.
            if (!_isSuccess) 
                return;

            if (_buffer.Length > 0)
            {
                _sw.Write(_buffer);
            }

            _sw.Close();
            _isSuccess = false;
        }
        #endregion Public Member Functions
    }
}
