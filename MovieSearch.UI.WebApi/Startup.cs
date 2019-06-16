using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MovieSearch.BL.Impls;
using MovieSearch.BL.Impls.Helpers;
using MovieSearch.BL.Impls.Services;
using MovieSearch.BL.Intefaces.Services;
using MovieSearch.BL.Interfaces;
using MovieSearch.BL.Interfaces.Helpers;
using MovieSearch.Domain.Data.Impls;
using MovieSearch.Domain.Data.Interfaces;
using MovieSearch.UI.WebApi.Impls.Filters;
using System;
using System.Text;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Logging;
using MovieSearch.UI.WebApi.Impls.Jobs;
using MovieSearch.UI.WebApi.Intefaces;

namespace MovieSearch.UI.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("MovieSearchDbContext");

            var secret = Configuration.GetSection("Jwt").GetValue<string>("Secret");

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services
                .AddMvc(config =>
                {
                    config.Filters.Add(typeof(CustomExceptionFilter));
                });

            services
                .AddDbContext<MovieSearchDbContext>(options =>
                {
                    //options
                    //    .UseInMemoryDatabase("MovieSearchDbContext")
                    //    .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                    options.UseSqlServer(connectionString);
                });

            services
                .AddHttpClient();

            services
                .AddMemoryCache();

            services.AddTransient<IMovieService, OmDbMovieService>();

            services.AddSingleton<ICacheHelper, CacheHelper>();

            services.AddTransient<IMovieBL, MovieBL>();
            services.AddTransient<IUserBL, UserBL>();

            services.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();

            services.AddTransient<IJobFactory, JobFactory>();

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            var containerBuilder = new ContainerBuilder();

            containerBuilder.Populate(services);

            ApplicationContainer = containerBuilder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IBackgroundJobClient backgroundJobs,
            IHostingEnvironment env,
            IJobFactory jobFactory)
        {
            int hangfireIntervalInMinutes = Configuration.GetSection("Hangfire").GetValue<int>("IntervalInMinutes");

            var cronString = Cron.MinuteInterval(hangfireIntervalInMinutes);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseHangfireDashboard();

            RecurringJob.AddOrUpdate(() => jobFactory.CreateNew("UpdateMoviesJob").DoJob(), cronString);

            app.UseMvc();
        }
    }
}
