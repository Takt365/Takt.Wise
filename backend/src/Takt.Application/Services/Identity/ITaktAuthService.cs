// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Identity
// 文件名称：ITaktAuthService.cs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：身份认证服务（登录验密、OAuth Claims、RBAC 权限与当前用户资料）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Identity;
using Takt.Shared.Options;

namespace Takt.Application.Services.Identity;

/// <summary>
/// 身份认证服务接口（登录 + 访问控制，供 TaktAuthsController 与权限过滤器使用）
/// </summary>
public interface ITaktAuthService
{
    #region 登录验密（Cookie 会话 / PKCE 前置）

    /// <summary>
    /// 验证用户登录凭据（租户权限 + 密码）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="UserName">用户名</param>
    /// <param name="password">密码</param>
    /// <returns>用户 ID；验证失败返回 null</returns>
    Task<long?> ValidateUserAsync(string tenantCode, string UserName, string password);

    /// <summary>
    /// 校验用户名在指定租户下是否具备登录权限（最优先，不校验密码）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="UserName">用户名</param>
    /// <returns>是否有权限</returns>
    Task<bool> ValidateUserTenantAccessAsync(string tenantCode, string UserName);

    /// <summary>
    /// 校验租户权限通过后验证密码
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="UserName">用户名</param>
    /// <param name="password">密码</param>
    /// <returns>用户 ID；验证失败返回 null</returns>
    Task<long?> ValidateUserPasswordAsync(string tenantCode, string UserName, string password);

    /// <summary>
    /// 仅校验密码（调用方须已单独通过 ValidateUserTenantAccessAsync）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="UserName">用户名</param>
    /// <param name="password">密码</param>
    /// <returns>用户 ID；密码错误返回 null</returns>
    Task<long?> ValidateUserPasswordOnlyAsync(string tenantCode, string UserName, string password);

    /// <summary>
    /// 登录凭据统一校验（租户权限、锁定、密码；成功时清零失败计数）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="UserName">用户名</param>
    /// <param name="plainPassword">明文密码</param>
    /// <returns>校验结果（含锁定态）</returns>
    Task<TaktLoginCredentialResult> AuthenticateLoginCredentialsAsync(
        string tenantCode,
        string UserName,
        string plainPassword);

    /// <summary>
    /// 解析登录会话租户与公司（用户公司关联 IsDefault + 权限校验）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">登录所选租户</param>
    /// <param name="cultureCode">区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）</param>
    /// <param name="requestedCompanyCode">前端传入的公司编码（可选）</param>
    /// <returns>租户编码与公司编码</returns>
    Task<(string TenantCode, string CompanyCode)> ResolveLoginTenantAndCompanyAsync(
        long userId,
        string tenantCode,
        string? cultureCode,
        string? requestedCompanyCode);

    /// <summary>
    /// 解析用户在指定租户下的默认登录公司代码（仅 takt_identity_user_company.is_default=Yes，无兜底）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="UserName">用户名（仅用于诊断日志）</param>
    /// <returns>公司代码；无 IsDefault 关联或不可访问时返回 null</returns>
    Task<string?> ResolveUserDefaultCompanyCodeAsync(long userId, string tenantCode, string? UserName = null);

    /// <summary>
    /// 解析当前会话生效公司（请求头 X-Company-Code 优先，否则 UserCompany.is_default）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="UserName">用户名（保留供诊断扩展）</param>
    /// <returns>生效公司编码；无可用公司时返回空字符串</returns>
    Task<string> ResolveCurrentActiveCompanyCodeAsync(long userId, string tenantCode, string? UserName = null);

    /// <summary>
    /// 记录用户最后登录时间与 IP（成功登录后由认证日志处理器调用）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="loginIp">登录 IP</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    Task RecordUserLastLoginAsync(
        long userId,
        string tenantCode,
        string? loginIp,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取当前用户可切换的公司选项（已登录；按权限过滤；ExtValue=关联工厂 RelatedPlant）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>公司下拉选项</returns>
    Task<List<TaktSelectOption>> GetUserCompanyOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 获取用户角色编码列表（写入 Token Claims）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>角色编码列表</returns>
    Task<List<string>> GetUserRoleCodesAsync(long userId, string tenantCode);

    /// <summary>
    /// 获取登录页租户选项（登录前；来源配置 TenantCodes，登录后不可跨租户切换）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>TaktSelectOption（DictValue=TenantCode，DictLabel=TenantName，ExtLabel=IsDefault 1/0）</returns>
    Task<List<TaktSelectOption>> GetSessionTenantOptionsAsync(string? plantCode = null, string? keyword = null);

    /// <summary>
    /// 校验登录页输入的租户编码是否存在且启用
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>存在且启用为 true</returns>
    Task<bool> ValidateSessionTenantCodeAsync(string tenantCode);

    /// <summary>
    /// 获取登录页语言切换选项（匿名；未传租户时合并全部配置租户）
    /// </summary>
    /// <param name="tenantCode">租户编码（可选）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>语言下拉选项</returns>
    Task<List<TaktSelectOption>> GetSessionCultureOptionsAsync(string? plantCode = null, string? keyword = null, string? tenantCode = null);

    /// <summary>
    /// 登录前预览：解析用户默认公司、用户 CultureCode 与公司 CultureCode（与假日无关）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="UserName">登录用户名</param>
    /// <returns>公司编码、用户/公司默认语言；解析失败时字段为空</returns>
    Task<TaktLoginPreviewLocaleDto> GetLoginPreviewLocaleAsync(string tenantCode, string UserName);

    #endregion

    #region RBAC 与当前用户

    /// <summary>
    /// 用户是否拥有指定功能权限码
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="permissionCode">权限码</param>
    /// <returns>是否拥有</returns>
    Task<bool> HasUserPermissionAsync(long userId, string tenantCode, string permissionCode);

    /// <summary>
    /// 获取用户功能权限码列表
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>权限码列表</returns>
    Task<List<string>> GetUserPermissionCodesAsync(long userId, string tenantCode);

    /// <summary>
    /// 获取当前登录用户资料（权限、菜单、路由）
    /// </summary>
    /// <returns>用户资料；未登录返回 null</returns>
    Task<TaktUserInfoResponseDto?> GetCurrentUserAsync();

    /// <summary>
    /// 获取用户可访问的前端路由路径列表
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>路由路径列表</returns>
    Task<List<string>> GetUserRoutePathsAsync(long userId, string tenantCode);

    #endregion
}
