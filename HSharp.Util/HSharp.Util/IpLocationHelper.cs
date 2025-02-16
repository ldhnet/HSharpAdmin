﻿using System;
using System.Threading.Tasks;

namespace HSharp.Util
{
    public class IpLocationHelper
    {
        #region IP位置查询

        public static async Task<string> GetIpLocation(string ipAddress)
        {
            string ipLocation = string.Empty;
            try
            {
                if (!IsInnerIP(ipAddress))
                {
                    ipLocation =await GetIpLocationFromTaoBao(ipAddress);
                    //内网IP判断
                    if (string.IsNullOrEmpty(ipLocation))
                    {
                        ipLocation = await GetIpLocationFromPCOnline(ipAddress);
                    }                    
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return ipLocation;
        }

        private static async Task<string> GetIpLocationFromTaoBao(string ipAddress)
        {
            string url = "http://ip.taobao.com/service/getIpInfo2.php";
            string postData = string.Format("ip={0}", ipAddress);
            string result =await HttpHelper.HttpPost(url, postData);
            string ipLocation = string.Empty;
            if (!string.IsNullOrEmpty(result))
            {
                var json = JsonHelper.ToJObject(result);
                var jsonData = json["data"];
                if (jsonData != null)
                {
                    ipLocation = jsonData["region"] + " " + jsonData["city"];
                    ipLocation = ipLocation.Trim();
                }
            }
            return ipLocation;
        }

        private static async Task<string> GetIpLocationFromPCOnline(string ipAddress)
        {
            HttpResult httpResult = await HttpHelper.GetHtml(new HttpItem
            {
                URL = "http://whois.pconline.com.cn/ip.jsp?ip=" + ipAddress,
                ContentType = "text/html; charset=gb2312"
            });

            string ipLocation = string.Empty;
            if (!string.IsNullOrEmpty(httpResult.Html))
            {
                var resultArr = httpResult.Html.Split(' ');
                ipLocation = resultArr[0].Replace("省", "  ").Replace("市", "");
                ipLocation = ipLocation.Trim();
            }
            return ipLocation;
        }

        #endregion IP位置查询

        #region 判断是否是外网IP

        public static bool IsInnerIP(string ipAddress)
        {
            bool isInnerIp = false;
            long ipNum = GetIpNum(ipAddress);
            /**
                私有IP：A类 10.0.0.0-10.255.255.255
                            B类 172.16.0.0-172.31.255.255
                            C类 192.168.0.0-192.168.255.255
                当然，还有127这个网段是环回地址
           **/
            long aBegin = GetIpNum("10.0.0.0");
            long aEnd = GetIpNum("10.255.255.255");
            long bBegin = GetIpNum("172.16.0.0");
            long bEnd = GetIpNum("172.31.255.255");
            long cBegin = GetIpNum("192.168.0.0");
            long cEnd = GetIpNum("192.168.255.255");
            isInnerIp = IsInner(ipNum, aBegin, aEnd) || IsInner(ipNum, bBegin, bEnd) || IsInner(ipNum, cBegin, cEnd) || ipAddress.Equals("127.0.0.1");
            return isInnerIp;
        }

        /// <summary>
        /// 把IP地址转换为Long型数字
        /// </summary>
        /// <param name="ipAddress">IP地址字符串</param>
        /// <returns></returns>
        private static long GetIpNum(string ipAddress)
        {
            string[] ip = ipAddress.Split('.');
            long a = int.Parse(ip[0]);
            long b = int.Parse(ip[1]);
            long c = int.Parse(ip[2]);
            long d = int.Parse(ip[3]);

            long ipNum = a * 256 * 256 * 256 + b * 256 * 256 + c * 256 + d;
            return ipNum;
        }

        private static bool IsInner(long userIp, long begin, long end)
        {
            return (userIp >= begin) && (userIp <= end);
        }

        #endregion 判断是否是外网IP
    }
}