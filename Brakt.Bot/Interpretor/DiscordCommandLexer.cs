using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Brakt.Bot.Interpretor
{
    public class DiscordCommandLexer : ILexer
    {
        // BRAKT COMMAND RULES
        //
        // Brakt command should follow the following pattern:
        // brakt [command] [arguments]
        //
        // the brakt keyword tells the Brakt bot that a command should be interpreted. Everything not starting with "brakt " is ignored.
        //
        // COMMANDS:
        //  * <create> - create a new tournament.
        //    * Arguments:
        //      * [swiss|single|rr] - determines the type of tournament that will be generated. Default swiss.
        //      * [dd:HH:mm:ss] - Time until scheduled to start. Default 1 hour.
        //      * #tag1 #tag2 ... #tagN - useful for finding player/group statistics. At least one tag argument is required
        //  * <list> - Lists tournaments associated with the server, past or anticipated.
        //    * Arguments:
        //      * [all] - optional. If specified, this will show all tournaments.
        //  * <cancel> - cancel a tournament. If underway, stats will be cleared for the given tournament.
        //    * Arguments:
        //      * [tournament id] - an integer id given when a tournament is generated. This can be found with the list command if it has been forgotten.
        //  * <fire> - generates round 1 bracket and starts a tournament.
        //    * Arguments:
        //      * [tournament id] - an integer id given when a tournament is generated. This can be found with the list command if it has been forgotten.
        //  * <join> - Join a created tournament that is not started. If tournament has fired, entry will not be permitted. An alternative to this is reacting to the tournament create message with the hand raise emoji.
        //      * [tournament id] - an integer id given when a tournament is generated. This can be found with the list command if it has been forgotten.
        //  * <drop> - Drop out of a tournament. Coward.
        //      * [tournament id] - an integer id given when a tournament is generated. This can be found with the list command if it has been forgotten.
        //  * <pairings> - Show the generated pairings for the anticipated round.
        //      * [tournament id] - an integer id given when a tournament is generated. This can be found with the list command if it has been forgotten.
        //  * <report> - Report pairing results.
        //    * Arguments:
        //      * #-# - report <your number of wins>-<opponent number of wins>
        //    * NOTE: Both players must agree to the report. The report only counts when the opposing player gives a thumbs up.
        //  * <results> - If tournament concluded, shows tournament winner. If underway, shows last round results.
        //      * [tournament id] - an integer id given when a tournament is generated. This can be found with the list command if it has been forgotten.
        //  * <advance> - advances tournament to the next round. If any matches are underway, the result will be recorded as a draw.
        //      * [tournament id] - an integer id given when a tournament is generated. This can be found with the list command if it has been forgotten.
        //  * <complete> - completes the tournament and calculates the winner.
        //      * [tournament id] - an integer id given when a tournament is generated. This can be found with the list command if it has been forgotten.
        //  * <leaderboard> - Within the context of a discord server, this will show you the leaderboard for tournaments held on the server.
        //    * Arguments:
        //      * #tag1 #tag2 ... #tagN - optional. If no tags specified, all stats for all tags will be shown.
        //  * <stats> - Within the context of a discord server, this will return stats for a player in the context of the server. If sent via DM, stats will be returned for all servers where that player is a member.
        //    * Arguments:
        //      * #tag1 #tag2 ... #tagN - optional. If no tags specified, all stats for all tags will be shown.


        const string TOKEN_RGX = "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        const string BRAKT_CMD_INVOKER = "brakt ";
        
        private static readonly string[] VALID_COMMANDS =
        {
            "create",
            "list",
            "cancel",
            "fire",
            "join",
            "drop",
            "pairings",
            "report",
            "results",
            "advance",
            "complete",
            "leaderboard",
            "stats"
        };

        public bool IsBraktCommand(string message)
        {
            return message.ToLower().StartsWith(BRAKT_CMD_INVOKER);
        }

        public CommandTokens TokenizeBraktCommand(string command)
        {
            command = command.ToLower().Replace(BRAKT_CMD_INVOKER, string.Empty);

            var parts = Regex.Split(command, TOKEN_RGX);
            var commandName = string.Empty;
            var args = new List<string>();
            var tags = new List<string>();

            for (int i = 0; i < parts.Length; i++)
            {
                if (string.IsNullOrEmpty(parts[i])) continue;

                if (i == 0)
                {
                    commandName = parts[i];
                    continue;
                }

                if (parts[i].StartsWith('#'))
                {
                    tags.Add(parts[i].Replace("#", ""));
                    continue;
                }

                args.Add(parts[i]);
            }

            if (!VALID_COMMANDS.Contains(commandName)) throw new ArgumentException($"'{commandName}' is not a valid brakt command.");

            return new CommandTokens(commandName, args, tags);
        }
    }
}
