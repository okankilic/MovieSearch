using Microsoft.EntityFrameworkCore;
using MovieSearch.Domain.Data.Models;
using System;

namespace MovieSearch.Domain.Data.Impls
{
    public class MovieSearchDbContext: DbContext
    {
        public MovieSearchDbContext(DbContextOptions<MovieSearchDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}
