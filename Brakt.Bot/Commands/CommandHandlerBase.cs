using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt.Bot.Commands
{
    public abstract class CommandHandlerBase
    {
        protected const string MSG_FORMAT_OPEN = "```";
        protected const string MSG_FORMAT_CLOSE = "```";
        protected const string DELIMITER = "|";
        protected const string LINE_TERMINATOR = "\n";
    }
}
