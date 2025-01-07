using HSharp.Entity.SystemManage;
using HSharp.Enum;
using HSharp.Model.Param.SystemManage;
using HSharp.Service.SystemManage;
using HSharp.Util;
using HSharp.Util.Extension;
using HSharp.Util.Model; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HSharp.Business.SystemManage
{
    public class LogLoginBLL
    {
        private LogLoginService logLoginService = new LogLoginService();

        #region 获取数据

        public async Task<TData<List<LogLoginEntity>>> GetList(LogLoginListParam param)
        {
            TData<List<LogLoginEntity>> obj = new TData<List<LogLoginEntity>>();
            obj.Data = await logLoginService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<LogLoginEntity>>> GetPageList(LogLoginListParam param, Pagination pagination)
        {
            TData<List<LogLoginEntity>> obj = new TData<List<LogLoginEntity>>();
            obj.Data = await logLoginService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<LogLoginEntity>> GetEntity(long id)
        {
            TData<LogLoginEntity> obj = new TData<LogLoginEntity>();
            obj.Data = await logLoginService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        #endregion 获取数据

        #region 提交数据
        public async Task SaveLoginLog(long? userId, int tag, string message)
        {
            string ip = NetHelper.Ip;
            string browser = NetHelper.Browser;
            string os = NetHelper.GetOSVersion();
            string userAgent = NetHelper.UserAgent; 
            LogLoginEntity logLoginEntity = new LogLoginEntity
            {
                LogStatus = tag == 1 ? OperateStatusEnum.Success.ParseToInt() : OperateStatusEnum.Fail.ParseToInt(),
                Remark = message,
                IpAddress = ip,
                IpLocation = await IpLocationHelper.GetIpLocation(ip),
                Browser = browser,
                OS = os,
                ExtraRemark = userAgent,
                BaseCreatorId = userId??0
            }; 
            // 让底层不用获取HttpContext
            logLoginEntity.BaseCreatorId = logLoginEntity.BaseCreatorId ?? 0; 
            await this.SaveForm(logLoginEntity);
        }


        public async Task<TData<string>> SaveForm(LogLoginEntity entity)
        {
            TData<string> obj = new TData<string>();
            await logLoginService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await logLoginService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> RemoveAllForm()
        {
            TData obj = new TData();
            await logLoginService.RemoveAllForm();
            obj.Tag = 1;
            return obj;
        }

        #endregion 提交数据
    }
}