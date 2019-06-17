using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieSearch.BL.Impls;
using MovieSearch.BL.Impls.Helpers;
using MovieSearch.BL.Intefaces.Services;
using MovieSearch.BL.Interfaces;
using MovieSearch.BL.Interfaces.Helpers;
using MovieSearch.BL.Mocks.Helpers;
using MovieSearch.BL.Mocks.Services;
using MovieSearch.Domain.Data.Impls;
using MovieSearch.Domain.Data.Impls.Helpers;
using MovieSearch.Domain.Data.Interfaces;
using MovieSearch.Domain.Data.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.BL.Tests
{
    [TestClass]
    public class UserBLTests
    {
        private MovieSearchDbContext movieSearchDbContext;
        private IUnitOfWork unitOfWork;
        private IUserBL userBL;
        private IConfiguration configuration;

        public UserBLTests()
        {

        }

        [TestInitialize]
        public void Initialize()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<MovieSearchDbContext>()
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .UseInMemoryDatabase("MovieSearchInMemoryDb")
                .Options;

            movieSearchDbContext = new MovieSearchDbContext(options);
            unitOfWork = new UnitOfWork(movieSearchDbContext);
            userBL = new UserBL(configuration);

            movieSearchDbContext.Users.Add(new User()
            {
                Email = "test@test.com",
                Password = AuthHelper.HashPassword("test")
            });

            movieSearchDbContext.SaveChanges();
        }

        [TestMethod]
        public void ShouldReturnListOfUsers()
        {
            var task = Task.Run<List<User>>(async () => await userBL.GetListAsync(unitOfWork));

            var userList = task.Result;

            Assert.AreEqual(1, userList.Count);
        }
    }
}
