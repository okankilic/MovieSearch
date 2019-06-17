using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieSearch.BL.Impls;
using MovieSearch.BL.Impls.Helpers;
using MovieSearch.BL.Intefaces.Services;
using MovieSearch.BL.Interfaces;
using MovieSearch.BL.Interfaces.Helpers;
using MovieSearch.BL.Mocks.Helpers;
using MovieSearch.BL.Mocks.Services;
using MovieSearch.Domain.Data.Impls;
using MovieSearch.Domain.Data.Interfaces;
using MovieSearch.Domain.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.BL.Tests
{
    [TestClass]
    public class MovieBLTests
    {
        private MovieSearchDbContext movieSearchDbContext;
        private IUnitOfWork unitOfWork;
        private IMovieService movieService;
        private IMovieBL movieBL;
        private ICacheHelper cacheHelper;

        public MovieBLTests()
        {

        }

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<MovieSearchDbContext>()
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .UseInMemoryDatabase("MovieSearchInMemoryDb")
                .Options;

            movieSearchDbContext = new MovieSearchDbContext(options);
            unitOfWork = new UnitOfWork(movieSearchDbContext);
            movieService = new MockMovieService();
            cacheHelper = new MockCacheHelper();
            movieBL = new MovieBL(movieService, cacheHelper);
        }

        [TestMethod]
        public void ShouldCreateMovie()
        {
            var movie = new Movie()
            {
                ImdbId = "x1234",
                Title = "Test Movie"
            };

            movieBL.Create(movie, unitOfWork);

            Assert.AreEqual(1, movieSearchDbContext.Movies.Count());
        }

        [TestMethod]
        public void ShouldFindMovie()
        {
            var movieTask = Task.Run<Movie>(async () => await movieBL.SearchAsync("Test", unitOfWork));

            var movie = movieTask.Result;

            Assert.IsNotNull(movie);
        }
    }
}
