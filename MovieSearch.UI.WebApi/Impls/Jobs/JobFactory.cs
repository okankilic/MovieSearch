using Microsoft.Extensions.Logging;
using MovieSearch.BL.Interfaces;
using MovieSearch.Domain.Data.Interfaces;
using MovieSearch.UI.WebApi.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.UI.WebApi.Impls.Jobs
{
    public class JobFactory : IJobFactory
    {
        private readonly IMovieBL movieBL;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly ILogger<JobFactory> logger;

        public JobFactory(IMovieBL movieBL,
            IUnitOfWorkFactory unitOfWorkFactory,
            ILogger<JobFactory> logger)
        {
            this.movieBL = movieBL;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.logger = logger;
        }

        public IJob CreateNew(string jobName)
        {
            if (jobName.Equals("UpdateMoviesJob"))
            {
                return new UpdateMoviesJob(movieBL, unitOfWorkFactory, logger);
            }

            return null;
        }
    }
}
