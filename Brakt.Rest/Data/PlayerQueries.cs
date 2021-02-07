using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Brakt.Rest.Data
{
    internal class PlayerQueries
    {
        internal const string SELECT_BY_DISCORDID = @"
            SELECT
                PlayerId,
                Username,
                DiscordDiscriminator,
                DiscordId
            FROM
                Player
            WHERE
                DiscordId = $discordId;
        ";

        internal const string SELECT_BY_PLAYERID = @"
            SELECT
                PlayerId,
                Username,
                DiscordDiscriminator,
                DiscordId
            FROM
                Player
            WHERE
                PlayerId = $playerId;
        ";

        internal const string SELECT_GROUPMEMBERSHIPS = @"
            SELECT
                PlayerId,
                GroupId,
                IsAdmin,
                IsOwner,
                IsActive
            FROM
                PlayerGroupMembership
            WHERE
                PlayerId = $playerId
                AND IsActive = 1;
        ";

        internal const string INSERT = @"
            INSERT INTO Player (
                Username,
                DiscordDiscriminator,
                DiscordId
            )
            VALUES (
                $username,
                $discordDiscriminator,
                $discordId
            );
        ";

        internal static Func<IDataReader, Player> PlayerDataMapper => reader =>
        {
            return new Player
            {
                PlayerId = reader.GetInt32(reader.GetOrdinal("PlayerId")),
                Username = reader.GetString(reader.GetOrdinal("Username")),
                DiscordDiscriminator = reader.GetString(reader.GetOrdinal("DiscordDiscriminator")),
                DiscordId = reader.GetInt64(reader.GetOrdinal("DiscordId"))
            };
        };
    }
}
