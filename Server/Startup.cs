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
            container.Register<IPipelineBuilder, PipelineBuilder>();
            container.Register<PipelineBuilder>();
            container.Register<IOperationRaiser, OperationRaiser>();
            container.Register<OperationRaiser>();
            container.Register<ProcessingInspectorPipe>();
        }

        public static void Build(Container container, PipelineBuilder pipeline)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            pipeline
                .AddSender(container.GetInstance(typeof(OperationRaiser)) as ISender)
                .AddPipe(container.GetInstance(typeof(ProcessingInspectorPipe)) as IPipe);
        }
    }
}
