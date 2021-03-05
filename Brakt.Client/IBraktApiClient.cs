using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Client
{
    public interface IBraktApiClient
    {
        Task<Group> GetGroupAsync(int groupId, CancellationToken cancellationToken);
        Task<Group> GetDiscordGroupAsync(long discordId, CancellationToken cancellationToken);
        Task<Group> CreateGroupAsync(CreateGroupRequest request, CancellationToken cancellationToken);
        Task AddMemberAsync(GroupMember request, CancellationToken cancellationToken);
        Task<IEnumerable<GroupMember>> GetMembersAsync(int groupId, CancellationToken cancellationToken);
        Task<IEnumerable<Player>> GetPlayersAsync(int groupId, CancellationToken cancellationToken);
        Task<GroupMember> GetMemberAsync(int groupId, int playerId, CancellationToken cancellationToken);
        Task RemoveMemberAsync(int groupId, int playerId, CancellationToken cancellationToken);
        Task SetGroupAdminAsync(int groupId, int playerId, CancellationToken cancellationToken, bool isAdmin = true);
        Task SetGroupOwnerAsync(int groupId, int playerId, CancellationToken cancellationToken, bool isOwner = true);
        Task<IEnumerable<Tournament>> GetTournamentsAsync(int groupId, CancellationToken cancellationToken);
        Task<IEnumerable<Round>> GetTournamentRoundsAsync(int tournamentId, CancellationToken cancellationToken);
        Task<IEnumerable<Statistic>> GetGroupStatisticsAsync(int groupId, CancellationToken cancellationToken);
        Task<IEnumerable<Statistic>> GetGroupStatisticsAsync(GroupStatsRequest request, CancellationToken cancellationToken);
        Task<Statistic> GetGroupStatisticsAsync(int playerId, GroupStatsRequest request, CancellationToken cancellationToken);
        Task<Player> GetDiscordPlayerAsync(long discordId, CancellationToken cancellationToken);
        Task<Player> GetPlayerAsync(int playerId, CancellationToken cancellationToken);
        Task<Player> CreatePlayerAsync(Player player, CancellationToken cancellationToken);
        Task<IEnumerable<Statistic>> GetPlayerStatisticsAsync(int playerId, CancellationToken cancellationToken);
        Task<IEnumerable<Statistic>> GetPlayerStatisticsAsync(PlayerStatsRequest request, CancellationToken cancellationToken);
        Task<IEnumerable<GroupMember>> GetMembershipsAsync(int playerId, CancellationToken cancellationToken);
        Task<Round> GetRoundAsync(int roundId, CancellationToken cancellationToken);
        Task<Round> GetRoundAsync(int tournamentId, int roundNumber, CancellationToken cancellationToken);
        Task<IEnumerable<Pairing>> GetPairingsAsync(int roundId, CancellationToken cancellationToken);
        Task<IEnumerable<PairingResult>> GetRoundResultsAsync(int roundId, CancellationToken cancellationToken);
        Task<Round> ReportPairingResultAsync(int roundId, PairingResult result, CancellationToken cancellationToken);
        Task<Tournament> GetTournamentAsync(int tournamentId, CancellationToken cancellationToken);
        Task<IEnumerable<TournamentWinner>> GetTournamentWinnersAsync(int tournamentId, CancellationToken cancellationToken);
        Task DeleteTournamentAsync(int tournamentId, CancellationToken cancellationToken);
        Task<Tournament> CreateTournamentAsync(CreateTournamentRequest request, CancellationToken cancellationToken);
        Task<Tournament> UpdateTournamentAsync(Tournament tournament, CancellationToken cancellationToken);
        Task RegisterPlayerAsync(TournamentEntry entry, CancellationToken cancellationToken);
        Task<IEnumerable<TournamentEntry>> GetTournamentEntriesAsync(int tournamentId, CancellationToken cancellationToken);
        Task RemovePlayerAsync(int tournamentId, int playerId, CancellationToken cancellationToken);
        Task<Round> StartTournamentAsync(int tournamentId, CancellationToken cancellationToken);
        Task<Round> AdvanceTournamentAsync(int tournamentId, CancellationToken cancellationToken);
        Task<IEnumerable<TournamentWinner>> CompleteTournamentAsync(int tournamentId, CancellationToken cancellationToken);
        Task<IEnumerable<Player>> GetContestantsAsync(int tournamentId, CancellationToken cancellationToken);
    }
}
