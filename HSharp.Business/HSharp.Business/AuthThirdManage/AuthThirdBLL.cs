using Azure;
using HSharp.Model.Param.AuthThirdManage;
using HSharp.Model.Result.AuthThirdManage;
using HSharp.Service.AuthThirdManage;
using HSharp.Service.OrganizationManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSharp.Business.AuthThirdManage
{
    public class AuthThirdBLL
    {

        private AuthThirdService _authThirdService = new AuthThirdService();

        public async Task<AuthThirdRenderResult> Render()
        {
            return await _authThirdService.Render();
        }

        public async Task<AuthThirdToken> Callback(string code)
        { 
            return await _authThirdService.Callback(code);
        }

        public async Task<AuthThirdUser> GetThirdUserDetail(string token)
        {
            return await _authThirdService.GetThirdUserDetail(token);
        } 
    }
}
