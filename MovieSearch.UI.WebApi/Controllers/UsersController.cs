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
using MovieSearch.Domain.Data.Interfaces;
using MovieSearch.Domain.Data.Models;
using MovieSearch.UI.WebApi.Impls;

namespace MovieSearch.UI.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : CustomControllerBase
    {
        private readonly IUserBL userBL;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public UsersController(IUserBL userBL,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.userBL = userBL;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        // GET: api/Movies
        /// <summary>
        /// Return All Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            List<User> userList = null;

            using (var uow = unitOfWorkFactory.CreateNew())
            {
                userList = await userBL.GetListAsync(uow);
            }

            return Ok(userList);
        }
        
    }
}