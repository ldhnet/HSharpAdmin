using HSharp.Business.OrganizationManage;
using HSharp.Business.SystemManage;
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
        private LogLoginBLL _logLoginBLL = new LogLoginBLL();
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
        public async Task<TData> IsCheckStarred(string token)
        {
            TData<bool> obj = new TData<bool>();
            var result = await _giteeAuthService.IsCheckStarred(token);
            obj.Tag = result?1:2;
            obj.Data = result;
            obj.Message = result ? "已star" : "检测到未star本项目,请先点Star";
            return obj;
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
            Action taskAction = async () =>
            {
                await _logLoginBLL.SaveLoginLog(returnUser.Data?.Id, returnUser.Tag, returnUser.Message); 
            };
            AsyncTaskHelper.StartTask(taskAction);
            return returnUser;
        }
    }
}
