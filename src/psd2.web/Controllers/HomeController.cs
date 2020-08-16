using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using psd2.web.Models;
using System.Diagnostics;

namespace psd2.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthenticationSchemeProvider _schemes;
        private readonly IAuthenticationHandlerProvider _handlerProvider;
        private readonly IAuthenticationService _authenticationService;

        public HomeController(ILogger<HomeController> logger, IAuthenticationSchemeProvider schemes, 
            IAuthenticationHandlerProvider handlerProvider, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _schemes = schemes;
            _handlerProvider = handlerProvider;
            _authenticationService = authenticationService;
        }

        public IActionResult Index()
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