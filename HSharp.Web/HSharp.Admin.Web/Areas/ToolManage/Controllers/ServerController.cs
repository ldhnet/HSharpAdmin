﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using HSharp.Admin.Web.Controllers;
using HSharp.Model.Result.SystemManage;
using HSharp.Util;
using HSharp.Util.Model;
using System.Threading.Tasks;

namespace HSharp.Admin.Web.Areas.ToolManage.Controllers
{
    [Area("ToolManage")]
    public class ServerController : BaseController
    {
        #region 视图功能
        [AuthorizeFilter("tool:server:view")]
        public IActionResult ServerIndex()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("tool:server:view")]
        public IActionResult GetServerJson()
        {
            TData<ComputerInfo> obj = new TData<ComputerInfo>();
            ComputerInfo computerInfo = null;
            try
            {
                computerInfo = ComputerHelper.GetComputerInfo();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                obj.Message = ex.Message;
            }
            obj.Data = computerInfo;
            obj.Tag = 1;
            return Json(obj);
        }

        public async Task<IActionResult> GetServerIpJson()
        {
            TData<string> obj = new TData<string>();
            string ip =await NetHelper.GetWanIp();
            string ipLocation = await IpLocationHelper.GetIpLocation(ip);
            obj.Data = string.Format("{0} ({1})", ip, ipLocation);
            obj.Tag = 1;
            return Json(obj);
        }
        #endregion    　
    }
}