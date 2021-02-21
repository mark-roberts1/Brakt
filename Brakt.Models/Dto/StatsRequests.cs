using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt
{
    public class PlayerStatsRequest
    {
        public int PlayerId { get; set; }
        public List<string> Tags { get; set; }
    }

    public class GroupStatsRequest
    {
        public int GroupId { get; set; }
        public List<string> Tags { get; set; }
    }
}
