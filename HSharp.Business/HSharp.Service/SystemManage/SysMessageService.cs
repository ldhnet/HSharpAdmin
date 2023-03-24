﻿using System;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using HSharp.Util;
using HSharp.Util.Extension;
using HSharp.Util.Model;
using HSharp.Data;
using HSharp.Data.Repository;
using HSharp.Entity.SystemManage;
using HSharp.Model.Param.SystemManage;
using HSharp.Web.Code;
using Newtonsoft.Json.Linq;

namespace HSharp.Service.SystemManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-03-24 11:34
    /// 描 述：站内信服务类
    /// </summary>
    public class SysMessageService :  RepositoryFactory
    {
        #region 获取数据
        public async Task<List<SysMessageEntity>> GetList(SysMessageListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<SysMessageEntity>> GetPageList(SysMessageListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<SysMessageEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<SysMessageEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(SysMessageEntity entity)
        {
            if (entity.SendUserId.IsNullOrZero())
            {
                OperatorInfo user = await Operator.Instance.Current(entity.Token);
                if (user != null)
                {
                    entity.SendUserId = user.UserId;
                } 
            }

            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.BaseRepository().Insert(entity);
            }
            else
            {
                
                await this.BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<SysMessageEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<SysMessageEntity, bool>> ListFilter(SysMessageListParam param)
        {
            var expression = LinqExtensions.True<SysMessageEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion
    }
}
