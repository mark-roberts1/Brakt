using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Bot.Formatters
{
    public interface IResponseFormatter
    {
        Task<string> FormatStatsAsync(IEnumerable<Statistic> stats, CancellationToken cancellationToken);
        Task<string> FormatAsLeaderboardAsync(IEnumerable<Statistic> stats, CancellationToken cancellationToken);
        Task<string> FormatRoundPairingsAsync(Round round, CancellationToken cancellationToken);
        Task<string> FormatTournamentWinnersAsync(IEnumerable<TournamentWinner> winners, CancellationToken cancellationToken);
        Task<string> FormatTournamentListAsync(IEnumerable<Tournament> tournaments, CancellationToken cancellationToken);
        Task<string> FormatTournamentResultsAsync(Tournament tournament, CancellationToken cancellationToken);
    }
}
