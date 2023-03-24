using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using HSharp.Util;
using HSharp.Util.Model;
using HSharp.Entity;
using HSharp.Model;
using HSharp.Admin.Web.Controllers;
using HSharp.Entity.SystemManage;
using HSharp.Business.SystemManage;
using HSharp.Model.Param.SystemManage;

namespace HSharp.Admin.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-03-24 11:34
    /// 描 述：站内信控制器类
    /// </summary>
    [Area("SystemManage")]
    public class SysMessageController :  BaseController
    {
        private SysMessageBLL sysMessageBLL = new SysMessageBLL();

        #region 视图功能
        [AuthorizeFilter("system:sysmessage:view")]
        public ActionResult SysMessageIndex()
        {
            return View();
        }

        public ActionResult SysMessageForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("system:sysmessage:search")]
        public async Task<ActionResult> GetListJson(SysMessageListParam param)
        {
            TData<List<SysMessageContentEntity>> obj = await sysMessageBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:sysmessage:search")]
        public async Task<ActionResult> GetPageListJson(SysMessageListParam param, Pagination pagination)
        {
            TData<List<SysMessageContentEntity>> obj = await sysMessageBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<ActionResult> GetFormJson(long id)
        {
            TData<SysMessageContentEntity> obj = await sysMessageBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("system:sysmessage:add,system:sysmessage:edit")]
        public async Task<ActionResult> SaveFormJson(SysMessageContentEntity entity)
        {
            TData<string> obj = await sysMessageBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("system:sysmessage:delete")]
        public async Task<ActionResult> DeleteFormJson(string ids)
        {
            TData obj = await sysMessageBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
