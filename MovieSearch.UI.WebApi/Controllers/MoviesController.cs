using Microsoft.AspNetCore.Mvc;
using MovieSearch.BL.Interfaces;
using MovieSearch.Domain.Data.Interfaces;
using MovieSearch.Domain.Data.Models;
using MovieSearch.Domain.Data.Models.Exceptions;
using MovieSearch.UI.WebApi.Impls;
using MovieSearch.UI.WebApi.Models;
using System.Threading.Tasks;

namespace MovieSearch.UI.WebApi.Controllers
{
    /// <summary>
    /// Movie operations
    /// </summary>
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

        /// <summary>
        /// Search movie by title
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostMovie([FromBody]SearchMovieRequest request)
        {
            Movie movie = null;

            using (var uow = unitOfWorkFactory.CreateNew())
            {
                movie = await movieBL.SearchAsync(request.SearchText, uow);
            }

            return Ok(movie);
        }
    }
}