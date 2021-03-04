using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Brakt.Bot.Formatters
{
    public class AsciiTableFormatter : ITableFormatter
    {
        public string FormatAsTable(DataTable dt)
        {
            var sb = new StringBuilder();

            int[] colLengths = GetColumnLengths(dt);

            WriteLine(dt.Columns.Cast<DataColumn>().Select(s => s.ColumnName).ToArray(), colLengths, sb);
            WriteLine(dt.Columns.Cast<DataColumn>().Select(s => "-").ToArray(), colLengths, sb);

            foreach (var row in dt.Rows.Cast<DataRow>())
            {
                WriteLine(row.ItemArray, colLengths, sb);
            }

            return $"```{sb}```";
        }

        private int[] GetColumnLengths(DataTable dt)
        {
            int[] colLengths = new int[dt.Columns.Count];

            for (int col = 0; col < dt.Columns.Count; col++)
            {
                int maxLength = dt.Columns[col].ColumnName.Length;

                foreach (var row in dt.Rows.Cast<DataRow>())
                {
                    int currentLength = row[col] == null ? 0 : row[col].ToString().Length;

                    if (currentLength > maxLength) maxLength = currentLength;
                }

                colLengths[col] = maxLength;
            }

            return colLengths;
        }

        private void WriteLine(object[] values, int[] lengths, StringBuilder sb)
        {
            var stringValues = values.Select(s => s == null ? string.Empty : s.ToString()).ToArray();

            sb.Append('|');

            for (int i = 0; i < lengths.Length; i++)
            {
                int length = lengths[i];
                var value = stringValues[i];

                sb.Append(PadValueForDisplay(value, length));
                sb.Append('|');
            }

            sb.AppendLine();
        }

        private string PadValueForDisplay(string value, int length)
        {
            if (value == "-")
            {
                int trueLength = length + 2;
                string retVal = string.Empty;

                for (int i = 0; i < trueLength; i++) retVal += "-";

                return retVal;
            }

            return $" {value.PadRight(length)} ";
        }
    }
}
