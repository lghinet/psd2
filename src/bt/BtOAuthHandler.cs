using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace bt
{
    public class BtOAuthHandler : OAuthHandler<BtOAuthOptions>
    {
        public BtOAuthHandler(IOptionsMonitor<BtOAuthOptions> options, ILoggerFactory logger, 
            UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        public override Task<bool> HandleRequestAsync()
        {
            return base.HandleRequestAsync();
        }
    }
}