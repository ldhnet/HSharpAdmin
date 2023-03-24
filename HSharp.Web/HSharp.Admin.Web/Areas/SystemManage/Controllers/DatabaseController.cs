using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using HSharp.Admin.Web.Controllers;
using HSharp.Business.SystemManage;
using HSharp.Entity;
using HSharp.Entity.SystemManage;
using HSharp.Model;
using HSharp.Model.Param.SystemManage;
using HSharp.Model.Result.SystemManage;
using HSharp.Util;
using HSharp.Util.Model;

namespace HSharp.Admin.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class DatabaseController : BaseController
    {
        private DatabaseTableBLL databaseTableBLL = new DatabaseTableBLL();

        #region 视图功能
        [AuthorizeFilter("system:datatable:view")]
        public IActionResult DatatableIndex()
        {
            return View();
        }
        public IActionResult AreaForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("system:datatable:search")]
        public async Task<IActionResult> GetTableListJson(string tableName)
        {
            TData<List<TableInfo>> obj = await databaseTableBLL.GetTableList(tableName);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:datatable:search")]
        public async Task<IActionResult> GetTablePageListJson(string tableName, Pagination pagination)
        {
            TData<List<TableInfo>> obj = await databaseTableBLL.GetTablePageList(tableName, pagination);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:datatable:view")]
        public async Task<IActionResult> GetTableFieldListJson(string tableName)
        {
            TData<List<TableFieldInfo>> obj = await databaseTableBLL.GetTableFieldList(tableName);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        public async Task<IActionResult> SyncDatabaseJson()
        {
            TData obj = await databaseTableBLL.SyncDatabase();
            return Json(obj);
        }
        #endregion
    }
}