using Common;
using Common.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Server.Operations;
using Server.Roles;

namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            services.AddControllers().AddNewtonsoftJson();
            services.AddSingleton<IClientRoleRegistry, ClientRoleRegistry>();
            services.AddSingleton<IContractFactory, ContractFactory>();
            services.AddSingleton<IContractRegistry, ContractRegistry>();
            services.AddSingleton<CookieIdentificationMiddleware>();
            services.AddSingleton<OperationPipelineBuilder>();
            services.AddSingleton<IOperationPipelineBuilder, OperationPipelineBuilder>();
            services.AddSingleton<IRoleDisputeFactory, RoleDisputeFactory>();
            services.AddSingleton<IEventPublisher, EventPublisher>();

            //Plugins.Motion.Startup.ConfigureServices(services);
            Plugins.SubDemo.Startup.ConfigureServices(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, OperationPipelineBuilder pipelineBuilder, IContractRegistry contracts)
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
                endpoints.MapControllers();
            });

            //Plugins.Motion.Startup.Configure(pipelineBuilder, contracts, app.ApplicationServices);
            Plugins.SubDemo.Startup.Configure(pipelineBuilder, contracts, app.ApplicationServices);
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
