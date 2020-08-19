using Common;
using SubDemo.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Plugins.SubDemo.Contracts
{
    // Контракт сервиса. Интерфейс включает выполняемые плагином операции и 
    // отправляемые клиенту события. Список операций может использоваться клиентом для генерации страниц.
    public interface ISubDemoPlugin
    {
        // Контракт операции. Возвращаемый тип указывает на формат возвращаемых клиенту данных
        // после прохождения конвейера.
        // Регистрируемое описание задаётся в атрибуте Display
        [Display(Description = "Узнать состав саба дня")]
        Recipe GetSubOfTheDayRecipe();

        // Роль (иинимальный уровень допуска) задаётся в атрибуте Role.
        // Если требуется, чтобы к операции имел доступ один клиент, то помечается как Writer.
        [Role(Role.Writer)]
        [Display(Description = "Заказать саб дня")]
        Sandwich OrderSubOfTheDay();

        // Тип входного параметра указывает на формат входных данных. Если нужно несколько параметров,
        // то их нужно объединить в класс.
        [Role(Role.Writer)]
        [Display(Description = "Заказать сэндвич")]
        Sandwich OrderSandwich(Recipe recipe);

        [Display(Description = "Получить общее количество заказов, совершённых за время работы системы")]
        int GetOrderCount();

        // Admin используется для выполнения опасных операций, потенциально нарушающих
        // ход взаимодействия с системой других клиентов. Клиент должен использовать такие команды только в ручном режиме
        [Role(Role.Admin)]
        [Display(Description = "Уволить сотрудника. Обслуживание клиентов прекратится")]
        void FireEmployee();

        // Контракт события. Определяет название и формат посылаемых клиенту событий.
        // События не поддерживают атрибут Display, поэтому используется DisplayName.
        [DisplayName("Открыта новая вакансия")]
        event EventHandler<JobOpeningArgs> PositionOpened;
    }
}
