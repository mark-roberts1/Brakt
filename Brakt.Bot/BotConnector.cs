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

        public BotConnector(IOptions<DiscordConfig> configOptions)
        {
            var config = new DiscordConfiguration
            {
                Token = configOptions.Value.Token,
                TokenType = configOptions.Value.TokenType
            };

            client = new DiscordClient(config);
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            client.MessageCreated += MessageCreated;

            await client.ConnectAsync();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
                try { await Task.Delay(1, stoppingToken); } catch { }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            client.MessageCreated -= MessageCreated;
            await client.DisconnectAsync();
        }

        private async Task MessageCreated(MessageCreateEventArgs e)
        {
            await Task.CompletedTask;
        }
    }
}
