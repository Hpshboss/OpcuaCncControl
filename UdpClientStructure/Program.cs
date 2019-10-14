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
                        Console.WriteLine("Sent {0} bytes to {1}", byteSize, destAddress.ToString());
                        stream.Close();
                        udpSender = false;
                        Console.WriteLine("Change to receiving mode");

                    }
                    else
                    {
                        IPEndPoint senderEndPoint = new IPEndPoint(localAddress, 0);
                        Console.WriteLine("Receiving datagrams in a loop...");

                        while (true)
                        {
                            receiveBuffer = udpSocket.Receive(ref senderEndPoint);
                            MemoryStream stream = new MemoryStream(receiveBuffer);
                            /*
                            Console.WriteLine("Read string \"{0}\"", Encoding.ASCII.GetString(receiveBuffer));
                            */
                            Console.WriteLine("It is {0} bytes from {1}", receiveBuffer.Length, senderEndPoint.ToString());
                            BinaryReader br = new BinaryReader(stream);
                            TransferInfo InfoContentReceived = new TransferInfo(0, 0, 0, 0, 0);
                            InfoContentReceived.ID = BitConverter.ToUInt16(br.ReadBytes(2));
                            InfoContentReceived.Sz = BitConverter.ToUInt16(br.ReadBytes(2));
                            InfoContentReceived.Cmd = br.ReadByte();
                            InfoContentReceived.Count = BitConverter.ToUInt16(br.ReadBytes(2));
                            InfoContentReceived.Sum = br.ReadByte();
                            Console.WriteLine("Receive ID = {0}", InfoContentReceived.ID);
                            Console.WriteLine("Receive Sz = {0}", InfoContentReceived.Sz);
                            Console.WriteLine("Receive Cmd = {0}", InfoContentReceived.Cmd);
                            Console.WriteLine("Receive Count = {0}", InfoContentReceived.Count);
                            Console.WriteLine("Receive Sum = {0}", InfoContentReceived.Sum);
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
                MachConnectCmdPack.Password[i] = '0';
            }
            int Sum3 = MachConnectCmdPack.ID + MachConnectCmdPack.Sz + MachConnectCmdPack.Cmd + MachConnectCmdPack.Count +
                        MachConnectCmdPack.DataSz + MachConnectCmdPack.DataCmd0 + MachConnectCmdPack.DataCmd1 +
                        MachConnectCmdPack.Part + MachConnectCmdPack.ver1 + MachConnectCmdPack.ver2 + MachConnectCmdPack.BugFix +
                        MachConnectCmdPack.TypeID + MachConnectCmdPack.SubTypeID;

            //+ Array.ConvertAll<char, int>(MachConnectCmdPack.Password, value => Convert.ToInt32(value));
            MachConnectCmdPack.Sum = (0x100 - Sum3) & 0xFF;

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
            int Sum4 = MachDataCmdPack.ID + MachDataCmdPack.Sz + MachDataCmdPack.Cmd + MachDataCmdPack.DataSz +
                       MachDataCmdPack.DataCmd0 + MachDataCmdPack.DataCmd1 + MachDataCmdPack.Part + MachDataCmdPack.Code
                       + MachDataCmdPack.Len;
            MachDataCmdPack.Sum = (0x100 - Sum4) & 0xFF;

            Console.WriteLine($"ScanCmdPack is{0}", ScanCmdPack);
            Console.WriteLine($"MachIDCmdPack is{0}", MachIDCmdPack);
            Console.WriteLine($"MachConnectCmdPack is{0}", MachConnectCmdPack);
            Console.WriteLine($"MachDataCmdPack is{0}", MachDataCmdPack);

        }
    }
}
