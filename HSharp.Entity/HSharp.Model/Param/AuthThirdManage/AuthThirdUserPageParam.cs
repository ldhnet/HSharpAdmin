using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSharp.Model.Param.AuthThirdManage
{
    public class AuthThirdUserPageParam
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int current;

        /// <summary>
        /// 每页条数
        /// </summary>
        public int size;

        /** 排序字段 */

        ///<summary>
        ///排序字段，字段驼峰名称，如：userName
        ///</summary>
        public string sortField;

        /// <summary>
        /// 排序方式，升序：ASCEND；降序：DESCEND
        /// </summary>
        public string sortOrder;

        /// <summary>
        /// 三方用户分类
        /// </summary>
        public string category;

        /// <summary>
        /// 用户名或昵称关键词
        /// </summary>
        public string searchKey;
    }
}
