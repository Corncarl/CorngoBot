using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CorngoBot
{
    public class Commands : ModuleBase
    {
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

        //Generate a random color hex value
        public UInt32 randomColorHex()
        {
            //Generate random hex value for color
            var random = new Random();
            var colorStr = String.Format("0x{0:X6}", random.Next(0x1000000));
            var color = Convert.ToUInt32(colorStr, 16);

            return color;
        }
    }
}
