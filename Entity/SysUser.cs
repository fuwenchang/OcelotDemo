using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Furion.DatabaseAccessor;

using Microsoft.EntityFrameworkCore;

namespace Api_Auth
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Comment("用户表")]
    public class SysUser
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        [Comment("账号")]
        [Required, MaxLength(50)]
        public string Account { get; set; }

        /// <summary>
        /// 密码（默认MD5加密）
        /// </summary>
        [Comment("密码")]
        [Required, MaxLength(50)]
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Comment("昵称")]
        [MaxLength(20)]
        public string NickName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Comment("姓名")]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Comment("头像")]
        public string Avatar { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Comment("生日")]
        public DateTimeOffset? Birthday { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Comment("邮箱")]
        [MaxLength(50)]
        public string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Comment("手机")]
        [MaxLength(20)]
        public string Phone { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Comment("电话")]
        [MaxLength(20)]
        public string Tel { get; set; }

        /// <summary>
        /// 最后登录IP
        /// </summary>
        [Comment("最后登录IP")]
        [MaxLength(20)]
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Comment("最后登录时间")]
        public DateTimeOffset? LastLoginTime { get; set; }

        /// <summary>
        /// 管理员类型-超级管理员_1、管理员_2、普通账号_3
        /// </summary>
        [Comment("管理员类型-超级管理员_1、管理员_2、普通账号_3")]
        public AdminType AdminType { get; set; }

        /// <summary>
        /// 状态-正常_0、停用_1、删除_2
        /// </summary>
        [Comment("状态-正常_0、停用_1、删除_2")]
        public CommonStatus Status { get; set; } = CommonStatus.ENABLE;


        /// <summary>
        /// 多对多中间表（用户-角色）
        /// </summary>
        public List<SysUserRole> SysUserRoles { get; set; }

        public string Roles { get; set; }
    }

    #region MyRegion
    /// <summary>
    /// 账号类型
    /// </summary>
    public enum AdminType
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        [Description("超级管理员")]
        SuperAdmin = 1,

        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        Admin = 2,

        /// <summary>
        /// 普通账号
        /// </summary>
        [Description("普通账号")]
        None = 3
    }
    public enum CommonStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        ENABLE = 0,

        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        DISABLE = 1,

        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        DELETED = 2
    }

    /// <summary>
    /// 用户角色表
    /// </summary>
    [Comment("用户角色表")]
    public class SysUserRole : IEntity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Comment("用户Id")]
        public long SysUserId { get; set; }

        /// <summary>
        /// 一对一引用（系统用户）
        /// </summary>
        public SysUser SysUser { get; set; }

        /// <summary>
        /// 系统角色Id
        /// </summary>
        [Comment("角色Id")]
        public long SysRoleId { get; set; }

    }
    #endregion
}