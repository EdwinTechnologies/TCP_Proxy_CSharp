using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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



        /***************************** Packet-Analyser for PwnAdventure3 ************************************/



        public static void CheckForClientsData(byte[] DataBuffer, int lenght)
        {
            if (ByteToHex(DataBuffer.ToArray()).Contains("232A"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Client -> Server] Detected Chat!");
            }
            else if (ByteToHex(DataBuffer.ToArray()).Contains("2a691000477265617442616c6c734f6646697265".ToUpper()))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Client -> Server] Detected Fireball!");
            }
            else if (ByteToHex(DataBuffer.ToArray()).Contains("2A690B00436F77626F79436F646572"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Client -> Server] Detected Cowboy-Coder!");
            }
            else if (ByteToHex(DataBuffer.ToArray()).Contains("2A690700414B5269666C65"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Client -> Server] Detected AKRifle!");
            }
            else if (ByteToHex(DataBuffer.ToArray()).Contains("2A690F00486F6C7948616E644772656E616465"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Client -> Server] Detected HolyHandGrenade!");
            }
            else if (ByteToHex(DataBuffer.ToArray()).Contains("6565"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Client -> Server] Collected Item! " + ByteToHex(DataBuffer.ToArray()));
            }
            else if (ByteToHex(DataBuffer.ToArray()).Contains("6A70"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[Client -> Server] Jumped!");
            }
            //else if (ByteToHex(DataBuffer.ToArray()).Contains("6D76"))
            //{
            //    //Don't display position
            //}
            else if (lenght != 0)
            {
                Console.WriteLine("[Client -> Server] " + ByteToHex(DataBuffer.ToArray()));       //Display the chunck
            }

            Console.ResetColor();
        }


        public static void CheckForServersData(byte[] DataBuffer, int lenght)
        {
            if (ByteToHex(DataBuffer.ToArray()).Contains("2B2B"))
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("[Server -> Client] Health: " + ByteToHex(DataBuffer.ToArray()));
            }
            else if (ByteToHex(DataBuffer.ToArray()).Contains("6D61"))
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("[Server -> Client] Mana: " + ByteToHex(DataBuffer.ToArray()));
            }
            else if (lenght != 0 && ByteToHex(DataBuffer.ToArray()) != "")
            {
                //Console.WriteLine("[Server -> Client] " + ByteToHex(DataBuffer.ToArray()));
            }

            Console.ResetColor();
        }

    }
}