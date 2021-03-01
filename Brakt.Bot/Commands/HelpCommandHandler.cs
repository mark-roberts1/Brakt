using Brakt.Bot.Formatters;
using Brakt.Bot.Identification;
using Brakt.Bot.Interpretor;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Bot.Commands
{
    public class HelpCommandHandler : ICommandHandler
    {
        public string Command => "help";

        public string HelpMessage
        {
            get
            {
                return $"Available commands: {string.Join(", ", CommandHandlerFactory.HelpMessages.Select(s => s.Command).ToArray())}";
            }
        }

        public async Task ExecuteAsync(MessageCreateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            var handlers = CommandHandlerFactory.HelpMessages.Where(w => cmdToken.Arguments.Contains(w.Command));

            if (handlers == null || !handlers.Any())
            {
                await args.Message.RespondAsync(HelpMessage);
                await args.Message.RespondAsync($"Use `brakt help <command>` to learn about specific commands.");
                return;
            }

            foreach (var handler in handlers)
            {
                await args.Message.RespondAsync(handler.HelpMessage);
            }
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
