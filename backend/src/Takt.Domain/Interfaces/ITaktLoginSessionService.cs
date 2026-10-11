// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktLoginSessionService.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：登录前会话数据（租户选项、租户内用户存在性校验；不跨库聚合目录）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Options;

namespace Takt.Domain.Interfaces;

/// <summary>
/// 登录前会话服务（匿名登录页使用；登录后租户不可切换）
/// </summary>
public interface ITaktLoginSessionService
{
    /// <summary>
    /// 获取登录页可选租户列表（ConnectionStrings:Tenant_* 对应库中 TaktTenant 启用记录）
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>下拉选项（DictValue=TenantCode）</returns>
    Task<List<TaktSelectOption>> GetLoginTenantOptionsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 校验用户名在指定租户库中是否存在且启用（仅连接该租户对应库，不遍历聚合）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="UserName">用户名</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>可登录为 true</returns>
    Task<bool> HasUserLoginAccessInTenantAsync(
        string tenantCode,
        string UserName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 校验登录页输入的租户编码在 TaktTenant 中是否存在且启用
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>存在且启用为 true</returns>
    Task<bool> ValidateLoginTenantCodeAsync(string tenantCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取登录页语言切换选项（匿名；须指定租户，仅查询该租户库）
    /// </summary>
    /// <param name="tenantCode">租户编码（必填）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>TaktSelectOption（DictValue=CultureCode）</returns>
    Task<List<TaktSelectOption>> GetLoginCultureOptionsAsync(
        string? tenantCode = null,
        CancellationToken cancellationToken = default);
}
