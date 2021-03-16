using System;
using System.Net.Sockets;
using System.IO;
using System.Net;

namespace Elgar_Server
{
    class Client
    {
        public int index;
        public string IP;
        public TcpClient socket;
        public NetworkStream myStream;
        private ServerHandleData shd;
        public bool closing;
        public byte[] readBuff;

        public void Start()
        {
            shd = new ServerHandleData();
            socket.SendBufferSize = 4096;
            socket.ReceiveBufferSize = 4096;
            myStream = socket.GetStream();
            Array.Resize(ref readBuff, socket.ReceiveBufferSize);
            myStream.BeginRead(readBuff, 0, socket.ReceiveBufferSize, OnReceiveData, null);
        }

        private void OnReceiveData( IAsyncResult ar)
        {
            try
            {
                int readBytes = myStream.EndRead(ar);
                if(readBytes <= 0)
                {
                    CloseSocket(index); //Disconnect the player
                    return;
                }

                byte[] newBytes = null;
                Array.Resize(ref newBytes, readBytes);
                Buffer.BlockCopy(readBuff, 0, newBytes, 0, readBytes);
                //HandleData
                shd.HandleNetworkMessages(index, newBytes);
                myStream.BeginRead(readBuff, 0, socket.ReceiveBufferSize, OnReceiveData, null);
            }
            catch
            {
                CloseSocket(index);
            }
        }

        private void CloseSocket(int index)
        {
            Console.WriteLine("Connection from " + IP + "has been terminated");
            socket.Close();
            socket = null;
        }
    }
}
