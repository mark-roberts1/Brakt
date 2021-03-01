﻿using Brakt.Bot.Formatters;
using Brakt.Bot.Identification;
using Brakt.Bot.Interpretor;
using Brakt.Client;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Bot.Commands
{
    public class JoinCommandHandler : ICommandHandler
    {
        private readonly IBraktApiClient _client;

        public JoinCommandHandler(IBraktApiClient client)
        {
            _client = client;
        }

        public string Command => "join";

        public string HelpMessage 
            => "Join a created tournament that is not started. If tournament has fired, entry will not be permitted. An alternative to this is reacting to the tournament create message with the hand raise emoji.\n     * [tournament id] - an integer id given when a tournament is generated.This can be found with the list command if it has been forgotten.";

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

            await _client.RegisterPlayerAsync(new TournamentEntry { TournamentId = tournamentId, PlayerId = userContext.Player.PlayerId }, cancellationToken);

            await args.Message.CreateReactionAsync(DiscordEmoji.FromName(BotConnector.Client, ":thumbsup:"));
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
