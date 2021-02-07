using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt
{
    public class Pairing : Validatable
    {
        public int PairingId { get; set; }
        public int Player1 { get; set; }
        public int Player2 { get; set; }
        public int RoundId { get; set; }
        public bool Concluded { get; set; }

        public override void Validate()
        {
            PairingId.ThrowIfDefault(nameof(PairingId));
            Player1.ThrowIfDefault(nameof(Player1));
            Player2.ThrowIfDefault(nameof(Player2));
            RoundId.ThrowIfDefault(nameof(RoundId));
        }
    }
}
