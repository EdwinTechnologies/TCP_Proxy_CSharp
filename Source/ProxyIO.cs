using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ProxyIO
{
    public static class PacketParser
    {

        public static string ByteToHex(byte[] byteData)
        {
            return BitConverter.ToString(byteData).Replace("-", string.Empty);
        }

        public static byte[] HexToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }

        public static float HexToFloat(string hexString, bool LittleEndian)
        {
            try
            {
                if (LittleEndian)
                {
                    int number = Convert.ToInt32(hexString, 16);
                    byte[] bytes = BitConverter.GetBytes(number);
                    string retval = "";
                    foreach (byte b in bytes)
                        retval += b.ToString("X2");

                    uint num = uint.Parse(retval, System.Globalization.NumberStyles.AllowHexSpecifier);

                    byte[] floatVals = BitConverter.GetBytes(num);
                    return BitConverter.ToSingle(floatVals, 0);
                }
                else
                {
                    uint num = uint.Parse(hexString, System.Globalization.NumberStyles.AllowHexSpecifier);

                    byte[] floatVals = BitConverter.GetBytes(num);
                    return BitConverter.ToSingle(floatVals, 0);
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {

            try
            {
                int Start, End;
                if (strSource.Contains(strStart) && strSource.Contains(strEnd))
                {
                    Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                    End = strSource.IndexOf(strEnd, Start);
                    return strSource.Substring(Start, End - Start);
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public static string getBetween(string strSource, string strStart, int strEnd)
        {
            try
            {
                int Start, End;
                if (strSource.Contains(strStart))
                {
                    Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                    End = Start + strEnd;
                    return strSource.Substring(Start, End - Start);
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public static string getBetween(string strSource, string strStart, int StartPoint, int EndPoint)
        {
            try
            {
                int Start, End;
                if (strSource.Contains(strStart))
                {
                    Start = strSource.IndexOf(strStart, 0) + strStart.Length + StartPoint;
                    End = (Start - StartPoint) + EndPoint;
                    return strSource.Substring(Start, End - Start);
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public static string getBetween(string strSource, int StartPoint, int EndPoint)
        {
            try
            {
                return strSource.Substring(StartPoint, EndPoint - StartPoint);
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public static string BinaryStringToHexString(string binary)
        {
            StringBuilder result = new StringBuilder(binary.Length / 8 + 1);
            int mod4Len = binary.Length % 8;
            if (mod4Len != 0)
            {
                binary = binary.PadLeft(((binary.Length / 8) + 1) * 8, '0');
            }
            for (int i = 0; i < binary.Length; i += 8)
            {
                string eightBits = binary.Substring(i, 8);
                result.AppendFormat("{0:X2}", Convert.ToByte(eightBits, 2));
            }
            return result.ToString();
        }

        public static int HexToInt32(string HexString, bool LittleEndian)
        {
            try
            {
                if (LittleEndian)
                {
                    int number = Convert.ToInt32(HexString, 16);
                    byte[] bytes = BitConverter.GetBytes(number);
                    string retval = "";
                    foreach (byte b in bytes)
                        retval += b.ToString("X2");
                    return int.Parse(retval, System.Globalization.NumberStyles.HexNumber);
                    //return BitConverter.ToInt32(HexToByteArray(HexString), 0);
                }
                else
                {
                    return int.Parse(HexString, System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch (Exception e)
            {
                return 0;
            }

            
        }

        public static byte[] BinaryToByteArray(string input)
        {
            var bytesAsStrings = input.Select((c, i) => new { Char = c, Index = i }).GroupBy(x => x.Index / 8).Select(g => new string(g.Select(x => x.Char).ToArray()));
            byte[] bytes = bytesAsStrings.Select(s => Convert.ToByte(s, 2)).ToArray();
            return bytes;
        }

        /***************************** Packet-Analyser for PwnAdventure3 ************************************/

        public static bool DisplayUnknownData = false;
        public static bool AutoItemPickUp = true;
        public static bool ShowActiveActorInformation = false;
        public static bool ShowPlayerPosition = false;

        /****************************************************************************************************/

        public static void CrackTheBlocky()
        {
            Thread BitCrack32_BlockyThread = new Thread(BitCrack32_Blocky);
            BitCrack32_BlockyThread.Start();
        }

        public static void BitCrack32_Blocky()
        {
            string CurrentBinStr = "";

            char[] pattern = "01".ToCharArray();

            foreach (char a in pattern)
            {
                foreach (char b in pattern)
                {
                    foreach (char c in pattern)
                    {
                        foreach (char d in pattern)
                        {
                            foreach (char e in pattern)
                            {
                                foreach (char f in pattern)
                                {
                                    foreach (char g in pattern)
                                    {
                                        foreach (char h in pattern)
                                        {
                                            foreach (char i in pattern)
                                            {
                                                foreach (char j in pattern)
                                                {
                                                    foreach (char k in pattern)
                                                    {
                                                        foreach (char l in pattern)
                                                        {
                                                            foreach (char m in pattern)
                                                            {
                                                                foreach (char n in pattern)
                                                                {
                                                                    foreach (char o in pattern)
                                                                    {
                                                                        foreach (char p in pattern)
                                                                        {
                                                                            CurrentBinStr = "0" + a.ToString() + "1" + b.ToString() + "1" + c.ToString() + "0" + d.ToString() + "1" + e.ToString() + "0" + f.ToString() + "1" + g.ToString() + "1" + h.ToString() + "1" + i.ToString() + "1" + j.ToString() + "1" + k.ToString() + "1" + l.ToString() + "1" + m.ToString() + "1" + n.ToString() + "1" + o.ToString() + "1" + p.ToString();

                                                                            string[] bytes = new string[4];

                                                                            bytes[0] = getBetween(CurrentBinStr, 0, 8);     //Reversing bytes
                                                                            bytes[1] = getBetween(CurrentBinStr, 8, 16);
                                                                            bytes[2] = getBetween(CurrentBinStr, 16, 24);
                                                                            bytes[3] = getBetween(CurrentBinStr, 24, 32);

                                                                            CurrentBinStr = bytes[3] + bytes[2] + bytes[1] + bytes[0];

                                                                            CurrentBinStr = "30310A0046696E616C5374616765" + BinaryStringToHexString(CurrentBinStr);

                                                                            ProxyServer.TCP_Proxy.DrainClient.Send(HexToByteArray(CurrentBinStr), HexToByteArray(CurrentBinStr).Length, SocketFlags.None);
                                                                            Console.WriteLine(CurrentBinStr);
                                                                            Thread.Sleep(100);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /****************************************************************************************************/

        public static void CheckForClientsData(byte[] DataBuffer, int lenght)
        {
            if (ByteToHex(DataBuffer.ToArray()).Contains("232A"))
            {
                Console.ForegroundColor = ConsoleColor.Green;

                string packet = ByteToHex(DataBuffer.ToArray());

                Int32 msg_lenght = HexToInt32(getBetween(packet, "232A", 4) + "0000", true);
                string message = getBetween(packet, "232A", 4, 4 + msg_lenght * 2);                 //FIX THIS

                Console.WriteLine("[Client -> Server] Detected Chat: " + Encoding.ASCII.GetString(HexToByteArray(message)) + " Lenght: " + msg_lenght.ToString());

                if (Encoding.ASCII.GetString(HexToByteArray(message)) == "Inject")      //Type "Inject" in Pwn3 Chat for Proxy Inject Mode
                {
                    if (!ProxyServer.TCP_Proxy.InjectMode)
                    {
                        ProxyServer.TCP_Proxy.InjectMode = true;
                        Thread InjectPacketThread = new Thread(ProxyServer.TCP_Proxy.InjectPacket);
                        InjectPacketThread.Start();
                    }
                }
                else if (Encoding.ASCII.GetString(HexToByteArray(message)) == "OpenBlocky")
                {
                    Console.WriteLine("Opened Blocky's Door!");
                    ProxyServer.TCP_Proxy.DrainClient.Send(HexToByteArray("30310A0046696E616C5374616765FAAB8F69"), HexToByteArray("30310A0046696E616C5374616765FAAB8F69").Length, SocketFlags.None);
                }
                else if (Encoding.ASCII.GetString(HexToByteArray(message)) == "BruteForceBlocky")
                {
                    Console.WriteLine("======= Started Blocky-BruteForce =======");
                    CrackTheBlocky();
                }
            }
            if (ByteToHex(DataBuffer.ToArray()).Contains("2a691000477265617442616c6c734f6646697265".ToUpper()))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Client -> Server] Detected Fireball!");
            }
            if (ByteToHex(DataBuffer.ToArray()).Contains("2A690B00436F77626F79436F646572"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Client -> Server] Detected Cowboy-Coder!");
            }
            if (ByteToHex(DataBuffer.ToArray()).Contains("2A690700414B5269666C65"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Client -> Server] Detected AKRifle!");
            }
            if (ByteToHex(DataBuffer.ToArray()).Contains("2A690F00486F6C7948616E644772656E616465"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Client -> Server] Detected HolyHandGrenade!");
            }
            if (ByteToHex(DataBuffer.ToArray()).Contains("6565"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Client -> Server] Collected Item! ID: " + getBetween(ByteToHex(DataBuffer.ToArray()), "6565", 4));

            }
            if (ByteToHex(DataBuffer.ToArray()).Contains("6A70"))       //For blocking/replace turn off ZeroRemover
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Client -> Server] Jumped!");
            }
            if (ByteToHex(DataBuffer.ToArray()).Contains("6D76"))       //Player Position
            {
                if (ShowPlayerPosition)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    string packet = ByteToHex(DataBuffer.ToArray());

                    string X = getBetween(packet, "6D76", 8);
                    string Y = getBetween(packet, "6D76", 8, 16);
                    string Z = getBetween(packet, "6D76", 16, 24);
                    Console.WriteLine("[Client -> Server] PlayerPos - X: " + HexToFloat(X, true) + " | Y: " + HexToFloat(Y, true) + " | Z: " + HexToFloat(Z, true));
                }
            }
            if (ByteToHex(DataBuffer.ToArray()).Contains("726E"))       //Sneak to switch to Inject-Mode
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Client -> Server] Detected Sneaking!");
            }


            //if (ByteToHex(DataBuffer.ToArray()).Contains("3031"))
            //{
            //    Console.ForegroundColor = ConsoleColor.Green;
            //    Console.WriteLine("[Client -> Server] Toggled Button: " + ByteToHex(DataBuffer.ToArray()));
            //}
            if (lenght != 0 && !(ByteToHex(DataBuffer.ToArray()).Contains("6D76")))
            {
                if (DisplayUnknownData)
                    Console.WriteLine("[Client -> Server] " + ByteToHex(DataBuffer.ToArray()));       //Display the chunck
            }

            Console.ResetColor();
        }


        //public static string myID;
        //public static bool setMyID = false;

        public static void CheckForServersData(byte[] DataBuffer, int lenght)
        {
            if (ByteToHex(DataBuffer.ToArray()).Contains("2B2B"))       //Health of Player/Monsters
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                string packet = ByteToHex(DataBuffer.ToArray());

                string ActorID = getBetween(packet, "2B2B", 8);
                string HealthValue = getBetween(packet, "2B2B", 8, 16);

                //if (!setMyID)
                //{
                //    myID = ActorID;
                //    setMyID = true;
                //}
                

                if (HexToInt32(HealthValue, true) > 0 && (HexToInt32(HealthValue, true) < 100))
                {
                    Console.WriteLine("[Server -> Client] " + "Actor ID: " + HexToInt32(ActorID, true) + " | Health: " + HexToInt32(HealthValue, true));
                }

                //ProxyServer.TCP_Proxy.SourceHandler.Send(HexToByteArray("2B2B" + myID + "0000" + "64000000"));
                //Console.WriteLine("MyID: " + myID + " | Actors ID: " + ActorID);
                    

            }
            //if (ByteToHex(DataBuffer.ToArray()).Contains("0600"))
            //{
            //    Console.ForegroundColor = ConsoleColor.Cyan;
            //    Console.WriteLine("[Server -> Client] Button Response: " + ByteToHex(DataBuffer.ToArray()));
            //}
            if (ByteToHex(DataBuffer.ToArray()).Contains("6D6B") && AutoItemPickUp)       //Get ID of Actors and Items + AutoPickUp
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                string id = getBetween(ByteToHex(DataBuffer.ToArray()), "6D6B", 4);
                Console.WriteLine("[Server -> Client] Item/Actor ID: " + id);

                string itemPacket = "6565" + id + "0000";

                ProxyServer.TCP_Proxy.DrainClient.Send(HexToByteArray(itemPacket), HexToByteArray(itemPacket).Length, SocketFlags.None);    //Send Item ID to get the Item
            }
            if (ByteToHex(DataBuffer.ToArray()).Contains("7073"))       //Get active Actors
            {
                if (ShowActiveActorInformation)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;

                    string Buffer = ByteToHex(DataBuffer.ToArray());

                    int i = 1;

                    while (i < 11)      //Iterate over the Buffer 10 times
                    {
                        string packet = getBetween(Buffer, "7073", 56);

                        packet = "7073" + packet;

                        string ID = getBetween(packet, "7073", 4);          //Parse each packet
                        string X = getBetween(packet, "7073", 8, 16);
                        string Y = getBetween(packet, "7073", 16, 24);
                        string Z = getBetween(packet, "7073", 24, 32);

                        Console.WriteLine("[Server -> Client] Active Actor " + i.ToString() + " - ID: " + ID + " | X: " + HexToFloat(X, true) + " | Y: " + HexToFloat(Y, true) + " | Z: " + HexToFloat(Z, true));
                        


                        Regex pattern = new Regex(packet);      //Remove the parsed packet from the Buffer
                        Buffer = pattern.Replace(Buffer, "");



                        i++;
                    }

                    Console.WriteLine("======================================");



                }
            }
            if (ByteToHex(DataBuffer.ToArray()).Contains("6D61"))       //Mana
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                string packet = getBetween(ByteToHex(DataBuffer.ToArray()), "6D61", 2);
                Console.WriteLine("[Server -> Client] Mana: " + HexToInt32(packet, false));

                //string ReplacedPacket = ByteToHex(DataBuffer.ToArray());
                //Regex pattern = new Regex("6D61" + packet);
                //ReplacedPacket = pattern.Replace(ReplacedPacket, "6D6164");

                ////Console.WriteLine("Mana replaced: " + ReplacedPacket);

                //ProxyServer.TCP_Proxy.ProxyToDrainBuffer = new Byte[4096];
                //ProxyServer.TCP_Proxy.ProxyToDrainBuffer = HexToByteArray(ReplacedPacket);

            }
            if (lenght != 0 && ByteToHex(DataBuffer.ToArray()) != "" && !(ByteToHex(DataBuffer.ToArray()).Contains("7073")))
            {
                if (DisplayUnknownData)
                    Console.WriteLine("[Server -> Client] " + ByteToHex(DataBuffer.ToArray()));
            }

            Console.ResetColor();
        }

    }
}