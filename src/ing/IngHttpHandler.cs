using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;

namespace ing
{
    public class IngHttpHandler : DelegatingHandler
    {
        private readonly string _tokenEndpoint;
        private readonly X509Certificate2 _signingCertificate;

        public IngHttpHandler(string tokenEndpoint) : base(new DigestHttpHandler())
        {
            _tokenEndpoint = tokenEndpoint;
            _signingCertificate = Utils.GetSigningCertificate();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await RequestClientCredentialsTokenAsync(Utils.GetSecuredHandler(), _signingCertificate,
                _tokenEndpoint, cancellationToken);
            if (response.IsError)
                throw new Exception(response.Error);

            request.Headers.Add("Authorization", "Bearer " + response.AccessToken);
            request.Headers.Add("keyId", response.TryGet("client_id"));
            return await base.SendAsync(request, cancellationToken);
        }

        public static async Task<TokenResponse> RequestClientCredentialsTokenAsync(HttpMessageHandler handler,
            X509Certificate2 certificate, string tokenEndpoint, CancellationToken cancellationToken)
        {
            var keyId = "SN=5E4299BE";

            var tokenEndpointUri = new Uri(tokenEndpoint);
            using var client = new HttpClient(handler);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, tokenEndpointUri);
            httpRequest.Headers.Accept.Clear();
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var contentParameters = new Dictionary<string, string>
            {
                {OidcConstants.TokenRequest.GrantType, OidcConstants.GrantTypes.ClientCredentials},
            };

            var date = DateTime.UtcNow.ToString("R");
            httpRequest.Headers.Add("Date", date);
            httpRequest.Content = new FormUrlEncodedContent(contentParameters);
            var payload = await httpRequest.Content.ReadAsStringAsync();
            var digest = "SHA-256=" + Utils.Hash(payload);

            var signature = Utils.GetSignature(date, "post", tokenEndpointUri.PathAndQuery, digest, certificate);
            httpRequest.Headers.Add("Digest", digest);
            httpRequest.Headers.Add("authorization",
                $"Signature keyId=\"{keyId}\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"{signature}\"");
            httpRequest.Headers.Add("TPP-Signature-Certificate",
                Convert.ToBase64String(certificate.Export(X509ContentType.Cert)));

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

    }

    public class DigestHttpHandler : DelegatingHandler
    {
        private readonly X509Certificate2 _signingCertificate;

        public DigestHttpHandler() : base(Utils.GetSecuredHandler())
        {
            _signingCertificate = Utils.GetSigningCertificate();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var keyId = request.Headers.GetValues("keyId").DefaultIfEmpty().FirstOrDefault();
            if (string.IsNullOrEmpty(keyId))
            {
                throw new InvalidOperationException("keyId is missing");
            }

            var payload = "";
            if (request.Content != null)
            {
                payload = await request.Content.ReadAsStringAsync();
            }

            var digest = "SHA-256=" + Utils.Hash(payload);
            var date = DateTime.UtcNow.ToString("R");
            var signature = Utils.GetSignature(date, request.Method.ToString().ToLower(),
                request.RequestUri.PathAndQuery, digest, _signingCertificate);
            request.Headers.Add("Digest", digest);
            request.Headers.Add("Date", date);
            request.Headers.Add("Signature",
                $"keyId=\"{keyId}\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"{signature}\"");

            return await base.SendAsync(request, cancellationToken);
        }
    }

    static class Utils
    {
        public static X509Certificate2 GetSigningCertificate()
        {
            return new X509Certificate2(Path.Combine(Directory.GetCurrentDirectory(), "bin\\Debug\\netcoreapp3.1\\sandbox", "example_eidas_client_signing.pfx"));
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
            using var shaProvider = new SHA256CryptoServiceProvider();
            var bytes = Encoding.ASCII.GetBytes(data);
            return Convert.ToBase64String(shaProvider.ComputeHash(bytes));
        }

        public static string SignData(string data, X509Certificate2 cert)
        {
            var prv = cert.GetRSAPrivateKey();
            var sgHash = prv.SignData(Encoding.UTF8.GetBytes(data), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(sgHash, Base64FormattingOptions.None);
        }

        public static HttpMessageHandler GetSecuredHandler()
        {
            var clientHandler = new HttpClientHandler();
            var tlsCert = new X509Certificate2(Path.Combine(Directory.GetCurrentDirectory(), "bin\\Debug\\netcoreapp3.1\\sandbox", "example_eidas_client_tls.pfx"));
            clientHandler.ClientCertificates.Add(tlsCert);

            return clientHandler;
        }
    }
}