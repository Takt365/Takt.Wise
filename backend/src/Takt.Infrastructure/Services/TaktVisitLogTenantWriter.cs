// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktVisitLogTenantWriter.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：按指定租户库累加 TaktVisitLog.VisitCount（认证成功访问 +1）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Takt.Domain.Entities.Statistics.Logging;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Data.Context;
using Takt.Infrastructure.Extensions;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Services;

/// <summary>
/// ITaktVisitLogTenantWriter 实现
/// </summary>
public class TaktVisitLogTenantWriter : ITaktVisitLogTenantWriter
{
    private readonly IConfiguration _configuration;
    private readonly PrimaryKeyTypeOptions _primaryKeyTypeOptions;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configuration">配置</param>
    /// <param name="primaryKeyTypeOptions">主键类型配置</param>
    public TaktVisitLogTenantWriter(
        IConfiguration configuration,
        IOptions<PrimaryKeyTypeOptions> primaryKeyTypeOptions)
    {
        _configuration = configuration;
        _primaryKeyTypeOptions = primaryKeyTypeOptions.Value;
    }

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
    public async Task IncrementDailyVisitCountAsync(
        string tenantCode,
        string companyCode,
        long userId,
        string UserName,
        DateTime? visitAt = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (userId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(userId), "用户 ID 无效");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(UserName);

        var trimmedTenant = tenantCode.Trim();
        var trimmedCompany = companyCode.Trim();
        var normalizedUserName = UserName.Trim();
        var statDate = (visitAt ?? DateTime.Now).Date;
        var now = DateTime.Now;

        using var seedContext = new TaktSeedContext(_configuration, trimmedTenant);
        var existing = await seedContext.Db.Queryable<TaktVisitLog>()
            .Where(x =>
                x.TenantCode == trimmedTenant
                && x.CompanyCode == trimmedCompany
                && x.UserId == userId
                && x.StatDate == statDate
                && x.IsDeleted == 0)
            .FirstAsync(cancellationToken);
        if (existing == null)
        {
            var entity = new TaktVisitLog
            {
                TenantCode = trimmedTenant,
                CompanyCode = trimmedCompany,
                UserId = userId,
                UserName = normalizedUserName,
                StatDate = statDate,
                VisitCount = 1,
            };
            TaktCompanyScopeFillHelper.ApplyCompanyScopeFromConfiguration(
                entity,
                entity.TenantCode,
                entity.CompanyCode,
                _configuration);
            entity.ApplyCreate(userId, now);

            await TaktPrimaryKeyInsertHelper.InsertEntityAsync(
                seedContext.Db,
                entity,
                _primaryKeyTypeOptions,
                cancellationToken);

            return;
        }

        existing.UserName = normalizedUserName;
        existing.VisitCount = checked(existing.VisitCount + 1);
        existing.ApplyUpdate(userId, now);
        await seedContext.Db.Updateable(existing).ExecuteCommandAsync(cancellationToken);
    }

    /// <summary>
    /// 是否已配置指定租户业务库连接（ConnectionStrings:Tenant_{code}）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>已配置为 true</returns>
    public bool IsTenantDatabaseConfigured(string tenantCode)
    {
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            return false;
        }

        var connectionString = _configuration.GetConnectionString($"Tenant_{tenantCode.Trim()}");
        return !string.IsNullOrWhiteSpace(connectionString);
    }
}
