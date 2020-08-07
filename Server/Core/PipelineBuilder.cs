using Common.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace Server.Core
{
    public class PipelineBuilder : IDisposable
    {
        private ITargetBlock<Transaction> _first = null;
        private ISourceBlock<Transaction> _last = null;
        private readonly List<IClient> _clients = new List<IClient>();
        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        public PipelineBuilder AddClient(IClient client)
        {
            _clients.Add(client);
            return this;
        }

        public PipelineBuilder AddPipe(IPipe pipe)
        {
            var block = new TransformBlock<Transaction, Transaction>(pipe.Process);
            if(_first == null)
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

        public void Build()
        {
            //forward single output transaction instance to all clients
            var broadcast = new BroadcastBlock<Transaction>(t => t);
            var disp = _last.LinkTo(broadcast);
            _disposables.Add(disp);

            foreach (var client in _clients)
            {
                disp = client.RecievedBlock.LinkTo(_first);
                _disposables.Add(disp);
                disp = broadcast.LinkTo(client.ProcessedBlock);
                _disposables.Add(disp);
            }
            _first = null;
            _last = null;
            _clients.Clear();
        }

        public void Dispose()
        {
            foreach(var disp in _disposables)
            {
                disp.Dispose();
            }
        }
    }
}
