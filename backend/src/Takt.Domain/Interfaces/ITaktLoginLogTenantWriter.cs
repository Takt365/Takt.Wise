// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktLoginLogTenantWriter.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：按指定租户库写入登录日志（认证流程专用；租户库硬隔离，库内 TenantCode 恒等于连接租户）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Statistics.Logging;

namespace Takt.Domain.Interfaces;

/// <summary>
/// 登录日志租户库写入器（显式连接 ConnectionStrings:Tenant_{code}，供匿名认证流程使用）
/// </summary>
public interface ITaktLoginLogTenantWriter
{
    /// <summary>
    /// 在指定租户业务库中插入登录日志
    /// </summary>
    /// <param name="tenantCode">租户编码（须与 ConnectionStrings:Tenant_{code} 一致）</param>
    /// <param name="entity">登录日志实体（TenantCode/CompanyCode 由调用方或本方法写入）</param>
    /// <param name="operatorUserId">操作人 ID；无效时由审计 Helper 写入 SystemAuditUser.Id</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>新记录主键 ID</returns>
    Task<long> CreateInTenantAsync(
        string tenantCode,
        TaktLoginLog entity,
        long? operatorUserId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 登出时回填未关闭登录会话的 LogoutAt
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码（为空时按最近未关闭会话推断）</param>
    /// <param name="UserName">用户名（小写）</param>
    /// <param name="logoutAt">登出时间</param>
    /// <param name="operatorUserId">操作人 ID；无效时按各条 CreatedBy 或 SystemAuditUser.Id</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>更新的记录数</returns>
    Task<int> CloseOpenLoginSessionAsync(
        string tenantCode,
        string? companyCode,
        string UserName,
        DateTime logoutAt,
        long? operatorUserId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 在指定租户库中解析用户默认登录公司（TaktUserCompany.is_default=Yes 且 TaktCompany 启用）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="userId">用户 ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>公司编码；无默认关联时返回 null</returns>
    Task<string?> ResolveUserDefaultCompanyCodeAsync(
        string tenantCode,
        long userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 在指定租户库按用户 ID 解析登录名（小写）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="userId">用户 ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>用户名；不存在时返回 null</returns>
    Task<string?> ResolveUserNameByUserIdAsync(
        string tenantCode,
        long userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 是否已配置指定租户业务库连接（ConnectionStrings:Tenant_{code}）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>已配置为 true</returns>
    bool IsTenantDatabaseConfigured(string tenantCode);
}
