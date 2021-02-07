using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt.Bot.Identification
{
    public class IdContext
    {
        public IdContext(Group group)
        {
            Group = group;
            GroupMember = null;
            Player = null;
        }

        public IdContext(Group group, GroupMember groupMember, Player player)
        {
            Group = group;
            GroupMember = groupMember;
            Player = player;
        }

        public IdContext(Player player)
        {
            Player = player;
            Group = null;
            GroupMember = null;
        }

        public Group Group { get; }
        public GroupMember GroupMember { get; }
        public Player Player { get; }
        public bool IsPlayerContext => Player != null;
        public bool IsGroupMemberContext => GroupMember != null;
        public bool IsGroupContext => Group != null;
    }
}
