using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace ToastGames.Client.Networking
{
    [RequireComponent(typeof(Network))]
    public class NetworkManager : MonoBehaviour
    {
        public static NetworkSettings networkSettings { get; private set; }
        public static Network network { get; private set; }
        public static NetworkHandleData networkHandleData { get; private set; }
        public static int ConnectionID { get; private set; }
        
        private void Awake()
        {
            ConnectionID = -1;
        }

        void Start()
        {
            networkSettings = new NetworkSettings("null", "127.0.0.1");
            network = GetComponent<Network>();
            networkHandleData = new NetworkHandleData();
            networkHandleData.InitializePackets();
            network.ConnectIP();
        }

        public static void SetConnectionID(int _connectionID)
        {
            if (ConnectionID == -1) ConnectionID = _connectionID;
        }

        private void OnApplicationQuit()
        {
            network.Disconnect();
        }
    }
}