using Microsoft.AspNetCore.Mvc;
using MovieSearch.BL.Interfaces;
using MovieSearch.BL.Interfaces.Helpers;
using MovieSearch.Domain.Data.Impls;
using MovieSearch.Domain.Data.Interfaces;
using MovieSearch.Domain.Data.Models;
using MovieSearch.UI.WebApi.Impls;
using System.Threading.Tasks;

namespace MovieSearch.UI.WebApi.Controllers
{
    /// <summary>
    /// Cache operations
    /// </summary>
    [Produces("application/json")]
    [Route("api/Cache")]
    public class CacheController : Controller
    {
        private readonly ICacheHelper cacheHelper;
        
        public CacheController(ICacheHelper cacheHelper)
        {
            this.cacheHelper = cacheHelper;
        }

        /// <summary>
        /// Clears all cache
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("clear")]
        public IActionResult ClearCache()
        {
            cacheHelper.Clear();

            return Ok();
        }
        
    }
}