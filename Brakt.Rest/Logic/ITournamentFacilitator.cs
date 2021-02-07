using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Rest.Logic
{
    public interface ITournamentFacilitator
    {
        Task<int> FigureRoundCountAsync(int tournamentId, CancellationToken cancellationToken);
        Task<IEnumerable<Pairing>> GeneratePairingsAsync(int tournamentId, int roundNumber, CancellationToken cancellationToken);
        Task<IEnumerable<TournamentWinner>> ChooseWinnersAsync(int tournamentId, CancellationToken cancellationToken);
    }
}
