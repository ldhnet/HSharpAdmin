using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using HSharp.Util;
using System.ComponentModel;

namespace HSharp.Entity.SystemManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-03-24 11:34
    /// 描 述：站内信实体类
    /// </summary>
    [Table("SysMessageUser")]
    public class SysMessageUserEntity : BaseEntity
    { 
        [JsonConverter(typeof(DateTimeJsonConverter))]
        [Description("创建时间")]
        public DateTime? BaseCreateTime { get; set; } 
        [JsonConverter(typeof(StringJsonConverter))]
        public long MessageId { get; set; } 
        public long ReceiveUserId { get; set; } 
        public int IsRead { get; set; }
    }
}
