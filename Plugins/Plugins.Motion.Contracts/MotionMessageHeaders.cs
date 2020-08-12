using Common;

namespace Plugins.Motion.Contracts
{
    [MessageInfoCollection]
    public static class MotionMessageHeaders
    {
        public static class Queries
        {
            static Queries()
            {
                DeclarationHelper.FillStringStaticFieldsWithNames(typeof(Queries), prefix: MessageHeaders.Families.Query);
            }

            [MessageInfo(
                Out = typeof(Vector6))]
            public static readonly string Coords;

            [MessageInfo(
                Out = typeof(Vector6))]
            public static readonly string AbsCoords;

            [MessageInfo(
                Out = typeof(Vector6))]
            public static readonly string Velocities;

            [MessageInfo(
                Out = typeof(Vector6))]
            public static readonly string AbsVelocities;
        }

        public static class Commands
        {
            static Commands()
            {
                DeclarationHelper.FillStringStaticFieldsWithNames(typeof(Commands), prefix: MessageHeaders.Families.Command);
            }

            [MessageInfo(
                Role = Role.Admin)]
            public static readonly string Freeze;
        }

        public static class Events
        {
            static Events()
            {
                DeclarationHelper.FillStringStaticFieldsWithNames(typeof(Events), prefix: MessageHeaders.Families.Event);
            }
        }
    }
}
