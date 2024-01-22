using System;

namespace HSharp.Util.Global
{
    /// <summary>
    /// RabbitMQ配置
    /// </summary>
    public class RabbitMQConfig
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// ip地址，多个时以英文“,”分割
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 虚拟队列名称
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// 虚拟交换机名称
        /// </summary>
        public string ExchangeName { get; set; }
    }
}
