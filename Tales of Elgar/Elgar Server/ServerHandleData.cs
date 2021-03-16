using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bindings;

namespace Elgar_Server
{
    class ServerHandleData
    {
        private delegate void Packet_(int index, byte[] data);
        private static Dictionary<int, Packet_> packets;

        public void InitializeMessages()
        {
            Console.WriteLine("Initializing network packets");
            packets = new Dictionary<int, Packet_>();
            packets.Add((int)ClientPackets.Clogin, HandleLogIn);
        }

        public void HandleNetworkMessages(int index, byte[] data)
        {
            int packetNum; PacketBuffer buffer;
            buffer = new PacketBuffer();

            buffer.AddBytes(data);
            packetNum = buffer.GetInteger();
            buffer.Dispose();

            if (packets.TryGetValue(packetNum, out Packet_ packet))
                packet.Invoke(index, data);
        }

        private void HandleLogIn(int index, byte[] data)
        {
            Console.WriteLine("Got Network message");
        }
    }
}
