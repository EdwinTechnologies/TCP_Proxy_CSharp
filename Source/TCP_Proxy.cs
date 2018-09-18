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
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ProxyIO;

namespace ProxyServer
{
    public static class TCP_Proxy
    {
        public static int BufferSize = 4096;   // 'Read' Buffer Size

        public static bool InjectMode = false;

        public static bool PwnAdventure3 = false;

        /**************************** Client -> Proxy Settings ***********************************/

        public static Int32 SourcePort = 81;                 //Client connects to this Port
        public static IPAddress LocalHostIP = IPAddress.Parse("0.0.0.0");            //IP of this machine
        public static IPEndPoint SourceHost = new IPEndPoint(LocalHostIP, SourcePort);

        public static byte[] SourceToProxyBuffer;

        /**************************** Proxy -> Server Settings ***********************************/


        public static Int32 DrainPort = 31416;          //Proxy connects to this Port of the remote Server
        public static IPAddress DrainIP = IPAddress.Parse("192.168.178.35");       //IP of the remote Server
        public static IPEndPoint DrainHost = new IPEndPoint(DrainIP, DrainPort);

        public static byte[] ProxyToDrainBuffer;

        /***************************************************************/


        public static Socket DrainClient = new Socket(DrainHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        public static Socket SourceHandler;

        public static Socket SourceListener = new Socket(SourceHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        public static bool Allowed_SendToServer = true;
        public static bool Allowed_SendToClient = true;

        /***************************************************************/

        public static Stopwatch StopWatch;
        public static double BytesDownloadedServer = 0;
        public static double BytesDownloadedClient = 0;

        public static void Init()
        {
            Console.WriteLine("╔══════════════════════════╗");
            Console.WriteLine("║    TCP Proxy # V. 2.7    ║");
            Console.WriteLine("╚══════════════════════════╝");
            Console.WriteLine("     ");
            Console.WriteLine("[Proxy] " + LocalHostIP.ToString() + ":" + SourcePort.ToString() + " -> " + "[Server] " + DrainIP.ToString() + ":" + DrainPort.ToString());

            Console.WriteLine("     ");


            //string Yes_No;
            //Console.Write("Is this configuration okay? [Y/N] ");
            //Yes_No = Console.ReadLine();
            //Console.WriteLine("     ");

            //if (Yes_No == "n" || Yes_No == "N")
            //{
            //    Console.Write("Local IPv4-Address? ");
            //    LocalHostIP = IPAddress.Parse(Console.ReadLine());
            //    Console.WriteLine("     ");

            //    Console.Write("Server-IP? ");
            //    DrainIP = IPAddress.Parse(Console.ReadLine());
            //    Console.WriteLine("     ");

            //    Console.Write("[Client -> Proxy] Port? ");
            //    SourcePort = Int32.Parse(Console.ReadLine());
            //    Console.WriteLine("     ");

            //    Console.Write("[Proxy -> Server] Port? ");
            //    DrainPort = Int32.Parse(Console.ReadLine());
            //    Console.WriteLine("     ");
            //    Console.WriteLine("════════════════════════ \n");

            //    DrainHost = new IPEndPoint(DrainIP, DrainPort);
            //    SourceHost = new IPEndPoint(LocalHostIP, SourcePort);

            //    DrainClient = new Socket(DrainHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            //    SourceListener = new Socket(SourceHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //}
        }



        public static void InjectPacket()
        {
            Console.WriteLine("Proxy is in Inject-Mode! ");
            byte[] PacketBuffer = new byte[1];

            while (InjectMode)
            {
                Console.Write("$ ");

                string input = Console.ReadLine();
                if (input == "SRV" || input == "srv")
                {
                    while (true)
                    {
                        Console.Write("Inject Packet -> Server $ ");
                        input = Console.ReadLine();

                        if (input == "exit")
                        {
                            break;
                        }

                        PacketBuffer = PacketParser.HexToByteArray(input);
                        DrainClient.Send(PacketBuffer);
                        Console.WriteLine("Injected into Stream!");
                    }

                }
                else if (input == "CLI" || input == "cli")
                {
                    while (true)
                    {
                        Console.Write("Inject Packet -> Client $ ");
                        input = Console.ReadLine();

                        if (input == "exit")
                        {
                            break;
                        }

                        PacketBuffer = PacketParser.HexToByteArray(input);
                        SourceHandler.Send(PacketBuffer);
                        Console.WriteLine("Injected into Stream!");
                    }
                }
                else if (input == "Monitor")
                {
                    Console.WriteLine("======= Monitoring-Mode =======");
                    InjectMode = false;
                }
                else if (input == "Speed")
                {
                    Console.WriteLine("======= Proxy-Byterate =======");

                    double Seconds_Before = 0;


                    while (true)
                    {
                        double SS = double.Parse(string.Format("{0:ss}", StopWatch.Elapsed));

                        double MM = double.Parse(string.Format("{0:mm}", StopWatch.Elapsed));

                        double HH = double.Parse(string.Format("{0:hh}", StopWatch.Elapsed));

                        MM *= 60;

                        HH *= 3600;

                        double Seconds = SS + MM + HH;

                        if (Seconds > Seconds_Before)       //Write transfered Bytes every Seconds
                        {
                            Console.WriteLine("=============================");

                            if (BytesDownloadedServer >= 1000)
                            {
                                Console.WriteLine("[Server -> Proxy] " + (BytesDownloadedServer / 1000).ToString() + " kBytes/s");
                            }
                            else
                            {
                                Console.WriteLine("[Server -> Proxy] " + BytesDownloadedServer.ToString() + " Bytes/s");
                            }

                            Console.WriteLine("=============================");

                            if (BytesDownloadedClient >= 1000)
                            {
                                Console.WriteLine("[Client -> Proxy] " + (BytesDownloadedClient / 1000).ToString() + " kBytes/s");
                            }
                            else
                            {
                                Console.WriteLine("[Client -> Proxy] " + BytesDownloadedClient.ToString() + " Bytes/s");
                            }

                            BytesDownloadedServer = 0;
                            BytesDownloadedClient = 0;

                            Console.WriteLine("=============================");
                        }

                        Seconds_Before = Seconds;

                    }

                    
                }
                //else if (input == "Mana")
                //{
                //    while (true)
                //    {
                //        SourceHandler.Send(PacketParser.HexToByteArray("6D615C000000"));
                //        Thread.Sleep(10);
                //    }

                //}
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

        public static void DisplayAnimation()
        {
            int Speed = 100;
            int X = 32;
            int Y = 6;


            while (!DrainClient.Connected)
            {
                Console.SetCursorPosition(X, Y);
                Console.WriteLine("[|]");
                Console.SetCursorPosition(X, Y);
                Thread.Sleep(Speed);
                Console.WriteLine("[/]");
                Console.SetCursorPosition(X, Y);
                Thread.Sleep(Speed);
                Console.WriteLine("[-]");
                Console.SetCursorPosition(X, Y);
                Thread.Sleep(Speed);
                Console.WriteLine("[\\]");
                Console.SetCursorPosition(X, Y);
                Thread.Sleep(Speed);
            }
        }

        /***************************************************************/

        public static void SourceToProxy()          //Local Proxy Server waiting for connection, the real Client will be connected here
        {
            SourceToProxyBuffer = new byte[BufferSize];     //Create 'read' Buffer

            try
            {
                SourceListener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                SourceListener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                SourceListener.Bind(SourceHost);
                SourceListener.Listen(1);

                Console.WriteLine("Waiting for Source [Client] ... \n");

                Thread AnimationThread = new Thread(DisplayAnimation);       //If Client has connected to Proxy -> start connection to the remote Server
                AnimationThread.Start();

                SourceHandler = SourceListener.Accept();

                AnimationThread.Abort();

                Console.WriteLine("");
                Console.WriteLine("Source [Client] has connected! \n");

                Thread ProxyToDrainThread = new Thread(ProxyToDrain);       //If Client has connected to Proxy -> start connection to the remote Server
                ProxyToDrainThread.Start();


                while (true)
                {
                    SourceToProxyBuffer = new byte[BufferSize];     //Clear 'read' Buffer

                    int lenght = SourceHandler.Receive(SourceToProxyBuffer, SourceToProxyBuffer.Length, 0);    //Read Data from Client

                    BytesDownloadedClient += lenght;

                    while (!DrainClient.Connected)      //Don't do anything if Proxy hasn't connected to the remote Server
                    {

                    }

                    if (!InjectMode)
                    {
                        if (PwnAdventure3)
                        {
                            PacketParser.CheckForClientsData(DeleteZeroTrail(SourceToProxyBuffer), lenght);
                        }
                        else
                        {
                            Console.WriteLine("[Client -> Server] " + PacketParser.ByteToHex(DeleteZeroTrail(SourceToProxyBuffer)));
                        }
                    }

                    if (!Allowed_SendToServer)
                    {
                        Allowed_SendToServer = true;
                        continue;
                    }


                    DrainClient.Send(SourceToProxyBuffer, lenght, SocketFlags.None);      //Forward the read Data to Server

                }

            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("### Connection Problems with Client ### " + e);
                Console.ResetColor();
            }

        }


        /***************************************************************/

        public static byte[] DeleteZeroTrail(byte[] array)       //Filter the trailing 0x0's out of the Array to get a cleaner look
        {
            int lastIndex = Array.FindLastIndex(array, b => b != 0);

            Array.Resize(ref array, lastIndex + 1);

            return array;
        }

        public static void ProxyToDrain()       //Proxy-Client will connect to the remote Server
        {

            Console.WriteLine("Trying to connect to Drain [Server] ...");

            ProxyToDrainBuffer = new byte[BufferSize];      //Create 'read' Buffer

            try
            {
                DrainClient.Connect(DrainHost);
                Console.WriteLine("Successfully connected to Drain [Server]");

                StopWatch = new Stopwatch();
                StopWatch.Start();

                if (InjectMode)
                {
                    Thread InjectPacketThread = new Thread(InjectPacket);
                    InjectPacketThread.Start();
                }

                while (true)
                {
                    ProxyToDrainBuffer = new byte[BufferSize];     //Clear 'read' Buffer
                    
                    int lenght = DrainClient.Receive(ProxyToDrainBuffer, ProxyToDrainBuffer.Length, 0);    //Read Data from Server

                    BytesDownloadedServer += lenght;

                    if (!InjectMode)
                    {
                        if (PwnAdventure3)
                        {
                            PacketParser.CheckForServersData(DeleteZeroTrail(ProxyToDrainBuffer), lenght);
                        }
                        else
                        {
                            Console.WriteLine("[Server -> Client] " + PacketParser.ByteToHex(DeleteZeroTrail(ProxyToDrainBuffer)));
                        }
                    }

                    if (!Allowed_SendToClient)
                    {
                        Allowed_SendToClient = true;
                        continue;
                    }

                    SourceHandler.Send(ProxyToDrainBuffer, lenght, SocketFlags.None);     //Forward the read Data to Client
                    

                }

            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("### Connection Problems with Server ### " + e);
                Console.ResetColor();
            }

        }

    }
}