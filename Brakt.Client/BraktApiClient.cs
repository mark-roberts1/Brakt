using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Client
{
    public class BraktApiClient : IBraktApiClient
    {
        private const string GROUP = "api/group";
        private const string PLAYER = "api/player";
        private const string ROUND = "api/round";
        private const string TOURNAMENT = "api/tournament";

        private readonly IRestClient _restClient;

        public BraktApiClient(ApiConfiguration config)
        {
            _restClient = new RestClient(config.BaseUrl);
        }

        public async Task AddMemberAsync(GroupMember member, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{GROUP}/member", Method.PUT).AddJsonBody(member);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            response.ThrowIfError();
        }

        public async Task<Round> AdvanceTournamentAsync(int tournamentId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{TOURNAMENT}/{tournamentId}/advance", Method.PUT);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<Round>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<IEnumerable<TournamentWinner>> CompleteTournamentAsync(int tournamentId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{TOURNAMENT}/{tournamentId}/complete", Method.PUT);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<List<TournamentWinner>>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<Group> CreateGroupAsync(CreateGroupRequest group, CancellationToken cancellationToken)
        {
            var request = new RestRequest(GROUP, Method.POST).AddJsonBody(group);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<Group>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<Player> CreatePlayerAsync(Player player, CancellationToken cancellationToken)
        {
            var request = new RestRequest(PLAYER, Method.POST).AddJsonBody(player);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<Player>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<Tournament> CreateTournamentAsync(CreateTournamentRequest tournament, CancellationToken cancellationToken)
        {
            var request = new RestRequest(TOURNAMENT, Method.POST).AddJsonBody(tournament);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<Tournament>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task DeleteTournamentAsync(int tournamentId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{TOURNAMENT}/{tournamentId}", Method.DELETE);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            response.ThrowIfError();
        }

        public async Task<IEnumerable<Player>> GetContestantsAsync(int tournamentId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{TOURNAMENT}/{tournamentId}/players", Method.GET);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<List<Player>>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<Group> GetDiscordGroupAsync(long discordId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{GROUP}/discord/{discordId}", Method.GET);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<Group>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<Player> GetDiscordPlayerAsync(long discordId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{PLAYER}/discord/{discordId}", Method.GET);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<Player>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<Group> GetGroupAsync(int groupId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{GROUP}/{groupId}", Method.GET);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<Group>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<IEnumerable<Statistic>> GetGroupStatisticsAsync(int groupId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{GROUP}/{groupId}/stats", Method.GET);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<List<Statistic>>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<IEnumerable<Statistic>> GetGroupStatisticsAsync(GroupStatsRequest requestBody, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{GROUP}/stats", Method.PUT).AddJsonBody(requestBody);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<List<Statistic>>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<Statistic> GetGroupStatisticsAsync(int playerId, GroupStatsRequest requestBody, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{GROUP}/stats/{playerId}/player", Method.PUT).AddJsonBody(requestBody);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<Statistic>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<GroupMember> GetMemberAsync(int groupId, int playerId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{GROUP}/{groupId}/member/{playerId}", Method.GET);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<GroupMember>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<IEnumerable<GroupMember>> GetMembersAsync(int groupId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{GROUP}/{groupId}/member", Method.GET);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<List<GroupMember>>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<IEnumerable<GroupMember>> GetMembershipsAsync(int playerId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{PLAYER}/{playerId}/groups", Method.GET);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<List<GroupMember>>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<IEnumerable<Pairing>> GetPairingsAsync(int roundId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{ROUND}/{roundId}/pairings", Method.GET);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<List<Pairing>>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<Player> GetPlayerAsync(int playerId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{PLAYER}/{playerId}", Method.GET);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<Player>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<IEnumerable<Player>> GetPlayersAsync(int groupId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{GROUP}/{groupId}/players", Method.GET);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<List<Player>>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<IEnumerable<Statistic>> GetPlayerStatisticsAsync(int playerId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{PLAYER}/{playerId}/stats", Method.GET);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<List<Statistic>>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<IEnumerable<Statistic>> GetPlayerStatisticsAsync(PlayerStatsRequest requestBody, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{PLAYER}/stats", Method.PUT).AddJsonBody(requestBody);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<List<Statistic>>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<Round> GetRoundAsync(int roundId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{ROUND}/{roundId}", Method.GET);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<Round>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<Round> GetRoundAsync(int tournamentId, int roundNumber, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{ROUND}/{tournamentId}/{roundNumber}", Method.GET);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<Round>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<IEnumerable<PairingResult>> GetRoundResultsAsync(int roundId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{ROUND}/{roundId}/results", Method.GET);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<List<PairingResult>>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<Tournament> GetTournamentAsync(int tournamentId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{TOURNAMENT}/{tournamentId}", Method.GET);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<Tournament>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<IEnumerable<Round>> GetTournamentRoundsAsync(int tournamentId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{TOURNAMENT}/{tournamentId}/rounds", Method.GET);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<List<Round>>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsAsync(int groupId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{GROUP}/{groupId}/tournaments", Method.GET);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<List<Tournament>>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<IEnumerable<TournamentWinner>> GetTournamentWinnersAsync(int tournamentId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{TOURNAMENT}/{tournamentId}/winners", Method.GET);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<List<TournamentWinner>>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task RegisterPlayerAsync(TournamentEntry entry, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{TOURNAMENT}/register", Method.PUT).AddJsonBody(entry);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            response.ThrowIfError();
        }

        public async Task RemoveMemberAsync(int groupId, int playerId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{GROUP}/{groupId}/member/{playerId}", Method.DELETE);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            response.ThrowIfError();
        }

        public async Task RemovePlayerAsync(int tournamentId, int playerId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{TOURNAMENT}/{tournamentId}/{playerId}", Method.DELETE);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            response.ThrowIfError();
        }

        public async Task<Round> ReportPairingResultAsync(int roundId, PairingResult result, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{ROUND}/{roundId}/report-result", Method.PUT).AddJsonBody(result);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<Round>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task SetGroupAdminAsync(int groupId, int playerId, CancellationToken cancellationToken, bool isAdmin = true)
        {
            var request = new RestRequest($"{GROUP}/{groupId}/member/{playerId}/set-admin", Method.PUT).AddQueryParameter("isAdmin", isAdmin ? "true" : "false");

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            response.ThrowIfError();
        }

        public async Task SetGroupOwnerAsync(int groupId, int playerId, CancellationToken cancellationToken, bool isOwner = true)
        {
            var request = new RestRequest($"{GROUP}/{groupId}/member/{playerId}/set-owner", Method.PUT).AddQueryParameter("isOwner", isOwner ? "true" : "false");

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            response.ThrowIfError();
        }

        public async Task<Round> StartTournamentAsync(int tournamentId, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"{TOURNAMENT}/{tournamentId}/start", Method.PUT);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<Round>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }

        public async Task<Tournament> UpdateTournamentAsync(Tournament tournament, CancellationToken cancellationToken)
        {
            var request = new RestRequest(TOURNAMENT, Method.PUT).AddJsonBody(tournament);

            var response = await _restClient.ExecuteAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NoContent) return null;

            return JsonSerializer.Deserialize<Tournament>(response.ThrowIfError().Content, ApiConfiguration.SerializerOptions);
        }
    }
}
