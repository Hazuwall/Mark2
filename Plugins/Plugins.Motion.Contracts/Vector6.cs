using System;
using System.Collections.Generic;
using System.Text;

namespace Plugins.Motion.Contracts
{
    public struct Vector6
    {
        public double Q1;
        public double Q2;
        public double Q3;
        public double Q4;
        public double Q5;
        public double Q6;

        public override string ToString()
        {
            return $"1: {Q1:0.00}, 2: {Q2:0.00}, 3: {Q3:0.00}, 4:{Q4:0.00}, 5:{Q5:0.00}, 6:{Q6:0.00}";
        }
    }
}
