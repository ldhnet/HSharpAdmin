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
            TData<UserEntity> obj = new TData<UserEntity>();

            var thirdUser = await _authThirdService.GetThirdUserDetail(token.access_token);

            obj =  await _userBLL.IsExistGiteeEntity(thirdUser.id);

            if (obj.Tag == 0 && obj.Data == null)
            {
                var userEntity = new UserEntity();
                userEntity.RealName = thirdUser.name;
                userEntity.UserName = thirdUser.login;
                userEntity.Password = "123456";
                userEntity.Email = thirdUser.email;
                userEntity.DepartmentId = 16508640061124405;
                userEntity.Gender = 1;
                userEntity.Portrait = thirdUser.avatar_url;
                userEntity.UserStatus = 1;
                userEntity.LoginCount = 1;
                userEntity.IsSystem = 0;
                userEntity.IsOnline = 0;
                userEntity.FirstVisit = DateTime.Now;
                userEntity.PreviousVisit = DateTime.Now;
                userEntity.LastVisit = DateTime.Now;
                userEntity.GiteeId = thirdUser.id;
                userEntity.WebToken = token.access_token;
                userEntity.RoleIds = "16508640061130147,16508640061130146";
                var addResult = await _userBLL.SaveForm(userEntity);
                obj.Tag = addResult.Tag;
                userEntity.Id = addResult.Data?.ParseToLong();
                obj.Data = userEntity;
                await Operator.Instance.AddCurrent(token.access_token);
            } 
            else if (obj.Tag == 1)
            {
                obj.Data.LoginCount = obj.Data.LoginCount + 1;
                obj.Data.LastVisit = DateTime.Now;
                obj.Data.GiteeId = thirdUser.id;
                obj.Data.WebToken = token.access_token;
                await new UserBLL().UpdateUser(obj.Data);
                await Operator.Instance.AddCurrent(token.access_token);
            }
            string ip = NetHelper.Ip;
            string browser = NetHelper.Browser;
            string os = NetHelper.GetOSVersion();
            string userAgent = NetHelper.UserAgent;

            Action taskAction = async () =>
            {
                LogLoginEntity logLoginEntity = new LogLoginEntity
                {
                    LogStatus = obj.Tag == 1 ? OperateStatusEnum.Success.ParseToInt() : OperateStatusEnum.Fail.ParseToInt(),
                    Remark = obj.Message,
                    IpAddress = ip,
                    IpLocation = IpLocationHelper.GetIpLocation(ip),
                    Browser = browser,
                    OS = os,
                    ExtraRemark = userAgent,
                    BaseCreatorId = obj.Data?.Id
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
