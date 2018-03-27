using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DataStructure;
using System.Threading;
using System.Net.Sockets;
using Library;

namespace socket
{
    public class ClientObject
    {
        #region Properties
        public Byte[] Buffer { get; set; }
        public Socket Socket { get; set; }
        #endregion Properties

        #region Constructor
        public ClientObject(Int32 bufferSize)
        {
            this.Buffer = new Byte[bufferSize];
        }
        #endregion Constructor
    }

    public class ClientSocket
    {
        #region Private Member Variables
        private Socket        _client = null;
        private AsyncCallback _receiverHandler;
        private AsyncCallback _senderHandler;
        #endregion Private Member Vairables

        #region Private Member Functions
        private Byte[] TrimMSG(Byte[] buffer, int size)
        {
            Byte[] result = new Byte[size];
            int j = 0;

            for (int i = 0; i < size; i++)
            {
                if (buffer[i] == 0)
                    continue;
                else
                    result[j++] = buffer[i];
            }

            return result;
        }

        private void HandleDataReceive(IAsyncResult ar)
        {
            // 넘겨진 추가 정보를 가져옵니다.
            // AsyncState 속성의 자료형은 Object 형식이기 때문에 형 변환이 필요합니다~!
            ClientObject co = (ClientObject)ar.AsyncState;

            // 받은 바이트 수 저장할 변수 선언
            Int32 recvBytes;

            try
            {
                if (_client == null) 
                    return;

                // 자료를 수신하고, 수신받은 바이트를 가져옵니다.
                recvBytes = co.Socket.EndReceive(ar);

                // 수신받은 자료의 크기가 1 이상일 때에만 자료 처리
                if (recvBytes > 0)
                {
                    // 공백 문자들이 많이 발생할 수 있으므로, 받은 바이트 수 만큼 배열을 선언하고 복사한다.
                    Byte[] msgByte = new Byte[recvBytes];
                    Array.Copy(co.Buffer, msgByte, recvBytes);

                    /// 받은 메세지 확인
                    /// [CMD]START, [CMD]STOP
                    string received = System.Text.Encoding.UTF8.GetString(msgByte);
                    //CheckCmdfromTEM(received);

                    // 받은 메세지를 출력
                    Handler.LogMsg.AddNShow(received);
                    // 자료 처리가 끝났으면~
                    // 이제 다시 데이터를 수신받기 위해서 수신 대기를 해야 합니다.
                    // Begin~~ 메서드를 이용해 비동기적으로 작업을 대기했다면
                    // 반드시 대리자 함수에서 End~~ 메서드를 이용해 비동기 작업이 끝났다고 알려줘야 합니다!
                    co.Socket.BeginReceive(co.Buffer, 0, co.Buffer.Length, SocketFlags.None, _receiverHandler, co);

                }
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 예외 정보 출력 후 함수를 종료한다.
                Handler.LogMsg.AddNShow("[CUI] Rcv Error: " + ex.Message);
                return;
            }
        }

        private void HandleDataSend(IAsyncResult ar)
        {
            // 넘겨진 추가 정보를 가져옵니다.
            ClientObject co = (ClientObject)ar.AsyncState;

            // 보낸 바이트 수를 저장할 변수 선언
            Int32 sentBytes;

            try
            {
                if (_client == null) 
                    return;

                // 자료를 전송하고, 전송한 바이트를 가져옵니다.
                sentBytes = co.Socket.EndSend(ar);

                if (sentBytes > 0)
                {
                    // 여기도 마찬가지로 보낸 바이트 수 만큼 배열 선언 후 복사한다.
                    Byte[] msgByte = new Byte[sentBytes];
                    Array.Copy(co.Buffer, msgByte, sentBytes);
                    Handler.LogMsg.AddNShow("[Send] " + System.Text.Encoding.UTF8.GetString(msgByte));
                }
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 예외 정보 출력 후 함수를 종료한다.
                Handler.LogMsg.AddNShow("[CUI] Send Error: " + ex.Message);
                return;
            }
        }

        private bool SyncReceiveMessage()
        {
            bool IsReceived = false;
            ClientObject co = new ClientObject(1024); ;
            co.Socket = _client;

            // 받은 바이트 수 저장할 변수 선언
            Int32 recvBytes;

            try
            {
                if (_client == null) 
                    return IsReceived;

                // 자료를 수신하고, 수신받은 바이트를 가져옵니다.
                recvBytes = co.Socket.Receive(co.Buffer);

                // 수신받은 자료의 크기가 1 이상일 때에만 자료 처리
                if (recvBytes > 0)
                {
                    // 공백 문자들이 많이 발생할 수 있으므로, 받은 바이트 수 만큼 배열을 선언하고 복사한다.
                    Byte[] msgByte = new Byte[recvBytes];
                    Array.Copy(co.Buffer, msgByte, recvBytes);

                    // 받은 메세지를 출력
                    Handler.LogMsg.AddNShow(System.Text.Encoding.UTF8.GetString(msgByte));
                    IsReceived = true;
                }
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 예외 정보 출력 후 함수를 종료한다.
                Handler.LogMsg.AddNShow("[CUI] Receive Error : " + ex.Message);
                return IsReceived;
            }

            return IsReceived;
        }
        #endregion Private Member Functions

