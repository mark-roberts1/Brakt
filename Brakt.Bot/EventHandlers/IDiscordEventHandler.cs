using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brakt.Bot.EventHandlers
{
    public interface IDiscordEventHandler
    {
        Task HandleAsync(MessageCreateEventArgs args);
        Task HandleAsync(GuildUpdateEventArgs args);
        Task HandleAsync(GuildCreateEventArgs args);
        Task HandleAsync(GuildBanRemoveEventArgs args);
        Task HandleAsync(GuildBanAddEventArgs args);
        Task HandleAsync(GuildMemberRemoveEventArgs args);
        Task HandleAsync(MessageReactionRemoveEventArgs args);
        Task HandleAsync(MessageReactionAddEventArgs args);
        Task HandleAsync(GuildMembersChunkEventArgs args);
        Task HandleAsync(UserUpdateEventArgs args);
        Task HandleAsync(GuildMemberAddEventArgs args);
        Task HandleAsync(UserSettingsUpdateEventArgs args);
        Task HandleAsync(MessageUpdateEventArgs args);
        Task HandleAsync(GuildMemberUpdateEventArgs args);
    }
}
