using Brakt.Rest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Rest.Logic
{
    public class StatsGenerator : IStatsGenerator
    {
        private readonly IDataLayer _dataLayer;

        public StatsGenerator(IDataLayer dataLayer)
        {
            _dataLayer = dataLayer;
        }

        public async Task<IEnumerable<Statistic>> GenerateGroupStatsAsync(int groupId, CancellationToken cancellationToken)
        {
            var members = await _dataLayer.GetGroupMembersAsync(groupId, cancellationToken);
            var stats = new List<Statistic>();

            foreach (var member in members)
            {
                stats.Add(await GenerateStatsAsync(member.PlayerId, groupId, cancellationToken));
            }

            return stats;
        }

        public async Task<IEnumerable<Statistic>> GeneratePlayerStatsAsync(int playerId, CancellationToken cancellationToken)
        {
            var memberships = await _dataLayer.GetGroupMembersForPlayerAsync(playerId, cancellationToken);
            var stats = new List<Statistic>();

            foreach (var membership in memberships)
            {
                stats.Add(await GenerateStatsAsync(playerId, membership.GroupId, cancellationToken));
            }

            return stats;
        }

        public async Task<Statistic> GenerateStatsAsync(int playerId, int groupId, CancellationToken cancellationToken)
        {
            var stat = new Statistic
            {
                PlayerId = playerId,
                GroupId = groupId,
                TournamentWins = 0,
                Wins = 0,
                Losses = 0
            };

            var tournaments = await _dataLayer.GetTournamentsAsync(groupId, cancellationToken);

            foreach (var tournament in tournaments)
            {
                var tournamentStat = await GetTournamentStatsAsync(playerId, tournament, cancellationToken);

                stat.TournamentWins += tournamentStat.TournamentWins;
                stat.Wins += tournamentStat.Wins;
                stat.Losses += tournamentStat.Losses;
            }

            return stat;
        }

        public async Task<Statistic> GetTournamentStatsAsync(int playerId, Tournament tournament, CancellationToken cancellationToken)
        {
            var stat = new Statistic
            {
                PlayerId = playerId,
                GroupId = tournament.GroupId,
                TournamentWins = 0,
                Wins = 0,
                Losses = 0
            };

            var entry = (await _dataLayer.GetTournamentEntriesAsync(tournament.TournamentId, cancellationToken)).FirstOrDefault(w => w.PlayerId == playerId);

            if (entry == null) return stat;

            var winner = (await _dataLayer.GetTournamentWinnersAsync(tournament.TournamentId, cancellationToken)).FirstOrDefault(w => w.PlayerId == playerId);

            if (winner != null) stat.TournamentWins++;

            var rounds = await _dataLayer.GetRoundsAsync(tournament.TournamentId, cancellationToken);

            if (rounds == null) return stat;

            foreach (var round in rounds)
            {
                var pairings = (await _dataLayer.GetPairingsAsync(round.RoundId, cancellationToken)).Where(w => w.Player1 == playerId || w.Player2 == playerId);

                if (!pairings.Any()) continue;

                var results = (await _dataLayer.GetPairingResultsAsync(round.RoundId, cancellationToken)).Where(w => pairings.Any(a => a.PairingId == w.PairingId));

                foreach (var pairing in pairings)
                {
                    var result = results.FirstOrDefault(w => w.PairingId == pairing.PairingId);

                    if (result == null) continue;

                    if (result.WinningPlayerId == playerId)
                    {
                        stat.Wins += result.Wins;
                        stat.Losses += result.Losses;
                    }
                    else if (result.Draw)
                    {
                        stat.Wins += result.Wins;
                        stat.Losses += result.Losses;
                    }
                    else
                    {
                        stat.Wins += result.Losses;
                        stat.Losses += result.Wins;
                    }
                }
            }

            return stat;
        }
    }
}
