using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bindings;

namespace Elgar_Server
{
    class General
    {
        private ServerTCP stcp;
        private ServerHandleData shd;

        public void InitializeServer()
        {
            stcp = new ServerTCP();
            shd = new ServerHandleData();

            shd.InitializeMessages();

            for(int i = 1; i < Constants.MAX_PLAYERS; ++i)
            {
                ServerTCP.clients[i] = new Client();
            }
            stcp.InitializeNetwork();
            Console.WriteLine("Server Initialized");
        }
    }
}
