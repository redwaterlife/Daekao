using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastPacket;

namespace ToastServer.Networking
{
    class NetworkHandleData
    {
        private delegate void ServerPacket(int _connectionID, byte[] _data);
        static Dictionary<int, ServerPacket> packets;
        static int pLength;

        public static void InitializePackets()
        {
            LogManager.WriteLog("네트워크 패킷 초기화를 시작합니다 ...");
            packets = new Dictionary<int, ServerPacket>();
            packets.Add((int)ToastPacket.PacketEnumeration.ClientPackets.AcceptConnection, AcceptConnection);
        }

        public static void HandleData(int _connectionID, byte[] _data)
        {
            byte[] Buffer = (byte[])_data.Clone();

            if (NetworkManager.GetClient(_connectionID).buffer == null)
                NetworkManager.GetClient(_connectionID).SetByteBuffer(new ByteBuffer());

            NetworkManager.GetClient(_connectionID).buffer.WriteBytes(Buffer);

            if (NetworkManager.GetClient(_connectionID).buffer.Count() == 0)
            {
                NetworkManager.GetClient(_connectionID).buffer.Clear();
                return;
            }

            if (NetworkManager.GetClient(_connectionID).buffer.Length() >= 8)
            {
                pLength = NetworkManager.GetClient(_connectionID).buffer.ReadInt(false);
                if (pLength <= 0)
                {
                    NetworkManager.GetClient(_connectionID).buffer.Clear();
                    return;
                }
            }

            while (pLength > 0 & pLength <= NetworkManager.GetClient(_connectionID).buffer.Length() - 8)
            {
                if (pLength <= NetworkManager.GetClient(_connectionID).buffer.Length() - 8)
                {
                    NetworkManager.GetClient(_connectionID).buffer.ReadInt();
                    _data = NetworkManager.GetClient(_connectionID).buffer.ReadBytes((int)pLength);
                    HandleDataPackets(_connectionID, _data);
                }

                pLength = 0;

                if (NetworkManager.GetClient(_connectionID).buffer.Length() >= 8)
                {
                    pLength = NetworkManager.GetClient(_connectionID).buffer.ReadInt(false);

                    if (pLength < 0)
                    {
                        NetworkManager.GetClient(_connectionID).buffer.Clear();
                        return;
                    }
                }
            }
        }

        static void HandleDataPackets(int _connectionID, byte[] _data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(_data);
            int packetIdentifier = buffer.ReadInt();

            buffer.Dispose();

            if (packets.TryGetValue(packetIdentifier, out ServerPacket packet))
                packet.Invoke(_connectionID, _data);
        }

        static void AcceptConnection(int _connectionID, byte[] _data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(_data);
            int packetIdentifier = buffer.ReadInt();
            int status  = buffer.ReadInt();
            LogManager.WriteClientMessage(status.ToString(), _connectionID);
            buffer.Dispose();
            //NetworkSendData.SendPlayerJoin(_connectionID);
        }
    }
}
