using MovieSearch.Domain.Data.Interfaces;
using MovieSearch.Domain.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearch.BL.Interfaces
{
    public interface IMovieBL
    {
        void Create(Movie movie, IUnitOfWork uow);

        Task<Movie> SearchAsync(string s, IUnitOfWork uow);
    }
}
