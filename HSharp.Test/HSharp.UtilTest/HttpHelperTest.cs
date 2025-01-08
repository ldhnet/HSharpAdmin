using System;
using System.Text;
using NUnit.Framework;
using HSharp.Util;
using System.Threading.Tasks;
using HSharp.Model.Result.AuthThirdManage;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net.Http;
using HSharp.Model.Param.AuthThirdManage;

namespace HSharp.UtilTest
{
    public class HttpHelperTest
    {
        [Test]
        public async Task TestMD5Encrypt()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  // 注册Encoding
       
            HttpResult httpResult =await HttpHelper.GetHtml(new HttpItem
            {
                //URL = "http://whois.pconline.com.cn/ip.jsp?ip=117.64.156.76",

                URL = "http://whois.pconline.com.cn/ip.jsp?ip=114.94.9.30", 
                ContentType = "text/html; charset=gb2312"
            });           
        }

        [Test]
        public async Task TestHttpClientFactoryUtil()
        { 
            string getTokenUri = "https://www.baidu.com/";

            var client = HttpClientFactoryUtil.Instance.CreateClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(getTokenUri);
                response.EnsureSuccessStatusCode();
                string tokenResult = await response.Content.ReadAsStringAsync();
 
            }
            catch (HttpRequestException e)
            {
                LogHelper.Error(e.Message, e);
            } 
        }

        [Test]
        public async Task TestHttpClientUtil()
        {
            string getTokenUri = "http://117.72.70.166/";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(getTokenUri);
                    response.EnsureSuccessStatusCode();
                  
                    string result = await response.Content.ReadAsStringAsync(); 
                }
                catch (HttpRequestException e)
                {
                    LogHelper.Error(e.Message, e);
                }
            }
        }
    }
}
