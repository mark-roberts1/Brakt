using System;
using System.Collections.Generic;
using System.Text;

namespace Brakt
{
    public class GroupMember : Validatable
    {
        public int PlayerId { get; set; }
        public int GroupId { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsOwner { get; set; }
        public bool IsActive { get; set; }

        public override void Validate()
        {
            PlayerId.ThrowIfDefault(nameof(PlayerId));
            GroupId.ThrowIfDefault(nameof(GroupId));
        }
    }
}
