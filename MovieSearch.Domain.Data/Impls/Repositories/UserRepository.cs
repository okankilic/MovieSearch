using Microsoft.EntityFrameworkCore;
using MovieSearch.Domain.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearch.Domain.Data.Impls.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
