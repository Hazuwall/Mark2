using Common;
using Microsoft.Extensions.DependencyInjection;
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

        public static void Configure(IOperationPipelineBuilder pipeline, IServiceProvider services)
        {
            pipeline.AddPipe(services.GetService<OdometryPipe>());
        }
    }
}
