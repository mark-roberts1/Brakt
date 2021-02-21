using Brakt.Rest.Data;
using Brakt.Rest.Logic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Rest.Controllers
{
    [Route("api/player"), ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IDataLayer _dataLayer;
        private readonly IStatsGenerator _statsGenerator;

        public PlayerController(IDataLayer dataLayer, IStatsGenerator statsGenerator)
        {
            _dataLayer = dataLayer;
            _statsGenerator = statsGenerator;
        }

        [HttpGet("discord/{discordId}")]
        public async Task<Player> GetDiscordPlayerAsync([FromRoute] long discordId, CancellationToken cancellationToken)
        {
            return await _dataLayer.GetPlayerAsync(discordId, cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<Player> GetPlayerAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            return await _dataLayer.GetPlayerAsync(id, cancellationToken);
        }

        [HttpPost]
        public async Task<Player> CreatePlayerAsync([FromBody] Player player, CancellationToken cancellationToken)
        {
            player.ThrowIfNull(nameof(player));
            player.Validate();

            await _dataLayer.AddPlayerAsync(player, cancellationToken);

            player = await _dataLayer.GetPlayerAsync(player.DiscordId, cancellationToken);

            return player;
        }

        [HttpGet("{id}/stats")]
        public async Task<IEnumerable<Statistic>> GetStatisticsAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            return await _statsGenerator.GeneratePlayerStatsAsync(id, cancellationToken);
        }

        [HttpPut("stats")]
        public async Task<IEnumerable<Statistic>> GetStatisticsAsync([FromBody] PlayerStatsRequest request, CancellationToken cancellationToken)
        {
            return await _statsGenerator.GeneratePlayerStatsAsync(request.PlayerId, cancellationToken, request.Tags);
        }

        [HttpGet("{id}/groups")]
        public async Task<IEnumerable<GroupMember>> GetMembershipsAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            return await _dataLayer.GetGroupMembersForPlayerAsync(id, cancellationToken);
        }
    }
}