        #region Public Member Functions
        public bool Connect(string IPaddr, int Port)
        {
            bool IsConnected = false;

            try
            {
                _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                _client.Connect(IPaddr, Port);

                if(_client.Connected)
                {
                    IsConnected = true;
                    _client.ReceiveBufferSize = 1000000;
                    _receiverHandler = new AsyncCallback(HandleDataReceive);
                    _senderHandler = new AsyncCallback(HandleDataSend);

                    // 4096 바이트의 크기를 갖는 바이트 배열을 가진 AsyncObject 클래스 생성
                    ClientObject ao = new ClientObject(4096);

                    // 작업 중인 소켓을 저장하기 위해 sockClient 할당
                    ao.Socket = _client;
                    //SendMessage(Command.cmd_Init);

                    // 비동기적으로 들어오는 자료를 수신하기 위해 BeginReceive 메서드 사용!
                    _client.BeginReceive(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, _receiverHandler, ao);

                    Handler.LogMsg.AddNShow("[CUI] Socket Connection Success ");
                }

            }
            catch (Exception e)
            {
                Handler.LogMsg.AddNShow("[CUI] Connection Error: " + e.Message);
            }

            return IsConnected;
        }
        
        public bool SyncSendMessage(string msg)
        {
            bool IsSent = false;

            ClientObject ao = new ClientObject(1);
            Byte[] buffer = new Byte[1024];
            // 보낸 바이트 수를 저장할 변수 선언
            Int32 sentBytes;

            buffer = Encoding.Unicode.GetBytes(msg);
            ao.Buffer = TrimMSG(buffer, buffer.Length);

            if (_client == null) 
                return IsSent = false;

            ao.Socket = _client;

            try
            {
                SocketError se = new SocketError();
                sentBytes = _client.Send(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, out se);

                if (sentBytes > 0)
                {
                    // 여기도 마찬가지로 보낸 바이트 수 만큼 배열 선언 후 복사한다.
                    Byte[] msgByte = new Byte[sentBytes];
                    Array.Copy(ao.Buffer, msgByte, sentBytes);
                    Handler.LogMsg.AddNShow("[CUI] Send: " + System.Text.Encoding.UTF8.GetString(msgByte));

                    IsSent = true;
                }

                SyncReceiveMessage();
                
            }
            catch (Exception e)
            {
                Handler.LogMsg.AddNShow("[CUI] Send Error: " + e.Message);
            }

            return IsSent;
        }

        public bool SendMessage(string msg)
        {
            // 추가 정보를 넘기기 위한 변수 선언
            // 크기를 설정하는게 의미가 없습니다.
            // 왜냐하면 바로 밑의 코드에서 문자열을 유니코드 형으로 변환한 바이트 배열을 반환하기 때문에
            // 최소한의 크기르 배열을 초기화합니다.
            bool IsSent = false;
            ClientObject ao = new ClientObject(1);
            Byte[] buffer = new Byte[1024];

            // 문자열을 바이트 배열으로 변환
            buffer = Encoding.Unicode.GetBytes(msg);
            ao.Buffer = TrimMSG(buffer, buffer.Length);
            
            if (_client == null) 
                return IsSent = false;

            ao.Socket = _client;

            // 전송 시작!
            try
            {
                _client.BeginSend(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, _senderHandler, ao);
                IsSent = true;
            }
            catch (Exception ex)
            {
                Handler.LogMsg.AddNShow("[CUI] Send Error: " + ex.Message);
            }

            return IsSent;
        }

        public bool Disconnect()
        {
            bool IsDisconnected = false;

            try
            {
                if (_client != null)
                {
                    _client.Close();
                    Handler.LogMsg.AddNShow("[CUI] Socket Disconnection");
                    IsDisconnected = true;
                }
                else
                {
                    Handler.LogMsg.AddNShow("[CUI] There is no Socket to disconnect");
                    
                }
            }
            catch (Exception e)
            {
                Handler.LogMsg.AddNShow("[CUI] Disconnection Error: " + e.Message);
            }
            return IsDisconnected;
        }

        public void exidDisconnet()
        {
            if (_client != null)
            {
                _client.Close();
            }
        }
        #endregion Public Member Functions
    }
}
