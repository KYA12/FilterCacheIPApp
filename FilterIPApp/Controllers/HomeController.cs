using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using FilterIPApp.Filters;

namespace FilterIPApp.Controllers
{
    public class HomeController: Controller
    {
        private ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;//create logger with DI

        }

        /* Filter resource attribute to create caching by ip*/
        //[CacheResource]
        /* Filter action attribute to create whitelist by ip */
        [TypeFilter(typeof(IPAddressFilter))]
        public IActionResult Index()
        {
            _logger.LogDebug("HomeController: Successful get.");
            return Content("This content was generated at " + DateTime.Now);
        }
    }
}
