﻿using HSharp.Business.SystemManage;
using HSharp.Entity.SystemManage;
using HSharp.Util;
using HSharp.Util.Model;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HSharp.Business.AutoJob
{
    public class JobCenter
    {
        public void Start()
        {
            Task.Run(async () =>
            {
                TData<List<AutoJobEntity>> obj = await new AutoJobBLL().GetList(null);
                if (obj.Tag == 1)
                {
                    AddScheduleJob(obj.Data);
                }
            });
        }

        #region 添加任务计划

        private void AddScheduleJob(List<AutoJobEntity> entityList)
        {
            try
            {
                foreach (AutoJobEntity entity in entityList)
                {
                    if (entity.StartTime == null)
                    {
                        entity.StartTime = DateTime.Now;
                    }
                    DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(entity.StartTime, 1);
                    if (entity.EndTime == null)
                    {
                        entity.EndTime = DateTime.MaxValue.AddDays(-1);
                    }
                    DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(entity.EndTime, 1);

                    var scheduler = JobScheduler.GetScheduler();
                    IJobDetail job = JobBuilder.Create<JobExecute>().WithIdentity(entity.JobName, entity.JobGroupName).Build();
                    job.JobDataMap.Add("Id", entity.Id);

                    ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                                                 .StartAt(starRunTime)
                                                 .EndAt(endRunTime)
                                                 .WithIdentity(entity.JobName, entity.JobGroupName)
                                                 .WithCronSchedule(entity.CronExpression)
                                                 .Build();

                    scheduler.ScheduleJob(job, trigger);
                    scheduler.Start();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        #endregion 添加任务计划

        #region 清除任务计划

        public void ClearScheduleJob()
        {
            try
            {
                JobScheduler.GetScheduler().Clear();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        #endregion 清除任务计划
    }
}