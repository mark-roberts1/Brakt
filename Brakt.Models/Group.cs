using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt
{
    public class Group : Validatable
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string DiscordDiscriminator { get; set; }
        public long DiscordId { get; set; }

        public override void Validate()
        {
            GroupId.ThrowIfDefault(nameof(GroupId));
            GroupName.ThrowIfNull(nameof(GroupName));
            DiscordDiscriminator.ThrowIfNull(nameof(DiscordDiscriminator));
            DiscordId.ThrowIfDefault(nameof(DiscordId));
        }
    }
}
