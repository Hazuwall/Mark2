using System;
using System.Collections.Generic;
using System.Text;

namespace SubDemo
{
    public class EmployeeRegistry
    {
        public List<string> List { get; set; }

        public EmployeeRegistry()
        {
            List = new List<string>(new[] { "Кассир", "Повар", "Уборщик", "Управляющий" });
        }
    }
}
