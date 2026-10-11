// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Identity
// 文件名称：TaktLoginDtos.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：用户登录相关 DTO（登录请求、登录响应、登录票据载荷、刷新令牌、修改密码、用户信息）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Enums;

namespace Takt.Application.Dtos.Identity;

// ========================================
// 登录请求/响应 DTO
// ========================================

/// <summary>
/// 登录密码 RSA 公钥响应
/// </summary>
public class TaktLoginPublicKeyResponseDto
{
    /// <summary>
    /// 算法标识（RSA-PKCS1）
    /// </summary>
    public string Algorithm { get; set; } = "RSA-PKCS1";

    /// <summary>
    /// RSA 公钥 PEM
    /// </summary>
    public string PublicKeyPem { get; set; } = string.Empty;
}

/// <summary>
/// 用户登录请求 DTO
/// </summary>
public class TaktLoginRequestDto
{
    /// <summary>
    /// 用户名（8位）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 密码（RSA PKCS#1 密文 Base64；signin 有 LoginTicket 时可省略）
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 租户编码（用于多租户登录）
    /// </summary>
    public string? TenantCode { get; set; }

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; }

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; }

    /// <summary>
    /// 验证码（如果启用）
    /// </summary>
    public string? CaptchaCode { get; set; }

    /// <summary>
    /// 验证码ID（如果启用）
    /// </summary>
    public string? CaptchaId { get; set; }

    /// <summary>
    /// 记住我（延长Token有效期）
    /// </summary>
    public bool RememberMe { get; set; } = false;

    /// <summary>
    /// 登录票据（由 session/verify-password 签发；signin 凭此跳过重复验密）
    /// </summary>
    public string? LoginTicket { get; set; }
}

/// <summary>
/// 登录预检请求 DTO（点登录：先租户用户权限，再密码；通过后决定是否弹验证码）
/// </summary>
public class TaktSessionVerifyPasswordRequestDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 密码（RSA PKCS#1 密文 Base64）
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 租户编码
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 验证码 ID（启用验证码时必填）
    /// </summary>
    public string? CaptchaId { get; set; }

    /// <summary>
    /// 验证码载荷（启用验证码时必填）
    /// </summary>
    public string? CaptchaCode { get; set; }
}

/// <summary>
/// 登录预检响应 DTO（密码已通过；captchaRequired 为 true 时前端弹验证码后再 signin）
/// </summary>
public class TaktSessionVerifyPasswordResponseDto
{
    /// <summary>
    /// 密码是否通过
    /// </summary>
    public bool PasswordValid { get; set; }

    /// <summary>
    /// 是否需要弹出验证码
    /// </summary>
    public bool CaptchaRequired { get; set; }

    /// <summary>
    /// 登录票据（短时有效，供 session/signin 使用）
    /// </summary>
    public string LoginTicket { get; set; } = string.Empty;
}

/// <summary>
/// 登录票据缓存载荷（verify-password 签发，signin 一次性消费）
/// </summary>
public class TaktLoginTicketPayload
{
    /// <summary>
    /// 用户 ID
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 租户编码
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;
}

/// <summary>
/// 用户登录响应 DTO
/// </summary>
public class TaktLoginResponseDto
{
    /// <summary>
    /// 访问令牌（Access Token）
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// 刷新令牌（Refresh Token）
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// 令牌类型（通常为 Bearer）
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// 过期时间（秒）
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 昵称
    /// </summary>
    public string? NickName { get; set; }

    /// <summary>
    /// 租户编码
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司编码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 用户类型
    /// </summary>
    public int UserType { get; set; }

    /// <summary>
    /// 角色列表
    /// </summary>
    public List<string> Roles { get; set; } = new();

    /// <summary>
    /// 权限列表
    /// </summary>
    public List<string> Permissions { get; set; } = new();

