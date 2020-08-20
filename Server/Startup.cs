using Common;
using Common.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Server.Contracts;
using Server.Operations;
using Server.Roles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    public class Startup
    {
        private readonly List<IPluginStartup> _startups;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            _startups = ReflectionHelper
                .LoadAssemblies("Plugins")
                .FindClassesOfType<IPluginStartup>()
                .Select(startupType => (IPluginStartup)Activator.CreateInstance(startupType))
                .OrderBy(startup => startup.Order)
                .ToList();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IClientRoleRegistry, ClientRoleRegistry>();
            services.AddSingleton<IContractFactory, ContractFactory>();
            services.AddSingleton<IContractRegistry, ContractRegistry>();
            services.AddSingleton<CookieIdentificationMiddleware>();
            services.AddSingleton<OperationPipeline>();
            services.AddSingleton<IOperationPipelineBuilder, OperationPipeline>();
            services.AddSingleton<IRoleDisputeFactory, RoleDisputeFactory>();
            services.AddSingleton<IEventPublisher, EventPublisher>();
            services.AddScoped<GlobalExceptionFilterAttribute>();
            services.AddScoped<OperationValidationFilter>();
            services.AddScoped<AuthorizationFilterAttribute>();
            services
                .AddControllers(options =>
                {
                    options.Filters.Add(typeof(GlobalExceptionFilterAttribute));
                })
                .AddNewtonsoftJson();

            foreach(var startup in _startups)
            {
                startup.ConfigureServices(services);
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, OperationPipeline pipelineBuilder, IContractRegistry contracts)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<CookieIdentificationMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("Default", "api/{controller}/{action=Index}");
            });

            foreach (var startup in _startups)
            {
                startup.Configure(pipelineBuilder, contracts, app.ApplicationServices);
            }
            pipelineBuilder.Build();

            if (Log.IsEnabled(Serilog.Events.LogEventLevel.Information))
            {
                Log.Information("Registered operations: {0}", string.Join(", ", contracts.OperationContracts.Keys));
                Log.Information("Registered events: {0}", string.Join(", ", contracts.EventContracts.Keys));
                Log.Information("Registered data types: {0}", string.Join(", ", contracts.DataContracts.Keys));
            }
        }
    }
}
