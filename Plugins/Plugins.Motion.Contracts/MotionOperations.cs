using Common;

namespace Plugins.Motion.Contracts
{
    [MessageInfoCollection]
    public static class MotionOperations
    {
        public static class Queries
        {
            static Queries()
            {
                DeclarationHelper.FillStringStaticFieldsWithNames(typeof(Queries), prefix: OperationFamilies.Query);
            }

            [MessageInfo(
                TOut = typeof(Vector6))]
            public static readonly string Coords;

            [MessageInfo(
                TOut = typeof(Vector6))]
            public static readonly string AbsCoords;

            [MessageInfo(
                TOut = typeof(Vector6))]
            public static readonly string Velocities;

            [MessageInfo(
                TOut = typeof(Vector6))]
            public static readonly string AbsVelocities;
        }

        public static class Commands
        {
            static Commands()
            {
                DeclarationHelper.FillStringStaticFieldsWithNames(typeof(Commands), prefix: OperationFamilies.Command);
            }

            [MessageInfo(
                Role = Role.Admin)]
            public static readonly string Freeze;
        }
    }
}
