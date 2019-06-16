using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MovieSearch.Domain.Data.Impls.Repositories;
using MovieSearch.Domain.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearch.Domain.Data.Impls
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly DbContext dbContext;

        private readonly IDbContextTransaction dbContextTransaction;

        private bool isDisposed = false;

        private UserRepository userRepository;
        private MovieRepository movieRepository;

        public UserRepository UserRepository
        {
            get
            {
                if(userRepository == null)
                {
                    userRepository = new UserRepository(dbContext);
                }

                return userRepository;
            }
        }

        public MovieRepository MovieRepository
        {
            get
            {
                if (movieRepository == null)
                {
                    movieRepository = new MovieRepository(dbContext);
                }

                return movieRepository;
            }
        }

        public UnitOfWork(DbContext dbContext)
        {
            this.dbContext = dbContext;

            dbContextTransaction = dbContext.Database.BeginTransaction();
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }

        public void Commit()
        {
            try
            {
                dbContext.SaveChanges();

                dbContextTransaction.Commit();
            }
            catch (Exception)
            {
                dbContextTransaction.Rollback();
                throw;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    if (dbContextTransaction != null)
                    {
                        dbContextTransaction.Dispose();
                    }

                    if (dbContext != null)
                    {
                        dbContext.Dispose();
                    }
                }

                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
