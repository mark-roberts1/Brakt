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
    public class ListCommandHandler : CommandHandlerBase, ICommandHandler
    {
        public ListCommandHandler(IBraktApiClient client, IResponseFormatter formatter) : base(client, formatter)
        {
        }

        public string Command => "list";

        public string HelpMessage
            => "Lists tournaments associated with the server, past or anticipated.\n   * Arguments:\n     * [all] - optional.If specified, this will show all tournaments.";

        public override async Task ExecuteAsync(MessageCreateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            AssertGroupMemberContext(userContext);

            var includeAll = cmdToken.Arguments != null && cmdToken.Arguments.Any(w => w == "all");

            var tournaments = await Client.GetTournamentsAsync(userContext.Group.GroupId, cancellationToken);

            if (!tournaments.Any())
            {
                var msg = await Formatter.FormatTournamentListAsync(tournaments, cancellationToken);

                await args.Message.RespondAsync(msg);
                return;
            }

            if (!includeAll)
                tournaments = tournaments.Where(w => !w.Completed);

            if (!tournaments.Any())
            {
                await args.Message.RespondAsync("No upcoming tournaments. Use `brakt list all` to show completed tournaments.");
                return;
            }

            var message = await Formatter.FormatTournamentListAsync(tournaments, cancellationToken);

            await args.Message.RespondAsync(message);
        }
    }
}
