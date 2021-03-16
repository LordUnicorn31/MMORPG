using System;
using System.Collections.Generic;
using Bindings;

namespace Tales_of_Elgar
{
    class ClientHandleData
    {
        public PacketBuffer buffer = new PacketBuffer();
        private delegate void Packet_(byte[] data);
        private Dictionary<int, Packet_> packets;

        public void InitializeMessages()
        {
            packets = new Dictionary<int, Packet_>();
        }

        public void HandleNetworkMessages(byte[] data)
        {
            int packetNum; PacketBuffer buffer;
            buffer = new PacketBuffer();

            buffer.AddBytes(data);
            packetNum = buffer.GetInteger();
            buffer.Dispose();

            if (packets.TryGetValue(packetNum, out Packet_ packet))
                packet.Invoke(data);
        }
    }
}
