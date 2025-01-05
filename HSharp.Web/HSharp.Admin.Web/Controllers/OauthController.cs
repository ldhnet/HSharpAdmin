using HSharp.Business.AuthThirdManage;
using HSharp.Business.SystemManage;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HSharp.Admin.Web.Controllers
{
    public class OauthController : BaseController
    {
        private AuthThirdBLL _authThirdBLL = new AuthThirdBLL();
        /// <summary>
        /// 第三方登录
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Login()
        {
            var result = await _authThirdBLL.Render();
            return Redirect(result.authorizeUrl);
        }
        /// <summary>
        /// 回调地址
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<IActionResult> Callback(string code)
        {
            var result = await _authThirdBLL.Callback(code); 
            var user = await _authThirdBLL.GetThirdUserDetail(result.access_token);
            return Ok(user);
        }
    }
}
