using Common;
using Common.Services;
using Plugins.SubDemo.Contracts;
using SubDemo.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubDemo.Pipes
{
    // Первый компонент конвейера обработки операций.
    // Он вызывается после предыдущего плагина, если операция не была полностью выполнена.
    // Каждый компонент выполняет одну операцию в момент времени, в порядке поступления.
    // Таким образом, если используемые в компоненте сервисы больше нигде не используются,
    // то код внутри них априори потокобезопасен.
    public class HrPipe : IPipe
    {
        private readonly EmployeeRegistry _registry;
        private readonly IEventPublisher _raiser;

        // Зависимости будут инъектированы DI контейнером.
        // IEventPublisher - сервис, поднимающий событие у всех подключенных клиентов 
        public HrPipe(EmployeeRegistry registry, IEventPublisher raiser)
        {
            _registry = registry;
            _raiser = raiser;
        }

        public void Process(OperationContext context)
        {
            if (context.CurrentOperation.Title == nameof(ISubDemoPlugin.FireEmployee))
            {
                var count = _registry.List.Count;
                if (count > 0)
                {
                    var firedIndex = new Random().Next(count);
                    var firedEmployee = _registry.List[firedIndex];
                    _registry.List.RemoveAt(firedIndex);
                    var args = new JobOpeningArgs()
                    {
                        Vacancy = firedEmployee
                    };
                    _raiser.Post(nameof(ISubDemoPlugin.PositionOpened), args);
                    context.Complete();
                }
            }
        }
    }
}
