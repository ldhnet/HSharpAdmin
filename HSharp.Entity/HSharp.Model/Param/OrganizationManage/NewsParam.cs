using System;
using System.Collections.Generic;
using HSharp.Model.Param.SystemManage;

namespace HSharp.Model.Param.OrganizationManage
{
    public class NewsListParam : BaseAreaParam
    {
        public string NewsTitle { get; set; }
        public int? NewsType { get; set; }
        public string NewsTag { get; set; }
    }
}
