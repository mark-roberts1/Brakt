using Brakt.Rest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Rest.Logic
{
    public class SingleElimTournamentFacilitator : FacilitatorBase, ITournamentFacilitator
    {
        public SingleElimTournamentFacilitator(IDataLayer dataLayer, IStatsGenerator statsGenerator) : base(dataLayer, statsGenerator)
        {
        }

        public async Task<int> FigureRoundCountAsync(int tournamentId, CancellationToken cancellationToken)
        {
            var entries = await DataLayer.GetTournamentEntriesAsync(tournamentId, cancellationToken);
            int count = entries.Count();

            if (count <= 1) throw new ArgumentException("Not enough players to fire tournament.");

            if (count % 2 != 0) count++;

            return (int)Math.Ceiling(Math.Log2(count));
        }

        public async Task<IEnumerable<Pairing>> GeneratePairingsAsync(int tournamentId, int roundNumber, CancellationToken cancellationToken)
        {
            var tournament = await DataLayer.GetTournamentAsync(tournamentId, cancellationToken);

            var previousRounds = await DataLayer.GetRoundsAsync(tournamentId, cancellationToken);

            await DataLayer.CreateTournamentRoundAsync(tournamentId, roundNumber, cancellationToken);

            var round = (await DataLayer.GetRoundsAsync(tournamentId, cancellationToken)).Single(w => w.RoundNumber == roundNumber);

            var entries = (await DataLayer.GetTournamentEntriesAsync(tournamentId, cancellationToken)).ToList();

            // check results of the previous rounds and eliminate players that lost.
            foreach (var previousRound in previousRounds)
            {
                var roundResults = await DataLayer.GetPairingResultsAsync(previousRound.RoundId, cancellationToken);
                var roundPairings = await DataLayer.GetPairingsAsync(previousRound.RoundId, cancellationToken);

                foreach (var result in roundResults)
                {
                    var pairing = roundPairings.First(w => w.PairingId == result.PairingId);

                    if (pairing.Player1 == Player.Bye.PlayerId || pairing.Player2 == Player.Bye.PlayerId) continue;

                    if (pairing.Player1 != result.WinningPlayerId) entries.RemoveAll(w => w.PlayerId == pairing.Player1);

                    if (pairing.Player2 != result.WinningPlayerId) entries.RemoveAll(w => w.PlayerId == pairing.Player2);
                }
            }

            var players = new List<Player>();
            var stats = new List<Statistic>();

            foreach (var entry in entries)
            {
                stats.Add(await StatsGenerator.GetTournamentStatsAsync(entry.PlayerId, tournament, cancellationToken));
                players.Add(await DataLayer.GetPlayerAsync(entry.PlayerId, cancellationToken));
            }

            if (stats.Count % 2 != 0)
            {
                players.Add(Player.Bye);
                stats.Add(new Statistic
                {
                    GroupId = tournament.GroupId,
                    PlayerId = Player.Bye.PlayerId,
                    Wins = 0,
                    Losses = 0,
                    TournamentWins = 0
                });
            }

            if (stats.Sum(s => s.Wins) == 0)
            {
                return RandomizePairings(players, round.RoundId);
            }

            return GenerateSeededPairings(stats, round.RoundId);
        }
    }
}
