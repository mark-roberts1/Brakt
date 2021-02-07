using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Brakt.Rest.Data
{
    internal class TournamentQueries
    {
        internal const string SELECT = @"
            SELECT
                TournamentId,
                Name,
                GroupId,
                BracketType,
                StartDate,
                Completed,
                NumberOfRounds
            FROM
                Tournament
            WHERE
                TournamentId = $tournamentId;
        ";

        internal const string SELECT_BYGROUPID = @"
            SELECT
                TournamentId,
                Name,
                GroupId,
                BracketType,
                StartDate,
                Completed,
                NumberOfRounds
            FROM
                Tournament
            WHERE
                GroupId = $groupId;
        ";

        internal const string DELETE = @"DELETE FROM Tournament WHERE TournamentId = $tournamentId;";

        internal const string INSERT = @"
            INSERT INTO Tournament (
                Name,
                GroupId,
                BracketType,
                StartDate,
                Completed,
                NumberOfRounds
            )
            VALUES (
                $name,
                $groupId,
                $bracketType,
                $startDate,
                $completed,
                $numberOfRounds
            );
        ";

        internal const string UPDATE = @"
            UPDATE
                Tournament
            SET
                Name = $name,
                GroupId = $groupId,
                BracketType = $bracketType,
                StartDate = $startDate,
                Completed = $completed,
                NumberOfRounds = $numberOfRounds
            WHERE
                TournamentId = $tournamentId;
        ";

        internal const string INSERT_ENTRY = @"
            INSERT INTO TournamentEntry (
                TournamentId,
                PlayerId
            )
            VALUES (
                $tournamentId,
                $playerId
            );
        ";

        internal const string SELECT_ENTRIES = @"
            SELECT
                TournamentId,
                PlayerId
            FROM
                TournamentEntry
            WHERE
                TournamentId = $tournamentId;
        ";

        internal const string DELETE_ENTRY = @"
            DELETE
            FROM
                TournamentEntry
            WHERE
                TournamentId = $tournamentId
                AND PlayerId = $playerId;
        ";

        internal const string SELECT_TOURNAMENTWINNERS = @"
            SELECT
                TournamentId,
                PlayerId
            FROM
                TournamentWinner
            WHERE
                TournamentId = $tournamentId;
        ";

        internal const string INSERT_TOURNAMENTWINNER = @"
            INSERT INTO TournamentWinner (
                TournamentId,
                PlayerId
            )
            VALUES (
                $tournamentId,
                $playerId
            );
        ";

        internal const string DELETE_TOURNAMENTWINNER = @"
            DELETE
            FROM
                TournamentWinner
            WHERE
                TournamentId = $tournamentId
                AND PlayerId = $playerId;
        ";

        internal static Func<IDataReader, Tournament> TournamentDataMapper => reader =>
        {
            return new Tournament
            {
                TournamentId = reader.GetInt32(reader.GetOrdinal("TournamentId")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                StartDate = (reader["StartDate"] as byte[]).FromBlob<DateTime>(),
                BracketType = (BracketType)reader.GetInt32(reader.GetOrdinal("BracketType")),
                Completed = reader.GetByte(reader.GetOrdinal("Completed")).ToBool(),
                GroupId = reader.GetInt32(reader.GetOrdinal("GroupId")),
                NumberOfRounds = reader.IsDBNull(reader.GetOrdinal("NumberOfRounds")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("NumberOfRounds"))
            };
        };

        internal static Func<IDataReader, TournamentEntry> TournamentEntryDataMapper => reader =>
        {
            return new TournamentEntry
            {
                TournamentId = reader.GetInt32(reader.GetOrdinal("TournamentId")),
                PlayerId = reader.GetInt32(reader.GetOrdinal("PlayerId"))
            };
        };

        internal static Func<IDataReader, TournamentWinner> TournamentWinnerDataMapper => reader =>
        {
            return new TournamentWinner
            {
                TournamentId = reader.GetInt32(reader.GetOrdinal("TournamentId")),
                PlayerId = reader.GetInt32(reader.GetOrdinal("PlayerId"))
            };
        };
    }
}
