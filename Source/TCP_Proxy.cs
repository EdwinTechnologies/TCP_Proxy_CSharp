/*
 * 
 * © EdwinTechnologies 2018
 * 
 * http://edwintech.ddns.net/
 * 
 * 
 * Source == Remote Client
 * Drain == Remote Server
 * 
 * 
*/




using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ProxyServer
{
    class TCP_Proxy
    {
        public static int BufferSize = 1;   // 'Read' Buffer Size
        
        /***************************************************************/

        public static Int32 SourcePort = 81;                 //Client connects to this Port
        public static IPAddress LocalHostIP = IPAddress.Parse("192.168.178.39");            //IP of this machine
        public static IPEndPoint SourceHost = new IPEndPoint(LocalHostIP, SourcePort);

        public static byte[] SourceToProxyBuffer;
        public static List<byte> SourceBuffer = new List<byte>();



        /***************************************************************/




        public static Int32 DrainPort = 31416;          //Proxy connects to this Port of the remote Server
        public static IPAddress DrainIP = IPAddress.Parse("192.168.178.35");       //IP of the remote Server
        public static IPEndPoint DrainHost = new IPEndPoint(DrainIP, DrainPort);


        public static byte[] ProxyToDrainBuffer;
        public static List<byte> DrainBuffer = new List<byte>();



        /***************************************************************/



        public static Socket DrainClient = new Socket(DrainHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        public static Socket SourceHandler;

        public static Socket SourceListener = new Socket(SourceHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);


        /***************************************************************/


        public static string ByteToHex(byte[] byteData)
        {
            return BitConverter.ToString(byteData).Replace("-", string.Empty);
        }

        public static void Init ()
        {
            Console.WriteLine("Current Settings: \n");
            Console.WriteLine("Local-IP: " + LocalHostIP.ToString() + "\n");
            Console.WriteLine("Server-IP: " + DrainIP.ToString() + "\n");
            Console.WriteLine("[Client -> Proxy] Port: " + SourcePort.ToString() + "\n");
            Console.WriteLine("[Proxy -> Server] Port: " + DrainPort.ToString() + "\n");


            string Yes_No;
            Console.Write("Is this configuration okay? [Y/N] ");
            Yes_No = Console.ReadLine();

            if (Yes_No == "n" || Yes_No == "N")
            {
                Console.Write("Local IPv4-Address? ");
                LocalHostIP = IPAddress.Parse(Console.ReadLine());

                Console.Write("Server-IP? ");
                DrainIP = IPAddress.Parse(Console.ReadLine());

                Console.Write("[Client -> Proxy] Port? ");
                SourcePort = Int32.Parse(Console.ReadLine());

                Console.Write("[Proxy -> Server] Port? ");
                DrainPort = Int32.Parse(Console.ReadLine());
                Console.WriteLine("\n");

                DrainHost = new IPEndPoint(DrainIP, DrainPort);
                SourceHost = new IPEndPoint(LocalHostIP, SourcePort);

                DrainClient = new Socket(DrainHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                SourceListener = new Socket(SourceHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            }
        }


        /***************************************************************/

        static void Main(string[] args)
        {

            Init();     //Asks for Proxy-Configuration

            Thread SourceToProxyThread = new Thread(SourceToProxy);     //Start Client -> Proxy Thread
            SourceToProxyThread.Start();
        }





        /***************************************************************/




        public static void SourceToProxy()          //Local Proxy Server waiting for connection, the real Client will be connected here
        {
            SourceToProxyBuffer = new Byte[BufferSize];     //Create 'read' Buffer

            try
            {
                SourceListener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                SourceListener.Bind(SourceHost);
                SourceListener.Listen(1);

                Console.WriteLine("Waiting for Source [RealClient] ... \n");

                SourceHandler = SourceListener.Accept();

                Console.WriteLine("Source [RealClient] has connected! \n");

                Thread ProxyToDrainThread = new Thread(ProxyToDrain);       //If Client has connected to Proxy -> start connection to the remote Server
                ProxyToDrainThread.Start();


                while (true)
                {
                    SourceToProxyBuffer = new Byte[BufferSize];     //Clear 'read' Buffer

                    int lenght = SourceHandler.Receive(SourceToProxyBuffer);    //Read Data from Client


                    while (!DrainClient.Connected)      //Don't do anything if Proxy hasn't connected to the remote Server
                    {

                    }

                    if (SourceBuffer.Count >= lenght)   //SourceBuffer is a list for displaying the read data (50 Byte chuncks)
                    {
                        if (lenght != 0)
                        {
                            Console.WriteLine("[Client -> Server] " + ByteToHex(SourceBuffer.ToArray()));       //Display the chunck
                        }
                        SourceBuffer.Clear();
                    }
                    else
                    {
                        SourceBuffer.Add(SourceToProxyBuffer[0]);
                    }


                    DrainClient.Send(SourceToProxyBuffer);      //Forward the read Data to Server

                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Source [Real Client] can't connect to the Proxy | " + e);
            }

        }


        /***************************************************************/


        public static void ProxyToDrain()       //Proxy-Client will connect to the remote Server
        {

            Console.WriteLine("Trying to connect to Drain [Real Server] ...");

            ProxyToDrainBuffer = new Byte[BufferSize];      //Create 'read' Buffer

            try
            {
                DrainClient.Connect(DrainHost);
                Console.WriteLine("Successfully connected to Drain [Real Server]");


                while (true)
                {
                    ProxyToDrainBuffer = new Byte[BufferSize];     //Clear 'read' Buffer

                    int lenght = DrainClient.Receive(ProxyToDrainBuffer);    //Read Data from Server


                    //if (DrainBuffer.Count >= 50)   //DrainBuffer is a list for displaying the read data (50 Byte chuncks)
                    //{
                    //    if (lenght != 0)
                    //    {
                    //        Console.WriteLine("[Server -> Client] " + ByteToHex(DrainBuffer.ToArray()));        //Display the chunck
                    //    }
                    //    DrainBuffer.Clear();
                    //}
                    //else
                    //{
                    //    DrainBuffer.Add(ProxyToDrainBuffer[0]);
                    //}

                    SourceHandler.Send(ProxyToDrainBuffer);     //Forward the read Data to Client

                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Drain [Real Server] is not available | " + e);
            }

        }

    }
}
