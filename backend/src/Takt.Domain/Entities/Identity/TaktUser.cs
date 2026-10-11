// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Identity
// 文件名称：TaktUser.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：用户实体，代表系统登录账号（身份认证域）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Identity;

/// <summary>
/// 用户实体
/// 代表系统登录账号（身份认证域）
/// 注意：用户与员工档案分离，用户仅用于认证和权限控制
/// 组合 2：无关联工厂、有语言（TaktTenantCultureEntityBase）
/// </summary>
[SugarTable("takt_identity_user", "用户表")]
[SugarIndex("ix_user_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_user_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_user_name_unique", nameof(TenantCode), OrderByType.Asc, nameof(UserName), OrderByType.Asc, true)]
public class TaktUser : TaktTenantCultureEntityBase
{
    /// <summary>
    /// 用户名（唯一索引：租户内唯一，见 ix_user_name_unique；登录账号，4–20 位）
    /// </summary>
    [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户名", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// 昵称（显示名称，2–20 位）
    /// </summary>
    [SugarColumn(ColumnName = "nick_name", ColumnDescription = "昵称", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string NickName { get; set; } = string.Empty;
    /// <summary>
    /// 用户类型（字典 sys_user_type）
    /// </summary>
    [SugarColumn(ColumnName = "user_type", ColumnDescription = "用户类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int UserType { get; set; } = 0;
    /// <summary>
    /// 密码哈希值（bcrypt加密）
    /// </summary>
    [SugarColumn(ColumnName = "password_hash", ColumnDescription = "密码哈希", ColumnDataType = "varchar", Length = 255, IsNullable = false)]
    public string PasswordHash { get; set; } = string.Empty;
    /// <summary>
    /// 关联的员工ID（（选项 TaktUsers/options；DictValue=Id,必须关联人事档案）
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "员工ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }
    /// <summary>
    /// 内置（字典 sys_yes_no；种子用户 admin/guest/demo 为内置，不允许删除）
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBuiltIn { get; set; } = 0;
    /// <summary>
    /// 最后登录时间（登录成功时 RecordUserLastLoginAsync 写入；登出不修改本字段）
    /// </summary>
    [SugarColumn(ColumnName = "last_login_at", ColumnDescription = "最后登录时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? LastLoginAt { get; set; }
    /// <summary>
    /// 最后登录IP
    /// </summary>
    [SugarColumn(ColumnName = "last_login_ip", ColumnDescription = "最后登录IP", ColumnDataType = "varchar", Length = 50, IsNullable = true)]
    public string? LastLoginIp { get; set; }
    /// <summary>
    /// 登录次数
    /// </summary>
    [SugarColumn(ColumnName = "login_count", ColumnDescription = "登录次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LoginCount { get; set; } = 0;
    /// <summary>
    /// 密码过期天数（0=永不过期，30=30天后过期）
    /// </summary>
    [SugarColumn(ColumnName = "password_expire_days", ColumnDescription = "密码过期天数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PasswordExpireDays { get; set; } = 0;
    /// <summary>
    /// 失败登录次数
    /// </summary>
    [SugarColumn(ColumnName = "login_fail_count", ColumnDescription = "失败登录次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LoginFailCount { get; set; } = 0;
    /// <summary>
    /// 锁定时间（登录失败过多时锁定）
    /// </summary>
    [SugarColumn(ColumnName = "locked_until", ColumnDescription = "锁定时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? LockedUntil { get; set; }
    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    [SugarColumn(ColumnName = "user_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int UserStatus { get; set; } = 1;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 用户角色关联（RBAC，表 takt_identity_user_role）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktUserRole.UserId))]
    public List<TaktUserRole>? UserRoles { get; set; }

    /// <summary>
    /// 用户可访问租户关联（RBAC，表 takt_identity_user_tenant）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktUserTenant.UserId))]
    public List<TaktUserTenant>? UserTenants { get; set; }

    /// <summary>
    /// 用户可访问公司关联（RBAC，表 takt_identity_user_company）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktUserCompany.UserId))]
    public List<TaktUserCompany>? UserCompanies { get; set; }

}
