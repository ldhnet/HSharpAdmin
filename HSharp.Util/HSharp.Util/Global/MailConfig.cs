using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSharp.Util.Global
{
    /// <summary>
    /// 邮件配置
    /// </summary>
    public class MailConfig
    {
        /// <summary>
        /// 主机地址
        /// </summary>
        public String Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 发件人地址
        /// </summary>
        public string SenderAddress { get; set; }
    }
}
