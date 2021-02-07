using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Brakt.Rest.Data
{
    internal class GroupQueries
    {
        internal const string SELECT_BY_GROUPID = @"
            SELECT
                GroupId,
                GroupName,
                DiscordDiscriminator,
                DiscordId
            FROM
                PlayerGroup
            WHERE
                GroupId = $groupId;
        ";

        internal const string SELECT_BY_DISCORDID = @"
            SELECT
                GroupId,
                GroupName,
                DiscordDiscriminator,
                DiscordId
            FROM
                PlayerGroup
            WHERE
                DiscordId = $discordId;
        ";

        internal const string INSERT = @"
            INSERT INTO PlayerGroup (
                GroupName,
                DiscordDiscriminator,
                DiscordId
            )
            VALUES (
                $groupName,
                $discordDiscriminator,
                $discordId
            );
        ";

        internal const string SELECT_MEMBERS = @"
            SELECT
                PlayerId,
                GroupId,
                IsAdmin,
                IsOwner,
                IsActive
            FROM
                PlayerGroupMembership
            WHERE
                GroupId = $groupId
                AND IsActive = 1;
        ";

        internal const string SELECT_MEMBER = @"
            SELECT
                PlayerId,
                GroupId,
                IsAdmin,
                IsOwner,
                IsActive
            FROM
                PlayerGroupMembership
            WHERE
                GroupId = $groupId
                AND PlayerId = $playerId;
        ";

        internal const string SET_ISACTIVE = @"
            UPDATE
                PlayerGroupMembership
            SET
                IsActive = $isActive
            WHERE
                GroupId = $groupId
                AND PlayerId = $playerId;
        ";

        internal const string SET_ADMIN = @"
            UPDATE
                PlayerGroupMembership
            SET
                IsAdmin = $isAdmin
            WHERE
                GroupId = $groupId
                AND PlayerId = $playerId;
        ";

        internal const string SET_OWNER = @"
            UPDATE
                PlayerGroupMembership
            SET
                IsOwner = $isOwner
            WHERE
                GroupId = $groupId
                AND PlayerId = $playerId;
        ";

        internal const string SELECT_PLAYERS = @"
            SELECT
                p.PlayerId,
                p.Username,
                p.DiscordDiscriminator,
                p.DiscordId
            FROM
                PlayerGroupMembership m
                INNER JOIN Player p
                    ON m.PlayerId = p.PlayerId
            WHERE
                m.GroupId = $groupId
                AND m.IsActive = 1;
        ";

        internal const string INSERT_MEMBER = @"
            INSERT INTO PlayerGroupMembership (
                PlayerId,
                GroupId,
                IsAdmin,
                IsOwner,
                IsActive
            )
            VALUES (
                $playerId,
                $groupId,
                $isAdmin,
                $isOwner,
                $isActive
            );
        ";

        internal static Func<IDataReader, Group> GroupDataMapper => reader =>
        {
            return new Group
            {
                GroupId = reader.GetInt32(reader.GetOrdinal("GroupId")),
                GroupName = reader.GetString(reader.GetOrdinal("GroupName")),
                DiscordDiscriminator = reader.GetString(reader.GetOrdinal("DiscordDiscriminator")),
                DiscordId = reader.GetInt64(reader.GetOrdinal("DiscordId"))
            };
        };

        internal static Func<IDataReader, GroupMember> GroupMemberDataMapper => reader =>
        {
            return new GroupMember
            {
                GroupId = reader.GetInt32(reader.GetOrdinal("GroupId")),
                PlayerId = reader.GetInt32(reader.GetOrdinal("PlayerId")),
                IsAdmin = reader.GetByte(reader.GetOrdinal("IsAdmin")).ToBool(),
                IsOwner = reader.GetByte(reader.GetOrdinal("IsOwner")).ToBool(),
                IsActive = reader.GetByte(reader.GetOrdinal("IsActive")).ToBool()
            };
        };
    }
}
