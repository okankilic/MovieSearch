using Microsoft.EntityFrameworkCore;
using MovieSearch.Domain.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearch.Domain.Data.Impls
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly MovieSearchDbContext dbContext;

        public UnitOfWorkFactory(MovieSearchDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IUnitOfWork CreateNew()
        {
            return new UnitOfWork(dbContext);
        }
    }
}
