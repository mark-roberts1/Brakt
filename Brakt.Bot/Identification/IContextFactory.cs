using DSharpPlus.EventArgs;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Bot.Identification
{
    public interface IContextFactory
    {
        Task<IdContext> GetIdContextAsync(GuildEmojisUpdateEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(PresenceUpdateEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(MessageCreateEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(GuildDeleteEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(GuildUpdateEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(GuildCreateEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(GuildBanRemoveEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(GuildBanAddEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(ChannelPinsUpdateEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(GuildIntegrationsUpdateEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(GuildMemberRemoveEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(MessageReactionsClearEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(MessageReactionRemoveEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(MessageReactionAddEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(UnknownEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(GuildMembersChunkEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(VoiceServerUpdateEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(VoiceStateUpdateEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(UserUpdateEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(GuildMemberAddEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(UserSettingsUpdateEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(MessageBulkDeleteEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(MessageDeleteEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(MessageUpdateEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(MessageAcknowledgeEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(GuildRoleDeleteEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(GuildRoleUpdateEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(GuildRoleCreateEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(GuildMemberUpdateEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(TypingStartEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(DmChannelDeleteEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(ReadyEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(ChannelUpdateEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(ChannelDeleteEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(ClientErrorEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(SocketErrorEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(HeartbeatEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(WebhooksUpdateEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(ChannelCreateEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(DmChannelCreateEventArgs args, CancellationToken cancellationToken);
        Task<IdContext> GetIdContextAsync(SocketCloseEventArgs args, CancellationToken cancellationToken);
    }
}
