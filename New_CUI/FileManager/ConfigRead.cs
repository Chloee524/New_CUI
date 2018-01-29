using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using DataStructure;

namespace FileManager
{
    class ConfigRead
    {
        private DataStructure.DS _ds = null;
        private const string fileName = "CUI_Config.cfg";
        private string filepath;

        public ConfigRead()
        {
            _ds = DS.Instance;
        }
        public String FilePath
        {
            get { return filepath; }
            set { filepath = value;}
        }


        /// <summary>
        /// Config 파일을 확인하고 Read 하는 Method
        /// </summary>
        /// <returns>-1은 filepath가 null인 경우
        /// -2는 Config 파일이 경로에 없을 경우
        /// 0은 정상적으로 Read 한 경우</returns>
        public int ConfigStart()
        {
            int cvalue = -1;

            if (string.IsNullOrEmpty(filepath)) return cvalue;
            else if (!File.Exists(Path.Combine(filepath, fileName))) return cvalue = -2;

            if (CfgRead()) cvalue = 0;

            return cvalue;
        }

        /// <summary>
        /// Configuration file을 읽는 Method
        /// </summary>
        /// <returns>return 값으로 Comment가 제대로 Parse 되었는지 확인</returns>
        private bool CfgRead()
        {
            bool IsTrue = false;
            foreach (string line in System.IO.File.ReadLines(Path.Combine(filepath, fileName), Encoding.UTF8))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string title = giveTitle(line);
                title = title.ToUpper();
                switch(title){
                    case Command.IP:
                        _ds.IPAddr = giveValue(line);
                        break;
                    case Command.Port:
                        _ds.Port = giveValue(line);
                        break;
                    case Command.Dir:
                        _ds.DIR= giveValue(line);
                        break;
                    case Command.cmd_1:
                    case Command.cmd_2:
                    case Command.cmd_3:
                    case Command.cmd_4:
                    case Command.cmd_5:
                        _ds._Cmd[Int32.Parse(title)] = giveValue(line);
                        break;
                    default:
                        IsTrue = checkComment(line);
                        break;
                }
            }
            return IsTrue;
        }

        /// <summary>
        /// Configuration file에 작성된 IP 또는 Port, Directory에 적힌 값들을
        /// 찾아서 return 값으로 보내줌.
        /// </summary>
        /// <param name="line">IP나 Port, Directory line</param>
        /// <returns>사용자가 작성한 IP, Port, Directory Value</returns>
        private string giveValue(string line)
        {
            string rvalue = string.Empty;
            string[] tdel = { "\t" };

            string[] data = line.Split(tdel, StringSplitOptions.RemoveEmptyEntries);
            if (data.Length == 2)
                rvalue = data[1];

            return rvalue;
        }

        /// <summary>
        /// Configuration file에 작성된 내용의 Title을 찾아서
        /// return 값으로 보내줌.
        /// </summary>
        /// <param name="line">Configuration에 적힌 line</param>
        /// <returns>Configuration에 적힌 내용의 Title</returns>
        private string giveTitle(string line)
        {
            string rvalue = string.Empty;
            string[] tdel = { "\t" };

            string[] data = line.Split(tdel, StringSplitOptions.RemoveEmptyEntries);
            if (data.Length == 2)
                rvalue = data[0];

            return rvalue;
        }

        private bool checkComment(string line)
        {
            bool IsTrue = false;
            string del = "//";

            Regex regex = new Regex(del, RegexOptions.IgnoreCase);

            Match m = regex.Match(line);
            Group g = m.Groups[0];

            if (g.Value.Equals("//")) IsTrue = true;

            return IsTrue;
        }
    }
}
