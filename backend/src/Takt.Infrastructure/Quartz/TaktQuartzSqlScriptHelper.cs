// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Quartz
// 文件名称：TaktQuartzSqlScriptHelper.cs
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：按相对 wwwroot 的 .sql 路径读取 Quartz SQL 并绑定租户/同步库占位符（禁止内联 SQL）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using Takt.Application.Services.Foundation;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Quartz;

/// <summary>
/// Quartz SQL 脚本解析（仅 wwwroot 相对 .sql 路径）
/// </summary>
public static class TaktQuartzSqlScriptHelper
{
    /// <summary>
    /// 源库占位符
    /// </summary>
    public const string SourceDatabasePlaceholder = "{{SourceDatabase}}";

    /// <summary>
    /// 目标库占位符
    /// </summary>
    public const string TargetDatabasePlaceholder = "{{TargetDatabase}}";

    /// <summary>
    /// 核算月份占位符（yyyy-MM；可空，脚本内自行默认当月）
    /// </summary>
    public const string CostingPeriodPlaceholder = "{{CostingPeriod}}";

    /// <summary>
    /// 解析为可执行 SQL：SqlScript 必须为相对 wwwroot 的 .sql 路径，读文件后替换占位符
    /// </summary>
    /// <param name="sqlScript">相对路径，如 Quartz/sync_mat.sql</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="cultureCode">区域文化编码</param>
    /// <param name="plantCode">工厂编码（脚本含 PlantCode 占位时必填；与 Database:CompanyCodes↔PlantCodes 对齐）</param>
    /// <param name="sourceDatabase">源库名（脚本含 SourceDatabase 占位时必填）</param>
    /// <param name="targetDatabase">目标库名（脚本含 TargetDatabase 占位时必填）</param>
    /// <param name="costingPeriod">核算月 yyyy-MM（脚本含 CostingPeriod 占位时替换；可空）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>可执行 SQL 文本</returns>
    /// <exception cref="ArgumentException">路径为空或格式非法、缺库名</exception>
    /// <exception cref="FileNotFoundException">.sql 文件不存在</exception>
    /// <exception cref="InvalidOperationException">路径越界或不安全</exception>
    public static async Task<string> ResolveExecutableSqlAsync(
        string? sqlScript,
        string tenantCode,
        string companyCode,
        string? cultureCode = null,
        string? plantCode = null,
        string? sourceDatabase = null,
        string? targetDatabase = null,
        string? costingPeriod = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sqlScript);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (!TaktQuartzSqlPathHelper.IsValidWwwRootRelativeSqlPath(sqlScript))
        {
            throw new ArgumentException(
                "SqlScript 只可填相对 wwwroot 的 .sql 路径（如 Quartz/sync_mat.sql），不允许填写具体 SQL 语句",
                nameof(sqlScript));
        }
        var body = await File.ReadAllTextAsync(ResolveWwwRootSqlFilePath(sqlScript.Trim()), cancellationToken);
        return BindPlaceholders(
            body,
            tenantCode,
            companyCode,
            cultureCode,
            plantCode,
            sourceDatabase,
            targetDatabase,
            costingPeriod);
    }

    /// <summary>
    /// 是否为相对 wwwroot 的 .sql 文件引用
    /// </summary>
    /// <param name="sqlScript">SqlScript 字段值</param>
    /// <returns>是文件引用则为 true</returns>
    public static bool IsWwwRootSqlFileReference(string sqlScript)
        => TaktQuartzSqlPathHelper.IsValidWwwRootRelativeSqlPath(sqlScript);

    /// <summary>
    /// 将相对 wwwroot 的路径解析为绝对路径（禁止 .. 穿越）
    /// </summary>
    /// <param name="relativePath">相对路径，如 Quartz/sync_mat.sql</param>
    /// <returns>绝对路径</returns>
    public static string ResolveWwwRootSqlFilePath(string relativePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);
        if (!TaktQuartzSqlPathHelper.IsValidWwwRootRelativeSqlPath(relativePath))
        {
            throw new InvalidOperationException($"非法 SQL 脚本路径：{relativePath}");
        }
        var normalized = relativePath.Trim().Replace('\\', '/');
        var wwwroot = Path.GetFullPath(TaktFileHelper.GetWwwRootPath());
        var fullPath = Path.GetFullPath(Path.Combine(wwwroot, normalized.Replace('/', Path.DirectorySeparatorChar)));
        var prefix = wwwroot.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
        if (!fullPath.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(fullPath, wwwroot, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"SQL 脚本路径越界：{relativePath}");
        }
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"未找到 Quartz SQL 脚本：{relativePath}", fullPath);
        }
        return fullPath;
    }

    /// <summary>
    /// 替换 {{TenantCode}} / {{CompanyCode}} / {{CultureCode}} / {{PlantCode}} / {{SyncUserId}} / {{SourceDatabase}} / {{TargetDatabase}} / {{CostingPeriod}}
    /// </summary>
    /// <param name="sqlTemplate">SQL 模板</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="cultureCode">区域文化编码</param>
    /// <param name="plantCode">工厂编码</param>
    /// <param name="sourceDatabase">源库名</param>
    /// <param name="targetDatabase">目标库名</param>
    /// <param name="costingPeriod">核算月 yyyy-MM（可空）</param>
    /// <returns>绑定后的 SQL</returns>
    public static string BindPlaceholders(
        string sqlTemplate,
        string tenantCode,
        string companyCode,
        string? cultureCode = null,
        string? plantCode = null,
        string? sourceDatabase = null,
        string? targetDatabase = null,
        string? costingPeriod = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sqlTemplate);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        var needsSource = sqlTemplate.Contains(SourceDatabasePlaceholder, StringComparison.Ordinal);
        var needsTarget = sqlTemplate.Contains(TargetDatabasePlaceholder, StringComparison.Ordinal);
        var needsPlant = sqlTemplate.Contains("{{PlantCode}}", StringComparison.Ordinal);
        var needsCostingPeriod = sqlTemplate.Contains(CostingPeriodPlaceholder, StringComparison.Ordinal);
        if (needsSource)
        {
            if (string.IsNullOrWhiteSpace(sourceDatabase))
            {
                throw new ArgumentException(
                    "SQL 脚本含 {{SourceDatabase}}，ExecuteParams 须提供 sourceDatabase",
                    nameof(sourceDatabase));
            }
            TaktQuartzSyncExecuteParamsHelper.ValidateDatabaseName(sourceDatabase);
        }
        if (needsTarget)
        {
            if (string.IsNullOrWhiteSpace(targetDatabase))
            {
                throw new ArgumentException(
                    "SQL 脚本含 {{TargetDatabase}}，ExecuteParams 须提供 targetDatabase",
                    nameof(targetDatabase));
            }
            TaktQuartzSyncExecuteParamsHelper.ValidateDatabaseName(targetDatabase);
        }
        if (needsPlant && string.IsNullOrWhiteSpace(plantCode))
        {
            throw new ArgumentException(
                "SQL 脚本含 {{PlantCode}}，须按 Database:CompanyCodes↔PlantCodes 传入 plantCode",
                nameof(plantCode));
        }
        if (!sqlTemplate.Contains("{{TenantCode}}", StringComparison.Ordinal)
            && !sqlTemplate.Contains("{{CompanyCode}}", StringComparison.Ordinal)
            && !sqlTemplate.Contains("{{CultureCode}}", StringComparison.Ordinal)
            && !needsPlant
            && !sqlTemplate.Contains("{{SyncUserId}}", StringComparison.Ordinal)
            && !needsSource
            && !needsTarget
            && !needsCostingPeriod)
        {
            return sqlTemplate;
        }
        static string Esc(string value) => value.Replace("'", "''", StringComparison.Ordinal);
        var effectiveCultureCode = string.IsNullOrWhiteSpace(cultureCode) ? "zh-CN" : cultureCode.Trim();
        var result = sqlTemplate
            .Replace("{{TenantCode}}", Esc(tenantCode.Trim()), StringComparison.Ordinal)
            .Replace("{{CompanyCode}}", Esc(companyCode.Trim()), StringComparison.Ordinal)
            .Replace("{{CultureCode}}", Esc(effectiveCultureCode), StringComparison.Ordinal)
            .Replace(
                "{{SyncUserId}}",
                TaktConstants.SystemAuditUser.Id.ToString(CultureInfo.InvariantCulture),
                StringComparison.Ordinal);
        if (needsPlant)
        {
            result = result.Replace("{{PlantCode}}", Esc(plantCode!.Trim()), StringComparison.Ordinal);
        }
        if (needsSource)
        {
            result = result.Replace(
                SourceDatabasePlaceholder,
                Esc(sourceDatabase!.Trim()),
                StringComparison.Ordinal);
        }
        if (needsTarget)
        {
            result = result.Replace(
                TargetDatabasePlaceholder,
                Esc(targetDatabase!.Trim()),
                StringComparison.Ordinal);
        }
        if (needsCostingPeriod)
        {
            var period = string.IsNullOrWhiteSpace(costingPeriod) ? string.Empty : costingPeriod.Trim();
            result = result.Replace(CostingPeriodPlaceholder, Esc(period), StringComparison.Ordinal);
        }
        return result;
    }
}
