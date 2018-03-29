using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DataStructure;

namespace FileManager
{
    abstract class Reader
    {
        #region protected Member Variables
        protected IFile            _file;
        protected string           _filepath;
        protected string           _errMsg;
        protected DataStructure.DS _ds = null;
        #endregion protected Member Variables

        #region Constructor
        public Reader(IFile file)
        {
            _file = file;
            _ds = DS.Instance;
        }
        #endregion Constructor

        #region Protected Member Functions
        /// <summary>
        /// file에 작성된 내용의 Title을 찾아서
        /// return 값으로 보내줌.
        /// </summary>
        /// <param name="line">Configuration에 적힌 line</param>
        /// <returns>Configuration에 적힌 내용의 Title</returns>
        protected string GiveTitle(string line)
        {
            string rvalue = string.Empty;
            string[] tdel = { "\t" };
            string[] data = line.Split(tdel, StringSplitOptions.RemoveEmptyEntries);
            rvalue = data[0];

            return rvalue;
        }

        protected bool CheckComment(string line)
        {
            bool   isTrue = false;
            string del    = "//";
            Regex  regex  = new Regex(del, RegexOptions.IgnoreCase);
            Match  m      = regex.Match(line);
            Group  g      = m.Groups[0];

            if (g.Value.Equals("//"))
                isTrue = true;

            return isTrue;
        }
        #endregion Protected Member Functions

        #region Abstract Functions
        public abstract bool Read();
        #endregion Abstract Functions

        #region Public Member Functions
        public string GetLastErrMsg()
        {
            return _errMsg;
        }
        #endregion Public Member Functions
    }
}
