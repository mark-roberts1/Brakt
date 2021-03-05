using Brakt.Rest.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Brakt.Rest.Controllers
{
    [Route("api/maintenance"), ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly IDataLayer _dataLayer;

        public MaintenanceController(IDataLayer dataLayer)
        {
            _dataLayer = dataLayer;
        }

        [HttpPut("sqlcommand")]
        public async Task RunSqlCommandAsync([FromBody] Sql sql, CancellationToken cancellationToken)
        {
            await _dataLayer.RunSqlAsync(sql.ThrowIfNull("sql").Command.ThrowIfDefault("sql.Command"), cancellationToken);
        }
    }

    public class Sql
    {
        public string Command { get; set; }
    }
}
