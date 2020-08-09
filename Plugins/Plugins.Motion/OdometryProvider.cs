using Plugins.Motion.Contracts;
using System;

namespace Plugins.Motion
{
    public class OdometryProvider : IOdometryProvider
    {
        public Vector6 GetCoords()
        {
            throw new NotImplementedException();
        }

        public Vector6 GetVelocities()
        {
            throw new NotImplementedException();
        }
    }
}
