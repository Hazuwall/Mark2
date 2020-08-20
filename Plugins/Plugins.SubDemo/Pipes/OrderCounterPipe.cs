using Common;
using Plugins.SubDemo.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubDemo.Pipes
{
    public class OrderCounterPipe : IPipe
    {
        private int _counter = 0;

        public void Process(OperationContext context)
        {
            if (context.CurrentOperation.Title == nameof(ISubDemoPlugin.OrderSandwich))
            {
                // Компонент не обрабатывает текущую операцию, но имеет возможность считать нужную информацию
                _counter++;
            }
            else if (context.CurrentOperation.Title == nameof(ISubDemoPlugin.GetOrderCount))
            {
                context.Complete(_counter);
            }
        }
    }
}
