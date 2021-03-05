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
    public class CreateCommandHandler : CommandHandlerBase, ICommandHandler
    {
        private readonly static Regex _tsFormat = new Regex(@"\d\d\:\d\d\:\d\d\:\d\d");
        private readonly static (string Name, BracketType Value)[] _bracketTypes =
        {
            ("swiss", BracketType.Swiss),
            ("single", BracketType.SingleElimination),
            ("rr", BracketType.RoundRobin),
        };

        public CreateCommandHandler(IBraktApiClient client, IResponseFormatter formatter) : base(client, formatter)
        {
        }

        public string Command => "create";

        public string HelpMessage
            => "Create a new tournament.\n   * Arguments:\n     * [swiss|single|rr] - determines the type of tournament that will be generated.Default swiss.\n     * [dd: HH:mm:ss] - Time until scheduled to start. Default 1 hour.\n     * #tag1 #tag2 ... #tagN - useful for finding player/group statistics. At least one tag argument is required";

        public override async Task ExecuteAsync(MessageCreateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            if (cmdToken.Tags == null || !cmdToken.Tags.Any())
            {
                await args.Message.RespondAsync("At least one tag is required to create a tournament.");
                return;
            }

            var request = new CreateTournamentRequest
            {
                GroupId = userContext.GroupMember.GroupId,
                StartDate = DateTime.Now.AddHours(1),
                Tags = cmdToken.Tags.ToList()
            };

            AssertGroupMemberContext(userContext);
            AssertUserIsAdmin(userContext.GroupMember);

            if (TryParseTime(cmdToken.Arguments, out TimeSpan ts)) request.StartDate = DateTime.Now + ts;
            if (TryFindBracketType(cmdToken.Arguments, out BracketType bracketType)) request.BracketType = bracketType;

            var tournament = await Client.CreateTournamentAsync(request, cancellationToken);

            await args.Message.RespondAsync($"Tournament {tournament.TournamentId} created! To enter, type ```brakt join {tournament.TournamentId}```");
        }

        private bool TryParseTime(IEnumerable<string> args, out TimeSpan ts)
        {
            ts = TimeSpan.MinValue;

            if (args == null) return false;

            var timeArgs = args.Where(w => _tsFormat.IsMatch(w));

            if (timeArgs.Count() > 1)
                throw new ArgumentException("More than one argument was supplied for time.");
            else if (!timeArgs.Any())
                return false;

            var parts = timeArgs.Single().Split(':');

            ts = new TimeSpan(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3]));
            return true;
        }

        private bool TryFindBracketType(IEnumerable<string> args, out BracketType bracketType)
        {
            bracketType = BracketType.Swiss;

            if (args == null) return false;

            var brktArgs = args.Where(w => _bracketTypes.Select(s => s.Name).Contains(w));

            if (brktArgs.Count() > 1)
                throw new ArgumentException("More than one argument was supplied for bracket type.");
            else if (!brktArgs.Any())
                return false;

            bracketType = _bracketTypes.Single(w => w.Name == brktArgs.Single()).Value;
            return true;
        }
    }
}
