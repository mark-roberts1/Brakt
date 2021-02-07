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
        public async Task<IdContext> GetIdContextAsync(MessageCreateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(GuildUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(GuildCreateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(GuildBanRemoveEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(GuildBanAddEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(GuildMemberRemoveEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(MessageReactionsClearEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(MessageReactionRemoveEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(MessageReactionAddEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(GuildMembersChunkEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("Maybe need this one?");
        }

        public async Task<IdContext> GetIdContextAsync(UserUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(GuildMemberAddEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(UserSettingsUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(MessageUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(MessageAcknowledgeEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("Maybe needed?");
        }

        public async Task<IdContext> GetIdContextAsync(GuildMemberUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #region Not Implemented
        public async Task<IdContext> GetIdContextAsync(GuildEmojisUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(UnknownEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(PresenceUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(ChannelPinsUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(GuildIntegrationsUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(VoiceServerUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(VoiceStateUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(MessageBulkDeleteEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(MessageDeleteEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(GuildRoleDeleteEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(GuildRoleUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(GuildRoleCreateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(TypingStartEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(DmChannelDeleteEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(ReadyEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(ChannelUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(ChannelDeleteEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(ClientErrorEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(SocketErrorEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(HeartbeatEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(WebhooksUpdateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(ChannelCreateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(DmChannelCreateEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(SocketCloseEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdContext> GetIdContextAsync(GuildDeleteEventArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
