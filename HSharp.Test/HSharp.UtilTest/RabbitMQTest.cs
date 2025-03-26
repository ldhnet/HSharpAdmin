using System;
using System.Text;
using NUnit.Framework;
using HSharp.Util;
using HSharp.Util.Global;
using HSharp.Util.Context;
using System.Threading.Tasks;

namespace HSharp.UtilTest
{
    public class RabbitMQTest
    {
        [SetUp]
        public void Init()
        {
            GlobalContext.RabbitMQConfig = new RabbitMQConfig
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost",
                Port = 5672,
                QueueName= "HSharp.Queue",
                ExchangeName= "HSharp.Exchange.Topic"

            };
        }

        [Test]
        public async Task SendMessage_RabbitMQSimple()
        {
           await HSharpRabbitMQContext.SendMessageAsync("HSharp=RabbitMQ 测试消息发布！" + DateTime.Now.ToString());


        }
        [Test]
        public async Task Publish_RabbitMQSimple()
        {
            await HSharpRabbitMQContext.PublishAsync("HSharp=RabbitMQ-Publish 测试消息发布！" + DateTime.Now.ToString());
        }
        [Test]
        public async Task Consume_RabbitMQSimple()
        {
            Action<string> action = (value) => { Console.WriteLine("消费成功" + value); };

            await HSharpRabbitMQContext.ConsumeAsync(action);

        }
    }
}
