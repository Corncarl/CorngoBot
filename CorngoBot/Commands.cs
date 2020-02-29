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

        [Command("jose")]
        public async Task Jose()
        {
            await Context.Channel.SendMessageAsync("fuck Jose", true);
        }
    }
}
