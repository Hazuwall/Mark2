using System;
using System.Collections.Generic;

namespace Common
{
    public static class MessageHeaders
    {
        public const string Query = "Query/";
        public const string Command = "Command/";
        public const string Event = "Event/";
        public const string Error = "Error/";

        [MessageInfoCollection]
        public static class Queries
        {
            [MessageInfo(Out = typeof(Role))]
            public const string Role = Query + "Role";

            [MessageInfo(Out = typeof(Dictionary<string,MessageInfoAttribute>))]
            public const string Help = Query + "Help";
        }

        [MessageInfoCollection]
        public static class Commands
        {
            [MessageInfo(In = typeof(Role), Out = typeof(bool))]
            public const string ClaimRole = Command + "ClaimRole";
        }

        [MessageInfoCollection]
        public static class Events
        {
            [MessageInfo(Out = typeof(RoleDisputeEventArgs))]
            public const string RoleDisputeEvent = Event + "RoleDisputeEvent";
        }
    }
}
