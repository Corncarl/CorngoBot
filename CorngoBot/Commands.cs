using Discord;
using Discord.Commands;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using DotNetOpenAuth.AspNet.Clients;
using System.Diagnostics;
using Discord.Audio;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Dropbox.Api;
using Dropbox;
using Dropbox.Api.Files;
using System.Threading;

namespace CorngoBot
{
    public class Commands : ModuleBase
    {
        //Call the FaceDetector constructor
        private FaceDetector faceDetector = new FaceDetector();
        private readonly CommandService _service;

        //Initiate imgur client
        private readonly ImgurClient _imgurClient; 
        private readonly AlbumEndpoint _imgurEndpoint;

        //Initiate Dropbox client
        private readonly DropboxClient _dropboxClient;

        public Commands(CommandService service)
        {
            //The Discord API service
            _service = service;

            //Setup the imgur client
            _imgurClient = new ImgurClient("xxxx");
            _imgurEndpoint = new AlbumEndpoint(_imgurClient);

            //Authorize Twitter API tokens
            Auth.SetUserCredentials("xxxx", "xxxx", "xxxx", "xxxx");

            //Setup the Dropbox client
            _dropboxClient = new DropboxClient("xxxx");
        }

        /*---------------------------------------------------------------------
        * All commands that send images using imgur
        --------------------------------------------------------------------*/
        [Command("dog")]
        [Summary("Posts a random dog")]
        public async Task Dog(params string[] args)
        {
            await ReplyImgur("vgW1p");
        }

        [Command("animal")]
        [Summary("Posts a random animal")]
        public async Task Animal(params string[] args)
        {
            await ReplyImgur("M3Gla");
        }

        [Command("Quote")]
        [Summary("Posts a motivational quote")]
        public async Task Quote(params string[] args)
        {
            await ReplyImgur("eJTLx");
        }

        [Command("boom")]
        [Summary("Posts unnecrssary explosion")]
        public async Task Boom(params string[] args)
        {
            await ReplyImgur("YL807FP");
        }

        [Command("ohno")]
        [Summary("Posts I am a genius, oh no image")]
        public async Task Ohno(params string[] args)
        {
            await ReplyImgur("GL32ZGg");
        }

        [Command("congrats")]
        [Alias("congratulations")]
        [Summary("Congratulate someone")]
        public async Task Congrats(params string[] args)
        {
            //Check if any parameters have been passed
            if (args.Length == 0)
            {
                await ReplyImgur("nK0zEGm");
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

                await ReplyImgur("nK0zEGm", message);
            }
        }

        [Command("X")]
        [Alias("Doubt")]
        [Summary("Doubt someone")]
        public async Task Doubt(params string[] args)
        {
            //Check if any parameters have been passed
            if (args.Length == 0)
            {
                await ReplyImgur("cIAg6");
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

                    message += "**" + user.Result.Username + "**, ";
                    numUsersMentioned += 1;
                }

                if (numUsersMentioned < 1)
                {
                    message = "```Sorry, you didn't use that command right. \n" +
                   "How to use command: <X @user```";
                }

                await ReplyImgur("cIAg6", message);
            }
        }





        /*---------------------------------------------------------------------
        * All commands that get tweets
        --------------------------------------------------------------------*/
        [Command("motivational")]
        [Alias("motivation", "motivational")]
        [Summary("Posts a tweet from @motivational")]
        public async Task Motivational(params String[] args)
        {
            await GetTweet("motivational");
        }

        [Command("facts")]
        [Alias("uberfacts")]
        [Summary("Posts a tweet from @UberFacts")]
        public async Task Facts(params String[] args)
        {
            await GetTweet("UberFacts");
        }

        [Command("ny")]
        [Alias("nytimes")]
        [Summary("Posts a tweet from @nytimes")]
        public async Task NYTimes(params String[] args)
        {
            await GetTweet("nytimes");
        }

        [Command("nintendo")]
        [Alias("ninten")]
        [Summary("Posts a tweet from @nintendo")]
        public async Task Nintendo(params String[] args)
        {
            await GetTweet("nintendo");
        }



