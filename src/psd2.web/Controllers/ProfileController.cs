using IdentityModel.Client;
using ing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using psd2.web.Models;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace psd2.web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthenticationSchemeProvider _schemes;
        private readonly IAuthenticationHandlerProvider _handlerProvider;
        private readonly IAuthenticationService _authenticationService;
        private readonly IHttpClientFactory _clientFactory;

        public ProfileController(ILogger<HomeController> logger, IAuthenticationSchemeProvider schemes, 
            IAuthenticationHandlerProvider handlerProvider, IAuthenticationService authenticationService, 
            IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _schemes = schemes;
            _handlerProvider = handlerProvider;
            _authenticationService = authenticationService;
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            using var client = _clientFactory.CreateClient("ING");
            var ac = await HttpContext.GetUserAccessTokenAsync();
            //client.SetBearerToken(ac);
            var result = await client.GetStringAsync("https://api.sandbox.ing.com/v3/accounts");
            var accounts = JsonConvert.DeserializeObject<IngModel>(result);
            return View(accounts);
        }

        [HttpPost]
        public IActionResult Ing(string scheme)
        {
            return new ChallengeResult(scheme);
        }

        [HttpGet]
        public async Task<IActionResult> Bt(string scheme)
        {
            //var btScheme = await _schemes.GetSchemeAsync("BT");
            //var handler = await _handlerProvider.GetHandlerAsync(HttpContext, "BT");


            //await _authenticationService.ChallengeAsync(HttpContext, "BT", new AuthenticationProperties(){RedirectUri = "/"});
            return new ChallengeResult("BT");
            //var ac = await HttpContext.GetUserAccessTokenAsync();
            //using var client = new HttpClient(new BtHttpHandler());
            //client.SetBearerToken(ac);
            //var result = await client.GetStringAsync("https://api.apistorebt.ro/bt/sb/bt-psd2-aisp/v1/accounts");
            //var accounts = JsonConvert.DeserializeObject<IngModel>(result);
            //return View(accounts);
        }

        [HttpPost]
        public async Task<IActionResult> Bt()
        {
            //var btScheme = await _schemes.GetSchemeAsync("BT");
            //var handler = await _handlerProvider.GetHandlerAsync(HttpContext, "BT");


            //await _authenticationService.ChallengeAsync(HttpContext, "BT", new AuthenticationProperties(){RedirectUri = "/"});
            return new ChallengeResult("BT");
            //var ac = await HttpContext.GetUserAccessTokenAsync();
            //using var client = new HttpClient(new BtHttpHandler());
            //client.SetBearerToken(ac);
            //var result = await client.GetStringAsync("https://api.apistorebt.ro/bt/sb/bt-psd2-aisp/v1/accounts");
            //var accounts = JsonConvert.DeserializeObject<IngModel>(result);
            //return View(accounts);
        }
    }
}