using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToastServer.GameManagement.Players;
using ToastPacket;

namespace ToastServer.Networking
{
    class NetworkSendData
    {
        static void SendDataAll(byte[] _data)
        {
            for (int i = 0; i < NetworkSettings.MAX_PLAYERS; i++)
            {
                if (!PlayerManager.CheckIsPlaying(i)) continue;
                SendDataTo(i, _data);
            }
        }

        static void SendDataAllThread(byte[] _data)
        {
            Thread thread = new Thread(delegate () { SendDataAll(_data); });
            thread.Start();
        }

        static void SendDataAllBut(int _connectionID, byte[] _data)
        {
            for (int i = 0; i < NetworkSettings.MAX_PLAYERS; i++)
            {
                if (!PlayerManager.CheckIsPlaying(i)) continue;
                if (i == _connectionID) continue;
                SendDataTo(i, _data);
            }
        }

        static void SendDataAllButThread(int _connectionID, byte[] _data)
        {
            Thread thread = new Thread(delegate () { SendDataAllBut(_connectionID, _data); });
            thread.Start();
        }

        static void SendDataTo(int _connectionID, byte[] _data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((_data.GetUpperBound(0) - _data.GetLowerBound(0)) + 1);
            buffer.WriteBytes(_data);
            NetworkManager.GetClient(_connectionID).myStream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
            buffer.Dispose();
        }

        public static void SendSWelcomeMessage(int _connectionID)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)PacketEnumeration.ServerPackets.AcceptConnection);

            buffer.WriteInt(_connectionID);

            SendDataTo(_connectionID, buffer.ToArray());
            buffer.Dispose();
        }
    }
}
