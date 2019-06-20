using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToastServer.GameManagement.Players
{
    class PlayerManager
    {
        static TempPlayer[] tempPlayers;

        public static void Initialize()
        {
            LogManager.WriteLog("플레이어 인스턴스들을 초기화합니다...");
            tempPlayers = new TempPlayer[Networking.NetworkSettings.MAX_PLAYERS];
            for (int i = 0; i < tempPlayers.Length; i++)
                tempPlayers[i] = new TempPlayer(false);
        }

        public static bool CheckIsPlaying(int _index)
        {
            lock (tempPlayers)
            {
                if (tempPlayers[_index].isPlaying && Networking.NetworkManager.GetClient(_index).socket != null) return true;
                return false;
            }
        }

        public static TempPlayer GetTempPlayer(int _index)
        {
            lock (tempPlayers)
            {
                if (_index > Networking.NetworkSettings.MAX_PLAYERS)
                {
                    LogManager.WriteError("PlayerManager::GetTempPlayer: 매개변수로 전달된 _index가 Networking.NetworkSettings.MAX_PLAYERS을 초과합니다!");
                    return null;
                }
                return tempPlayers[_index];
            }
        }
    }
}
