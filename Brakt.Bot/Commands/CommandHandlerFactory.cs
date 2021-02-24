using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brakt.Bot.Commands
{
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private static readonly IList<ICommandHandler> _handlers = new List<ICommandHandler>
        {
        };

        public ICommandHandler GetCommandHandler(string name)
        {
            return _handlers.FirstOrDefault(w => w.Command == name);
        }
    }
}
