using System;
using System.Collections.Generic;
using System.Text;

namespace SubDemo.Contracts
{
    public class JobOpeningArgs : EventArgs
    {
        public string Vacancy { get; set; }
    }
}
