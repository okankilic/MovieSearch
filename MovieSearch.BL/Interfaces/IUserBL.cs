using MovieSearch.Domain.Data.Interfaces;
using MovieSearch.Domain.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearch.BL.Interfaces
{
    public interface IUserBL
    {
        Task<string> LoginAsync(string email, string password, IUnitOfWork uow);

        Task<List<User>> GetListAsync(IUnitOfWork uow);
    }
}
