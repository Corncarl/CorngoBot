using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace CorngoBot
{
    //https://www.gngrninja.com/code/2019/3/10/c-discord-bot-on-raspberry-pi-simple-bot-with-config-file
    class Program
    {
        public static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());



            return Task.CompletedTask;
        }

        private DiscordSocketClient _client;

        public async Task MainAsync()
        {
            // When working with events that have Cacheable<IMessage, ulong> parameters,
            // you must enable the message cache in your config settings if you plan to
            // use the cached message entity. 
            var _config = new DiscordSocketConfig { MessageCacheSize = 100 };
            _client = new DiscordSocketClient(_config);

            //  You can assign your bot token to a string, and pass that in to connect.
            //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
            var token = "MjY2Mjg5MTI4ODI2NjAxNDcy.Xlg1XQ.iT1r68sw9wwqAuoShiZBkdMIOZU";

            // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
            // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
            // var token = File.ReadAllText("token.txt");
            // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            _client.MessageUpdated += MessageUpdated;
            _client.Ready += () =>
            {
                Console.WriteLine("Bot is connected!");

                foreach (SocketGuild server in _client.Guilds)
                {
                    Console.WriteLine(server.Name + ", " + server.Id);
                }

                return Task.CompletedTask;
            };

            

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            // If the message was not in the cache, downloading it will result in getting a copy of `after`.
            var message = await before.GetOrDownloadAsync();
            Console.WriteLine($"{message} -> {after}");
        }
    }
}
