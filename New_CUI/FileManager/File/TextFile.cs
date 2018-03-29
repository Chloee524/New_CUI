using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FileManager
{
    class TextFile : IFile
    {
        #region IFile Interface
        public bool ReadFile(string path, out List<string> data)
        {
            data = null;

            if (string.IsNullOrEmpty(path))
                return false;

            if (!File.Exists(path))
                return false;

            data = new List<string>();

            using (StreamReader sr = File.OpenText(path))
            {
                string s = string.Empty;

                while ((s = sr.ReadLine()) != null)
                {
                    data.Add(s);
                }
            }

            return true;
        }
        #endregion IFile Interface
    }
}
