using Microsoft.EntityFrameworkCore;
using MovieSearch.BL.Interfaces;
using MovieSearch.Domain.Data.Impls.Helpers;
using MovieSearch.Domain.Data.Interfaces;
using MovieSearch.Domain.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearch.BL.Impls
{
    public class UserBL : IUserBL
    {
        public UserBL()
        {

        }

        public async Task<List<User>> GetListAsync(IUnitOfWork uow)
        {
            var userList = await uow.UserRepository.Find().ToListAsync();

            return userList;
        }

        public async Task<User> SearchAsync(string email, string password, IUnitOfWork uow)
        {
            var hashedPassword = AuthHelper.HashPassword(password);

            var users = uow.UserRepository.Find(q => q.Email.Equals(email) && q.Password.Equals(hashedPassword));

            var user = await users.FirstOrDefaultAsync();

            return user;
        }
    }
}
