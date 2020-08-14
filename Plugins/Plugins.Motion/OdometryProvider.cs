using Plugins.Motion.Contracts;
using System;

namespace Plugins.Motion
{
    public class OdometryProvider : IOdometryProvider
    {
        public Vector6 GetCoords()
        {
            return new Vector6();
        }

        public Vector6 GetVelocities()
        {
            return new Vector6();
        }
    }
}
