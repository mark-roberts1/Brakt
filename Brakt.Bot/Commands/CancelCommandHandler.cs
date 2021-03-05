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
    public class CancelCommandHandler : CommandHandlerBase, ICommandHandler
    {
        public CancelCommandHandler(IBraktApiClient client, IResponseFormatter formatter) : base(client, formatter)
        {
        }

        public string Command => "cancel";

        public string HelpMessage
            => "Cancel a tournament. If underway, stats will be cleared for the given tournament.\n   * Arguments:\n     * [tournament id] - an integer id given when a tournament is generated. This can be found with the list command if it has been forgotten.";

        public override async Task ExecuteAsync(MessageCreateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            AssertGroupMemberContext(userContext);
            AssertUserIsAdmin(userContext.GroupMember);

            if (!TryGetTournamentId(cmdToken.Arguments, out int tournamentId))
            {
                await args.Message.RespondAsync("TournamentId argument required.");
                return;
            }

            var tournament = await Client.GetTournamentAsync(tournamentId, cancellationToken);

            AssertTournamentExists(tournament);
            AssertTournamentBelongsToGroup(tournament, userContext.GroupMember.GroupId);

            await Client.DeleteTournamentAsync(tournamentId, cancellationToken);

            await args.Message.RespondAsync("Cancelled the tournament. If any rounds were completed, stats from this tournament will be removed.");
        }
    }
}
