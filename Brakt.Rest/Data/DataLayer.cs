using Brakt.Rest.Database;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Rest.Data
{
    public class DataLayer : IDataLayer
    {
        private readonly string _connectionString;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDbCommandFactory _commandFactory;
        private readonly ICommandExecutor _executor;

        public DataLayer(
            IConfiguration configuration,
            IDbConnectionFactory connectionFactory,
            IDbCommandFactory commandFactory,
            ICommandExecutor executor)
        {
            _connectionString = configuration.GetConnectionString("BraktDb");
            _connectionFactory = connectionFactory;
            _commandFactory = commandFactory;
            _executor = executor;
        }

        public async Task AddGroupAsync(CreateGroupRequest group, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                GroupQueries.INSERT,
                CommandType.Text,
                connection,
                DbParameter.From("$groupName", group.GroupName),
                DbParameter.From("$discordDiscriminator", group.DiscordDiscriminator),
                DbParameter.From("$discordId", group.DiscordId));

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task AddGroupMemberAsync(GroupMember member, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                GroupQueries.INSERT_MEMBER,
                CommandType.Text,
                connection,
                DbParameter.From("$playerId", member.PlayerId),
                DbParameter.From("$groupId", member.GroupId),
                DbParameter.From("$isAdmin", member.IsAdmin.ToBit()),
                DbParameter.From("$isOwner", member.IsOwner.ToBit()),
                DbParameter.From("$isActive", member.IsActive.ToBit()));

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task AddPairingAsync(Pairing pairing, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                RoundQueries.INSERT_PAIRING,
                CommandType.Text,
                connection,
                DbParameter.From("$pairingId", pairing.PairingId),
                DbParameter.From("$player1", pairing.Player1),
                DbParameter.From("$player2", pairing.Player2),
                DbParameter.From("$roundId", pairing.RoundId),
                DbParameter.From("$concluded", pairing.Concluded.ToBit()));

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task AddPairingResultAsync(PairingResult result, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                RoundQueries.INSERT_PAIRINGRESULT,
                CommandType.Text,
                connection,
                DbParameter.From("$pairingId", result.PairingId),
                DbParameter.From("$winningPlayerId", result.WinningPlayerId.DbNullIfNull()),
                DbParameter.From("$draw", result.Draw.ToBit()),
                DbParameter.From("$wins", result.Wins),
                DbParameter.From("$losses", result.Losses));

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task AddPlayerAsync(Player player, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                PlayerQueries.INSERT,
                CommandType.Text,
                connection,
                DbParameter.From("$username", player.Username),
                DbParameter.From("$discordDiscriminator", player.DiscordDiscriminator),
                DbParameter.From("$discordId", player.DiscordId));

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task AddTournamentAsync(CreateTournamentRequest request, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                TournamentQueries.INSERT,
                CommandType.Text,
                connection,
                DbParameter.From("$name", request.Name),
                DbParameter.From("$groupId", request.GroupId),
                DbParameter.From("$bracketType", (int)(request.BracketType ?? BracketType.Swiss)),
                DbParameter.From("$startDate", request.StartDate.ToBlob()),
                DbParameter.From("$completed", false.ToBit()),
                DbParameter.From("$numberOfRounds", DBNull.Value));

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task AddTournamentEntryAsync(TournamentEntry tournamentEntry, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                TournamentQueries.INSERT_ENTRY,
                CommandType.Text,
                connection,
                DbParameter.From("$tournamentId", tournamentEntry.TournamentId),
                DbParameter.From("$playerId", tournamentEntry.PlayerId));

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task AddTournamentWinnerAsync(TournamentWinner winner, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                TournamentQueries.INSERT_TOURNAMENTWINNER,
                CommandType.Text,
                connection,
                DbParameter.From("$tournamentId", winner.TournamentId),
                DbParameter.From("$playerId", winner.PlayerId));

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task CreateTournamentRoundAsync(int tournamentId, int roundNumber, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                RoundQueries.INSERT,
                CommandType.Text,
                connection,
                DbParameter.From("$tournamentId", tournamentId),
                DbParameter.From("$roundNumber", roundNumber));

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task DeleteRoundAsync(int roundId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                RoundQueries.DELETE,
                CommandType.Text,
                connection,
                DbParameter.From("$roundId", roundId));

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task DeleteTournamentAsync(int tournamentId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                TournamentQueries.DELETE,
                CommandType.Text,
                connection,
                DbParameter.From("$tournamentId", tournamentId));

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task DeleteTournamentEntryAsync(TournamentEntry tournamentEntry, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                TournamentQueries.DELETE_ENTRY,
                CommandType.Text,
                connection,
                DbParameter.From("$tournamentId", tournamentEntry.TournamentId),
                DbParameter.From("$playerId", tournamentEntry.PlayerId));

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task DeleteTournamentWinnerAsync(TournamentWinner winner, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                TournamentQueries.DELETE_TOURNAMENTWINNER,
                CommandType.Text,
                connection,
                DbParameter.From("$tournamentId", winner.TournamentId),
                DbParameter.From("$playerId", winner.PlayerId));

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task<Group> GetGroupAsync(int groupId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                GroupQueries.SELECT_BY_GROUPID,
                CommandType.Text,
                connection,
                DbParameter.From("$groupId", groupId));

            return await _executor.ExecuteSingleAsync(command, GroupQueries.GroupDataMapper, cancellationToken);
        }

        public async Task<Group> GetGroupAsync(long discordId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                GroupQueries.SELECT_BY_GROUPID,
                CommandType.Text,
                connection,
                DbParameter.From("$discordId", discordId));

            return await _executor.ExecuteSingleAsync(command, GroupQueries.GroupDataMapper, cancellationToken);
        }

        public async Task<IEnumerable<GroupMember>> GetGroupMembersAsync(int groupId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                GroupQueries.SELECT_MEMBERS,
                CommandType.Text,
                connection,
                DbParameter.From("$groupId", groupId));

            return await _executor.ExecuteAsync(command, GroupQueries.GroupMemberDataMapper, cancellationToken);
        }

        public async Task<IEnumerable<GroupMember>> GetGroupMembersForPlayerAsync(int playerId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                PlayerQueries.SELECT_GROUPMEMBERSHIPS,
                CommandType.Text,
                connection,
                DbParameter.From("$playerId", playerId));

            return await _executor.ExecuteAsync(command, GroupQueries.GroupMemberDataMapper, cancellationToken);
        }

        public async Task<IEnumerable<Player>> GetGroupPlayersAsync(int groupId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                GroupQueries.SELECT_PLAYERS,
                CommandType.Text,
                connection,
                DbParameter.From("$groupId", groupId));

            return await _executor.ExecuteAsync(command, PlayerQueries.PlayerDataMapper, cancellationToken);
        }

        public async Task<GroupMember> GetMemberAsync(int groupId, int playerId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                GroupQueries.SELECT_MEMBER,
                CommandType.Text,
                connection,
                DbParameter.From("$groupId", groupId),
                DbParameter.From("$playerId", playerId));

            return await _executor.ExecuteSingleAsync(command, GroupQueries.GroupMemberDataMapper, cancellationToken);
        }

        public async Task<Pairing> GetPairingAsync(int pairingId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                RoundQueries.SELECT_PAIRING,
                CommandType.Text,
                connection,
                DbParameter.From("$pairingId", pairingId));

            return await _executor.ExecuteSingleAsync(command, RoundQueries.PairingDataMapper, cancellationToken);
        }

        public async Task<IEnumerable<PairingResult>> GetPairingResultsAsync(int roundId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                RoundQueries.SELECT_RESULTS,
                CommandType.Text,
                connection,
                DbParameter.From("$roundId", roundId));

            return await _executor.ExecuteAsync(command, RoundQueries.PairingResultDataMapper, cancellationToken);
        }

        public async Task<IEnumerable<Pairing>> GetPairingsAsync(int roundId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                RoundQueries.SELECT_PAIRINGS,
                CommandType.Text,
                connection,
                DbParameter.From("$roundId", roundId));

            return await _executor.ExecuteAsync(command, RoundQueries.PairingDataMapper, cancellationToken);
        }

        public async Task<Player> GetPlayerAsync(long discordId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                PlayerQueries.SELECT_BY_DISCORDID,
                CommandType.Text,
                connection,
                DbParameter.From("$discordId", discordId));

            return await _executor.ExecuteSingleAsync(command, PlayerQueries.PlayerDataMapper, cancellationToken);
        }

        public async Task<Player> GetPlayerAsync(int playerId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                PlayerQueries.SELECT_BY_PLAYERID,
                CommandType.Text,
                connection,
                DbParameter.From("$playerId", playerId));

            return await _executor.ExecuteSingleAsync(command, PlayerQueries.PlayerDataMapper, cancellationToken);
        }

        public async Task<Round> GetRoundAsync(int roundId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                RoundQueries.SELECT,
                CommandType.Text,
                connection,
                DbParameter.From("$roundId", roundId));

            return await _executor.ExecuteSingleAsync(command, RoundQueries.RoundDataMapper, cancellationToken);
        }

        public async Task<IEnumerable<Round>> GetRoundsAsync(int tournamentId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                RoundQueries.SELECT_BYTOURNAMENTID,
                CommandType.Text,
                connection,
                DbParameter.From("$tournamentId", tournamentId));

            return await _executor.ExecuteAsync(command, RoundQueries.RoundDataMapper, cancellationToken);
        }

        public async Task<Tournament> GetTournamentAsync(int tournamentId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                TournamentQueries.SELECT,
                CommandType.Text,
                connection,
                DbParameter.From("$tournamentId", tournamentId));

            return await _executor.ExecuteSingleAsync(command, TournamentQueries.TournamentDataMapper, cancellationToken);
        }

        public async Task<IEnumerable<TournamentEntry>> GetTournamentEntriesAsync(int tournamentId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                TournamentQueries.SELECT_ENTRIES,
                CommandType.Text,
                connection,
                DbParameter.From("$tournamentId", tournamentId));

            return await _executor.ExecuteAsync(command, TournamentQueries.TournamentEntryDataMapper, cancellationToken);
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsAsync(int groupId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                TournamentQueries.SELECT_BYGROUPID,
                CommandType.Text,
                connection,
                DbParameter.From("$groupId", groupId));

            return await _executor.ExecuteAsync(command, TournamentQueries.TournamentDataMapper, cancellationToken);
        }

        public async Task<IEnumerable<TournamentWinner>> GetTournamentWinnersAsync(int tournamentId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                TournamentQueries.SELECT_TOURNAMENTWINNERS,
                CommandType.Text,
                connection,
                DbParameter.From("$tournamentId", tournamentId));

            return await _executor.ExecuteAsync(command, TournamentQueries.TournamentWinnerDataMapper, cancellationToken);
        }

        public async Task<bool> HasMigrationAppliedAsync(int migrationId, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(Migrations.HAS_MIGRATION_APPLIED, CommandType.Text, connection, DbParameter.From("$id", migrationId));

            return (await _executor.ExecuteScalarAsync<int>(command, cancellationToken)) > 0;
        }

        public async Task RunMigrationsAsync(CancellationToken cancellationToken)
        {
            await EnsureMigrationTableExistsAsync(cancellationToken);

            if (!(await HasMigrationAppliedAsync(Migrations.INITIAL_DB_STATE_ID, cancellationToken)))
            {
                using var connection = _connectionFactory.BuildConnection(_connectionString);
                using var command = _commandFactory.BuildCommand(Migrations.INITIAL_DB_STATE, CommandType.Text, connection);

                await connection.OpenAsync(cancellationToken);
                await command.ExecuteNonQueryAsync(cancellationToken);

                await AddPlayerAsync(Player.Bye, cancellationToken);
            }
        }

        private async Task EnsureMigrationTableExistsAsync(CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(Migrations.MIGRATION_TABLE, CommandType.Text, connection);

            await connection.OpenAsync(cancellationToken);
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task SetMemberIsActiveAsync(int groupId, int playerId, bool isActive, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                GroupQueries.SET_ISACTIVE,
                CommandType.Text,
                connection,
                DbParameter.From("$isActive", isActive.ToBit()),
                DbParameter.From("$groupId", groupId),
                DbParameter.From("$playerId", playerId));

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task SetMemberIsAdminAsync(int groupId, int playerId, bool isAdmin, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                GroupQueries.SET_ADMIN,
                CommandType.Text,
                connection,
                DbParameter.From("$isAdmin", isAdmin.ToBit()),
                DbParameter.From("$groupId", groupId),
                DbParameter.From("$playerId", playerId));

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task SetMemberIsOwnerAsync(int groupId, int playerId, bool isOwner, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                GroupQueries.SET_OWNER,
                CommandType.Text,
                connection,
                DbParameter.From("$isOwner", isOwner.ToBit()),
                DbParameter.From("$groupId", groupId),
                DbParameter.From("$playerId", playerId));

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task SetPairingConcludedAsync(int pairingId, bool concluded, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                RoundQueries.SET_PAIRING_CONCLUDED,
                CommandType.Text,
                connection,
                DbParameter.From("$concluded", concluded.ToBit()),
                DbParameter.From("$pairingId", pairingId));

            await connection.OpenAsync(cancellationToken);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task UpdateTournamentAsync(Tournament tournament, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.BuildConnection(_connectionString);
            using var command = _commandFactory.BuildCommand(
                TournamentQueries.UPDATE,
                CommandType.Text,
                connection,
                DbParameter.From("$name", tournament.Name),
                DbParameter.From("$groupId", tournament.GroupId),
                DbParameter.From("$bracketType", (int)tournament.BracketType),
                DbParameter.From("$startDate", tournament.StartDate.ToBlob()),
                DbParameter.From("$completed", tournament.Completed.ToBit()),
                DbParameter.From("$numberOfRounds", (object)tournament.NumberOfRounds ?? DBNull.Value),
                DbParameter.From("$tournamentId", tournament.TournamentId));

            await connection.OpenAsync(cancellationToken);
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
