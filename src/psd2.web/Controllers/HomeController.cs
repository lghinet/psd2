using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using psd2.web.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace psd2.web.Controllers
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

        [Authorize]
        public async Task<IActionResult> Ing()
        {
            var ac = await HttpContext.GetUserAccessTokenAsync();
            using var client = new HttpClient(new DigestHttpHandler(Utils.CreateCertificateFromFile()));
            client.DefaultRequestHeaders.Add("keyId", "5ca1ab1e-c0ca-c01a-cafe-154deadbea75");
            client.SetBearerToken(ac);
            var result = await client.GetStringAsync("https://api.sandbox.ing.com/v3/accounts");
            var accounts = JsonConvert.DeserializeObject<IngModel>(result);
            return View(accounts);
        }

        [Authorize]
        public async Task<IActionResult> Bt()
        {
            var ac = await HttpContext.GetUserAccessTokenAsync();
            using var client = new HttpClient(new BtHttpHandler());
            client.SetBearerToken(ac);
            var result = await client.GetStringAsync("https://api.apistorebt.ro/bt/sb/bt-psd2-aisp/v1/accounts");
            var accounts = JsonConvert.DeserializeObject<IngModel>(result);
            return View(accounts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}