// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktLoginSessionService.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：登录前租户选项与租户内用户校验（TaktTenant / TaktUser 数据表）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Data.Context;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Services;

/// <summary>
/// ITaktLoginSessionService 实现
/// 登录页租户与用户校验均查询对应租户库实体表，不按配置白名单兜底
/// </summary>
public class TaktLoginSessionService : ITaktLoginSessionService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configuration">配置</param>
    public TaktLoginSessionService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// 获取登录页可选租户列表（遍历 ConnectionStrings:Tenant_*，仅返回 TaktTenant 存在且启用的项）
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>下拉选项（DictValue=TenantCode）</returns>
    public async Task<List<TaktSelectOption>> GetLoginTenantOptionsAsync(CancellationToken cancellationToken = default)
    {
        var options = new List<TaktSelectOption>();
        var sortOrder = 0;
        var connectionFailures = 0;
        var configuredCount = 0;

        foreach (var code in GetConfiguredTenantConnectionCodes())
        {
            configuredCount++;
            try
            {
                using var seedContext = CreateSeedContext(code);
                var tenant = await seedContext.Query<TaktTenant>()
                    .Where(t => t.TenantCode == code && t.TenantStatus == 1)
                    .FirstAsync(cancellationToken);
                if (tenant == null)
                {
                    continue;
                }

                var label = string.IsNullOrWhiteSpace(tenant.TenantName)
                    ? code
                    : tenant.TenantName.Trim();

                options.Add(new TaktSelectOption
                {
                    DictValue = code,
                    DictLabel = label,
                    SortOrder = sortOrder++,
                });
            }
            catch (Exception ex) when (TaktTenantDatabaseHelper.IsInfrastructureFailure(ex))
            {
                connectionFailures++;
                TaktLogger.Warning(ex, "读取租户 {TenantCode} 登录选项失败（库/表未就绪）", code);
            }
            catch (Exception ex)
            {
                TaktLogger.Warning(ex, "读取租户 {TenantCode} 实体失败，跳过登录选项", code);
            }
        }

        if (configuredCount > 0 && connectionFailures == configuredCount)
        {
            ThrowTenantDatabaseFailure("全部配置租户", null);
        }

        if (options.Count == 0)
        {
            throw new InvalidOperationException("未在 TaktTenant 表中找到任何启用的租户");
        }

        return options;
    }

    /// <summary>
    /// 校验用户名在指定租户库 TaktUser 中是否存在且启用
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="UserName">用户名</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>存在且启用为 true</returns>
    public async Task<bool> HasUserLoginAccessInTenantAsync(
        string tenantCode,
        string UserName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tenantCode) || string.IsNullOrWhiteSpace(UserName))
        {
            return false;
        }

        var trimmedTenant = tenantCode.Trim();
        var normalizedUserName = UserName.Trim().ToLowerInvariant();

        try
        {
            using var seedContext = CreateSeedContext(trimmedTenant);
            return await seedContext.Query<TaktUser>()
                .AnyAsync(
                    u =>
                        u.TenantCode == trimmedTenant
                        && u.UserName == normalizedUserName
                        && u.UserStatus == 1,
                    cancellationToken);
        }
        catch (Exception ex) when (TaktTenantDatabaseHelper.IsInfrastructureFailure(ex))
        {
            ThrowTenantDatabaseFailure(trimmedTenant, ex);
            throw;
        }
    }

    /// <summary>
    /// 校验登录页输入的租户编码在 TaktTenant 中是否存在且启用
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>存在且启用为 true；库/表缺失时抛出 TaktBusinessException</returns>
    public async Task<bool> ValidateLoginTenantCodeAsync(
        string tenantCode,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            return false;
        }

        var trimmedTenant = tenantCode.Trim();

        try
        {
            using var seedContext = CreateSeedContext(trimmedTenant);
            return await seedContext.Query<TaktTenant>()
                .AnyAsync(
                    t => t.TenantCode == trimmedTenant && t.TenantStatus == 1,
                    cancellationToken);
        }
        catch (Exception ex) when (TaktTenantDatabaseHelper.IsInfrastructureFailure(ex))
        {
            ThrowTenantDatabaseFailure(trimmedTenant, ex);
            throw;
        }
    }

    /// <summary>
    /// 获取登录页语言切换选项（匿名；须指定租户，仅查询该租户库）
    /// </summary>
    /// <param name="tenantCode">租户编码（必填）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetLoginCultureOptionsAsync(
        string? tenantCode = null,
        CancellationToken cancellationToken = default)
    {
        var trimmedTenant = tenantCode?.Trim();
        if (string.IsNullOrWhiteSpace(trimmedTenant))
        {
            throw new ArgumentException("租户编码不能为空", nameof(tenantCode));
        }

        if (!await ValidateLoginTenantCodeAsync(trimmedTenant, cancellationToken))
        {
            return new List<TaktSelectOption>();
        }

        return await LoadCultureOptionsForTenantAsync(trimmedTenant, cancellationToken);
    }

    /// <summary>
    /// 读取指定租户库中启用的区域文化选项
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>语言下拉选项</returns>
    private async Task<List<TaktSelectOption>> LoadCultureOptionsForTenantAsync(
        string tenantCode,
        CancellationToken cancellationToken)
    {
        try
        {
            using var seedContext = CreateSeedContext(tenantCode);
            var list = await seedContext.Query<TaktCulture>()
                .Where(c =>
                    c.TenantCode == tenantCode
                    && c.IsDeleted == 0)
                .OrderBy(c => c.SortOrder)
                .ToListAsync(cancellationToken);

            return list.Select(e => new TaktSelectOption
            {
                DictValue = e.CultureCode,
                DictLabel = e.NativeName,
                ExtValue = e.Icon,
                ExtLabel = ((int)e.IsDefault).ToString(),
                SortOrder = e.SortOrder,
            }).ToList();
        }
        catch (Exception ex) when (TaktTenantDatabaseHelper.IsInfrastructureFailure(ex))
        {
            ThrowTenantDatabaseFailure(tenantCode, ex);
            throw;
        }
    }

    /// <summary>
    /// 创建指定租户的种子上下文
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>种子上下文</returns>
    private TaktSeedContext CreateSeedContext(string tenantCode)
    {
        return new TaktSeedContext(_configuration, tenantCode);
    }

    /// <summary>
    /// 从 ConnectionStrings 读取已配置租户连接编码（Tenant_{code}）
    /// </summary>
    /// <returns>租户编码序列</returns>
    private IEnumerable<string> GetConfiguredTenantConnectionCodes()
    {
        return _configuration.GetTenantCodes();
    }

    /// <summary>
    /// 解析租户连接串中的数据库名
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>数据库名</returns>
    private string ResolveTenantDatabaseName(string tenantCode)
    {
        return TaktTenantDatabaseHelper.ResolveDatabaseName(_configuration, tenantCode);
    }

    /// <summary>
    /// 抛出租户库/表未就绪业务异常（供 API 与前端直接展示）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="innerException">内部异常</param>
    private void ThrowTenantDatabaseFailure(string tenantCode, Exception? innerException)
    {
        var trimmed = tenantCode.Trim();
        if (innerException == null)
        {
            var (errorCode, message) = TaktTenantDatabaseHelper.ResolveError(
                TaktTenantDatabaseHelper.TenantDatabaseFailureKind.ConnectionFailed,
                trimmed,
                ResolveTenantDatabaseName(trimmed));
            throw new TaktBusinessException(message, errorCode);
        }

        throw TaktTenantDatabaseHelper.CreateBusinessException(innerException, _configuration, trimmed);
    }
}

