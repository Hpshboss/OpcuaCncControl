using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Quickstarts.ReferenceServer
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct LaserPACKET  //雷射封包結構體
    {
        public uint DateTime;   //系統時間
        public uint TotalWCount;   //累計工作時間
        public uint ThisWCount;   //目前加工時間
        public byte Unit;
        public byte CStart;
        public byte CStop;
        public byte MemStart;  //MemStart
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public char[] Fn;   //加工程式名稱 FileName
        public uint Nn, Bn;
        public ushort MMode;   //系統模式
        public ushort MStatus;   //機台狀態
        public ushort SCode, Ms;   //抓資料的兩個變數
                                   //------------------------------
        public LaserCOORD Coord;
        public LaserSDATA SData;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct LaserSDATA//<64Byte> //雷射本身加工參數
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public char[] Flag;
        public byte Mode;      //CW\QCW
        public byte Gas;       //Air,N2,O2
        public ushort Power;     //0~10000 unit:0.01%
        public ushort ResW;     //不理他
        public uint Dura;     //0~50000 unit:0.001ms 佔空比
        public uint Hz;       //0~5000000 unit:0.01hz
        public uint RefHt;    //pulse
        public uint Dead;     //pulse
        public ushort AirBar;       //0~50000 unit:0.001bar
        public ushort Kp;        //0~50000 unit:0.001 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public char[] Comment;  //comment
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        public char[] ResvB1;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct LaserCOORD  //各軸參數
    {
        public PT9L Mp, Pp, Lp, Dp;                     //座標群
        public PT9L RefWp, OftWp;
        public double Speed;                          //移動速度
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public uint[] Rpm;      //各軸轉速
    }
    //---------------------------------
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct PT9L
    {
        public int x, y, z, a, b, c, u, v, w;
    }
}
