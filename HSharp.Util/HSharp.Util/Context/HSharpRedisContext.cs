using CSRedis; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HSharp.Util.Context
{
    /// <summary>
    /// Redis工具类
    /// </summary>
    public class HSharpRedisContext
    {
        /// <summary>
        /// 私有化构造函数
        /// 用于单例模式
        /// </summary>
        private HSharpRedisContext() { }

        /// <summary>
        /// Lazy对象
        /// </summary>
        private static readonly Lazy<CSRedisClient> LazyInstance = new Lazy<CSRedisClient>(() =>
        {
            if (!GlobalContext.RedisConfig.SupportRedisSentinel)
            {
                var redisClient = new CSRedisClient(GlobalContext.RedisConfig.ConnectionString);
                RedisHelper.Initialization(redisClient);
                return redisClient;
            }
            else
            {
                var redisClient = new CSRedisClient(
                    GlobalContext.RedisConfig.ConnectionString,
                    GlobalContext.RedisConfig.RedisSentinels.Split(',')
                    );
                RedisHelper.Initialization(redisClient);
                return redisClient;
            }
        });

        /// <summary>
        /// 单例对象
        /// </summary>
        public static CSRedisClient Instance { get { return LazyInstance.Value; } }

        /// <summary>
        /// 是否已创建
        /// </summary>
        public static bool IsInstanceCreated { get { return LazyInstance.IsValueCreated; } }
        /// <summary>
        /// 用于单元测试
        /// </summary>
        /// <returns></returns>

        public static CSRedisClient GetRedisClient()
        {
            var redisClient = new CSRedisClient("127.0.0.1:6379");
            RedisHelper.Initialization(redisClient);
            return redisClient;
        }

        /// <summary>
        /// 依据Key获取字符串
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>字符串</returns>
        public static string Get(string key)
        {
            return Instance.Get(key);
        }

        /// <summary>
        /// 依据Key获取字符串
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>字符串</returns>
        public static async Task<string> GetAsync(string key)
        {
            return await Instance.GetAsync(key);
        }

        /// <summary>
        /// 缓存对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="obj">对象</param>
        /// <param name="expireSeconds">时效（秒）</param>
        public static void Set(string key, object obj, int expireSeconds = 0)
        {
            Instance.Set(key, obj, expireSeconds);
        }

        /// <summary>
        /// 缓存对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="obj">对象</param>
        /// <param name="expireSeconds">时效（秒）</param>
        /// <returns></returns>
        public static async Task SetAsync(string key, object obj, int expireSeconds = 0)
        {
            await Instance.SetAsync(key, obj, expireSeconds);
        }

        /// <summary>
        /// 获取泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>泛型对象</returns>
        public static T Get<T>(string key) where T : new()
        {
            return Instance.Get<T>(key);
        }

        /// <summary>
        /// 获取泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>泛型对象</returns>
        public static async Task<T> GetAsync<T>(string key) where T : new()
        {
            return await Instance.GetAsync<T>(key);
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns>字符串</returns>
        public static string GetHash(string key, string field)
        {
            return Instance.HGet(key, field);
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns>字符串</returns>
        public static async Task<String> GetHashAsync(string key, string field)
        {
            return await Instance.HGetAsync(key, field);
        }

        /// <summary>
        /// 缓存哈希表的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns>标志</returns>
        public static bool SetHash(string key, string field, string value)
        {
            return Instance.HSet(key, field, value);
        }

        /// <summary>
        /// 缓存哈希表的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns>标志</returns>
        public static async Task<bool> SetHashAsync(string key, string field, string value)
        {
            return await Instance.HSetAsync(key, field, value);
        }

        /// <summary>
        /// 获取哈希表所有字段的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>字典</returns>
        public static Dictionary<string, string> GetHashAll(string key)
        {
            return Instance.HGetAll(key);
        }

        /// <summary>
        /// 获取哈希表所有字段的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>字典</returns>
        public static async Task<Dictionary<string, string>> GetHashAllAsync(string key)
        {
            return await Instance.HGetAllAsync(key);
        }

        /// <summary>
        /// 删除哈希表字段的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public static long DeleteHash(string key, string[] field)
        {
            return Instance.HDel(key, field);
        }

        /// <summary>
        /// 删除哈希表字段的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public static async Task<long> DeleteHashAsync(string key, string[] field)
        {
            return await Instance.HDelAsync(key, field);
        }

        /// <summary>
        /// 获取哈希表泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <param name="field">字段</param>
        /// <returns>泛型对象</returns>
        public static T GetHash<T>(string key, string field) where T : new()
        {
            return Instance.HGet<T>(key, field);
        }

        /// <summary>
        /// 获取哈希表泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <param name="field">字段</param>
        /// <returns>泛型对象</returns>
        public static async Task<T> GetHashAsync<T>(string key, string field) where T : new()
        {
            return await Instance.HGetAsync<T>(key, field);
        }

        /// <summary>
        /// 获取哈希表所有泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>字典</returns>
        public static Dictionary<string, T> GetHashAll<T>(string key) where T : new()
        {
            return Instance.HGetAll<T>(key);
        }

        /// <summary>
        /// 获取哈希表所有泛型对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>字典</returns>
        public static async Task<Dictionary<string, T>> GetHashAllAsync<T>(string key) where T : new()
        {
            return await Instance.HGetAllAsync<T>(key);
        }

        /// <summary>
        /// 删除Key
        /// </summary>
        /// <param name="keys">key</param>
        /// <returns></returns>
        public static long Delete(string[] keys)
        {
            return Instance.Del(keys);
        }

        /// <summary>
        /// 删除Key
        /// </summary>
        /// <param name="keys">key</param>
        /// <returns></returns>
        public static async Task<long> DeleteAsync(string[] keys)
        {
            return await Instance.DelAsync(keys);
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            return Instance.Exists(key);
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="keys">key</param>
        /// <returns></returns>
        public static long Exists(string[] keys)
        {
            return Instance.Exists(keys);
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static async Task<bool> ExistsAsync(string key)
        {
            return await Instance.ExistsAsync(key);
        }

        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="keys">key</param>
        /// <returns></returns>
        public static async Task<long> ExistsAsync(string[] keys)
        {
            return await Instance.ExistsAsync(keys);
        }

        /// <summary>
        /// 查看哈希表 key 中，指定的字段是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="field">字段</param>
        /// <returns>bool</returns>
        public static bool HExists(string key, string field)
        {
            return Instance.HExists(key, field);
        }

        /// <summary>
        /// 查看哈希表 key 中，指定的字段是否存在
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="field">字段</param>
        /// <returns>bool</returns>
        public static async Task<bool> HExistsAsync(string key, string field)
        {
            return await Instance.HExistsAsync(key, field);
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="expire">时间间隔</param>
        /// <returns>标志</returns>
        public static bool Expire(string key, TimeSpan expire)
        {
            return Instance.Expire(key, expire);
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="seconds">秒数</param>
        /// <returns>标志 </returns>
        public static bool Expire(string key, int seconds)
        {
            return Instance.Expire(key, seconds);
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="expire">时间间隔</param>
        /// <returns>标志</returns>
        public static async Task<bool> ExpireAsync(string key, TimeSpan expire)
        {
            return await Instance.ExpireAsync(key, expire);
        }

        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="seconds">秒数</param>
        /// <returns>标志</returns>
        public static async Task<bool> ExpireAsync(string key, int seconds)
        {
            return await Instance.ExpireAsync(key, seconds);
        }
         
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="channel">通道</param>
        /// <param name="message">消息</param>
        public static void PublishMessage(string channel, string message)
        {
            Instance.Publish(channel, message);
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="channel">通道</param>
        /// <param name="callBack">回调函数:通道,消息</param>
        public static void SubscribeMessage(string channel, Action<string> callBack)
        {
            Instance.Subscribe((channel, msg =>
            {
                callBack(msg.Body);
            }
            ));
        }

        /// <summary>
        /// 获取指定值
        /// </summary>
        /// <param name="connString">连接串</param>
        /// <param name="key">指定键</param>
        /// <returns>键值</returns>
        public static string GetDefaultValue(string connString, string key)
        {
            var pattern = string.Empty;
            var array = connString.Split(',');

            foreach (var item in array)
            {
                if (item.IndexOf(key) > 0)
                {
                    pattern = item;
                    break;
                }
            }

            var value = pattern.Split('=')[1];
            return value;
        }

        ///  如果不存在则执行，存在则忽略
        /// <param name="value"></param>
        public static bool SetNx(string key, string value, TimeSpan? expire = null)
        {
            var res = false;
            if (Instance.SetNx(key, value)) {
                var expireSeconds = expire ?? TimeSpan.FromSeconds(5);
                res = Instance.Expire(key, expireSeconds);
            }
            return res;
        }  
        /// <summary>
        /// 分布式锁
        /// </summary>
        /// <param name="lockKey">锁名称，不可重复</param>
        /// <param name="action">委托事件</param>
        /// <returns>bool</returns>
        public static bool LockTake(string lockKey, Action action)
        {
            var result = false; 
            //token用来标识谁拥有该锁并用来释放锁。
            var token = "lockNX:"+ Environment.MachineName; 
            if (SetNx(lockKey, token))
            {
                action();
                result = true;
            } 
            return result;
        }

        /// <summary>
        /// 删除共享锁
        /// </summary>
        /// <param name="key">锁名称，不可重复</param>
        /// <returns></returns>
        public static bool ReleaseLock(string key, string token)
        {
            var script = GetReleaseLockScript();
            var res = Instance.Eval(script, key, token);
            if (0 == (long)res)
            {
                return false;
            }
            return true;
        }

        ///  lua脚本删除共享锁
        ///  解决在A申请锁 xxkey  过期的瞬间，B 申请锁xxkey,
        ///  此时恰好A执行到释放xxkey从而引起的异常释放
        private static string GetReleaseLockScript()
        {
            return "if redis.call(\"get\",KEYS[1]) == ARGV[1] \nthen\nreturn redis.call(\"del\", KEYS[1])\nelse\nreturn 0\nend";
        }
    }
}
