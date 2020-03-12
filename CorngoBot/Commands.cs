﻿using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CorngoBot
{
    public class Commands : ModuleBase
    {
        //Call the FaceDetector constructor once
        private FaceDetector faceDetector = new FaceDetector();
        private readonly CommandService _service;

        public Commands(CommandService service)
        {
            _service = service;
        }

        /*[Command("hello")]
        public async Task HelloCommand()
        {
            // initialize empty string builder for reply
            var sb = new StringBuilder();

            // get user info from the Context
            var user = Context.User;

            // build out the reply
            sb.AppendLine($"You are -> [" + user.Username + "]");

            // send simple string reply
            await ReplyAsync(sb.ToString());
        }*/

        [Command("tomoko")]
        [Summary("Posts a random Tomoko")]
        public async Task Tomoko()
        {
            List<String> tomokoLinks = Program.tomokoLinks;

            var embedSettings = new EmbedBuilder();
            var color = randomColorHex();

            Random rnd = new Random();
            var rndImg = rnd.Next(0, tomokoLinks.Count);

            embedSettings.WithColor(color);
            embedSettings.ImageUrl = tomokoLinks[rndImg];

            var channel = Context.Channel;
            await channel.SendMessageAsync("", false, embedSettings.Build());

            /*Old code for reference
            String[] tomoko = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"\tomoko\");
            Random rnd = new Random();
            int rndImg = rnd.Next(0, tomoko.Length);

            var channel = Context.Channel;
            await channel.SendFileAsync(tomoko[rndImg]);
            */
        }

        [Command("vsauce")]
        [Summary("Posts a random VSauce meme")]
        public async Task VSauce()
        {
            List<String> vsauceLinks = Program.vsauceLinks;

            var embedSettings = new EmbedBuilder();
            var color = randomColorHex();

            Random rnd = new Random();
            var rndImg = rnd.Next(0, vsauceLinks.Count);

            embedSettings.WithColor(color);
            embedSettings.ImageUrl = vsauceLinks[rndImg];

            var channel = Context.Channel;
            await channel.SendMessageAsync("", false, embedSettings.Build());
        }

        /*[Command("example")]
        public async Task Example()
        {
            //await Context.Channel.SendMessageAsync("rrrr", true);

            String example = Program.tomokoLinks[2];

            var exampleEmbed = new EmbedBuilder();
            var color = randomColorHex();

            exampleEmbed.WithColor(color);
            exampleEmbed.WithTitle("Testing Title");

            exampleEmbed.ImageUrl = example;

            var channel = Context.Channel;
            await channel.SendMessageAsync(" ", false, exampleEmbed.Build());
            

            //await channel.SendMessageAsync(example, false, Discord.Embed);

            //await ReplyAsync(example);
        }*/

        [Command("face")]
        [Summary("Detects faces in an image")]
        public async Task Face(params string[] args)
        {

            var channel = Context.Channel;
            var message = Context.Message.Content;
            var outputFile = "./external_resources/facialOutput.jpg";

            if (args.Length < 1)
            {
                await ReplyAsync("```Sorry, you didn't use that command right. \n" +
                    "How to use command: <test [Image_URL]```");
            }
            else
            {
                String url = args[0];


                //Check if argument is a valid URL, then chek if it is a valid image
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    await channel.SendMessageAsync("Sorry, not a valid image URL");
                }
                else if (isImageUrl(url))
                {
                    int numFaces = faceDetector.detectFaces(url);
                    await channel.SendFileAsync(outputFile, "Detected **"+ numFaces + "** face(s)");
                }
                else
                {
                    await channel.SendMessageAsync("Sorry, not a valid image URL");
                }
            }
        }

        [Command("l")]
        [Summary("Hand someone an L")]
        public async Task L(params string[] args)
        {
            //Check if any parameters have been passed
            if (args.Length < 1)
            {
                await ReplyAsync("```Sorry, you didn't use that command right. \n" +
                   "How to use command: <L @user```");
            }
            else
            {
                //Check if there have been any mentioned users in the message
                var mentionedUsers = Context.Message.MentionedUserIds;
                int numUsersMentioned = 0;
                String message = "";

                //Build the message while cheking if any users were mentioned
                foreach(var userID in mentionedUsers)
                {
                    var user = Context.Client.GetUserAsync(userID);

                    message += "**" + user.Result.Username + "**, ";
                    numUsersMentioned += 1;
                }

                if(numUsersMentioned < 1)
                {
                    message = "```Sorry, you didn't use that command right. \n" +
                   "How to use command: <L @user```";
                }
                else if(numUsersMentioned > 1)
                {
                    message += "have ";
                }
                else
                {
                    message += "has ";
                }

                message += "been given an L by **" + Context.User.Username + "**";

                List<String> lLinks = Program.lLinks;

                var embedSettings = new EmbedBuilder();
                var color = randomColorHex();

                Random rnd = new Random();
                var rndImg = rnd.Next(0, lLinks.Count);

                embedSettings.WithColor(color);
                embedSettings.ImageUrl = lLinks[rndImg];
                embedSettings.WithDescription(message);

                var channel = Context.Channel;
                await channel.SendMessageAsync("", false, embedSettings.Build());
            }
        }

        [Command("congrats")]
        [Alias("congratulations")]
        [Summary("Congratulate someone")]
        public async Task Congrats(params string[] args)
        {
            var embedSettings = new EmbedBuilder();
            var color = randomColorHex();

            embedSettings.WithColor(color);
            embedSettings.ImageUrl = "https://i.imgur.com/6I5BfqZ.gif";

            //Check if any parameters have been passed
            if (args.Length == 0)
            {
                await ReplyAsync(null, false, embedSettings.Build());
            }
            else
            {
                //Check if there have been any mentioned users in the message
                var mentionedUsers = Context.Message.MentionedUserIds;
                int numUsersMentioned = 0;
                String message = "**" + Context.User.Username + "** has congradulated";

                //Build the message while cheking if any users were mentioned
                foreach (var userID in mentionedUsers)
                {
                    var user = Context.Client.GetUserAsync(userID);

                    message += ", **" + user.Result.Username + "**";
                    numUsersMentioned += 1;
                }

                if (numUsersMentioned < 1)
                {
                    message = "```Sorry, you didn't use that command right. \n" +
                   "How to use command: <Congrats @user```";
                }

                embedSettings.WithDescription(message);

                await ReplyAsync(null, false, embedSettings.Build());
            }
        }

        [Command("pathetic")]
        [Summary("Call someone pathetic")]
        public async Task Pathetic(params string[] args)
        {
            List<String> patheticLinks = Program.patheticLinks;

            Random rnd = new Random();
            var rndImg = rnd.Next(0, patheticLinks.Count);

            var embedSettings = new EmbedBuilder();
            var color = randomColorHex();

            embedSettings.WithColor(color);
            embedSettings.ImageUrl = patheticLinks[rndImg];

            //Check if any parameters have been passed
            if (args.Length < 1)
            {
                await ReplyAsync(null, false, embedSettings.Build());
            }
            else
            {
                //Check if there have been any mentioned users in the message
                var mentionedUsers = Context.Message.MentionedUserIds;
                int numUsersMentioned = 0;
                String message = "";

                //Build the message while cheking if any users were mentioned
                foreach (var userID in mentionedUsers)
                {
                    var user = Context.Client.GetUserAsync(userID);

                    message += "<@" + user.Result.Id + ">  ";
                    numUsersMentioned += 1;
                }

                if (numUsersMentioned == 0)
                {
                    await ReplyAsync(null, false, embedSettings.Build());
                }

                await ReplyAsync(message, false, embedSettings.Build());
            }
        }

        [Command("uhuh")]
        [Summary("Tell someone that sounds like their problem")]
        public async Task Uhuh(params string[] args)
        {
            string message = "That sounds like a ***you*** problem.";

            //Check if any parameters have been passed
            if (args.Length < 1)
            {
                await ReplyAsync(message);
            }
            else
            {
                //Check if there have been any mentioned users in the message
                var mentionedUsers = Context.Message.MentionedUserIds;

                //Build the message while cheking if any users were mentioned
                foreach (var userID in mentionedUsers)
                {
                    var user = Context.Client.GetUserAsync(userID);

                    message = message.Insert(0, "**" + user.Result.Username + "**, ");
                }

                await ReplyAsync(message);
            }
        }

        [Command("r")]
        [Alias("roll", "dice")]
        [Summary("Roll a dice")]
        public async Task Roll(params string[] args)
        {
            //Check if any parameters have been passed
            if (args.Length < 1)
            {
                await ReplyAsync("Sorry, you didn't use that command right. \n" +
                   "Command example: <roll 1d20");
            }
            else
            {
                var command = args[0];
                var regex = "^([1-9]|[1-9][0-9]|[1-9][0-9][0-9])[dD]{1}([1-9]|[1-9][0-9]|[1-9][0-9][0-9])$";
                var match = Regex.Match(command, regex);

                if(match.Success)
                {
                    var message = new StringBuilder("Rolling " + command + ": [");
                    var commandArr = command.ToLower().Split("d");

                    var rolls = int.Parse(commandArr[0]);
                    var dice = int.Parse(commandArr[1]);
                    var currRoll = 0;
                    var diceTotal = 0;

                    Random rnd = new Random();
                    // = rnd.Next(0, tomokoLinks.Count);

                    for (int i = 0; i < rolls; i++)
                    {
                        currRoll = rnd.Next(1, dice);

                        if (i < rolls - 1)
                        {
                            message.Append(currRoll + ", ");
                        }
                        else
                        {
                            message.Append(currRoll + "]```");
                        }

                        diceTotal += currRoll;
                    }

                    message.Insert(0, "```Total: " + diceTotal + "\n");

                    await ReplyAsync(message.ToString());
                }
                else
                {
                    await ReplyAsync("Sorry, you didn't use that command right. \n" +
                   "Command example: <r 1d20");
                }
            }
        }

        [Command("help")]
        public async Task Help(params string[] args)
        {
            var channel = Context.Channel;
            var commandList = _service.Commands;

            if (Context.User.Id.ToString() == "528853927471480833")
            {
                var embedSettings = new EmbedBuilder();
                var color = randomColorHex();

                embedSettings.WithColor(color);
                embedSettings.ImageUrl = "https://cdn.discordapp.com/attachments/673330432569376791/682786212167155747/JPEG_20200227_210834.jpg";
                embedSettings.WithTitle("L");

                await channel.SendMessageAsync("The only help you need is beating **Corngo** in a fighting game", false, embedSettings.Build());
            }
            else
            {
                StringBuilder message = new StringBuilder("```");
                /*StreamReader helpTxtFile = new StreamReader(@"./external_resources/helpMenu.txt");
                string currLine;

                while ((currLine = helpTxtFile.ReadLine()) != null)
                {
                    String[] currLineArr = currLine.Split("~");

                    String command = currLineArr[0];
                    String description = currLineArr[1];

                    message.AppendLine(String.Format("{0,-25}{1}", command, description));
                }

                helpTxtFile.Close();
                helpTxtFile.Dispose();

                message.AppendLine("```");*/

                foreach(var commands in commandList)
                {
                    message.AppendLine(String.Format("<{0,-18}{1}", commands.Name, commands.Summary));
                }

                message.Append("```");
                await channel.SendMessageAsync(message.ToString());
            }

        }

        //Generate a random color hex value
        public UInt32 randomColorHex()
        {
            //Generate random hex value for color
            var random = new Random();
            var colorStr = String.Format("0x{0:X6}", random.Next(0x1000000));
            var color = Convert.ToUInt32(colorStr, 16);

            return color;
        }

        //Check if URL is an image
        bool isImageUrl(string URL)
        {
            var req = (HttpWebRequest)HttpWebRequest.Create(URL);
            req.Method = "GET";
            try
            {
                using (var resp = req.GetResponse())
                {
                    return resp.ContentType.ToLower(CultureInfo.InvariantCulture)
                               .StartsWith("image/", StringComparison.OrdinalIgnoreCase);
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
