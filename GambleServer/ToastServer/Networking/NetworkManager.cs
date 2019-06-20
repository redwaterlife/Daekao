using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToastServer.Networking
{
    class NetworkManager
    {
        public static void InitializeServer()
        {
            int startTime = 0;
            int endTime = 0;
            startTime = GetTickCount();

            LogManager.WriteLog("서버 구동을 시작합니다!");
            LogManager.WriteLog("게임 데이터 초기화를 시작합니다 ...");
            Network.InitializeClients();

            LogManager.WriteLog("서버 구축을 시작합니다 ...");
            NetworkHandleData.InitializePackets();
            Network.InitializeNetwork();

            endTime = GetTickCount();
            LogManager.WriteInfo("서버 초기화가 끝났습니다. 소요시간: " + (endTime - startTime) + "ms");
        }

        public static Client GetClient(long _index)
        {
            return Network.Client[_index];
        }

        public static int GetTickCount()
        {
            return Environment.TickCount;
        }

        public static void MoveScene()
        {

        }
    }
}
