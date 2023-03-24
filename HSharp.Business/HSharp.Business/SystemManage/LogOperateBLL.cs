using HSharp.Entity.OrganizationManage;
using HSharp.Entity.SystemManage;
using HSharp.Enum;
using HSharp.Model.Param.SystemManage;
using HSharp.Service.OrganizationManage;
using HSharp.Service.SystemManage;
using HSharp.Util.Extension;
using HSharp.Util.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HSharp.Business.SystemManage
{
    public class LogOperateBLL
    {
        private LogOperateService logOperateService = new LogOperateService();

        #region 获取数据

        public async Task<TData<List<LogOperateEntity>>> GetList(LogOperateListParam param)
        {
            TData<List<LogOperateEntity>> obj = new TData<List<LogOperateEntity>>();
            obj.Data = await logOperateService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<LogOperateEntity>>> GetPageList(LogOperateListParam param, Pagination pagination)
        {
            TData<List<LogOperateEntity>> obj = new TData<List<LogOperateEntity>>();
            obj.Data = await logOperateService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<LogOperateEntity>> GetEntity(long id)
        {
            TData<LogOperateEntity> obj = new TData<LogOperateEntity>();
            obj.Data = await logOperateService.GetEntity(id);
            if (obj.Data != null)
            {
                UserEntity userEntity = await new UserService().GetEntity(obj.Data.BaseCreatorId.Value);
                if (userEntity != null)
                {
                    obj.Data.UserName = userEntity.UserName;
                    DepartmentEntity departmentEntitty = await new DepartmentService().GetEntity(userEntity.DepartmentId.Value);
                    if (departmentEntitty != null)
                    {
                        obj.Data.DepartmentName = departmentEntitty.DepartmentName;
                    }
                }
            }
            obj.Tag = 1;
            return obj;
        }

        #endregion 获取数据

        #region 提交数据

        public async Task<TData<string>> SaveForm(LogOperateEntity entity)
        {
            TData<string> obj = new TData<string>();
            await logOperateService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<string>> SaveForm(string remark)
        {
            TData<string> obj = new TData<string>();
            LogOperateEntity entity = new LogOperateEntity();
            await logOperateService.SaveForm(entity);
            entity.LogStatus = OperateStatusEnum.Success.ParseToInt();
            entity.ExecuteUrl = remark;
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await logOperateService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> RemoveAllForm()
        {
            TData obj = new TData();
            await logOperateService.RemoveAllForm();
            obj.Tag = 1;
            return obj;
        }

        #endregion 提交数据
    }
}