using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt.Bot.Commands
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler GetCommandHandler(string name);
    }
}
