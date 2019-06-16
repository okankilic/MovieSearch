using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using MovieSearch.BL.Interfaces.Helpers;
using MovieSearch.Domain.Data.Interfaces;
using MovieSearch.Domain.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MovieSearch.BL.Impls.Helpers
{
    public class CacheHelper: ICacheHelper
    {
        static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        const string RegionName = "MovieSearch";

        private readonly IMemoryCache cache;
        private readonly IConfiguration configuration;

        private TimeSpan slidingExpirationTime = TimeSpan.MinValue;

        public TimeSpan SlidingExpirationTime
        {
            get
            {
                if(slidingExpirationTime == TimeSpan.MinValue)
                {
                    var minutes = configuration.GetSection("Caching").GetValue<int>("SlidingExpirationTime");

                    slidingExpirationTime = TimeSpan.FromMinutes(minutes);
                }

                return slidingExpirationTime;
            }
        }

        public CacheHelper(IMemoryCache cache,
            IConfiguration configuration)
        {
            this.cache = cache;
            this.configuration = configuration;
        }

        public string GenerateCacheKey(string cacheKey, params object[] args)
        {
            var stringBuilder = new StringBuilder(RegionName);

            stringBuilder.Append("::");
            stringBuilder.Append(cacheKey);

            foreach (var arg in args)
            {
                if (arg is IUnitOfWork
                    || arg is Movie
                    || arg is User
                    //|| arg is AccountDTO
                    //|| arg is CountryDTO
                    //|| arg is CityDTO
                    //|| arg is ProjectDTO
                    //|| arg is StoreDTO
                    //|| arg is IssueDTO
                    )
                {
                    continue;
                }

                if (arg is Array)
                {
                    var subArgs = arg as Array;

                    foreach (var subArg in subArgs)
                    {
                        if (subArg is IUnitOfWork
                            || subArg is Movie
                            || subArg is User
                    //|| subArg is AccountDTO
                    //|| subArg is CountryDTO
                    //|| subArg is CityDTO
                    //|| subArg is ProjectDTO
                    //|| subArg is StoreDTO
                    //|| subArg is IssueDTO
                    )
                        {
                            continue;
                        }

                        stringBuilder.Append("_");
                        stringBuilder.Append(subArg);
                    }
                }
                else
                {
                    stringBuilder.Append("_");
                    stringBuilder.Append(arg);
                }
            }

            return stringBuilder.ToString();
        }

        public void Clear()
        {
            if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested && cancellationTokenSource.Token.CanBeCanceled)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }

            cancellationTokenSource = new CancellationTokenSource();
        }

        public object Get(string cacheKey)
        {
            if(cache.TryGetValue(cacheKey, out object value))
            {
                return value;
            }

            return null;
        }

        public void Set(string cacheKey, object cacheObject)
        {
            var options = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(SlidingExpirationTime)
                .AddExpirationToken(new CancellationChangeToken(cancellationTokenSource.Token));

            cache.Set(cacheKey, cacheObject, options);
        }
    }
}
