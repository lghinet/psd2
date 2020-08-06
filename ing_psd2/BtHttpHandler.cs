using IdentityModel;
using IdentityModel.Client;
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

namespace ing_psd2
{
    public class BtHttpHandler : DelegatingHandler
    {
        public BtHttpHandler() : base(new HttpClientHandler())
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("psu-geo-location", "46.931808,26.369690");
            request.Headers.Add("psu-ip-address", "128.0.57.43");
            request.Headers.Add("x-request-id", Guid.NewGuid().ToString());
            request.Headers.Add("consent-id", "273c540e6c534b1f8d873baf23728969");
            return await base.SendAsync(request, cancellationToken);
        }
    }
}