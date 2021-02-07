using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brakt
{
    public class Round : Validatable
    {
        public int RoundId { get; set; }
        public int TournamentId { get; set; }
        public int RoundNumber { get; set; }
        public bool Complete { get; set; }

        public override void Validate()
        {
            RoundId.ThrowIfDefault(nameof(RoundId));
            TournamentId.ThrowIfDefault(nameof(TournamentId));
            RoundNumber.ThrowIfDefault(nameof(RoundNumber));
        }
    }
}
