using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace HSharp.Util.Context
{
    /// <summary>
    /// RabbitMQ工具类
    /// </summary>
    public class HSharpRabbitMQContext
    {
        /// <summary>
        /// 私有化构造函数
        /// 用于单例模式
        /// </summary>
        private HSharpRabbitMQContext() { }

        /// <summary>
        /// Lazy对象
        /// </summary>
        private  static readonly  Lazy<IConnection> LazyConnection = new Lazy<IConnection>( () =>
        {
            ConnectionFactory factory = null;
            IConnection connection = null;

            #region 初始工厂类

            if (GlobalContext.RabbitMQConfig.HostName.Contains(","))
            {
                //创建连接对象工厂
                factory = new ConnectionFactory()
                {
                    UserName = GlobalContext.RabbitMQConfig.UserName,
                    Password = GlobalContext.RabbitMQConfig.Password,
                    AutomaticRecoveryEnabled = true,//如果connection挂掉是否重新连接
                    TopologyRecoveryEnabled = true//连接恢复后，连接的交换机，队列等是否一同恢复
                };
                //创建连接对象
                connection =  factory.CreateConnectionAsync(GlobalContext.RabbitMQConfig.HostName.Split(',')).Result; 
            }
            else
            {
                //创建连接对象工厂
                factory = new ConnectionFactory()
                {
                    UserName = GlobalContext.RabbitMQConfig.UserName,
                    Password = GlobalContext.RabbitMQConfig.Password,
                    HostName = GlobalContext.RabbitMQConfig.HostName,
                    Port = GlobalContext.RabbitMQConfig.Port
                };
                //创建连接对象
                connection = factory.CreateConnectionAsync().Result;
            }

            #endregion

            return connection;
        });

        /// <summary>
        /// 单例对象
        /// </summary>
        public static IConnection ConnectionInstance { get { return LazyConnection.Value; } }

        /// <summary>
        /// 是否已创建
        /// </summary>
        public static bool IsConnectionInstanceCreated { get { return LazyConnection.IsValueCreated; } }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息体</param>
        public static async Task SendMessageAsync(string message)
        {
            IConnection connection = ConnectionInstance;
            //定义通道
            var channel =await connection.CreateChannelAsync();
            //定义交换机
            await channel.ExchangeDeclareAsync(GlobalContext.RabbitMQConfig.ExchangeName, ExchangeType.Topic, true, false);
            //定义队列
            await channel.QueueDeclareAsync(GlobalContext.RabbitMQConfig.QueueName, false, false, false, null);
            //将队列绑定到交换机
            await channel.QueueBindAsync(GlobalContext.RabbitMQConfig.QueueName, GlobalContext.RabbitMQConfig.ExchangeName, "", null);
            //发布消息
            await channel.BasicPublishAsync(GlobalContext.RabbitMQConfig.ExchangeName, "", false, Encoding.Default.GetBytes(message));
        }

   
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息体</param>
        public static async Task PublishAsync<T>(T message)
        {
            string routeKey = "*";
            IConnection connection = ConnectionInstance;
            //定义通道
            var channel =await connection.CreateChannelAsync();

            //声明一个队列 (durable=true 持久化消息）
            await channel.QueueDeclareAsync(GlobalContext.RabbitMQConfig.QueueName, false, false, false, null);

            if (!string.IsNullOrEmpty(GlobalContext.RabbitMQConfig.ExchangeName))
            {
                await channel.ExchangeDeclareAsync(GlobalContext.RabbitMQConfig.ExchangeName, ExchangeType.Topic, true, false, null);
                //将队列绑定到交换机
                await channel.QueueBindAsync(GlobalContext.RabbitMQConfig.QueueName, GlobalContext.RabbitMQConfig.ExchangeName, routeKey, null);
            }

            var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            var properties = new BasicProperties
            {
                Persistent = true // 设置消息持久化
            };

            await channel.BasicPublishAsync(GlobalContext.RabbitMQConfig.ExchangeName, routeKey,false, properties, sendBytes);
        }

        /// <summary>
        /// 消费消息
        /// </summary> 
        public static async Task ConsumeAsync<T>(Action<T> action= null) where T : class
        {
            string routeKey = "*";

            IConnection connection = ConnectionInstance;
            //定义通道
            var channel =await connection.CreateChannelAsync();

            //声明一个队列 (durable=true 持久化消息）
            await channel.QueueDeclareAsync(GlobalContext.RabbitMQConfig.QueueName, false, false, false, null);

            if (!string.IsNullOrEmpty(GlobalContext.RabbitMQConfig.ExchangeName))
            {
                await channel.ExchangeDeclareAsync(GlobalContext.RabbitMQConfig.ExchangeName, ExchangeType.Topic, true, false, null);
                //将队列绑定到交换机
                await channel.QueueBindAsync(GlobalContext.RabbitMQConfig.QueueName, GlobalContext.RabbitMQConfig.ExchangeName, routeKey, null);
            }

            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var obj = JsonConvert.DeserializeObject<T>(message);

                try
                {
                    if (action != null)
                    {
                        action?.Invoke(obj!);
                    }
                }
                catch //(Exception ex)
                {
                    //throw ex;
                }
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            await channel.BasicConsumeAsync(queue: GlobalContext.RabbitMQConfig.QueueName, autoAck: false, consumer: consumer);
        }
    }
}
