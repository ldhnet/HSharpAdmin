using System;
using System.Text;
using NUnit.Framework;
using HSharp.Util;
using HSharp.Util.Global;
using HSharp.Util.Context;

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
        public void SendMessage_RabbitMQSimple()
        {
            HSharpRabbitMQContext.SendMessage("HSharp=RabbitMQ 测试消息发布！" + DateTime.Now.ToString());


        }
        [Test]
        public void Publish_RabbitMQSimple()
        {
            HSharpRabbitMQContext.Publish("HSharp=RabbitMQ-Publish 测试消息发布！" + DateTime.Now.ToString());


        }
        [Test]
        public void Consume_RabbitMQSimple()
        {
            Action<string> action = (value) => { Console.WriteLine("消费成功" + value); };

            HSharpRabbitMQContext.Consume(action);

        }
    }
}
