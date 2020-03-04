using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using TestGoQuoProblem1.Models;

namespace TestGoQuoProblem1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IDistributedCache cache;

        public HomeController(ILogger<HomeController> logger, IDistributedCache cache)
        {
            _logger = logger;
            this.cache = cache;
        }

        public async Task<IActionResult> IndexAsync()
        {
            string message = "not found";

            var value = await this.cache.GetAsync("msg");
            if (value == null)
            {
                message = "This content was cached at " + DateTime.Now;
                value = Encoding.UTF8.GetBytes(message);
                this.cache.Set("msg", value);
            }
            else
            {
                message = Encoding.UTF8.GetString(value);
            }
            ViewBag.message = message;
            return View();
        }

        public async Task<IActionResult> PrivacyAsync()
        {
            var value = await this.cache.GetAsync("msg");
            var message = Encoding.UTF8.GetString(value);
            ViewBag.message = message;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
