using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearch.Domain.Data.Interfaces
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateNew();
    }
}
