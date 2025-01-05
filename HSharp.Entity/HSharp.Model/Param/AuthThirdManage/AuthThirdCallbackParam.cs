using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSharp.Model.Param.AuthThirdManage
{
    public class AuthThirdCallbackParam
    {
        /// <summary>
        /// 第三方平台标识
        /// </summary>
        public string platform;

        /// <summary>
        /// 第三方回调code 
        /// </summary>
        public string code;

        /// <summary>
        /// 第三方回调state
        /// </summary>
        public string state;
    }
}
