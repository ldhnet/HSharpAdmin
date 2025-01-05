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
        public string thirdId;

        /// <summary>
        /// 系统用户id
        /// </summary>
        public string userId;

        /// <summary>
        /// 头像
        /// </summary>
        public string avatar;

        /// <summary>
        /// 姓名
        /// </summary>
        public string name;

        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname;

        /// <summary>
        /// 性别
        /// </summary>
        public string gender;

        /// <summary>
        /// 分类
        /// </summary>
        public string category;
        /// <summary>
        /// 扩展信息
        /// </summary>
        public string extJson;
    }
}
