using Common;
using Microsoft.Extensions.DependencyInjection;
using Plugins.SubDemo.Contracts;
using SubDemo;
using SubDemo.Pipes;
using System;

namespace Plugins.SubDemo
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Используемые сервисы регистрируются в DI контейнере
            services.AddSingleton<EmployeeRegistry>();
            // Компоненты конвейера регистрируются в контейнере, для инъекции зависимостей, в частоности, IEventRaiser
            // Они являются синглтонами по дизайну
            services.AddSingleton<HrPipe>();
            services.AddSingleton<SubOfTheDayPipe>();
            services.AddSingleton<KitchenPipe>();
            services.AddSingleton<OrderCounterPipe>();
        }

        public static void Configure(IOperationPipelineBuilder pipeline, IContractRegistry contracts, IServiceProvider services)
        {
            // Контракты операций, событий и данных автоматически регистрируются из контракта сервиса
            contracts.RegisterServiceContract<ISubDemoPlugin>();
            // Дополняется конвейер обработки операций
            pipeline
                .AddPipe(services.GetService<HrPipe>())
                .AddPipe(services.GetService<SubOfTheDayPipe>())
                .AddPipe(services.GetService<OrderCounterPipe>())
                .AddPipe(services.GetService<KitchenPipe>());
        }
    }
}
