using MovieSearch.Domain.Data.Impls.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearch.Domain.Data.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        UserRepository UserRepository { get; }
        MovieRepository MovieRepository { get; }

        void SaveChanges();

        void Commit();
    }
}
