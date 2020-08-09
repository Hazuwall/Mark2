using Common;
using Common.Messaging;
using Serilog;
using Server.Core;
using Server.Core.Pipes;
using Server.Motion.Pipes;
using Server.Motion.Services;
using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            var raiser = new OperationRaiser();
            var builder = new PipelineBuilder();
            builder.AddSender(raiser)
                .AddPipe(new OdometryPipe(new OdometryProvider(), new KinematicsService()))
                .AddPipe(new ProcessingInspectorPipe())
                .Build();
            raiser.Raise(new Message(MessageHeaders.Queries.AbsCoords));
            raiser.Raise(new Message(MessageHeaders.Commands.Freeze));
            raiser.Raise(new Message(MessageHeaders.Event + "SomeEvent"));
            Thread.Sleep(1000);
        }
    }
}
