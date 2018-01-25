using System;

namespace Interface
{
    public delegate void DisplayMsgHandler(string msg);

    public interface IErrorLog
    {
        DisplayMsgHandler DispHandler { get; set; }
        void Add(string msg, Exception ex = null);
        void RemoveAll();
        void ShowReport();
        void Debug();
        void AddNShow(string msg, Exception ex = null);
        void Removeat(int indexed, int countno);
    }
}
