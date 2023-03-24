using System;
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
using NPOI.POIFS.FileSystem;

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
        public async Task<List<SysMessageContentEntity>> GetList(SysMessageListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<SysMessageContentEntity>> GetPageList(SysMessageListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list= await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<SysMessageContentEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<SysMessageContentEntity>(id);
        }
        public async Task<int> GetUnreadCount(long userId)
        { 
            var list = await this.BaseRepository().FindList<SysMessageUserEntity>(c => c.ReceiveUserId == userId && c.IsRead == 0);
            return list.Count();
        }
        public async Task<Tuple<bool, List<long>>> IsExistRead(long userId)
        {  
            var listUser = await this.BaseRepository().FindList<SysMessageUserEntity>(c=>c.ReceiveUserId == userId);       
            var msgIds = listUser.ToList().Select(c => c.MessageId).ToList();   
            var listMsg = await this.BaseRepository().FindList<SysMessageContentEntity>(c => !msgIds.Contains(c.Id ?? 0));
            if (listMsg.Any())
            {
                return new(true, listMsg.Select(c => c.Id.ParseToLong()).ToList());
            }
            else
            {
                return new(false, new List<long>());
            }
        }

        #endregion

        #region 提交数据
        public async Task SaveForm(SysMessageContentEntity entity)
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
        public async Task AddUnreadMessage(long userId, List<long> entityList)
        {
            var entityUserList = new List<SysMessageUserEntity>();
            foreach (var msgId in entityList)
            {
                SysMessageUserEntity entity = new();
                entity.Create();
                entity.ReceiveUserId = userId;
                entity.MessageId = msgId;
                entity.BaseCreateTime = DateTime.Now;
                entity.IsRead = 0;
                entityUserList.Add(entity);
            }
            await this.BaseRepository().Insert(entityUserList);
        }
        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<SysMessageContentEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<SysMessageContentEntity, bool>> ListFilter(SysMessageListParam param)
        {
            var expression = LinqExtensions.True<SysMessageContentEntity>();
            if (param != null)
            {
            }
            return expression;
        }
        #endregion
    }
}
