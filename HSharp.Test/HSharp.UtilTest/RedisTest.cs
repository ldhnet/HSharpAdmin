using HSharp.Util.Context;
using HSharp.Util.Global;
using HSharp.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

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
            var inst = HSharpRedisContext.GetRedisClient();
            inst.Set("lee:demo:testString", "1232222");

            var aa = inst.Get("lee:demo:testString");
        }
        [Test]
        public void Redis_Get()
        {
            HSharpRedisContext.Get("test");
        }

        [Test]
        public void Redis_LockTake()
        {
            HSharpRedisContext.LockTake("test", () => {
                Console.WriteLine("test执行");
            });
        }

        private static readonly string channel = "channelMY-lee";
        [Test]
        public void Redis_PublishAndSubscribe()
        { 
            var inst = HSharpRedisContext.GetRedisClient();
            inst.Publish(channel, "lee test 666");


            inst.Subscribe((channel, msg =>
             {
                 Console.WriteLine($"SubscribeMessage执行：{msg}");
             }));
        
        }


        private static void PublishMsg(string message)
        {
            HSharpRedisContext.PublishMessage(channel, message);
        }
         
        private static void SubscribeMsg()
        {
            HSharpRedisContext.SubscribeMessage(channel, (msg) =>
            {
                Console.WriteLine($"SubscribeMessage执行：{msg}");
            });
        }
    }
}
