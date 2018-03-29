using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataStructure;

namespace FileManager
{
    class ConfigReader : Reader
    {
        #region Constants
        private const string FileName = "CUI_Config.cfg";
        #endregion Constants

        #region Constructor
        public ConfigReader(IFile file) : base(file)
        {
            base._filepath = FileName;
        }
        #endregion Constructor

        #region Private Member Functions
        /// <summary>
        /// file에 작성된 Title이외의 내용 값들을
        /// 찾아서 return 값으로 보내줌.
        /// </summary>
        /// <param name="line">IP나 Port, Directory line</param>
        /// <returns>사용자가 작성한 IP, Port, Directory Value</returns>
        private string GiveValue(string line)
        {
            string rvalue = string.Empty;
            string[] tdel = { "\t" };

            string[] data = line.Split(tdel, StringSplitOptions.RemoveEmptyEntries);

            if (data.Length == 2)
                rvalue = data[1];

            return rvalue;
        }

        private bool CheckIpValidation(string ipAddress)
        {
            System.Net.IPAddress ipClass;

            const char delimeter = '.';

            string[] ip = ipAddress.Split(delimeter);

            if (ip.Length != 4)
                return false;

            return System.Net.IPAddress.TryParse(ipAddress, out ipClass);
        }

        private void ParseLine(string line)
        {
            string title = GiveTitle(line);
            title = title.ToUpper();

            string val = GiveValue(line);

            switch (title)
            {
                case Command.IP:
                    if(CheckIpValidation(val))
                        _ds.IpAddr = val;
                    break;
                case Command.Port:
                    int port;
                    if (Int32.TryParse(val, out port))
                        _ds.Port = val;
                    break;
                case Command.Dir:
                    _ds.Dir = val;
                    break;
                case Command.Cmd1:
                case Command.Cmd2:
                case Command.Cmd3:
                case Command.Cmd4:
                case Command.Cmd5:
                    _ds.Cmd[Int32.Parse(title)] = val;
                    break;
                default:
                    /* What to do ? */
                    break;
            }
        }
        /// <summary>
        /// Configuration file을 읽는 Method
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool ReadCfg(List<string> data)
        {
            foreach (string line in data)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                ParseLine(line);
            }

            return true;
        }
        #endregion Private Member Functions

        #region Reader Implementation
        public override bool Read()
        {
            List<string> data;
            string path = _filepath;
            bool ret = false;

            if (!_file.ReadFile(path, out data))
            {
                _errMsg = "경로가 제대로 설정되지 않았거나, 경로에 파일이 없습니다";
                return false;
            }

            try
            {
                ret = ReadCfg(data);
            }
            catch (Exception ex)
            {
                _errMsg = ex.Message;
            }

            return ret;
        }
        #endregion Reader Implementation
    }
}
