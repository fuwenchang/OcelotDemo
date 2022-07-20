using System;
using System.Collections.Generic;

namespace Hys.AddActivityLog.Models
{
    /// <summary>
    /// 账号表
    /// </summary>
    public partial class Account
    {
        /// <summary>
        /// Id主键
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 系统编码
        /// </summary>
        public string Appcode { get; set; } = null!;
        /// <summary>
        /// 系统密码
        /// </summary>
        public string Password { get; set; } = null!;
        /// <summary>
        /// 系统名称
        /// </summary>
        public string Appname { get; set; } = null!;
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; } = null!;
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Mobile { get; set; } = null!;
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; } = null!;
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Company { get; set; } = null!;
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remarks { get; set; }
        /// <summary>
        /// 账号状态
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
