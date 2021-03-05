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
    public class PairingsCommandHandler : CommandHandlerBase, ICommandHandler
    {
        public PairingsCommandHandler(IBraktApiClient client, IResponseFormatter formatter) : base(client, formatter)
        {
        }

        public string Command => "pairings";

        public string HelpMessage
            => "Show the generated pairings for the anticipated round.\n     * [tournament id] - an integer id given when a tournament is generated. This can be found with the list command if it has been forgotten.";

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

            var rounds = await Client.GetTournamentRoundsAsync(tournamentId, cancellationToken);

            if (rounds == null || !rounds.Any())
            {
                await args.Message.RespondAsync($"This tournament hasn't started yet. Start the tournament with ```brakt fire {tournamentId}```");
            }

            var round = rounds.OrderBy(ob => ob.RoundNumber).Last();

            var resp = await Formatter.FormatRoundPairingsAsync(round, cancellationToken);

            await args.Message.RespondAsync(resp);
        }
    }
}
