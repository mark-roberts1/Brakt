using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Rest.Data
{
    public interface IDataLayer
    {
        Task RunMigrationsAsync(CancellationToken cancellationToken);
        Task<bool> HasMigrationAppliedAsync(int migrationId, CancellationToken cancellationToken);
        Task<Group> GetGroupAsync(int groupId, CancellationToken cancellationToken);
        Task<Group> GetGroupAsync(long discordId, CancellationToken cancellationToken);
        Task AddGroupAsync(CreateGroupRequest group, CancellationToken cancellationToken);
        Task<IEnumerable<GroupMember>> GetGroupMembersAsync(int groupId, CancellationToken cancellationToken);
        Task<IEnumerable<GroupMember>> GetGroupMembersForPlayerAsync(int playerId, CancellationToken cancellationToken);
        Task<GroupMember> GetMemberAsync(int groupId, int playerId, CancellationToken cancellationToken);
        Task SetMemberIsActiveAsync(int groupId, int playerId, bool isActive, CancellationToken cancellationToken);
        Task SetMemberIsAdminAsync(int groupId, int playerId, bool isAdmin, CancellationToken cancellationToken);
        Task SetMemberIsOwnerAsync(int groupId, int playerId, bool isOwner, CancellationToken cancellationToken);
        Task<IEnumerable<Player>> GetGroupPlayersAsync(int groupId, CancellationToken cancellationToken);
        Task AddGroupMemberAsync(GroupMember member, CancellationToken cancellationToken);
        Task<Player> GetPlayerAsync(long discordId, CancellationToken cancellationToken);
        Task<Player> GetPlayerAsync(int playerId, CancellationToken cancellationToken);
        Task AddPlayerAsync(Player player, CancellationToken cancellationToken);
        Task CreateTournamentRoundAsync(int tournamentId, int roundNumber, CancellationToken cancellationToken);
        Task<Round> GetRoundAsync(int roundId, CancellationToken cancellationToken);
        Task DeleteRoundAsync(int roundId, CancellationToken cancellationToken);
        Task<IEnumerable<Round>> GetRoundsAsync(int tournamentId, CancellationToken cancellationToken);
        Task<IEnumerable<Pairing>> GetPairingsAsync(int roundId, CancellationToken cancellationToken);
        Task<Pairing> GetPairingAsync(int pairingId, CancellationToken cancellationToken);
        Task<IEnumerable<PairingResult>> GetPairingResultsAsync(int roundId, CancellationToken cancellationToken);
        Task AddPairingAsync(Pairing pairing, CancellationToken cancellationToken);
        Task SetPairingConcludedAsync(int pairingId, bool concluded, CancellationToken cancellationToken);
        Task AddPairingResultAsync(PairingResult result, CancellationToken cancellationToken);
        Task<Tournament> GetTournamentAsync(int tournamentId, CancellationToken cancellationToken);
        Task<IEnumerable<Tournament>> GetTournamentsAsync(int groupId, CancellationToken cancellationToken);
        Task DeleteTournamentAsync(int tournamentId, CancellationToken cancellationToken);
        Task AddTournamentAsync(CreateTournamentRequest request, CancellationToken cancellationToken);
        Task UpdateTournamentAsync(Tournament tournament, CancellationToken cancellationToken);
        Task AddTournamentEntryAsync(TournamentEntry tournamentEntry, CancellationToken cancellationToken);
        Task<IEnumerable<TournamentEntry>> GetTournamentEntriesAsync(int tournamentId, CancellationToken cancellationToken);
        Task DeleteTournamentEntryAsync(TournamentEntry tournamentEntry, CancellationToken cancellationToken);
        Task<IEnumerable<TournamentWinner>> GetTournamentWinnersAsync(int tournamentId, CancellationToken cancellationToken);
        Task AddTournamentWinnerAsync(TournamentWinner winner, CancellationToken cancellationToken);
        Task DeleteTournamentWinnerAsync(TournamentWinner winner, CancellationToken cancellationToken);
        Task<Tag> GetTagAsync(string value, CancellationToken cancellationToken);
        Task<Tag> GetTagAsync(int tagId, CancellationToken cancellationToken);
        Task<IEnumerable<Tag>> GetTagsAsync(int tournamentId, CancellationToken cancellationToken);
        Task<Tag> AddTagAsync(string value, CancellationToken cancellationToken);
        Task AddTournamentTagAsync(int tournamentId, int tagId, CancellationToken cancellationToken);
        Task DeleteTournamentTagsAsync(int tournamentId, CancellationToken cancellationToken);
    }
}
