﻿using System;
using System.Collections.Generic;
using HSharp.Util.Model;

namespace HSharp.Model.Param.SystemManage
{
    public class AreaListParam
    {
        public string AreaName { get; set; }
    }

    public class BaseAreaParam : BaseApiToken
    {
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public int? CountyId { get; set; }
        /// <summary>
        /// 逗号分隔的Id
        /// </summary>
        public string AreaId { get; set; }
    }
}
