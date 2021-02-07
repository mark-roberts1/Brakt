using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt
{
    public class TournamentEntry : Validatable
    {
        public int PlayerId { get; set; }
        public int TournamentId { get; set; }

        public override void Validate()
        {
            TournamentId.ThrowIfDefault(nameof(TournamentId));
            PlayerId.ThrowIfDefault(nameof(PlayerId));
        }
    }
}
