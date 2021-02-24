using Brakt.Bot.Formatters;
using Brakt.Bot.Identification;
using Brakt.Bot.Interpretor;
using Brakt.Client;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Bot.Commands
{
    public class LeaderboardCommandHandler : ICommandHandler
    {
        private readonly IBraktApiClient _client;
        private readonly IResponseFormatter _formatter;

        public LeaderboardCommandHandler(IBraktApiClient client, IResponseFormatter formatter)
        {
            _client = client;
            _formatter = formatter;
        }

        public string Command => "leaderboard";

        public async Task ExecuteAsync(MessageCreateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            if (!userContext.IsGroupContext)
            {
                await args.Message.RespondAsync("This command is only available in the context of a server. Did you mean to use ```brakt stats```?");
            }

            var stats = await _client.GetGroupStatisticsAsync(new GroupStatsRequest
            {
                GroupId = userContext.Group.GroupId,
                Tags = cmdToken.Tags.ToList()
            }, cancellationToken);

            if (stats == null || !stats.Any())
            {
                await args.Message.RespondAsync("There have been no tournaments on this server, yet. To initiate your first tournament, use ```brakt create```");
                return;
            }

            var rankedStats = stats.OrderByDescending(ob => ob.Wins).ThenBy(tb => tb.Losses);

            var lbDisplay = await _formatter.FormatAsLeaderboardAsync(stats, cancellationToken);

            await args.Message.RespondAsync(lbDisplay);
        }

        public Task ExecuteAsync(MessageReactionRemoveEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task ExecuteAsync(MessageReactionAddEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task ExecuteAsync(MessageUpdateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
