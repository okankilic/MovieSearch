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
    public class UpdateMoviesJob: IJob
    {
        private readonly IMovieBL movieBL;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly ILogger<JobFactory> logger;

        public UpdateMoviesJob(IMovieBL movieBL,
            IUnitOfWorkFactory unitOfWorkFactory,
            ILogger<JobFactory> logger)
        {
            this.movieBL = movieBL;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.logger = logger;
        }

        public void DoJob()
        {
            try
            {
                using (var uow = unitOfWorkFactory.CreateNew())
                {
                    movieBL.UpdateAll(uow).Wait();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured in UpdateMoviesJob");
            }
        }
    }
}
