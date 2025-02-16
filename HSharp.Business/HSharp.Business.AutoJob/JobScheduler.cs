﻿using Quartz;
using Quartz.Impl;

namespace HSharp.Business.AutoJob
{
    public class JobScheduler
    {
        private static object lockHelper = new object();

        private static IScheduler scheduler = null;

        public static IScheduler GetScheduler()
        {
            lock (lockHelper)
            {
                if (scheduler != null)
                {
                    return scheduler;
                }
                else
                {
                    ISchedulerFactory schedf = new StdSchedulerFactory();
                    IScheduler sched = schedf.GetScheduler().Result;
                    return sched;
                }
            }
        }
    }
}