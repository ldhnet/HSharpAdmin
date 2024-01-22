using System; 
using NUnit.Framework;
using HSharp.Util;
using HSharp.Util.Global;
using HSharp.Util.Context;

namespace HSharp.UtilTest
{
    public class LoggerTest
    {
        [SetUp]
        public void Init()
        {
            GlobalContext.LogConfig = new LogConfig
            {
                IsEnabled = true
            };
        }

        [Test]
        public void Nlog_Simple()
        {
            HSharpLoggerContext.Info("HSharp=Nlog_Simple---Info！" + DateTime.Now.ToString());
            HSharpLoggerContext.Warn("HSharp=Nlog_Simple---Warn！" + DateTime.Now.ToString());
            HSharpLoggerContext.Debug("HSharp=Nlog_Simple---Debug！" + DateTime.Now.ToString());
        } 
    }
}
