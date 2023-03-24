using HSharp.Data.Repository;
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
    public class AutoJobLogService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<AutoJobLogEntity>> GetList(AutoJobLogListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<AutoJobLogEntity>> GetPageList(AutoJobLogListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<AutoJobLogEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<AutoJobLogEntity>(id);
        }

        #endregion 获取数据

        #region 提交数据

        public async Task SaveForm(AutoJobLogEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.BaseRepository().Insert<AutoJobLogEntity>(entity);
            }
            else
            {
                await this.BaseRepository().Update<AutoJobLogEntity>(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<AutoJobLogEntity>(idArr);
        }

        #endregion 提交数据

        #region 私有方法

        private Expression<Func<AutoJobLogEntity, bool>> ListFilter(AutoJobLogListParam param)
        {
            var expression = LinqExtensions.True<AutoJobLogEntity>();
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