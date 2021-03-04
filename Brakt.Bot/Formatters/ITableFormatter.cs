using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Brakt.Bot.Formatters
{
    public interface ITableFormatter
    {
        string FormatAsTable(DataTable dt);
    }
}
