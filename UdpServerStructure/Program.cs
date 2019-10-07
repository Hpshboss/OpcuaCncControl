using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Reflection;

namespace UdpClientStructure
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress localAddress = IPAddress.Parse("192.168.0.101");
            IPAddress destAddress = null;
            ushort portNumber = 5150;
            bool udpSender = false;
            int bufferSize = 512;

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

                    sendBuffer = Encoding.ASCII.GetBytes("Hello world");

                    byteSize = udpSocket.Send(sendBuffer, sendBuffer.Length);
                    Console.WriteLine("Sent {0} bytes to {1}", byteSize, destAddress.ToString());
                }
                else
                {
                    IPEndPoint senderEndPoint = new IPEndPoint(localAddress, 0);
                    Console.WriteLine("Receiving datagrams in a loop until a zero byte datagram is received...");

                    
                    
                    while (true)
                    {
                        IFormatter bf = new BinaryFormatter();
                        receiveBuffer = udpSocket.Receive(ref senderEndPoint);
                        MemoryStream stream = new MemoryStream(receiveBuffer);
                        /*
                        Console.WriteLine("Read string \"{0}\"", Encoding.ASCII.GetString(receiveBuffer));
                        */
                        Console.WriteLine("It is {0} bytes from {1}", receiveBuffer.Length, senderEndPoint.ToString());
                        stream.Seek(0, SeekOrigin.Begin);
                        TransferInfo InfoContentReceived = bf.Deserialize(stream) as TransferInfo;
                        Console.WriteLine("Receive ID = \"{0}\"", InfoContentReceived.ID);
                        Console.WriteLine("Receive Sz = \"{0}\"", InfoContentReceived.Sz);
                        Console.WriteLine("Receive Cmd = \"{0}\"", InfoContentReceived.Cmd);
                        Console.WriteLine("Receive Count = \"{0}\"", InfoContentReceived.Count);
                        Console.WriteLine("Receive Sum = \"{0}\"", InfoContentReceived.Sum);


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
