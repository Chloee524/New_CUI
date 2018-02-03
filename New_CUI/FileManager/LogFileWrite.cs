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
        private readonly Int32 buffersize = 64000;
        private readonly Int32 marginsize = 100;

        private DataStructure.DS _ds = null;
        private string filepath = string.Empty;
        private StreamWriter sw = null;
        private string buffer;
        private string margin;
        private bool IsSuccess = false;

        public String FilePath
        {
            get { return filepath; }
            set { filepath = value; }
        }

        public LogFileWrite()
        {
            _ds = DS.Instance;
        }

        public bool logfileopen()
        {
            sw = new StreamWriter(filepath, true, System.Text.UTF8Encoding.UTF8);
            buffer = string.Empty;
            IsSuccess = true;
            
            return IsSuccess;
        }

        public void logfilewrite(string msg)
        {
            if(!IsSuccess) return;

            if (buffer.Length + msg.Length+1 >= buffersize)
            {
                sw.Write(buffer);
                buffer = string.Empty;
                buffer = msg;
            }
            else
            {
                buffer = buffer + msg;
            }
        }

        public void logfileclose()
        {
            // 남아있는 buffer 사이즈 확인 후, length가 0보다 이상이면
            // 오픈한 로그 파일에 저장을 하고,
            // 파일을 클로즈 한다.
            if (!IsSuccess) return;

            if (buffer.Length > 0)
            {
                sw.Write(buffer);
            }
            sw.Close();
            IsSuccess = false;
        }
    }
}
