using HSharp.Business.AuthThirdManage; 
using HSharp.Model.Result.AuthThirdManage; 
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HSharp.Admin.Web.Controllers
{
    public class OauthController : BaseController
    {
        private GiteeAuthBLL _giteeAuthBLL = new GiteeAuthBLL();
        private GitCodeAuthBLL _gitCodeAuthBLL = new GitCodeAuthBLL();
        /// <summary>
        /// Gitee第三方登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var result = await _giteeAuthBLL.Render();
            return Redirect(result.authorizeUrl);
        }
        /// <summary>
        /// Gitee回调地址
        /// </summary> 
        /// <returns></returns> 
        [HttpGet]
        public async Task<IActionResult> Callback([FromQuery]ThirdAuthorizeResult param)
        {
            if (!string.IsNullOrWhiteSpace(param.error) || string.IsNullOrWhiteSpace(param.code)) {
                return RedirectToAction("Error", "Home", new { message ="用户授权失败！"+ param.error + param.error_description });
            }
            var tokenResult = await _giteeAuthBLL.Callback(param.code);  
            var result =await _giteeAuthBLL.LoginHandle(tokenResult);
            if (result.Tag == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        /// <summary>
        /// GitCode第三方登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GitCodeLogin()
        {
            var result = await _gitCodeAuthBLL.Render();
            return Redirect(result.authorizeUrl);
        }
        /// <summary>
        /// GitCode回调地址
        /// </summary> 
        /// <returns></returns> 
        [HttpGet]
        public async Task<IActionResult> GitCodeCallback([FromQuery] ThirdAuthorizeResult param)
        {
            if (!string.IsNullOrWhiteSpace(param.error) || string.IsNullOrWhiteSpace(param.code))
            {
                return RedirectToAction("Error", "Home", new { message = "用户授权失败！" + param.error + param.error_description });
            }
            var tokenResult = await _gitCodeAuthBLL.Callback(param.code);
            var result = await _gitCodeAuthBLL.LoginHandle(tokenResult);
            if (result.Tag == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
    }
}
