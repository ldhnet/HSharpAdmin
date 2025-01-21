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
using StackExchange.Redis;

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
        public void Redis_ZAdd()
        {
            var csredis = HSharpRedisContext.GetRedisClient();
            csredis.ZAdd("lee:demoZSet:Quiz", (79, "Math"));
            csredis.ZAdd("lee:demoZSet:Quiz", (98, "English"));
            csredis.ZAdd("lee:demoZSet:Quiz", (87, "Algorithm"));
            csredis.ZAdd("lee:demoZSet:Quiz", (84, "Database"));
            csredis.ZAdd("lee:demoZSet:Quiz", (59, "Operation System"));

            //返回集合中的元素数量
            var aa = csredis.ZCard("lee:demoZSet:Quiz"); 

            // 获取集合中指定范围(90~100)的元素集合
            var list1=  csredis.ZRangeByScore("lee:demoZSet:Quiz", 90, 100);

            // 获取集合所有元素并升序排序
            var list2 = csredis.ZRangeWithScores("lee:demoZSet:Quiz", 0, -1);

            // 移除集合中的元素
            //csredis.ZRem("lee:demoZSet:Quiz", "Math");
        }
        [Test]
        public void Redis_Set()
        {
            var inst = HSharpRedisContext.GetRedisClient();
            inst.Set("lee:demo:testString", "1232222");

            var aa = inst.Get("lee:demo:testString");
        }
        [Test]
        public void Redis_Set_expire()
        {
            var inst = HSharpRedisContext.GetRedisClient();
            inst.Set("lee:demoExpire:testString", "6666666", 60);
            //Task.Delay(15000).Wait();
            var aa = inst.Get("lee:demoExpire:testString");
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
