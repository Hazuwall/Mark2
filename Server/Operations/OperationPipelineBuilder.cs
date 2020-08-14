using Common;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace Server.Operations
{
    public class OperationPipelineBuilder : IDisposable, IOperationPipelineBuilder
    {
        private ITargetBlock<OperationContext> _first = null;
        private ISourceBlock<OperationContext> _last = null;
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        private readonly List<string> _presentationList = new List<string>();

        public ITargetBlock<OperationContext> Pipeline { get; private set; }

        public OperationPipelineBuilder AddPipe(IPipe pipe)
        {
            if(Pipeline != null)
            {
                throw new Exception("The pipeline has already been built.");
            }

            var block = new TransformBlock<OperationContext, OperationContext>(t => 
            {
                if (t.CurrentOperation != null && t.Result == null)
                    pipe.Process(t);
                return t;
            }); ;
            if (_first == null)
            {
                _first = block;
            }
            else
            {
                var disp = _last.LinkTo(block);
                _disposables.Add(disp);
            }
            _last = block;
            _presentationList.Add(pipe.GetType().Name);
            return this;
        }

        IOperationPipelineBuilder IOperationPipelineBuilder.AddPipe(IPipe pipe)
        {
            return AddPipe(pipe);
        }

        public void Build()
        {
            if (Log.IsEnabled(Serilog.Events.LogEventLevel.Information))
            {
                Log.Information("The operation pipeline has been built: {0}", string.Join(" -> ", _presentationList));
            }
            Pipeline = _first;
            _first = null;
            _last = null;
            _presentationList.Clear();
        }

        public void Dispose()
        {
            foreach (var disp in _disposables)
            {
                disp.Dispose();
            }
        }
    }
}
