﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Rest.Logic
{
    public interface IStatsGenerator
    {
        Task<IEnumerable<Statistic>> GenerateGroupStatsAsync(int groupId, CancellationToken cancellationToken);
        Task<IEnumerable<Statistic>> GeneratePlayerStatsAsync(int playerId, CancellationToken cancellationToken);
        Task<Statistic> GenerateStatsAsync(int playerId, int groupId, CancellationToken cancellationToken);
        Task<Statistic> GetTournamentStatsAsync(int playerId, Tournament tournament, CancellationToken cancellationToken);
    }
}
