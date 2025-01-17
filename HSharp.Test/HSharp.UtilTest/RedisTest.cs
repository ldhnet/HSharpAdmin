using HSharp.Util.Context;
using HSharp.Util.Global;
using HSharp.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSharp.UtilTest
{
    /// <summary>
    /// Redis工具类
    /// </summary>
    internal class RedisTest
    {
        [SetUp]
        public void Init()
        {
            GlobalContext.RedisConfig = new RedisConfig
            {
                ConnectionString = "guest" 

            };
        }

        [Test]
        public void Redis_Set()
        {
            HSharpRedisContext.Set("test", "123");
        }
        [Test]
        public void Redis_Get()
        {
            HSharpRedisContext.Get("test");
        }
    }
}
