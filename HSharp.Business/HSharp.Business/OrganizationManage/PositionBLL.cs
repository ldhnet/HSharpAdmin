﻿using HSharp.Entity.OrganizationManage;
using HSharp.Model.Param.OrganizationManage;
using HSharp.Service.OrganizationManage;
using HSharp.Util.Extension;
using HSharp.Util.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HSharp.Business.OrganizationManage
{
    public class PositionBLL
    {
        private PositionService positionService = new PositionService();

        #region 获取数据

        public async Task<TData<List<PositionEntity>>> GetList(PositionListParam param)
        {
            TData<List<PositionEntity>> obj = new TData<List<PositionEntity>>();
            obj.Data = await positionService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<PositionEntity>>> GetPageList(PositionListParam param, Pagination pagination)
        {
            TData<List<PositionEntity>> obj = new TData<List<PositionEntity>>();
            obj.Data = await positionService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<PositionEntity>> GetEntity(long id)
        {
            TData<PositionEntity> obj = new TData<PositionEntity>();
            obj.Data = await positionService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<int>> GetMaxSort()
        {
            TData<int> obj = new TData<int>();
            obj.Data = await positionService.GetMaxSort();
            obj.Tag = 1;
            return obj;
        }

        #endregion 获取数据

        #region 提交数据

        public async Task<TData<string>> SaveForm(PositionEntity entity)
        {
            TData<string> obj = new TData<string>();
            if (positionService.ExistPositionName(entity))
            {
                obj.Message = "职位名称已经存在！";
                return obj;
            }
            await positionService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await positionService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        #endregion 提交数据
    }
}