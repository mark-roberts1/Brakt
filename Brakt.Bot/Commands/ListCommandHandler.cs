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
    public class ListCommandHandler : ICommandHandler
    {
        private readonly IBraktApiClient _client;
        private readonly IResponseFormatter _formatter;

        public ListCommandHandler(IBraktApiClient client, IResponseFormatter formatter)
        {
            _client = client;
            _formatter = formatter;
        }

        public string Command => "list";

        public string HelpMessage
            => "Lists tournaments associated with the server, past or anticipated.\n   * Arguments:\n     * [all] - optional.If specified, this will show all tournaments.";

        public async Task ExecuteAsync(MessageCreateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            if (!userContext.IsGroupContext)
            {
                await args.Message.RespondAsync("This command is only valid in the context of a Server.");
                return;
            }

            var includeAll = cmdToken.Arguments != null && cmdToken.Arguments.Any(w => w == "all");

            var tournaments = await _client.GetTournamentsAsync(userContext.Group.GroupId, cancellationToken);

            if (!includeAll)
                tournaments = tournaments.Where(w => !w.Completed);

            var message = await _formatter.FormatTournamentListAsync(tournaments, cancellationToken);

            await args.Message.RespondAsync(message);
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
