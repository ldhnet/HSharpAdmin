﻿using HSharp.Entity.OrganizationManage;
using HSharp.Model.Param.OrganizationManage;
using HSharp.Model.Result;
using HSharp.Service.OrganizationManage;
using HSharp.Util;
using HSharp.Util.Extension;
using HSharp.Util.Model;
using HSharp.Web.Code;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HSharp.Business.OrganizationManage
{
    public class DepartmentBLL
    {
        private DepartmentService departmentService = new DepartmentService();
        private UserService userService = new UserService();

        #region 获取数据

        public async Task<TData<List<DepartmentEntity>>> GetList(DepartmentListParam param)
        {
            TData<List<DepartmentEntity>> obj = new TData<List<DepartmentEntity>>();
            obj.Data = await departmentService.GetList(param);
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            if (operatorInfo.IsSystem != 1)
            {
                List<long> childrenDepartmentIdList = await GetChildrenDepartmentIdList(obj.Data, operatorInfo.DepartmentId.Value);
                obj.Data = obj.Data.Where(p => childrenDepartmentIdList.Contains(p.Id.Value)).ToList();
            }
            List<UserEntity> userList = await userService.GetList(new UserListParam { UserIds = string.Join(",", obj.Data.Select(p => p.PrincipalId).ToArray()) });
            foreach (DepartmentEntity entity in obj.Data)
            {
                if (entity.PrincipalId > 0)
                {
                    entity.PrincipalName = userList.Where(p => p.Id == entity.PrincipalId).Select(p => p.RealName).FirstOrDefault();
                }
                else
                {
                    entity.PrincipalName = string.Empty;
                }
            }
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<ZtreeInfo>>> GetZtreeDepartmentList(DepartmentListParam param)
        {
            var obj = new TData<List<ZtreeInfo>>();
            obj.Data = new List<ZtreeInfo>();
            List<DepartmentEntity> departmentList = await departmentService.GetList(param);
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            if (operatorInfo.IsSystem != 1)
            {
                List<long> childrenDepartmentIdList = await GetChildrenDepartmentIdList(departmentList, operatorInfo.DepartmentId.Value);
                departmentList = departmentList.Where(p => childrenDepartmentIdList.Contains(p.Id.Value)).ToList();
            }
            foreach (DepartmentEntity department in departmentList)
            {
                obj.Data.Add(new ZtreeInfo
                {
                    id = department.Id,
                    pId = department.ParentId,
                    name = department.DepartmentName
                });
            }
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<ZtreeInfo>>> GetZtreeUserList(DepartmentListParam param)
        {
            var obj = new TData<List<ZtreeInfo>>();
            obj.Data = new List<ZtreeInfo>();
            List<DepartmentEntity> departmentList = await departmentService.GetList(param);
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            if (operatorInfo.IsSystem != 1)
            {
                List<long> childrenDepartmentIdList = await GetChildrenDepartmentIdList(departmentList, operatorInfo.DepartmentId.Value);
                departmentList = departmentList.Where(p => childrenDepartmentIdList.Contains(p.Id.Value)).ToList();
            }
            List<UserEntity> userList = await userService.GetList(null);
            foreach (DepartmentEntity department in departmentList)
            {
                obj.Data.Add(new ZtreeInfo
                {
                    id = department.Id,
                    pId = department.ParentId,
                    name = department.DepartmentName
                });
                List<long> userIdList = userList.Where(t => t.DepartmentId == department.Id).Select(t => t.Id.Value).ToList();
                foreach (UserEntity user in userList.Where(t => userIdList.Contains(t.Id.Value)))
                {
                    obj.Data.Add(new ZtreeInfo
                    {
                        id = user.Id,
                        pId = department.Id,
                        name = user.RealName
                    });
                }
            }
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<DepartmentEntity>> GetEntity(long id)
        {
            TData<DepartmentEntity> obj = new TData<DepartmentEntity>();
            obj.Data = await departmentService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<int>> GetMaxSort()
        {
            TData<int> obj = new TData<int>();
            obj.Data = await departmentService.GetMaxSort();
            obj.Tag = 1;
            return obj;
        }

        #endregion 获取数据

        #region 提交数据

        public async Task<TData<string>> SaveForm(DepartmentEntity entity)
        {
            TData<string> obj = new TData<string>();
            if (!entity.Id.IsNullOrZero() && entity.Id == entity.ParentId)
            {
                obj.Message = "不能选择自己作为上级部门！";
                return obj;
            }
            if (departmentService.ExistDepartmentName(entity))
            {
                obj.Message = "部门名称已经存在！";
                return obj;
            }
            await departmentService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            foreach (long id in TextHelper.SplitToArray<long>(ids, ','))
            {
                if (departmentService.ExistChildrenDepartment(id))
                {
                    obj.Message = "该部门下面有子部门！";
                    return obj;
                }
            }
            await departmentService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        #endregion 提交数据

        #region 公共方法

        /// <summary>
        /// 获取当前部门及下面所有的部门
        /// </summary>
        /// <param name="departmentList"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public async Task<List<long>> GetChildrenDepartmentIdList(List<DepartmentEntity> departmentList, long departmentId)
        {
            if (departmentList == null)
            {
                departmentList = await departmentService.GetList(null);
            }
            List<long> departmentIdList = new List<long>();
            departmentIdList.Add(departmentId);
            GetChildrenDepartmentIdList(departmentList, departmentId, departmentIdList);
            return departmentIdList;
        }

        #endregion 公共方法



        #region 私有方法

        /// <summary>
        /// 获取该部门下面所有的子部门
        /// </summary>
        /// <param name="departmentList"></param>
        /// <param name="departmentId"></param>
        /// <param name="departmentIdList"></param>
        private void GetChildrenDepartmentIdList(List<DepartmentEntity> departmentList, long departmentId, List<long> departmentIdList)
        {
            var children = departmentList.Where(p => p.ParentId == departmentId).Select(p => p.Id.Value).ToList();
            if (children.Count > 0)
            {
                departmentIdList.AddRange(children);
                foreach (long id in children)
                {
                    GetChildrenDepartmentIdList(departmentList, id, departmentIdList);
                }
            }
        }

        #endregion 私有方法
    }
}