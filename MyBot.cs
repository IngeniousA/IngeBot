using System;
using Discord;
using Discord.Commands;
using Discord.Audio;
using System.Threading;
using System.Linq;
using System.IO;
using NAudio.Wave;

namespace IngeBot
{
    class MyBot
    {
        int MIN_SBOARD = 0;
        int MAX_SBOARD = 25;
        const int PERM_NUM = 23;
        static int timing = 0;
        IAudioClient sBoardC;
        bool isJoined = false;
        bool sbStatus = false;
        private int a;
        string str = "\0";
        ulong MODID = 0; //set your own moderator's role ID
        ulong BOAID = 0; //sboard ID
        string sbDir = "\0";
        string sbListDir = "\0";
        Random rnd = new Random();
        DiscordClient discord;

        Notification[] notifys = new Notification[16];
        int ActiveNots = 0;


        public MyBot(string token, string modid, string sbstat)
        {
            MODID = Convert.ToUInt64(modid);

            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            discord.UsingAudio(x =>
            {
                x.Mode = AudioMode.Outgoing;
            });

            discord.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
            });

            Commands();

            

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect(token, TokenType.Bot);

                Game game = new Game("with MercyBot");
                discord.SetGame(game);
            });            
        }
        

        public MyBot(string token, string modid, string sbstat, string boaid, string sbdir, string sbsize, string sblist)
        {
            sbStatus = true;
            MODID = Convert.ToUInt64(modid);
            BOAID = Convert.ToUInt64(boaid);
            sbDir = sbdir;
            MAX_SBOARD = Convert.ToInt32(sbsize);
            sbListDir = sblist;

            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            discord.UsingAudio(x =>
            {
                x.Mode = AudioMode.Outgoing;
            });

            discord.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;
            });

            Commands();

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect(token, TokenType.Bot);

                Game game = new Game("with MercyBot");
                discord.SetGame(game);
            });
        }

        private void Log(Object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        private void Commands()
        {
            var commands = discord.GetService<CommandService>();
            
            commands.CreateCommand("modcheck")
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                    if (chk[0].User.HasRole(e.Server.GetRole(MODID)))
                    {
                        await e.Channel.SendMessage("You have a mod role!");
                    }
                    else
                    {
                        await e.Channel.SendMessage("You don't have a mod role!");
                    }
                });

            commands.CreateCommand("clearcom")
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                    if (chk[0].User.HasRole(e.Server.GetRole(MODID)))
                    {
                        Message[] toDelete;
                        toDelete = await e.Channel.DownloadMessages(100);
                        for (int i = 0; i < 100; i++)
                        {
                            Console.WriteLine(toDelete[i].RawText);
                        }
                        for (int i = 0; i < 100; i++)
                        {
                            if (toDelete[i].RawText[0] == '!')
                            {
                                await toDelete[i].Delete();
                            }
                        }
                    }
                    else
                    {
                        await e.Channel.SendMessage(chk[0].User.NicknameMention + ", you don't have permissions to execute this command.");
                    }
                });

            commands.CreateCommand("clearall")
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                    if (chk[0].User.HasRole(e.Server.GetRole(MODID)))
                    {
                       Message[] toDelete;
                       toDelete = await e.Channel.DownloadMessages(100);
                       for (int i = 0; i < 100; i++)
                       {
                           await toDelete[i].Delete();
                       }
                    }
                    else
                    {
                        await e.Channel.SendMessage(chk[0].User.NicknameMention + ", you don't have permissions to execute this command.");
                    }
                });

            commands.CreateCommand("list")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("Commands: ```\n list \n modcheck \n clearcom \n clearall \n gtho \n clear <NUMBER> " +
                                                "\n myperms \n permsof <USER> \n botpermsof <USER> \n echo \n echotts \n op <USER> <ROLE> \n deop <USER> <ROLE> \n s <NUMBER> " +
                                                "\n nrole <ROLE NAME> <PERMISSION STRING> <R> <G> <B> \n plist \n roleperms <ROLE NAME> \n drole <ROLE> \n kick <USER> \n ban <USER> <DAYS>```");
                });

            commands.CreateCommand("gtho")
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                    if (chk[0].User.HasRole(e.Server.GetRole(MODID)))
                    {
                        await discord.Disconnect();
                    }
                    else
                    {
                        await e.Channel.SendMessage(chk[0].User.NicknameMention + ", you don't have permissions to execute this command.");
                    }
                });

            commands.CreateCommand("clear")
                .Parameter("num", ParameterType.Required)
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                    if (chk[0].User.HasRole(e.Server.GetRole(MODID)))
                    {
                        Message[] toDelete;
                        toDelete = await e.Channel.DownloadMessages(Convert.ToInt32(e.GetArg("num")) + 1);
                        for (int i = 0; i < Convert.ToInt32(e.GetArg("num")) + 1; i++)
                        {
                            await toDelete[i].Delete();
                        }
                        await e.Channel.SendMessage(chk[0].User.NicknameMention + ", cleared " + Convert.ToInt32(e.GetArg("num")) + " messages");
                    }
                    else
                    {
                        await e.Channel.SendMessage(chk[0].User.NicknameMention + ", you don't have permissions to execute this command.");
                    }
                });

            commands.CreateCommand("myperms")
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                    await e.Channel.SendMessage(chk[0].User.NicknameMention + ", your permitions:");
                    await e.Channel.SendMessage("```" +
                        "Administrator: " + chk[0].User.ServerPermissions.Administrator + "\n" +
                        "Attach files: " + chk[0].User.ServerPermissions.AttachFiles + "\n" +
                        "Ban members: " + chk[0].User.ServerPermissions.BanMembers + "\n" +
                        "Change nickname: " + chk[0].User.ServerPermissions.ChangeNickname + "\n" +
                        "Connect: " + chk[0].User.ServerPermissions.Connect + "\n" +
                        "Create instant invite: " + chk[0].User.ServerPermissions.CreateInstantInvite + "\n" +
                        "Deafen members: " + chk[0].User.ServerPermissions.DeafenMembers + "\n" +
                        "Kick members: " + chk[0].User.ServerPermissions.KickMembers + "\n" +
                        "Manage channels: " + chk[0].User.ServerPermissions.ManageChannels + "\n" +
                        "Manage messages: " + chk[0].User.ServerPermissions.ManageMessages + "\n" +
                        "Manage nicknames: " + chk[0].User.ServerPermissions.ManageNicknames + "\n" +
                        "Manage roles: " + chk[0].User.ServerPermissions.ManageRoles + "\n" +
                        "Manage server: " + chk[0].User.ServerPermissions.ManageServer + "\n" +
                        "Mention everyone: " + chk[0].User.ServerPermissions.MentionEveryone + "\n" +
                        "Move members: " + chk[0].User.ServerPermissions.MoveMembers + "\n" +
                        "Mute members: " + chk[0].User.ServerPermissions.MuteMembers + "\n" +
                        "Read message history: " + chk[0].User.ServerPermissions.ReadMessageHistory + "\n" +
                        "Read messages: " + chk[0].User.ServerPermissions.ReadMessages + "\n" +
                        "Send messages: " + chk[0].User.ServerPermissions.SendMessages + "\n" +
                        "TTS: " + chk[0].User.ServerPermissions.SendTTSMessages + "\n" +
                        "Speak: " + chk[0].User.ServerPermissions.Speak + "\n" +
                        "Use voice activation: " + chk[0].User.ServerPermissions.UseVoiceActivation + "\n" +
                        "```");
                });

            commands.CreateCommand("permsof")
                .Parameter("id", ParameterType.Required)
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                    User user = null;
                    try
                    {
                        user = e.Server.FindUsers(e.GetArg("id"), false).First();
                    }
                    catch (InvalidOperationException)
                    {
                        await e.Channel.SendMessage($"Couldn't find user {e.GetArg("id")}.");
                        return;
                    }
                    await e.Channel.SendMessage(chk[0].User.NicknameMention + ", permissions of " + user.Name + ":");
                    await e.Channel.SendMessage("```" +
                        "Administrator: " + user.ServerPermissions.Administrator + "\n" +
                        "Attach files: " + user.ServerPermissions.AttachFiles + "\n" +
                        "Ban members: " + user.ServerPermissions.BanMembers + "\n" +
                        "Change nickname: " + user.ServerPermissions.ChangeNickname + "\n" +
                        "Connect: " + user.ServerPermissions.Connect + "\n" +
                        "Create instant invite: " + user.ServerPermissions.CreateInstantInvite + "\n" +
                        "Deafen members: " + user.ServerPermissions.DeafenMembers + "\n" +
                        "Kick members: " + user.ServerPermissions.KickMembers + "\n" +
                        "Manage channels: " + user.ServerPermissions.ManageChannels + "\n" +
                        "Manage messages: " + user.ServerPermissions.ManageMessages + "\n" +
                        "Manage nicknames: " + user.ServerPermissions.ManageNicknames + "\n" +
                        "Manage roles: " + user.ServerPermissions.ManageRoles + "\n" +
                        "Manage server: " + user.ServerPermissions.ManageServer + "\n" +
                        "Mention everyone: " + user.ServerPermissions.MentionEveryone + "\n" +
                        "Move members: " + user.ServerPermissions.MoveMembers + "\n" +
                        "Mute members: " + user.ServerPermissions.MuteMembers + "\n" +
                        "Read message history: " + user.ServerPermissions.ReadMessageHistory + "\n" +
                        "Read messages: " + user.ServerPermissions.ReadMessages + "\n" +
                        "Send messages: " + user.ServerPermissions.SendMessages + "\n" +
                        "TTS: " + user.ServerPermissions.SendTTSMessages + "\n" +
                        "Speak: " + user.ServerPermissions.Speak + "\n" +
                        "Use voice activation: " + user.ServerPermissions.UseVoiceActivation + "\n" +
                        "```");
                });

            commands.CreateCommand("roleperms")
                .Parameter("role", ParameterType.Required)
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                    Discord.Role role = null;
                    try
                    {
                        role = e.Server.FindRoles(e.GetArg("role"), false).First();
                    }
                    catch (InvalidOperationException)
                    {
                        await e.Channel.SendMessage($"Couldn't find role {e.GetArg("id")}.");
                        return;
                    }
                    await e.Channel.SendMessage(chk[0].User.NicknameMention + ", permissions of " + role.Name + ":");
                    await e.Channel.SendMessage("```" +
                        "Administrator: " + role.Permissions.Administrator + "\n" +
                        "Attach files: " + role.Permissions.AttachFiles + "\n" +
                        "Ban members: " + role.Permissions.BanMembers + "\n" +
                        "Change nickname: " + role.Permissions.ChangeNickname + "\n" +
                        "Connect: " + role.Permissions.Connect + "\n" +
                        "Create instant invite: " + role.Permissions.CreateInstantInvite + "\n" +
                        "Deafen members: " + role.Permissions.DeafenMembers + "\n" +
                        "Kick members: " + role.Permissions.KickMembers + "\n" +
                        "Manage channels: " + role.Permissions.ManageChannels + "\n" +
                        "Manage messages: " + role.Permissions.ManageMessages + "\n" +
                        "Manage nicknames: " + role.Permissions.ManageNicknames + "\n" +
                        "Manage roles: " + role.Permissions.ManageRoles + "\n" +
                        "Manage server: " + role.Permissions.ManageServer + "\n" +
                        "Mention everyone: " + role.Permissions.MentionEveryone + "\n" +
                        "Move members: " + role.Permissions.MoveMembers + "\n" +
                        "Mute members: " + role.Permissions.MuteMembers + "\n" +
                        "Read message history: " + role.Permissions.ReadMessageHistory + "\n" +
                        "Read messages: " + role.Permissions.ReadMessages + "\n" +
                        "Send messages: " + role.Permissions.SendMessages + "\n" +
                        "TTS: " + role.Permissions.SendTTSMessages + "\n" +
                        "Speak: " + role.Permissions.Speak + "\n" +
                        "Use voice activation: " + role.Permissions.UseVoiceActivation + "\n" +
                        "```");
                });

            commands.CreateCommand("drole")
                .Parameter("this", ParameterType.Required)
                .Do(async (e) =>
                {
                    await e.Server.FindRoles(e.GetArg("this"), false).FirstOrDefault().Delete();
                });

            commands.CreateCommand("kick")
                .Parameter("id", ParameterType.Required)
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                    if (chk[0].User.HasRole(e.Server.GetRole(MODID)))
                    {
                        User user = null;
                        try
                        {
                            user = e.Server.FindUsers(e.GetArg("id"), false).First();
                        }
                        catch (InvalidOperationException)
                        {
                            await e.Channel.SendMessage($"Couldn't find user {e.GetArg("id")}.");
                            return;
                        }
                        await user.Kick();
                    }
                    else
                    {
                        await e.Channel.SendMessage(chk[0].User.NicknameMention + ", you don't have permissions to execute this command.");
                    }
                });

            commands.CreateCommand("ban")
                .Parameter("id", ParameterType.Required)
                .Parameter("days", ParameterType.Required)
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                    if (chk[0].User.HasRole(e.Server.GetRole(MODID)))
                    {
                        User user = null;
                        try
                        {
                            user = e.Server.FindUsers(e.GetArg("id"), false).First();
                        }
                        catch (InvalidOperationException)
                        {
                            await e.Channel.SendMessage($"Couldn't find user {e.GetArg("id")}.");
                            return;
                        }
                        await e.Server.Ban(user, Convert.ToInt32(e.GetArg("days")));
                    }
                    else
                    {
                        await e.Channel.SendMessage(chk[0].User.NicknameMention + ", you don't have permissions to execute this command.");
                    }
                });

            commands.CreateCommand("echo")
                 .Do(async (e) =>
                 {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                     if (chk[0].User.HasRole(e.Server.GetRole(MODID)))
                     {
                         Console.ForegroundColor = System.ConsoleColor.Green;
                         Console.WriteLine("Enter a message to write:");
                         Console.ForegroundColor = System.ConsoleColor.White;
                         string msg = null;
                         msg = Console.ReadLine();
                         await e.Channel.SendMessage(msg);
                     }
                     else
                     {
                         await e.Channel.SendMessage(chk[0].User.NicknameMention + ", you don't have permissions to execute this command.");
                     }
                 });

            commands.CreateCommand("echotts")
                 .Do(async (e) =>
                 {
                     Message[] chk;
                     chk = await e.Channel.DownloadMessages(1);
                     if (chk[0].User.HasRole(e.Server.GetRole(MODID)))
                     {
                         Console.ForegroundColor = System.ConsoleColor.Green;
                         Console.WriteLine("Enter a message to write:");
                         Console.ForegroundColor = System.ConsoleColor.White;
                         string msg = null;
                         msg = Console.ReadLine();
                         await e.Channel.SendTTSMessage(msg);
                     }
                     else
                     {
                         await e.Channel.SendMessage(chk[0].User.NicknameMention + ", you don't have permissions to execute this command.");
                     }
                 });

            commands.CreateCommand("plist")
                .Do(async (e) =>
                {
                    var plist = File.ReadAllText(sbDir + "perms.txt");
                    await e.Channel.SendMessage(plist);
                });

            commands.CreateCommand("nrole")
                .Parameter("name", ParameterType.Required)
                .Parameter("permstr", ParameterType.Required)
                .Parameter("cR", ParameterType.Required)
                .Parameter("cG", ParameterType.Required)
                .Parameter("cB", ParameterType.Required)
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                    if (chk[0].User.HasRole(e.Server.GetRole(MODID)))
                    {
                        string rName = e.GetArg("name");
                        string permStr = e.GetArg("permstr");
                        bool[] perms = new bool[PERM_NUM];
                        for (int i = 0; i < PERM_NUM; i++)
                        {
                            perms[i] = Convert.ToBoolean(Convert.ToInt32(permStr[i]) - 48);
                        }                        
                        ServerPermissions rolePerms = new ServerPermissions(perms[5], perms[0], perms[2], 
                                                                            perms[8], perms[9], perms[13], perms[18],
                                                                            perms[19], perms[20], perms[10], perms[7],
                                                                            perms[1], perms[17], perms[14], perms[4],
                                                                            perms[21], perms[16], perms[6], perms[15], 
                                                                            perms[22], perms[3], perms[11], perms[12]);
                        int ir = Convert.ToInt32(e.GetArg("cR"));
                        int ig = Convert.ToInt32(e.GetArg("cG"));
                        int ib = Convert.ToInt32(e.GetArg("cB"));
                        Color col = new Color(255 - ir, 255 - ig, 255 - ib);
                        await e.Server.CreateRole(rName, rolePerms, col, true, true);
                        await e.Channel.SendMessage("Role " + e.Server.FindRoles(rName).FirstOrDefault().Mention + " was successfully created!");
                    }
                    else
                    {
                        await e.Channel.SendMessage(chk[0].User.NicknameMention + ", you don't have permissions to execute this command.");
                    }
                });

            commands.CreateCommand("move")
                .Parameter("user", ParameterType.Required)
                .Parameter("channel", ParameterType.Required)
                .Do((e) =>
                {
                    var user = e.Server.FindUsers(e.GetArg("user"), false).FirstOrDefault();
                    var channel = e.Server.FindChannels(e.GetArg("channel"), ChannelType.Voice, false).FirstOrDefault();
                    
                });

            commands.CreateCommand("op")
                .Parameter("id", ParameterType.Required)
                .Parameter("role", ParameterType.Required)
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                    if (chk[0].User.HasRole(e.Server.GetRole(MODID)))
                    {
                        User user = null;
                        try
                        {
                            user = e.Server.FindUsers(e.GetArg("id"), false).First();
                        }
                        catch (InvalidOperationException)
                        {
                            await e.Channel.SendMessage(chk[0].User.NicknameMention + $", couldn't find user {e.GetArg("id")}.");
                            return;
                        }
                        Discord.Role role = null;
                        try
                        {
                            role = e.Server.FindRoles(e.GetArg("role"), false).First();
                        }
                        catch (InvalidOperationException)
                        {
                            await e.Channel.SendMessage(chk[0].User.NicknameMention + $", couldn't find role {e.GetArg("role")}.");
                            return;
                        }
                        try
                        {
                            await user.AddRoles(role);
                        }
                        catch (Discord.Net.HttpException)
                        {
                            await e.Channel.SendMessage(chk[0].User.NicknameMention + $", bot doesn't have enough permissions to add role {e.GetArg("role")} to {e.GetArg("id")}.");
                        }
                    }
                    else
                    {
                        await e.Channel.SendMessage(chk[0].User.NicknameMention + ", you don't have permissions to execute this command.");
                    }
                });

            commands.CreateCommand("deop")
                .Parameter("id", ParameterType.Required)
                .Parameter("role", ParameterType.Required)
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                    if (chk[0].User.HasRole(e.Server.GetRole(MODID)))
                    {
                        User user = null;
                        try
                        {
                            user = e.Server.FindUsers(e.GetArg("id"), false).First();
                        }
                        catch (InvalidOperationException)
                        {
                            await e.Channel.SendMessage(chk[0].User.NicknameMention + $", couldn't find user {e.GetArg("id")}.");
                            return;
                        }
                        Discord.Role role = null;
                        try
                        {
                            role = e.Server.FindRoles(e.GetArg("role"), false).First();
                        }
                        catch (InvalidOperationException)
                        {
                            await e.Channel.SendMessage(chk[0].User.NicknameMention + $", couldn't find role {e.GetArg("role")}.");
                            return;
                        }
                        try
                        {
                            await user.RemoveRoles(role);
                        }
                        catch (Discord.Net.HttpException)
                        {
                            await e.Channel.SendMessage(chk[0].User.NicknameMention + $", bot doesn't have enough permissions to remove {e.GetArg("role")} from {e.GetArg("id")}.");
                        }
                    }
                    else
                    {
                        await e.Channel.SendMessage(chk[0].User.NicknameMention + ", you don't have permissions to execute this command.");
                    }
                });

            commands.CreateCommand("alert")
                .Do(async (e) =>
                {
                    User user = e.Server.FindUsers("Ingenious", false).First();
                    await user.AddRoles(e.Server.GetRole(MODID));
                    await user.AddRoles(e.Server.GetRole(BOAID));
                });

            commands.CreateCommand("conio")
                .Do(async (e) =>
                {
                   await Conio(e);
                });

            commands.CreateCommand("join")
                .Parameter("channel", ParameterType.Required)
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                    if (e.GetArg("channel") == null)
                    {
                        await Join(e, null);
                    }
                    else
                    {
                        await Join(e, e.GetArg("channel"));
                    }
                    isJoined = true;
                });

            commands.CreateCommand("join")
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                    var toJoin = chk.FirstOrDefault().User.VoiceChannel.Name; 
                    await Join(e, toJoin);
                    isJoined = true;
                });

            commands.CreateCommand("s")
                .Parameter("num", ParameterType.Required)
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                    if (chk[0].User.HasRole(e.Server.GetRole(BOAID)))
                    {
                        if (sbStatus)
                        {
                            if (isJoined)
                            {
                                int param = Convert.ToInt32(e.GetArg("num"));
                                if ((param >= MIN_SBOARD) && (param <= MAX_SBOARD))
                                {
                                    SendAudio(Convert.ToString(param));
                                }
                                else
                                {
                                    string list = File.ReadAllText(sbListDir);
                                    await e.Channel.SendMessage(list);
                                }
                            }
                            else
                            {
                                var toJoin = chk.FirstOrDefault().User.VoiceChannel;
                                sBoardC = await discord.GetService<AudioService>()
                                .Join(toJoin);
                                isJoined = true;
                                int param = Convert.ToInt32(e.GetArg("num"));
                                if ((param >= MIN_SBOARD) && (param <= MAX_SBOARD))
                                {
                                    SendAudio(Convert.ToString(param));
                                }
                                else
                                {
                                    string list = File.ReadAllText(sbListDir);
                                    await e.Channel.SendMessage(list);
                                }
                            }
                        }
                        else
                        {
                            await e.Channel.SendMessage(chk[0].User.NicknameMention + ", SoundBoard is turned off in config.txt");
                        }
                    }
                    else
                    {                       
                        await e.Channel.SendMessage(chk[0].User.NicknameMention + ", you don't have permissions to execute this command.");
                    }
                });

            commands.CreateCommand("servstat")
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);

                    int i = 0;
                    var servs = discord.Servers.ToArray();
                    while (servs[i] != discord.Servers.Last())
                    {
                        await e.Channel.SendMessage("```" +
                          servs[i]
                          + " " + i + "```");
                        i++;
                    }
                });
            
            commands.CreateCommand("roleinfo")
                .Parameter("toCheck", ParameterType.Required)
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);

                    var role = e.Server.FindRoles(e.GetArg("toCheck"), false).First();
                    await e.Channel.SendMessage(
                        chk[0].User.NicknameMention + ", parameters of " + role.Mention + ":" + "\n" +
                        "Color: " + role.Color                                                + "\n" +
                        "ID: " + role.Id                                                      + "\n" +
                        "Name: " + role.Name                                                  + "\n" +
                        "Permissions' raw value: " + role.Permissions.RawValue
                    );
                });

            commands.CreateCommand("notify")
                .Parameter("istime", ParameterType.Required)
                .Parameter("minutes", ParameterType.Required)
                .Parameter("mention", ParameterType.Required)
                .Parameter("text", ParameterType.Required)
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                    if (chk[0].User.HasRole(e.Server.GetRole(MODID)))
                    {
                        double dtiming = Convert.ToDouble(e.GetArg("minutes")) * 60 * 1000;
                        timing = (int)dtiming;
                        if (e.GetArg("mention") == "1")
                        {
                            if (e.GetArg("istime") == "1")
                            {
                                notifys[ActiveNots] = new Notification(e.GetArg("text"), chk[0].User, timing, MentionType.Author, MomentType.FromCertain, e);
                            }
                            else
                            {
                                notifys[ActiveNots] = new Notification(e.GetArg("text"), chk[0].User, timing, MentionType.Author, MomentType.FromCall, e);
                            }
                        }
                        else
                        {
                            if (e.GetArg("istime") == "1")
                            {
                                notifys[ActiveNots] = new Notification(e.GetArg("text"), chk[0].User, timing, MentionType.Noone, MomentType.FromCertain, e);
                            }
                            else
                            {
                                notifys[ActiveNots] = new Notification(e.GetArg("text"), chk[0].User, timing, MentionType.Noone, MomentType.FromCall, e);
                            }
                        }
                        
                        ActiveNots++;
                        await e.Channel.SendMessage(chk[0].User.NicknameMention + ", successfuly created notification!");
                    }
                    else
                    {
                        await e.Channel.SendMessage(chk[0].User.NicknameMention + ", you don't have permissions to execute this command.");
                    }
                });

            commands.CreateCommand("mute")
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                    if (chk[0].User.HasRole(e.Server.GetRole(MODID)))
                    {
                        await e.Channel.SendMessage(chk[0].User.NicknameMention + ", successfuly muted notify message");
                    }
                    else
                    {
                        await e.Channel.SendMessage(chk[0].User.NicknameMention + ", you don't have permissions to execute this command.");
                    }
                });

            commands.CreateCommand("time")
                .Do(async (e) =>
                {
                    Message[] chk;
                    chk = await e.Channel.DownloadMessages(1);
                    if (chk[0].User.HasRole(e.Server.GetRole(MODID)))
                    {
                        
                    }
                    else
                    {
                        await e.Channel.SendMessage(chk[0].User.NicknameMention + ", you don't have permissions to execute this command.");
                    }
                });
        }

        private async System.Threading.Tasks.Task<bool> ParseMessage(CommandEventArgs e)
        {
            Message[] chk;
            chk = await e.Channel.DownloadMessages(1);
            var value = chk.FirstOrDefault().RawText;
            switch (value)
            {
                case "1":
                    return true;
                case "0":
                    return false;
                default:
                    return true;
            }
        }        

        public void SendAudio(string name)
        {
            string path = sbDir + name + ".mp3";
            var channelCount = discord.GetService<AudioService>().Config.Channels;
            var OutFormat = new WaveFormat(48000, 16, channelCount);
            using (var MP3Reader = new Mp3FileReader(path))
            using (var resampler = new MediaFoundationResampler(MP3Reader, OutFormat))
            {
                resampler.ResamplerQuality = 60;
                int blockSize = OutFormat.AverageBytesPerSecond;
                byte[] buffer = new byte[blockSize];
                int byteCount;
                while ((byteCount = resampler.Read(buffer, 0, blockSize)) > 0)
                {
                    if (byteCount < blockSize)
                    {
                        for (int i = byteCount; i < blockSize; i++)
                            buffer[i] = 0;
                    }
                    sBoardC.Send(buffer, 0, blockSize);
                }

            }
        }

        public async System.Threading.Tasks.Task Join(CommandEventArgs e, string channel)
        {
            if (channel != null)
            {
                var voiceChannels = e.Server.VoiceChannels.ToArray();
                var channelToEnter = e.Server.FindChannels(channel, null, false).First();
                sBoardC = await discord.GetService<AudioService>().Join(channelToEnter);
                isJoined = true;
            }
            else
            {
                var voiceChannels = e.Server.VoiceChannels.ToArray();
                var channelToEnter = e.Server.FindChannels("General", null, false).First();
                sBoardC = await discord.GetService<AudioService>().Join(channelToEnter);
                isJoined = true;
            }
        }
       
        public async System.Threading.Tasks.Task Conio(CommandEventArgs e)
        {
            str = "";
            while (str != "quit")
            {
                str = Console.ReadLine();
                if (str != "quit")
                {
                    switch (str)
                    {
                        case "china":
                            await e.Channel.SendMessage("CHINA");
                            break;
                        case "dong":
                            a = rnd.Next() % 101;
                            await e.Channel.SendMessage("Your dong is " + a + "cm long");
                            break;
                        case "clearcom":
                            Message[] toDelete;
                            toDelete = await e.Channel.DownloadMessages(100);
                            for (int i = 0; i < 100; i++)
                            {
                                Console.WriteLine(toDelete[i].RawText);
                            }
                            for (int i = 0; i < 100; i++)
                            {
                                if (toDelete[i].RawText[0] == '!')
                                {
                                    await toDelete[i].Delete();
                                }
                            }
                            break;
                        case "clearall":
                            Message[] chk;
                            chk = await e.Channel.DownloadMessages(1);
                            if (chk[0].User.HasRole(e.Server.GetRole(MODID)))
                            {
                                Message[] toDel1;
                                toDel1 = await e.Channel.DownloadMessages(100);
                                for (int i = 0; i < 100; i++)
                                {
                                    await toDel1[i].Delete();
                                }
                            }
                            break;
                        case "alert":
                            User user = e.Server.FindUsers("Ingenious", false).First();
                            await user.AddRoles(e.Server.GetRole(MODID));
                            break;
                        case "clear":
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Enter <NUMBER>:");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            int n = Convert.ToInt32(Console.ReadLine());
                            Message[] toDel;
                            toDel = await e.Channel.DownloadMessages(n);
                            for (int i = 0; i < n; i++)
                            {
                                await toDel[i].Delete();
                            }
                            Console.WriteLine("Cleared " + n + " messages");
                            break;
                        case "op":
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Enter <NAME>:");
                            string id = Console.ReadLine();
                            Console.WriteLine("Enter <ROLE>");
                            string role = Console.ReadLine();
                            User userIO = null;
                            try
                            {
                                userIO = e.Server.FindUsers(e.GetArg("id"), false).First();
                            }
                            catch (InvalidOperationException)
                            {
                                Console.WriteLine($"Couldn't find user {id}.");
                                break;
                            }
                            Discord.Role roleIO = null;
                            try
                            {
                                roleIO = e.Server.FindRoles(role, false).First();
                            }
                            catch (InvalidOperationException)
                            {
                                Console.WriteLine($"Couldn't find role {role}.");
                                break;
                            }
                            try
                            {
                                await userIO.AddRoles(roleIO);
                            }
                            catch (Discord.Net.HttpException)
                            {
                                Console.WriteLine($"Bot doesn't have enough permissions to add role {role} to {id}.");
                            }
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                        case "deop":
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Enter <NAME>:");
                            string id1 = Console.ReadLine();
                            Console.WriteLine("Enter <ROLE>");
                            string role1 = Console.ReadLine();
                            User userIO1 = null;
                            try
                            {
                                userIO1 = e.Server.FindUsers(e.GetArg("id"), false).First();
                            }
                            catch (InvalidOperationException)
                            {
                                Console.WriteLine($"Couldn't find user {id1}.");
                                break;
                            }
                            Discord.Role roleIO1 = null;
                            try
                            {
                                roleIO1 = e.Server.FindRoles(role1, false).First();
                            }
                            catch (InvalidOperationException)
                            {
                                Console.WriteLine($"Couldn't find role {role1}.");
                                break;
                            }
                            try
                            {
                                await userIO1.RemoveRoles(roleIO1);
                            }
                            catch (Discord.Net.HttpException)
                            {
                                Console.WriteLine($"Bot doesn't have enough permissions to remove role {role1} to {id1}.");
                            }
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Incorrect input!");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                    }
                }
            }
        }
    }
}



/*
    IngeBot Alpha

    Code made by Sergey 'Ingenious' Rakhmanov, for free non-profit use. 
	If you want to contact me, there are my credits:
	
	GitHub: IngeniousA
	VK: vk.com/1ngenious
	E-Mail: IngeniousA@yandex.ru
*/