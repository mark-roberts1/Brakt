using System.Text.RegularExpressions;

namespace Brakt.Bot.Interpretor
{
    public class DiscordCommandLexer : ILexer
    {
        const string TOKEN_RGX = "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        const string BRAKT_CMD_INVOKER = "brakt ";

        public bool IsBraktCommand(string message)
        {
            return message.StartsWith(BRAKT_CMD_INVOKER);
        }

        public string[] TokenizeBraktCommand(string command)
        {
            return Regex.Split(command, TOKEN_RGX);
        }
    }
}
