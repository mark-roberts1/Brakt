using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brakt.Bot.Identification
{
    public class IdContext
    {
        public IdContext(Group group)
        {
            Group = group;
            Players = new List<Player>();
            GroupMembers = new List<GroupMember>();
        }

        public IdContext(Group group, IEnumerable<Player> players, IEnumerable<GroupMember> groupMembers)
        {
            Group = group;
            Players = players ?? new List<Player>();
            GroupMembers = groupMembers ?? new List<GroupMember>();
        }

        public IdContext(Group group, GroupMember groupMember, Player player)
        {
            Group = group;
            Players = new List<Player>() { player };
            GroupMembers = new List<GroupMember> { groupMember };
        }

        public IdContext(Player player)
        {
            Group = null;
            GroupMembers = new List<GroupMember>();
            Players = Players = new List<Player>() { player };
        }

        public Group Group { get; }
        public GroupMember GroupMember => GroupMembers.SingleOrDefault();
        public Player Player => Players.SingleOrDefault();
        public IEnumerable<Player> Players { get; }
        public IEnumerable<GroupMember> GroupMembers { get; }

        public bool IsPlayerContext => Player != null;
        public bool IsGroupMemberContext => GroupMember != null;
        public bool IsGroupContext => Group != null;
        public bool IsBulkPlayerContext => Players.Count() > 1;
        public bool IsBulkGroupMemberContext => GroupMembers.Count() > 1;
    }
}
