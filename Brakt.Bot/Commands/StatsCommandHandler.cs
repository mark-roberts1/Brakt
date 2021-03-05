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
    public class StatsCommandHandler : CommandHandlerBase, ICommandHandler
    {
        public StatsCommandHandler(IBraktApiClient client, IResponseFormatter formatter) : base(client, formatter)
        {
        }

        public string Command => "stats";

        public string HelpMessage
            => "Within the context of a discord server, this will return stats for a player in the context of the server. If sent via DM, stats will be returned for all servers where that player is a member.\n   * Arguments:\n     * #tag1 #tag2 ... #tagN - optional. If no tags specified, all stats for all tags will be shown.";

        public override async Task ExecuteAsync(MessageCreateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            IEnumerable<Statistic> stats = null;

            if (userContext.IsGroupMemberContext)
            {
                stats = new List<Statistic>
                {
                    await Client.GetGroupStatisticsAsync(
                    userContext.GroupMember.PlayerId,
                    new GroupStatsRequest
                    {
                        GroupId = userContext.GroupMember.GroupId,
                        Tags = cmdToken.Tags.ToList()
                    },
                    cancellationToken)
                };
            }
            else if (userContext.IsPlayerContext && !cmdToken.Tags.Any())
            {
                stats = await Client.GetPlayerStatisticsAsync(userContext.Player.PlayerId, cancellationToken);
            }
            else if (userContext.IsPlayerContext)
            {
                stats = await Client.GetPlayerStatisticsAsync(new PlayerStatsRequest { PlayerId = userContext.Player.PlayerId, Tags = cmdToken.Tags.ToList() }, cancellationToken);
            }

            if (stats == null || !stats.Any())
            {
                await args.Message.RespondAsync("Sorry, I did not find any stats for you. Maybe try playing some games, idk.");
                return;
            }

            var statsDisplay = await Formatter.FormatStatsAsync(stats, cancellationToken);

            await args.Message.RespondAsync(statsDisplay);
        }
    }
}
