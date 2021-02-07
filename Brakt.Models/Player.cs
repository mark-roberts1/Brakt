using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt
{
    public class Player : Validatable
    {
        private readonly static Player bye = new Player
        {
            PlayerId = 1,
            Username = "Bye",
            DiscordDiscriminator = "#0000",
            DiscordId = -1
        };

        public static Player Bye
        {
            get
            {
                return bye;
            }
        }

        public int PlayerId { get; set; }
        public string Username { get; set; }
        public string DiscordDiscriminator { get; set; }
        public long DiscordId { get; set; }

        public override void Validate()
        {
            Username.ThrowIfNull(nameof(Username));
            DiscordDiscriminator.ThrowIfNull(nameof(DiscordDiscriminator));
            DiscordId.ThrowIfDefault(nameof(DiscordId));
        }
    }
}
