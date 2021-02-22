using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt.Bot.Interpretor
{
    public class CommandTokens
    {
        public CommandTokens(string command, IEnumerable<string> args, IEnumerable<string> tags)
        {
            Command = command;
            Arguments = args ?? new string[0];
            Tags = tags ?? new string[0];
        }

        public string Command { get; }
        public IEnumerable<string> Arguments { get; }
        public IEnumerable<string> Tags { get; }
    }
}
