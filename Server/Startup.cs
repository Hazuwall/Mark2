using Common;
using Common.Services;
using Serilog;
using Server.Pipes;
using SimpleInjector;

namespace Server
{
    public static class Startup
    {
        public static void Register(Container container)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            container.Register<IPipelineBuilder, PipelineBuilder>();
            container.Register<PipelineBuilder>();
            container.Register<IEventRaiser, EventRaiser>();
            container.Register<EventRaiser>();
            container.Register<IMessageDeclarationRegistry, MessageDeclarationRegistry>();
            container.Register<IClientAccessRegistry, ClientAccessRegistry>();
            container.Register<IClientAccessController, ClientAccessController>();
            container.Register<IClientAccessDisputeFactory, ClientAccessDisputeFactory>();
            container.Register<AccessPipe>();
            container.Register<HelpPipe>();
            container.Register<ProcessingInspectorPipe>();
        }

        public static void BeginBuild(Container container, PipelineBuilder pipeline)
        {
            pipeline
                .AddSender(container.GetInstance(typeof(EventRaiser)) as ISender)
                .AddPipe(container.GetInstance(typeof(AccessPipe)) as IPipe)
                .AddPipe(container.GetInstance(typeof(HelpPipe)) as IPipe);
        }

        public static void EndBuild(Container container, PipelineBuilder pipeline)
        {
            pipeline
                .AddPipe(container.GetInstance(typeof(ProcessingInspectorPipe)) as IPipe)
                .Build();
        }
    }
}
