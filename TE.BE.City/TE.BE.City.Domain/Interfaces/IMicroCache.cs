using System;
using System.Runtime.Caching;

namespace TE.BE.City.Domain.Interfaces
{
    public interface IMicroCache<T>
    {
        bool Contains(string key);
        T GetOrAdd(string key, Func<T> loadFunction, Func<CacheItemPolicy> getCacheItemPolicyFunction);
        void Remove(string key);
    }
}
