using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MovieSearch.Domain.Data.Impls
{
    public class MovieSearchDbContextFactory : IDesignTimeDbContextFactory<MovieSearchDbContext>
    {
        public MovieSearchDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            
            var builder = new DbContextOptionsBuilder<MovieSearchDbContext>();

            var connectionString = configuration.GetConnectionString("MovieSearchDbContext");

            builder.UseSqlServer(connectionString);

            return new MovieSearchDbContext(builder.Options);
        }
    }
}
