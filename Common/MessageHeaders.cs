using System;
using System.Collections.Generic;

namespace Common
{
    [MessageInfoCollection]
    public static class MessageHeaders
    {
        public static class Families
        {
            public static readonly string Query = "Query/";
            public static readonly string Command = "Command/";
            public static readonly string Event = "Event/";
            public static readonly string Error = "Error/";
        }

        public static class Queries
        {
            static Queries()
            {
                DeclarationHelper.FillStringStaticFieldsWithNames(typeof(Queries), prefix: Families.Query);
            }

            [MessageInfo(
                Out = typeof(Role),
                Summary = "Узнать текущую роль.")]
            public static readonly string Role;


            [MessageInfo(
                Out = typeof(Dictionary<string, MessageInfoAttribute>),
                Summary = "Получить список с описанием всех зарегистрированных операций.")]
            public static readonly string Help;
        }

        public static class Commands
        {
            static Commands()
            {
                DeclarationHelper.FillStringStaticFieldsWithNames(typeof(Commands), prefix: Families.Command);
            }

            [MessageInfo(
                In = typeof(Role), Out = typeof(bool),
                Summary = "Подать заявление на роль. Если роль занята, то будет открыт спор, " +
                "в течение которого текущий владелец для сохранения роли должен повторно подать заявление.")]
            public static readonly string ClaimRole;
        }

        public static class Events
        {
            static Events()
            {
                DeclarationHelper.FillStringStaticFieldsWithNames(typeof(Events), prefix: Families.Event);
            }

            [MessageInfo(
                Out = typeof(RoleDisputeEventArgs),
                Summary = "Происходит при открытии или закрытии спора о роли.")]
            public static readonly string RoleDisputeEvent;
        }
    }
}
