using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieSearch.BL.Interfaces;
using MovieSearch.Domain.Data;
using MovieSearch.Domain.Data.Impls;
using MovieSearch.Domain.Data.Impls.Helpers;
using MovieSearch.Domain.Data.Interfaces;
using MovieSearch.Domain.Data.Models;
using MovieSearch.UI.WebApi.Impls;

namespace MovieSearch.UI.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : CustomControllerBase
    {
        private readonly IUserBL userBL;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public AuthController(IUserBL userBL,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.userBL = userBL;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <summary>
        /// Login Funcionality
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody]User user)
        {
            if(user == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            User dbUser = null;

            using (var uow = unitOfWorkFactory.CreateNew())
            {
                dbUser = await userBL.SearchAsync(user.Email, user.Password, uow);
            }

            if (dbUser == null)
            {
                return Unauthorized();
            }

            var token = AuthHelper.CreateToken(dbUser.Email);

            return Ok(new { Token = token });
        }
        
    }
}