        /*---------------------------------------------------------------------
         * All commands that have the bot join voice chat and play sound
         --------------------------------------------------------------------*/
        [Command("explosion", RunMode = RunMode.Async)]
        [Summary("Posts Megumin explosion sound")]
        public async Task Explosion(IVoiceChannel channel = null)
        {
            String[] audio = Context.Message.Content.Split(" ");
            audio[0] = audio[0].Replace("<", "").Trim();
            await SendAudio(audio[0]);
            //await SendAudio("external_resources\\explosion.mp3");
        }

        [Command("objection", RunMode = RunMode.Async)]
        [Alias("object")]
        [Summary("Posts objection sound")]
        public async Task Objection(IVoiceChannel channel = null)
        {
            String[] audio = Context.Message.Content.Split(" ");
            audio[0] = audio[0].Replace("<", "").Trim();
            await SendAudio(audio[0]);
            //await SendAudio("external_resources\\objection.mp3");
        }

        [Command("dial", RunMode = RunMode.Async)]
        [Alias("dialup")]
        [Summary("Make a dialup sound")]
        public async Task Dialup(IVoiceChannel channel = null)
        {
            String[] audio = Context.Message.Content.Split(" ");
            audio[0] = audio[0].Replace("<", "").Trim();
            await SendAudio(audio[0]);
            //await SendAudio("external_resources\\dial.mp3");
        }

        [Command("bonk", RunMode = RunMode.Async)]
        [Summary("Make bonk sound")]
        public async Task Bonk(IVoiceChannel channel = null)
        {
            String[] audio = Context.Message.Content.Split(" ");
            audio[0] = audio[0].Replace("<", "").Trim();
            await SendAudio(audio[0]);
        }

        [Command("sumo", RunMode = RunMode.Async)]
        [Summary("Make a sumo sound")]
        public async Task Sumo(IVoiceChannel channel = null)
        {
            String[] audio = Context.Message.Content.Split(" ");
            audio[0] = audio[0].Replace("<", "").Trim();
            await SendAudio(audio[0]);
        }

        [Command("cola", RunMode = RunMode.Async)]
        [Summary("Make's Rushia dying sound")]
        public async Task Cola(IVoiceChannel channel = null)
        {
            String[] audio = Context.Message.Content.Split(" ");
            audio[0] = audio[0].Replace("<", "").Trim();
            await SendAudio(audio[0]);
        }

        [Command("peko", RunMode = RunMode.Async)]
        [Summary("Makes peko sound")]
        public async Task Peko(IVoiceChannel channel = null)
        {
            String[] audio = Context.Message.Content.Split(" ");
            audio[0] = audio[0].Replace("<", "").Trim();
            await SendAudio(audio[0]);
        }

        [Command("haha", RunMode = RunMode.Async)]
        [Summary("Makes peko haha sound")]
        public async Task Haha(IVoiceChannel channel = null)
        {
            String[] audio = Context.Message.Content.Split(" ");
            audio[0] = audio[0].Replace("<", "").Trim();
            await SendAudio(audio[0]);
        }

        [Command("haato", RunMode = RunMode.Async)]
        [Summary("Haachamachamaaa")]
        public async Task Haato(IVoiceChannel channel = null)
        {
            String[] audio = Context.Message.Content.Split(" ");
            audio[0] = audio[0].Replace("<", "").Trim();
            await SendAudio(audio[0]);
        }

        



        /*---------------------------------------------------------------------
         * All misc commands
         --------------------------------------------------------------------*/
        [Command("sunny")]
        [Summary("Make a Sunny in Philidelphia meme")]
        public async Task SunnyMeme(params String[] args)
        {
            String file = "./external_resources/sunny.png";
            int width = 1920;
            int height = 1080;

            String msg = Context.Message.ToString().Replace("<sunny ", "");
            msg = "\"" + msg + "\"";

            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile("./external_resources/Textile Regular/Textile Regular.ttf");

            Bitmap bitmap = new Bitmap(
                width, height,
                PixelFormat.Format24bppRgb);

            RectangleF rectf = new RectangleF(0, 0, width, height);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                StringFormat sf = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawString(msg, new Font(pfc.Families[0], 78), Brushes.White, rectf, sf);


                bitmap.Save(file);
            } // graphics will be disposed at this line

