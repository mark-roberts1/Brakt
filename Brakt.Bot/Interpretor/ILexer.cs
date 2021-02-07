using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt.Bot.Interpretor
{
    public interface ILexer
    {
        bool IsBraktCommand(string message);
        string[] TokenizeBraktCommand(string command);
    }
}
