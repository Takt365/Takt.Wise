// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Identity
// 文件名称：TaktUserDtos.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：User 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktUser 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Identity;

// ========================================
// User 响应 DTO
// ========================================

/// <summary>
/// 用户实体 代表系统登录账号（身份认证域） 注意：用户与员工档案分离，用户仅用于认证和权限控制
/// 对应前端 TaktUserDto
/// 继承 TaktTenantCultureDtoBase
/// </summary>
public class TaktUserDto : TaktTenantCultureDtoBase
{
    /// <summary>
    /// UserID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户名（唯一索引：租户内唯一，见 ix_user_name_unique；登录账号，4–20 位）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 昵称（显示名称，2–20 位）
    /// </summary>
    public string NickName { get; set; } = string.Empty;

    /// <summary>
    /// 用户类型（字典 sys_user_type）
    /// </summary>
    public int UserType { get; set; } = 0;

    /// <summary>
    /// 密码哈希值（bcrypt加密）
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// 关联的员工ID（必须关联人事档案）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 关联的员工名称（填充字段）
    /// </summary>
    public string? EmployeeName { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；种子用户 admin/guest/demo 为内置，不允许删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// 最后登录IP
    /// </summary>
    public string? LastLoginIp { get; set; } = string.Empty;

    /// <summary>
    /// 登录次数
    /// </summary>
    public int LoginCount { get; set; } = 0;

    /// <summary>
    /// 密码过期天数（0=永不过期，30=30天后过期）
    /// </summary>
    public int PasswordExpireDays { get; set; } = 0;

    /// <summary>
    /// 失败登录次数
    /// </summary>
    public int LoginFailCount { get; set; } = 0;

    /// <summary>
    /// 锁定时间（登录失败过多时锁定）
    /// </summary>
    public DateTime? LockedUntil { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int UserStatus { get; set; } = 0;

    /// <summary>
    /// 用户角色关联（RBAC，表 takt_identity_user_role）
    /// （子表：TaktUserRole）
    /// </summary>
    public List<TaktUserRoleDto>? UserRoles { get; set; }

    /// <summary>
    /// 用户可访问租户关联（RBAC，表 takt_identity_user_tenant）
    /// （子表：TaktUserTenant）
    /// </summary>
    public List<TaktUserTenantDto>? UserTenants { get; set; }

    /// <summary>
    /// 用户可访问公司关联（RBAC，表 takt_identity_user_company）
    /// （子表：TaktUserCompany）
    /// </summary>
    public List<TaktUserCompanyDto>? UserCompanies { get; set; }

    /// <summary>
    /// 角色 ID 列表（填充字段）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long[]? RoleIds { get; set; }

    /// <summary>
    /// 角色名称列表（填充字段）
    /// </summary>
    public string[]? RoleNames { get; set; }

    /// <summary>
    /// 可访问公司编码列表（填充字段）
    /// </summary>
    public string[]? CompanyCodes { get; set; }

}

// ========================================
// User 查询 DTO
// ========================================

/// <summary>
/// User 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktUserQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;
    /// <summary>
    /// 用户名（唯一索引：租户内唯一，见 ix_user_name_unique；登录账号，4–20 位）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 昵称（显示名称，2–20 位）
    /// </summary>
    public string NickName { get; set; } = string.Empty;

    /// <summary>
    /// 用户类型（字典 sys_user_type）
    /// </summary>
    public int? UserType { get; set; }

    /// <summary>
    /// 密码哈希值（bcrypt加密）
    /// </summary>
    public string? PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// 关联的员工ID（必须关联人事档案）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；种子用户 admin/guest/demo 为内置，不允许删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 最后登录时间（范围查询-开始）
    /// </summary>
    public DateTime? LastLoginAtStart { get; set; }

    /// <summary>
    /// 最后登录时间（范围查询-结束）
    /// </summary>
    public DateTime? LastLoginAtEnd { get; set; }

    /// <summary>
    /// 最后登录IP
    /// </summary>
    public string? LastLoginIp { get; set; } = string.Empty;

    /// <summary>
    /// 登录次数
    /// </summary>
    public int? LoginCount { get; set; }

    /// <summary>
    /// 密码过期天数（0=永不过期，30=30天后过期）
    /// </summary>
    public int? PasswordExpireDays { get; set; }

    /// <summary>
    /// 失败登录次数
    /// </summary>
    public int? LoginFailCount { get; set; }

    /// <summary>
    /// 锁定时间（登录失败过多时锁定）（范围查询-开始）
    /// </summary>
    public DateTime? LockedUntilStart { get; set; }

    /// <summary>
    /// 锁定时间（登录失败过多时锁定）（范围查询-结束）
    /// </summary>
    public DateTime? LockedUntilEnd { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? UserStatus { get; set; }

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建人 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CreatedBy { get; set; }
}

// ========================================
// 创建User DTO
// ========================================

/// <summary>
/// 创建User DTO
/// </summary>
public class TaktUserCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;
    /// <summary>
    /// 用户名（唯一索引：租户内唯一，见 ix_user_name_unique；登录账号，4–20 位）
    /// </summary>
    [Required(ErrorMessage = "用户名（唯一索引：租户内唯一，见 ix_user_name_unique；登录账号，4–20 位）不能为空")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 昵称（显示名称，2–20 位）
    /// </summary>
    public string NickName { get; set; } = string.Empty;

    /// <summary>
    /// 用户类型（字典 sys_user_type）
    /// </summary>
    public int UserType { get; set; } = 0;

    /// <summary>
    /// 密码哈希值（bcrypt加密）
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// 关联的员工ID（必须关联人事档案）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    [Required(ErrorMessage = "区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）不能为空")]
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；种子用户 admin/guest/demo 为内置，不允许删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 密码过期天数（0=永不过期，30=30天后过期）
    /// </summary>
    public int PasswordExpireDays { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int UserStatus { get; set; } = 0;

    /// <summary>
    /// 用户角色关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? RoleIds { get; set; }

    /// <summary>
    /// 用户可访问租户关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public string[]? TenantCodes { get; set; }

    /// <summary>
    /// 用户可访问公司关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public string[]? CompanyCodes { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新User DTO
// ========================================

/// <summary>
/// 更新User DTO
/// 继承 TaktUserCreateDto，添加 UserId 字段
/// </summary>
public class TaktUserUpdateDto : TaktUserCreateDto
{
    /// <summary>
    /// UserID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

}

// ========================================
// User 状态 DTO
// ========================================

/// <summary>
/// User 状态更新 DTO
/// </summary>
public class TaktUserStatusDto
{
    /// <summary>
    /// UserID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    [Required(ErrorMessage = "状态（字典 sys_normal_disable）不能为空")]
    public int UserStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// User 导入模板行 DTO
/// </summary>
public class TaktUserTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;
    /// <summary>
    /// 用户名（唯一索引：租户内唯一，见 ix_user_name_unique；登录账号，4–20 位）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 昵称（显示名称，2–20 位）
    /// </summary>
    public string NickName { get; set; } = string.Empty;

    /// <summary>
    /// 用户类型（字典 sys_user_type）
    /// </summary>
    public int? UserType { get; set; }

    /// <summary>
    /// 密码哈希值（bcrypt加密）
    /// </summary>
    public string? PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// 关联的员工ID（必须关联人事档案）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；种子用户 admin/guest/demo 为内置，不允许删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 密码过期天数（0=永不过期，30=30天后过期）
    /// </summary>
    public int? PasswordExpireDays { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? UserStatus { get; set; }

    /// <summary>
    /// 用户角色关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? RoleIds { get; set; }

    /// <summary>
    /// 用户可访问租户关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public string[]? TenantCodes { get; set; }

    /// <summary>
    /// 用户可访问公司关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public string[]? CompanyCodes { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// User 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktUserImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;
    /// <summary>
    /// 用户名（唯一索引：租户内唯一，见 ix_user_name_unique；登录账号，4–20 位）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 昵称（显示名称，2–20 位）
    /// </summary>
    public string NickName { get; set; } = string.Empty;

    /// <summary>
    /// 用户类型（字典 sys_user_type）
    /// </summary>
    public int? UserType { get; set; }

    /// <summary>
    /// 密码哈希值（bcrypt加密）
    /// </summary>
    public string? PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// 关联的员工ID（必须关联人事档案）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 员工编码（导入时按编码匹配人事档案）
    /// </summary>
    public string? EmployeeCode { get; set; }

    /// <summary>
    /// 用户类型名称（导入 Excel 字典标签）
    /// </summary>
    public string? UserTypeName { get; set; }

    /// <summary>
    /// 状态名称（导入 Excel 字典标签）
    /// </summary>
    public string? StatusName { get; set; }

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；种子用户 admin/guest/demo 为内置，不允许删除）
    /// </summary>
    public int? IsBuiltIn { get; set; }

    /// <summary>
    /// 密码过期天数（0=永不过期，30=30天后过期）
    /// </summary>
    public int? PasswordExpireDays { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int? UserStatus { get; set; }

    /// <summary>
    /// 用户角色关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public long[]? RoleIds { get; set; }

    /// <summary>
    /// 用户可访问租户关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public string[]? TenantCodes { get; set; }

    /// <summary>
    /// 用户可访问公司关联（RBAC 全量覆盖，分配走 ITaktRbacService）
    /// </summary>
    public string[]? CompanyCodes { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// User 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktUserExportDto
{
    /// <summary>
    /// UserID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户名（唯一索引：租户内唯一，见 ix_user_name_unique；登录账号，4–20 位）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 昵称（显示名称，2–20 位）
    /// </summary>
    public string NickName { get; set; } = string.Empty;

    /// <summary>
    /// 用户类型（字典 sys_user_type）
    /// </summary>
    public int UserType { get; set; } = 0;

    /// <summary>
    /// 密码哈希值（bcrypt加密）
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// 关联的员工ID（必须关联人事档案）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }

    /// <summary>
    /// 关联的员工名称（填充字段）
    /// </summary>
    public string? EmployeeName { get; set; }

    /// <summary>
    /// 用户类型名称（导出字典标签）
    /// </summary>
    public string? UserTypeName { get; set; }

    /// <summary>
    /// 状态名称（导出字典标签）
    /// </summary>
    public string? StatusName { get; set; }

    /// <summary>
    /// 角色名称（导出填充，逗号分隔）
    /// </summary>
    public string? RoleNames { get; set; }

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；种子用户 admin/guest/demo 为内置，不允许删除）
    /// </summary>
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// 最后登录IP
    /// </summary>
    public string? LastLoginIp { get; set; } = string.Empty;

    /// <summary>
    /// 登录次数
    /// </summary>
    public int LoginCount { get; set; } = 0;

    /// <summary>
    /// 密码过期天数（0=永不过期，30=30天后过期）
    /// </summary>
    public int PasswordExpireDays { get; set; } = 0;

    /// <summary>
    /// 失败登录次数
    /// </summary>
    public int LoginFailCount { get; set; } = 0;

    /// <summary>
    /// 锁定时间（登录失败过多时锁定）
    /// </summary>
    public DateTime? LockedUntil { get; set; }

    /// <summary>
    /// 状态（字典 sys_normal_disable）
    /// </summary>
    public int UserStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

// ========================================
// 用户创建/更新别名与密码 DTO
// ========================================

/// <summary>
/// 创建用户 DTO（与 TaktUserCreateDto 同义，供服务/控制器引用）
/// </summary>
public class TaktCreateUserDto : TaktUserCreateDto
{
}

/// <summary>
/// 更新用户 DTO（与 TaktUserUpdateDto 同义，供服务/控制器引用）
/// </summary>
public class TaktUpdateUserDto : TaktUserUpdateDto
{
}

/// <summary>
/// 重置密码 DTO（管理员按 UserId 重置）
/// </summary>
public class TaktResetPasswordDto
{
    /// <summary>
    /// 用户 ID
    /// </summary>
    [Required(ErrorMessage = "用户ID不能为空")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 新密码
    /// </summary>
    [Required(ErrorMessage = "新密码不能为空")]
    public string NewPassword { get; set; } = string.Empty;
}

/// <summary>
/// 修改密码 DTO（当前登录用户修改自己的密码）
/// </summary>
public class TaktChangePasswordDto
{
    /// <summary>
    /// 旧密码
    /// </summary>
    [Required(ErrorMessage = "旧密码不能为空")]
    public string OldPassword { get; set; } = string.Empty;

    /// <summary>
    /// 新密码
    /// </summary>
    [Required(ErrorMessage = "新密码不能为空")]
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// 确认新密码
    /// </summary>
    [Required(ErrorMessage = "确认密码不能为空")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// 当前登录用户更新头像 DTO（写入关联员工 Avatar；空串表示清除）
/// </summary>
public class TaktUpdateMyAvatarDto
{
    /// <summary>
    /// 头像访问 URL（空或空白表示清除；最长 500，与员工 Avatar 列一致）
    /// </summary>
    [MaxLength(500)]
    public string? Avatar { get; set; }
}

/// <summary>
/// 当前登录用户更新个人资料 DTO
/// NickName → takt_identity_user；Mobile/Email → 当前会话公司下解析后的 takt_human_resource_personnel_employee（姓名/性别不可自助修改）
/// </summary>
public class TaktUpdateMyProfileDto
{
    /// <summary>
    /// 昵称（落库 TaktUser.NickName；2–20 位）
    /// </summary>
    [Required(ErrorMessage = "昵称不能为空")]
    [MaxLength(20)]
    public string NickName { get; set; } = string.Empty;

    /// <summary>
    /// 手机号码（落库 TaktEmployee.Mobile；已绑定员工时必填）
    /// </summary>
    [MaxLength(11)]
    public string? Mobile { get; set; }

    /// <summary>
    /// 电子邮箱（落库 TaktEmployee.Email；已绑定员工时必填）
    /// </summary>
    [MaxLength(100)]
    public string? Email { get; set; }
}

/// <summary>
/// 当前登录用户更新个人资料结果（便于前端同步 userInfo，无需整份 /me 重载）
/// </summary>
public class TaktUpdateMyProfileResultDto
{
    /// <summary>
    /// 昵称（TaktUser）
    /// </summary>
    public string NickName { get; set; } = string.Empty;

    /// <summary>
    /// 手机号码（TaktEmployee）
    /// </summary>
    public string? Mobile { get; set; }

    /// <summary>
    /// 电子邮箱（TaktEmployee）
    /// </summary>
    public string? Email { get; set; }
}

/// <summary>
/// 忘记密码 DTO
/// </summary>
public class TaktForgotPasswordDto
{
    /// <summary>
    /// 用户名或邮箱
    /// </summary>
    [Required(ErrorMessage = "用户名或邮箱不能为空")]
    public string UserNameOrEmail { get; set; } = string.Empty;
}

/// <summary>
/// 忘记密码结果 DTO
/// </summary>
public class TaktForgotPasswordResultDto
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// 错误码（Success 为 false 时有效）
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 提示信息
    /// </summary>
    public string? Message { get; set; }
}

/// <summary>
/// 解锁用户 DTO
/// </summary>
public class TaktUserUnlockDto
{
    /// <summary>
    /// 用户 ID
    /// </summary>
    [Required(ErrorMessage = "用户ID不能为空")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 解锁原因
    /// </summary>
    public string? Reason { get; set; }
}
