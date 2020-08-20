using Common;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Server.Operations
{
    public class OperationPipeline : IDisposable, IOperationPipelineBuilder
    {
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        private readonly List<IPipe> _pipes = new List<IPipe>();
        private readonly IServiceProvider _services;
        private ITargetBlock<OperationContext> _firstBlock = null;

        public OperationPipeline(IServiceProvider services)
        {
            _services = services;
        }

        public OperationPipeline AddPipe(IPipe pipe)
        {
            _pipes.Add(pipe);
            return this;
        }

        IOperationPipelineBuilder IOperationPipelineBuilder.AddPipe(IPipe pipe)
        {
            return AddPipe(pipe);
        }

        IOperationPipelineBuilder IOperationPipelineBuilder.AddPipe<T>()
        {
            var pipe = _services.GetService<T>();
            return AddPipe(pipe);
        }

        public void Build()
        {
            if (_firstBlock != null)
            {
                throw new Exception("The pipeline has already been built.");
            }

            var lastBlock = CreateBlock(_pipes.FirstOrDefault());
            _firstBlock = lastBlock;
            IDisposable disp;
            for (int i = 1; i < _pipes.Count; i++)
            {
                var currentBlock = CreateBlock(_pipes[i]);
                disp = lastBlock.LinkTo(currentBlock);
                lastBlock = currentBlock;
                _disposables.Add(disp);
            }
            disp = lastBlock.LinkTo(CreateResultBlock());

            if (Log.IsEnabled(Serilog.Events.LogEventLevel.Information))
            {
                Log.Information("The operation pipeline has been built: {0}",
                    string.Join(" -> ", _pipes.Select(p => p.GetType().Name)));
            }
        }

        public Task<object> ExecuteAsync(Guid id, string[] flags, Message operation)
        {
            var tcs = new TaskCompletionSource<object>();
            var context = new OperationContext(id, flags, operation, tcs);
            _firstBlock.SendAsync(context);
            return tcs.Task;
        }

        public void Dispose()
        {
            foreach (var disp in _disposables)
            {
                disp.Dispose();
            }
        }

        private TransformBlock<OperationContext, OperationContext> CreateBlock(IPipe pipe) {
            if (pipe == null)
            {
                return new TransformBlock<OperationContext, OperationContext>(t => t);
            }
            else
            {
                return new TransformBlock<OperationContext, OperationContext>(context =>
                {
                    if (!context.IsCompleted)
                    {
                        try
                        {
                            pipe.Process(context);
                        }
                        catch (Exception ex)
                        {
                            context.CompleteWithException(ex);
                        }
                    }
                    return context;
                });
            }
        }

        private ActionBlock<OperationContext> CreateResultBlock(){
            return new ActionBlock<OperationContext>(context =>
            {
                if (!context.IsCompleted)
                {
                    context.CompleteWithException(
                        new Exception($"An operation {context.CurrentOperation.Title} was not completed."));
                }
            });
        }
    }
}
