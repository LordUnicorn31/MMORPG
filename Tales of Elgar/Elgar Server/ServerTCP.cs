using System;
using System.Net.Sockets;
using System.IO;
using System.Net;
using Bindings;

namespace Elgar_Server
{
    class ServerTCP
    {
        public TcpListener serverSocket;
        public static Client[] clients = new Client[Constants.MAX_PLAYERS];

        public void InitializeNetwork()
        {
            Console.WriteLine("Initializing server network...");
            serverSocket = new TcpListener(IPAddress.Any, 5555);
            serverSocket.Start();
            serverSocket.BeginAcceptTcpClient(OnClientConnect, null);

        }

        private void OnClientConnect(IAsyncResult ar)
        {
            TcpClient client = serverSocket.EndAcceptTcpClient(ar);
            client.NoDelay = false;
            serverSocket.BeginAcceptTcpClient(OnClientConnect, null);

            for(int i = 1; i <= Constants.MAX_PLAYERS; ++i)
            {
                if(clients[i].socket == null)
                {
                    clients[i].socket = client;
                    clients[i].index = i;
                    clients[i].IP = client.Client.RemoteEndPoint.ToString();
                    clients[i].Start();
                    Console.WriteLine("Connection received from" + clients[i].IP);
                    return;
                }
            }
        }
    }
}
