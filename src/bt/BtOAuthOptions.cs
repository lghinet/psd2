using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace bt
{
    public class BtOAuthOptions : OAuthOptions
    {
        public BtOAuthOptions()
        {
            AuthorizationEndpoint = "https://apistorebt.ro/mga/sps/oauth/oauth20/authorize";
            TokenEndpoint = "https://api.apistorebt.ro/bt/sb/oauth/token";
            CallbackPath = "/signin-bt";
            SaveTokens = true;

            Scope.Add("openid");
            Scope.Add("profile");
            Scope.Add("email");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

            Events.OnTicketReceived = context =>
            {
                context.HandleResponse();

                // Default redirect path is the base path
                if (string.IsNullOrEmpty(context.ReturnUri))
                {
                    context.ReturnUri = "/";
                }

                context.Response.Redirect(context.ReturnUri);
                return Task.CompletedTask;
            };
        }
    }
}