// SERVER 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;


namespace server
{
    public static class globalvar
    {
        public static int counter = 0;
        public static int cliNo = 0;
        public static string playerUname;

    }
    class Program
    {
        static readonly object _lock = new object();
        static readonly Dictionary<int, TcpClient> list_clients = new Dictionary<int, TcpClient>();

        public static void ProcessClientRequests(object argument)
        {

            int id = (int)argument;
            TcpClient client; //= (TcpClient)argument;
            lock (_lock) client = list_clients[id];
            try
            {

                StreamReader reader = new StreamReader(client.GetStream());
                StreamWriter writer = new StreamWriter(client.GetStream());

                int win = 1001;
                double result = 0;
                int clientNo = globalvar.cliNo;
                string colorStr = "";
                string colorStr1 = "";
                string colorStr2 = "";
                string colorStr3 = "";

                globalvar.playerUname = reader.ReadLine();
                Console.WriteLine(globalvar.playerUname + " joined the game");
                Console.WriteLine();



                int cek = 0;

                while (cek != 4)
                {

                    // server receive input
                    string playerChoose = reader.ReadLine();
                    int Chose = Convert.ToInt16(playerChoose);
                    // Console.WriteLine(playerUname + " Choose : " + Chose);

                    if (Chose == 1)
                    {
                        string playerBet = reader.ReadLine();
                        int Bet1 = Convert.ToInt16(playerBet);
                        Console.WriteLine(globalvar.playerUname + "'s Bet : " + playerBet);

                        string color = reader.ReadLine();
                        Console.WriteLine(globalvar.playerUname + "'s Color : " + color);

                        // server countdown
                        Console.WriteLine();

                        for (int countDown = 3; countDown >= 0; countDown--)
                        {
                            Console.CursorLeft = 0;
                            Console.Write("DICE ROLL IN {0} ", countDown);
                            Thread.Sleep(1000);

                        }

                        //dice random
                        Console.WriteLine();
                        RandomGenerator generator = new RandomGenerator();
                        int dice1 = generator.RandomNumber(1, 7);
                        int dice2 = generator.RandomNumber(1, 7);
                        int dice3 = generator.RandomNumber(1, 7);
                        int dice = dice1 + dice2 + dice3;
                        Console.WriteLine("");
                        Console.WriteLine($"DICE IS [ {dice1} ] [ {dice2} ] [ {dice3} ]");
                        Console.WriteLine("TOTAL DICE IS " + dice);


                        if (dice == 5 || dice == 8 || dice == 11 || dice == 14 || dice == 3)
                        {
                            Console.WriteLine("THE COLOR IS RED");
                            Console.WriteLine("");
                            string sr = "THE COLOR IS RED";
                            writer.WriteLine(sr);
                            writer.Flush();
                            colorStr = "r";
                            colorStr1 = "R";
                            colorStr2 = "RED";
                            colorStr3 = "red";


                        }

                        if (dice == 6 || dice == 9 || dice == 12 || dice == 15 || dice == 4 || dice == 17)
                        {
                            Console.WriteLine("THE COLOR IS GREEN");
                            Console.WriteLine("");
                            string sg = "THE COLOR IS GREEN";
                            writer.WriteLine(sg);
                            writer.Flush();
                            colorStr = "g";
                            colorStr1 = "G";
                            colorStr2 = "GREEN";
                            colorStr3 = "green";
                        }

                        if (dice == 7 || dice == 10 || dice == 13 || dice == 16 || dice == 18)
                        {
                            Console.WriteLine("THE COLOR IS BLUE");
                            Console.WriteLine("");
                            string sb = "THE COLOR IS BLUE";
                            writer.WriteLine(sb);
                            writer.Flush();
                            colorStr = "b";
                            colorStr1 = "B";
                            colorStr2 = "BLUE";
                            colorStr3 = "blue";
                        }

                        // server checking guess color
                        if ((color == colorStr || color == colorStr1 || color == colorStr2 || color == colorStr3) && ((colorStr1 == "R" || colorStr1 == "G") || (colorStr == "r" || colorStr == "g") || (colorStr2 == "RED" || colorStr2 == "GREEN") || (colorStr3 == "red" || colorStr3 == "green")))
                        {
                            result = 2 * Bet1;
                        }
                        else if ((color == colorStr || color == colorStr1 || color == colorStr2 || color == colorStr3) && (colorStr2 == "BLUE " || colorStr3 == "blue" || colorStr1 == "B" || colorStr == "b"))
                        {
                            result = 2 * Bet1;
                        }
                        else
                        {
                            result = 0;
                        }

                        Console.WriteLine(globalvar.playerUname + " WON : " + result);
                        Console.WriteLine();
                        string score = Convert.ToString(result);
                        writer.WriteLine(dice1);
                        writer.Flush();
                        writer.WriteLine(dice2);
                        writer.Flush();
                        writer.WriteLine(dice3);
                        writer.Flush();
                        writer.WriteLine(dice);
                        writer.Flush();
                        writer.WriteLine(score);
                        writer.Flush();
                    }
                    if (Chose == 4)
                    {
                        cek = 4;
                    }

                    if (Chose == 5)
                    {
                        globalvar.counter--;
                        globalvar.cliNo--;
                        Console.WriteLine("");
                        Console.WriteLine(globalvar.playerUname + " Restart The Game");
                        Console.WriteLine();
                        // Console.WriteLine(globalvar.counter + " player in the server.");
                        Console.WriteLine();

                    }




                }

                if (cek == 4)
                {
                    string win1 = reader.ReadLine();
                    int winn = Convert.ToInt16(win1);
                    if (win == winn)
                    {


                        string winner = reader.ReadLine();
                        broadcast(winner);
                        Console.WriteLine(winner);
                        Console.WriteLine();

                        for (int countDown = 5; countDown >= 0; countDown--)
                        {
                            Console.CursorLeft = 0;
                            Console.Write("SERVER CLOSING IN {0} ", countDown);
                            System.Threading.Thread.Sleep(1000);
                        }

                        Console.WriteLine();
                        Console.WriteLine("GAME OVER");
                        Console.WriteLine();
                        globalvar.counter = 0;
                        globalvar.cliNo = 0;
                        /*Environment.Exit(0);
                        reader.Close();
                        writer.Close();
                        client.Close();
                        */


                    }
                    else
                    {
                        // Console.WriteLine(playerUname + " NEED MORE CREDITS TO BUY GOLDEN DICE ");
                    }
                }

                lock (_lock) list_clients.Remove(id);
                client.Client.Shutdown(SocketShutdown.Both);

                reader.Close();
                writer.Close();
                client.Close();
                Console.WriteLine("Closing player connection!");
            }
            catch (IOException)
            {
                Console.WriteLine("");
                Console.WriteLine(globalvar.playerUname + " has left the game");
                Console.WriteLine();
                globalvar.counter--;
                Console.WriteLine("");
                // Console.WriteLine(globalvar.counter + " player in the server.");
                Console.WriteLine();
            }
            finally
            {
                if (client != null)
                {
                    client.Close();
                }
            }
        }

