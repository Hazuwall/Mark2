using Common;
using Common.Services;
using Serilog;
using Server.Pipes;
using Server.Roles;
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
            container.Register<IMessageInfoRegistry, MessageInfoRegistry>();
            container.Register<IClientRoleRegistry, ClientRoleRegistry>();
            container.Register<IRoleManager, RoleManager>();
            container.Register<IRoleDisputeFactory, RoleDisputeFactory>();
            container.Register<AccessVerificationPipe>();
            container.Register<RoleManagementPipe>();
            container.Register<HelpPipe>();
            container.Register<ProcessingCheckPipe>();
        }

        public static void BeginBuild(Container container, PipelineBuilder pipeline)
        {
            pipeline
                .AddSender(container.GetInstance(typeof(EventRaiser)) as ISender)
                .AddPipe(container.GetInstance(typeof(AccessVerificationPipe)) as IPipe)
                .AddPipe(container.GetInstance(typeof(RoleManagementPipe)) as IPipe)
                .AddPipe(container.GetInstance(typeof(HelpPipe)) as IPipe);
        }

        public static void EndBuild(Container container, PipelineBuilder pipeline)
        {
            pipeline
                .AddPipe(container.GetInstance(typeof(ProcessingCheckPipe)) as IPipe)
                .Build();
        }
    }
}
