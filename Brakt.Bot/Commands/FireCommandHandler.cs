using Brakt.Bot.Formatters;
using Brakt.Bot.Identification;
using Brakt.Bot.Interpretor;
using Brakt.Client;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Bot.Commands
{
    public class FireCommandHandler : CommandHandlerBase, ICommandHandler
    {
        public FireCommandHandler(IBraktApiClient client, IResponseFormatter formatter) : base(client, formatter)
        {
        }

        public string Command => "fire";

        public string HelpMessage
            => "Generates round 1 bracket and starts a tournament.\n   * Arguments:\n     * [tournament id] - an integer id given when a tournament is generated.This can be found with the list command if it has been forgotten.";

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

            var round = await Client.StartTournamentAsync(tournamentId, cancellationToken);

            var resp = await Formatter.FormatRoundPairingsAsync(round, cancellationToken);

            await args.Message.RespondAsync(resp);
        }
    }
}
