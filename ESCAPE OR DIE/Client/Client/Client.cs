// CLIENT

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

public class EchoClient
{

    public static class globalvar
    {
        public static bool status = false;
        public static bool bc = false;

    }
    public static void pilihanMenu(int n)
    {

        switch (n)
        {
            case 2:

                Console.WriteLine("=================*** HOW TO PLAY ***====================");
                Console.WriteLine("");
                Console.WriteLine("1. PLACE YOUR BET");
                Console.WriteLine("2. CHOOSE WHAT COLOR YOU BET");
                Console.WriteLine("3. IF YOU'RE RIGHT YOU WILL GET MULTIPLIER");
                Console.WriteLine("4. IF YOU FALSE, YOU WILL LOSE YOUR BET AND GET NOTHING");
                Console.WriteLine("5. IF YOUR CREDIT IS 0 YOU LOSE");
                Console.WriteLine("6. SERVER WILL RANDOMIZE THE DICE WITHIN 3 SECONDS FOR EACH PLAYER");
                Console.WriteLine("7. TO WIN THIS GAME YOU MUST HAVE 5000 CREDIT UNTUK KELUAR DARI PENJARA");
                Console.WriteLine("8. IF SOMEONE WIN THE GAME ALL CLIENT WILL CLOSE AUTOMATICALLY. GOODLUCK");
                Console.WriteLine("");
                Console.WriteLine("==================*** COLOR GUIDE ***====================");
                Console.WriteLine("");
                Console.WriteLine("RED = 5,8,11,14,3. GREEN = 6,9,12,15,4,17. BLUE = 7,10,13,16,18 \n");
                Console.WriteLine("RED, GREEN, & BLUE = 2X MULTIPLIER");
                Console.WriteLine("");
                Console.WriteLine("------ ESCAPE OR DIE BY Alfin x Amca  -----");
                Console.WriteLine("");
                break;

        }
    }





