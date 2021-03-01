﻿using Brakt.Bot.Formatters;
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
    public class AdvanceCommandHandler : ICommandHandler
    {
        private readonly IBraktApiClient _client;
        private readonly IResponseFormatter _formatter;

        public AdvanceCommandHandler(IBraktApiClient client, IResponseFormatter formatter)
        {
            _client = client;
            _formatter = formatter;
        }

        public string Command => "advance";

        public string HelpMessage 
            => "Advances tournament to the next round. If any matches are underway, the result will be recorded as a draw.\n     * [tournament id] - an integer id given when a tournament is generated. This can be found with the list command if it has been forgotten.";

        public async Task ExecuteAsync(MessageCreateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            if (!userContext.IsGroupMemberContext)
            {
                await args.Message.RespondAsync("A tournament must be managed within the context of a group.");
                return;
            }

            if (!userContext.GroupMember.IsAdmin && !userContext.GroupMember.IsOwner)
            {
                await args.Message.RespondAsync("A tournament must be advanced by an admin.");
                return;
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

            var round = await _client.AdvanceTournamentAsync(tournamentId, cancellationToken);

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
