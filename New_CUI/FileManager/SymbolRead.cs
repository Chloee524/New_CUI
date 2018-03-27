using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DataStructure;

namespace FileManager
{
    class SymbolRead : FileRead
    {
        #region Private Member Variables
        private DataStructure.DS _ds       = null;
        private string           _filepath = string.Empty;
        #endregion Private Member Variabls

        #region Constants
        private const string FileName = "SymbolDataFile.txt";
        #endregion Constants

        #region Constructor
        public SymbolRead()
        {
            _ds = DS.Instance;
        }
        #endregion Constructor

        public String FilePath
        {
            get { return _filepath; }
            set { _filepath = value; }
        }

        #region Public Member Functions
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int StartSymbol()
        {
            int rvalue = -1;

            if (string.IsNullOrEmpty(_filepath)) 
                return rvalue;
            else if (!File.Exists(Path.Combine(_filepath, FileName))) 
                return rvalue = -2;

            if (ReadSym()) 
                rvalue = 0;

            return rvalue;
        }
        /// <summary>
        /// 읽은 Symbol Data가 없으면 0, 있으면 1
        /// </summary>
        /// <returns></returns>
        public bool ReadSym()
        {
            bool IsSuccess = false;
            List<string> symlist = new List<string>();

            //한줄씩 읽으면서 line에 있는 게 comment인지 확인
            //comment가 아니면, 배열의 0번째 값을 Symbol 값으로 인식하고 List에 업데이트
            foreach (string line in System.IO.File.ReadLines(Path.Combine(_filepath, FileName), Encoding.UTF8))
            {
                if (string.IsNullOrWhiteSpace(line)) 
                    continue;

                if (CheckComment(line)) 
                    continue;

                string symbol = GiveTitle(line);
                
                if(!string.IsNullOrEmpty(symbol))
                    symlist.Add(symbol);
            }

            if (symlist.Count > 1)
            {
                _ds.Symbol = symlist;
                IsSuccess = true;
            }
                
            return IsSuccess;
        }
        #endregion Public Member Functions
    }
}