            await Context.Channel.SendFileAsync("./external_resources/sunny.png");


        }

        [Command("queue")]
        [Alias("q")]
        [Summary("Displays current queue for what shows the discord members are watching")]
        public async Task WatchQueue(params string[] args)
        {
            //Path for the dropbox text files that will be read and written to
            String qPath = "/Queue/queue.txt";
            String finishedQPath = "/Queue/finishedQ.txt";

            //Initialize a dictionary that will hold the contents of the queue
            //Its key is the ID of users in the queue and its value is their queue list and username
            Dictionary<String, List<String>> queueList = new Dictionary<String, List<String>>();

            //Queue is written in a txt file stored in dropbox
            //Use the dropbox API to download the file
            using (var file = await _dropboxClient.Files.DownloadAsync(qPath))
            {
                //Convert the data into a stream and read the stream contents
                using (var fileStream = await file.GetContentAsStreamAsync())
                {
                    using (var streamReader = new StreamReader(fileStream))
                    {
                        String currLine = null;

                        //Read each line of the queue file and add its contents to the dictionary
                        while ((currLine = streamReader.ReadLine()) != null)
                        {
                            //Remove any whitespace from beginning and end of the current line
                            currLine = currLine.Trim();

                            //Skip reading if blank line
                            if (currLine == "")
                            {
                                continue;
                            }

                            //Using "|" as a delimiter, split the current line and add its contents to a list
                            //The second index of the list is the ID of a user
                            //Use the ID of the user as a key for the dictionary and use the list as the dictionary's value
                            List<String> watchList = currLine.Split("|").ToList();
                            queueList.Add(watchList.ElementAt(1), watchList);
                        }
                    }
                }  
            }

            //Make a copy of the dictionary to compare to the original and see if any changes were made to it
            Dictionary<String, List<String>> queueCopy = 
                new Dictionary<String, List<String>>(queueList.ToDictionary(k => k.Key, k => k.Value.ToList()));

            //queueOption will be the action the user wants to do with this command
            //queueMessage will display the final message to the user
            //userID is the ID of the user
            //userName is the username of the user
            StringBuilder queueMessage = new StringBuilder("");
            string queueOption = "";
            var userID = Context.Message.Author.Id.ToString();
            var userName = Context.Message.Author.Username;

            //User will use "|" as a delimiter when sending their message if they have additional arguments
            //Any string after index 0 will be shows or users they wish to remove or add
            //Then remove any leading or trailing whitespaces from the message array
            String[] commandMsg = Context.Message.ToString().Split("|");

            for (int i = 1; i < commandMsg.Length; i++)
            {
                commandMsg[i] = commandMsg[i].Trim();
            }

            //Check if any arguments haven been passed in
            //If not, set the queueOption to "list" 
            //If so, set the queueOption to what the user specified,
            if (args.Length < 1)
            {
                queueOption = "list";
            }
            else
            {
                queueOption = args[0].Trim();

                //Check to see if the queueOption is a mentioned user
                //If so, change the userID and userName to this mentioned user
                //Change the queueOption back to "list"
                //Mentioned user's string will start with a "<@" or a "<@!" and end with the ">" character
                if ((queueOption.Substring(0, 2).Equals("<@") || queueOption.Substring(0, 3).Equals("<@!")) 
                    && queueOption.Substring(queueOption.Length - 1).Equals(">"))
                {
                    userID = queueOption.Replace("<@!", "").Replace("<@", "").Replace(">", "");
                    userName = Context.Client.GetUserAsync(ulong.Parse(userID)).Result.Username;

                    queueOption = "list";
                }
            }

            //Check to see which option the user used for this command
            //"add": adds a show to the user's queue
            //"remove": removes a show from the user's queue
            //"list": displays the current user's queue
            //"addu": adds a new user to the queue
            //"removeu": removes a user from the queue
            //"finish": adds a show to, or displays a finished watch list
            //If none of these options are specified then only display the queue order
            switch (queueOption.ToLower())
            {
                case "add":
                    //Check to see if the user is in the queue
                    //If not, display an error saying they're not in the queue
                    if (queueList.ContainsKey(userID))
                    {
                        int addedShows = 0;

                        //Iterate through all the shows the user would like to add to their queue
                        //Ignore shows already in the queue
                        for (int i = 1; i < commandMsg.Length; i++)
                        {
                            if (commandMsg[i] != "")
                            {
                                //Check if the show is already in their queue, ignoring case
                                if (! queueList[userID].Contains(commandMsg[i], StringComparer.InvariantCultureIgnoreCase))
                                {
                                    queueList[userID].Add(commandMsg[i]);

                                    addedShows++;
                                    queueMessage.Append("*" + commandMsg[i] + "*, ");
                                }
                            }
                        }

                        //Check to see if any shows were added and change the message accordingly
                        if(addedShows > 0)
                        {
                            queueMessage.Append((addedShows > 1) ? "have " : "has ");
                            queueMessage.Append("been added for **" + userName + "**");
                        }
                        else
                        {
                            queueMessage.Append("Sorry, didn't add any shows");
                        }
                    }
                    else
                    {
                        queueMessage.Append("Sorry, you aren't in the queue");
                    }
                    break;



                case "remove":
                    //Check to see if the user is in the queue
                    //If not, display an error saying they're not in the queue
                    if (queueList.ContainsKey(userID))
                    {
                        int removedShows = 0;

                        //Iterate through all the shows the user would like to remove from their queue
                        //Ignore shows that aren't in the queue
                        for (int i = 1; i < commandMsg.Length; i++)
                        {
                            if (commandMsg[i] != "")
                            {
                                //Check if the show exists in their queue, ignoring case
                                if (queueList[userID].Contains(commandMsg[i], StringComparer.InvariantCultureIgnoreCase))
                                {
                                    var removedIndex = queueList[userID].FindIndex(a => a.Equals(commandMsg[i], StringComparison.OrdinalIgnoreCase));


                                    queueList[userID].RemoveAt(removedIndex);

                                    removedShows++;
                                    queueMessage.Append("*" + commandMsg[i] + "*, ");
                                }
                            }
                        }

                        //Check to see if any shows were removed and change the message accordingly
                        if (removedShows > 0)
                        {
                            queueMessage.Append((removedShows > 1) ? "have " : "has ");
                            queueMessage.Append("been removed for **" + userName + "**");
                        }
                        else
                        {
                            queueMessage.Append("Sorry, didn't remove any shows");
                        }
                    }
                    else
                    {
                        queueMessage.Append("Sorry, you aren't in the queue");
                    }
                    break;



                case "list":
                    //Check to see if the user is in the queue
                    //If not, display an error saying they're not in the queue
                    if (queueList.ContainsKey(userID))
                    {
                        //Check to see if the user has any shows in their queue and display message appropriately
                        if (queueList[userID].Count < 3)
                        {
                            queueMessage.Append(userName + " does not have any shows in their queue");
                        }
                        else
                        {
                            queueMessage.Append("**" + userName + "'s** queue list:\n");

                            for(int i = 2; i < queueList[userID].Count; i++)
                            {
                                queueMessage.Append("  - " + queueList[userID].ElementAt(i) + "\n");
                            }
                        }
                    }
                    else
                    {
                        queueMessage.Append("Sorry, **" + userName + "** isn't in the queue");
                    }
                    break;



                case "addu":
                    //Only let Corngo use this command
                    if (Context.Message.Author.Id.ToString() == "153339037225058304")
                    {
                        //Check to see if any users were mentioned in this message
                        //If they weren't, then display error message and break
                        var mentions = Context.Message.MentionedUserIds;
                        if (mentions.Count == 0)
                        {
                            queueMessage.Append("Sorry, you have to mention a user to add them");
                            break;
                        }

                        //Get the ID and username of the mentioned user
                        //If multiple users were mentioned, only use the first mentioned user
                        userID = mentions.ElementAt(0).ToString();
                        userName = Context.Client.GetUserAsync(ulong.Parse(userID)).Result.Username;

                        //Check if this ID already exists in the dictionary
                        //If so, display an error message and break, if not add them
                        if (queueList.ContainsKey(userID))
                        {
                            queueMessage.Append("Sorry, **" + userName + "** is already in the queue");
                            break;
                        }

                        //Create a new list with this user's information
                        List<string> newList = new List<string>();
                        newList.Add(userName);
                        newList.Add(userID);

                        //Check to see if the user wanted to add any shows while they were being added to the queue
                        if (commandMsg.Length >= 2)
                        {
                            for (int i = 1; i < commandMsg.Length; i++)
                            {
                                if (commandMsg[i] != "")
                                {
                                    newList.Add(commandMsg[i]);
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }

                        //Add this user to the dictionary using their ID as a key
                        queueList.Add(userID, newList);
                        queueMessage.Insert(0, "**" + userName + "** has been succesfully addded to the queue");
                    }
                    else
                    {
                        await ReplyAsync("Sorry, you don't have permission to do that");
                        return;
                    }
                    break;



                case "removeu":
                    //Only let Corngo use this command
                    if (Context.Message.Author.Id.ToString() == "153339037225058304")
                    {
                        //Check to see if any users were mentioned in this message
                        //If they weren't, then display error message and break
                        var mentions = Context.Message.MentionedUserIds;
                        if (mentions.Count == 0)
                        {
                            queueMessage.Append("Sorry, you have to mention a user to remove them");
                            break;
                        }

                        //Get the ID and username of the mentioned user
                        //If multiple users were mentioned, only use the first mentioned user
                        userID = mentions.ElementAt(0).ToString();
                        userName = Context.Client.GetUserAsync(ulong.Parse(userID)).Result.Username;

                        //Check if this ID exists in the dictionary
                        //If not, display an error message and break, if not add them
                        if (! queueList.ContainsKey(userID))
                        {
                            queueMessage.Append("Sorry, **" + userName + "** wasn't in the queue");
                            break;
                        }

                        queueList.Remove(userID);
                        queueMessage.Append("**" + userName + "** has been removed from the queue");
                    }
                    else
                    {
                        await ReplyAsync("Sorry, you don't have permission to do that");
                        return;
                    }
                    break;


                case "finish":
                    //Initialize a list for the finished shows
                    List<String> finishedList = new List<string>();

                    //The finished Queue list is stored in Dropbox
                    //Use Dropbox API to read in the contents of the file
                    using (var finishedQFile = await _dropboxClient.Files.DownloadAsync(finishedQPath))
                    {
                        using(var finishedQStream = await finishedQFile.GetContentAsStreamAsync())
                        {
                            using(var finishedQReader = new StreamReader(finishedQStream))
                            {
                                String currLine = null;

                                //Read each line of the finished watched file and add its contents to a list
                                while ((currLine = finishedQReader.ReadLine()) != null)
                                {
                                    //Remove any whitespace from beginning and end of the current line
                                    currLine = currLine.Trim();

                                    //Skip reading if blank line
                                    if (currLine == "")
                                    {
                                        continue;
                                    }

                                    finishedList.Add(currLine);
                                }
                            }
                        }
                    }

                    //Make a copy of the finished watched list to compare and see if any changes were made to it
                    List<String> copyFinishedList = new List<string>(finishedList.ToList());

                    //Iterate through all the shows the user would like to add to the finished list
                    //Ignore shows already in the queue
                    int fininshedShows = 0;

                    for (int i = 1; i < commandMsg.Length; i++)
                    {
                        if (commandMsg[i] != "")
                        {
                            //Check if the show is already in their queue, ignoring case
                            if (! finishedList.Contains(commandMsg[i]))
                            {
                                finishedList.Add(commandMsg[i]);

                                fininshedShows++;
                                queueMessage.Append("*" + commandMsg[i] + "*, ");
                            }
                        }
                    }

                    //Check to see if any shows were added and change the message accordingly
                    if (fininshedShows > 0)
                    {
                        queueMessage.Append((fininshedShows > 1) ? "have " : "has ");
                        queueMessage.Append("been added to the watched list");
                    }

                    //Check to see if any changes were made to the list and save it
                    if (! finishedList.SequenceEqual(copyFinishedList))
                    {
                        //Order the watched list alphabetically
                        finishedList.Sort();

                        //Add every finished show in a single string, using "|" as a delimiter
                        var finishedString = "";

                        foreach(var list in finishedList)
                        {
                            finishedString += list + "\n";
                        }

                        using(var finishedQWriter = new MemoryStream(UTF8Encoding.UTF8.GetBytes(finishedString)))
                        {
                            var response = await _dropboxClient.Files.UploadAsync(finishedQPath, 
                                WriteMode.Overwrite.Instance, 
                                body: finishedQWriter);
                        }
                    }

                    //Append the finished list to the queueMessage
                    queueMessage.Append("\n**Current finished list:**\n");

                    foreach(var watched in finishedList)
                    {
                        queueMessage.Append("  - " + watched + "\n");
                    }

                    break;
            }

            //Build the final queue order string
            String tempStr = "";
            foreach (var queueUser in queueList)
            {
                tempStr += " > **" + queueUser.Value.ElementAt(0) + "**";
            }

            queueMessage.Append("\nQueue order: " + tempStr.Substring(3));

            //Compare the dictionary with a copy to see if any changes were made to it
            //If so, save those changes in the txt file
            if(!queueList.SequenceEqual(queueCopy))
            {
                //Sort the values dictionary alphabetically, excluding the first two values
                //This will display the shows in alphabetical order
                foreach(var queue in queueList.ToList())
                {
                    queueList[queue.Value.ElementAt(1)] = queue.Value.OrderBy(q => q != queue.Value.ElementAt(0))
                        .ThenBy(q => q != queue.Value.ElementAt(1))
                        .ThenBy(q => q).ToList();
                }

                //Write the new dictionary into the txt file
                String streamMessage = "";

                foreach (var queue in queueList)
                {
                    String temp = "";

                    foreach (var list in queue.Value)
                    {
                        temp += "|" + list.Trim();
                    }

                    //Use Substring 1 so the delimiter "|" is removed from the beginning of each line
                    streamMessage += temp.Substring(1) + "\n";
                }

                using (var queueWriter = new MemoryStream(UTF8Encoding.UTF8.GetBytes(streamMessage)))
                {
                    var response = await _dropboxClient.Files.UploadAsync(qPath,
                        WriteMode.Overwrite.Instance,
                        body: queueWriter);
                }
            }

            await ReplyAsync(queueMessage.ToString());
        }


        [Command("uinfo")]
        [Summary("Display information of a discord user")]
        public async Task UserInfo(params string[] args)
        {
            Discord.IUser user = null;

            //Check to see if any users were mentioned in the message
            //If multiple users were mentioned, only use the first mentioned user
            //If no users were mentioned then use the author of the message
            var mentionedUsers = Context.Message.MentionedUserIds.ToList();
            if(mentionedUsers.Count == 0)
            {
                user = Context.User;
            }
            else
            {
                var mentionedUserID = mentionedUsers.ElementAt(0);
                user = Context.Client.GetUserAsync(mentionedUserID).Result;
            }

            //General user properties
            var activ = user.Activity;
            var usrAccMade = user.CreatedAt;
            var usrID = user.Id;
            var usrAvi = user.GetAvatarUrl();
            var disc = user.Discriminator;
            var isBot = user.IsBot;
            var status = user.Status;
            var usrName = user.Username;
            string statusSymbol;

            //Server properties regarding the specified user
            var userGuild = Context.Guild.GetUserAsync(usrID).Result;
            var userJoined = userGuild.JoinedAt;

            //Get list of users in server and sort it based on when they joined
            //Get the index in which the specified user joined the server
            var guildUsers = Context.Guild.GetUsersAsync().Result.OrderBy(s => s.JoinedAt).ToList();
            int index = guildUsers.IndexOf(userGuild);

            //Set index to start with the 3 previous users that joined the server
            //If this is not possible, start index at 0
            String joinOrder = "";
            index -= 3;

            if(index < 0)
            {
                index = 0;
            }    
            
            //Start building the string at this index
            //If the specified user is in this index then highlight their username
            if(guildUsers.ElementAt(index).Equals(userGuild))
            {
                joinOrder += "**" + guildUsers.ElementAt(index).Username + "**";
            }
            else
            {

                joinOrder += guildUsers.ElementAt(index).Username;
            }

            //String will only display a maximum of 7 users
            //Iterate through the remaining users to build the string
            //Highlight specified user when found
            for(int i = index + 1; i < index + 7; i++)
            {
                if(i >= guildUsers.Count)
                {
                    break;
                }

                if (guildUsers.ElementAt(i).Equals(userGuild))
                {
                    joinOrder += " > **" + guildUsers.ElementAt(i).Username + "**";
                }
                else
                {
                    joinOrder += " > " + guildUsers.ElementAt(i).Username + "";
                }
            }

            //Get a list of roles the specified user has in this server
            var usrRoles = userGuild.RoleIds;
            String roles = "";

            //Iterate through the list and build string with the roles the user has
            //Ignore the universal "@everyone" role.
            foreach(var role in usrRoles)
            {
                var r = Context.Guild.GetRole(role);

                if(r.Name.ToUpper() != "@EVERYONE")
                {
                    roles += ", " + r.Name;
                }
            }

            //If user had roles, then remove the first 2 characters in the string to remove the starting ", " from the string
            //If user had no roles, then use a default string
            if (roles.Equals(""))
            {
                roles = "None";
            }
            else
            {
                roles = roles.Substring(2);
            }


            //Set the status symbol based on the user's current activity
            switch (status.ToString().ToUpper()){
                case "ONLINE":
                    statusSymbol = "\uD83D\uDFE2 ";
                    break;
                case "IDLE":
                    statusSymbol = "\uD83D\uDFE1 ";
                    break;
                case "DONOTDISTURB":
                    statusSymbol = "\uD83D\uDD34 ";
                    break;
                default:
                    statusSymbol = "\u26AA ";
                    break;
            }

            //If user is a bot, then use a bot emoji, otherwise use a silhouette emoji
            var message = (isBot ? "\uD83E\uDD16" : "\uD83D\uDC64") + " Information about **" + user.Username + "**#" + disc;

            var description = "\u25AB User ID: **" + usrID + "**\n"
                + "\u25AB Status: " + statusSymbol + " **" + status + "** " + (activ != null ? "(Playing *" + activ + "*)" : "") + "\n"
                + "\u25AB Roles: **" + roles + "** \n"
                + "\u25AB Account Creation: **" + usrAccMade.DateTime.ToString("r", CultureInfo.CreateSpecificCulture("en-US")) + "** \n"
                + "\u25AB Server Join: **" + userJoined.Value.ToString("r", CultureInfo.CreateSpecificCulture("en-US")) + "** \n"
                + "\u25AB Join Order: " + joinOrder + "\n";

            var embedSettings = new EmbedBuilder();
            embedSettings.Description = description;
            embedSettings.ThumbnailUrl = usrAvi;

            await ReplyAsync(message, false, embedSettings.Build());
        }

        //Only useable by the server owner
        [RequireOwner]
        [Command("msg")]
        [Summary("Have the bot send a custom message")]
        public async Task Message(params string[] args)
        {
            var guilds = Context.Guild.GetTextChannelsAsync().Result;

            foreach (var guild in guilds)
            {
                if(guild.Name == "general")
                {
                    var userMsg = Context.Message.Content;
                    userMsg = userMsg.Replace("<msg", "").Replace("<message", "");

                    await guild.SendMessageAsync("```" + userMsg + "```", false);
                }
            }
        }


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
                    await channel.SendFileAsync(outputFile, "Detected **" + numFaces + "** face(s)");
                }
                else
                {
                    await channel.SendMessageAsync("Sorry, not a valid image URL");
                }
            }
        }

        [Command("cheer")]
        [Summary("Cheer someone up")]
        public async Task Cheer(params string[] args)
        {
            string message = "You got this. You can do it!";

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

                if (match.Success)
                {
                    var message = new StringBuilder("Rolling " + command + ": [");
                    var commandArr = command.ToLower().Split("d");

                    var rolls = int.Parse(commandArr[0]);
                    var dice = int.Parse(commandArr[1]);
                    var currRoll = 0;
                    var diceTotal = 0;

                    Random rnd = new Random();

                    for (int i = 0; i < rolls; i++)
                    {
                        currRoll = rnd.Next(1, dice + 1);

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
        [Summary("Shows all available commands")]
        public async Task Help(params string[] args)
        {
            //Get the list of commands in the server and then sort them
            var commandList = _service.Commands;
            commandList = commandList.OrderBy(s => s.Name);

            StringBuilder message = new StringBuilder("```");

            foreach (var commands in commandList)
            {
                if (commands.Name == "msg")
                {
                    continue;
                }
                message.AppendLine(String.Format("<{0,-18}{1}", commands.Name, commands.Summary));

                //Check to see if the help message has exceeded Discord's character limit of 2000
                //If so, send what we have so far and begin building the resy pf the message
                //Use a size of 1800 to ensure we haven't exceeded the limit
                if (message.Length >= 1800)
                {
                    message.Append("```");
                    await ReplyAsync(message.ToString());
                    message.Clear();
                    message.Append("```");
                }
            }

            message.Append("```");
            await ReplyAsync(message.ToString());
        }





        /*---------------------------------------------------------------------
         * All non-command functions
         --------------------------------------------------------------------*/
        
        //Reply to the user with a tweet from a specified user
        //Used by commands that send twitter links
        public async Task GetTweet(String userId)
        {
            var lastTweets = Timeline.GetUserTimeline(userId, 200).ToArray();

            var allTweets = new List<ITweet>(lastTweets);

            //Remove any tweets that were replies
            foreach(var tweet in allTweets.ToList())
            {
                if(tweet.InReplyToScreenName != null)
                    allTweets.Remove(tweet);
            }

            //Pick a random tweet from the list to send
            Random rnd = new Random();
            var rndTweet = rnd.Next(0, allTweets.Count());

            var tweetLink = allTweets[rndTweet].Url;

            await ReplyAsync(tweetLink);
        }

        //Reply to user with an imgur image
        //Used by commands that grab images from imgur
        public async Task ReplyImgur(String albumID, String message = null)
        {
            var images = await _imgurEndpoint.GetAlbumImagesAsync(albumID);

            var embedSettings = new EmbedBuilder();
            var color = randomColorHex();

            Random rnd = new Random();
            var rndImg = rnd.Next(0, images.Count());

            embedSettings.WithColor(color);
            embedSettings.ImageUrl = images.ElementAt(rndImg).Link;

            await ReplyAsync(message, false, embedSettings.Build());
        }

        //Connects to a user's voice channel and streams an audio file
        //Used by commands that have the bot join a channel and play audio
        public async Task SendAudio(String audioPath, IVoiceChannel channel = null)
        {
            try
            {
                var file = await _dropboxClient.Files.GetTemporaryLinkAsync("/Audio/" + audioPath + ".mp3");
                audioPath = file.Link;
            }
            catch(Exception e)
            {
                await ReplyAsync("Sorry there was a problem. Tell the dev to do a better job\n" + e.Message);
                return;
            }

            // Get the audio channel
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync("User must be in a voice channel."); return; }

            // For the next step with transmitting audio, you would want to pass this Audio Client in to a service.
            var audioClient = await channel.ConnectAsync();

            Task t = SendAsync(audioClient, audioPath);

            //Wait for audio to finish playing and disconnect from channel
            t.Wait();

            await channel.DisconnectAsync();
        }

        //Uses ffmpeg to stream the audio
        //Used by commands that have the bot join a channel and play audio
        private Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
        }

        //Send the audio
        //Used by commands that have the bot join a channel and play audio
        private async Task SendAsync(IAudioClient client, string path)
        {
            // Create FFmpeg using the previous example
            using (var ffmpeg = CreateStream(path))
            using (var output = ffmpeg.StandardOutput.BaseStream)
            using (var discord = client.CreatePCMStream(AudioApplication.Mixed))
            {
                try { await output.CopyToAsync(discord); }
                finally { await discord.FlushAsync(); }
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
