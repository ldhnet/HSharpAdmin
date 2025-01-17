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
    public class DataDictDetailService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<DataDictDetailEntity>> GetList(DataDictDetailListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.OrderBy(p => p.DictSort).ToList();
        }

        public async Task<List<DataDictDetailEntity>> GetPageList(DataDictDetailListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<DataDictDetailEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<DataDictDetailEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            object result = await this.BaseRepository().FindObject("SELECT MAX(DictSort) FROM SysDataDictDetail");
            int sort = result.ParseToInt();
            sort++;
            return sort;
        }

        public bool ExistDictKeyValue(DataDictDetailEntity entity)
        {
            var expression = LinqExtensions.True<DataDictDetailEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.DictType == entity.DictType && (t.DictKey == entity.DictKey || t.DictValue == entity.DictValue));
            }
            else
            {
                expression = expression.And(t => t.DictType == entity.DictType && (t.DictKey == entity.DictKey || t.DictValue == entity.DictValue) && t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }

        #endregion 获取数据

        #region 提交数据

        public async Task SaveForm(DataDictDetailEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.BaseRepository().Insert<DataDictDetailEntity>(entity);
            }
            else
            {
                await entity.Modify();
                await this.BaseRepository().Update<DataDictDetailEntity>(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<DataDictDetailEntity>(idArr);
        }

        #endregion 提交数据

        #region 私有方法

        private Expression<Func<DataDictDetailEntity, bool>> ListFilter(DataDictDetailListParam param)
        {
            var expression = LinqExtensions.True<DataDictDetailEntity>();
            if (param != null)
            {
                if (param.DictKey.ParseToInt() > 0)
                {
                    expression = expression.And(t => t.DictKey == param.DictKey);
                }

                if (!string.IsNullOrEmpty(param.DictValue))
                {
                    expression = expression.And(t => t.DictValue.Contains(param.DictValue));
                }

                if (!string.IsNullOrEmpty(param.DictType))
                {
                    expression = expression.And(t => t.DictType.Contains(param.DictType));
                }
            }
            return expression;
        }

        #endregion 私有方法
    }
}