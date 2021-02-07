using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt
{
    public class CreateTournamentRequest : Validatable
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public BracketType? BracketType { get; set; }

        public override void Validate()
        {
            Name.ThrowIfNull(nameof(Name));
            GroupId.ThrowIfDefault(nameof(GroupId));
            StartDate.ThrowIfDefault(nameof(StartDate));
        }
    }
}
