using Azure;
using HSharp.Business.OrganizationManage;
using HSharp.Business.SystemManage;
using HSharp.Entity.OrganizationManage;
using HSharp.Entity.SystemManage;
using HSharp.Enum;
using HSharp.Model.Param.AuthThirdManage;
using HSharp.Model.Result.AuthThirdManage;
using HSharp.Service.AuthThirdManage;
using HSharp.Service.OrganizationManage;
using HSharp.Service.SystemManage;
using HSharp.Util;
using HSharp.Util.Extension;
using HSharp.Util.Model;
using HSharp.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSharp.Business.AuthThirdManage
{
    public class AuthThirdBLL
    {
        private UserBLL _userBLL = new UserBLL();
        private AuthThirdService _authThirdService = new AuthThirdService();
        private LogLoginService _logLoginService = new LogLoginService();

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

        public async Task<TData> LoginHandle(AuthThirdToken token)
        {
            TData obj = new TData();

            var thirdUser = await _authThirdService.GetThirdUserDetail(token.access_token);

            var userObj =  await _userBLL.GetEntity(thirdUser.id);

            if (userObj.Tag == 0 && userObj.Data == null) 
            {
                var userEntity = new UserEntity();
                userEntity.Id = thirdUser.id;
                userEntity.RealName = thirdUser.name;
                userEntity.UserName = thirdUser.login;
                userEntity.Email = thirdUser.email;
                userEntity.Password = "3f0ea22060c164bf4bdc3a67d0e12cdf";
                userEntity.Salt = "28585";
                userEntity.DepartmentId = 16508640061124405;
                userEntity.Gender = 1;
                userEntity.Portrait = thirdUser.avatar_url;
                userEntity.UserStatus = 1;
                userEntity.WebToken = token.access_token;

                var addResult = await _userBLL.SaveForm(userEntity);
                if (addResult.Tag == 1)
                {
                    await Operator.Instance.AddCurrent(token.access_token);
                }
            }  
            string ip = NetHelper.Ip;
            string browser = NetHelper.Browser;
            string os = NetHelper.GetOSVersion();
            string userAgent = NetHelper.UserAgent;

            Action taskAction = async () =>
            {
                LogLoginEntity logLoginEntity = new LogLoginEntity
                {
                    LogStatus = userObj.Tag == 1 ? OperateStatusEnum.Success.ParseToInt() : OperateStatusEnum.Fail.ParseToInt(),
                    Remark = userObj.Message,
                    IpAddress = ip,
                    IpLocation = IpLocationHelper.GetIpLocation(ip),
                    Browser = browser,
                    OS = os,
                    ExtraRemark = userAgent,
                    BaseCreatorId = userObj.Data?.Id
                };

                // 让底层不用获取HttpContext
                logLoginEntity.BaseCreatorId = logLoginEntity.BaseCreatorId ?? 0;

                await _logLoginService.SaveForm(logLoginEntity);
            };
            AsyncTaskHelper.StartTask(taskAction);

            return obj;
        }
    }
}
