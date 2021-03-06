﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace bt
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