        public static void broadcast(string data)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data + Environment.NewLine);

            lock (_lock)
            {
                foreach (TcpClient c in list_clients.Values)
                {
                    NetworkStream stream = c.GetStream();

                    stream.Write(buffer, 0, buffer.Length);
                    // Console.WriteLine(c.ToString());
                }
            }
        }
        public class RandomGenerator
        {
            Random random = new Random();

            public int RandomNumber(int min, int max)
            {

                return random.Next(min, max);
            }
        }



        public static void Main()
        {
            int count = 1;

            TcpListener listener = null;
            try
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 12000);
                listener.Start();
                Console.WriteLine("==================== SERVER ====================");
                Console.WriteLine("================ SERVER STARTED ================");
                Console.WriteLine("- ESCAPE OR DIE (Alfin x Amca)");
                Console.WriteLine("........ WAITING FOR CONNECTION ........");
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    lock (_lock) list_clients.Add(count, client);
                    globalvar.counter++;
                    globalvar.cliNo++;
                    //Console.WriteLine(globalvar.counter + " Player in the server.");
                    Thread t = new Thread(ProcessClientRequests);
                    t.Start(count);
                    count++;

                }
            }
            catch (Exception n2)
            {
                Console.WriteLine(n2);
                Console.ReadKey();
            }
            finally
            {
                if (listener != null)
                {

                    listener.Stop();

                }
            }

        }
    }
}