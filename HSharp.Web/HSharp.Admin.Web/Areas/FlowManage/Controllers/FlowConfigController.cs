using HSharp.Admin.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HSharp.Admin.Web.Areas.FlowManage.Controllers
{
    [Area("FlowManage")]
    public class FlowConfigController : BaseController
    {
        [AuthorizeFilter("flowmanage:flowconfig:index")]
        public IActionResult Index()
        {
            return View();
        }
        [AuthorizeFilter("flowmanage:flowconfig:addindex")]
        public IActionResult AddIndex()
        {
            return View();
        }
    }
}
