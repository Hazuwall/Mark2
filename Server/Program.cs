using Common;
using Common.Services;
using Plugins.Motion.Contracts;
using SimpleInjector;
using System.Threading;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = new Container();
            container.Options.DefaultLifestyle = Lifestyle.Singleton;
            Startup.Register(container);
            Plugins.Motion.Startup.Register(container);

            var pipeline = container.GetInstance(typeof(PipelineBuilder)) as PipelineBuilder;
            Startup.Build(container, pipeline);
            Plugins.Motion.Startup.Build(container, pipeline);
            pipeline.Build();

            var raiser = container.GetInstance(typeof(IOperationRaiser)) as OperationRaiser;
            raiser.Raise(new Message(MotionMessageHeaders.Queries.AbsCoords));
            raiser.Raise(new Message(MotionMessageHeaders.Commands.Freeze));
            raiser.Raise(new Message(MessageHeaders.Event + "SomeEvent"));
            Thread.Sleep(1000);
        }
    }
}
