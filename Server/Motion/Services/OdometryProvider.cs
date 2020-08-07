using Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Motion.Services
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
