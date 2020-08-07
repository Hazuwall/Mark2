namespace Common.Messaging
{
    public static class MessageTypes
    {
        public const string Query = "Query/";
        public const string Command = "Command/";
        public const string Event = "Event/";
        public const string Info = "Info/";
        public const string Error = "Error/";

        public static class Queries
        {
            public const string Coords = Query + "Coords";
            public const string AbsCoords = Query + "AbsCoords";
            public const string Velocities = Query + "Velocities";
            public const string AbsVelocities = Query + "AbsVelocities";
        }

        public static class Commands
        {
            public const string SetCoords = Command + "SetCoords";
            public const string SetAbsCoords = Command + "SetAbsCoords";

            public const string Freeze = Command + "Freeze";
            public const string NotifyQuequePointPassed = Command + "NotifyQuequePointPassed";
        }

        public static class Events
        {
            public const string QuequePointPassed = Event + "QuequePointPassed";
        }

        public static class Infos
        {
            public const string Ok = Info + "Ok";
        }

        public static class Errors
        {
            public const string AccessDenied = Error + "AccessDenied";
            public const string Calculation = Error + "Calculation";
        }
    }
}
