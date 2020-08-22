using Common;
using Common.Services;
using Plugins.SubDemo.Contracts;
using SubDemo.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SubDemo.Pipes
{
    public class KitchenPipe : IPipe
    {
        private readonly EmployeeRegistry _registry;

        public KitchenPipe(EmployeeRegistry registry)
        {
            _registry = registry;
        }

        public void Process(OperationContext context)
        {
            if (!_registry.List.Contains("Повар"))
                return;

            if(context.CurrentOperation.Title == nameof(ISubDemoPlugin.OrderSandwich))
            {
                // Входные данные операции передаются в свойстве Payload в типе, соответствующему контракту операции
                var recipe = context.CurrentOperation.Payload as Recipe;
                var sandwich = new Sandwich()
                {
                    Depiction = $"Вкусный сэндвич, включающий {string.Join(", ", recipe.Ingridients)}"
                };
                context.Complete(sandwich);
            }
        }
    }
}
