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
    public class PairingsCommandHandler : ICommandHandler
    {
        private readonly IBraktApiClient _client;
        private readonly IResponseFormatter _formatter;

        public PairingsCommandHandler(IBraktApiClient client, IResponseFormatter formatter)
        {
            _client = client;
            _formatter = formatter;
        }

        public string Command => "pairings";

        public async Task ExecuteAsync(MessageCreateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
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

            var tournament = await _client.GetTournamentAsync(tournamentId, cancellationToken);

            if (tournament == null)
            {
                await args.Message.RespondAsync("Are you sure that id is correct? I couldn't find a tournament. :thinking:");
                return;
            }

            var rounds = await _client.GetTournamentRoundsAsync(tournamentId, cancellationToken);

            if (rounds == null || !rounds.Any())
            {
                await args.Message.RespondAsync($"This tournament hasn't started yet. Start the tournament with ```brakt fire {tournamentId}```");
            }

            var round = rounds.OrderBy(ob => ob.RoundNumber).Last();

            var resp = await _formatter.FormatRoundPairingsAsync(round, cancellationToken);

            await args.Message.RespondAsync(resp);
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
