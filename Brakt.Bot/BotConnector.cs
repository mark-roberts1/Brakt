using Brakt.Bot.EventHandlers;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Bot
{
    public class BotConnector : BackgroundService
    {
        public static DiscordClient Client { get; private set; }
        private readonly IDiscordEventHandler _eventDelegates;

        public BotConnector(IOptions<DiscordConfig> configOptions, IDiscordEventHandler eventDelegates)
        {
            var config = new DiscordConfiguration
            {
                Token = configOptions.Value.Token,
                TokenType = configOptions.Value.TokenType
            };

            Client = new DiscordClient(config);
            _eventDelegates = eventDelegates;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Client.MessageCreated += _eventDelegates.HandleAsync;
            Client.GuildUpdated += _eventDelegates.HandleAsync;
            Client.GuildCreated += _eventDelegates.HandleAsync;
            Client.GuildBanRemoved += _eventDelegates.HandleAsync;
            Client.GuildBanAdded += _eventDelegates.HandleAsync;
            Client.GuildMemberRemoved += _eventDelegates.HandleAsync;
            Client.MessageReactionRemoved += _eventDelegates.HandleAsync;
            Client.MessageReactionAdded += _eventDelegates.HandleAsync;
            Client.GuildMembersChunked += _eventDelegates.HandleAsync;
            Client.UserUpdated += _eventDelegates.HandleAsync;
            Client.GuildMemberAdded += _eventDelegates.HandleAsync;
            Client.UserSettingsUpdated += _eventDelegates.HandleAsync;
            Client.MessageUpdated += _eventDelegates.HandleAsync;
            Client.GuildMemberUpdated += _eventDelegates.HandleAsync;

            await Client.ConnectAsync();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
                try { await Task.Delay(1, stoppingToken); } catch { }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Client.MessageCreated -= _eventDelegates.HandleAsync;
            Client.GuildUpdated -= _eventDelegates.HandleAsync;
            Client.GuildCreated -= _eventDelegates.HandleAsync;
            Client.GuildBanRemoved -= _eventDelegates.HandleAsync;
            Client.GuildBanAdded -= _eventDelegates.HandleAsync;
            Client.GuildMemberRemoved -= _eventDelegates.HandleAsync;
            Client.MessageReactionRemoved -= _eventDelegates.HandleAsync;
            Client.MessageReactionAdded -= _eventDelegates.HandleAsync;
            Client.GuildMembersChunked -= _eventDelegates.HandleAsync;
            Client.UserUpdated -= _eventDelegates.HandleAsync;
            Client.GuildMemberAdded -= _eventDelegates.HandleAsync;
            Client.UserSettingsUpdated -= _eventDelegates.HandleAsync;
            Client.MessageUpdated -= _eventDelegates.HandleAsync;
            Client.GuildMemberUpdated -= _eventDelegates.HandleAsync;

            await Client.DisconnectAsync();
        }
    }
}
