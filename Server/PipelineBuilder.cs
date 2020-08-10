using Common;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace Server
{
    public class PipelineBuilder : IDisposable, IPipelineBuilder
    {
        private ITargetBlock<Transaction> _first = null;
        private ISourceBlock<Transaction> _last = null;
        private readonly List<ISender> _senders = new List<ISender>();
        private readonly List<IReciever> _recievers = new List<IReciever>();
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        private readonly PipeWrapper _pipeWrapper = new PipeWrapper();
        private readonly List<string> _presentationList = new List<string>();

        public PipelineBuilder AddSender(ISender sender)
        {
            _senders.Add(sender);
            return this;
        }

        public PipelineBuilder AddPipe(IPipe pipe)
        {
            var block = _pipeWrapper.Wrap(pipe);
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

        IPipelineBuilder IPipelineBuilder.AddPipe(IPipe pipe)
        {
            return AddPipe(pipe);
        }

        public PipelineBuilder AddReciever(IReciever reciever)
        {
            _recievers.Add(reciever);
            return this;
        }

        public void Build()
        {
            IDisposable disp;
            foreach (var sender in _senders)
            {
                disp = sender.SourceBlock.LinkTo(_first);
                _disposables.Add(disp);
                _presentationList.Insert(0,sender.GetType().Name);
            }

            //forward single output transaction instance to all recievers
            var broadcast = new BroadcastBlock<Transaction>(t => t);
            disp = _last.LinkTo(broadcast);
            _disposables.Add(disp);

            foreach (var reciever in _recievers)
            {
                disp = broadcast.LinkTo(reciever.TargetBlock);
                _disposables.Add(disp);
                _presentationList.Add(reciever.GetType().Name);
            }
            if (Log.IsEnabled(Serilog.Events.LogEventLevel.Information))
            {
                Log.Information("A pipeline is built: {0}", string.Join(" -> ", _presentationList));
            }
            _first = null;
            _last = null;
            _senders.Clear();
            _recievers.Clear();
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
