using Brakt.Rest.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Rest.Controllers
{
    [Route("api/round"), ApiController]
    public class RoundCountroller : ControllerBase
    {
        private readonly IDataLayer _dataLayer;

        public RoundCountroller(IDataLayer dataLayer)
        {
            _dataLayer = dataLayer;
        }

        [HttpGet("{id}")]
        public async Task<Round> GetRoundAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            return await _dataLayer.GetRoundAsync(id, cancellationToken);
        }

        [HttpGet("{tournamentId}/{roundNumber}")]
        public async Task<Round> GetRoundAsync([FromRoute] int tournamentId, [FromRoute] int roundNumber, CancellationToken cancellationToken)
        {
            return (await _dataLayer.GetRoundsAsync(tournamentId, cancellationToken)).FirstOrDefault(w => w.RoundNumber == roundNumber);
        }

        [HttpGet("{id}/pairings")]
        public async Task<IEnumerable<Pairing>> GetPairingsAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            return await _dataLayer.GetPairingsAsync(id, cancellationToken);
        }

        [HttpGet("{id}/results")]
        public async Task<IEnumerable<PairingResult>> GetRoundResultsAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            return await _dataLayer.GetPairingResultsAsync(id, cancellationToken);
        }

        [HttpPut("{id}/report-result")]
        public async Task<Round> ReportPairingResultAsync([FromRoute] int id, [FromBody] PairingResult result, CancellationToken cancellationToken)
        {
            id.ThrowIfDefault(nameof(id));
            result.ThrowIfNull(nameof(result));
            result.Validate();

            var round = await _dataLayer.GetRoundAsync(id, cancellationToken);

            var pairing = await _dataLayer.GetPairingAsync(result.PairingId, cancellationToken);

            pairing.ThrowIfNull(nameof(pairing));
            pairing.ThrowIf(p => $"Pairing {p.PairingId} not a member of round {id}.", p => p.RoundId != id);

            await _dataLayer.AddPairingResultAsync(result, cancellationToken);

            await _dataLayer.SetPairingConcludedAsync(pairing.PairingId, concluded: true, cancellationToken);

            round.Complete = !(await _dataLayer.GetPairingsAsync(id, cancellationToken)).Any(w => !w.Concluded);

            return round;
        }
    }
}
