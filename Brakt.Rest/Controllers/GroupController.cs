using Brakt.Rest.Data;
using Brakt.Rest.Database;
using Brakt.Rest.Logic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Rest.Controllers
{
    [Route("api/group"), ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IDataLayer _dataLayer;
        private readonly IStatsGenerator _statsGenerator;

        public GroupController(IDataLayer dataLayer, IStatsGenerator statsGenerator)
        {
            _dataLayer = dataLayer;
            _statsGenerator = statsGenerator;
        }

        [HttpGet("{id}")]
        public async Task<Group> GetGroupAsync([FromRoute] int id, CancellationToken cancellationToken)
            => await _dataLayer.GetGroupAsync(id, cancellationToken);

        [HttpGet("discord/{discordId}")]
        public async Task<Group> GetDiscordGroupAsync([FromRoute] long discordId, CancellationToken cancellationToken)
            => await _dataLayer.GetGroupAsync(discordId, cancellationToken);

        [HttpPost]
        public async Task<Group> CreateGroupAsync([FromBody] CreateGroupRequest request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));
            request.Validate();

            await _dataLayer.AddGroupAsync(request, cancellationToken);
            var group = await _dataLayer.GetGroupAsync(request.DiscordId, cancellationToken);

            await _dataLayer.AddGroupMemberAsync(new GroupMember
            {
                GroupId = group.GroupId,
                PlayerId = request.OwnerId,
                IsActive = true,
                IsAdmin = true,
                IsOwner = true
            }, cancellationToken);

            return group;
        }

        [HttpPut("member")]
        public async Task AddMemberAsync([FromBody] GroupMember request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));
            request.Validate();

            if ((await _dataLayer.GetMemberAsync(request.GroupId, request.PlayerId, cancellationToken)) != null)
            {
                await _dataLayer.SetMemberIsActiveAsync(request.GroupId, request.PlayerId, isActive: true, cancellationToken);
                return;
            }

            await _dataLayer.AddGroupMemberAsync(request, cancellationToken);
        }

        [HttpGet("{id}/member")]
        public async Task<IEnumerable<GroupMember>> GetMembersAsync([FromRoute] int id, CancellationToken cancellationToken)
            => await _dataLayer.GetGroupMembersAsync(id, cancellationToken);

        [HttpGet("{id}/players")]
        public async Task<IEnumerable<Player>> GetPlayersAsync([FromRoute] int id, CancellationToken cancellationToken)
            => await _dataLayer.GetGroupPlayersAsync(id, cancellationToken);

        [HttpGet("{id}/member/{playerId}")]
        public async Task<GroupMember> GetMemberAsync([FromRoute] int id, [FromRoute] int playerId, CancellationToken cancellationToken)
            => await _dataLayer.GetMemberAsync(id, playerId, cancellationToken);

        [HttpDelete("{id}/member/{playerId}")]
        public async Task RemoveMemberAsync([FromRoute] int id, [FromRoute] int playerId, CancellationToken cancellationToken)
        {
            id.ThrowIfDefault(nameof(id));
            playerId.ThrowIfDefault(nameof(playerId));

            await _dataLayer.SetMemberIsActiveAsync(id, playerId, isActive: false, cancellationToken);
        }

        [HttpPut("{id}/member/{playerId}/set-admin")]
        public async Task SetGroupAdminAsync([FromRoute] int id, [FromRoute] int playerId, CancellationToken cancellationToken, [FromQuery] bool isAdmin = true)
        {
            id.ThrowIfDefault(nameof(id));
            playerId.ThrowIfDefault(nameof(playerId));

            await _dataLayer.SetMemberIsAdminAsync(id, playerId, isAdmin, cancellationToken);
        }

        [HttpPut("{id}/member/{playerId}/set-owner")]
        public async Task SetGroupOwnerAsync([FromRoute] int id, [FromRoute] int playerId, CancellationToken cancellationToken, [FromQuery] bool isOwner = true)
        {
            id.ThrowIfDefault(nameof(id));
            playerId.ThrowIfDefault(nameof(playerId));

            await _dataLayer.SetMemberIsOwnerAsync(id, playerId, isOwner, cancellationToken);
        }

        [HttpGet("{id}/tournaments")]
        public async Task<IEnumerable<Tournament>> GetTournamentsAsync([FromRoute] int groupId, CancellationToken cancellationToken)
        {
            return await _dataLayer.GetTournamentsAsync(groupId, cancellationToken);
        }

        [HttpGet("{id}/stats")]
        public async Task<IEnumerable<Statistic>> GetStatisticsAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            return await _statsGenerator.GenerateGroupStatsAsync(id, cancellationToken);
        }
    }
}
