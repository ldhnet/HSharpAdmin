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
    public class AreaService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<AreaEntity>> GetList(AreaListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<AreaEntity>> GetPageList(AreaListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<AreaEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<AreaEntity>(id);
        }

        public async Task<AreaEntity> GetEntityByAreaCode(string areaCode)
        {
            return await this.BaseRepository().FindEntity<AreaEntity>(p => p.AreaCode == areaCode);
        }

        #endregion 获取数据

        #region 提交数据

        public async Task SaveForm(AreaEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.BaseRepository().Insert<AreaEntity>(entity);
            }
            else
            {
                await entity.Modify();
                await this.BaseRepository().Update<AreaEntity>(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<AreaEntity>(idArr);
        }

        #endregion 提交数据

        #region 私有方法

        private Expression<Func<AreaEntity, bool>> ListFilter(AreaListParam param)
        {
            var expression = LinqExtensions.True<AreaEntity>();
            if (param != null)
            {
                if (!param.AreaName.IsEmpty())
                {
                    expression = expression.And(t => t.AreaName.Contains(param.AreaName));
                }
            }
            return expression;
        }

        #endregion 私有方法
    }
}