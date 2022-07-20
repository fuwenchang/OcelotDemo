using System;
using System.Collections.Generic;

namespace Hys.AddActivityLog.Models
{
    /// <summary>
    /// 服务调用记录
    /// </summary>
    public partial class Config
    {
        /// <summary>
        /// Id主键
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; } = null!;
        /// <summary>
        /// 配置项名称
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 配置项Key
        /// </summary>
        public string Key { get; set; } = null!;
        /// <summary>
        /// 配置项Value
        /// </summary>
        public string Value { get; set; } = null!;
        /// <summary>
        /// 备注信息
        /// </summary>
        public string? Remark { get; set; }
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
