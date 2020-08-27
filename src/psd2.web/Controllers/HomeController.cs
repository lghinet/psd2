using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using psd2.web.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;

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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Type = context.Error.Source,
                Detail = context.Error.StackTrace,
                Title = string.Join(Environment.NewLine, context.Error.Message, 
                    context.Error.InnerException?.Message)
            });
        }
    }
}