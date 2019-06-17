using MovieSearch.BL.Intefaces.Services;
using MovieSearch.Domain.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearch.BL.Mocks.Services
{
    public class MockMovieService : IMovieService
    {
        public async Task<Movie> GetByIdAsync(string id)
        {
            return null;
        }

        public async Task<Movie> Search(string s)
        {
            return null;
        }
    }
}
