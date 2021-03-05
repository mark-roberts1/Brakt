using Brakt.Bot.Formatters;
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
    public class DropCommandHandler : CommandHandlerBase, ICommandHandler
    {
        public DropCommandHandler(IBraktApiClient client, IResponseFormatter formatter) : base(client, formatter)
        {
        }

        public string Command => "drop";

        public string HelpMessage
            => "Drop out of a tournament. Coward.\n     * [tournament id] - an integer id given when a tournament is generated.This can be found with the list command if it has been forgotten.";

        public override async Task ExecuteAsync(MessageCreateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            if (!TryGetTournamentId(cmdToken.Arguments, out int tournamentId))
            {
                await args.Message.RespondAsync("TournamentId argument required.");
                return;
            }

            var tournament = await Client.GetTournamentAsync(tournamentId, cancellationToken);

            AssertTournamentExists(tournament);
            AssertTournamentNotComplete(tournament);

            await Client.RemovePlayerAsync(tournamentId, userContext.Player.PlayerId, cancellationToken);
            await args.Message.CreateReactionAsync(DiscordEmoji.FromName(BotConnector.Client, ":thumbsup:"));
        }
    }
}
