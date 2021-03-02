using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Brakt.Rest.Data
{
    internal class RoundQueries
    {
        internal const string INSERT = @"
            INSERT INTO TournamentRound (
                TournamentId,
                RoundNumber
            )
            VALUES (
                $tournamentId,
                $roundNumber
            );
        ";

        internal const string SELECT = @"
            SELECT
                RoundId,
                TournamentId,
                RoundNumber
            FROM
                TournamentRound
            WHERE
                RoundId = $roundId;
        ";

        internal const string DELETE = @"DELETE FROM TournamentRound WHERE RoundId = $roundId;";

        internal const string SELECT_BYTOURNAMENTID = @"
            SELECT
                RoundId,
                TournamentId,
                RoundNumber
            FROM
                TournamentRound
            WHERE
                TournamentId = $tournamentId;
        ";

        internal const string SELECT_PAIRINGS = @"
            SELECT
                PairingId,
                Player1,
                Player2,
                RoundId,
                Concluded
            FROM
                Pairing
            WHERE
                RoundId = $roundId;
        ";

        internal const string SELECT_PAIRING = @"
            SELECT
                PairingId,
                Player1,
                Player2,
                RoundId,
                Concluded
            FROM
                Pairing
            WHERE
                PairingId = $pairingId;
        ";

        internal const string SET_PAIRING_CONCLUDED = @"
            UPDATE
                Pairing
            SET
                Concluded = $concluded
            WHERE
                PairingId = $pairingId;
        ";

        internal const string SELECT_RESULTS = @"
            SELECT
                pr.PairingId,
                pr.WinningPlayerId,
                pr.Draw,
                pr.Wins,
                pr.Losses
            FROM
                Pairing p
                INNER JOIN PairingResult pr
                    ON p.PairingId = pr.PairingId
            WHERE
                p.RoundId = $roundId;
        ";

        internal const string INSERT_PAIRING = @"
            INSERT INTO Pairing (
                Player1,
                Player2,
                RoundId,
                Concluded
            )
            VALUES (
                $player1,
                $player2,
                $roundId,
                $concluded
            );
        ";

        internal const string INSERT_PAIRINGRESULT = @"
            INSERT INTO PairingResult (
                PairingId,
                WinningPlayerId,
                Draw,
                Wins,
                Losses
            )
            VALUES (
                $pairingId,
                $winningPlayerId,
                $draw,
                $wins,
                $losses
            );
        ";

        internal static Func<IDataReader, Round> RoundDataMapper => reader =>
        {
            return new Round
            {
                RoundId = reader.GetInt32(reader.GetOrdinal("RoundId")),
                RoundNumber = reader.GetInt32(reader.GetOrdinal("RoundNumber")),
                TournamentId = reader.GetInt32(reader.GetOrdinal("TournamentId"))
            };
        };

        internal static Func<IDataReader, PairingResult> PairingResultDataMapper => reader =>
        {
            return new PairingResult
            {
                PairingId = reader.GetInt32(reader.GetOrdinal("PairingId")),
                WinningPlayerId = reader.IsDBNull(reader.GetOrdinal("WinningPlayerId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("WinningPlayerId")),
                Wins = reader.GetInt32(reader.GetOrdinal("Wins")),
                Losses = reader.GetInt32(reader.GetOrdinal("Losses")),
                Draw = reader.GetByte(reader.GetOrdinal("Draw")).ToBool()
            };
        };

        internal static Func<IDataReader, Pairing> PairingDataMapper => reader =>
        {
            return new Pairing
            {
                PairingId = reader.GetInt32(reader.GetOrdinal("PairingId")),
                Player1 = reader.GetInt32(reader.GetOrdinal("Player1")),
                Player2 = reader.GetInt32(reader.GetOrdinal("Player2")),
                Concluded = reader.GetByte(reader.GetOrdinal("Concluded")).ToBool(),
                RoundId = reader.GetInt32(reader.GetOrdinal("RoundId"))
            };
        };
    }
}
