
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using BarProject.DatabaseProxy.Functions;
using BarProject.DatabaseProxy.Models.ExceptionHandlers;

namespace BarProject.WebService.Infrastructure
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using DatabaseProxy.Models;
    using DatabaseProxy.Models.ReadModels;
    using Microsoft.Owin.Security.OAuth;

    public class AuthorizationServer : OAuthAuthorizationServerProvider
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            context.Validated();
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            try
            {
                var username = context.Identity.Claims.FirstOrDefault(x => x.Type == "username");
                if (username != null)
                {
                    var firstOrDefault = context.Identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
                    if (firstOrDefault != null)
                        context.AdditionalResponseParameters.Add("UserPrivileges", firstOrDefault.Value);
                    UserFunctions.LogUserLogin(username.Value);

                }
            }
            catch (Exception ex)
            {

            }
            return Task.FromResult<object>(null);
        }
        Regex IdRegex = new Regex(@"workstationId=(\d+)");
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var userData = UserFunctions.GetUserFullData(context.UserName);
            if (userData == null)
            {
                context.OwinContext.Response.StatusCode = 401;
                context.Response.StatusCode = 401;
                context.SetError("no_user", "This user cannot be authenticated.");
                return;
            }
            var role = UserFunctions.GetPrivileges(context.UserName, context.Password);
            context.Request.Body.Position = 0;
            string content = new StreamReader(context.Request.Body).ReadToEnd();
            var match = IdRegex.Match(content);
            int id = 0;
            bool goodId = false;
            if (match.Success)
            {
                if (int.TryParse(match.Groups[1].Value, out id))
                {
                    goodId = true;
                }
            }
            if (role == UserPrivileges.NoUser)
            {
                context.SetError("unauthorized", "This user cannot be authenticated.");
                return;
            }
            try
            {
                if (!goodId || !UserFunctions.UserCanLoginOnLocation(role, id))
                {
                    context.SetError("unauthorized", "You do not have permission to log on this location.");
                    return;
                }
            }
            catch (NullReferenceException ex)
            {
                context.SetError("unauthorized", "There is some problem with this location.");
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            if (identity.IsAuthenticated)
            {
                identity.AddClaim(new Claim("username", context.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, role.ToReadable()));
                identity.AddClaim(new Claim(ClaimTypes.Name, userData.name));
                identity.AddClaim(new Claim(ClaimTypes.Surname, userData.surname));
                identity.AddClaim(new Claim(ClaimTypes.Dns, context.Request.RemoteIpAddress));
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid_grant", "This user cannot be authenticated.");
            }

        }



    }
}