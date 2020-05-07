using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ing_psd2
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

        protected override Task<OAuthTokenResponse> ExchangeCodeAsync(OAuthCodeExchangeContext context)
        {
            return base.ExchangeCodeAsync(context);
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
            var clientHandler = new HttpClientHandler();
            var tlsCert = new X509Certificate2(Path.Combine(Directory.GetCurrentDirectory(), "certs", "sandbox", "example_eidas_client_tls.pfx"));
            clientHandler.ClientCertificates.Add(tlsCert);

            var signCert = new X509Certificate2(Path.Combine(Directory.GetCurrentDirectory(), "certs", "sandbox", "example_eidas_client_signing.pfx"));
            using var client = new HttpClient(clientHandler);
            var response = await client.RequestClientCredentialsTokenSignedAsync(signCert, tokenEndpoint,
                scopes, CancellationToken.None);
            if (response.IsError)
                throw new Exception(response.Error);

            response = await client.RequestAuthorizationUrlSignedAsync(signCert, response.AccessToken, authorizationEndpoint,
                response.TryGet("client_id"), CancellationToken.None);

            if (response.IsError)
                throw new Exception(response.Error);

            return response.TryGet("location");
        }
    }


    internal static class HttpMessageInvokerExtensions
    {
        public static async Task<TokenResponse> RequestAuthorizationUrlSignedAsync(this HttpMessageInvoker client,
            X509Certificate2 certificate, string accessToken, string authorizationEndpoint, string clientId, 
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new InvalidOperationException("client_id is missing");
            }

            var authorizationEndpointUri = new Uri(authorizationEndpoint);
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, authorizationEndpointUri);
            httpRequest.Headers.Accept.Clear();
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequest.Headers.Add("Authorization", "Bearer " + accessToken);
            httpRequest.Content = new StringContent(string.Empty);
            httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var digest = "SHA-256=" + Hash("");
            var date = DateTime.UtcNow.ToString("R");
            var signature = GetSignature(date, "get", authorizationEndpointUri.PathAndQuery, digest, certificate);
            httpRequest.Headers.Add("Digest", digest);
            httpRequest.Headers.Add("Date", date);
            httpRequest.Headers.Add("Signature",
                $"keyId=\"{clientId}\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"{signature}\"");

            try
            {
                var response = await client.SendAsync(httpRequest, cancellationToken);
                return await ProtocolResponse.FromHttpResponseAsync<TokenResponse>(response);
            }
            catch (Exception ex)
            {
                return ProtocolResponse.FromException<TokenResponse>(ex);
            }
        }

        public static async Task<TokenResponse> RequestClientCredentialsTokenSignedAsync(this HttpMessageInvoker client,
            X509Certificate2 certificate, string tokenEndpoint, string scopes, CancellationToken cancellationToken)
        {
            var keyId = "SN=5E4299BE";

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
            httpRequest.Headers.Accept.Clear();
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var contentParameters = new Dictionary<string, string>
            {
                //{OidcConstants.TokenRequest.ClientId, clientId},
                {OidcConstants.TokenRequest.GrantType, OidcConstants.GrantTypes.ClientCredentials},
                {OidcConstants.TokenRequest.Scope, scopes},
            };

            var date = DateTime.UtcNow.ToString("R");
            httpRequest.Headers.Add("Date", date);
            httpRequest.Content = new FormUrlEncodedContent(contentParameters);
            var payload = await httpRequest.Content.ReadAsStringAsync();
            var digest = "SHA-256=" + Hash(payload);

            var signature = GetSignature(date, "post", "/oauth2/token", digest, certificate);
            httpRequest.Headers.Add("Digest", digest);
            httpRequest.Headers.Add("authorization",
                $"Signature keyId=\"{keyId}\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"{signature}\"");
            httpRequest.Headers.Add("TPP-Signature-Certificate", Convert.ToBase64String(certificate.Export(X509ContentType.Cert)));

            try
            {
                var response = await client.SendAsync(httpRequest, cancellationToken);
                return await ProtocolResponse.FromHttpResponseAsync<TokenResponse>(response);
            }
            catch (Exception ex)
            {
                return ProtocolResponse.FromException<TokenResponse>(ex);
            }
        }

        public static string GetSignature(string date, string method, string url, string digest, X509Certificate2 cert)
        {
            var dataToSign = "(request-target): " + method + " " + url + "\n" +
                             "date: " + date + "\n" +
                             "digest: " + digest;
            return SignData(dataToSign, cert);
        }

        public static string Hash(string data)
        {
            using (var shaProvider = new SHA256CryptoServiceProvider())
            {
                var bytes = Encoding.ASCII.GetBytes(data);
                return Convert.ToBase64String(shaProvider.ComputeHash(bytes));
            }
        }

        public static string SignData(string data, X509Certificate2 cert)
        {
            var prv = cert.GetRSAPrivateKey();
            var sgHash = prv.SignData(Encoding.UTF8.GetBytes(data), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(sgHash, Base64FormattingOptions.None);
        }
    }
}