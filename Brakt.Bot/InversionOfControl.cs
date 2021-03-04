using Brakt.Bot.Commands;
using Brakt.Bot.EventHandlers;
using Brakt.Bot.Formatters;
using Brakt.Bot.Identification;
using Brakt.Bot.Interpretor;
using Brakt.Client;
using DSharpPlus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

namespace Brakt.Bot
{
    public static class InversionOfControl
    {
        public static IServiceCollection InjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DiscordConfig>(configuration.GetSection("Discord"));
            services.Configure<ApiConfiguration>(configuration.GetSection("BraktClient"));

            services.AddTransient<IContextFactory, IdContextFactory>();
            services.AddTransient<ILexer, DiscordCommandLexer>();
            services.AddTransient<IDiscordEventHandler, DiscordEventHandler>();
            services.RegisterAllTypes<ICommandHandler>(new[] { typeof(ICommandHandler).Assembly });
            services.AddTransient<ICommandHandlerFactory, CommandHandlerFactory>();
            services.AddTransient<IBraktApiClient>(p => new BraktApiClient(p.GetService<IOptions<ApiConfiguration>>().Value));
            services.AddTransient(p => GetTokenSource(p));
            services.AddTransient<IResponseFormatter, DiscordResponseFormatter>();
            services.AddTransient<ITableFormatter, AsciiTableFormatter>();

            return services;
        }

        private static Func<CancellationTokenSource> GetTokenSource(IServiceProvider provider)
        {
            return () => new CancellationTokenSource();
        }

        public static void RegisterAllTypes<T>(this IServiceCollection services, Assembly[] assemblies, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T))));
            foreach (var type in typesFromAssemblies)
                services.Add(new ServiceDescriptor(typeof(T), type, lifetime));
        }
    }
}
