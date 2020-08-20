using Common;
using Microsoft.Extensions.DependencyInjection;
using Plugins.Motion.Contracts;
using Plugins.Motion.Pipes;
using System;

namespace Plugins.Motion
{
    public class Startup : IPluginStartup
    {
        public int Order => 0;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IOdometryProvider, OdometryProvider>();
            services.AddSingleton<IKinematicsService, KinematicsService>();
            services.AddSingleton<OdometryPipe>();
        }

        public void Configure(IOperationPipelineBuilder pipeline, IContractRegistry contracts, IServiceProvider services)
        {
            contracts.RegisterServiceContract<IMotionPlugin>();
            pipeline.AddPipe(services.GetService<OdometryPipe>());
        }
    }
}
