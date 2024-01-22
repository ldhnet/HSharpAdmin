//-----------------------------------------------------------------------
// <Copyright>
// * Copyright (C) 2022 RuYiAdmin All Rights Reserved
// </Copyright>
//-----------------------------------------------------------------------
 
using Microsoft.Extensions.Logging;
using System; 

namespace HSharp.Util.Context
{
    public class HSharpLoggerContext
    {
        /// <summary>
        /// ILog实例
        /// </summary>
        private static ILogger logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        static HSharpLoggerContext()
        {
            //判断审计日志记录开关是否开启
            if (logger == null && GlobalContext.LogConfig.IsEnabled)
            {
                //var repository = LogManager.CreateRepository("NETCoreRepository");
                //var path = Directory.GetCurrentDirectory() + "/Log4net/log4net.config";
                ////读取配置信息
                //XmlConfigurator.Configure(repository, new FileInfo(path));
                //logger = LogManager.GetLogger(repository.Name, "InfoLogger");
            }
        }

        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="message">摘要</param>
        /// <param name="exception">异常</param>
        public static void Info(string message, Exception exception = null)
        {
            //判断审计日志记录开关是否开启
            if (logger != null && GlobalContext.LogConfig.IsEnabled)
            {
                Console.WriteLine("HSharp Runtime Info:" + message);
                if (exception == null)
                {
                    logger.LogInformation(message);
                }
                else
                {
                    logger.LogInformation(message, exception);
                }
            }
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="message">摘要</param>
        /// <param name="exception">异常</param>
        public static void Warn(string message, Exception exception = null)
        {
            //判断审计日志记录开关是否开启
            if (logger != null && GlobalContext.LogConfig.IsEnabled)
            {
                Console.WriteLine("HSharp Warnning Info:" + message);
                if (exception == null)
                {
                    logger.LogWarning(message);
                }
                else
                {
                    logger.LogWarning(message, exception);
                }
            }
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message">摘要</param>
        /// <param name="exception">异常</param>
        public static void Error(string message, Exception exception = null)
        {
            //判断审计日志记录开关是否开启
            if (logger != null && GlobalContext.LogConfig.IsEnabled)
            {
                Console.WriteLine("HSharp Error Info:" + message);
                if (exception == null)
                {
                    logger.LogError(message);
                }
                else
                {
                    logger.LogError(message, exception);
                }
            }
        }

        /// <summary>
        /// Debug日志
        /// </summary>
        /// <param name="message">摘要</param>
        /// <param name="exception">异常</param>
        public static void Debug(string message, Exception exception = null)
        {
            //判断审计日志记录开关是否开启
            if (logger != null && GlobalContext.LogConfig.IsEnabled)
            {
                Console.WriteLine("HSharp Debug Info:" + message);
                if (exception == null)
                {
                    logger.LogDebug(message);
                }
                else
                {
                    logger.LogDebug(message, exception);
                }
            }
        }
    }
}
