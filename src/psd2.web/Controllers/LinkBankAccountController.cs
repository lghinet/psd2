using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using psd2.web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace psd2.web.Controllers
{
    [Authorize]
    public class LinkBankAccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthenticationSchemeProvider _schemes;
        private readonly IAuthenticationHandlerProvider _handlerProvider;
        private readonly IAuthenticationService _authenticationService;

        public LinkBankAccountController(ILogger<HomeController> logger, IAuthenticationSchemeProvider schemes,
            IAuthenticationHandlerProvider handlerProvider, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _schemes = schemes;
            _handlerProvider = handlerProvider;
            _authenticationService = authenticationService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new LinkBankAccountModel();
            var schemes = await _schemes.GetAllSchemesAsync();
            model.ExternalLogins = schemes.Where(s => !string.IsNullOrEmpty(s.DisplayName)).ToList();
            model.ReturnUrl = Url.Action("Index");
            return View(model);
        }


        [HttpPost]
        public IActionResult LinkNewAccount(string provider, string returnUrl = null)
        {
            return new ChallengeResult(provider, new OAuthChallengeProperties(){ RedirectUri = returnUrl });
        }

    }
}