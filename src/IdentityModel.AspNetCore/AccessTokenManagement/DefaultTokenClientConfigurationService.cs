using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityModel.AspNetCore.AccessTokenManagement
{
    /// <summary>
    /// Options-based configuration service for token clients
    /// </summary>
    public class DefaultTokenClientConfigurationService : ITokenClientConfigurationService
    {
        private readonly AccessTokenManagementOptions _accessTokenManagementOptions;
        private readonly IOptionsMonitor<OAuthOptions> _oidcOptions;
        private readonly IAuthenticationSchemeProvider _schemeProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="accessTokenManagementOptions"></param>
        /// <param name="oidcOptions"></param>
        /// <param name="schemeProvider"></param>
        public DefaultTokenClientConfigurationService(
            IOptions<AccessTokenManagementOptions> accessTokenManagementOptions,
            IOptionsMonitor<OAuthOptions> oidcOptions,
            IAuthenticationSchemeProvider schemeProvider)
        {
            _accessTokenManagementOptions = accessTokenManagementOptions.Value;
            _oidcOptions = oidcOptions;
            _schemeProvider = schemeProvider;
        }
        
        /// <inheritdoc />
        public virtual async Task<ClientCredentialsTokenRequest> GetClientCredentialsRequestAsync(string clientName)
        {
            ClientCredentialsTokenRequest requestDetails;
            
            // if a named client configuration was passed in, try to load it
            if (string.Equals(clientName, AccessTokenManagementDefaults.DefaultTokenClientName))
            {
                // if only one client configuration exists, load that
                if (_accessTokenManagementOptions.Client.Clients.Count == 1)
                {
                    requestDetails = _accessTokenManagementOptions.Client.Clients.First().Value;
                }
                // otherwise fall back to the scheme configuration
                else
                {
                    var options = await GetOptions(_accessTokenManagementOptions.User.Scheme);

                    requestDetails = new ClientCredentialsTokenRequest()
                    {
                        Address = options.TokenEndpoint,
                        ClientId = options.ClientId,
                        ClientSecret = options.ClientSecret
                    };

                    if (!string.IsNullOrWhiteSpace(_accessTokenManagementOptions.Client.Scope))
                    {
                        requestDetails.Scope = _accessTokenManagementOptions.Client.Scope;
                    }

                    var assertion = await CreateAssertionAsync(clientName);
                    if (assertion != null)
                    {
                        requestDetails.ClientAssertion = assertion;
                    }
                }
            }
            else
            {
                if (!_accessTokenManagementOptions.Client.Clients.TryGetValue(clientName, out requestDetails))
                {
                    throw new InvalidOperationException($"No access token client configuration found for client: {clientName}");
                }
            }

            return requestDetails;
        }

        /// <inheritdoc />
        public virtual async Task<RefreshTokenRequest> GetRefreshTokenRequestAsync()
        {
            var options = await GetOptions(_accessTokenManagementOptions.User.Scheme);

            var requestDetails = new RefreshTokenRequest
            {
                Address = options.TokenEndpoint,
                ClientId = options.ClientId,
                ClientSecret = options.ClientSecret
            };
            
            var assertion = await CreateAssertionAsync();
            if (assertion != null)
            {
                requestDetails.ClientAssertion = assertion;
            }

            return requestDetails;
        }

        /// <inheritdoc />
        public virtual Task<TokenRevocationRequest> GetTokenRevocationRequestAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves configuration from a named OpenID Connect handler
        /// </summary>
        /// <param name="schemeName"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual async Task<OAuthOptions> GetOptions(string schemeName)
        {
            OAuthOptions options;

            if (string.IsNullOrWhiteSpace(schemeName))
            {
                var scheme = await _schemeProvider.GetDefaultChallengeSchemeAsync();

                if (scheme is null)
                {
                    throw new InvalidOperationException("No OpenID Connect authentication scheme configured for getting client configuration. Either set the scheme name explicitly or set the default challenge scheme");
                }

                options = _oidcOptions.Get(scheme.Name);
            }
            else
            {
                options = _oidcOptions.Get(schemeName);
            }

            return options;
        }

        /// <summary>
        /// Allows injecting a client assertion into outgoing requests
        /// </summary>
        /// <param name="clientName">Name of client (if present)</param>
        /// <returns></returns>
        protected virtual Task<ClientAssertion> CreateAssertionAsync(string clientName = default)
        {
            return Task.FromResult<ClientAssertion>(null);
        }
    }
}