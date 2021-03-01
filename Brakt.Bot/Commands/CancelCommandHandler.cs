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
    public class CancelCommandHandler : ICommandHandler
    {
        private readonly IBraktApiClient _client;

        public CancelCommandHandler(IBraktApiClient client)
        {
            _client = client;
        }

        public string Command => "cancel";

        public string HelpMessage 
            => "Cancel a tournament. If underway, stats will be cleared for the given tournament.\n   * Arguments:\n     * [tournament id] - an integer id given when a tournament is generated. This can be found with the list command if it has been forgotten.";

        public async Task ExecuteAsync(MessageCreateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            if (!userContext.IsGroupMemberContext || !userContext.GroupMember.IsAdmin || !userContext.GroupMember.IsOwner)
            {
                await args.Message.RespondAsync("You must be an owner or admin to cancel a tournament.");
            }

            int tournamentId;

            try
            {
                if (!TryGetTournamentId(cmdToken.Arguments, out tournamentId))
                {
                    await args.Message.RespondAsync("TournamentId argument required.");
                    return;
                }
            }
            catch (ArgumentException e)
            {
                await args.Message.RespondAsync(e.Message);
                return;
            }

            await _client.DeleteTournamentAsync(tournamentId, cancellationToken);

            await args.Message.RespondAsync("Cancelled the tournament. If any rounds were completed, stats from this tournament will be removed.");
        }

        private bool TryGetTournamentId(IEnumerable<string> args, out int tournamentId)
        {
            tournamentId = -1;

            if (args == null || !args.Any()) return false;

            var intArgs = args.Where(w => int.TryParse(w, out int _));

            if (!intArgs.Any())
                return false;
            else if (intArgs.Count() > 1)
                throw new ArgumentException($"Cannot determine which integer is tournament id from {string.Join(", ", intArgs.ToArray())}");
            else
                tournamentId = int.Parse(intArgs.Single());

            return true;
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
