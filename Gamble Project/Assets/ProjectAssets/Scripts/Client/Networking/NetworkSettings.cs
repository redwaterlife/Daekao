namespace ToastGames.Client.Networking
{
    [System.Serializable]
    public class NetworkSettings
    {
        public NetworkSettings(string _domain, string _ip)
        {
            Domain = _domain;
            IP = _ip;
        }

        public string Domain { get; private set; }
        public string IP { get; private set; }
        public const int PORT = 17370;
        public const int CLIENT_BUFFERSIZE = 4096;

        public void SetDomain(string _domain)
        {
            Domain = _domain;
        }

        public void SetIP (string _ip)
        {
            IP = _ip;
        }
    }
}