using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;

namespace ToastServer.GameManagement.Rooms
{
    public class RoomManager
    {
        public const int MAX_ROOM_COUNT = 12;
        public static Room[] rooms = new Room[MAX_ROOM_COUNT];
        public static List<int> roomsPool = new List<int>();
        public static List<int> enabledRoomsPool = new List<int>();

        public static Queue<int>[] matchMakingQueue = new Queue<int>[1];

        private static Timer timer;
        public static int j = 0;
        public static void Initialize()
        {
            if (rooms[0] != null) return;

            for (int i = 0; i < rooms.Length; i++)
            {
                rooms[i] = new Room();
                rooms[i].SetIndex(i);
                roomsPool.Add(i);
            }

            for (int i = 0; i < matchMakingQueue.Length; i++)
            {
                matchMakingQueue[i] = new Queue<int>();
            }

            Room.Initialize();

            timer = new Timer();
            timer.Interval = 3000;

            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public static void AddPlayerToQueue(int index, eRoomType type)
        {
            matchMakingQueue[(int)type].Enqueue(index);
        }
        
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            AddPlayerToQueue(j++, eRoomType.RSP);

            for (int i = 0; i < enabledRoomsPool.Count; i++)
            {
                rooms[enabledRoomsPool[i]].Process();
            }

            for (int i = 0; i < matchMakingQueue.Length; i++)
            {
                Queue<int> queue = matchMakingQueue[i];
                if (queue.Count >= Room.roomProfiles[i].properPlayerCount)
                {
                    StartRoom(i);
                }
            }
        }

        private static void StartRoom(int type)
        {
            if (roomsPool.Count < 1) return;
            Queue<int> queue = matchMakingQueue[type];
            int roomIndex = roomsPool[0];
            roomsPool.RemoveAt(0);
            Room room = rooms[roomIndex];

            enabledRoomsPool.Add(roomIndex);
            room.Start((eRoomType)type);
            for (int i = 0; i < Room.roomProfiles[type].properPlayerCount; i++)
            {
                int player = queue.Dequeue();
                room.AddPlayer(player);
            }
            switch ((eRoomType)type)
            {
                case eRoomType.RSP:
                    {
                        room = new RSPRoom();
                    }
                    break;
            }

        }
    }
}