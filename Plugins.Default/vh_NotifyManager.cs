using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Threading;
using System.Data;
using AoLib.Utils;

namespace VhaBot.Plugins
{
    public class NotifyManager : PluginBase
    {
        public NotifyManager()
        {
            this.Name = "Notify List Manager";
            this.InternalName = "vhNotifyManager";
            this.Author = "Vhab";
            this.Version = 101;
            this.DefaultState = PluginState.Installed;
            this.Commands = new Command[] {
                new Command("notify", true, UserLevel.Leader),
                new Command("notify add", true, UserLevel.Leader),
                new Command("notify remove", true, UserLevel.Leader),
                new Command("notify sync", true, UserLevel.Admin),
                new Command("notify clean", true, UserLevel.SuperAdmin)
            };
        }

        public override void OnLoad(BotShell bot) { }
        public override void OnUnload(BotShell bot) { }

        public override void OnCommand(BotShell bot, CommandArgs e)
        {
            switch (e.Command)
            {
                case "notify":
                    string[] notifyList = bot.FriendList.List("notify");
                    RichTextWindow window = new RichTextWindow(bot);
                    window.AppendTitle("Notify List");
                    foreach (string user in notifyList)
                    {
                        UserLevel userLevel = bot.Users.GetUser(user);
                        window.AppendHighlight(Format.UppercaseFirst(user));
                        window.AppendNormal(" (" + userLevel + ")");
                        window.AppendLineBreak();
                    }
                    bot.SendReply(e, HTML.CreateColorString(bot.ColorHeaderHex, notifyList.Length.ToString()) + " Users �� ", window);
                    break;
                case "notify sync":
                    bot.SendReply(e, "Notify �� Synchronizing Friendslist...");
                    bot.FriendList.Sync();
                    bot.SendReply(e, "Notify �� Synchronized Friendslist");
                    break;
                case "notify clean":
                    if (e.Args.Length == 0 || e.Args[0] != "confirm")
                    {
                        bot.SendReply(e, "This command will remove ALL non-members from the notify list. If you wish to continue use: /tell " +
                                          bot.Character + " notify clean confirm");
                        break;
                    }
                    string[] notifyList1 = bot.FriendList.List("notify");
                    foreach (string user1 in notifyList1)
                    {
                        UserLevel userLevel1 = bot.Users.GetUser(user1);
                        if (userLevel1 < UserLevel.Member)
                            bot.FriendList.Remove("notify", user1);
                    }
                    bot.SendReply(e, "All non-members on notify list have been removed");
                    break;
                case "notify add":
                case "notify remove":
                    if (e.Args.Length < 1)
                    {
                        bot.SendReply(e, "Correct Usage: " + e.Command + " [username]");
                        break;
                    }
                    if (bot.GetUserID(e.Args[0]) < 1)
                    {
                        bot.SendReply(e, "No such user: " + HTML.CreateColorString(bot.ColorHeaderHex, e.Args[0]));
                        break;
                    }
                    string username = Format.UppercaseFirst(e.Args[0]);
                    switch (e.Command)
                    {
                        case "notify add":
                            if (bot.FriendList.IsFriend("notify", username))
                            {
                                bot.SendReply(e, HTML.CreateColorString(bot.ColorHeaderHex, Format.UppercaseFirst(username)) + " already is on the notify list");
                                break;
                            }
                            bot.FriendList.Add("notify", username);
                            bot.SendReply(e, HTML.CreateColorString(bot.ColorHeaderHex, Format.UppercaseFirst(username)) + " has been added to the notify list");
                            break;
                        case "notify remove":
                            if (!bot.FriendList.IsFriend("notify", username))
                            {
                                bot.SendReply(e, HTML.CreateColorString(bot.ColorHeaderHex, Format.UppercaseFirst(username)) + " is not on the notify list");
                                break;
                            }
                            bot.FriendList.Remove("notify", username);
                            bot.SendReply(e, HTML.CreateColorString(bot.ColorHeaderHex, Format.UppercaseFirst(username)) + " has been removed from the notify list");
                            break;
                    }
                    break;
            }
        }

        public override string OnHelp(BotShell bot, string command)
        {
            switch (command)
            {
                case "notify":
                    return "Shows a list of all users currently on the notify list.\n" +
                        "Usage: /tell " + bot.Character + " members";
                case "notify add":
                    return "Allows you to add a new user to the notify list.\n" +
                        "Usage: /tell " + bot.Character + " notify add [username]";
                case "notify remove":
                    return "Allows you to remove a user from the notify list.\n" +
                        "Usage: /tell " + bot.Character + " notify remove [username]";
            }
            return null;
        }
    }
}
