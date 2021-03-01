using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brakt.Bot.Commands
{
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IEnumerable<ICommandHandler> _handlers;
        public static IEnumerable<(string Command, string HelpMessage)> HelpMessages { get; private set; }

        public CommandHandlerFactory(IEnumerable<ICommandHandler> handlers)
        {
            _handlers = handlers;

            if (HelpMessages == null)
            {
                HelpMessages = handlers.Where(w => !(w is HelpCommandHandler)).Select(s => (s.Command, s.HelpMessage)).ToList();
            }
        }

        public ICommandHandler GetCommandHandler(string name)
        {
            return _handlers.FirstOrDefault(w => w.Command == name);
        }
    }
}
