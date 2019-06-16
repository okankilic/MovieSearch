using Microsoft.AspNetCore.Mvc;
using MovieSearch.BL.Interfaces;
using MovieSearch.Domain.Data.Interfaces;
using MovieSearch.Domain.Data.Models;
using MovieSearch.Domain.Data.Models.Exceptions;
using MovieSearch.UI.WebApi.Impls;
using System.Threading.Tasks;

namespace MovieSearch.UI.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Movies")]
    public class MoviesController : CustomControllerBase
    {
        private readonly IMovieBL movieBL;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public MoviesController(IMovieBL movieBL,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.movieBL = movieBL;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        // GET: api/Movies
        /// <summary>
        /// Search
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMovies(string s = null)
        {
            Movie movie = null;

            using (var uow = unitOfWorkFactory.CreateNew())
            {
                movie = await movieBL.SearchAsync(s, uow);
            }

            return Ok(movie);
        }

        // GET: api/Movies/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetMovie([FromRoute] string id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var movie = await context.Movies.SingleOrDefaultAsync(m => m.ImdbId == id);

        //    if (movie == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(movie);
        //}
    }
}