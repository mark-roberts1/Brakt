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
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Bot.Commands
{
    public class RemoveAdminCommandHandler : CommandHandlerBase, ICommandHandler
    {
        private readonly IContextFactory _contextFactory;
        private readonly Regex _mentionedUserRgx = new Regex(@"\<\@\!\d+\>");

        public RemoveAdminCommandHandler(IBraktApiClient client, IContextFactory contextFactory, IResponseFormatter formatter) : base(client, formatter)
        {
            _contextFactory = contextFactory;
        }

        public string Command => "remove-admin";

        public string HelpMessage =>
            "Removes mentioned player(s) administrative priveleges.";

        public override async Task ExecuteAsync(MessageCreateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            AssertGroupMemberContext(userContext);
            AssertUserIsAdmin(userContext.GroupMember);

            foreach (var mention in cmdToken.Arguments.Where(w => _mentionedUserRgx.IsMatch(w)))
            {
                var subjectContext = await _contextFactory.GetIdContextAsync(ParseUserId(mention), userContext.Group, cancellationToken);

                await Client.SetGroupAdminAsync(subjectContext.GroupMember.GroupId, subjectContext.GroupMember.PlayerId, cancellationToken, false);
            }

            await args.Message.CreateReactionAsync(DiscordEmoji.FromName(BotConnector.Client, ":thumbsup:"));
        }

        private ulong ParseUserId(string mention)
        {
            mention = mention.Replace("<", "").Replace("@", "").Replace("!", "").Replace(">", "");

            return ulong.Parse(mention);
        }
    }
}