    public static void Main()
    {
        globalvar.status = false;
        while (!globalvar.status)
        {
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 12000);
                StreamReader reader = new StreamReader(client.GetStream());
                StreamWriter writer = new StreamWriter(client.GetStream());

                TcpClient client2 = new TcpClient("127.0.0.1", 12000);
                NetworkStream ns = client2.GetStream();
                Thread thread = new Thread(o => ReceiveData((TcpClient)o));
                thread.Start(client2);


                String s = String.Empty;
                String s1 = String.Empty;
                String ans = String.Empty;
                // int option = 0;

                int credit = 5400;
                bool main = true;
                int opt = 0;
                //  bool wrong = false;



                Console.WriteLine("=================*** CLIENT ***======================");
                Console.WriteLine("============*** YOU'RE CONNECTED ***=================");
                Console.WriteLine("      ---- ESCAPE OR DIE BY Alfin x Amca ----"        );
                Console.WriteLine();
                Console.Write("INPUT NAME : ");
                string uname = Console.ReadLine();

                while (uname == "")
                {
                    Console.WriteLine();
                    Console.Write("INVALID INPUT, PLEASE INPUT NAME : ");
                    uname = Console.ReadLine();
                }

                writer.WriteLine(uname);
                writer.Flush();



                Console.WriteLine("");
                Console.WriteLine("( W E L C O M E, " + uname + " !!!, YOU HAVE 500 CREDIT, HAPPY FARMING ^_^ )");
                Console.WriteLine("");




                while (main)
                {

                    Console.WriteLine("*** CHOOSE MENU ***");
                    Console.WriteLine("");

                    Console.WriteLine("1. PLAY");
                    Console.WriteLine("2. INSTRUCTION");
                    Console.WriteLine("3. CHECK CREDIT");
                    Console.WriteLine("4. BUY KEY");
                    Console.WriteLine("5. RESTART GAME");
                    Console.WriteLine("");


                    Console.WriteLine("");
                    Console.Write("PICK MENU : ");



                    string menu = Console.ReadLine();

                    while (menu != "5" && menu != "4" && menu != "3" && menu != "2" && menu != "1")
                    {


                        Console.WriteLine();
                        Console.Write("INVALID INPUT, PLEASE CHOOSE MENU : ");
                        menu = Console.ReadLine();


                    }

                    if (menu == "1")
                    {
                        globalvar.bc = true;
                    }


                    int option = Convert.ToInt16(menu);
                    pilihanMenu(option);

                    if (option == 5)
                    {
                        opt = 5;
                        writer.WriteLine(opt);
                        writer.Flush();
                        throw new Exception("RESTART GAME");
                    }



                    if (option == 4)
                    {
                        opt = 4;
                        writer.WriteLine(opt);
                        writer.Flush();

                        if (credit >= 5000)
                        {

                            credit = credit - 5000;
                            int win = 1001;
                            writer.WriteLine(win);
                            writer.Flush();
                            writer.WriteLine(uname + " WON THE GAME !!!");
                            writer.Flush();


                            Console.WriteLine();
                            Console.WriteLine("CONGRATULATIONS! YOU BUY KEY");
                            Console.WriteLine("=============*** YOU WON ***=============");
                            Console.WriteLine();
                        }

                        else
                        {
                            int win2 = 10012;
                            writer.WriteLine(win2);
                            writer.Flush();
                            Console.WriteLine();
                            Console.WriteLine("INSUFFICIENT CREDIT");
                            Console.WriteLine();
                        }
                    }



                    while (option == 1 && globalvar.bc == true)
                    {

                        opt = 1;
                        writer.WriteLine(opt);
                        writer.Flush();
                        if (credit == 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine("YOU HAVE NO CREDIT");
                            Console.WriteLine();
                            break;
                        }

                        Console.WriteLine("YOUR CREDIT NOW : " + credit);
                        Console.Write("PLACE YOUR BET : ");
                        s = Console.ReadLine();
                        int temp;
                        while (!int.TryParse(s, out temp) || temp <= 0 || temp > credit)
                        {
                            Console.Write("INVALID INPUT, PLEASE INPUT VALID NUMBER FOR YOUR BET : ");
                            s = Console.ReadLine();
                        }


                        temp = Convert.ToInt32(s);

                        if (temp <= credit)
                        {
                            credit = credit - temp;
                        }
                        Console.WriteLine("YOUR CREDIT NOW : " + credit);
                        writer.WriteLine(s);
                        writer.Flush();


                        Console.Write("PICK YOUR COLOR, TYPE R/G/B : ");
                        s1 = Console.ReadLine();

                        while (s1 != "R" && s1 != "G" && s1 != "B" && s1 != "r" && s1 != "g" && s1 != "b" && s1 != "RED" && s1 != "GREEN" && s1 != "BLUE" && s1 != "red" && s1 != "green" && s1 != "blue")
                        {

                            Console.WriteLine();
                            Console.Write("INVALID INPUT, PLEASE TYPE R/G/B : ");
                            s1 = Console.ReadLine();

                        }

                        writer.WriteLine(s1);
                        writer.Flush();
                        Console.WriteLine();
                        for (int countDown = 3; countDown >= 0; countDown--)
                        {
                            Console.CursorLeft = 0;
                            Console.Write("DICE ROLL IN {0} ", countDown);
                            Thread.Sleep(1000);

                        }
                        Console.WriteLine();
                        // server countdown


                        //update credit

                        String dc = reader.ReadLine();
                        String d1 = reader.ReadLine();
                        String d2 = reader.ReadLine();
                        String d3 = reader.ReadLine();
                        String td = reader.ReadLine();

                        String server_string = reader.ReadLine();
                        int creditServ = Convert.ToInt16(server_string);


                        Console.WriteLine();

                        Console.WriteLine($"DICE IS  [ {d1} ] [ {d2} ] [ {d3} ]");
                        Console.WriteLine("TOTAL DICE IS " + td);
                        Console.WriteLine(dc);
                        Console.WriteLine();
                        Console.WriteLine("CONGRATULATIONS! YOU GET : " + creditServ);
                        credit = credit + creditServ;
                        Console.WriteLine("YOUR CREDIT NOW : " + credit);
                        Console.WriteLine();

                        option = 0;
                    }

                    if (credit == 0)
                    {

                        Console.WriteLine("=============== *** YOU LOSE! *** ================");
                        Console.WriteLine();

                    }

                    if (option == 3)
                    {
                        opt = 3;
                        writer.WriteLine(opt);
                        writer.Flush();
                        Console.WriteLine();
                        Console.WriteLine("YOUR CREDIT NOW : " + credit);
                        Console.WriteLine();
                    }



                }
                reader.Close();
                writer.Close();
                client.Close();
                client.Client.Shutdown(SocketShutdown.Both);
                globalvar.status = true;
                Environment.Exit(0);
            }

            catch (Exception)
            {
                Console.WriteLine("XXXXXXXXXXXXXXXXXXX ERROR OCCURED XXXXXXXXXXXXXXXXXXX");
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>> ERROR LIST <<<<<<<<<<<<<<<<<<<<<");
                Console.WriteLine("1. SERVER SHUTDOWN (NO SERVER)");
                Console.WriteLine("2. YOU RESTART THE GAME");
                Console.WriteLine("");
                Console.WriteLine("............ PRESS ENTER TO RESTART GAME ............");
                string er = Console.ReadLine();
                if (er == "")
                {
                    globalvar.status = false;
                }

            }
        }

    }

    private static void ReceiveData(TcpClient cl)
    {

        NetworkStream ns = cl.GetStream();
        byte[] receivedBytes = new byte[1024];
        int byte_count;


        while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
        {

            Console.WriteLine(Encoding.ASCII.GetString(receivedBytes, 0, byte_count));


            for (int countDown = 5; countDown >= 0; countDown--)
            {
                Console.CursorLeft = 0;
                Console.Write("GAME CLOSING IN {0} ", countDown);
                System.Threading.Thread.Sleep(1000);
            }
            Console.WriteLine("");
            Console.WriteLine("GAME OVER");
            Console.WriteLine();

            break;


        }
        Environment.Exit(0);

    }
}