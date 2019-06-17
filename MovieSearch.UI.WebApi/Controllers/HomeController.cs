using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieSearch.UI.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Home")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Returns current time to detect if app is up
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetHome()
        {
            return Ok(DateTime.Now);
        }
    }
}