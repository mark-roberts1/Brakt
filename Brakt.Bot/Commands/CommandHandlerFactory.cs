using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brakt.Bot.Commands
{
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IEnumerable<ICommandHandler> _handlers;

        public CommandHandlerFactory(IEnumerable<ICommandHandler> handlers)
        {
            _handlers = handlers;
        }

        public ICommandHandler GetCommandHandler(string name)
        {
            return _handlers.FirstOrDefault(w => w.Command == name);
        }
    }
}
