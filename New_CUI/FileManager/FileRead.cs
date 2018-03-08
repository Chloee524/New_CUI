using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FileManager
{
    public class FileRead
    {
        public bool checkComment(string line)
        {
            bool IsTrue = false;
            string del = "//";

            Regex regex = new Regex(del, RegexOptions.IgnoreCase);

            Match m = regex.Match(line);
            Group g = m.Groups[0];

            if (g.Value.Equals("//")) IsTrue = true;

            return IsTrue;
        }

        /// <summary>
        /// file에 작성된 Title이외의 내용 값들을
        /// 찾아서 return 값으로 보내줌.
        /// </summary>
        /// <param name="line">IP나 Port, Directory line</param>
        /// <returns>사용자가 작성한 IP, Port, Directory Value</returns>
        public string giveValue(string line)
        {
            string rvalue = string.Empty;
            string[] tdel = { "\t" };

            string[] data = line.Split(tdel, StringSplitOptions.RemoveEmptyEntries);
            if (data.Length == 2)
                rvalue = data[1];

            return rvalue;
        }

        /// <summary>
        /// file에 작성된 내용의 Title을 찾아서
        /// return 값으로 보내줌.
        /// </summary>
        /// <param name="line">Configuration에 적힌 line</param>
        /// <returns>Configuration에 적힌 내용의 Title</returns>
        public string giveTitle(string line)
        {
            string rvalue = string.Empty;
            string[] tdel = { "\t" };

            string[] data = line.Split(tdel, StringSplitOptions.RemoveEmptyEntries);
            //if (data.Length == 2)
                rvalue = data[0];

            return rvalue;
        }
    }
}
