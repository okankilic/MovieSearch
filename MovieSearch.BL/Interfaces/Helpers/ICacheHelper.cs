using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearch.BL.Interfaces.Helpers
{
    public interface ICacheHelper
    {
        string GenerateCacheKey(string cacheKey, params object[] args);

        void Reset(string cacheKey);

        object Get(string cacheKey);

        void Set(string cacheKey, object cacheObject);
    }
}
