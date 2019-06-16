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
            movie.LastUpdateTime = DateTime.Now;

            uow.MovieRepository.Create(movie);

            uow.SaveChanges();
            uow.Commit();
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
                Create(movie, uow);
            }

            return movie;
        }

        public async Task UpdateAll(IUnitOfWork uow)
        {
            var movies = uow.MovieRepository.Find();

            if(movies.Count() == 0)
            {
                return;
            }

            foreach (var exMovie in movies)
            {
                var movie = await movieService.GetByIdAsync(exMovie.ImdbId);

                exMovie.Title = movie.Title;
                exMovie.Year = movie.Year;
                exMovie.Type = movie.Type;
                exMovie.Poster = movie.Poster;

                exMovie.LastUpdateTime = DateTime.Now;
            }

            uow.SaveChanges();
            uow.Commit();
        }
    }
}
