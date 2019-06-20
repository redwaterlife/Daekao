using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToastPacket;

namespace ToastGames.Client.Extensions
{
    public static class ByteBufferExtensions
    {
        public static void WriteVector3(this ByteBuffer buffer, Vector3 _vector3)
        {
            buffer.WriteFloat3(_vector3.x, _vector3.y, _vector3.z);
        }

        public static void WriteQuaternion(this ByteBuffer buffer, Quaternion _quaternion)
        {
            buffer.WriteFloat4(_quaternion.x, _quaternion.y, _quaternion.z, _quaternion.w);
        }

        public static Vector3 ReadVector3(this ByteBuffer buffer)
        {
            return new Vector3(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());
        }

        public static Quaternion ReadQuaternion(this ByteBuffer buffer)
        {
            return new Quaternion(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());
        }
    }
}