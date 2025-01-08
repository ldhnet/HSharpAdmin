using HSharp.Util.Extension;  
using System;
using System.Collections.Generic;
using System.IO; 
using System.Net;
using System.Net.Http; 
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates; 
using System.Text;
using System.Text.Json; 
using System.Threading.Tasks; 

namespace HSharp.Util
{
    /// <summary>
    /// Http连接操作帮助类
    /// </summary>
    public class HttpHelper
    {
        #region 是否是网址

        public static bool IsUrl(string url)
        {
            url = url.ParseToString().ToLower();
            if (url.StartsWith("http://") || url.StartsWith("https://"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion 是否是网址 

        #region 模拟GET

        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="postDataStr">The post data string.</param>
        /// <returns>System.String.</returns>
        public static async Task<string> HttpGet(string url, int timeout = 10 * 1000)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(timeout); 
                client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

        #endregion 模拟GET

        #region 模拟POST

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="posturl">The posturl.</param>
        /// <param name="postData">The post data.</param>
        /// <returns>System.String.</returns>
        public static async Task<string> HttpPost(string posturl, string postData, string contentType = "application/x-www-form-urlencoded", int timeout = 10 * 1000)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(timeout); 
                client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
                var jsonContent = new StringContent(JsonSerializer.Serialize(postData), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(posturl, jsonContent);                 
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();  
            } 
        }

        /// <summary>
        /// 模拟httpPost提交表单
        /// </summary>
        /// <param name="url">POS请求的网址</param>
        /// <param name="data">表单里的参数和值</param>
        /// <param name="encoder">页面编码</param>
        /// <returns></returns>
        public static string CreateAutoSubmitForm(string url, Dictionary<string, string> data, Encoding encoder)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendFormat("<meta http-equiv=\"Content-Type\" content=\"text/html; charset={0}\" />", encoder.BodyName);
            html.AppendLine("</head>");
            html.AppendLine("<body onload=\"OnLoadSubmit();\">");
            html.AppendFormat("<form id=\"pay_form\" action=\"{0}\" method=\"post\">", url);
            foreach (KeyValuePair<string, string> kvp in data)
            {
                html.AppendFormat("<input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"{1}\" />", kvp.Key, kvp.Value);
            }
            html.AppendLine("</form>");
            html.AppendLine("<script type=\"text/javascript\">");
            html.AppendLine("<!--");
            html.AppendLine("function OnLoadSubmit()");
            html.AppendLine("{");
            html.AppendLine("document.getElementById(\"pay_form\").submit();");
            html.AppendLine("}");
            html.AppendLine("//-->");
            html.AppendLine("</script>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            return html.ToString();
        }

        #endregion 模拟POST

        #region 普通类型

        /// <summary>
        /// 传入一个正确或不正确的URl，返回正确的URL
        /// </summary>
        /// <param name="URL">url</param>
        /// <returns>
        /// </returns>
        public static string GetUrl(string URL)
        {
            if (!(URL.Contains("http://") || URL.Contains("https://")))
            {
                URL = "http://" + URL;
            }
            return URL;
        }

        #endregion 普通类型

        #region 预定义方法或者变更

        ///<summary>
        ///采用https协议访问网络,根据传入的URl地址，得到响应的数据字符串。
        ///</summary>
        ///<param name="httpItem">参数列表</param>
        ///<returns>String类型的数据</returns>
        public static async Task<HttpResult> GetHtml(HttpItem httpItem)
        {
            //返回参数
            HttpResult result = new HttpResult();
            try
            {
                #region 得到请求的response
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(httpItem.Timeout); 
                    client.DefaultRequestHeaders.Add("Accept", httpItem.Accept);
                    client.DefaultRequestHeaders.Add("User-Agent", httpItem.UserAgent);
                    HttpResponseMessage response = await client.GetAsync(httpItem.URL);
                    response.EnsureSuccessStatusCode();
                    result.Html = await response.Content.ReadAsStringAsync();
                    result.ResultString = response.ToString();

                }                 
                #endregion 得到请求的response
            }
            catch (Exception ex)
            { 
                result.Html = "String Error:" + ex.Message;
            } 
            if (httpItem.IsToLower)
            {
                result.Html = result.Html.ToLower();
            }
            return result;
        }  
        /// <summary>
        /// 验证证书 未用到
        /// </summary>
        /// <param name="certificatePath"></param>
        /// <param name="privateKeyPath"></param>
        /// <returns></returns>
        public static async Task<X509Certificate2> LoadPemCertificate(string certificatePath, string privateKeyPath)
        {
            using var publicKey = new X509Certificate2(certificatePath);

            var privateKeyText = await File.ReadAllTextAsync(privateKeyPath);
            var privateKeyBlocks = privateKeyText.Split("-", StringSplitOptions.RemoveEmptyEntries);
            var privateKeyBytes = Convert.FromBase64String(privateKeyBlocks[1]);
            using var rsa = RSA.Create();

            if (privateKeyBlocks[0] == "BEGIN PRIVATE KEY")
            {
                rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
            }
            else if (privateKeyBlocks[0] == "BEGIN RSA PRIVATE KEY")
            {
                rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
            }

            var keyPair = publicKey.CopyWithPrivateKey(rsa);
            return new X509Certificate2(keyPair.Export(X509ContentType.Pfx));
        }
         
        #endregion 预定义方法或者变更
          
    } 
    /// <summary>
    /// Http请求参考类
    /// </summary>
    public class HttpItem
    { 
        /// <summary>
        /// 请求URL必须填写
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// 请求方式默认为GET方式
        /// </summary>
        public string Method { get; set; } = "GET";     

