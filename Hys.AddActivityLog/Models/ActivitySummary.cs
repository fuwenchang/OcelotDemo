using System;
using System.Collections.Generic;

namespace Hys.AddActivityLog.Models
{
    /// <summary>
    /// 服务调用汇总
    /// </summary>
    public partial class ActivitySummary
    {
        /// <summary>
        /// Id主键
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 调用服务的帐号Id
        /// </summary>
        public string AccountId { get; set; } = null!;
        /// <summary>
        /// 服务Id
        /// </summary>
        public string ServiceId { get; set; } = null!;
        /// <summary>
        /// 调用次数
        /// </summary>
        public int Times { get; set; }
        /// <summary>
        /// 成功次数
        /// </summary>
        public int Successes { get; set; }
        /// <summary>
        /// 失败次数
        /// </summary>
        public int Faults { get; set; }
        /// <summary>
        /// 平均调用耗时（毫秒）
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// 调用者IP
        /// </summary>
        public string? UserHostAddress { get; set; }
        /// <summary>
        /// 周期
        /// </summary>
        public int Period { get; set; }
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
