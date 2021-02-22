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
        private readonly DiscordClient client;
        private readonly IDiscordEventHandler _eventDelegates;

        public BotConnector(IOptions<DiscordConfig> configOptions, IDiscordEventHandler eventDelegates)
        {
            var config = new DiscordConfiguration
            {
                Token = configOptions.Value.Token,
                TokenType = configOptions.Value.TokenType
            };

            client = new DiscordClient(config);
            _eventDelegates = eventDelegates;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            client.MessageCreated += _eventDelegates.HandleAsync;
            client.GuildUpdated += _eventDelegates.HandleAsync;
            client.GuildCreated += _eventDelegates.HandleAsync;
            client.GuildBanRemoved += _eventDelegates.HandleAsync;
            client.GuildBanAdded += _eventDelegates.HandleAsync;
            client.GuildMemberRemoved += _eventDelegates.HandleAsync;
            client.MessageReactionRemoved += _eventDelegates.HandleAsync;
            client.MessageReactionAdded += _eventDelegates.HandleAsync;
            client.GuildMembersChunked += _eventDelegates.HandleAsync;
            client.UserUpdated += _eventDelegates.HandleAsync;
            client.GuildMemberAdded += _eventDelegates.HandleAsync;
            client.UserSettingsUpdated += _eventDelegates.HandleAsync;
            client.MessageUpdated += _eventDelegates.HandleAsync;
            client.GuildMemberUpdated += _eventDelegates.HandleAsync;

            await client.ConnectAsync();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
                try { await Task.Delay(1, stoppingToken); } catch { }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            client.MessageCreated -= _eventDelegates.HandleAsync;
            client.GuildUpdated -= _eventDelegates.HandleAsync;
            client.GuildCreated -= _eventDelegates.HandleAsync;
            client.GuildBanRemoved -= _eventDelegates.HandleAsync;
            client.GuildBanAdded -= _eventDelegates.HandleAsync;
            client.GuildMemberRemoved -= _eventDelegates.HandleAsync;
            client.MessageReactionRemoved -= _eventDelegates.HandleAsync;
            client.MessageReactionAdded -= _eventDelegates.HandleAsync;
            client.GuildMembersChunked -= _eventDelegates.HandleAsync;
            client.UserUpdated -= _eventDelegates.HandleAsync;
            client.GuildMemberAdded -= _eventDelegates.HandleAsync;
            client.UserSettingsUpdated -= _eventDelegates.HandleAsync;
            client.MessageUpdated -= _eventDelegates.HandleAsync;
            client.GuildMemberUpdated -= _eventDelegates.HandleAsync;

            await client.DisconnectAsync();
        }
    }
}
