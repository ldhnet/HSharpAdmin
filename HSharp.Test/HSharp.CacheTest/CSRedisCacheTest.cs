using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using HSharp.Cache.Factory;
using HSharp.Util;
using HSharp.Util.Model; 
using HSharp.Business.SystemManage;
using HSharp.Entity.SystemManage;
using HSharp.Util.Global;
using HSharp.Util.Context;

namespace HSharp.CacheTest
{
    public class CSRedisCacheTest
    {
        [SetUp]
        public void Init()
        { 
            GlobalContext.RedisConfig  = new RedisConfig {
                ConnectionString = "127.0.0.1:6379,defaultDatabase=1,ssl=false,writeBuffer=10240"
            }; 
        }

        [Test]
        public void TestRedisSimple()
        {
            string key = "test_simple_key";
            string value = "test_simple_value";
            HSharpRedisContext.Set(key, value);


            var valueStr = HSharpRedisContext.Get(key);

            Assert.Equals(value, HSharpRedisContext.Get(key));
        }
         
    }
}