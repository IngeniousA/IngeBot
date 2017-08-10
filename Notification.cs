using System;
using Discord;
using Discord.Commands;
using System.Threading;

namespace IngeBot
{
    enum MentionType
    {
        Noone = 0,
        Author = 1
    }

    class Notification
    {
        static string Text { get; set; }
        public string ReadyStr { get; set; }
        public static string StringToSend { get; set; }
        public User Author { get; set; }
        static int Timing { get; set; }
        static DateTime Start { get; set; }
        static string ToMention { get; set; }
        public static bool Status { get; set; }
        Thread notifyThread = new Thread(new ParameterizedThreadStart(Send));

        public Notification(string text, User auth, int timing, MentionType ment, CommandEventArgs e)
        {
            Text = text;
            Author = auth;

            if (ment == MentionType.Author)
            {
                ToMention = auth.Mention;
            }

            ReadyStr = ToMention + " " + Text;
            StringToSend = ReadyStr;
            Timing = timing;
            Status = true;

            notifyThread.Start(e);
        }
                
        public static void Send(object e)
        {
            while (Status)
            {
                CommandEventArgs ComEv = (CommandEventArgs)e;
                ComEv.Channel.SendMessage(StringToSend);
                Thread.Sleep(Timing);
            }                       
        }

        public void Terminate()
        {
            Status = false;
            Timing = 0;
            Author = null;

        }

        public string Condition()
        {
            string statstr = ", message: " + ReadyStr + ", author: " + Author.Name;
            return statstr;
        }
    }

    class TimerN
    {
        static User Author { get; set; }
        static int Timing { get; set; }
        static bool Status { get; set; }
        Thread notifyThread = new Thread(new ParameterizedThreadStart(Send));

        public TimerN(User auth, int timing, CommandEventArgs e)
        {
            Author = auth;
            Timing = timing;

            notifyThread.Start(e);
        }

        public static void Send(object e)
        {
            CommandEventArgs ComEv = (CommandEventArgs)e;
            Thread.Sleep(Timing);
            ComEv.Channel.SendMessage(Author.Mention + ", time's up!");
        }
    }
}


