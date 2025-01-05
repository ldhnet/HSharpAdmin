using Azure;
using HSharp.Entity.OrganizationManage;
using HSharp.Model.Param.AuthThirdManage;
using HSharp.Model.Param.OrganizationManage;
using HSharp.Model.Result.AuthThirdManage;
using HSharp.Util;
using HSharp.Util.Model;
using Org.BouncyCastle.Tsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HSharp.Service.AuthThirdManage
{
    public class AuthThirdService
    {
        public const string GITEE_URI = "https://gitee.com";
        public const string GITEE_RESPONSE_TYPE = "code";
        public const string GITEE_STATE = "hsharpadmin";
        public const string GITEE_CLIENT_ID = "e6b611e97a54e5c80ae99c03c6934133d7de88df0f219a1cda210389f2664cf6";
        public const string GITEE_CLIENT_SECRET_KEY = "da170f47d3aab74d1453c7a161cda2988cd03e8c5bc68ac3b40252da7e74a778";
        public const string GITEE_REDIRECT_URL_KEY = "http://117.72.70.166:9000/Oauth/Callback";
        /// <summary>
        /// 第三方登录页面渲染
        /// </summary>
        /// <param name="authThirdRenderParam"></param>
        /// <returns></returns>
        public async Task<AuthThirdRenderResult> Render()
        { 
            AuthThirdRenderResult authThirdRenderResult = new AuthThirdRenderResult(); 
            string url = GITEE_URI + "/oauth/authorize?" +
                "response_type=" + GITEE_RESPONSE_TYPE +
                "&client_id=" + GITEE_CLIENT_ID +
                "&state=" + GITEE_STATE +
                "&redirect_uri=" + GITEE_REDIRECT_URL_KEY;
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
            string getTokenUri = GITEE_URI + "/oauth/token";
            AuthThirdToken token = new AuthThirdToken(); 
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(new
                {
                    grant_type = "authorization_code",
                    code,
                    client_id = GITEE_CLIENT_ID,
                    client_secret = GITEE_CLIENT_SECRET_KEY,
                    redirect_uri = GITEE_REDIRECT_URL_KEY, 
                }),Encoding.UTF8,"application/json");
             
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsync(getTokenUri, jsonContent);
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
            string getUserDetailUri = GITEE_URI + $"/api/v5/user?access_token={token}";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(getUserDetailUri);
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync(); 
                    thirdUser = JsonHelper.ToObject<AuthThirdUser>(result);
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
        /// <summary>
        /// 获取三方用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns> 
        public async Task<bool> IsCheckStarred(string token)
        {
            AuthThirdUser thirdUser = new AuthThirdUser();
            string getCheckStarredUri = GITEE_URI + $"/api/v5/user/starred/ldhnet/HSahrpAdmin?access_token={token}";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(getCheckStarredUri);
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();
                   
                }
                catch (HttpRequestException e)
                {
                    LogHelper.Error(e.Message, e);
                }
            }
            return true;
        }
        /// <summary>
        /// 获取三方用户分页
        /// </summary>
        /// <param name="authThirdUserPageParam"></param>
        /// <returns></returns> 
        public async Task<List<AuthThirdUser>> GetUserPage(string token, Pagination pagination)
        {
            string getUserPageUri = GITEE_URI + "/api/v5/user"; 
            return await Task.FromResult(new List<AuthThirdUser>());
        } 

    }
}
