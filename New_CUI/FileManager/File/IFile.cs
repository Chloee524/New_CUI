using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileManager
{
    interface IFile
    {
        bool ReadFile(string path, out List<string> data);
    }
}
