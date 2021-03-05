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
    public class LeaderboardCommandHandler : CommandHandlerBase, ICommandHandler
    {
        public LeaderboardCommandHandler(IBraktApiClient client, IResponseFormatter formatter) : base(client, formatter)
        {
        }

        public string Command => "leaderboard";

        public string HelpMessage 
            => "Within the context of a discord server, this will show you the leaderboard for tournaments held on the server.\n   * Arguments:\n     * #tag1 #tag2 ... #tagN - optional. If no tags specified, all stats for all tags will be shown.";

        public override async Task ExecuteAsync(MessageCreateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            if (!userContext.IsGroupContext)
            {
                await args.Message.RespondAsync("This command is only available in the context of a server. Did you mean to use ```brakt stats```?");
            }

            var stats = await Client.GetGroupStatisticsAsync(new GroupStatsRequest
            {
                GroupId = userContext.Group.GroupId,
                Tags = cmdToken.Tags.ToList()
            }, cancellationToken);

            if (stats == null || !stats.Any())
            {
                await args.Message.RespondAsync("There have been no tournaments on this server, yet. To initiate your first tournament, use ```brakt create```");
                return;
            }

            var rankedStats = stats.OrderByDescending(ob => ob.Wins).ThenBy(tb => tb.Losses).ThenByDescending(tbd => tbd.TournamentWins);

            var lbDisplay = await Formatter.FormatAsLeaderboardAsync(stats, cancellationToken);

            await args.Message.RespondAsync(lbDisplay);
        }
    }
}
