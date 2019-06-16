using MovieSearch.Domain.Data.Impls.Helpers;
using MovieSearch.Domain.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieSearch.Domain.Data.Impls
{
    public static class SeedData
    {
        public static void Initialize(MovieSearchDbContext context)
        {
            //if (context.Users.Any())
            //{
            //    foreach (var user in context.Users)
            //    {
            //        context.Users.Remove(user);
            //    }

            //    context.SaveChanges();
            //}

            if (!context.Users.Any())
            {
                context.Users.Add(new User()
                {
                    Email = "test@test.com",
                    Password = AuthHelper.HashPassword("test")
                });

                context.SaveChanges();
            }

            if (!context.Movies.Any())
            {
                context.Movies.AddRange(
                    new Movie()
                    {
                        ImdbId = "tt0468569",
                        Title = "The Dark Knight",
                        Year = "2018",
                        Type = "movie",
                        Poster = "https://m.media-amazon.com/images/M/MV5BMTMxNTMwODM0NF5BMl5BanBnXkFtZTcwODAyMTk2Mw@@._V1_SX300.jpg"
                    },
                    new Movie()
                    {
                        ImdbId = "tt1345836",
                        Title = "The Dark Knight Rises",
                        Year = "2012",
                        Type = "movie",
                        Poster = "https://m.media-amazon.com/images/M/MV5BMTk4ODQzNDY3Ml5BMl5BanBnXkFtZTcwODA0NTM4Nw@@._V1_SX300.jpg"
                    });

                context.SaveChanges();
            }
        }
    }
}
