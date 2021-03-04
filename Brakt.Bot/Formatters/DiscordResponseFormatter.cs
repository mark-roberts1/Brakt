using Brakt.Client;
using DSharpPlus.Entities;
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
        private readonly ITableFormatter _tableFormatter;

        public DiscordResponseFormatter(IBraktApiClient client, ITableFormatter tableFormatter)
        {
            _client = client;
            _tableFormatter = tableFormatter;
        }

        public async Task<string> FormatAsLeaderboardAsync(IEnumerable<Statistic> stats, CancellationToken cancellationToken)
        {
            using var dt = new DataTable();

            dt.Columns.Add("Rank", typeof(string));
            dt.Columns.Add("Player", typeof(string));
            dt.Columns.Add("Wins", typeof(int));
            dt.Columns.Add("Losses", typeof(int));
            dt.Columns.Add("Tournament Wins", typeof(int));

            int rank = 0;
            int prevWins = -1;
            int prevLosses = -1;
            int prevTournamentWins = -1;
            var prefetchData = new List<Player>();

            foreach (var playerId in stats.Select(s => s.PlayerId).Distinct())
            {
                prefetchData.Add(await _client.GetPlayerAsync(playerId, cancellationToken));
            }

            foreach (var stat in stats.OrderByDescending(ob => ob.Wins).ThenBy(tb => tb.Losses).ThenByDescending(tbd => tbd.TournamentWins))
            {
                bool rankChanged = false;
                var player = prefetchData.First(w => w.PlayerId == stat.PlayerId);

                if (stat.Wins != prevWins || stat.Losses != prevLosses || stat.TournamentWins != prevTournamentWins)
                {
                    rank++;
                    prevWins = stat.Wins;
                    prevLosses = stat.Losses;
                    prevTournamentWins = stat.TournamentWins;
                    rankChanged = true;
                }

                dt.Rows.Add(rankChanged ? $"{rank}." : "", player.Username, stat.Wins, stat.Losses, stat.TournamentWins);
            }

            return _tableFormatter.FormatAsTable(dt);
        }

        public async Task<string> FormatRoundPairingsAsync(Round round, CancellationToken cancellationToken)
        {
            var pairings = await _client.GetPairingsAsync(round.RoundId, cancellationToken);

            var playerIds = pairings.Select(s => s.Player1).Union(pairings.Select(s => s.Player2)).Distinct();

            var players = new List<Player>();

            foreach (var id in playerIds)
            {
                players.Add(await _client.GetPlayerAsync(id, cancellationToken));
            }

            using var dt = new DataTable();
            dt.Columns.Add("Pairings", typeof(string));

            foreach (var pairing in pairings)
            {
                var p1 = players.First(w => w.PlayerId == pairing.Player1);
                var p2 = players.First(w => w.PlayerId == pairing.Player2);

                dt.Rows.Add($"{p1.Username} vs. {p2.Username}");
            }

            return _tableFormatter.FormatAsTable(dt);
        }

        public async Task<string> FormatStatsAsync(IEnumerable<Statistic> stats, CancellationToken cancellationToken)
        {
            using var dt = new DataTable();

            dt.Columns.Add("Group", typeof(string));
            dt.Columns.Add("Player", typeof(string));
            dt.Columns.Add("Wins", typeof(int));
            dt.Columns.Add("Losses", typeof(int));
            dt.Columns.Add("Tournament Wins", typeof(int));

            var players = new List<Player>();
            var groups = new List<Group>();

            foreach (var playerId in stats.Select(s => s.PlayerId).Distinct())
            {
                players.Add(await _client.GetPlayerAsync(playerId, cancellationToken));
            }

            foreach (var groupId in stats.Select(s => s.GroupId).Distinct())
            {
                groups.Add(await _client.GetGroupAsync(groupId, cancellationToken));
            }

            foreach (var stat in stats)
            {
                var player = players.First(w => w.PlayerId == stat.PlayerId);
                var group = groups.First(w => w.GroupId == stat.GroupId);

                dt.Rows.Add(group.GroupName, player.Username, stat.Wins, stat.Losses, stat.TournamentWins);
            }

            return _tableFormatter.FormatAsTable(dt);
        }

        public Task<string> FormatTournamentListAsync(IEnumerable<Tournament> tournaments, CancellationToken cancellationToken)
        {
            using var dt = new DataTable();

            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Tags", typeof(string));
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Completed", typeof(char));

            foreach (var tournament in tournaments)
            {
                dt.Rows.Add(
                    tournament.TournamentId,
                    string.Join(", ", tournament.Tags.Select(s => s.TagValue)),
                    $"{tournament.BracketType}",
                    tournament.StartDate.ToString("yyyy-MM-dd"),
                    tournament.Completed ? 'Y' : 'N');
            }

            return Task.FromResult(_tableFormatter.FormatAsTable(dt));
        }

        public async Task<string> FormatTournamentResultsAsync(Tournament tournament, CancellationToken cancellationToken)
        {
            if (tournament.Completed)
            {
                var winners = await _client.GetTournamentWinnersAsync(tournament.TournamentId, cancellationToken);

                return await FormatTournamentWinnersAsync(winners, cancellationToken);
            }

            var rounds = await _client.GetTournamentRoundsAsync(tournament.TournamentId, cancellationToken);

            var latestRound = rounds.OrderByDescending(ob => ob.RoundNumber).LastOrDefault();

            var results = await _client.GetRoundResultsAsync(latestRound.RoundId, cancellationToken);
            var pairings = await _client.GetPairingsAsync(latestRound.RoundId, cancellationToken);

            using var dt = new DataTable();
            
            dt.Columns.Add("Winner", typeof(string));
            dt.Columns.Add("Matchup", typeof(string));
            dt.Columns.Add("Result", typeof(string));

            foreach (var pairing in pairings)
            {
                var p1 = await _client.GetPlayerAsync(pairing.Player1, cancellationToken);
                var p2 = await _client.GetPlayerAsync(pairing.Player2, cancellationToken);
                var result = results.FirstOrDefault(w => w.PairingId == pairing.PairingId);

                var winnerText = "";
                var resultText = "";

                if (result == null)
                {
                    winnerText = "Incomplete";
                    resultText = " - ";
                }
                else if (result.Draw)
                {
                    winnerText = "Draw";
                    resultText = $"{result.Wins}-{result.Losses}";
                }
                else
                {
                    var winner = new List<Player> { p1, p2 }.Single(w => w.PlayerId == result.WinningPlayerId.Value);
                    winnerText = winner.Username;
                    resultText = $"{result.Wins}-{result.Losses}";
                }

                dt.Rows.Add(winnerText, $"{p1.Username} vs. {p2.Username}", resultText);
            }

            return _tableFormatter.FormatAsTable(dt);
        }

        public async Task<string> FormatTournamentWinnersAsync(IEnumerable<TournamentWinner> winners, CancellationToken cancellationToken)
        {
            if (winners.Count() == 1)
            {
                var player = await _client.GetPlayerAsync(winners.Single().PlayerId, cancellationToken);

                return $"Behold your champion: {player.Username}!";
            }

            var players = new List<Player>();

            foreach (var winner in winners)
            {
                players.Add(await _client.GetPlayerAsync(winner.PlayerId, cancellationToken));
            }

            using var dt = new DataTable();
            dt.Columns.Add("Player", typeof(string));

            foreach (var player in players)
                dt.Rows.Add(player.Username);

            return _tableFormatter.FormatAsTable(dt);
        }
    }
}
