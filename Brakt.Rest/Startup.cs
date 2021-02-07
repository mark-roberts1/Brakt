using Brakt.Rest.Data;
using Brakt.Rest.Database;
using Brakt.Rest.Database.Sqlite;
using Brakt.Rest.Filters;
using Brakt.Rest.Logic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brakt.Rest
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDbConnectionFactory, SqliteConnectionFactory>();
            services.AddTransient<IDbCommandFactory, SqliteCommandFactory>();
            services.AddTransient<ICommandExecutor, DatabaseCommandExecutor>();
            services.AddTransient<IDataMapper, ReflectionDataMapper>();
            services.AddTransient<IDataLayer, DataLayer>();
            services.AddTransient<IStatsGenerator, StatsGenerator>();
            services.AddTransient<ITournamentFacilitatorFactory, TournamentFacilitatorFactory>();

            services.AddControllers();

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add(new ApiExceptionFilter());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Brakt RESTful API",
                        Description = "Supports the Brakt Discord Bot",
                        Version = "v1"
                    });
            });

            services.AddHostedService<DbMigrator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }
    }
}
