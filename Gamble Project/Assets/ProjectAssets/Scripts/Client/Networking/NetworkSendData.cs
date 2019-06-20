using UnityEngine;
using System;
using System.Collections.Generic;
using ToastPacket;
using ToastGames.Client.Extensions;

namespace ToastGames.Client.Networking
{
    public class NetworkSendData
    {
        static void Send(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
            buffer.WriteBytes(data);
            NetworkManager.network.myStream.Write(buffer.ToArray(), 0, buffer.ToArray().Length);
        }

        public static void AcceptConnection()
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInt((int)PacketEnumeration.ClientPackets.AcceptConnection);
            buffer.WriteInt(1);
            // 버전 체크 등의 사항 추가하기
            Send(buffer.ToArray());
            buffer.Dispose();
        }
    }
}