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
            services.AddSingleton<IApiDeclaraionRegistry, ReflectionApiDeclarationRegistry>();
            services.AddSingleton<CookieAuthenticationMiddleware>();
            services.AddSingleton<OperationPipelineBuilder>();
            services.AddSingleton<IOperationPipelineBuilder, OperationPipelineBuilder>();
            services.AddScoped<IRoleDisputeFactory, RoleDisputeFactory>();
            services.AddScoped<IEventRaiser, EventRaiser>();
            
            Plugins.Motion.Startup.ConfigureServices(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, OperationPipelineBuilder pipelineBuilder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<CookieAuthenticationMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            Plugins.Motion.Startup.Configure(pipelineBuilder, app.ApplicationServices);
            pipelineBuilder.Build();
        }
    }
}
