using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt
{
    public class CreateTournamentRequest : Validatable
    {
        public int GroupId { get; set; }
        public DateTime StartDate { get; set; }
        public BracketType? BracketType { get; set; }
        public List<string> Tags { get; set; }

        public override void Validate()
        {
            GroupId.ThrowIfDefault(nameof(GroupId));
            StartDate.ThrowIfDefault(nameof(StartDate));
            Tags
                .ThrowIfNull(nameof(Tags))
                .ThrowIf(t => $"A tournament must have at least one tag.", t => t.Count == 0);
        }
    }
}
