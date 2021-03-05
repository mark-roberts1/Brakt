using Brakt.Bot.Formatters;
using Brakt.Bot.Identification;
using Brakt.Bot.Interpretor;
using Brakt.Client;
using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Bot.Commands
{
    public class ReportCommandHandler : CommandHandlerBase, ICommandHandler
    {
        private readonly static Regex _matchupPattern = new Regex(@"\d+\-\d+");
     
        public ReportCommandHandler(IBraktApiClient client, IResponseFormatter formatter) : base(client, formatter)
        {
        }

        public string Command => "report";

        public string HelpMessage
            => "Report pairing results.\n   * Arguments:\n     * #-# - report <your number of wins>-<opponent number of wins>";

        public override async Task ExecuteAsync(MessageCreateEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            AssertGroupMemberContext(userContext);

            if (cmdToken.Arguments == null || cmdToken.Arguments.Count() == 0)
            {
                await args.Message.RespondAsync("To report a pairing result, use ```brakt report wins-losses```");
                return;
            }

            var matchupArgs = cmdToken.Arguments.Where(w => _matchupPattern.IsMatch(w));

            if (matchupArgs.Count() == 0)
            {
                await args.Message.RespondAsync("To report a pairing result, use ```brakt report wins-losses```");
                return;
            }
            else if (matchupArgs.Count() > 1)
            {
                await args.Message.RespondAsync("Only one pairing result may be reported at a time.");
                return;
            }
        }

        public override async Task ExecuteAsync(MessageReactionAddEventArgs args, CommandTokens cmdToken, IdContext userContext, CancellationToken cancellationToken)
        {
            if (!args.Emoji.Name.Contains("👍")) return;

            AssertGroupMemberContext(userContext);

            var p1 = userContext.GroupMember.PlayerId;
            var p2 = (await Client.GetDiscordPlayerAsync((long)args.Message.Author.Id, cancellationToken)).PlayerId;

            var tournaments = (await Client.GetTournamentsAsync(userContext.GroupMember.GroupId, cancellationToken)).Where(w => !w.Completed);

            foreach (var tournament in tournaments)
            {
                var rounds = (await Client.GetTournamentRoundsAsync(tournament.TournamentId, cancellationToken)).Where(w => !w.Complete);

                foreach (var round in rounds)
                {
                    var pairings = await Client.GetPairingsAsync(round.RoundId, cancellationToken);

                    var pairing = pairings.FirstOrDefault(w => (w.Player1 == p1 && w.Player2 == p2) || (w.Player1 == p2 && w.Player2 == p1));

                    if (pairing != null)
                    {
                        PairingResult result;

                        if (pairing.Concluded)
                        {
                            return;
                        }

                        try
                        {
                            if (!TryGetPairingResult(cmdToken.Arguments, pairing.PairingId, p2, p1, out result))
                            {
                                await args.Message.RespondAsync("To report a pairing result, use ```brakt report wins-losses```");
                                return;
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            await args.Message.RespondAsync(ex.Message);
                            return;
                        }

                        var roundUpdated = await Client.ReportPairingResultAsync(round.RoundId, result, cancellationToken);

                        if (roundUpdated.Complete)
                        {
                            await args.Message.RespondAsync($"This round has completed. When ready to advance to the next round, use ```brakt advance {tournament.TournamentId}```");
                        }

                        await args.Message.CreateReactionAsync(DSharpPlus.Entities.DiscordEmoji.FromName(BotConnector.Client, ":thumbsup:"));
                        return;
                    }
                }
            }
        }

        private bool TryGetPairingResult(IEnumerable<string> args, int pairingId, int reporterId, int otherId, out PairingResult result)
        {
            result = new PairingResult();

            if (args == null || args.Count() == 0) return false;

            var matchupArgs = args.Where(w => _matchupPattern.IsMatch(w));

            if (matchupArgs.Count() == 0) return false;
            else if (matchupArgs.Count() > 1)
                throw new ArgumentException("Only one pairing result may be reported at a time.");

            var arg = args.Single();

            var parts = arg.Split('-');

            int wins = byte.Parse(parts[0]);
            int losses = byte.Parse(parts[1]);

            if (wins < 0 || losses < 0) throw new ArgumentException("Ya can't have negative wins or losses silly goose");

            if (wins > 2 || losses > 2 || wins + losses > 3) throw new ArgumentException("Only best 2/3 is currently supported.");

            result.Draw = wins == losses;
            result.WinningPlayerId = wins > losses ? reporterId : otherId;
            result.Wins = wins > losses ? wins : losses;
            result.Losses = wins < losses ? wins : losses;
            result.PairingId = pairingId;

            return true;
        }
    }
}
