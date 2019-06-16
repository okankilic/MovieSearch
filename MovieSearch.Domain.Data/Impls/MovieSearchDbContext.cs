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

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //}

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}
