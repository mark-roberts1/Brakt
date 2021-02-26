using Brakt.Rest.Data;
using Brakt.Rest.Logic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Rest.Controllers
{
    [Route("api/tournament"), ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly IDataLayer _dataLayer;
        private readonly ITournamentFacilitatorFactory _tournamentFacilitatorFactory;

        public TournamentController(IDataLayer dataLayer, ITournamentFacilitatorFactory tournamentFacilitatorFactory)
        {
            _dataLayer = dataLayer;
            _tournamentFacilitatorFactory = tournamentFacilitatorFactory;
        }

        [HttpGet("{id}")]
        public async Task<Tournament> GetTournamentAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            return await _dataLayer.GetTournamentAsync(id, cancellationToken);
        }

        [HttpGet("{id}/rounds")]
        public async Task<IEnumerable<Round>> GetTournamentRoundsAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            return await _dataLayer.GetRoundsAsync(id, cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task DeleteTournamentAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            await _dataLayer.DeleteTournamentAsync(id, cancellationToken);
        }

        [HttpPost]
        public async Task<Tournament> CreateTournamentAsync([FromBody] CreateTournamentRequest request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));
            request.Validate();

            if (!_tournamentFacilitatorFactory.HasFacilitator(request.BracketType ?? BracketType.Swiss))
                throw new NotImplementedException($"{request.BracketType ?? BracketType.Swiss} has no associated facilitator.");

            var activeTournaments = await _dataLayer.GetTournamentsAsync(request.GroupId, cancellationToken);

            if (activeTournaments.Any(w => w.Tags.Select(s => s.TagValue).IsEquivalentTo(request.Tags) && w.StartDate == request.StartDate))
                throw new ArgumentException("A tournament with the same tags is slated for the same time.");

            await _dataLayer.AddTournamentAsync(request, cancellationToken);

            activeTournaments = await _dataLayer.GetTournamentsAsync(request.GroupId, cancellationToken);

            var tournament = activeTournaments.First(w => w.StartDate == request.StartDate);

            foreach (var val in request.Tags)
            {
                var tag = await _dataLayer.GetTagAsync(val, cancellationToken);

                if (tag == null)
                {
                    tag = await _dataLayer.AddTagAsync(val, cancellationToken);
                }

                await _dataLayer.AddTournamentTagAsync(tournament.TournamentId, tag.TagId, cancellationToken);
            }

            return tournament;
        }

        [HttpPut]
        public async Task<Tournament> UpdateTournamentAsync([FromBody] Tournament tournament, CancellationToken cancellationToken)
        {
            tournament.ThrowIfNull(nameof(tournament));
            tournament.Validate();

            await _dataLayer.UpdateTournamentAsync(tournament, cancellationToken);

            return tournament;
        }

        [HttpPut("register")]
        public async Task RegisterPlayerAsync([FromBody] TournamentEntry entry, CancellationToken cancellationToken)
        {
            entry.ThrowIfNull(nameof(entry));
            entry.Validate();

            await _dataLayer.AddTournamentEntryAsync(entry, cancellationToken);
        }

        [HttpDelete("{id}/{playerId}")]
        public async Task RemovePlayerAsync([FromRoute] int id, [FromRoute] int playerId, CancellationToken cancellationToken)
        {
            await _dataLayer.DeleteTournamentEntryAsync(new TournamentEntry { PlayerId = playerId, TournamentId = id }, cancellationToken);
        }

        [HttpPut("{id}/start")]
        public async Task<Round> StartTournamentAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var tournament = await _dataLayer.GetTournamentAsync(id, cancellationToken);

            tournament.ThrowIfNull(nameof(tournament));

            var rounds = await _dataLayer.GetRoundsAsync(id, cancellationToken);

            if (rounds != null && rounds.Any()) throw new ArgumentException("The tournament has already started. Use the advance method to go to the next round.");

            var facilitator = _tournamentFacilitatorFactory.GetTournamentFacilitator(tournament.BracketType);

            tournament.NumberOfRounds = await facilitator.FigureRoundCountAsync(id, cancellationToken);

            await _dataLayer.UpdateTournamentAsync(tournament, cancellationToken);

            await facilitator.GeneratePairingsAsync(id, 1, cancellationToken);

            return (await _dataLayer.GetRoundsAsync(id, cancellationToken)).Single(w => w.RoundNumber == 1);
        }

        [HttpPut("{id}/advance")]
        public async Task<Round> AdvanceAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var tournament = await _dataLayer.GetTournamentAsync(id, cancellationToken);

            tournament.ThrowIfNull(nameof(tournament));

            var rounds = await _dataLayer.GetRoundsAsync(id, cancellationToken);

            if (!rounds.Any()) 
                throw new ArgumentException("The tournament has not started. Use the start method to start the tournament.");

            var nextRoundNumber = rounds.Max(m => m.RoundNumber) + 1;

            if (nextRoundNumber > tournament.NumberOfRounds) 
                throw new ArgumentException("The tournament has already completed the number of rounds. Use the complete method to wrap up the tournament.");

            var facilitator = _tournamentFacilitatorFactory.GetTournamentFacilitator(tournament.BracketType);

            await facilitator.GeneratePairingsAsync(id, nextRoundNumber, cancellationToken);

            return (await _dataLayer.GetRoundsAsync(id, cancellationToken)).Single(w => w.RoundNumber == nextRoundNumber);
        }

        [HttpPut("{id}/complete")]
        public async Task<IEnumerable<TournamentWinner>> CompleteTournamentAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var tournament = await _dataLayer.GetTournamentAsync(id, cancellationToken);

            tournament.ThrowIfNull(nameof(tournament));

            var facilitator = _tournamentFacilitatorFactory.GetTournamentFacilitator(tournament.BracketType);

            var winners = await facilitator.ChooseWinnersAsync(id, cancellationToken);

            tournament.Completed = true;

            foreach (var winner in winners)
                await _dataLayer.AddTournamentWinnerAsync(winner, cancellationToken);

            await _dataLayer.UpdateTournamentAsync(tournament, cancellationToken);

            return winners;
        }

        [HttpGet("{id}/players")]
        public async Task<IEnumerable<Player>> GetContestantsAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var entries = await _dataLayer.GetTournamentEntriesAsync(id, cancellationToken);

            var players = new List<Player>();

            foreach (var entry in entries)
            {
                players.Add(await _dataLayer.GetPlayerAsync(entry.PlayerId, cancellationToken));
            }

            return players;
        }
    }
}
