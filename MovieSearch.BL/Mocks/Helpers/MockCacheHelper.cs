using MovieSearch.BL.Interfaces.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearch.BL.Mocks.Helpers
{
    public class MockCacheHelper : ICacheHelper
    {
        public void Clear()
        {
            
        }

        public string GenerateCacheKey(string cacheKey, params object[] args)
        {
            return null;
        }

        public object Get(string cacheKey)
        {
            return null;
        }

        public void Set(string cacheKey, object cacheObject)
        {
            
        }
    }
}
