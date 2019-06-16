using Microsoft.EntityFrameworkCore;
using MovieSearch.BL.Intefaces.Services;
using MovieSearch.BL.Interfaces;
using MovieSearch.BL.Interfaces.Helpers;
using MovieSearch.Domain.Data.Interfaces;
using MovieSearch.Domain.Data.Models;
using MovieSearch.Domain.Data.Models.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.BL.Impls
{
    public class MovieBL: IMovieBL
    {
        private readonly IMovieService movieService;
        private readonly ICacheHelper cacheHelper;

        public MovieBL(IMovieService movieService,
            ICacheHelper cacheHelper)
        {
            this.movieService = movieService;
            this.cacheHelper = cacheHelper;
        }

        public void Create(Movie movie, IUnitOfWork uow)
        {
            uow.MovieRepository.Create(movie);
            uow.SaveChanges();
        }

        public async Task<Movie> SearchAsync(string s, IUnitOfWork uow)
        {
            ValidateForSearch(s);

            var cacheKey = cacheHelper.GenerateCacheKey("SearchAsync", "s", s);
            var cached = cacheHelper.Get(cacheKey);

            if (cached != null)
            {
                return cached as Movie;
            }

            var movie = await SearchInDb(s, uow);
            if (movie == null)
            {
                movie = await SearchInService(s, uow, movie);
            }

            if (movie != null)
            {
                cacheHelper.Set(cacheKey, movie);
            }

            return movie;
        }

        private static void ValidateForSearch(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new BusinessException("Search parameter cannot be null or empty");
            }
        }

        private async Task<Movie> SearchInDb(string s, IUnitOfWork uow)
        {
            var movies = uow.MovieRepository.Find(q => q.Title.ToUpperInvariant().Contains(s.ToUpperInvariant()));

            var movie = await movies.FirstOrDefaultAsync();

            return movie;
        }

        private async Task<Movie> SearchInService(string s, IUnitOfWork uow, Movie movie)
        {
            movie = await movieService.Search(s);

            if (movie != null)
            {
                uow.MovieRepository.Create(movie);
                uow.SaveChanges();
                uow.Commit();
            }

            return movie;
        }
    }
}
