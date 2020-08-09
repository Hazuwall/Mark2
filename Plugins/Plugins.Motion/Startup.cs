using Common;
using Plugins.Motion.Pipes;
using SimpleInjector;

namespace Plugins.Motion
{
    public static class Startup
    {
        public static void Register(Container container)
        {
            container.Register<IOdometryProvider, OdometryProvider>();
            container.Register<IKinematicsService, KinematicsService>();
            container.Register<OdometryPipe>();
        }

        public static void Build(Container container, IPipelineBuilder pipeline)
        {
            pipeline.AddPipe(container.GetInstance(typeof(OdometryPipe)) as IPipe);
        }
    }
}
