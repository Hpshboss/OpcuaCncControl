using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace UdpClientStructure
{
    class Program
    {
        static void Main(string[] args)
        {
            ScanCmdPacket ScanCmdPack = new ScanCmdPacket();
            ScanEchoPacket ScanEchoPack = new ScanEchoPacket();
            MachIDCmdPacket MachIDCmdPack = new MachIDCmdPacket();
            MachIDEchoPacket MachIDEchoPack = new MachIDEchoPacket();
            MachConnectCmdPacket MachConnectCmdPack = new MachConnectCmdPacket();
            MachConnectEchoPacket MachConnectEchoPack = new MachConnectEchoPacket();
            MachDataCmdPacket MachDataCmdPack = new MachDataCmdPacket();
            MachDataEchoPacket MachDataEchoPack = new MachDataEchoPacket();
            WIRE_MMI_INF01 MMI_INF = new WIRE_MMI_INF01();

            PackageSetting(ScanCmdPack, MachIDCmdPack, MachConnectCmdPack, MachDataCmdPack);
            Console.WriteLine(ScanCmdPack.ToString());



            //設定本機IP位址，可用IPAddress.Any自行尋找，或直接使用IPAddress.Parse("X.X.X.X")設定
            IPAddress localAddress = IPAddress.Any;
            //如果為傳送端，使用IPAddress.Parse("X.X.X.X")設定
            IPAddress destAddress = IPAddress.Parse("192.168.0.101");
            ushort portNumber = 0x869C;


            //*******************選擇是否為傳送端*******************//
            bool udpSender = true;

            //預設byte陣列數量
            int bufferSize = 512;

            //初始化物件
            UdpClient udpSocket = null;
            byte[] sendBuffer = new byte[bufferSize], receiveBuffer = new byte[bufferSize];
            int byteSize;
            ushort cmdState = 0;

            try
            {
                while(true)
                {
                    if (udpSender == false)
                    {
                        udpSocket = new UdpClient(new IPEndPoint(localAddress, portNumber));
                    }
                    else
                    {
                        udpSocket = new UdpClient(new IPEndPoint(localAddress, 0));
                    }


                    if (udpSender == true)
                    {
                        udpSocket.Connect(destAddress, portNumber);
                        Console.WriteLine("Connect() is OK...");
                    }

                    if (udpSender == true)
                    {
                        if (cmdState == 0)
                        {
                            Console.WriteLine("Sending the requested number of packets to the destination, Send()...");

                            MemoryStream stream = new MemoryStream();
                            BinaryWriter bw = new BinaryWriter(stream);
                            Console.WriteLine(ScanCmdPack.Sum);
                            bw.Write(ScanCmdPack.ID);
                            bw.Write(ScanCmdPack.Sz);
                            bw.Write(ScanCmdPack.Cmd);
                            bw.Write(ScanCmdPack.Count);
                            bw.Write(ScanCmdPack.Sum);

                            sendBuffer = stream.ToArray();
                            byteSize = udpSocket.Send(sendBuffer, sendBuffer.Length);
                            cmdState = (ushort)ME.scan;

                            Console.WriteLine("Sent {0} bytes to {1}", byteSize, destAddress.ToString());
                            stream.Close();
                            udpSender = false;
                            Console.WriteLine("Change to receiving mode");
                        }

                        if (cmdState == (ushort)ME.scanE)
                        {
                            Console.WriteLine("Sending the requested number of packets to the destination, Send()...");

                            MemoryStream stream = new MemoryStream();
                            BinaryWriter bw = new BinaryWriter(stream);
                            Console.WriteLine(ScanCmdPack.Sum);
                            bw.Write(MachIDCmdPack.ID);
                            bw.Write(MachIDCmdPack.Sz);
                            bw.Write(MachIDCmdPack.Cmd);
                            bw.Write(MachIDCmdPack.Count);
                            bw.Write(MachIDCmdPack.Sum);

                            sendBuffer = stream.ToArray();
                            byteSize = udpSocket.Send(sendBuffer, sendBuffer.Length);
                            cmdState = (ushort)ME.machid;

                            Console.WriteLine("Sent {0} bytes to {1}", byteSize, destAddress.ToString());
                            stream.Close();
                            udpSender = false;
                            Console.WriteLine("Change to receiving mode");
                        }

                        if (cmdState == (ushort)ME.machidE)
                        {
                            Console.WriteLine("Sending the requested number of packets to the destination, Send()...");

                            MemoryStream stream = new MemoryStream();
                            BinaryWriter bw = new BinaryWriter(stream);
                            Console.WriteLine(ScanCmdPack.Sum);
                            bw.Write(MachConnectCmdPack.ID);
                            bw.Write(MachConnectCmdPack.Sz);
                            bw.Write(MachConnectCmdPack.Cmd);
                            bw.Write(MachConnectCmdPack.Count);
                            bw.Write(MachConnectCmdPack.DataSz);
                            bw.Write(MachConnectCmdPack.DataCmd0);
                            bw.Write(MachConnectCmdPack.DataCmd1);
                            bw.Write(MachConnectCmdPack.Part);
                            bw.Write(MachConnectCmdPack.ver1);
                            bw.Write(MachConnectCmdPack.ver2);
                            bw.Write(MachConnectCmdPack.BugFix);
                            bw.Write(MachConnectCmdPack.TypeID);
                            bw.Write(MachConnectCmdPack.SubTypeID);
                            bw.Write(MachConnectCmdPack.Password);
                            bw.Write(MachConnectCmdPack.Sum);

                            sendBuffer = stream.ToArray();
                            byteSize = udpSocket.Send(sendBuffer, sendBuffer.Length);
                            cmdState = (ushort)ME.machcon;

                            Console.WriteLine("Sent {0} bytes to {1}", byteSize, destAddress.ToString());
                            stream.Close();
                            udpSender = false;
                            Console.WriteLine("Change to receiving mode");
                        }

                        if (cmdState == (ushort)ME.machconE)
                        {
                            Console.WriteLine("Sending the requested number of packets to the destination, Send()...");

                            MemoryStream stream = new MemoryStream();
                            BinaryWriter bw = new BinaryWriter(stream);
                            Console.WriteLine(ScanCmdPack.Sum);
                            bw.Write(MachDataCmdPack.ID);
                            bw.Write(MachDataCmdPack.Sz);
                            bw.Write(MachDataCmdPack.Cmd);
                            bw.Write(MachDataCmdPack.Count);
                            bw.Write(MachDataCmdPack.DataSz);
                            bw.Write(MachDataCmdPack.DataCmd0);
                            bw.Write(MachDataCmdPack.DataCmd1);
                            bw.Write(MachDataCmdPack.Part);
                            bw.Write(MachDataCmdPack.Code);
                            bw.Write(MachDataCmdPack.Len);
                            bw.Write(MachDataCmdPack.DataBuf);
                            bw.Write(MachDataCmdPack.Sum);

                            sendBuffer = stream.ToArray();
                            byteSize = udpSocket.Send(sendBuffer, sendBuffer.Length);
                            cmdState = (ushort)ME.machdata;

                            Console.WriteLine("Sent {0} bytes to {1}", byteSize, destAddress.ToString());
                            stream.Close();
                            udpSender = false;
                            Console.WriteLine("Change to receiving mode");
                        }
                    }
                    else
                    {
                        IPEndPoint senderEndPoint = new IPEndPoint(localAddress, 0);
                        Console.WriteLine("Receiving datagrams in a loop...");

                        if (cmdState == (ushort)ME.scan)
                        {
                            receiveBuffer = udpSocket.Receive(ref senderEndPoint);
                            cmdState = (ushort)ME.scanE;
                            Console.WriteLine("It is {0} bytes from {1}", receiveBuffer.Length, senderEndPoint.ToString());

                            MemoryStream stream = new MemoryStream(receiveBuffer);
                            BinaryReader br = new BinaryReader(stream);

                            ScanEchoPack.ID = BitConverter.ToUInt16(br.ReadBytes(2));
                            ScanEchoPack.Sz = BitConverter.ToUInt16(br.ReadBytes(2));
                            ScanEchoPack.Cmd = br.ReadByte();
                            ScanEchoPack.Count = BitConverter.ToUInt16(br.ReadBytes(2));
                            ScanEchoPack.Sum = br.ReadByte();

                            Console.WriteLine("Receive ID = {0}", ScanEchoPack.ID);
                            Console.WriteLine("Receive Sz = {0}", ScanEchoPack.Sz);
                            Console.WriteLine("Receive Cmd = {0}", ScanEchoPack.Cmd);
                            Console.WriteLine("Receive Count = {0}", ScanEchoPack.Count);
                            Console.WriteLine("Receive Sum = {0}", ScanEchoPack.Sum);

                        }

                        if (cmdState == (ushort)ME.machid)
                        {
                            receiveBuffer = udpSocket.Receive(ref senderEndPoint);
                            cmdState = (ushort)ME.machidE;
                            Console.WriteLine("It is {0} bytes from {1}", receiveBuffer.Length, senderEndPoint.ToString());

                            MemoryStream stream = new MemoryStream(receiveBuffer);
                            BinaryReader br = new BinaryReader(stream);

                            MachIDEchoPack.ID = BitConverter.ToUInt16(br.ReadBytes(2));
                            MachIDEchoPack.Sz = BitConverter.ToUInt16(br.ReadBytes(2));
                            MachIDEchoPack.Cmd = br.ReadByte();
                            MachIDEchoPack.Count = BitConverter.ToUInt16(br.ReadBytes(2));
                            MachIDEchoPack.ID0 = br.ReadByte();
                            MachIDEchoPack.Ver1 = br.ReadByte();
                            MachIDEchoPack.Ver2 = br.ReadByte();
                            MachIDEchoPack.BugFix = BitConverter.ToUInt16(br.ReadBytes(2));
                            MachIDEchoPack.TypeID = br.ReadByte();
                            MachIDEchoPack.SubTypeID = br.ReadByte();
                            MachIDEchoPack.UserDef = br.ReadBytes(60);
                            MachIDEchoPack.Sum = br.ReadByte();

                            Console.WriteLine("Receive ID = {0}", MachIDEchoPack.ID);
                            Console.WriteLine("Receive Sz = {0}", MachIDEchoPack.Sz);
                            Console.WriteLine("Receive Cmd = {0}", MachIDEchoPack.Cmd);
                            Console.WriteLine("Receive Count = {0}", MachIDEchoPack.Count);
                            Console.WriteLine("Receive ID0 = {0}", MachIDEchoPack.ID0);
                            Console.WriteLine("Receive Ver1 = {0}", MachIDEchoPack.Ver1);
                            Console.WriteLine("Receive Ver2 = {0}", MachIDEchoPack.Ver2);
                            Console.WriteLine("Receive BugFix = {0}", MachIDEchoPack.BugFix);
                            Console.WriteLine("Receive TypeID = {0}", MachIDEchoPack.TypeID);
                            Console.WriteLine("Receive SubTypeID = {0}", MachIDEchoPack.SubTypeID);
                            Console.WriteLine("Receive UserDef");
                            Console.WriteLine("Receive Sum = {0}", MachIDEchoPack.Sum);

                        }

                        if (cmdState == (ushort)ME.machcon)
                        {
                            receiveBuffer = udpSocket.Receive(ref senderEndPoint);
                            cmdState = (ushort)ME.machconE;
                            Console.WriteLine("It is {0} bytes from {1}", receiveBuffer.Length, senderEndPoint.ToString());

                            MemoryStream stream = new MemoryStream(receiveBuffer);
                            BinaryReader br = new BinaryReader(stream);

                            MachConnectEchoPack.ID = BitConverter.ToUInt16(br.ReadBytes(2));
                            MachConnectEchoPack.Sz = BitConverter.ToUInt16(br.ReadBytes(2));
                            MachConnectEchoPack.Cmd = br.ReadByte();
                            MachConnectEchoPack.Count = BitConverter.ToUInt16(br.ReadBytes(2));
                            MachConnectEchoPack.DataSz = BitConverter.ToUInt16(br.ReadBytes(2));
                            MachConnectEchoPack.DataCmd0 = br.ReadByte();
                            MachConnectEchoPack.DataCmd1 = br.ReadByte();
                            MachConnectEchoPack.Part = BitConverter.ToUInt16(br.ReadBytes(4));
                            MachConnectEchoPack.Security = BitConverter.ToUInt16(br.ReadBytes(4));
                            MachConnectEchoPack.MachID = BitConverter.ToUInt16(br.ReadBytes(2));
                            MachConnectEchoPack.Sum = br.ReadByte();

                            Console.WriteLine("Receive ID = {0}", MachConnectEchoPack.ID);
                            Console.WriteLine("Receive Sz = {0}", MachConnectEchoPack.Sz);
                            Console.WriteLine("Receive Cmd = {0}", MachConnectEchoPack.Cmd);
                            Console.WriteLine("Receive Count = {0}", MachConnectEchoPack.Count);
                            Console.WriteLine("Receive DataSz = {0}", MachConnectEchoPack.DataSz);
                            Console.WriteLine("Receive DataCmd0 = {0}", MachConnectEchoPack.DataCmd0);
                            Console.WriteLine("Receive DataCmd1 = {0}", MachConnectEchoPack.DataCmd1);
                            Console.WriteLine("Receive Part = {0}", MachConnectEchoPack.Part);
                            Console.WriteLine("Receive Security = {0}", MachConnectEchoPack.Security);
                            Console.WriteLine("Receive MachID = {0}", MachConnectEchoPack.MachID);
                            Console.WriteLine("Receive Sum = {0}", MachIDEchoPack.Sum);

                        }

                        if (cmdState == (ushort)ME.machdata)
                        {
                            receiveBuffer = udpSocket.Receive(ref senderEndPoint);
                            cmdState = (ushort)ME.machdataE;
                            Console.WriteLine("It is {0} bytes from {1}", receiveBuffer.Length, senderEndPoint.ToString());

                            MemoryStream stream = new MemoryStream(receiveBuffer);
                            BinaryReader br = new BinaryReader(stream);

                            MachDataEchoPack.ID = BitConverter.ToUInt16(br.ReadBytes(2));
                            MachDataEchoPack.Sz = BitConverter.ToUInt16(br.ReadBytes(2));
                            MachDataEchoPack.Cmd = br.ReadByte();
                            MachDataEchoPack.Count = BitConverter.ToUInt16(br.ReadBytes(2));
                            MachDataEchoPack.DataSz = BitConverter.ToUInt16(br.ReadBytes(2));
                            MachDataEchoPack.DataCmd0 = br.ReadByte();
                            MachDataEchoPack.DataCmd1 = br.ReadByte();
                            MachDataEchoPack.Part = BitConverter.ToUInt32(br.ReadBytes(4));
                            MachDataEchoPack.Code = BitConverter.ToUInt32(br.ReadBytes(4));
                            MachDataEchoPack.Len = BitConverter.ToUInt32(br.ReadBytes(4));
                            MachDataEchoPack.ActctLen = BitConverter.ToUInt32(br.ReadBytes(4));
                            MachDataEchoPack.DataBuf = br.ReadBytes(800);
                            MachDataEchoPack.Sum = br.ReadByte();

                            Console.WriteLine("Receive ID = {0}", MachDataEchoPack.ID);
                            Console.WriteLine("Receive Sz = {0}", MachDataEchoPack.Sz);
                            Console.WriteLine("Receive Cmd = {0}", MachDataEchoPack.Cmd);
                            Console.WriteLine("Receive Count = {0}", MachDataEchoPack.Count);
                            Console.WriteLine("Receive DataSz = {0}", MachDataEchoPack.DataSz);
                            Console.WriteLine("Receive DataCmd0 = {0}", MachDataEchoPack.DataCmd0);
                            Console.WriteLine("Receive DataCmd1 = {0}", MachDataEchoPack.DataCmd1);
                            Console.WriteLine("Receive Part = {0}", MachDataEchoPack.Part);
                            Console.WriteLine("Receive Code = {0}", MachDataEchoPack.Code);
                            Console.WriteLine("Receive Len = {0}", MachDataEchoPack.Len);
                            Console.WriteLine("Receive ActctLen = {0}", MachDataEchoPack.ActctLen);
                            Console.WriteLine("Receive DataBuf");
                            Console.WriteLine("Receive Sum = {0}", MachDataEchoPack.Sum);

                        }
                    }
                }
                
            }
            catch (SocketException err)
            {
                Console.WriteLine("Socket error occurred: {0}", err.Message);
                Console.WriteLine("Stack: {0}", err.StackTrace);
            }
            finally
            {
                if (udpSocket != null)
                {
                    // Free up the underlying network resources
                    Console.WriteLine("Closing the socket...");
                    udpSocket.Close();
                }
            }
        }


        static void PackageSetting(ScanCmdPacket ScanCmdPack, MachIDCmdPacket MachIDCmdPack,
                                              MachConnectCmdPacket MachConnectCmdPack, MachDataCmdPacket MachDataCmdPack)
        {
            //   ScanCmdPacket
            ScanCmdPack.ID = 0x0;
            ScanCmdPack.Sz = 0x0;
            ScanCmdPack.Cmd = 0x20;
            ScanCmdPack.Count = 0x0;
            int Sum1 = ScanCmdPack.ID + ScanCmdPack.Sz + ScanCmdPack.Cmd + ScanCmdPack.Count;
            ScanCmdPack.Sum = (0x100 - Sum1) & 0xFF;

            //   MachIDCmdPacket
            MachIDCmdPack.ID = 0x0;
            MachIDCmdPack.Sz = 0x0;
            MachIDCmdPack.Cmd = 0x21;
            MachIDCmdPack.Count = 0x0;
            int Sum2 = MachIDCmdPack.ID + MachIDCmdPack.Sz + MachIDCmdPack.Cmd + MachIDCmdPack.Count;
            MachIDCmdPack.Sum = (0x100 - Sum2) & 0xFF;

            //   MachConnectCmdPacket
            MachConnectCmdPack.ID = 1;
            MachConnectCmdPack.Sz = 0x4a;
            MachConnectCmdPack.Cmd = 0x22;
            MachConnectCmdPack.Count = 0;
            MachConnectCmdPack.DataSz = 0x42;
            MachConnectCmdPack.DataCmd0 = 0x03;
            MachConnectCmdPack.DataCmd1 = 0;
            MachConnectCmdPack.Part = 0;
            MachConnectCmdPack.ver1 = 4;
            MachConnectCmdPack.ver2 = 3;
            MachConnectCmdPack.BugFix = 7;
            MachConnectCmdPack.TypeID = 0x10;
            MachConnectCmdPack.SubTypeID = 0xa0;
            Array.Clear(MachConnectCmdPack.Password, 0x00, 60);
            for (int i = 0; i < 4; i++)
            {
                MachConnectCmdPack.Password[i] = (byte)0;
            }
            int Sum3 = (int)(MachConnectCmdPack.ID + MachConnectCmdPack.Sz + MachConnectCmdPack.Cmd + MachConnectCmdPack.Count +
                        MachConnectCmdPack.DataSz + MachConnectCmdPack.DataCmd0 + MachConnectCmdPack.DataCmd1 +
                        MachConnectCmdPack.Part + MachConnectCmdPack.ver1 + MachConnectCmdPack.ver2 + MachConnectCmdPack.BugFix +
                        MachConnectCmdPack.TypeID + MachConnectCmdPack.SubTypeID);

            //+ Array.ConvertAll<char, int>(MachConnectCmdPack.Password, value => Convert.ToInt32(value));
            MachConnectCmdPack.Sum = (byte)((0x100 - Sum3) & 0xFF);

            //   MachDataCmdPacket
            MachDataCmdPack.ID = 1;
            MachDataCmdPack.Sz = 0x330;
            MachDataCmdPack.Cmd = 0x01;
            MachDataCmdPack.DataSz = 0x328;
            MachDataCmdPack.DataCmd0 = 0x50;
            MachDataCmdPack.DataCmd1 = 0;
            MachDataCmdPack.Part = 0;
            MachDataCmdPack.Code = 0xaa1;
            MachDataCmdPack.Len = 0x320;
            //memset((char*)MachDataCmdPack.DataBuf, 0x00, sizeof(DataSent));
            Array.Clear(MachDataCmdPack.DataBuf, 0x00, 800);
            int Sum4 = (int)(MachDataCmdPack.ID + MachDataCmdPack.Sz + MachDataCmdPack.Cmd + MachDataCmdPack.DataSz +
                       MachDataCmdPack.DataCmd0 + MachDataCmdPack.DataCmd1 + MachDataCmdPack.Part + MachDataCmdPack.Code
                       + MachDataCmdPack.Len);
            MachDataCmdPack.Sum = (byte)((0x100 - Sum4) & 0xFF);

            Console.WriteLine($"ScanCmdPack is{0}", ScanCmdPack);
            Console.WriteLine($"MachIDCmdPack is{0}", MachIDCmdPack);
            Console.WriteLine($"MachConnectCmdPack is{0}", MachConnectCmdPack);
            Console.WriteLine($"MachDataCmdPack is{0}", MachDataCmdPack);

        }
    }
}
