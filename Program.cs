using System;
using System.IO;

namespace IngeBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("IngeBot - Bot for Discord");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Please notice that this is an alpha version and can include bugs \nand other wierd things." +
                "\n" + "Thank you for your attention." +
                "\n" + "------------LOGS:------------");
            if (File.Exists("config.txt"))
            {
                StreamReader config = new StreamReader("config.txt");
                string token = config.ReadLine();
                string modid = config.ReadLine();
                string sbstat = config.ReadLine(); 
                if (sbstat == "1")
                {
                    string boaid = config.ReadLine();
                    string sbdir = config.ReadLine();
                    string sbsze = config.ReadLine();
                    string sblist = config.ReadLine();
                    MyBot bot = new MyBot(token, modid, sbstat, boaid, sbdir, sbsze, sblist);
                }
                else
                {
                    MyBot bot = new MyBot(token, modid, sbstat);
                }
            }
            else
            {
                Console.WriteLine("config.txt doesn't exist!");
            }
        }
    }
}

/*
    Code made by Sergey 'Ingenious' Rakhmanov, for free non-profit use. 
	If you want to contact me, there are my credits:
	
	GitHub: IngeniousA
	VK: vk.com/1ngenious
	E-Mail: crashtranslator@yandex.ru
*/
