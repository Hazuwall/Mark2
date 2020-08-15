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

        public void Process(OperationContext context)
        {
            var title = context.CurrentOperation.Title;
            if(title == nameof(IMotionServiceContract.GetCoords))
            {
                context.Complete(_odometry.GetCoords());
            }
            else if(title == nameof(IMotionServiceContract.GetVelocities))
            {
                context.Complete(_odometry.GetVelocities());
            }
            else if (title == nameof(IMotionServiceContract.GetAbsCoords))
            {
                var coords = _odometry.GetCoords();
                context.Complete(_kinematics.ForwardPosTransform(coords));
            }
            else if (title == nameof(IMotionServiceContract.GetAbsVelocities))
            {
                var velocities = _odometry.GetCoords();
                context.Complete(_kinematics.ForwardVelocityTransform(velocities));
            }
        }
    }
}
