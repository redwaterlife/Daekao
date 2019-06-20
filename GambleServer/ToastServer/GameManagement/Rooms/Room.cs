using System;
using System.Collections;
using System.Collections.Generic;

namespace ToastServer.GameManagement.Rooms
{
    public enum eRoomType { RSP = 0 }
    public enum eRoomStatus { Waiting, Processing }

    public class Room
    {
        public class RoomProfile
        {
            public RoomProfile(int properPlayerCount, int minPlayerCount)
            {
                this.properPlayerCount = properPlayerCount;
                this.minPlayerCount = minPlayerCount;
            }

            public int properPlayerCount = 2;
            public int minPlayerCount = 2;
        }

        public static RoomProfile[] roomProfiles;

        public List<int> Players { get; protected set; }
        public eRoomType Type { get; private set; }
        public eRoomStatus Status { get; protected set; }
        public int RoomIndex { get; private set; }

        public void SetIndex(int index)
        {
            RoomIndex = index;
        }

        public static void Initialize()
        {
            roomProfiles = new RoomProfile[1];
            roomProfiles[0] = new RoomProfile(2, 2);
        }

        public virtual void Start(eRoomType type)
        {
            Players = new List<int>();
            Type = type;
            Status = eRoomStatus.Waiting;

        }

        public virtual void Process()
        {
            LogManager.WriteRoomLog($"프로세스 진입", RoomIndex);

            switch (Status)
            {
                case eRoomStatus.Waiting:
                    {
                        if (Players.Count >= roomProfiles[(int)Type].properPlayerCount)
                        {
                            StartGame();
                        }
                    }
                    break;
            }
        }

        public virtual void StartGame()
        {
            Status = eRoomStatus.Processing;
            LogManager.WriteRoomLog($"게임을 시작합니다!", RoomIndex);
            ShowPlayers();
        }

        public virtual void AddPlayer(int index)
        {
            Players.Add(index);
            ShowPlayers();
        }

        public void ShowPlayers()
        {
            string players = "";
            for (int i = 0; i < Players.Count; i++) players += Players[i] + " ";
            LogManager.WriteRoomLog(players, RoomIndex);
        }
    }
}