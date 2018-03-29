using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileManager
{
    class SymbolReader : Reader
    {
        #region Constants
        private const string FileName = "SymbolDataFile.txt";
        #endregion Constants

        #region Constructor
        public SymbolReader(IFile file) : base(file)
        {
            base._filepath = FileName;
        }
        #endregion Constructor

        #region Private Member Functions
        /// <summary>
        /// 읽은 Symbol Data가 없으면 false, 있으면 true
        /// </summary>
        /// <returns></returns>
        private bool ReadSym(List<string> data)
        {
            bool isSuccess = false;
            List<string> symlist = new List<string>();

            //한줄씩 읽으면서 line에 있는 게 comment인지 확인
            //comment가 아니면, 배열의 0번째 값을 Symbol 값으로 인식하고 List에 업데이트
            foreach (string line in data)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (CheckComment(line))
                    continue;

                string symbol = GiveTitle(line);

                if (!string.IsNullOrEmpty(symbol) && !symlist.Contains(symbol))
                    symlist.Add(symbol);
            }

            if (symlist.Count > 1)
            {
                _ds.Symbol = symlist;
                isSuccess = true;
            }

            return isSuccess;
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
                ret = ReadSym(data);
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
