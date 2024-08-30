﻿using HSharp.Admin.Web.Controllers;
using HSharp.Web.Code;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HSharp.Admin.Web.Areas.FlowManage.Controllers
{
    [Area("FlowManage")]
    public class PendingApprovalController : BaseController
    {
        [AuthorizeFilter("flowmanage:pendingapproval:index")]
        public async Task<IActionResult> Index()
        {
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            ViewBag.userId = operatorInfo.UserId;
            return View();
        }
    }
}