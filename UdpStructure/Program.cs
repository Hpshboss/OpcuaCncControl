using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace UdpStructure
{
    class Program
    {
        static void Main(string[] args)
        {
            //設定本機IP位址，可用IPAddress.Any自行尋找，或直接使用IPAddress.Parse("X.X.X.X")設定
            IPAddress localAddress = IPAddress.Any;
            //如果為傳送端，使用IPAddress.Parse("X.X.X.X")設定
            IPAddress destAddress = null;
            ushort portNumber = 5150;


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

                    //初始化一個物件(可自行替換其他物件)
                    TransferInfo InfoContent = new TransferInfo(1000, 2, 3, 4, 5);
                    //傳輸此物件三次(測試用)
                    for (int i = 0; i < 3; i++)
                    {
                        //序列程序初始化
                        IFormatter bf = new BinaryFormatter();
                        MemoryStream stream = new MemoryStream();
                        //序列化
                        bf.Serialize(stream, InfoContent);
                        sendBuffer = stream.ToArray();
                        //傳送
                        byteSize = udpSocket.Send(sendBuffer, sendBuffer.Length);
                        Console.WriteLine("Sent {0} bytes to {1}", byteSize, destAddress.ToString());
                        stream.Close();
                    }
                }
                else
                {
                    IPEndPoint senderEndPoint = new IPEndPoint(localAddress, 0);
                    Console.WriteLine("Receiving datagrams in a loop until a zero byte datagram is received...");

                    while (true)
                    {
                        //序列程序初始化
                        IFormatter bf = new BinaryFormatter();
                        receiveBuffer = udpSocket.Receive(ref senderEndPoint);
                        MemoryStream stream = new MemoryStream(receiveBuffer);
                        Console.WriteLine("It is {0} bytes from {1}", receiveBuffer.Length, senderEndPoint.ToString());
                        stream.Seek(0, SeekOrigin.Begin);
                        //反序列化
                        TransferInfo InfoContentReceived = bf.Deserialize(stream) as TransferInfo;
                        Console.WriteLine("Receive ID = {0}", InfoContentReceived.ID);
                        Console.WriteLine("Receive Sz = {0}", InfoContentReceived.Sz);
                        Console.WriteLine("Receive Cmd = {0}", InfoContentReceived.Cmd);
                        Console.WriteLine("Receive Count = {0}", InfoContentReceived.Count);
                        Console.WriteLine("Receive Sum = {0}", InfoContentReceived.Sum);

                        //if (receiveBuffer.Length == 0)
                        //    break;
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
    }
    
}
