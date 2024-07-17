using HSharp.Admin.Web.Controllers;
using HSharp.Web.Code;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HSharp.Admin.Web.Areas.FlowManage.Controllers
{
    [Area("FlowManage")]
    public class FlowDemoController : BaseController
    {
        //[AuthorizeFilter("flowmanage:flowdemo:demo1")]
        public async Task<IActionResult> Demo1()
        {
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            ViewBag.userId = operatorInfo.UserId;
            return View();
        }
        //[AuthorizeFilter("flowmanage:flowdemo:demo2")]
        public async Task<IActionResult> Demo2()
        {
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            ViewBag.userId = operatorInfo.UserId;
            return View();
        }
    }
}
