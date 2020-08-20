using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Plugins.SubDemo.Contracts
{
    // Так как упоминается в контракте операций, то описание типа доступно клиенту,
    // где может использоваться для генерации форм ввода данных.
    [Display(Description = "Рецепт")]
    public class Recipe
    {
        [Display(Description = "Список ингридиентов")]
        public List<string> Ingridients { get; set; }

        public override string ToString()
        {
            return string.Join(", ", Ingridients);
        }
    }
}
