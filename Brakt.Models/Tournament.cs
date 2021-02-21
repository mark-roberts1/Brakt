using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt
{
    public class Tournament : Validatable
    {
        public int TournamentId { get; set; }
        public int GroupId { get; set; }
        public BracketType BracketType { get; set; }
        public DateTime StartDate { get; set; }
        public bool Completed { get; set; }
        public int? NumberOfRounds { get; set; }
        public List<Tag> Tags { get; set; }

        public override void Validate()
        {
            TournamentId.ThrowIfDefault(nameof(TournamentId));
            GroupId.ThrowIfDefault(nameof(GroupId));
            StartDate.ThrowIfDefault(nameof(StartDate));
        }
    }
}
