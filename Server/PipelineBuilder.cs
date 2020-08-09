using Common;
using System;
using System.Collections.Generic;
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
            }

            //forward single output transaction instance to all recievers
            var broadcast = new BroadcastBlock<Transaction>(t => t);
            disp = _last.LinkTo(broadcast);
            _disposables.Add(disp);

            foreach (var reciever in _recievers)
            {
                disp = broadcast.LinkTo(reciever.TargetBlock);
                _disposables.Add(disp);
            }
            _first = null;
            _last = null;
            _senders.Clear();
            _recievers.Clear();
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
