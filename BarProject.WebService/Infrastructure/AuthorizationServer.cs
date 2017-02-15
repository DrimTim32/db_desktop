
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
         
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context) 
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var userData = DatabaseProxy.Functions.UserFunctions.GetUserFullData(context.UserName);
            if (userData == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }
            var role = DatabaseProxy.Functions.UserFunctions.GetPrivileges(context.UserName, context.Password);
            if (role == UserPrivileges.NoUser)
            {
                context.SetError("invalid_grant", "You got no power here Gandalf the Grey.");
                return;
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