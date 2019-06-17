using Microsoft.AspNetCore.Mvc;
using MovieSearch.BL.Interfaces;
using MovieSearch.Domain.Data.Impls;
using MovieSearch.Domain.Data.Interfaces;
using MovieSearch.Domain.Data.Models;
using MovieSearch.UI.WebApi.Impls;
using System.Threading.Tasks;

namespace MovieSearch.UI.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        private readonly IUserBL userBL;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public AuthController(IUserBL userBL,
            IUnitOfWorkFactory unitOfWorkFactory,
            MovieSearchDbContext movieSearchDbContext)
        {
            this.userBL = userBL;
            this.unitOfWorkFactory = unitOfWorkFactory;

            SeedData.Initialize(movieSearchDbContext);
        }

        /// <summary>
        /// Login Funcionality
        /// </summary>
        /// <param name="user"></param>
        /// <returns>token</returns>
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

            string token = string.Empty;

            using (var uow = unitOfWorkFactory.CreateNew())
            {
                token = await userBL.LoginAsync(user.Email, user.Password, uow);
            }

            return Ok(new { Token = token });
        }
        
    }
}