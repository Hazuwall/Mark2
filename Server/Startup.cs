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
            container.Register<IOperationRaiser, OperationRaiser>();
            container.Register<OperationRaiser>();
            container.Register<IMessageDeclarationRegistry, MessageDeclarationRegistry>();
            container.Register<HelpPipe>();
            container.Register<ProcessingInspectorPipe>();
        }

        public static void BeginBuild(Container container, PipelineBuilder pipeline)
        {
            pipeline
                .AddSender(container.GetInstance(typeof(OperationRaiser)) as ISender)
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
