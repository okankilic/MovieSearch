using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieSearch.BL.Interfaces;
using MovieSearch.Domain.Data.Impls.Helpers;
using MovieSearch.Domain.Data.Interfaces;
using MovieSearch.Domain.Data.Models;
using MovieSearch.Domain.Data.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearch.BL.Impls
{
    public class UserBL : IUserBL
    {
        private readonly IConfiguration configuration;

        public UserBL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<List<User>> GetListAsync(IUnitOfWork uow)
        {
            var userList = await uow.UserRepository.Find().ToListAsync();

            return userList;
        }

        private async Task<User> SearchAsync(string email, string password, IUnitOfWork uow)
        {
            var hashedPassword = AuthHelper.HashPassword(password);

            var users = uow.UserRepository.Find(q => q.Email.Equals(email) && q.Password.Equals(hashedPassword));

            var user = await users.FirstOrDefaultAsync();

            return user;
        }

        public async Task<string> LoginAsync(string email, string password, IUnitOfWork uow)
        {
            ValidateForLogin(email, password);

            var dbUser = await SearchAsync(email, password, uow);
            if (dbUser == null)
            {
                throw new BusinessException("Invalid email or password");
            }

            return CreateToken(dbUser);
        }

        private string CreateToken(User dbUser)
        {
            var secret = configuration.GetSection("Jwt").GetValue<string>("Secret");

            var tokenHandler = new JwtSecurityTokenHandler();
            var bytes = Encoding.UTF8.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, dbUser.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(bytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        private static void ValidateForLogin(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new BusinessException("Invalid Email adress");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new BusinessException("Invalid password");
            }
        }
    }
}
