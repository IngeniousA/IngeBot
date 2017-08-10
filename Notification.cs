using System;
using Discord;
using Discord.Commands;
using System.Threading;

namespace IngeBot
{
    enum MomentType
    {
        FromCall = 0,
        FromCertain = 1
    }
    
    enum MentionType
    {
        Noone = 0,
        Author = 1
    }

    class Notification
    {
        static string Text { get; set; }
        static string ReadyStr { get; set; }
        static User Author { get; set; }
        static int Timing { get; set; }
        static MomentType Moment { get; set; }
        static DateTime Start { get; set; }
        static string ToMention { get; set; }
        public static bool Status { get; set; }
        Thread notifyThread = new Thread(new ParameterizedThreadStart(Send));

        public Notification(string text, User auth, int timing, MentionType ment, MomentType mom, CommandEventArgs e)
        {
            Text = text;
            Author = auth;

            if (ment == MentionType.Author)
            {
                ToMention = auth.Mention;
            }

            if (mom == MomentType.FromCall)
            {                
                ReadyStr = ToMention + " " + Text;
                Timing = timing;
                Start = DateTime.Now;
                Status = true;
            }
            else
            {
                ReadyStr = ToMention + " It's ";
                Timing = timing;
                Start = DateTime.Now;
                Start.AddMinutes(60 - Start.Minute);
                Status = true;
            }

            notifyThread.Start(e);
        }
                
        public static void Send(object e)
        {
            while (Status)
            {
                CommandEventArgs ComEv = (CommandEventArgs)e;
                ComEv.Channel.SendMessage(ReadyStr);
                Thread.Sleep(Timing);
            }                       
        }

        public void Terminate()
        {
            Status = false;
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


