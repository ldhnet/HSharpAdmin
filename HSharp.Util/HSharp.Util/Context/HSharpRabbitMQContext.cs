using HSharp.Util;
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
        private static readonly Lazy<IConnection> LazyConnection = new Lazy<IConnection>(() =>
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
                connection = factory.CreateConnection(GlobalContext.RabbitMQConfig.HostName.Split(','));
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
                connection = factory.CreateConnection();
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
        public static void SendMessage(string message)
        {
            IConnection connection = ConnectionInstance;
            //定义通道
            var channel = connection.CreateModel();
            //定义交换机
            channel.ExchangeDeclare(GlobalContext.RabbitMQConfig.ExchangeName, ExchangeType.Topic, true, false);
            //定义队列
            channel.QueueDeclare(GlobalContext.RabbitMQConfig.QueueName, false, false, false, null);
            //将队列绑定到交换机
            channel.QueueBind(GlobalContext.RabbitMQConfig.QueueName, GlobalContext.RabbitMQConfig.ExchangeName, "", null);
            //发布消息
            channel.BasicPublish(GlobalContext.RabbitMQConfig.ExchangeName, "", null, Encoding.Default.GetBytes(message));
        }

        /// <summary>
        /// 异步发送消息
        /// </summary>
        /// <param name="message">消息体</param>
        /// <returns></returns>
        public static async Task SendMessageAsynce(string message)
        {
            await Task.Run(() => SendMessage(message)).ConfigureAwait(false);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息体</param>
        public static void Publish<T>(T message)
        {
            string routeKey = "*";
            IConnection connection = ConnectionInstance;
            //定义通道
            var channel = connection.CreateModel();

            //声明一个队列 (durable=true 持久化消息）
            channel.QueueDeclare(GlobalContext.RabbitMQConfig.QueueName, false, false, false, null);

            if (!string.IsNullOrEmpty(GlobalContext.RabbitMQConfig.ExchangeName))
            {
                channel.ExchangeDeclare(GlobalContext.RabbitMQConfig.ExchangeName, ExchangeType.Topic, true, false, null);
                //将队列绑定到交换机
                channel.QueueBind(GlobalContext.RabbitMQConfig.QueueName, GlobalContext.RabbitMQConfig.ExchangeName, routeKey, null);
            }

            var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(GlobalContext.RabbitMQConfig.ExchangeName, routeKey, properties, sendBytes);
        }

        /// <summary>
        /// 消费消息
        /// </summary> 
        public static void Consume<T>(Action<T> action= null) where T : class
        {
            string routeKey = "*";

            IConnection connection = ConnectionInstance;
            //定义通道
            var channel = connection.CreateModel();

            //声明一个队列 (durable=true 持久化消息）
            channel.QueueDeclare(GlobalContext.RabbitMQConfig.QueueName, false, false, false, null);

            if (!string.IsNullOrEmpty(GlobalContext.RabbitMQConfig.ExchangeName))
            {
                channel.ExchangeDeclare(GlobalContext.RabbitMQConfig.ExchangeName, ExchangeType.Topic, true, false, null);
                //将队列绑定到交换机
                channel.QueueBind(GlobalContext.RabbitMQConfig.QueueName, GlobalContext.RabbitMQConfig.ExchangeName, routeKey, null);
            }

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
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
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: GlobalContext.RabbitMQConfig.QueueName, autoAck: false, consumer: consumer);
        }
    }
}
