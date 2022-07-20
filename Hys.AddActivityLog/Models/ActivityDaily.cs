using System;
using System.Collections.Generic;

using Furion.DatabaseAccessor;

namespace Hys.AddActivityLog.Models
{
    /// <summary>
    /// 服务调用记录
    /// </summary>
    public partial class ActivityDaily : IEntity
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
        /// 订阅Id
        /// </summary>
        public string InterfaceId { get; set; } = null!;
        /// <summary>
        /// 调用耗时（毫秒）
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string? ServerHostAddress { get; set; }
        /// <summary>
        /// 调用者IP
        /// </summary>
        public string? UserHostAddress { get; set; }
        /// <summary>
        /// 输入
        /// </summary>
        public string? Input { get; set; }
        /// <summary>
        /// 输出
        /// </summary>
        public string? Output { get; set; }
        /// <summary>
        /// 状态(-1:失败 0:未知 1:成功 2:
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 调用状态
        /// </summary>
        public int CallStatus { get; set; }
        /// <summary>
        /// 服务开始
        /// </summary>
        public DateTime ServiceStart { get; set; }
        /// <summary>
        /// 服务结束
        /// </summary>
        public DateTime ServiceEnd { get; set; }
        /// <summary>
        /// 服务耗时
        /// </summary>
        public int ServiceDuration { get; set; }
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
