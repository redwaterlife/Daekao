using System;
using System.Threading;

namespace ToastServer
{
    public class Program
    {
        private static Thread consoleThread;

        static void Main(string[] args)
        {
            consoleThread = new Thread(new ThreadStart(ConsoleThread));
            consoleThread.Start();
            GameManagement.GameManager.Initialize();
            Networking.NetworkManager.InitializeServer();
        }

        static void ConsoleThread()
        {
            while (true)
            {
            }
        }
    }
}
