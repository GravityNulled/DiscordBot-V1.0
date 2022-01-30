using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
#pragma warning disable 1998

namespace XLR8_Bot
{
    internal class Program
    {
        private DiscordSocketClient Client;
        private CommandService Commands;

        public static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });
            Commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Debug,
                CaseSensitiveCommands = true,
                DefaultRunMode = RunMode.Async
            });
            Client.MessageReceived += Client_MessageReceived;
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            Client.Ready += Client_Ready;
            Client.Log += Client_Log;
            Client.UserJoined += Client_UserJoined;
            var token = "NTQ2NjIyMjMxMzM0MzU0OTU1.D0rIZQ.ff_zf24nx7H5y0CfkQsTeGjgnIY";
            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();

            await Task.Delay(-1);
        }


        private async Task Client_UserJoined(SocketGuildUser arg)
        {
            Console.WriteLine($"User {arg} has joined");
        }

        private async Task Client_Log(LogMessage msg)
        {
            Console.WriteLine($"{DateTime.Now} at {msg.Source} {msg.Message}");
        }

        private async Task Client_Ready()
        {
            await Client.SetGameAsync("Pushing Ranks", "https://www.youtube.com/watch?v=n6pCgzHMJY8",
                ActivityType.Listening);
        }

        private async Task Client_MessageReceived(SocketMessage MessageParam)
        {
            var Message = MessageParam as SocketUserMessage;
            var Context = new SocketCommandContext(Client, Message);
            if (Context.Message == null || Context.Message.Content == "") return;
            if (Context.User.IsBot) return;
            var ArgPos = 0;
            if (!Message.HasStringPrefix(".", ref ArgPos) ||
                Message.HasMentionPrefix(Client.CurrentUser, ref ArgPos)) return;

            var Results = await Commands.ExecuteAsync(Context, ArgPos, null);
            if (!Results.IsSuccess)
                Console.WriteLine(
                    $"{DateTime.Now} at Command Something Went Wrong: Text {Context.Message.Content} Error {Results.ErrorReason}");
        }
    }
}