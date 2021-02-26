using Brakt.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Bot.Formatters
{
    public class DiscordResponseFormatter : IResponseFormatter
    {
        private readonly IBraktApiClient _client;
        
        private DataTable GetMockTable()
        {
            var dt = new DataTable();
            var rand = new Random();

            dt.Columns.Add("test 1", typeof(int));
            dt.Columns.Add("test 2", typeof(string));
            dt.Columns.Add("test 3", typeof(int));

            for (int i = 0; i < 5; i++)
            {
                dt.Rows.Add(rand.Next(1, 100), $"test value {i}", rand.Next(1000, 10000));
            }

            return dt;
        }

        public DiscordResponseFormatter(IBraktApiClient client)
        {
            _client = client;
        }

        public async Task<string> FormatAsLeaderboardAsync(IEnumerable<Statistic> stats, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            using var dt = GetMockTable();

            var stringTable = $"```{AsciiTableGenerator.CreateAsciiTableFromDataTable(dt)}```";

            return stringTable;
        }

        public async Task<string> FormatRoundPairingsAsync(Round round, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            using var dt = GetMockTable();

            var stringTable = $"```{AsciiTableGenerator.CreateAsciiTableFromDataTable(dt)}```";

            return stringTable;
        }

        public async Task<string> FormatStatsAsync(IEnumerable<Statistic> stats, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            using var dt = GetMockTable();

            var stringTable = $"```{AsciiTableGenerator.CreateAsciiTableFromDataTable(dt)}```";

            return stringTable;
        }

        public async Task<string> FormatTournamentListAsync(IEnumerable<Tournament> tournaments, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            using var dt = GetMockTable();

            var stringTable = $"```{AsciiTableGenerator.CreateAsciiTableFromDataTable(dt)}```";

            return stringTable;
        }

        public async Task<string> FormatTournamentResultsAsync(Tournament tournament, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            using var dt = GetMockTable();

            var stringTable = $"```{AsciiTableGenerator.CreateAsciiTableFromDataTable(dt)}```";

            return stringTable;
        }

        public async Task<string> FormatTournamentWinnersAsync(IEnumerable<TournamentWinner> winners, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            using var dt = GetMockTable();

            var stringTable = $"```{AsciiTableGenerator.CreateAsciiTableFromDataTable(dt)}```";

            return stringTable;
        }
    }

    public class AsciiTableGenerator
    {
        public static StringBuilder CreateAsciiTableFromDataTable(DataTable table)
        {
            var lenghtByColumnDictionary = GetTotalSpaceForEachColumn(table);

            var tableBuilder = new StringBuilder();
            AppendColumns(table, tableBuilder, lenghtByColumnDictionary);
            AppendRows(table, lenghtByColumnDictionary, tableBuilder);
            return tableBuilder;
        }

        private static void AppendRows(DataTable table, IReadOnlyDictionary<int, int> lenghtByColumnDictionary,
            StringBuilder tableBuilder)
        {
            for (var i = 0; i < table.Rows.Count; i++)
            {
                var rowBuilder = new StringBuilder();
                for (var j = 0; j < table.Columns.Count; j++)
                {
                    rowBuilder.Append(PadWithSpaceAndSeperator(table.Rows[i][j].ToString().Trim(),
                        lenghtByColumnDictionary[j]));
                }
                tableBuilder.AppendLine(rowBuilder.ToString());
            }
        }

        private static void AppendColumns(DataTable table, StringBuilder builder,
            IReadOnlyDictionary<int, int> lenghtByColumnDictionary)
        {
            for (var i = 0; i < table.Columns.Count; i++)
            {
                var columName = table.Columns[i].ColumnName.Trim();
                var paddedColumNames = PadWithSpaceAndSeperator(columName, lenghtByColumnDictionary[i]);
                builder.Append(paddedColumNames);
            }
            builder.AppendLine();
            builder.AppendLine(string.Join("", Enumerable.Repeat("-", builder.ToString().Length - 3).ToArray()));
        }

        private static Dictionary<int, int> GetTotalSpaceForEachColumn(DataTable table)
        {
            var lengthByColumn = new Dictionary<int, int>();
            for (var i = 0; i < table.Columns.Count; i++)
            {
                var length = new int[table.Rows.Count];
                for (var j = 0; j < table.Rows.Count; j++)
                {
                    length[j] = table.Rows[j][i].ToString().Trim().Length;
                }
                lengthByColumn[i] = length.Max();
            }
            return CompareToColumnNameLengthAndUpdate(table, lengthByColumn);
        }

        private static Dictionary<int, int> CompareToColumnNameLengthAndUpdate(DataTable table,
            IReadOnlyDictionary<int, int> lenghtByColumnDictionary)
        {
            var dictionary = new Dictionary<int, int>();
            for (var i = 0; i < table.Columns.Count; i++)
            {
                var columnNameLength = table.Columns[i].ColumnName.Trim().Length;
                dictionary[i] = columnNameLength > lenghtByColumnDictionary[i]
                    ? columnNameLength
                    : lenghtByColumnDictionary[i];
            }
            return dictionary;
        }

        private static string PadWithSpaceAndSeperator(string value, int totalColumnLength)
        {
            var remaningSpace = value.Length < totalColumnLength
                ? totalColumnLength - value.Length
                : value.Length - totalColumnLength;
            var spaces = string.Join("", Enumerable.Repeat(" ", remaningSpace).ToArray());
            return value + spaces + " | ";
        }
    }
}
