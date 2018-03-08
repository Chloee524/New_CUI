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
        private DataStructure.DS _ds = null;
        private const string fileName = "SymbolDataFile.txt";
        private string filepath = string.Empty;

        public SymbolRead()
        {
            _ds = DS.Instance;
        }

        public String FilePath
        {
            get { return filepath; }
            set { filepath = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SymbolStart()
        {
            int rvalue = -1;

            if (string.IsNullOrEmpty(filepath)) return rvalue;
            else if (!File.Exists(Path.Combine(filepath, fileName))) return rvalue = -2;

            if (SymRead()) rvalue = 0;

            return rvalue;
        }

        /// <summary>
        /// 읽은 Symbol Data가 없으면 0, 있으면 1
        /// </summary>
        /// <returns></returns>
        public bool SymRead()
        {
            bool IsSuccess = false;
            List<string> symlist = new List<string>();

            //한줄씩 읽으면서 line에 있는 게 comment인지 확인
            //comment가 아니면, 배열의 0번째 값을 Symbol 값으로 인식하고 List에 업데이트

            foreach (string line in System.IO.File.ReadLines(Path.Combine(filepath, fileName), Encoding.UTF8))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                if (checkComment(line)) continue;

                string symbol = giveTitle(line);
                
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
    }
}
