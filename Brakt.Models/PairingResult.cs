using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt
{
    public class PairingResult : Validatable
    {
        public int PairingId { get; set; }
        public int? WinningPlayerId { get; set; }
        public bool Draw { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public override void Validate()
        {
            PairingId.ThrowIfDefault(nameof(PairingId));
            if (!WinningPlayerId.HasValue && !Draw)
            {
                throw new ArgumentException("A result must have a winner, or be marked as a draw.");
            }

            if (Draw && Wins != Losses)
            {
                throw new ArgumentException("In the case of a draw, Wins must equal Losses.");
            }
        }
    }
}
