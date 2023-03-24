using HSharp.Entity.SystemManage;
using HSharp.Model.Param.SystemManage;
using HSharp.Service.SystemManage;
using HSharp.Util.Extension;
using HSharp.Util.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HSharp.Business.SystemManage
{
    public class AutoJobLogBLL
    {
        private AutoJobLogService autoJobLogService = new AutoJobLogService();

        #region 获取数据

        public async Task<TData<List<AutoJobLogEntity>>> GetList(AutoJobLogListParam param)
        {
            TData<List<AutoJobLogEntity>> obj = new TData<List<AutoJobLogEntity>>();
            obj.Data = await autoJobLogService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<AutoJobLogEntity>>> GetPageList(AutoJobLogListParam param, Pagination pagination)
        {
            TData<List<AutoJobLogEntity>> obj = new TData<List<AutoJobLogEntity>>();
            obj.Data = await autoJobLogService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<AutoJobLogEntity>> GetEntity(long id)
        {
            TData<AutoJobLogEntity> obj = new TData<AutoJobLogEntity>();
            obj.Data = await autoJobLogService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        #endregion 获取数据

        #region 提交数据

        public async Task<TData<string>> SaveForm(AutoJobLogEntity entity)
        {
            TData<string> obj = new TData<string>();
            await autoJobLogService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData<long> obj = new TData<long>();
            await autoJobLogService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        #endregion 提交数据
    }
}