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
    public class StatsCommandHandler : ICommandHandler
    {
        private readonly IBraktApiClient _client;
        private readonly IResponseFormatter _formatter;

        public StatsCommandHandler(IBraktApiClient client, IResponseFormatter formatter)
        {
            _client = client;
            _formatter = formatter;
        }

        public string Command => "stats";

        public string HelpMessage
            => "Within the context of a discord server, this will return stats for a player in the context of the server. If sent via DM, stats will be returned for all servers where that player is a member.\n   * Arguments:\n     * #tag1 #tag2 ... #tagN - optional. If no tags specified, all stats for all tags will be shown.";

        public async Task ExecuteAsync(MessageCreateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            IEnumerable<Statistic> stats = null;

            if (userContext.IsGroupMemberContext)
            {
                stats = new List<Statistic>
                {
                    await _client.GetGroupStatisticsAsync(
                    userContext.GroupMember.PlayerId,
                    new GroupStatsRequest
                    {
                        GroupId = userContext.GroupMember.GroupId,
                        Tags = cmdToken.Tags.ToList()
                    },
                    cancellationToken)
                };
            }
            else if (userContext.IsPlayerContext)
            {
                stats = await _client.GetPlayerStatisticsAsync(userContext.Player.PlayerId, cancellationToken);
            }

            if (stats == null || !stats.Any())
            {
                await args.Message.RespondAsync("Sorry, I did not find any stats for you. Maybe try playing some games, idk.");
                return;
            }

            var statsDisplay = await _formatter.FormatStatsAsync(stats, cancellationToken);

            await args.Message.RespondAsync(statsDisplay);
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
