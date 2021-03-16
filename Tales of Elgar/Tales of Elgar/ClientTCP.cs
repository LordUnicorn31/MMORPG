using System;
using System.Net.Sockets;
using Bindings;

namespace Tales_of_Elgar
{
    class ClientTCP
    {
        public TcpClient playerSocket;
        private static NetworkStream myStream;
        private ClientHandleData chd;
        private byte[] asyncBuff;
        private bool connecting;
        private bool connected;

        public void ConnectToServer()
        {
            if(playerSocket != null)
            {
                if (playerSocket.Connected || connected)
                    return;
                playerSocket.Close();
                playerSocket = null;
            }
            playerSocket = new TcpClient();
            chd = new ClientHandleData();
            playerSocket.ReceiveBufferSize = 4096;
            playerSocket.SendBufferSize = 4096;
            playerSocket.NoDelay = false;
            Array.Resize(ref asyncBuff, 8192);
            playerSocket.BeginConnect("127.0.0.1", 5555, new AsyncCallback(ConnectCallback), playerSocket);
            connecting = true;
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            playerSocket.EndConnect(ar);
            if(playerSocket.Connected == false)
            {
                connecting = false;
                connected = false;
                return;
            }

            else
            {
                playerSocket.NoDelay = true;
                myStream = playerSocket.GetStream();
                myStream.BeginRead(asyncBuff, 0, 8192, OnReceive, null);
                connected = true;
                connecting = false;
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            int byteAnt = myStream.EndRead(ar);
            byte[] myBytes = null;
            Array.Resize(ref myBytes, byteAnt);
            Buffer.BlockCopy(asyncBuff, 0, myBytes, 0, byteAnt);

            if (byteAnt == 0)
                return;

            // Handle network packets
            chd.HandleNetworkMessages(myBytes);
            myStream.BeginRead(asyncBuff, 0, 8192, OnReceive, null);
        }

        public void SendData(byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddBytes(data);
            myStream.Write(buffer.ToArray(), 0, buffer.ToArray().Length);
            buffer.Dispose();
        }

        public void SendLogIn()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddInteger((int)ClientPackets.Clogin);
            buffer.AddString("Jordi");
            buffer.AddString("Jordi2");
            SendData(buffer.ToArray());
            buffer.Dispose();
        }
    }
}
