namespace Plugins.Motion.Contracts
{
    public struct Vector3
    {
        public double Q1;
        public double Q2;
        public double Q3;

        public override string ToString()
        {
            return $"1: {Q1:0.00}, 2: {Q2:0.00}, 3: {Q3:0.00}";
        }
    }
}
