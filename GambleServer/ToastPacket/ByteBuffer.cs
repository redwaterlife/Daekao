using System;
using System.Collections.Generic;
using System.Text;

namespace ToastPacket
{
    public class ByteBuffer : IDisposable
    {
        #region Variables
        private List<byte> buffer;
        private byte[] readBuffer;
        private int readPosition;
        private bool bufferUpdated = false;
        #endregion

        #region Functions
        public ByteBuffer()
        {
            buffer = new List<byte>();
            readPosition = 0;
        }

        public long GetReadPos()
        {
            return readPosition;
        }

        public byte[] ToArray()
        {
            return buffer.ToArray();
        }

        public int Count()
        {
            return buffer.Count;
        }

        public int Length()
        {
            return Count() - readPosition;
        }

        public void Clear()
        {
            buffer.Clear();
            readPosition = 0;
        }
        #endregion

        #region Write
        public void WriteBytes(byte[] Input)
        {
            buffer.AddRange(Input);
            bufferUpdated = true;
        }

        public void WriteShort(short Input)
        {
            buffer.AddRange(BitConverter.GetBytes(Input));
            bufferUpdated = true;
        }

        public void WriteInt(int Input)
        {
            buffer.AddRange(BitConverter.GetBytes(Input));
            bufferUpdated = true;
        }

        public void WriteLong(long Input)
        {
            buffer.AddRange(BitConverter.GetBytes(Input));
            bufferUpdated = true;
        }

        public void WriteFloat(float Input)
        {
            buffer.AddRange(BitConverter.GetBytes(Input));
            bufferUpdated = true;
        }

        public void WriteString(string Input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Input);
            buffer.AddRange(BitConverter.GetBytes(bytes.Length));
            buffer.AddRange(bytes);
            bufferUpdated = true;
        }

        public void WriteFloat2(float _x, float _y)
        {
            WriteFloat(_x);
            WriteFloat(_y);
        }

        public void WriteFloat3(float _x, float _y, float _z)
        {
            WriteFloat(_x);
            WriteFloat(_y);
            WriteFloat(_z);
        }

        public void WriteFloat4(float _x, float _y, float _z, float _w)
        {
            WriteFloat(_x);
            WriteFloat(_y);
            WriteFloat(_z);
            WriteFloat(_w);
        }
        #endregion

        #region Read
        public byte[] ReadBytes(int Length, bool Peek = true)
        {
            if (bufferUpdated)
            {
                readBuffer = buffer.ToArray();
                bufferUpdated = false;
            }

            byte[] value = buffer.GetRange(readPosition, Length).ToArray();

            if (Peek)
            {
                readPosition += Length;
            }
            return value;
        }

        public short ReadShort(bool Peek = true)
        {
            if (buffer.Count > readPosition)
            {
                if (bufferUpdated)
                {
                    readBuffer = buffer.ToArray();
                    bufferUpdated = false;
                }

                short value = BitConverter.ToInt16(readBuffer, readPosition);

                if (Peek & buffer.Count > readPosition)
                {
                    readPosition += 2;
                }
                return value;
            }
            else
            {
                throw new Exception("ByteBuffer::ReadShort:읽을 수 있는 한도를 초과했어요! 프로토콜을 다시 한번 확인해 보세요!");
            }
        }

        public int ReadInt(bool Peek = true)
        {
            if (buffer.Count > readPosition)
            {
                if (bufferUpdated)
                {
                    readBuffer = buffer.ToArray();
                    bufferUpdated = false;
                }

                int value = BitConverter.ToInt32(readBuffer, readPosition);
                if (Peek & buffer.Count > readPosition)
                {
                    readPosition += 4;
                }
                return value;
            }
            else
            {
                throw new Exception("ByteBuffer::ReadInt32:읽을 수 있는 한도를 초과했어요! 프로토콜을 다시 한번 확인해 보세요!");
            }
        }

        public long ReadLong(bool Peek = true)
        {
            if (buffer.Count > readPosition)
            {
                if (bufferUpdated)
                {
                    readBuffer = buffer.ToArray();
                    bufferUpdated = false;
                }

                long value = BitConverter.ToInt64(readBuffer, readPosition);
                if (Peek & buffer.Count > readPosition)
                {
                    readPosition += 8;
                }
                return value;
            }
            else
            {
                throw new Exception("ByteBuffer::ReadLong:읽을 수 있는 한도를 초과했어요! 프로토콜을 다시 한번 확인해 보세요!");
            }
        }

        public float ReadFloat(bool Peek = true)
        {
            if (buffer.Count > readPosition)
            {
                if (bufferUpdated)
                {
                    readBuffer = buffer.ToArray();
                    bufferUpdated = false;
                }

                float value = BitConverter.ToSingle(readBuffer, readPosition);
                if (Peek & buffer.Count > readPosition)
                {
                    readPosition += 4;
                }
                return value;
            }
            else
            {
                throw new Exception("ByteBuffer::ReadFloat:읽을 수 있는 한도를 초과했어요! 프로토콜을 다시 한번 확인해 보세요!");
            }
        }

        public string ReadString(bool Peek = true)
        {
            int stringLength = ReadInt(true);

            if (bufferUpdated)
            {
                readBuffer = buffer.ToArray();
                bufferUpdated = false;
            }

            string value = Encoding.UTF8.GetString(readBuffer, readPosition, stringLength);
            if (Peek & buffer.Count > readPosition)
            {
                readPosition += stringLength;
            }
            return value;
        }
        #endregion

        #region IDisposable Interface
        private bool disposedValue = false;
        protected virtual void Dispose(bool dispoing)
        {
            if (!disposedValue)
            {
                if (dispoing)
                {
                    buffer.Clear();
                    readPosition = 0;
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
