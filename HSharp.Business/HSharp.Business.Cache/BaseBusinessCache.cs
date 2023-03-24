using HSharp.Cache.Factory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HSharp.Business.Cache
{
    public abstract class BaseBusinessCache<T>
    {
        public abstract string CacheKey { get; }

        public virtual bool Remove()
        {
            return CacheFactory.Cache.RemoveCache(CacheKey);
        }

        public virtual Task<List<T>> GetList()
        {
            throw new Exception("请在子类实现");
        }
    }
}