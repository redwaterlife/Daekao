using System;
using System.Net.Sockets;
using ToastPacket;

namespace ToastServer.Networking
{
    class Client
    {
        public int connectionID { get; private set; }

        public string ip { get; private set; }
        private byte[] readBuff;

        public TcpClient socket { get; private set; }
        public NetworkStream myStream { get; private set; }
        public ByteBuffer buffer { get; private set; }

        public void Start(TcpClient _socket, int _connectionID)
        {
            socket = _socket;
            connectionID = _connectionID;
            ip = _socket.Client.RemoteEndPoint.ToString();

            socket.SendBufferSize = 4096;
            socket.ReceiveBufferSize = 4096;
            myStream = socket.GetStream();
            readBuff = new byte[4096];
            myStream.BeginRead(readBuff, 0, socket.ReceiveBufferSize, OnReceiveData, null);

            NetworkSendData.SendSWelcomeMessage(NetworkManager.GetClient(_connectionID).connectionID);
        }

        private void OnReceiveData(IAsyncResult result)
        {
            try
            {
                int readbytes = myStream.EndRead(result);

                if (readbytes <= 0)
                {
                    // 서버와의 접속이 끊긴 경우
                    CloseSocket();
                    return;
                }

                byte[] newBytes = new byte[readbytes];
                Buffer.BlockCopy(readBuff, 0, newBytes, 0, readbytes);
                NetworkHandleData.HandleData(connectionID, newBytes);
                myStream.BeginRead(readBuff, 0, socket.ReceiveBufferSize, OnReceiveData, null);

            }
            catch (Exception)
            {
                CloseSocket();
            }
        }

        public void CloseSocket()
        {
            LogManager.WriteInfo(ip + "와의 통신이 끊겼습니다.");
            socket.Close();
            socket = null;
            //NetworkSendData.SendPlayerQuit(connectionID);
            GameManagement.Players.PlayerManager.GetTempPlayer(connectionID).isPlaying = false;
        }

        public void SetByteBuffer(ByteBuffer _buffer)
        {
            buffer = _buffer;
        }
    }
}
