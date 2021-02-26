using Brakt.Bot.Commands;
using Brakt.Bot.Identification;
using Brakt.Bot.Interpretor;
using Brakt.Client;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Bot.EventHandlers
{
    public class DiscordEventHandler : IDiscordEventHandler
    {
        private readonly Func<CancellationTokenSource> _ctsFac;
        private readonly ILexer _lexer;
        private readonly IContextFactory _contextFactory;
        private readonly IBraktApiClient _client;
        private readonly ICommandHandlerFactory _cmdFactory;

        public DiscordEventHandler(
            Func<CancellationTokenSource> ctsFac,
            ILexer lexer,
            IContextFactory contextFactory,
            IBraktApiClient client,
            ICommandHandlerFactory cmdFactory)
        {
            _ctsFac = ctsFac;
            _lexer = lexer;
            _contextFactory = contextFactory;
            _client = client;
            _cmdFactory = cmdFactory;
        }

        public async Task HandleAsync(MessageCreateEventArgs args)
        {
            try
            {
                if (!_lexer.IsBraktCommand(args.Message.Content)) return;

                var cmdToken = _lexer.TokenizeBraktCommand(args.Message.Content);

                using var cts = _ctsFac();
                var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);

                var command = _cmdFactory.GetCommandHandler(cmdToken.Command);

                if (command == null)
                {
                    await args.Message.RespondAsync("Unrecognized command :(");
                    return;
                }

                await command.ExecuteAsync(args, cmdToken, userCtx, cts.Token);
            }
            catch (Exception e)
            {
                await args.Message.RespondAsync(e.Message);
            }
        }

        public async Task HandleAsync(MessageReactionRemoveEventArgs args)
        {
            try
            {
                if (!_lexer.IsBraktCommand(args.Message.Content)) return;

                var cmdToken = _lexer.TokenizeBraktCommand(args.Message.Content);

                using var cts = _ctsFac();
                var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);

                var command = _cmdFactory.GetCommandHandler(cmdToken.Command);

                if (command == null)
                {
                    await args.Message.RespondAsync("Unrecognized command :(");
                    return;
                }

                await command.ExecuteAsync(args, cmdToken, userCtx, cts.Token);
            }
            catch (Exception e)
            {
                await args.Message.RespondAsync(e.Message);
            }
        }

        public async Task HandleAsync(MessageReactionAddEventArgs args)
        {
            try
            {
                if (!_lexer.IsBraktCommand(args.Message.Content)) return;

                var cmdToken = _lexer.TokenizeBraktCommand(args.Message.Content);

                using var cts = _ctsFac();
                var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);

                var command = _cmdFactory.GetCommandHandler(cmdToken.Command);

                if (command == null)
                {
                    await args.Message.RespondAsync("Unrecognized command :(");
                    return;
                }

                await command.ExecuteAsync(args, cmdToken, userCtx, cts.Token);
            }
            catch (Exception e)
            {
                await args.Message.RespondAsync(e.Message);
            }
        }

        public async Task HandleAsync(MessageUpdateEventArgs args)
        {
            try
            {
                if (!_lexer.IsBraktCommand(args.Message.Content)) return;

                var cmdToken = _lexer.TokenizeBraktCommand(args.Message.Content);

                using var cts = _ctsFac();
                var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);

                var command = _cmdFactory.GetCommandHandler(cmdToken.Command);

                if (command == null)
                {
                    await args.Message.RespondAsync("Unrecognized command :(");
                    return;
                }

                await command.ExecuteAsync(args, cmdToken, userCtx, cts.Token);
            }
            catch (Exception e)
            {
                await args.Message.RespondAsync(e.Message);
            }
        }

        public async Task HandleAsync(GuildUpdateEventArgs args)
        {
            using var cts = _ctsFac();
            var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);
        }

        public async Task HandleAsync(GuildCreateEventArgs args)
        {
            using var cts = _ctsFac();
            var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);
        }

        public async Task HandleAsync(GuildBanAddEventArgs args)
        {
            using var cts = _ctsFac();
            var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);

            await _client.RemoveMemberAsync(userCtx.Group.GroupId, userCtx.Player.PlayerId, cts.Token);
        }

        public async Task HandleAsync(GuildBanRemoveEventArgs args)
        {
            using var cts = _ctsFac();
            var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);

            await _client.AddMemberAsync(new GroupMember
            {
                GroupId = userCtx.Group.GroupId,
                IsActive = true,
                IsAdmin = userCtx.GroupMember.IsAdmin,
                IsOwner = userCtx.GroupMember.IsOwner,
                PlayerId = userCtx.Player.PlayerId
            }, cts.Token);
        }

        public async Task HandleAsync(GuildMemberRemoveEventArgs args)
        {
            using var cts = _ctsFac();
            var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);

            await _client.RemoveMemberAsync(userCtx.Group.GroupId, userCtx.Player.PlayerId, cts.Token);
        }

        public async Task HandleAsync(GuildMembersChunkEventArgs args)
        {
            using var cts = _ctsFac();
            var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);
        }

        public async Task HandleAsync(UserUpdateEventArgs args)
        {
            using var cts = _ctsFac();
            var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);
        }

        public async Task HandleAsync(GuildMemberAddEventArgs args)
        {
            using var cts = _ctsFac();
            var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);

            await _client.AddMemberAsync(new GroupMember
            {
                GroupId = userCtx.Group.GroupId,
                IsActive = true,
                IsAdmin = userCtx.GroupMember.IsAdmin,
                IsOwner = userCtx.GroupMember.IsOwner,
                PlayerId = userCtx.Player.PlayerId
            }, cts.Token);
        }

        public async Task HandleAsync(UserSettingsUpdateEventArgs args)
        {
            using var cts = _ctsFac();
            var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);
        }

        public async Task HandleAsync(GuildMemberUpdateEventArgs args)
        {
            using var cts = _ctsFac();
            var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);
        }
    }
}
