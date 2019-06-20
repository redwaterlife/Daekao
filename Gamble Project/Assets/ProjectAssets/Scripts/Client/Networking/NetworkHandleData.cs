using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;
using ToastPacket;
using ToastGames.Client.Extensions;

namespace ToastGames.Client.Networking
{
    public class NetworkHandleData
    {
        public static ByteBuffer playerBuffer { get; private set; }
        private delegate void ServerPacket(byte[] data);
        private static Dictionary<int, ServerPacket> packets;
        private static long pLength;

        public void InitializePackets()
        {
            Debug.Log("네트워크 패킷 초기화를 시작합니다 ...");
            packets = new Dictionary<int, ServerPacket>();
            packets.Add((int)PacketEnumeration.ServerPackets.AcceptConnection, AcceptConnection);
        }

        public static void HandleData(byte[] data)
        {
            byte[] Buffer;
            Buffer = (byte[])data.Clone();

            if (playerBuffer == null) { playerBuffer = new ByteBuffer(); };

            playerBuffer.WriteBytes(Buffer);

            if (playerBuffer.Count() == 0)
            {
                playerBuffer.Clear();
                return;
            }

            if (playerBuffer.Length() >= 8)
            {
                pLength = playerBuffer.ReadLong(false);
                if (pLength <= 0)
                {
                    playerBuffer.Clear();
                    return;
                }
            }

            while (pLength > 0 & pLength <= playerBuffer.Length() - 8)
            {
                if (pLength <= playerBuffer.Length() - 8)
                {
                    playerBuffer.ReadLong(); //Reads out the Packet Identifier;
                    data = playerBuffer.ReadBytes((int)pLength); // Gets the full package Length
                    HandleDataPackets(data);
                }

                pLength = 0;

                if (playerBuffer.Length() >= 8)
                {
                    pLength = playerBuffer.ReadLong(false);

                    if (pLength < 0)
                    {
                        playerBuffer.Clear();
                        return;
                    }
                }
            }
        }

        private static void HandleDataPackets(byte[] _datas)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(_datas);
            int packetIdentifier = buffer.ReadInt();
            buffer.Dispose();

            if (packets.TryGetValue(packetIdentifier, out ServerPacket packet))
                packet.Invoke(_datas);
        }

        private static void AcceptConnection(byte[] datas)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(datas);
            int connectionID = buffer.ReadInt();
            Debug.Log(connectionID);
            NetworkManager.SetConnectionID(connectionID);
            NetworkSendData.AcceptConnection();
            buffer.Dispose();
        }
    }
}