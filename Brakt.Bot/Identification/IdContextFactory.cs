using Brakt.Client;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Bot.Identification
{
    public class IdContextFactory : IContextFactory
    {
        private readonly IBraktApiClient _client;

        public IdContextFactory(IBraktApiClient client)
        {
            _client = client;
        }

        public async Task<IdContext> GetIdContextAsync(MessageCreateEventArgs args, CancellationToken cancellationToken)
        {
            var group = await GetGroupCreateIfNecessaryAsync(args.Guild, cancellationToken);

            var player = await GetPlayerCreateIfNecessaryAsync(args.Author, cancellationToken);

            var member = await GetGroupMemberCreateIfNecessaryAsync(group.GroupId, player.PlayerId, cancellationToken);

            return new IdContext(group, member, player);
        }

        public async Task<IdContext> GetIdContextAsync(GuildUpdateEventArgs args, CancellationToken cancellationToken)
        {
            var group = await GetGroupCreateIfNecessaryAsync(args.Guild, cancellationToken);

            return new IdContext(group);
        }

        public async Task<IdContext> GetIdContextAsync(GuildCreateEventArgs args, CancellationToken cancellationToken)
        {
            var group = await GetGroupCreateIfNecessaryAsync(args.Guild, cancellationToken);

            return new IdContext(group);
        }

        public async Task<IdContext> GetIdContextAsync(GuildBanAddEventArgs args, CancellationToken cancellationToken)
        {
            var group = await GetGroupCreateIfNecessaryAsync(args.Guild, cancellationToken);

            var player = await GetPlayerCreateIfNecessaryAsync(args.Member, cancellationToken);

            var member = await GetGroupMemberCreateIfNecessaryAsync(group.GroupId, player.PlayerId, cancellationToken);

            return new IdContext(group, member, player);
        }

        public async Task<IdContext> GetIdContextAsync(GuildBanRemoveEventArgs args, CancellationToken cancellationToken)
        {
            var group = await GetGroupCreateIfNecessaryAsync(args.Guild, cancellationToken);

            var player = await GetPlayerCreateIfNecessaryAsync(args.Member, cancellationToken);

            var member = await GetGroupMemberCreateIfNecessaryAsync(group.GroupId, player.PlayerId, cancellationToken);

            return new IdContext(group, member, player);
        }

        public async Task<IdContext> GetIdContextAsync(GuildMemberRemoveEventArgs args, CancellationToken cancellationToken)
        {
            var group = await GetGroupCreateIfNecessaryAsync(args.Guild, cancellationToken);

            var player = await GetPlayerCreateIfNecessaryAsync(args.Member, cancellationToken);

            var member = await GetGroupMemberCreateIfNecessaryAsync(group.GroupId, player.PlayerId, cancellationToken);

            return new IdContext(group, member, player);
        }

        public async Task<IdContext> GetIdContextAsync(MessageReactionRemoveEventArgs args, CancellationToken cancellationToken)
        {
            var group = await GetGroupCreateIfNecessaryAsync(args.Channel.Guild, cancellationToken);

            var player = await GetPlayerCreateIfNecessaryAsync(args.User, cancellationToken);

            var member = await GetGroupMemberCreateIfNecessaryAsync(group.GroupId, player.PlayerId, cancellationToken);

            return new IdContext(group, member, player);
        }

        public async Task<IdContext> GetIdContextAsync(MessageReactionAddEventArgs args, CancellationToken cancellationToken)
        {
            var group = await GetGroupCreateIfNecessaryAsync(args.Channel.Guild, cancellationToken);

            var player = await GetPlayerCreateIfNecessaryAsync(args.User, cancellationToken);

            var member = await GetGroupMemberCreateIfNecessaryAsync(group.GroupId, player.PlayerId, cancellationToken);

            return new IdContext(group, member, player);
        }

        public async Task<IdContext> GetIdContextAsync(GuildMembersChunkEventArgs args, CancellationToken cancellationToken)
        {
            var group = await GetGroupCreateIfNecessaryAsync(args.Guild, cancellationToken);

            var players = new List<Player>();
            var members = new List<GroupMember>();

            foreach (var member in args.Members)
            {
                var player = await GetPlayerCreateIfNecessaryAsync(member, cancellationToken);
                var groupMember = await GetGroupMemberCreateIfNecessaryAsync(group.GroupId, player.PlayerId, cancellationToken);

                players.Add(player);
                members.Add(groupMember);
            }

            return new IdContext(group, players, members);
        }

        public async Task<IdContext> GetIdContextAsync(UserUpdateEventArgs args, CancellationToken cancellationToken)
        {
            var player = await GetPlayerCreateIfNecessaryAsync(args.UserAfter, cancellationToken);

            return new IdContext(player);
        }

        public async Task<IdContext> GetIdContextAsync(GuildMemberAddEventArgs args, CancellationToken cancellationToken)
        {
            var group = await GetGroupCreateIfNecessaryAsync(args.Guild, cancellationToken);

            var player = await GetPlayerCreateIfNecessaryAsync(args.Member, cancellationToken);

            var member = await GetGroupMemberCreateIfNecessaryAsync(group.GroupId, player.PlayerId, cancellationToken);

            return new IdContext(group, member, player);
        }

        public async Task<IdContext> GetIdContextAsync(UserSettingsUpdateEventArgs args, CancellationToken cancellationToken)
        {
            var player = await GetPlayerCreateIfNecessaryAsync(args.User, cancellationToken);

            return new IdContext(player);
        }

        public async Task<IdContext> GetIdContextAsync(MessageUpdateEventArgs args, CancellationToken cancellationToken)
        {
            var group = await GetGroupCreateIfNecessaryAsync(args.Guild, cancellationToken);

            var player = await GetPlayerCreateIfNecessaryAsync(args.Author, cancellationToken);

            var member = await GetGroupMemberCreateIfNecessaryAsync(group.GroupId, player.PlayerId, cancellationToken);

            return new IdContext(group, member, player);
        }

        public async Task<IdContext> GetIdContextAsync(GuildMemberUpdateEventArgs args, CancellationToken cancellationToken)
        {
            var group = await GetGroupCreateIfNecessaryAsync(args.Guild, cancellationToken);

            var player = await GetPlayerCreateIfNecessaryAsync(args.Member, cancellationToken);

            var member = await GetGroupMemberCreateIfNecessaryAsync(group.GroupId, player.PlayerId, cancellationToken);

            return new IdContext(group, member, player);
        }

        private async Task<Group> GetGroupCreateIfNecessaryAsync(DiscordGuild discordGroup, CancellationToken cancellationToken)
        {
            var owner = await GetPlayerCreateIfNecessaryAsync(discordGroup.Owner, cancellationToken);

            var group = await _client.GetDiscordGroupAsync((long)discordGroup.Id, cancellationToken);

            if (group == null)
            {
                return await _client.CreateGroupAsync(new CreateGroupRequest
                {
                    DiscordId = (long)discordGroup.Id,
                    DiscordDiscriminator = discordGroup.Owner.Discriminator,
                    GroupName = discordGroup.Name,
                    OwnerId = owner.PlayerId
                }, cancellationToken);
            }

            return group;
        }

        private async Task<Player> GetPlayerCreateIfNecessaryAsync(DiscordUser discordUser, CancellationToken cancellationToken)
        {
            var player = await _client.GetDiscordPlayerAsync((long)discordUser.Id, cancellationToken);

            if (player == null)
            {
                return await _client.CreatePlayerAsync(new Player
                {
                    DiscordDiscriminator = discordUser.Discriminator,
                    DiscordId = (long)discordUser.Id,
                    Username = discordUser.Username
                }, cancellationToken);
            }

            return player;
        }

        private async Task<GroupMember> GetGroupMemberCreateIfNecessaryAsync(int groupId, int playerId, CancellationToken cancellationToken)
        {
            var member = await _client.GetMemberAsync(groupId, playerId, cancellationToken);

            if (member == null)
            {
                member = new GroupMember
                {
                    GroupId = groupId,
                    PlayerId = playerId,
                    IsActive = true,
                    IsAdmin = false,
                    IsOwner = false
                };

                await _client.AddMemberAsync(member, cancellationToken);
            }

            return member;
        }

        #region Not Implemented
        public Task<IdContext> GetIdContextAsync(GuildEmojisUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(MessageAcknowledgeEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(UnknownEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(MessageReactionsClearEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(PresenceUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(ChannelPinsUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(GuildIntegrationsUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(VoiceServerUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(VoiceStateUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(MessageBulkDeleteEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(MessageDeleteEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(GuildRoleDeleteEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(GuildRoleUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(GuildRoleCreateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(TypingStartEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(DmChannelDeleteEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(ReadyEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(ChannelUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(ChannelDeleteEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(ClientErrorEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(SocketErrorEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(HeartbeatEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(WebhooksUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(ChannelCreateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(DmChannelCreateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(SocketCloseEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdContext> GetIdContextAsync(GuildDeleteEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
