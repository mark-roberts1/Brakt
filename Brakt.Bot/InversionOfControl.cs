using Brakt.Client;
using DSharpPlus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brakt.Bot
{
    public static class InversionOfControl
    {
        public static IServiceCollection InjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DiscordConfig>(configuration.GetSection("Discord"));
            services.Configure<ApiConfiguration>(configuration.GetSection("BraktClient"));
            return services;
        }
    }
}
