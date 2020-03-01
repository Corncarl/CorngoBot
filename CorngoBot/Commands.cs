using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CorngoBot
{
    public class Commands : ModuleBase
    {
        //Call the FaceDetector constructor once
        private FaceDetector faceDetector = new FaceDetector();


        [Command("hello")]
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
        }

        [Command("tomoko")]
        public async Task Tomoko()
        {
            String[] tomoko = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"\tomoko\");
            Random rnd = new Random();
            int rndImg = rnd.Next(0, tomoko.Length);

            var channel = Context.Channel;
            await channel.SendFileAsync(tomoko[rndImg]);

            //var guild = Context.Guild.//_client.Guilds.Single(g => g.Name == "guild name");
            //var channel = guild.TextChannels.Single(ch => ch.Name == "channel name");
            
            //await channel.SendFileAsync("D:\\Corngo\\Pictures\\IMG_3186.png", "Caption goes here");
        }

        [Command("vsauce")]
        public async Task VSauce()
        {
            String[] vsauce = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"\vsauce\");
            Random rnd = new Random();
            int rndImg = rnd.Next(0, vsauce.Length);

            var channel = Context.Channel;
            await channel.SendFileAsync(vsauce[rndImg]);
        }

        [Command("example")]
        public async Task Example()
        {
            //await Context.Channel.SendMessageAsync("rrrr", true);

            String example = Program.exampleStrings[2];

            var exampleEmbed = new EmbedBuilder();
            var color = randomColorHex();

            exampleEmbed.WithColor(color);
            exampleEmbed.WithTitle("Testing Title");

            exampleEmbed.ImageUrl = example;

            var channel = Context.Channel;
            await channel.SendMessageAsync(" ", false, exampleEmbed.Build());
            

            //await channel.SendMessageAsync(example, false, Discord.Embed);

            //await ReplyAsync(example);
        }

        [Command("test")]
        public async Task Test(params string[] args)
        {

            var channel = Context.Channel;
            var message = Context.Message.Content;

            if(args.Length < 1)
            {
                await channel.SendMessageAsync("Sorry, you didn't use that command right. \n " +
                    "How to use command: **!test \"Image_URL\"**");
            }
            else
            {
                String url = args[0];

                if (isImageUrl(url))
                {
                    String outputFile = faceDetector.detectFaces(url);
                    await channel.SendFileAsync(outputFile);
                }
                else
                {
                    await channel.SendMessageAsync("Sorry, not a valid image URL");
                }
            }


            //String faces = faceDetector.detectFaces

            
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
            req.Method = "HEAD";
            using (var resp = req.GetResponse())
            {
                return resp.ContentType.ToLower(CultureInfo.InvariantCulture)
                           .StartsWith("image/");
            }
        }
    }
}
