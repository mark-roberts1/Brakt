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
    public class EntriesCommandHandler : CommandHandlerBase, ICommandHandler
    {
        public EntriesCommandHandler(IBraktApiClient client, IResponseFormatter formatter) : base(client, formatter) { }

        public string Command => "entries";

        public string HelpMessage => "Lists the active players registered for a given tournament.\n     * [tournament id] - an integer id given when a tournament is generated.This can be found with the list command if it has been forgotten.";

        public override async Task ExecuteAsync(MessageCreateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            AssertGroupMemberContext(userContext);

            if (!TryGetTournamentId(cmdToken.Arguments, out int tournamentId))
            {
                await args.Message.RespondAsync("TournamentId argument required.");
                return;
            }

            var tournament = await Client.GetTournamentAsync(tournamentId, cancellationToken);

            AssertTournamentExists(tournament);
            AssertTournamentBelongsToGroup(tournament, userContext.GroupMember.GroupId);

            var entries = await Client.GetTournamentEntriesAsync(tournamentId, cancellationToken);

            if (entries == null || !entries.Any())
            {
                await args.Message.RespondAsync($"No one has joined this tournament yet. use `brakt join {tournamentId}` to join.");
                return;
            }

            var groupPlayers = await Client.GetPlayersAsync(userContext.GroupMember.GroupId, cancellationToken);

            groupPlayers = groupPlayers.Where(w => entries.Select(s => s.PlayerId).Contains(w.PlayerId));

            var msg = string.Join("\n", groupPlayers.Select(s => s.Username).ToArray());

            await args.Message.RespondAsync($"```Entries:\n\n{msg}```");
        }
    }
}
