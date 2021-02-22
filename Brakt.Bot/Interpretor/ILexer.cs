using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt.Bot.Interpretor
{
    public interface ILexer
    {
        bool IsBraktCommand(string message);
        CommandTokens TokenizeBraktCommand(string command);
    }
}
