// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Health
// 文件名称：TaktTenantDatabaseHealthCheck.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：租户库连通性 HealthCheck（就绪探针 /health/ready）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Takt.Infrastructure.Services;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Health;

/// <summary>
/// 按当前请求租户（或默认租户）Ping 业务库的就绪检查
/// </summary>
public sealed class TaktTenantDatabaseHealthCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// 初始化租户库健康检查
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <param name="httpContextAccessor">HTTP 上下文访问器</param>
    public TaktTenantDatabaseHealthCheck(
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// 健康检查
    /// </summary>
    /// <param name="context">检查上下文</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>检查结果</returns>
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var tenantCode = httpContext != null
            ? TaktUserContext.TryResolveTenantCode(httpContext)?.Trim()
            : null;
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            tenantCode = _configuration["Tenant:DefaultTenantCode"]?.Trim() ?? "000";
        }

        var databaseName = TaktTenantDatabaseHelper.ResolveDatabaseName(_configuration, tenantCode);
        var reachable = TaktTenantDatabaseHelper.TryPingTenantDatabase(_configuration, tenantCode);
        var data = new Dictionary<string, object>
        {
            ["tenantCode"] = tenantCode,
            ["databaseName"] = databaseName,
        };

        if (reachable)
        {
            return Task.FromResult(HealthCheckResult.Healthy("租户数据库可达", data));
        }

        return Task.FromResult(HealthCheckResult.Unhealthy("租户数据库不可达", data: data));
    }
}
