using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using Microsoft.VisualBasic;

namespace ing
{
    public class PostConfigureIngOptions : IPostConfigureOptions<IngOAuthOptions>
    {
        private readonly IOptions<IngConfig> _config;
        private readonly IHttpMessageHandlerFactory _handlerFactory;
        private readonly IHttpClientFactory _httpClientFactory;

        public PostConfigureIngOptions(IOptions<IngConfig> config, IHttpMessageHandlerFactory handlerFactory,
            IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _handlerFactory = handlerFactory;
            _httpClientFactory = httpClientFactory;
        }

        public void PostConfigure(string name, IngOAuthOptions options)
        {
            if (options.BackchannelHttpHandler == null && options.Backchannel == null)
            {
                // var inner = _handlerFactory.CreateHandler("ING");
                var inner = new DigestHttpHandler(Utils.CreateCertificate(_config.Value.SingingCertificate),
                    Utils.GetSecuredHandler(_config.Value.ClientCertificate));

                options.BackchannelHttpHandler = new IngHttpHandler(options.TokenEndpoint,
                    _config.Value.SingingCertificate, _config.Value.ClientCertificate, inner);
            }
        }
    }
}