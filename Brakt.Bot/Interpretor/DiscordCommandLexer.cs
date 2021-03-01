using Brakt.Bot.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Brakt.Bot.Interpretor
{
    public class DiscordCommandLexer : ILexer
    {
        const string TOKEN_RGX = "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        const string BRAKT_CMD_INVOKER = "brakt ";
        private readonly IEnumerable<string> _validCommands;

        public DiscordCommandLexer(IEnumerable<ICommandHandler> handlers)
        {
            _validCommands = handlers.Select(s => s.Command).ToArray();
        }

        public bool IsBraktCommand(string message)
        {
            return message.ToLower().StartsWith(BRAKT_CMD_INVOKER);
        }

        public CommandTokens TokenizeBraktCommand(string command)
        {
            command = command.ToLower().Replace(BRAKT_CMD_INVOKER, string.Empty);

            var parts = Regex.Split(command, TOKEN_RGX);
            var commandName = string.Empty;
            var args = new List<string>();
            var tags = new List<string>();

            for (int i = 0; i < parts.Length; i++)
            {
                if (string.IsNullOrEmpty(parts[i])) continue;

                if (i == 0)
                {
                    commandName = parts[i];
                    continue;
                }

                if (parts[i].StartsWith('#'))
                {
                    tags.Add(parts[i].Replace("#", ""));
                    continue;
                }

                args.Add(parts[i]);
            }

            if (!_validCommands.Contains(commandName)) throw new ArgumentException($"'{commandName}' is not a valid brakt command.");

            return new CommandTokens(commandName, args, tags);
        }
    }
}
