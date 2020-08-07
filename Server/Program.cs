using Common;
using Common.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();
            var builder = new PipelineBuilder();
            builder
                .AddClient(client)
                .AddClient(new Client())
                .AddPipe(new Pipe())
                .AddPipe(new Pipe2())
                .Build();

            Thread.Sleep(1000);
            (client.RecievedBlock as ITargetBlock<Transaction>).Post(new RequestDto().ToTransaction(client.Id, null));
            Thread.Sleep(1000);
            (client.RecievedBlock as ITargetBlock<Transaction>).Post(new RequestDto().ToTransaction(client.Id, null));
            Console.ReadKey();
        }
    }

    public class Client : IClient
    {
        public Guid Id { get; }

        public ISourceBlock<Transaction> RecievedBlock { get; }

        public ITargetBlock<Transaction> ProcessedBlock { get; }

        public Client()
        {
            Id = Guid.NewGuid();
            Console.WriteLine("Client");
            Console.WriteLine(Id);
            RecievedBlock = new BufferBlock<Transaction>();
            (RecievedBlock as ITargetBlock<Transaction>).Post(new RequestDto().ToTransaction(new Guid(), null));
            ProcessedBlock = new ActionBlock<Transaction>(t=> {if(Id == t.ClientId) Console.WriteLine($"It's {Id}"); });
        }
    }

    public class Pipe : IPipe
    {
        public Transaction Process(Transaction transaction)
        {
            transaction.Messages.Add(new Message(MessageTypes.Commands.Freeze, null));
            return transaction;
        }
    }

    public class Pipe2 : IPipe
    {
        public Transaction Process(Transaction transaction)
        {
            Console.WriteLine("Transaction");
            Console.WriteLine(transaction.ClientId);
            foreach (var mes in transaction.Messages)
                Console.WriteLine(mes.MessageType);
            return transaction;
        }
    }
}
