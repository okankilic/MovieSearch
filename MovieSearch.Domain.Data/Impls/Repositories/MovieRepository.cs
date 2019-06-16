using Microsoft.EntityFrameworkCore;
using MovieSearch.Domain.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearch.Domain.Data.Impls.Repositories
{
    public class MovieRepository : GenericRepository<Movie>
    {
        public MovieRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
