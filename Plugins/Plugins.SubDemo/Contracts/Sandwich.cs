using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Plugins.SubDemo.Contracts
{
    [Display(Description = "Сэндвич")]
    public class Sandwich
    {
        [Display(Description = "Зарисовка")]
        public string Depiction { get; set; }

        public override string ToString()
        {
            return Depiction;
        }
    }
}
