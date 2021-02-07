using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt
{
    public class TournamentWinner : Validatable
    {
        public int TournamentId { get; set; }
        public int PlayerId { get; set; }

        public override void Validate()
        {
            TournamentId.ThrowIfDefault(nameof(TournamentId));
            PlayerId.ThrowIfDefault(nameof(PlayerId));
        }
    }
}
