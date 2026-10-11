// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktVisitLogTenantWriter.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：按指定租户库累加用户日访问量（认证成功时 +1，与在线时长无关）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Domain.Interfaces;

/// <summary>
/// 用户日访问量租户库写入器（显式 ConnectionStrings:Tenant_{code}，供认证流程使用）
/// </summary>
public interface ITaktVisitLogTenantWriter
{
    /// <summary>
    /// 累加指定用户自然日访问次数（VisitCount +1；无行则新建）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="userId">用户 ID</param>
    /// <param name="UserName">用户名</param>
    /// <param name="visitAt">访问时刻；默认当前时间</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    Task IncrementDailyVisitCountAsync(
        string tenantCode,
        string companyCode,
        long userId,
        string UserName,
        DateTime? visitAt = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 是否已配置指定租户业务库连接（ConnectionStrings:Tenant_{code}）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>已配置为 true</returns>
    bool IsTenantDatabaseConfigured(string tenantCode);
}
