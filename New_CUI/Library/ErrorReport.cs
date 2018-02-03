using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Library
{
    public class ErrorReport : Interface.IErrorLog
    {
        ///////////////////////////////////////////////////////////////////////////
        //
        //  STRUCT
        //
        ///////////////////////////////////////////////////////////////////////////
        #region STRUCT

        protected struct ErrorMessage
        {
            public DateTime time;
            public string message;
            public Exception exception;
        };

        #endregion  // STRUCT


        ///////////////////////////////////////////////////////////////////////////
        //
        //  VARIABLES
        //
        ///////////////////////////////////////////////////////////////////////////
        #region VARIABLES

        private static volatile ErrorReport _instance = null;
        private List<ErrorMessage> _errorMessage = new List<ErrorMessage>();
        private Interface.DisplayMsgHandler _display;

        private delegate string TimeToString(DateTime time);
        TimeToString ConvTimeToString = (x) => (x).ToString("HH:mm:ss");
        private static readonly object syncLock = new Object();
        //private New_CUI.NCUIForm mainForm = new New_CUI.NCUIForm();

        #endregion  // VARIABLES


        ///////////////////////////////////////////////////////////////////////////
        //
        //  PROPERTIES
        //
        ///////////////////////////////////////////////////////////////////////////
        #region PROPERTIES

        public static ErrorReport Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock(syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ErrorReport();
                        }
                    }
                }
                return _instance;
            }

        }

        public Interface.DisplayMsgHandler DispHandler
        {
            get { return  _display; }
            set { _display = value; }
        }

        protected List<ErrorMessage> GetErrorMessage
        {
            get
            {
                return _errorMessage;
            }
        }

        #endregion  // PROPERTIES


        ///////////////////////////////////////////////////////////////////////////
        //
        //  CONSTRUCTOR
        //
        ///////////////////////////////////////////////////////////////////////////
        #region CONSTRUCTOR

        //protected ErrorReport()
        //{
            
        //}

        ErrorReport()
        {

        }
        #endregion  // CONSTRUCTOR


        ///////////////////////////////////////////////////////////////////////////
        //
        //  METHODS
        //
        ///////////////////////////////////////////////////////////////////////////
        #region METHODS

        #region PRIVATE

        #endregion  // PRIVATE

        #region PROTECTED
        #endregion  // PROTECTED

        #region PUBLIC

        /// <summary>
        /// 오류 메시지를 출력한다.
        /// </summary>
        public void ShowReport()
        {
            try
            {
                if (_display == null)
                    return;

                lock (_errorMessage)
                {
                    foreach (var msg in _errorMessage)
                    {
        #if true // 스레드 작업시
                        string log = string.Empty;
                        log = string.Format("[{0}]> {1} {2}", ConvTimeToString(msg.time), msg.message, Environment.NewLine);
                        _display(log);
        #else
                        string log = string.Empty;
                        log = string.Format("[{0}]> {1} {2}", ConvTimeToString(msg.time), msg.message, Environment.NewLine);
                        _display(log);

        #endif
                    }

                }
            }
            catch (Exception e)
            {
                string logError = string.Format("ERR> {0}{1}", e.Message, Environment.NewLine);
                Console.WriteLine(logError);
            }
            finally
            {
                RemoveAll();
            }
        }

        /// <summary>
        /// 오류 메시지 출력에서 보여질 시간 표기 방법을 입력한다.
        /// </summary>
        /// <param name="str"> 입력) "yyyy-MM-dd hh:mm:ss" 출력) 년-월-일 시:분:초 </param>
        public void SetTimeString(string str)
        {
            ConvTimeToString = (x) => x.ToString(str);
        }

        /// <summary>
        /// 오류 메시지를 저장한다.
        /// </summary>
        /// <param name="msg"> 오류 메시지 </param>
        public void Add(string msg, Exception ex = null)
        {
            DateTime time = new DateTime();
            time = DateTime.Now;
            lock (_errorMessage)
            {                          
                ErrorMessage error = new ErrorMessage();
                error.message = msg;
                error.time = time;
                error.exception = ex;

                _errorMessage.Add(error);
            }
        }
        /// <summary>
        /// 오류 메시지를 저장하고 바로 출력한다.
        /// </summary>
        /// <param name="msg"> 오류 메시지</param>
        public void AddNShow(string msg, Exception ex = null)
        {
            Add(msg, ex);
            ShowReport();
        }

        /// <summary>
        /// 저장된 Exception 정보를 디버깅 한다.
        /// </summary>
        public void Debug()
        {
            lock (_errorMessage)
            {

                StackTrace st = new StackTrace(true);

                foreach (var err in _errorMessage)
                {
                    if (err.exception == null)
                        continue;

                    Console.WriteLine("---------------------------------------------------------------");
                    Console.WriteLine("발생시간: {0}", ConvTimeToString(err.time));
                    Console.WriteLine("발생함수: {0}", err.exception.TargetSite);
                    Console.WriteLine("메시지: {0}", err.exception.Message);
                    Console.WriteLine("[THROW 스택]\n{0}", err.exception.StackTrace);
                    Console.WriteLine("[CATCH 스택]");

                    string stackIndent = "";
                    for (int i = 0; i < st.FrameCount; i++)
                    {
                        if (i >= 5)
                            break;

                        StackFrame sf = st.GetFrame(i);
                        Console.WriteLine(stackIndent + " 함수: {0}", sf.GetMethod());
                        stackIndent += " ";
                    }
                    Console.WriteLine("---------------------------------------------------------------");
                }

                RemoveAll();
            }
        }

        /// <summary>
        /// 저장된 모든 오류 메시지를 삭제한다.
        /// </summary>
        public void RemoveAll()
        {
            lock (_errorMessage)
            {
                _errorMessage.RemoveRange(0, _errorMessage.Count);
            }
        }

        public void Removeat(int indexed, int countno)
        {
            lock (_errorMessage)
            {
                _errorMessage.RemoveRange(indexed, countno);
            }
        }

        #endregion  // PUBLIC

        #endregion  // METHODS

       
    }
}
