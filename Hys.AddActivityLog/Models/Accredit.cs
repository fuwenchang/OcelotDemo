using System;
using System.Collections.Generic;

namespace Hys.AddActivityLog.Models
{
    /// <summary>
    /// 授权
    /// </summary>
    public partial class Accredit
    {
        /// <summary>
        /// Id主键
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 服务Id
        /// </summary>
        public string ServiceId { get; set; } = null!;
        /// <summary>
        /// 帐号Id
        /// </summary>
        public string AccountId { get; set; } = null!;
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset? CreatedTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTimeOffset? UpdatedTime { get; set; }
        /// <summary>
        /// 创建者Id
        /// </summary>
        public long? CreatedUserId { get; set; }
        /// <summary>
        /// 创建者名称
        /// </summary>
        public string? CreatedUserName { get; set; }
        /// <summary>
        /// 修改者Id
        /// </summary>
        public long? UpdatedUserId { get; set; }
        /// <summary>
        /// 修改者名称
        /// </summary>
        public string? UpdatedUserName { get; set; }
        /// <summary>
        /// 软删除标记
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
