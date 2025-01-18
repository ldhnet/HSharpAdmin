using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace HSharp.Util
{
    /// <summary>
    /// Smtp工具类
    /// </summary>
    public class HSharpMailUtil
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="to">收件人</param>
        /// <param name="files">附件</param>
        /// <param name="isBodyHtml">是否html</param>
        public static void SendMail(string subject, string body, string to, List<FileInfo> files = null, bool isBodyHtml = false)
        {
            var host = GlobalContext.MailConfig.Host;
            var port = GlobalContext.MailConfig.Port;
            var userName = GlobalContext.MailConfig.UserName;
            var password = GlobalContext.MailConfig.Password;
            var senderAddress = GlobalContext.MailConfig.SenderAddress;
            SmtpClient mailClient = null;
            try
            {
                if (port != null && port > 0)
                {
                    mailClient = new SmtpClient(host, (int)port);
                }
                else
                {
                    mailClient = new SmtpClient(host);
                }
                //SMTP服务器身份验证
                mailClient.Credentials = new NetworkCredential(userName, password);
                //发件人地址、收件人地址
                MailMessage message = new MailMessage(senderAddress, to);
                //邮件主题
                message.Subject = subject;
                //邮件内容
                message.Body = body;
                //是否html格式
                if (isBodyHtml)
                {
                    message.IsBodyHtml = true;
                }
                foreach (var file in files)
                {
                    //附件
                    Attachment att = new Attachment(file.FullName);
                    //添加附件
                    message.Attachments.Add(att);
                }
                //发送
                mailClient.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.Error(ex);
            }
        }
    }
}
