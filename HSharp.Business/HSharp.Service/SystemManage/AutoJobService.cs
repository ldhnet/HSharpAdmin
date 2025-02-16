﻿using HSharp.Data.Repository;
using HSharp.Entity.SystemManage;
using HSharp.Model.Param.SystemManage;
using HSharp.Util;
using HSharp.Util.Extension;
using HSharp.Util.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HSharp.Service.SystemManage
{
    public class AutoJobService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<AutoJobEntity>> GetList(AutoJobListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<AutoJobEntity>> GetPageList(AutoJobListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<AutoJobEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<AutoJobEntity>(id);
        }

        public bool ExistJob(AutoJobEntity entity)
        {
            var expression = LinqExtensions.True<AutoJobEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.JobGroupName == entity.JobGroupName && t.JobName == entity.JobName);
            }
            else
            {
                expression = expression.And(t => t.JobGroupName == entity.JobGroupName && t.JobName == entity.JobName && t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }

        #endregion 获取数据

        #region 提交数据

        public async Task SaveForm(AutoJobEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.BaseRepository().Insert<AutoJobEntity>(entity);
            }
            else
            {
                await entity.Modify();
                await this.BaseRepository().Update<AutoJobEntity>(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<AutoJobEntity>(idArr);
        }

        #endregion 提交数据

        #region 私有方法

        private Expression<Func<AutoJobEntity, bool>> ListFilter(AutoJobListParam param)
        {
            var expression = LinqExtensions.True<AutoJobEntity>();
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.JobName))
                {
                    expression = expression.And(t => t.JobName.Contains(param.JobName));
                }
            }
            return expression;
        }

        #endregion 私有方法
    }
}