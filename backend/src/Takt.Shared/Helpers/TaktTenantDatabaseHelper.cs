// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktTenantDatabaseHelper.cs
// 创建时间：2026-06-06
// 创建人：Takt365(Cursor AI)
// 功能描述：租户业务库连接/缺库/缺表等 SqlSugar 基础设施失败识别与业务异常构造
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using SqlSugar;
using Takt.Shared.Exceptions;
using Takt.Shared.Options;

namespace Takt.Shared.Helpers;

/// <summary>
/// 租户业务库基础设施失败（缺库、缺表、连库失败）识别与 TaktBusinessException 构造
/// </summary>
public static class TaktTenantDatabaseHelper
{
    /// <summary>
    /// 租户库基础设施失败类型
    /// </summary>
    public enum TenantDatabaseFailureKind
    {
        /// <summary>
        /// 业务库不存在
        /// </summary>
        DatabaseMissing,
        /// <summary>
        /// 业务表缺失
        /// </summary>
        TableMissing,
        /// <summary>
        /// SQL Server 登录失败
        /// </summary>
        LoginFailed,
        /// <summary>
        /// 其他连接失败
        /// </summary>
        ConnectionFailed,
    }

    /// <summary>
    /// 判断异常是否为 SqlSugar 连库/缺表类基础设施错误（不含表达式翻译等 ORM 逻辑错误）
    /// </summary>
    /// <param name="ex">捕获的异常</param>
    /// <returns>基础设施错误为 true</returns>
    public static bool IsInfrastructureFailure(Exception ex)
    {
        ArgumentNullException.ThrowIfNull(ex);
        var hasSqlSugar = false;
        for (var current = ex; current != null; current = current.InnerException)
        {
            if (current is SqlSugarException)
            {
                hasSqlSugar = true;
                break;
            }
        }
        if (!hasSqlSugar)
        {
            return false;
        }

        var text = CollectExceptionText(ex);
        if (IsOrmExpressionOrLogicError(text))
        {
            return false;
        }

        var kind = ClassifyFailure(ex);
        if (kind is TenantDatabaseFailureKind.DatabaseMissing
            or TenantDatabaseFailureKind.TableMissing
            or TenantDatabaseFailureKind.LoginFailed)
        {
            return true;
        }

        return LooksLikeConnectionFailure(text);
    }

