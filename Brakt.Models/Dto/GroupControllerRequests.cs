using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt
{
    public class CreateGroupRequest : Validatable
    {
        public string GroupName { get; set; }
        public int OwnerId { get; set; }
        public string DiscordDiscriminator { get; set; }
        public long DiscordId { get; set; }

        public override void Validate()
        {
            GroupName.ThrowIfNull(nameof(GroupName));
            DiscordDiscriminator.ThrowIfNull(nameof(DiscordDiscriminator));
            DiscordId.ThrowIfDefault(nameof(DiscordId));
            OwnerId.ThrowIfDefault(nameof(OwnerId));
        }
    }
}
