﻿using HSharp.Cache.Factory;
using HSharp.Entity.SystemManage;
using HSharp.Service.SystemManage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HSharp.Business.Cache
{
    public class DataDictDetailCache : BaseBusinessCache<DataDictDetailEntity>
    {
        private DataDictDetailService dataDictDetailService = new DataDictDetailService();

        public override string CacheKey => this.GetType().Name;

        public override async Task<List<DataDictDetailEntity>> GetList()
        {
            var cacheList = CacheFactory.Cache.GetCache<List<DataDictDetailEntity>>(CacheKey);
            if (cacheList == null)
            {
                var list = await dataDictDetailService.GetList(null);
                CacheFactory.Cache.SetCache(CacheKey, list);
                return list;
            }
            else
            {
                return cacheList;
            }
        }
    }
}