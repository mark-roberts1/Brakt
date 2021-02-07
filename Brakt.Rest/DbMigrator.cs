using Brakt.Rest.Data;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Rest
{
    public class DbMigrator : BackgroundService
    {
        private readonly IDataLayer _dataLayer;

        public DbMigrator(IDataLayer dataLayer)
        {
            _dataLayer = dataLayer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _dataLayer.RunMigrationsAsync(stoppingToken);
        }
    }
}
