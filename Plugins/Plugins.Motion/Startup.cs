using Common;
using Microsoft.Extensions.DependencyInjection;
using Plugins.Motion.Contracts;
using Plugins.Motion.Pipes;
using SimpleInjector;
using System;

namespace Plugins.Motion
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IOdometryProvider, OdometryProvider>();
            services.AddSingleton<IKinematicsService, KinematicsService>();
            services.AddSingleton<OdometryPipe>();
        }

        public static void Configure(IOperationPipelineBuilder pipeline, IContractRegistry contracts, IServiceProvider services)
        {
            contracts.RegisterServiceContract<IMotionServiceContract>();
            pipeline.AddPipe(services.GetService<OdometryPipe>());
        }
    }
}
