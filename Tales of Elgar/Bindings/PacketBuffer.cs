using System;
using System.Collections.Generic;
using System.Text;

namespace Bindings
{
    class PacketBuffer : IDisposable
    {
        List<byte> buff;
        byte[] readBuff;
        int readPos;
        bool buffUpdate = false;

        public PacketBuffer()
        {
            buff = new List<byte>();
            readPos = 0;
        }

        public int GetReadPos()
        {
            return readPos;
        }

        public byte[] ToArray()
        {
            return buff.ToArray();
        }

        public int Count()
        {
            return buff.Count;
        }

        public int Length()
        {
            return Count() - readPos;
        }

        public void Clear()
        {
            buff.Clear();
            readPos = 0;
        }

        //Write data
        public void AddInteger(int Input)
        {
            buff.AddRange(BitConverter.GetBytes(Input));
            buffUpdate = true;
        }

        public void AddFloat(float Input)
        {
            buff.AddRange(BitConverter.GetBytes(Input));
            buffUpdate = true;
        }

        public void AddString(string Input)
        {
            buff.AddRange(BitConverter.GetBytes(Input.Length));
            buff.AddRange(Encoding.ASCII.GetBytes(Input));
            buffUpdate = true;

        }

        public void AddByte(byte Input)
        {
            buff.Add(Input);
            buffUpdate = true;
        }

        public void AddBytes(byte[] Input)
        {
            buff.AddRange(Input);
            buffUpdate = true;

        }

        public void AddShort(short Input)
        {
            buff.AddRange(BitConverter.GetBytes(Input));
            buffUpdate = true;

        }

        //Read Data
        public int GetInteger(bool peek = true)
        {
            if(buff.Count > readPos)
            {
                if(buffUpdate)
                {
                    readBuff = buff.ToArray();
                    buffUpdate = false;
                }

                int ret = BitConverter.ToInt32(readBuff, readPos);

                if(peek & buff.Count > readPos)
                {
                    readPos += 4;
                }

                return ret;
            }

            else
            {
                throw new Exception("Packet Buffer is limit");
            }
        }

        public float GetFloat(bool peek = true)
        {
            if (buff.Count > readPos)
            {
                if (buffUpdate)
                {
                    readBuff = buff.ToArray();
                    buffUpdate = false;
                }

                float ret = BitConverter.ToSingle(readBuff, readPos);

                if (peek & buff.Count > readPos)
                {
                    readPos += 4;
                }

                return ret;
            }

            else
            {
                throw new Exception("Packet Buffer is limit");
            }
        }

        public string GetString(bool peek = true)
        {
            int length = GetInteger(true);
        
            if (buffUpdate)
            {
                readBuff = buff.ToArray();
                buffUpdate = false;
            }

            string ret = Encoding.ASCII.GetString(readBuff, readPos, length);

            if (peek & buff.Count > readPos)
            {
                if(ret.Length > 0)
                {
                    readPos += length;
                }
            }

            return ret;
          
        }

        public byte GetByte(bool peek = true)
        {
            if (buff.Count > readPos)
            {
                if (buffUpdate)
                {
                    readBuff = buff.ToArray();
                    buffUpdate = false;
                }

                byte ret = readBuff[readPos];

                if (peek & buff.Count > readPos)
                {
                    readPos += 1;
                }

                return ret;
            }

            else
            {
                throw new Exception("Packet Buffer is limit");
            }
        }

        public byte[] GetBytes(int length, bool peek = true)
        {
            if(buffUpdate)
            {
                readBuff = buff.ToArray();
                buffUpdate = false;
            }

            byte[] ret = buff.GetRange(readPos, length).ToArray();

            if(peek)
            {
                readPos += length;
            }

            return ret;
        }

        private bool disposedValue = false;

        //iDisposable
        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposedValue)
            {
                if(disposing)
                {
                    buff.Clear();
                }

                readPos = 0;
            }

            this.disposedValue = true;

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