    /// <summary>
    /// SqlSugar 表达式/导航等逻辑错误（非缺库缺表）
    /// </summary>
    /// <param name="text">异常链文本</param>
    /// <returns>是否为 ORM 逻辑错误</returns>
    private static bool IsOrmExpressionOrLogicError(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }
        return text.Contains("不支持", StringComparison.Ordinal)
            || text.Contains("no support", StringComparison.OrdinalIgnoreCase)
            || text.Contains("Includes()", StringComparison.OrdinalIgnoreCase)
            || text.Contains("导航", StringComparison.Ordinal)
            || text.Contains("表达式", StringComparison.Ordinal)
            || text.Contains("Check if the navigation", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 文本是否像真实连库失败（超时、网络、无法连接等）
    /// </summary>
    /// <param name="text">异常链文本</param>
    /// <returns>是否像连库失败</returns>
    private static bool LooksLikeConnectionFailure(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }
        return text.Contains("无法打开登录所请求的数据库", StringComparison.Ordinal)
            || text.Contains("Cannot open database", StringComparison.OrdinalIgnoreCase)
            || text.Contains("无法连接", StringComparison.Ordinal)
            || text.Contains("A network-related", StringComparison.OrdinalIgnoreCase)
            || text.Contains("error occurred while establishing a connection", StringComparison.OrdinalIgnoreCase)
            || text.Contains("Connection refused", StringComparison.OrdinalIgnoreCase)
            || text.Contains("timeout", StringComparison.OrdinalIgnoreCase)
            || text.Contains("超时", StringComparison.Ordinal)
            || text.Contains("登录失败", StringComparison.Ordinal)
            || text.Contains("Login failed", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 根据 SqlSugar 异常文本分类库/表/登录失败
    /// </summary>
    /// <param name="ex">SqlSugar 或包装异常</param>
    /// <returns>失败类型</returns>
    public static TenantDatabaseFailureKind ClassifyFailure(Exception ex)
    {
        ArgumentNullException.ThrowIfNull(ex);
        var text = CollectExceptionText(ex);
        if (text.Contains("无法打开登录所请求的数据库", StringComparison.Ordinal)
            || text.Contains("Cannot open database", StringComparison.OrdinalIgnoreCase))
        {
            return TenantDatabaseFailureKind.DatabaseMissing;
        }

        if (text.Contains("Invalid object name", StringComparison.OrdinalIgnoreCase)
            || text.Contains("对象名", StringComparison.Ordinal)
            || text.Contains("invalid object", StringComparison.OrdinalIgnoreCase))
        {
            return TenantDatabaseFailureKind.TableMissing;
        }

        if (text.Contains("登录失败", StringComparison.Ordinal)
            || text.Contains("Login failed", StringComparison.OrdinalIgnoreCase))
        {
            return TenantDatabaseFailureKind.LoginFailed;
        }

        return TenantDatabaseFailureKind.ConnectionFailed;
    }

    /// <summary>
    /// 解析租户连接串中的数据库名
    /// </summary>
    /// <param name="configuration">应用程序配置</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>数据库展示名</returns>
    public static string ResolveDatabaseName(IConfiguration? configuration, string tenantCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        var trimmed = tenantCode.Trim();
        var connectionString = configuration?.GetConnectionString($"Tenant_{trimmed}");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return $"Takt_{trimmed}_Dev";
        }

        const string key = "Database=";
        var idx = connectionString.IndexOf(key, StringComparison.OrdinalIgnoreCase);
        if (idx < 0)
        {
            return $"Takt_{trimmed}_Dev";
        }

        var start = idx + key.Length;
        var end = connectionString.IndexOf(';', start);
        var dbName = end > start ? connectionString[start..end] : connectionString[start..];
        return string.IsNullOrWhiteSpace(dbName) ? $"Takt_{trimmed}_Dev" : dbName.Trim();
    }

    /// <summary>
    /// 构造租户库未就绪业务异常（含 ErrorCode 与默认中文说明）
    /// </summary>
    /// <param name="ex">原始 SqlSugar 异常</param>
    /// <param name="configuration">应用程序配置</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>业务异常</returns>
    public static TaktBusinessException CreateBusinessException(
        Exception ex,
        IConfiguration? configuration,
        string tenantCode)
    {
        ArgumentNullException.ThrowIfNull(ex);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        var trimmed = tenantCode.Trim();
        var databaseName = ResolveDatabaseName(configuration, trimmed);
        var kind = ClassifyFailure(ex);
        var (errorCode, message) = ResolveError(kind, trimmed, databaseName);
        return new TaktBusinessException(message, errorCode, ex);
    }

    /// <summary>
    /// 解析错误码与默认说明（与 i18n 键 error.tenant.database.* 对齐）
    /// </summary>
    /// <param name="kind">失败类型</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="databaseName">数据库名</param>
    /// <returns>错误码与默认消息</returns>
    public static (string ErrorCode, string Message) ResolveError(
        TenantDatabaseFailureKind kind,
        string tenantCode,
        string databaseName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);
        return kind switch
        {
            TenantDatabaseFailureKind.DatabaseMissing => (
                "error.tenant.database.missing",
                $"租户 {tenantCode} 的业务数据库不存在（库名：{databaseName}）。请在 appsettings 将 Init.InitDb 设为 true 后重启后端建库，或手动创建该数据库。"),
            TenantDatabaseFailureKind.TableMissing => (
                "error.tenant.database.tables.missing",
                $"租户 {tenantCode} 的业务数据库 {databaseName} 已连接，但缺少业务数据表。请将 Init.InitDb 设为 true 后重启建表，并视需要开启 Init.SeedData 写入种子。"),
            TenantDatabaseFailureKind.LoginFailed => (
                "error.tenant.database.login.failed",
                $"租户 {tenantCode} 无法登录 SQL Server（库名：{databaseName}）。请检查 ConnectionStrings:Tenant_{tenantCode} 中的服务器地址与 sa 密码。"),
            _ => (
                "error.tenant.database.connection",
                $"租户 {tenantCode} 的业务数据库无法连接（库名：{databaseName}）。请检查 ConnectionStrings:Tenant_{tenantCode}，或执行 InitDb 建库。"),
        };
    }

    /// <summary>
    /// 校验指定租户业务库可连接（SELECT 1）
    /// </summary>
    /// <param name="configuration">应用程序配置</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>可连接为 true</returns>
    public static bool TryPingTenantDatabase(IConfiguration configuration, string tenantCode)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        var connectionString = configuration.GetConnectionString($"Tenant_{tenantCode.Trim()}");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return false;
        }

        try
        {
            using var db = TaktSqlSugarConnectionHelper.CreateClient(
                configuration.GetSugarDbType(),
                tenantCode.Trim(),
                connectionString,
                registerRepositorySqlFunc: false);
            db.Ado.GetInt("SELECT 1");
            return true;
        }
        catch (Exception ex) when (IsInfrastructureFailure(ex))
        {
            return false;
        }
    }

    /// <summary>
    /// 收集异常链全部 Message 文本
    /// </summary>
    /// <param name="ex">异常</param>
    /// <returns>合并文本</returns>
    private static string CollectExceptionText(Exception ex)
    {
        var parts = new List<string>();
        for (var current = ex; current != null; current = current.InnerException)
        {
            if (!string.IsNullOrWhiteSpace(current.Message))
            {
                parts.Add(current.Message);
            }
        }

        return string.Join(' ', parts);
    }
}
