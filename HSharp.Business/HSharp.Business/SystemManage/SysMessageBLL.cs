using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using HSharp.Util;
using HSharp.Util.Extension;
using HSharp.Util.Model;
using HSharp.Entity.SystemManage;
using HSharp.Model.Param.SystemManage;
using HSharp.Service.SystemManage;
using NPOI.POIFS.FileSystem;

namespace HSharp.Business.SystemManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-03-24 11:34
    /// 描 述：站内信业务类
    /// </summary>
    public class SysMessageBLL
    {
        private SysMessageService sysMessageService = new SysMessageService();

        #region 获取数据
        public async Task<TData<List<SysMessageContentEntity>>> GetList(SysMessageListParam param)
        {
            TData<List<SysMessageContentEntity>> obj = new TData<List<SysMessageContentEntity>>();
            obj.Data = await sysMessageService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<SysMessageContentEntity>>> GetPageList(SysMessageListParam param, Pagination pagination)
        {
            TData<List<SysMessageContentEntity>> obj = new TData<List<SysMessageContentEntity>>();
            obj.Data = await sysMessageService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<SysMessageContentEntity>> GetEntity(long id)
        {
            TData<SysMessageContentEntity> obj = new TData<SysMessageContentEntity>();
            obj.Data = await sysMessageService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }
        public async Task<TData<int>> GetUnreadCount(long userId)
        {
            TData<int> obj = new TData<int>();
            int count = await sysMessageService.GetUnreadCount(userId);
            obj.Data = count;
            obj.Tag = count > 0 ? 1 : 0;
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(SysMessageContentEntity entity)
        {
            TData<string> obj = new TData<string>();
            await sysMessageService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> SendUnreadMessageToUser(long userId)
        {
            TData obj = new TData(); 
            var result = await sysMessageService.IsExistRead(userId);
            obj.Tag = 1;
            if (result.Item1)
            {
                await sysMessageService.AddUnreadMessage(userId, result.Item2);           
            }     
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await sysMessageService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 私有方法
        #endregion
    }
}
