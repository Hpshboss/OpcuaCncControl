using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace UdpClientStructure
{
    [Serializable]
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

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class ScanCmdPacket
    {
        //外部
        public ushort ID;
        public ushort Sz;
        public byte Cmd; //its type is char originally
        public ushort Count;
        public int Sum; //its type is char originally
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class ScanEchoPacket
    {
        public ushort ID;
        public ushort Sz;
        public byte Cmd;
        public ushort Count;
        public byte Sum; //its type is char originally
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class MachIDCmdPacket
    {
        public ushort ID;
        public ushort Sz;
        public byte Cmd;   //its type is char originally
        public short Count;
        public int Sum;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class MachIDEchoPacket
    {
        public ushort ID;
        public ushort Sz;
        public byte Cmd;        //its type is char originally
        public ushort Count;
        public byte ID0;        //its type is char originally
        public byte Ver1;       //its type is char originally
        public byte Ver2;       //its type is char originally
        public ushort BugFix;
        public byte TypeID;
        public byte SubTypeID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] UserDef = new byte[60];
        public byte Sum;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class MachConnectCmdPacket
    {
        public ushort ID;
        public ushort Sz;
        public byte Cmd;
        public ushort Count;
        public ushort DataSz;
        public byte DataCmd0;
        public byte DataCmd1;
        public uint Part;
        public byte ver1;
        public byte ver2;
        public ushort BugFix;
        public byte TypeID;
        public byte SubTypeID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] Password = new byte[60];
        public byte Sum;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class MachConnectEchoPacket
    {
        public int ID;
        public int Sz;
        public int Cmd;
        public int Count;
        public int DataSz;
        public int DataCmd0;
        public int DataCmd1;
        public int Part;
        public int Security;
        public int MachID;
        public int Sum;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class MachDataCmdPacket
    {
        public ushort ID;
        public ushort Sz;
        public byte Cmd;
        public ushort Count;
        public ushort DataSz;
        public byte DataCmd0;
        public byte DataCmd1;
        public uint Part;
        public uint Code;
        public uint Len;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 800)]
        public byte[] DataBuf = new byte[800];
        public byte Sum;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class MachDataEchoPacket
    {
        public int ID;
        public int Sz;
        public int Cmd;
        public int Count;
        public int DataSz;
        public int DataCmd0;
        public int DataCmd1;
        public uint Part;
        public uint Code;
        public uint Len;
        public uint ActctLen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 800)]
        public byte[] DataBuf = new byte[800];
        public int Sum;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class WIRE_MMI_INF01
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public int[] FLAG = new int[16];
        UInt64 DATA_TIME;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 776)]
        public int[] OrtherData = new int[776];
    }
    public enum ME
    {
        Yes = 1,
        scan = 1,
        scanE = 2,
        machid = 3,
        machidE = 4,
        machcon = 5,
        machconE = 6,
        machdata = 7,
        machdataE = 8,
        ConnectEnd = 9
    }

}
