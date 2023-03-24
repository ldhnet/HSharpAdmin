using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using HSharp.Util;

namespace HSharp.Entity.SystemManage
{
    /// <summary>
    /// 创 建：admin
    /// 日 期：2023-03-24 11:34
    /// 描 述：站内信实体类
    /// </summary>
    [Table("SysMessageContent")]
    public class SysMessageContentEntity : BaseCreateEntity
    {
        /// <summary>
        /// 站内信内容
        /// </summary>
        /// <returns></returns>
        public string Content { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
        /// <summary>
        /// 发送人ID
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? SendUserId { get; set; }
    }
}
