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
    public class SubOfTheDayPipe : IPipe
    {
        private Recipe GetRecipe()
        {
            return new Recipe()
            {
                Ingridients = new List<string>()
                {
                    "Хлеб",
                    "Помидоры",
                    "Салями",
                    "Салат",
                    "Перец",
                    "Лук",
                    "Огурчики",
                    "Горчица",
                    "Халапеньо"
                }
            };
        }

        public void Process(OperationContext context)
        {
            // Вместо константы "GetSubOfTheDayRecipe" используется непосредственно контракт сервиса
            // с целью более простого обновления наименований в будущем
            if (context.CurrentOperation.Title == nameof(ISubDemoPlugin.GetSubOfTheDayRecipe))
            {
                // Операция полностью выполнена, следующие обработчики в конвейере вызваны не будут
                var recipe = GetRecipe();
                context.Complete(recipe);
            }
            else if (context.CurrentOperation.Title == nameof(ISubDemoPlugin.OrderSubOfTheDay))
            {
                // Вместо непосредственного вызова необходимых функций, операция трасформируется
                // в более простую и передаётся дальше. Таким образом компоненты остаются слабо связанными.
                var recipe = GetRecipe();
                context.SetNextOperation(new Message(nameof(ISubDemoPlugin.OrderSandwich), recipe));
            }
        }
    }
}
