using Common;

namespace Plugins.Motion.Contracts
{
    public static class MotionMessageHeaders
    {
        private const string Query = MessageHeaders.Query;
        private const string Command = MessageHeaders.Command;
        private const string Event = MessageHeaders.Event;

        [MessageInfoCollection]
        public static class Queries
        {
            [MessageInfo(Out = typeof(Vector6))]
            public const string Coords = Query + "Coords";

            [MessageInfo(Out = typeof(Vector6))]
            public const string AbsCoords = Query + "AbsCoords";

            [MessageInfo(Out = typeof(Vector6))]
            public const string Velocities = Query + "Velocities";

            [MessageInfo(Out = typeof(Vector6))]
            public const string AbsVelocities = Query + "AbsVelocities";
        }

        [MessageInfoCollection]
        public static class Commands
        {
            [MessageInfo(Role = Role.Admin)]
            public const string Freeze = Command + "Freeze";
        }

        [MessageInfoCollection]
        public static class Events
        {
        }
    }
}
