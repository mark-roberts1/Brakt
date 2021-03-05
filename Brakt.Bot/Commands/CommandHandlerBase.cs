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
    public abstract class CommandHandlerBase
    {
        protected IBraktApiClient Client { get; }
        protected IResponseFormatter Formatter { get; }

        public CommandHandlerBase(IBraktApiClient client, IResponseFormatter formatter)
        {
            Client = client;
            Formatter = formatter;
        }

        public virtual Task ExecuteAsync(MessageCreateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public virtual Task ExecuteAsync(MessageReactionRemoveEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public virtual Task ExecuteAsync(MessageReactionAddEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public virtual Task ExecuteAsync(MessageUpdateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected void AssertGroupMemberContext(IdContext context)
        {
            if (!context.IsGroupMemberContext)
            {
                throw new ArgumentException("This command must be run on a server.");
            }
        }

        protected void AssertUserIsAdmin(GroupMember groupMember)
        {
            if (!groupMember.IsAdmin && !groupMember.IsOwner)
            {
                throw new ArgumentException("Only an admin or owner may do this.");
            }
        }

        protected bool TryGetTournamentId(IEnumerable<string> args, out int tournamentId)
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

        protected void AssertTournamentExists(Tournament tournament)
        {
            if (tournament == null)
            {
                throw new ArgumentException("Invalid TournamentId provided.");
            }
        }

        protected void AssertTournamentNotComplete(Tournament tournament)
        {
            if (tournament.Completed)
            {
                throw new ArgumentException("This tournament has completed.");
            }
        }

        protected void AssertTournamentNotInProgress(IEnumerable<Round> rounds)
        {
            if (rounds != null && rounds.Any())
            {
                throw new ArgumentException("This tournament is already in progress.");
            }
        }

        protected void AssertTournamentBelongsToGroup(Tournament tournament, int groupId)
        {
            if (tournament.GroupId != groupId)
            {
                throw new ArgumentException("Invalid TournamentId provided.");
            }
        }
    }
}
