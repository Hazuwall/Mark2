using Common;
using Plugins.Motion.Contracts;

namespace Plugins.Motion.Pipes
{
    public class OdometryPipe : IPipe
    {
        private readonly IOdometryProvider _odometry;
        private readonly IKinematicsService _kinematics;

        public OdometryPipe(IOdometryProvider odometry, IKinematicsService kinematics)
        {
            _odometry = odometry;
            _kinematics = kinematics;
        }

        public void Process(Transaction transaction)
        {
            object data = null;
            var header = transaction.Operation.Header;
            if(header == MotionMessageHeaders.Queries.Coords)
            {
                data = _odometry.GetCoords();
            }
            else if(header == MotionMessageHeaders.Queries.Velocities)
            {
                data = _odometry.GetVelocities();
            }
            else if (header == MotionMessageHeaders.Queries.AbsCoords)
            {
                var coords = _odometry.GetCoords();
                data = _kinematics.ForwardPosTransform(coords);
            }
            else if (header == MotionMessageHeaders.Queries.AbsVelocities)
            {
                var velocities = _odometry.GetCoords();
                data = _kinematics.ForwardVelocityTransform(velocities);
            }

            if (data != null)
            {
                transaction.Result = new Message(header, data);
                transaction.Operation = null;
            }
        }
    }
}
