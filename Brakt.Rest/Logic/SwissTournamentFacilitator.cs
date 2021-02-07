using Brakt.Rest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Rest.Logic
{
    public class SwissTournamentFacilitator : FacilitatorBase, ITournamentFacilitator
    {
        public SwissTournamentFacilitator(IDataLayer dataLayer, IStatsGenerator statsGenerator)
            : base (dataLayer, statsGenerator)
        {
        }

        public async Task<int> FigureRoundCountAsync(int tournamentId, CancellationToken cancellationToken)
        {
            var entries = await DataLayer.GetTournamentEntriesAsync(tournamentId, cancellationToken);
            int count = entries.Count();

            if (count < 5)
                throw new ArgumentException("Cannot fire swiss tournament with less than 5 players.");
            else if (count < 9)
                return 3;
            else if (count < 17)
                return 4;
            else if (count < 33)
                return 5;
            else if (count < 65)
                return 6;
            else if (count < 129)
                return 7;
            else if (count < 213)
                return 8;
            else if (count < 386)
                return 9;

            return 10; // JESUS
        }

        public async Task<IEnumerable<Pairing>> GeneratePairingsAsync(int tournamentId, int roundNumber, CancellationToken cancellationToken)
        {
            var tournament = await DataLayer.GetTournamentAsync(tournamentId, cancellationToken);

            await DataLayer.CreateTournamentRoundAsync(tournamentId, roundNumber, cancellationToken);

            var round = (await DataLayer.GetRoundsAsync(tournamentId, cancellationToken)).Single(w => w.RoundNumber == roundNumber);

            var entries = await DataLayer.GetTournamentEntriesAsync(tournamentId, cancellationToken);
            
            var players = new List<Player>();

            foreach (var entry in entries)
                players.Add(await DataLayer.GetPlayerAsync(entry.PlayerId, cancellationToken));

            var playerStats = new List<Statistic>();

            foreach (var entry in entries)
            {
                playerStats.Add(await StatsGenerator.GetTournamentStatsAsync(entry.PlayerId, tournament, cancellationToken));
            }

            if (players.Count % 2 != 0)
            {
                players.Add(Player.Bye);

                playerStats.Add(new Statistic
                {
                    PlayerId = Player.Bye.PlayerId,
                    GroupId = tournament.GroupId,
                    Losses = 0,
                    Wins = 0,
                    TournamentWins = 0
                });
            }

            var totalWins = playerStats.Sum(s => s.Wins);

            if (totalWins == 0)
            {
                return RandomizePairings(players, round.RoundId);
            }

            return base.GenerateTieredPairings(playerStats, round.RoundId);
        }
    }
}
