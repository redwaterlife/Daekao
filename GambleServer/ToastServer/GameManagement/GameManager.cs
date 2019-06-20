using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastServer.GameManagement.Players;
using ToastServer.GameManagement.Rooms;

namespace ToastServer.GameManagement
{
    class GameManager
    {
        public static void Initialize()
        {
            LogManager.WriteLog("게임 데이터 초기화를 시작합니다...");
            PlayerManager.Initialize();
            RoomManager.Initialize();
        }
    }
}
