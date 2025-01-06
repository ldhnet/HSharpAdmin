using HSharp.Model.Param.AuthThirdManage;
using HSharp.Model.Result.AuthThirdManage;
using HSharp.Util; 
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HSharp.Service.AuthThirdManage
{
    public class GitCodeAuthService
    {
        public const string GITCODE_URI = "https://gitcode.com";
        public const string GITCODE_RESPONSE_TYPE = "code";
        public const string GITCODE_STATE = "hsharpadmin-gitcode";
        public const string GITCODE_CLIENT_ID = "838c5aa3c35547a594a29a8ea90f907e";
        public const string GITCODE_CLIENT_SECRET_KEY = "972651438018400a8b6d6622fe1aa00a";
        //public const string GITCODE_REDIRECT_URL_KEY = "http://117.72.70.166:9000/Oauth/GitCodeCallback";
        public const string GITCODE_REDIRECT_URL_KEY = "http://117.72.70.166:9000/Oauth/GitCodeCallback";

        /// <summary>
        /// 第三方登录页面渲染
        /// </summary>
        /// <param name="authThirdRenderParam"></param>
        /// <returns></returns>
        public async Task<AuthThirdRenderResult> Render()
        {
            AuthThirdRenderResult authThirdRenderResult = new AuthThirdRenderResult();
            string url = GITCODE_URI + "/oauth/authorize?" + 
                "client_id=" + GITCODE_CLIENT_ID + 
                "&redirect_uri=" + GITCODE_REDIRECT_URL_KEY +
                "&response_type=" + GITCODE_RESPONSE_TYPE +
                "&scope=user" +
                "&state=" + GITCODE_STATE;
            authThirdRenderResult.authorizeUrl = url;

            return await Task.FromResult(authThirdRenderResult);
        }


        /// <summary>
        /// 第三方登录授权回调，返回登录token
        /// </summary>
        /// <param name="authThirdCallbackParam"></param>
        /// <param name="authCallback"></param>
        /// <returns></returns>
        public async Task<AuthThirdToken> Callback(string code)
        { 
            string getTokenUri = GITCODE_URI + "/oauth/token?" +
            "client_id=" + GITCODE_CLIENT_ID +
            "&grant_type=authorization_code" +
            "&code=" + code +
            "&redirect_uri=" + GITCODE_REDIRECT_URL_KEY; 
            var formContent = new MultipartFormDataContent();
            formContent.Add(new StringContent(GITCODE_CLIENT_SECRET_KEY), "client_secret");

            var token = new AuthThirdToken();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsync(getTokenUri, formContent);
                    response.EnsureSuccessStatusCode();
                    string tokenResult = await response.Content.ReadAsStringAsync();
                    return JsonHelper.ToObject<AuthThirdToken>(tokenResult);
                }
                catch (HttpRequestException e)
                {
                    LogHelper.Error(e.Message, e);
                }
            }
            return await Task.FromResult(token);
        }


        /// <summary>
        /// 获取三方用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AuthThirdUser> GetThirdUserDetail(string token)
        {
            AuthThirdUser thirdUser = new AuthThirdUser();
            string getUserDetailUri = GITCODE_URI + $"/api/v5/user?access_token={token}";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(getUserDetailUri);
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();
                    thirdUser = JsonHelper.ToObject<AuthThirdUser>(result);
                    thirdUser.login = "gitcode-" + thirdUser.login;
                    thirdUser.extJson = result;
                    return thirdUser;
                }
                catch (HttpRequestException e)
                {
                    LogHelper.Error(e.Message, e);
                }
            }
            return thirdUser;
        }
    }
}
