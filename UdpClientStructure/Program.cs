using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace UdpClientStructure
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress localAddress = IPAddress.Any;
            IPAddress destAddress = IPAddress.Parse("192.168.0.101");
            ushort portNumber = 5150;
            bool udpSender = true;
            int bufferSize = 256;

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
                    for(int i=0; i<5; i++){
                        byteSize = udpSocket.Send(sendBuffer, sendBuffer.Length);
                        Console.WriteLine("Sent {0} bytes to {1}", byteSize, destAddress.ToString());
                    }

                    TransferInfo InfoContent = new TransferInfo(1000,2,3,4,5);
                    string Value__ = Convert.ToString(InfoContent.ID, 16);
                    Padding ModifiedValue = new Padding(Value__, 2);
                    sendBuffer = Encoding.ASCII.GetBytes(ModifiedValue.GOGO());
                    byteSize = udpSocket.Send(sendBuffer, Encoding.ASCII.GetByteCount(ModifiedValue.GOGO()));
                    Console.WriteLine("Sent {0} bytes to {1}", byteSize, destAddress.ToString());

                    while (true)
                    {
                        Console.Write("What do you want to send:");
                        sendBuffer = Encoding.ASCII.GetBytes(Console.ReadLine());
                        byteSize = udpSocket.Send(sendBuffer, sendBuffer.Length);
                        Console.WriteLine("Sent {0} bytes to {1}", byteSize, destAddress.ToString());
                    }
                }
                else
                {
                    IPEndPoint senderEndPoint = new IPEndPoint(localAddress, 0);
                    Console.WriteLine("Receiving datagrams in a loop until a zero byte datagram is received...");

                    while (true)
                    {
                        receiveBuffer = udpSocket.Receive(ref senderEndPoint);
                        Console.WriteLine("Read {0} bytes from {1}", receiveBuffer.Length, senderEndPoint.ToString());

                        if (receiveBuffer.Length == 0)
                            break;
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
