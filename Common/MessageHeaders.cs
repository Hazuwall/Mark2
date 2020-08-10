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

        [MessageDeclarationCollection]
        public static class Queries
        {
            [MessageDeclaration(Out = typeof(AccessLevel))]
            public const string AccessLevel = Query + "AccessLevel";

            [MessageDeclaration(Out = typeof(Dictionary<string,MessageDeclarationAttribute>))]
            public const string Help = Query + "Help";
        }

        [MessageDeclarationCollection]
        public static class Commands
        {
            [MessageDeclaration(In = typeof(AccessLevel), Out = typeof(AccessDisputeStatus))]
            public const string ClaimAccessLevel = Command + "ClaimAccessLevel";
        }

        [MessageDeclarationCollection]
        public static class Events
        {
            [MessageDeclaration(Out = typeof(AccessDisputeEventArgs))]
            public const string AccessDisputeEvent = Event + "AccessDisputeEvent";
        }
    }
}
