using HSharp.Business.OrganizationManage;
using HSharp.Model.Result.AuthThirdManage;
using HSharp.Service.AuthThirdManage; 
using HSharp.Util.Model; 
using System.Threading.Tasks;
using HSharp.Web.Code; 
using HSharp.Model.Param.AuthThirdManage;
using HSharp.Business.SystemManage;

namespace HSharp.Business.AuthThirdManage
{
    public class GitCodeAuthBLL
    {
        private UserBLL _userBLL = new UserBLL();
        private GitCodeAuthService _gitCodeAuthService = new GitCodeAuthService();
        private LogLoginBLL _logLoginBLL = new LogLoginBLL();
        /// <summary>
        /// Render申请code url
        /// </summary>
        /// <returns></returns>
        public async Task<AuthThirdRenderResult> Render()
        {
            return await _gitCodeAuthService.Render();
        }
        /// <summary>
        /// 集成三方登录回调地址
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<AuthThirdToken> Callback(string code)
        {
            return await _gitCodeAuthService.Callback(code);
        }
        /// <summary>
        /// 获取授权用户个人信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AuthThirdUser> GetThirdUserDetail(string token)
        {
            return await _gitCodeAuthService.GetThirdUserDetail(token);
        } 
        /// <summary>
        /// 登录逻辑处理
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<TData> LoginHandle(AuthThirdToken token)
        { 
            var thirdUser = await _gitCodeAuthService.GetThirdUserDetail(token.access_token);
            var returnUser = await _userBLL.SaveOrUpdateOAuthUser(thirdUser, token.access_token);
            await Operator.Instance.AddCurrent(token.access_token);
            await _logLoginBLL.SaveLoginLog(returnUser.Data?.Id, returnUser.Tag, returnUser.Message);
            return returnUser;
        }
    }
}
