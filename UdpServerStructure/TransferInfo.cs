using System;
using System.Collections.Generic;
using System.Text;

namespace UdpClientStructure
{
    public class TransferInfo
    {
        public ushort ID;
        public ushort Sz;
        public byte Cmd;
        public ushort Count;
        public byte Sum;
        public TransferInfo(ushort ID, ushort Sz, byte Cmd, ushort Count, byte Sum)
        {
            this.ID = ID;
            this.Sz = Sz;
            this.Cmd = Cmd;
            this.Count = Count;
            this.Sum = Sum;
        }
    }
}
