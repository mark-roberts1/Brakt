using Brakt.Rest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Rest.Logic
{
    public abstract class FacilitatorBase
    {
        protected IDataLayer DataLayer { get; }
        protected IStatsGenerator StatsGenerator { get; }

        protected FacilitatorBase(IDataLayer dataLayer, IStatsGenerator statsGenerator)
        {
            DataLayer = dataLayer;
            StatsGenerator = statsGenerator;
        }

        private const string NON_ROUND_NUMBER_OF_PLAYERS_ERR = "Bye must be inserted before generating pairings.";

        protected IEnumerable<Pairing> RandomizePairings(IEnumerable<Player> players, int roundId)
        {
            if (players.Count() % 2 != 0) throw new ArgumentException(NON_ROUND_NUMBER_OF_PLAYERS_ERR);

            var pairings = new List<Pairing>();
            var randomized = players.Shuffle().ToArray();

            for (int i = 0; i < randomized.Length - 1; i += 2)
            {
                var p1 = randomized[i];
                var p2 = randomized[i + 1];

                pairings.Add(new Pairing
                {
                    RoundId = roundId,
                    Player1 = p1.PlayerId,
                    Player2 = p2.PlayerId,
                    Concluded = false
                });
            }

            return pairings;
        }

        protected IEnumerable<Pairing> GenerateTieredPairings(IEnumerable<Statistic> stats, int roundId)
        {
            if (stats.Count() % 2 != 0) throw new ArgumentException(NON_ROUND_NUMBER_OF_PLAYERS_ERR);

            var pairings = new List<Pairing>();

            var orderedStats = stats.OrderByDescending(ob => ob.Wins).ThenBy(tb => tb.Losses).ToArray();

            for (int i = 0; i < orderedStats.Length - 1; i += 2)
            {
                var p1 = orderedStats[i];
                var p2 = orderedStats[i + 1];

                pairings.Add(new Pairing
                {
                    Player1 = p1.PlayerId,
                    Player2 = p2.PlayerId,
                    RoundId = roundId,
                    Concluded = false
                });
            }

            return pairings;
        }

        protected IEnumerable<Pairing> GenerateSeededPairings(IEnumerable<Statistic> stats, int roundId)
        {
            var orderedStats = stats.OrderByDescending(ob => ob.Wins).ThenBy(tb => tb.Losses).ToArray();

            if (orderedStats.Length % 2 != 0) throw new ArgumentException(NON_ROUND_NUMBER_OF_PLAYERS_ERR);

            int pairingCount = orderedStats.Length / 2;
            
            var pairings = new List<Pairing>();

            for (int i = 0; i < pairingCount; i++)
            {
                int k = orderedStats.Length - i - 1;

                var p1 = orderedStats[i];
                var p2 = orderedStats[k];

                pairings.Add(new Pairing
                {
                    Player1 = p1.PlayerId,
                    Player2 = p2.PlayerId,
                    RoundId = roundId,
                    Concluded = false
                });
            }

            return pairings;
        }

        public virtual async Task<IEnumerable<TournamentWinner>> ChooseWinnersAsync(int tournamentId, CancellationToken cancellationToken)
        {
            var tournament = await DataLayer.GetTournamentAsync(tournamentId, cancellationToken);
            var entries = await DataLayer.GetTournamentEntriesAsync(tournamentId, cancellationToken);
            var stats = new List<Statistic>();

            foreach (var entry in entries)
                stats.Add(await StatsGenerator.GetTournamentStatsAsync(entry.PlayerId, tournament, cancellationToken));

            int maxWins = stats.Max(m => m.Wins);
            int minLosses = stats.Where(w => w.Wins == maxWins).Min(m => m.Losses);

            return stats.Where(w => w.Wins == maxWins && w.Losses == minLosses).Select(s => new TournamentWinner
            {
                PlayerId = s.PlayerId,
                TournamentId = tournamentId
            });
        }
    }
}
