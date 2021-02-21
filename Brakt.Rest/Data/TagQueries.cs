using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Brakt.Rest.Data
{
    internal class TagQueries
    {
        internal const string SELECT_BY_VALUE = @"
            SELECT
                TagId,
                TagValue
            FROM
                Tag
            WHERE
                TagValue = $tagValue;
        ";

        internal const string SELECT_BY_ID = @"
            SELECT
                TagId,
                TagValue
            FROM
                Tag
            WHERE
                TagId = $tagId;
        ";

        internal const string SELECT_BY_TOURNAMENTID = @"
            SELECT
                Tag.TagId,
                Tag.TagValue
            FROM
                Tag
                INNER JOIN TournamentTag
                    ON Tag.TagId = TournamentTag.TagId
            WHERE
                TournamentTag.TournamentId = $tournamentId;
        ";

        internal const string INSERT = @"INSERT INTO Tag (TagValue) VALUES ($tagValue);";

        internal const string INSERT_TOURNAMENTTAG = @"INSERT INTO TournamentTag (TournamentId, TagId) VALUES ($tournamentId, $tagId);";

        internal const string DELETE_TOURNAMENTTAG_TOURNAMENT = @"DELETE FROM TournamentTag WHERE TournamentId = $tournamentId;";

        internal static Func<IDataReader, Tag> TagMapper => reader => new Tag
        {
            TagId = reader.GetInt32(reader.GetOrdinal("TagId")),
            TagValue = reader.GetString(reader.GetOrdinal("TagValue"))
        };
    }
}
