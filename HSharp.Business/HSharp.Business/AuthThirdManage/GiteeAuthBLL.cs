using HSharp.Business.OrganizationManage;  
using HSharp.Entity.SystemManage;
using HSharp.Enum;
using HSharp.Model.Param.AuthThirdManage;
using HSharp.Model.Result.AuthThirdManage;
using HSharp.Service.AuthThirdManage; 
using HSharp.Service.SystemManage;
using HSharp.Util;
using HSharp.Util.Extension;
using HSharp.Util.Model;
using HSharp.Web.Code;
using System; 
using System.Threading.Tasks;

namespace HSharp.Business.AuthThirdManage
{
    public class GiteeAuthBLL
    {
        private UserBLL _userBLL = new UserBLL();
        private GiteeAuthService _giteeAuthService = new GiteeAuthService();
        private LogLoginService _logLoginService = new LogLoginService();
        /// <summary>
        /// Render申请code url
        /// </summary>
        /// <returns></returns>
        public async Task<AuthThirdRenderResult> Render()
        {
            return await _giteeAuthService.Render();
        }
        /// <summary>
        /// 集成三方登录回调地址
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<AuthThirdToken> Callback(string code)
        { 
            return await _giteeAuthService.Callback(code);
        }
        /// <summary>
        /// 获取授权用户个人信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AuthThirdUser> GetThirdUserDetail(string token)
        {
            return await _giteeAuthService.GetThirdUserDetail(token);
        }
        /// <summary>
        /// 检查授权用户是否已经star了仓库
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> IsCheckStarred(string token)
        {
            return await _giteeAuthService.IsCheckStarred(token);
        }
        /// <summary>
        /// 登录逻辑处理
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TData> LoginHandle(AuthThirdToken token)
        {
            //var aaa = await IsCheckStarred(token.access_token);  
            var thirdUser = await _giteeAuthService.GetThirdUserDetail(token.access_token);
            var returnUser = await _userBLL.SaveOrUpdateOAuthUser(thirdUser, token.access_token); 
            await Operator.Instance.AddCurrent(token.access_token);
            string ip = NetHelper.Ip;
            string browser = NetHelper.Browser;
            string os = NetHelper.GetOSVersion();
            string userAgent = NetHelper.UserAgent;

            Action taskAction = async () =>
            {
                LogLoginEntity logLoginEntity = new LogLoginEntity
                {
                    LogStatus = returnUser.Tag == 1 ? OperateStatusEnum.Success.ParseToInt() : OperateStatusEnum.Fail.ParseToInt(),
                    Remark = returnUser.Message,
                    IpAddress = ip,
                    IpLocation = IpLocationHelper.GetIpLocation(ip),
                    Browser = browser,
                    OS = os,
                    ExtraRemark = userAgent,
                    BaseCreatorId = returnUser.Data?.Id
                };

                // 让底层不用获取HttpContext
                logLoginEntity.BaseCreatorId = logLoginEntity.BaseCreatorId ?? 0;

                await _logLoginService.SaveForm(logLoginEntity);
            };
            AsyncTaskHelper.StartTask(taskAction);
            return returnUser;
        }
    }
}
