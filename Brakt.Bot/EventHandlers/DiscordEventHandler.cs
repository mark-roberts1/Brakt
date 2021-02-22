using Brakt.Bot.Identification;
using Brakt.Bot.Interpretor;
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

        public DiscordEventHandler(Func<CancellationTokenSource> ctsFac, ILexer lexer, IContextFactory contextFactory)
        {
            _ctsFac = ctsFac;
            _lexer = lexer;
            _contextFactory = contextFactory;
        }

        public async Task HandleAsync(MessageCreateEventArgs args)
        {
            if (!_lexer.IsBraktCommand(args.Message.Content)) return;

            var cmdToken = _lexer.TokenizeBraktCommand(args.Message.Content);

            using var cts = _ctsFac();
            var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);
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
        }

        public async Task HandleAsync(GuildBanRemoveEventArgs args)
        {
            using var cts = _ctsFac();
            var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);
        }

        public async Task HandleAsync(GuildMemberRemoveEventArgs args)
        {
            using var cts = _ctsFac();
            var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);
        }

        public async Task HandleAsync(MessageReactionRemoveEventArgs args)
        {
            if (!_lexer.IsBraktCommand(args.Message.Content)) return;

            var cmdToken = _lexer.TokenizeBraktCommand(args.Message.Content);

            using var cts = _ctsFac();
            var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);
        }

        public async Task HandleAsync(MessageReactionAddEventArgs args)
        {
            if (!_lexer.IsBraktCommand(args.Message.Content)) return;

            var cmdToken = _lexer.TokenizeBraktCommand(args.Message.Content);

            using var cts = _ctsFac();
            var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);
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
        }

        public async Task HandleAsync(UserSettingsUpdateEventArgs args)
        {
            using var cts = _ctsFac();
            var userCtx = await _contextFactory.GetIdContextAsync(args, cts.Token);
        }

        public async Task HandleAsync(MessageUpdateEventArgs args)
        {
            if (!_lexer.IsBraktCommand(args.Message.Content)) return;

            var cmdToken = _lexer.TokenizeBraktCommand(args.Message.Content);

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
