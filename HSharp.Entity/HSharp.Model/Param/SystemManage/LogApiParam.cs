﻿using System;
using System.Collections.Generic;

namespace HSharp.Model.Param.SystemManage
{
    public class LogApiListParam : DateTimeParam
    {
        public string UserName { get; set; }
        public string ExecuteUrl { get; set; }
        public int? LogStatus { get; set; }
    }
}
