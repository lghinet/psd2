using System.Collections.Generic;
using IdentityModel.Client;
using ing_psd2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ing_psd2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = "ING")]
        public async Task<IActionResult> Ing()
        {
            var ac = await HttpContext.GetTokenAsync("access_token");
            using var client = new HttpClient(new DigestHttpHandler(Utils.GetSigningCertificate()));
            client.DefaultRequestHeaders.Add("keyId", "5ca1ab1e-c0ca-c01a-cafe-154deadbea75");
            client.SetBearerToken(ac);
            var result = await client.GetStringAsync("https://api.sandbox.ing.com/v3/accounts");
            var accounts = JsonConvert.DeserializeObject<IngModel>(result);
            return View(accounts);
        }

        [Authorize(AuthenticationSchemes = "BT")]
        public IActionResult Bt()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}