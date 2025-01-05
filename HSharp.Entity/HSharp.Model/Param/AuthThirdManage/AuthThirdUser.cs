using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSharp.Model.Param.AuthThirdManage
{
    public class AuthThirdUser
    {
        public string id;

        /// <summary>
        /// 三方用户id
        /// </summary>
        public string login; 

        /// <summary>
        /// 姓名
        /// </summary>
        public string name;

        /// <summary>
        /// 头像
        /// </summary>
        public string avatar_url;

        /// <summary>
        /// htmlurl
        /// </summary>
        public string html_url;

        /// <summary>
        /// type
        /// </summary>
        public string type;

        /// <summary>
        /// 简介
        /// </summary>
        public string bio;
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email;
        /// <summary>
        /// 备注
        /// </summary>
        public string remark;
        /// <summary>
        /// 扩展信息
        /// </summary>
        public string extJson;
    }
}
