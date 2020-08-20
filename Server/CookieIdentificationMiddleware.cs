using Microsoft.AspNetCore.Http;
using Server.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class CookieIdentificationMiddleware : IMiddleware
    {
        public static readonly string ClientIdKey = "ClientId";
        public static readonly string ClientRoleKey = "ClientRole";

        private readonly IClientRoleRegistry _roleRegistry;

        public CookieIdentificationMiddleware(IClientRoleRegistry roleRegistry)
        {
            _roleRegistry = roleRegistry;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Guid clientId;
            if (!context.Request.Cookies.TryGetValue(ClientIdKey, out string clientIdString))
            {
                clientId = Guid.NewGuid();
                context.Response.Cookies.Append(ClientIdKey, clientId.ToString());
            }
            else
            {
                clientId = Guid.Parse(clientIdString);
            }
            context.Items.Add(ClientIdKey, clientId);
            context.Items.Add(ClientRoleKey, _roleRegistry.GetRole(clientId));
            await next.Invoke(context);
        }
    }
}