        /// <summary>
        /// 默认请求超时时间
        /// </summary>
        public int Timeout { get; set; } = 100000;
     
        /// <summary>
        /// 默认写入Post数据超时间
        /// </summary>
        public int ReadWriteTimeout { get; set; } = 30000;  
        /// <summary>
        /// 请求标头值 默认为text/html, application/xhtml+xml, */*
        /// </summary>
        public string Accept { get; set; } = "text/html, application/xhtml+xml, */*"; 
        /// <summary>
        /// 请求返回类型默认 text/html
        /// </summary>
        public string ContentType { get; set; } = "text/html"; 
         
        /// <summary>
        /// 客户端访问信息默认Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)
        /// </summary>
        public string UserAgent { get; set; } = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
    
        /// <summary>
        /// 返回数据编码默认为NUll,可以自动识别
        /// </summary>
        public string Encoding { get; set; } 
        /// <summary>
        /// Post的数据类型
        /// </summary>
        public PostDataType PostDataType { get; set; } = PostDataType.String;
     
        /// <summary>
        /// Post请求时要发送的字符串Post数据
        /// </summary>
        public string Postdata { get; set; } 
   
        /// <summary>
        /// Post请求时要发送的Byte类型的Post数据
        /// </summary>
        public byte[] PostdataByte { get; set; }  
        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection { get; set; } = null; 

        /// <summary>
        /// 请求时的Cookie
        /// </summary>
        public string Cookie { get; set; }  
        /// <summary>
        /// 来源地址，上次访问地址
        /// </summary>
        public string Referer { get; set; } = string.Empty; 
        /// <summary>
        /// 证书绝对路径
        /// </summary>
        public string CerPath { get; set; } = string.Empty; 
        /// <summary>
        /// 是否设置为全文小写
        /// </summary>
        public Boolean IsToLower { get; set; } = true;  
        /// <summary>
        /// 支持跳转页面，查询结果将是跳转后的页面
        /// </summary>
        public Boolean Allowautoredirect { get; set; } = true;  
        /// <summary>
        /// 最大连接数
        /// </summary>
        public int Connectionlimit { get; set; } = 1024;   
        /// <summary>
        /// 代理Proxy 服务器用户名
        /// </summary>
        public string ProxyUserName { get; set; } = string.Empty;  

        /// <summary>
        /// 代理 服务器密码
        /// </summary>
        public string ProxyPwd { get; set; }  
        /// <summary>
        /// 代理 服务IP
        /// </summary>
        public string ProxyIp { get; set; } = string.Empty;   
        /// <summary>
        /// 设置返回类型String和Byte
        /// </summary>
        public ResultType ResultType { get; set; } = ResultType.String;
        
    }

    /// <summary>
    /// Http返回参数类
    /// </summary>
    public class HttpResult
    {  
        /// <summary>
        /// 返回的String类型数据 只有ResultType.String时才返回数据，其它情况为空
        /// </summary>
        public string Html { get; set; } = string.Empty;    
        /// <summary>
        /// 返回的Byte数组 只有ResultType.Byte时才返回数据，其它情况为空
        /// </summary>
        public byte[] ResultByte { get; set; } = null;
        /// <summary>
        /// Http请求返回的 
        /// </summary>
        public string ResultString { get; set; } = string.Empty;
    }

    /// <summary>
    /// 返回类型
    /// </summary>
    public enum ResultType
    {
        String, //表示只返回字符串
        Byte //表示返回字符串和字节流
    }

    /// <summary>
    /// Post的数据格式默认为string
    /// </summary>
    public enum PostDataType
    {
        String, //字符串
        Byte, //字符串和字节流
        FilePath //表示传入的是文件
    }
}