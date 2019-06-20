using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace ToastGames.Client.Networking
{
    public class Network : MonoBehaviour
    {
        public byte[] receivedBytes;
        byte[] asyncBuffer;
        public bool isConnected { get; private set; }
        public bool shouldHandleData { get; private set; }
        public TcpClient client { get; private set; }
        public NetworkStream myStream { get; private set; }

        private void Awake()
        {
            UnityThread.initUnityThread();
            shouldHandleData = false;
        }

        public void ConnectIP()
        {
            Debug.Log("서버에 아이피방식으로 접속을 시도합니다.");
            client = new TcpClient { ReceiveBufferSize = NetworkSettings.CLIENT_BUFFERSIZE, SendBufferSize = NetworkSettings.CLIENT_BUFFERSIZE };
            asyncBuffer = new byte[NetworkSettings.CLIENT_BUFFERSIZE * 2];
            try
            {
                client.BeginConnect(NetworkManager.networkSettings.IP, NetworkSettings.PORT, new AsyncCallback(ConnectCallback), client);
            }
            catch
            {
                Debug.Log("서버에 접속할 수 없습니다!");
            }
        }
        
        private void ConnectCallback(IAsyncResult result)
        {
            try
            {
                client.EndConnect(result);
                if (client.Connected == false) { return; }
                else
                {
                    myStream = client.GetStream();
                    myStream.BeginRead(asyncBuffer, 0, NetworkSettings.CLIENT_BUFFERSIZE * 2, OnReceiveData, null);
                    isConnected = true;
                    Debug.Log("서버와의 접속에 성공했습니다!");
                }
            }
            catch (Exception)
            {

                isConnected = false;
                return;
            }
        }

        private void OnReceiveData(IAsyncResult result)
        {
            try
            {
                int packetLength = myStream.EndRead(result);
                receivedBytes = new byte[packetLength];
                Buffer.BlockCopy(asyncBuffer, 0, receivedBytes, 0, packetLength);

                if (packetLength == 0)
                {
                    Debug.LogWarning("서버와의 접속이 끊겼습니다.");
                    UnityThread.ExecuteInUpdate(() =>
                    {
                        Application.Quit();
                    });
                    return;
                }
                UnityThread.ExecuteInUpdate(() =>
                {
                    NetworkHandleData.HandleData(receivedBytes);
                });
                myStream.BeginRead(asyncBuffer, 0, NetworkSettings.CLIENT_BUFFERSIZE * 2, OnReceiveData, null);

            }
            catch (Exception)
            {
                Debug.LogWarning("서버와의 접속이 끊겼습니다.");
                UnityThread.ExecuteInUpdate(() =>
                {
                    Application.Quit();
                });
                return;
            }
        }

        public void Disconnect()
        {
            client.Close();
        }
    }
}