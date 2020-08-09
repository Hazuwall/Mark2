using Common.Messaging;
using Server.Motion.Services;

namespace Server.Motion.Pipes
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
            switch (transaction.Operation.Header)
            {
                case MessageHeaders.Queries.Coords:
                    data = _odometry.GetCoords();
                    break;
                case MessageHeaders.Queries.Velocities:
                    data = _odometry.GetVelocities();
                    break;
                case MessageHeaders.Queries.AbsCoords:
                    var coords = _odometry.GetCoords();
                    data = _kinematics.ForwardPosTransform(coords);
                    break;
                case MessageHeaders.Queries.AbsVelocities:
                    var velocities = _odometry.GetCoords();
                    data = _kinematics.ForwardVelocityTransform(velocities);
                    break;
                default:
                    break;
            }
            if (data != null)
            {
                transaction.Result = new Message(transaction.Operation.Header, data);
                transaction.Operation = null;
            }
        }
    }
}
