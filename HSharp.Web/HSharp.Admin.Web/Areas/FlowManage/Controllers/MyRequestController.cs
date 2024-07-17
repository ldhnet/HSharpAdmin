using HSharp.Admin.Web.Controllers;
using HSharp.Web.Code;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HSharp.Admin.Web.Areas.FlowManage.Controllers
{
    [Area("FlowManage")]
    public class MyRequestController : BaseController
    {
        [AuthorizeFilter("flowmanage:myrequest:index")]
        public async Task<IActionResult> Index()
        {
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            ViewBag.userId = operatorInfo.UserId;
            return View();
        }
    }
}
