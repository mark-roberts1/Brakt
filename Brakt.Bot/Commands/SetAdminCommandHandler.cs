using Brakt.Bot.Identification;
using Brakt.Bot.Interpretor;
using Brakt.Client;
using DSharpPlus.Entities;
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
    public class SetAdminCommandHandler : ICommandHandler
    {
        private readonly IBraktApiClient _client;
        private readonly IContextFactory _contextFactory;
        private readonly Regex _mentionedUserRgx = new Regex(@"\<\@\!\d+\>");

        public SetAdminCommandHandler(IBraktApiClient client, IContextFactory contextFactory)
        {
            _client = client;
            _contextFactory = contextFactory;
        }

        public string Command => "set-admin";

        public string HelpMessage => 
            "gives mentioned player(s) administrative priveleges.";

        public async Task ExecuteAsync(MessageCreateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            if (!userContext.IsGroupMemberContext)
            {
                await args.Message.RespondAsync("This command is only available in the context of a group.");
                return;
            }
            else if (!userContext.GroupMember.IsAdmin)
            {
                await args.Message.RespondAsync("Only admins can grant administrative priveleges to another player.");
                return;
            }

            foreach (var mention in cmdToken.Arguments.Where(w => _mentionedUserRgx.IsMatch(w)))
            {
                var subjectContext = await _contextFactory.GetIdContextAsync(ParseUserId(mention), userContext.Group, cancellationToken);
                
                await _client.SetGroupAdminAsync(subjectContext.GroupMember.GroupId, subjectContext.GroupMember.PlayerId, cancellationToken, true);
            }

            await args.Message.CreateReactionAsync(DiscordEmoji.FromName(BotConnector.Client, ":thumbsup:"));
        }

        private ulong ParseUserId(string mention)
        {
            mention = mention.Replace("<", "").Replace("@", "").Replace("!", "").Replace(">", "");

            return ulong.Parse(mention);
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
