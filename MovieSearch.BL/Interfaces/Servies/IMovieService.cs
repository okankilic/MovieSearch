using MovieSearch.Domain.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.BL.Intefaces.Services
{
    public interface IMovieService
    {
        Task<Movie> Search(string s);
        Task<Movie> GetByIdAsync(string id);
    }
}
