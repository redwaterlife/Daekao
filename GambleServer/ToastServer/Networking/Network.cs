using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;


namespace ToastServer.Networking
{
    class Network
    {
        public static Client[] Client = new Client[NetworkSettings.MAX_PLAYERS];
        static TcpListener serverSocket;
        
        public static void InitializeClients()
        {
            for (int i = 0; i < NetworkSettings.MAX_PLAYERS; i++)
                Client[i] = new Client();
        }

        public static void InitializeNetwork()
        {
            if (NetworkSettings.IP == "0.0.0.0")
                serverSocket = new TcpListener(IPAddress.Any, NetworkSettings.PORT);
            else
                serverSocket = new TcpListener(IPAddress.Parse(NetworkSettings.IP), NetworkSettings.PORT);
            serverSocket.Start();
            serverSocket.BeginAcceptTcpClient(OnClientConnect, null);
        }

        static void OnClientConnect(IAsyncResult result)
        {
            TcpClient client = serverSocket.EndAcceptTcpClient(result);
            serverSocket.BeginAcceptTcpClient(OnClientConnect, null);
            for (int i = 0; i < NetworkSettings.MAX_PLAYERS; i++)
            {
                if (Client[i].socket == null)
                {
                    Client[i].Start(client, i);
                    LogManager.WriteInfo(Client[i].ip + "와의 통신을 시작합니다. 아이디: " + Client[i].connectionID);
                    return;
                }
            }
        }
    }
}
