// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Schema
// 文件名称：TaktDatabaseCloneSqlHelper.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：数据库克隆 SQL 工具（标识符校验、租户连接解析、SqlSugar 客户端）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Code;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Schema;

/// <summary>
/// 数据库克隆 SQL 工具
/// </summary>
internal static class TaktDatabaseCloneSqlHelper
{
    internal const int MaxCloneRowCount = 50000;
    internal const int BatchSize = 1000;
    internal const string TenantCodeColumn = "tenant_code";
    internal const string CompanyCodeColumn = "company_code";

    private static readonly Regex TableNamePattern = new(@"^takt_[a-z0-9_]+$", RegexOptions.Compiled);
    private static readonly Regex ColumnNamePattern = new(@"^[a-z][a-z0-9_]*$", RegexOptions.Compiled);
    private static readonly Regex DatabaseNamePattern = new(@"^[A-Za-z][A-Za-z0-9_]*$", RegexOptions.Compiled);
    private static readonly Regex TenantCodeValuePattern = new(@"^[A-Za-z0-9]{3}$", RegexOptions.Compiled);
    private static readonly Regex CompanyCodeValuePattern = new(@"^[A-Za-z0-9]{4}$", RegexOptions.Compiled);

    /// <summary>
    /// 解析并校验租户与数据库展示名
    /// </summary>
    internal static (string TenantCode, string ConnectionString, string DatabaseName) ResolveTenantDatabase(
        IConfiguration configuration,
        string tenantCode,
        string databaseName)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ValidateTenantCode(tenantCode);
        ValidateDatabaseName(databaseName);
        var normalizedTenantCode = tenantCode.Trim();
        var normalizedDatabaseName = databaseName.Trim();
        var match = configuration.GetTenantConnections()
            .FirstOrDefault(x => string.Equals(x.TenantCode, normalizedTenantCode, StringComparison.OrdinalIgnoreCase));
        if (string.IsNullOrWhiteSpace(match.ConnectionString))
        {
            throw new InvalidOperationException($"未找到 TenantCode={normalizedTenantCode} 对应的租户连接字符串");
        }
        var resolvedDatabaseName = ExtractDatabaseDisplayName(match.ConnectionString, normalizedTenantCode);
        if (!string.Equals(resolvedDatabaseName, normalizedDatabaseName, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"租户 {normalizedTenantCode} 对应的数据库名为 {resolvedDatabaseName}，与指定的 {normalizedDatabaseName} 不一致");
        }
        return (match.TenantCode, match.ConnectionString, resolvedDatabaseName);
    }

    /// <summary>
    /// 创建 SqlSugar 客户端
    /// </summary>
    /// <param name="sugarDbType">已解析的 SqlSugar 数据库类型</param>
    /// <param name="connectionString">连接字符串</param>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>SqlSugar 客户端</returns>
    internal static SqlSugarClient CreateClient(DbType sugarDbType, string connectionString, string tenantCode) =>
        TaktSqlSugarConnectionHelper.CreateClient(sugarDbType, tenantCode, connectionString);

    /// <summary>
    /// 判断两个租户连接是否指向同一 SQL Server 实例
    /// </summary>
    internal static bool IsSameServer(string sourceConnectionString, string targetConnectionString)
    {
        return string.Equals(
            ExtractDataSource(sourceConnectionString),
            ExtractDataSource(targetConnectionString),
            StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 限定表名为 [db].[dbo].[table]
    /// </summary>
    internal static string QualifyTable(string databaseName, string tableName)
    {
        return $"{BracketDatabase(databaseName)}.[dbo].{BracketTable(tableName)}";
    }

    /// <summary>
    /// 包装 SQL Server 列标识符
    /// </summary>
    internal static string BracketColumn(string identifier)
    {
        ValidateColumnName(identifier);
        return $"[{identifier}]";
    }

    /// <summary>
    /// 包装 SQL Server 表标识符
    /// </summary>
    internal static string BracketTable(string identifier)
    {
        ValidateTableName(identifier);
        return $"[{identifier}]";
    }

    /// <summary>
    /// 包装 SQL Server 数据库标识符
    /// </summary>
    internal static string BracketDatabase(string identifier)
    {
        ValidateDatabaseName(identifier);
        return $"[{identifier}]";
    }

    /// <summary>
    /// 校验租户编码
    /// </summary>
    internal static void ValidateTenantCode(string tenantCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        if (!TenantCodeValuePattern.IsMatch(tenantCode.Trim()))
        {
            throw new ArgumentException($"非法租户编码: {tenantCode}");
        }
    }

    /// <summary>
    /// 校验公司编码
    /// </summary>
    internal static void ValidateCompanyCode(string companyCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (!CompanyCodeValuePattern.IsMatch(companyCode.Trim()))
        {
            throw new ArgumentException($"非法公司编码: {companyCode}");
        }
    }

    /// <summary>
    /// 校验物理表名
    /// </summary>
    internal static void ValidateTableName(string tableName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
        if (!TableNamePattern.IsMatch(tableName.Trim()))
        {
            throw new ArgumentException($"非法表名: {tableName}");
        }
    }

    /// <summary>
    /// 校验数据库名
    /// </summary>
    internal static void ValidateDatabaseName(string databaseName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);
        if (!DatabaseNamePattern.IsMatch(databaseName.Trim()))
        {
            throw new ArgumentException($"非法数据库名: {databaseName}");
        }
    }

    /// <summary>
    /// 校验列名
    /// </summary>
    internal static void ValidateColumnName(string columnName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(columnName);
        if (!ColumnNamePattern.IsMatch(columnName.Trim()))
        {
            throw new ArgumentException($"非法列名: {columnName}");
        }
    }

    /// <summary>
    /// 从连接字符串提取 Database= 段
    /// </summary>
    internal static string ExtractDatabaseDisplayName(string connectionString, string tenantCode)
    {
        const string key = "Database=";
        var idx = connectionString.IndexOf(key, StringComparison.OrdinalIgnoreCase);
        if (idx < 0)
        {
            return $"Takt_{tenantCode}";
        }
        var start = idx + key.Length;
        var end = connectionString.IndexOf(';', start);
        var dbName = end > start
            ? connectionString[start..end]
            : connectionString[start..];
        return string.IsNullOrWhiteSpace(dbName) ? $"Takt_{tenantCode}" : dbName;
    }

    /// <summary>
    /// 从连接字符串提取 Data Source / Server
    /// </summary>
    internal static string ExtractDataSource(string connectionString)
    {
        foreach (var key in new[] { "Data Source=", "Server=" })
        {
            var idx = connectionString.IndexOf(key, StringComparison.OrdinalIgnoreCase);
            if (idx < 0)
            {
                continue;
            }
            var start = idx + key.Length;
            var end = connectionString.IndexOf(';', start);
            var value = end > start
                ? connectionString[start..end]
                : connectionString[start..];
            return value.Trim();
        }
        return string.Empty;
    }

    /// <summary>
    /// 生成备份表名（takt_xxx_bak_yyyyMMddHHmmss）
    /// </summary>
    internal static string BuildBackupTableName(string targetTableName)
    {
        ValidateTableName(targetTableName);
        var backupName = $"{targetTableName.Trim()}_bak_{DateTime.UtcNow:yyyyMMddHHmmss}";
        if (backupName.Length > 128)
        {
            throw new InvalidOperationException($"备份表名过长: {backupName}");
        }
        ValidateTableName(backupName);
        return backupName;
    }

    /// <summary>
    /// 统计整表行数
    /// </summary>
    internal static Task<int> CountTableRowsAsync(SqlSugarClient db, string tableName)
    {
        var sql = $"SELECT COUNT(1) FROM {BracketTable(tableName)}";
        return db.Ado.GetIntAsync(sql);
    }

    /// <summary>
    /// 备份目标整表数据到备份表（SELECT INTO）
    /// </summary>
    internal static async Task<TaktCloneTargetBackupStepResult> BackupFullTableAsync(
        SqlSugarClient db,
        string targetTableName)
    {
        ValidateTableName(targetTableName);
        var rowCount = await CountTableRowsAsync(db, targetTableName).ConfigureAwait(false);
        var backupTableName = BuildBackupTableName(targetTableName);
        var sql = rowCount > 0
            ? $"SELECT * INTO {BracketTable(backupTableName)} FROM {BracketTable(targetTableName)}"
            : $"SELECT TOP 0 * INTO {BracketTable(backupTableName)} FROM {BracketTable(targetTableName)}";
        await db.Ado.ExecuteCommandAsync(sql).ConfigureAwait(false);
        return new TaktCloneTargetBackupStepResult
        {
            BackupTableName = backupTableName,
            BackedUpRowCount = rowCount,
            SummaryMessage = rowCount > 0
                ? $"已备份目标表 {targetTableName} 全部 {rowCount} 行到 {backupTableName}"
                : $"目标表 {targetTableName} 无数据，已创建空结构备份表 {backupTableName}"
        };
    }

    /// <summary>
    /// TRUNCATE 清空目标整表
    /// </summary>
    internal static async Task<int> TruncateTableAsync(SqlSugarClient db, string targetTableName)
    {
        var rowCount = await CountTableRowsAsync(db, targetTableName).ConfigureAwait(false);
        await db.Ado.ExecuteCommandAsync($"TRUNCATE TABLE {BracketTable(targetTableName)}").ConfigureAwait(false);
        return rowCount;
    }

    /// <summary>
    /// 构建公司范围 WHERE 子句
    /// </summary>
    internal static string BuildCompanyWhereClause(bool hasTenantCodeColumn)
    {
        var conditions = new List<string>
        {
            $"{BracketColumn(CompanyCodeColumn)} = @TargetCompanyCode"
        };
        if (hasTenantCodeColumn)
        {
            conditions.Add($"{BracketColumn(TenantCodeColumn)} = @TargetTenantCode");
        }
        return "WHERE " + string.Join(" AND ", conditions);
    }

    /// <summary>
    /// 构建公司范围 SQL 参数
    /// </summary>
    internal static List<SugarParameter> BuildCompanyParameters(string targetTenantCode, string targetCompanyCode)
    {
        return new List<SugarParameter>
        {
            new("@TargetTenantCode", targetTenantCode),
            new("@TargetCompanyCode", targetCompanyCode)
        };
    }

    /// <summary>
    /// 统计目标公司行数
    /// </summary>
    internal static Task<int> CountCompanyRowsAsync(
        SqlSugarClient db,
        string tableName,
        string targetTenantCode,
        string targetCompanyCode,
        bool hasTenantCodeColumn)
    {
        var whereClause = BuildCompanyWhereClause(hasTenantCodeColumn);
        var sql = $"""
            SELECT COUNT(1)
            FROM {BracketTable(tableName)}
            {whereClause}
            """;
        return db.Ado.GetIntAsync(sql, BuildCompanyParameters(targetTenantCode, targetCompanyCode));
    }

    /// <summary>
    /// 备份目标公司数据到备份表
    /// </summary>
    internal static async Task<TaktCloneTargetBackupStepResult> BackupCompanyRowsAsync(
        SqlSugarClient db,
        string targetTableName,
        string targetTenantCode,
        string targetCompanyCode,
        bool hasTenantCodeColumn)
    {
        ValidateTableName(targetTableName);
        var rowCount = await CountCompanyRowsAsync(
            db,
            targetTableName,
            targetTenantCode,
            targetCompanyCode,
            hasTenantCodeColumn).ConfigureAwait(false);
        var backupTableName = BuildBackupTableName(targetTableName);
        var whereClause = BuildCompanyWhereClause(hasTenantCodeColumn);
        var parameters = BuildCompanyParameters(targetTenantCode, targetCompanyCode);
        var sql = rowCount > 0
            ? $"""
                SELECT * INTO {BracketTable(backupTableName)}
                FROM {BracketTable(targetTableName)}
                {whereClause}
                """
            : $"SELECT TOP 0 * INTO {BracketTable(backupTableName)} FROM {BracketTable(targetTableName)}";
        await db.Ado.ExecuteCommandAsync(sql, rowCount > 0 ? parameters : null).ConfigureAwait(false);
        return new TaktCloneTargetBackupStepResult
        {
            BackupTableName = backupTableName,
            BackedUpRowCount = rowCount,
            SummaryMessage = rowCount > 0
                ? $"已备份目标表 {targetTableName} 中公司 {targetCompanyCode} 的 {rowCount} 行到 {backupTableName}"
                : $"目标表 {targetTableName} 中公司 {targetCompanyCode} 无数据，已创建空结构备份表 {backupTableName}"
        };
    }

    /// <summary>
    /// 删除目标公司数据
    /// </summary>
    internal static async Task<int> DeleteCompanyRowsAsync(
        SqlSugarClient db,
        string targetTableName,
        string targetTenantCode,
        string targetCompanyCode,
        bool hasTenantCodeColumn)
    {
        var rowCount = await CountCompanyRowsAsync(
            db,
            targetTableName,
            targetTenantCode,
            targetCompanyCode,
            hasTenantCodeColumn).ConfigureAwait(false);
        if (rowCount == 0)
        {
            return 0;
        }
        var whereClause = BuildCompanyWhereClause(hasTenantCodeColumn);
        var sql = $"""
            DELETE FROM {BracketTable(targetTableName)}
            {whereClause}
            """;
        await db.Ado.ExecuteCommandAsync(sql, BuildCompanyParameters(targetTenantCode, targetCompanyCode)).ConfigureAwait(false);
        return rowCount;
    }

    /// <summary>
    /// 构建跨租户整表克隆备份预览文案
    /// </summary>
    internal static TaktCloneTargetBackupPreview BuildFullTableBackupPreview(string targetTableName, int targetRowCount)
    {
        var plannedBackupTableName = BuildBackupTableName(targetTableName);
        return new TaktCloneTargetBackupPreview
        {
            TargetTableName = targetTableName,
            TargetRowCount = targetRowCount,
            PlannedBackupTableName = plannedBackupTableName,
            BackupDescription = targetRowCount > 0
                ? $"步骤 1：将目标表 {targetTableName} 的全部 {targetRowCount} 行备份到 {plannedBackupTableName}"
                : $"步骤 1：目标表 {targetTableName} 当前无数据，将创建空结构备份表 {plannedBackupTableName}",
            ClearDescription = $"步骤 2：TRUNCATE 清空目标表 {targetTableName} 中的全部数据（共 {targetRowCount} 行）",
            WarningMessage = $"警告：克隆前将先备份再清空目标表 {targetTableName} 的全部数据，此操作不可撤销，请确认后继续。"
        };
    }

    /// <summary>
    /// 构建公司级克隆备份预览文案
    /// </summary>
    internal static TaktCloneTargetBackupPreview BuildCompanyBackupPreview(
        string targetTableName,
        string targetCompanyCode,
        int targetRowCount)
    {
        var plannedBackupTableName = BuildBackupTableName(targetTableName);
        return new TaktCloneTargetBackupPreview
        {
            TargetTableName = targetTableName,
            TargetCompanyCode = targetCompanyCode,
            TargetRowCount = targetRowCount,
            PlannedBackupTableName = plannedBackupTableName,
            BackupDescription = targetRowCount > 0
                ? $"步骤 1：将目标表 {targetTableName} 中公司 {targetCompanyCode} 的全部 {targetRowCount} 行备份到 {plannedBackupTableName}"
                : $"步骤 1：目标表 {targetTableName} 中公司 {targetCompanyCode} 当前无数据，将创建空结构备份表 {plannedBackupTableName}",
            ClearDescription = $"步骤 2：删除目标表 {targetTableName} 中公司 {targetCompanyCode} 的全部数据（共 {targetRowCount} 行）",
            WarningMessage = $"警告：克隆前将先备份再清空目标表 {targetTableName} 中公司 {targetCompanyCode} 的数据，此操作不可撤销，请确认后继续。"
        };
    }
}
