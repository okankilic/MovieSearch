using Microsoft.EntityFrameworkCore;
using MovieSearch.BL.Intefaces.Services;
using MovieSearch.BL.Interfaces;
using MovieSearch.Domain.Data.Interfaces;
using MovieSearch.Domain.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.BL.Impls
{
    public class MovieBL: IMovieBL
    {
        private readonly IMovieService movieService;

        public MovieBL(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        public void Create(Movie movie, IUnitOfWork uow)
        {
            uow.MovieRepository.Create(movie);
            uow.SaveChanges();
        }

        public async Task<Movie> SearchAsync(string s, IUnitOfWork uow)
        {
            var movies = uow.MovieRepository.Find(q => q.Title.ToUpperInvariant().Contains(s.ToUpperInvariant()));

            var movie = await movies.FirstOrDefaultAsync();

            if(movie == null)
            {
                movie = await SearchInService(s, uow, movie);
            }

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
