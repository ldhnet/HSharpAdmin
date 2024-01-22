using System;

namespace HSharp.Util.Global
{
    /// <summary>
    /// Redis配置
    /// </summary>
    public class RedisConfig
    {
        /// <summary>
        /// Redis连接串
        /// </summary>
        public String ConnectionString { get; set; }

        /// <summary>
        /// 用户模糊搜索格式
        /// </summary>
        public String Pattern { get; set; }

        /// <summary>
        /// 统一认证模糊搜索格式
        /// </summary>
        public String UnifiedAuthenticationPattern { get; set; }

        /// <summary>
        /// 同步模糊搜索格式
        /// </summary>
        public String SynchronizationPattern { get; set; }

        /// <summary>
        /// 是否支持哨兵
        /// </summary>
        public bool SupportRedisSentinel { get; set; }

        /// <summary>
        /// 哨兵集群
        /// </summary>
        public String RedisSentinels { get; set; }
    }
}