    /// <summary>
    /// 是否需要修改密码（首次登录或密码过期）
    /// </summary>
    public bool MustChangePassword { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// 登录时间
    /// </summary>
    public DateTime LoginAt { get; set; } = DateTime.Now;
}

// ========================================
// 刷新令牌 DTO
// ========================================

/// <summary>
/// 刷新令牌请求 DTO
/// </summary>
public class TaktRefreshTokenRequestDto
{
    /// <summary>
    /// 刷新令牌
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}

// ========================================
// 用户信息 DTO
// ========================================

/// <summary>
/// 用户信息响应 DTO
/// 用于获取当前登录用户详细信息
/// </summary>
public class TaktUserInfoResponseDto
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 昵称
    /// </summary>
    public string? NickName { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    public int UserType { get; set; }

    /// <summary>
    /// 用户类型名称
    /// </summary>
    public string UserTypeName => UserType switch
    {
        0 => "普通用户",
        1 => "管理员",
        2 => "超级管理员",
        _ => "未知"
    };

    /// <summary>
    /// 关联的员工ID
    /// </summary>
    public long EmployeeId { get; set; }

    /// <summary>
    /// 员工姓名（从员工表 name 关联）
    /// </summary>
    public string? EmployeeName { get; set; }

    /// <summary>
    /// 员工性别（0=未知，1=男，2=女）
    /// </summary>
    public int Gender { get; set; }

    /// <summary>
    /// 员工手机号码
    /// </summary>
    public string? Mobile { get; set; }

    /// <summary>
    /// 员工电子邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 用户头像 URL（来自员工档案 avatar）
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// 租户编码
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 租户名称（TaktTenant.TenantName）
    /// </summary>
    public string? TenantName { get; set; }

    /// <summary>
    /// 公司编码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前公司关联工厂（takt_company.related_plant，如 2300→C100）
    /// </summary>
    public string RelatedPlant { get; set; } = string.Empty;

    /// <summary>
    /// 用户区域文化 BCP47（takt_identity_user.culture_code，UI 偏好）
    /// </summary>
    public string DefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 用户区域文化本地化名称（TaktCulture.NativeName，与 DefaultCulture 对应）
    /// </summary>
    public string? CultureName { get; set; }

    /// <summary>
    /// 当前公司区域文化 BCP47（takt_company.culture_code，业务行戳记与界面语言）
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 公司名称（从公司表关联）
    /// </summary>
    public string? CompanyName { get; set; }

    /// <summary>
    /// 当前公司下所属部门显示名（TaktEmployeeDept → TaktDept；多部门顿号拼接）
    /// </summary>
    public string? DeptName { get; set; }

    /// <summary>
    /// 当前公司下所属部门编码列表（供前端 org.dept.* 翻译；与 DeptName 同序）
    /// </summary>
    public List<string> DeptCodes { get; set; } = new();

    /// <summary>
    /// 状态（1=启用，0=禁用）
    /// </summary>
    public int UserStatus { get; set; }

    /// <summary>
    /// 状态名称
    /// </summary>
    public string StatusName => UserStatus == 1 ? "启用" : "禁用";

    /// <summary>
    /// 角色编码列表（鉴权 / 前端权限；与 RoleNames 同序）
    /// </summary>
    public List<string> Roles { get; set; } = new();

    /// <summary>
    /// 角色显示名列表（TaktRole.RoleName；与 Roles 同序，个人中心展示）
    /// </summary>
    public List<string> RoleNames { get; set; } = new();

    /// <summary>
    /// 权限列表
    /// </summary>
    public List<string> Permissions { get; set; } = new();

    /// <summary>
    /// 可访问的菜单树（目录与菜单）
    /// </summary>
    public List<TaktMenuTreeDto> Menus { get; set; } = new();

    /// <summary>
    /// 可访问的前端路由路径列表
    /// </summary>
    public List<string> RoutePaths { get; set; } = new();

    /// <summary>
    /// 可访问的公司列表
    /// </summary>
    public List<string> AccessibleCompanies { get; set; } = new();

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// 最后登录IP
    /// </summary>
    public string? LastLoginIp { get; set; }

    /// <summary>
    /// 登录次数
    /// </summary>
    public int LoginCount { get; set; }

    /// <summary>
    /// 密码过期天数（0=永不过期）
    /// </summary>
    public int PasswordExpireDays { get; set; }

    /// <summary>
    /// 密码是否即将过期（7天内）
    /// </summary>
    public bool IsPasswordExpiringSoon { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// 登录前预览：用户默认公司、用户 DefaultCulture 与假日主题（与 TaktUser / TaktUserCompany 对齐）
/// </summary>
public class TaktLoginPreviewLocaleDto
{
    /// <summary>
    /// 租户在 TaktTenant 中存在且启用
    /// </summary>
    public bool TenantFound { get; set; }

    /// <summary>
    /// 用户在 TaktUser 中存在且启用
    /// </summary>
    public bool UserFound { get; set; }

    /// <summary>
    /// 已解析到 is_default=Yes 的 TaktUserCompany 且对应 TaktCompany 启用
    /// </summary>
    public bool DefaultCompanyFound { get; set; }

    /// <summary>
    /// 用户默认登录公司代码（takt_identity_user_company.is_default=Yes）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 用户区域文化 BCP47（takt_identity_user.default_culture，用于界面语言）
    /// </summary>
    public string DefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 公司区域文化 BCP47（takt_company.default_culture，用于业务数据 CRUD 语言校验）
    /// </summary>
    public string CompanyDefaultCulture { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; }
}

/// <summary>
/// 登录凭据校验结果状态
/// </summary>
public enum TaktLoginCredentialStatus
{
    /// <summary>
    /// 校验通过
    /// </summary>
    Success = 0,

    /// <summary>
    /// 用户名或密码错误（含无租户权限、账号停用、用户不存在）
    /// </summary>
    InvalidCredentials = 1,

    /// <summary>
    /// 账号因连续失败已锁定
    /// </summary>
    AccountLocked = 2,
}

/// <summary>
/// 登录凭据校验结果
/// </summary>
public class TaktLoginCredentialResult
{
    /// <summary>
    /// 校验状态
    /// </summary>
    public TaktLoginCredentialStatus Status { get; set; }

    /// <summary>
    /// 用户 ID（仅 Success 时有值）
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// 锁定截止时间（仅 AccountLocked 时有值）
    /// </summary>
    public DateTime? LockedUntil { get; set; }
}

/// <summary>
/// 登录前预览对外响应（不含 userFound/tenantFound，防用户名枚举）
/// </summary>
public class TaktLoginPreviewLocaleResponseDto
{
    /// <summary>
    /// 用户默认登录公司代码（takt_identity_user_company.is_default）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;
}
