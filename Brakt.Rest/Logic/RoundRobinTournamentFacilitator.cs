using Brakt.Rest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Rest.Logic
{
    public class RoundRobinTournamentFacilitator : FacilitatorBase, ITournamentFacilitator
    {
        public RoundRobinTournamentFacilitator(IDataLayer dataLayer, IStatsGenerator statsGenerator) : base(dataLayer, statsGenerator)
        {
        }

        public async Task<int> FigureRoundCountAsync(int tournamentId, CancellationToken cancellationToken)
        {
            var entries = await DataLayer.GetTournamentEntriesAsync(tournamentId, cancellationToken);
            int count = entries.Count();
            
            if (count <= 1) throw new ArgumentException("Not enough players to fire tournament.");

            return count - 1;
        }

        public async Task<IEnumerable<Pairing>> GeneratePairingsAsync(int tournamentId, int roundNumber, CancellationToken cancellationToken)
        {
            var tournament = await DataLayer.GetTournamentAsync(tournamentId, cancellationToken);

            await DataLayer.CreateTournamentRoundAsync(tournamentId, roundNumber, cancellationToken);

            var round = (await DataLayer.GetRoundsAsync(tournamentId, cancellationToken)).Single(w => w.RoundNumber == roundNumber);

            var entries = await DataLayer.GetTournamentEntriesAsync(tournamentId, cancellationToken);

            var ordered = entries.OrderBy(ob => ob.PlayerId).ToList();

            int count = ordered.Count;
            var pairings = new List<Pairing>();

            for (int i = 0; i < count; i++)
            {
                var p1 = ordered[i];
                var p2 = ordered[(i + roundNumber) % count];

                if (pairings.Any(w => w.Player1 == p1.PlayerId || w.Player2 == p2.PlayerId)) continue;

                pairings.Add(new Pairing
                {
                    Player1 = p1.PlayerId,
                    Player2 = p2.PlayerId,
                    RoundId = round.RoundId,
                    Concluded = false
                });
            }

            return pairings;
        }
    }
}
