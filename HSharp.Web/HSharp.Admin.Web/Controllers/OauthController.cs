﻿using HSharp.Business.AuthThirdManage;
using HSharp.Business.SystemManage;
using HSharp.Model.Param.AuthThirdManage;
using HSharp.Model.Result.AuthThirdManage;
using HSharp.Util.Model;
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
        [HttpGet]
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
        [HttpGet]
        public async Task<IActionResult> Callback(string code)
        {
            var tokenResult = await _authThirdBLL.Callback(code);  
            var result =await _authThirdBLL.LoginHandle(tokenResult);
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
