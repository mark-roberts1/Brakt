using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brakt.Rest.Data
{
    internal class Migrations
    {
        internal const string MIGRATION_TABLE = @"
            CREATE TABLE IF NOT EXISTS Migration (
                MigrationId INTEGER NOT NULL PRIMARY KEY
            ) WITHOUT ROWID;
        ";

        internal const string HAS_MIGRATION_APPLIED = @"SELECT COUNT(*) FROM Migration WHERE MigrationId = $id;";

        internal const int INITIAL_DB_STATE_ID = 1;

        internal const string INITIAL_DB_STATE = @"
            CREATE TABLE IF NOT EXISTS Player (
                PlayerId INTEGER NOT NULL PRIMARY KEY,
                Username TEXT NOT NULL,
                DiscordDiscriminator TEXT NOT NULL,
                DiscordId INTEGER NOT NULL
            );

            CREATE UNIQUE INDEX Player_DiscordId ON Player(DiscordId);

            CREATE TABLE IF NOT EXISTS PlayerGroup (
                GroupId INTEGER NOT NULL PRIMARY KEY,
                GroupName TEXT NOT NULL,
                DiscordDiscriminator TEXT NOT NULL,
                DiscordId INTEGER NOT NULL
            );

            CREATE UNIQUE INDEX PlayerGroup_DiscordId ON PlayerGroup(DiscordId);

            CREATE TABLE IF NOT EXISTS PlayerGroupMembership (
                PlayerId INTEGER NOT NULL,
                GroupId INTEGER NOT NULL,
                IsAdmin INTEGER NULL,
                IsOwner INTEGER NULL,
                IsActive INTEGER NOT NULL,
                PRIMARY KEY (PlayerId, GroupId),
                FOREIGN KEY (PlayerId) REFERENCES Player (PlayerId),
                FOREIGN KEY (GroupId) REFERENCES PlayerGroup (GroupId)
            ) WITHOUT ROWID;

            CREATE TABLE IF NOT EXISTS Tournament (
                TournamentId INTEGER NOT NULL PRIMARY KEY,
                Name TEXT NOT NULL,
                GroupId INTEGER NOT NULL,
                BracketType INTEGER NOT NULL,
                StartDate BLOB NOT NULL,
                Completed INTEGER NOT NULL,
                NumberOfRounds INTEGER NULL,
                FOREIGN KEY (GroupId) REFERENCES PlayerGroup (GroupId)
            );

            CREATE INDEX Tournament_GroupId ON Tournament(GroupId);

            CREATE TABLE IF NOT EXISTS TournamentWinner (
                TournamentId INT NOT NULL,
                PlayerId INT NOT NULL,
                PRIMARY KEY (TournamentId, PlayerId),
                FOREIGN KEY (PlayerId) REFERENCES Player (PlayerId),
                FOREIGN KEY (TournamentId) REFERENCES Tournament (TournamentId) ON DELETE CASCADE ON UPDATE NO ACTION
            ) WITHOUT ROWID;

            CREATE TABLE IF NOT EXISTS TournamentEntry (
                TournamentId INTEGER NOT NULL,
                PlayerId INTEGER NOT NULL,
                PRIMARY KEY (TournamentId, PlayerId),
                FOREIGN KEY (TournamentId) REFERENCES Tournament (TournamentId) ON DELETE CASCADE ON UPDATE NO ACTION,
                FOREIGN KEY (PlayerId) REFERENCES Player (PlayerId)
            ) WITHOUT ROWID;

            CREATE TABLE IF NOT EXISTS TournamentRound (
                RoundId INTEGER NOT NULL PRIMARY KEY,
                TournamentId INTEGER NOT NULL,
                RoundNumber INTEGER NOT NULL,
                FOREIGN KEY (TournamentId) REFERENCES Tournament (TournamentId) ON DELETE CASCADE ON UPDATE NO ACTION
            );

            CREATE INDEX TournamentRound_TournamentId ON TournamentRound(TournamentId);

            CREATE TABLE IF NOT EXISTS Pairing (
                PairingId INTEGER NOT NULL PRIMARY KEY,
                Player1 INTEGER NOT NULL,
                Player2 INTEGER NOT NULL,
                RoundId INTEGER NOT NULL,
                Concluded INTEGER NOT NULL,
                FOREIGN KEY (Player1) REFERENCES Player (PlayerId),
                FOREIGN KEY (Player2) REFERENCES Player (PlayerId),
                FOREIGN KEY (RoundId) REFERENCES TournamentRound (RoundId) ON DELETE CASCADE ON UPDATE NO ACTION
            );

            CREATE INDEX Pairing_RoundId ON Pairing(RoundId);

            CREATE TABLE IF NOT EXISTS PairingResult (
                PairingId INTEGER NOT NULL PRIMARY KEY,
                WinningPlayerId INTEGER NULL,
                Draw INTEGER NOT NULL,
                Wins INTEGER NOT NULL,
                Losses INTEGER NOT NULL,
                FOREIGN KEY (PairingId) REFERENCES Pairing (PairingId) ON DELETE CASCADE ON UPDATE NO ACTION,
                FOREIGN KEY (WinningPlayerId) REFERENCES Player (PlayerId)
            ) WITHOUT ROWID;

            INSERT INTO Migration ( MigrationId ) VALUES ( 1 );
        ";
    }
}
