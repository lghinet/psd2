using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace ing
{
    public class IngOAuthHandler : OAuthHandler<OAuthOptions>
    {
        public IngOAuthHandler(IOptionsMonitor<OAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            return base.HandleChallengeAsync(properties);
        }

        protected override Task<AuthenticationTicket> CreateTicketAsync(
            ClaimsIdentity identity,
            AuthenticationProperties properties,
            OAuthTokenResponse tokens)
        {
            return base.CreateTicketAsync(identity, properties, tokens);

            //var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            //var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            //if (!response.IsSuccessStatusCode)
            //{
            //    throw new HttpRequestException($"An error occurred when retrieving Google user information ({response.StatusCode}). Please check if the authentication information is correct.");
            //}

            //using (var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
            //{
            //    var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
            //    context.RunClaimActions();
            //    await Events.CreatingTicket(context);
            //    return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
            //}
        }


        private static async Task<string> Display(HttpResponseMessage response)
        {
            var output = new StringBuilder();
            output.Append("Status: " + response.StatusCode + ";");
            output.Append("Headers: " + response.Headers.ToString() + ";");
            output.Append("Body: " + await response.Content.ReadAsStringAsync() + ";");
            return output.ToString();
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var authorizationEndpoint = $"{Options.AuthorizationEndpoint}?scope={FormatScope()}&redirect_uri={redirectUri}&country_code=RO";
            var newChallengeUrl = RequestAuthorizationUrlAsync(Options.TokenEndpoint, FormatScope(), authorizationEndpoint)
                .GetAwaiter().GetResult();

            var challengeUrl = base.BuildChallengeUrl(properties, redirectUri);
            var builder = new UriBuilder(newChallengeUrl) {Query = new Uri(challengeUrl).Query};

            return builder.Uri.ToString();
        }

        private async Task<string> RequestAuthorizationUrlAsync(string tokenEndpoint, string scopes, string authorizationEndpoint)
        {
            var response = await RequestAuthorizationUrlSignedAsync(authorizationEndpoint, Context.RequestAborted);

            if (response.IsError)
                throw new Exception(response.Error);

            return response.TryGet("location");
        }

        protected async Task<TokenResponse> RequestAuthorizationUrlSignedAsync(string authorizationEndpoint, CancellationToken cancellationToken)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, authorizationEndpoint);
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequest.Content = new StringContent(string.Empty);
            httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                var response = await Backchannel.SendAsync(httpRequest, cancellationToken);
                return await ProtocolResponse.FromHttpResponseAsync<TokenResponse>(response);
            }
            catch (Exception ex)
            {
                return ProtocolResponse.FromException<TokenResponse>(ex);
            }
        }
    }
}