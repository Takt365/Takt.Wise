// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Schema
// 文件名称：TaktDataCloneProvider.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：公司级数据克隆（按 company_code 过滤/重写，支持租户内与跨租户）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Data;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Code;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Schema;

/// <summary>
/// 公司级数据克隆提供者
/// </summary>
public class TaktDataCloneProvider : ITaktDataCloneProvider
{
    private readonly IConfiguration _configuration;
    private readonly SqlSugar.DbType _sugarDbType;
    private readonly ITaktDatabaseSchemaProvider _schemaProvider;

    /// <summary>
    /// 初始化公司级数据克隆提供者
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <param name="schemaProvider">Schema 元数据提供者</param>
    public TaktDataCloneProvider(
        IConfiguration configuration,
        ITaktDatabaseSchemaProvider schemaProvider)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _sugarDbType = configuration.GetSugarDbType();
        _schemaProvider = schemaProvider ?? throw new ArgumentNullException(nameof(schemaProvider));
    }

    /// <summary>
    /// 按公司范围克隆数据（支持租户内跨公司、跨租户跨公司）
    /// </summary>
    /// <param name="options">克隆选项</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>克隆结果</returns>
    public async Task<TaktDataCloneResult> CloneDataAsync(
        TaktDataCloneOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        ValidateOptions(options);
        if (IsSameScope(options))
        {
            throw new InvalidOperationException("源与目标租户、公司、数据库、数据表不能完全相同");
        }

        var source = TaktDatabaseCloneSqlHelper.ResolveTenantDatabase(
            _configuration,
            options.SourceTenantCode,
            options.SourceDatabaseName);
        var target = TaktDatabaseCloneSqlHelper.ResolveTenantDatabase(
            _configuration,
            options.TargetTenantCode,
            options.TargetDatabaseName);

        var columnMapping = await BuildColumnMappingAsync(source.TenantCode, target.TenantCode, options).ConfigureAwait(false);
        if (!columnMapping.HasCompanyCodeColumn)
        {
            throw new InvalidOperationException($"表 {options.SourceTableName} 不含 company_code 列，无法按公司克隆");
        }

        using var sourceDb = TaktDatabaseCloneSqlHelper.CreateClient(_sugarDbType, source.ConnectionString, source.TenantCode);
        using var targetDb = TaktDatabaseCloneSqlHelper.CreateClient(_sugarDbType, target.ConnectionString, target.TenantCode);

        var sourceRowCount = await CountScopedRowsAsync(
            sourceDb,
            options.SourceTableName,
            options.SourceTenantCode,
            options.SourceCompanyCode,
            columnMapping.HasTenantCodeColumn,
            cancellationToken).ConfigureAwait(false);
        if (sourceRowCount > TaktDatabaseCloneSqlHelper.MaxCloneRowCount)
        {
            throw new InvalidOperationException(
                $"源公司数据行数 {sourceRowCount} 超过克隆上限 {TaktDatabaseCloneSqlHelper.MaxCloneRowCount}");
        }

        var clonedRowCount = 0;
        var sameServer = TaktDatabaseCloneSqlHelper.IsSameServer(source.ConnectionString, target.ConnectionString);
        await targetDb.Ado.BeginTranAsync().ConfigureAwait(false);
        TaktCloneTargetBackupStepResult backupStep;
        try
        {
            backupStep = await TaktDatabaseCloneSqlHelper.BackupCompanyRowsAsync(
                targetDb,
                options.TargetTableName,
                options.TargetTenantCode.Trim(),
                options.TargetCompanyCode.Trim(),
                columnMapping.HasTenantCodeColumn).ConfigureAwait(false);
            var clearedRowCount = await TaktDatabaseCloneSqlHelper.DeleteCompanyRowsAsync(
                targetDb,
                options.TargetTableName,
                options.TargetTenantCode.Trim(),
                options.TargetCompanyCode.Trim(),
                columnMapping.HasTenantCodeColumn).ConfigureAwait(false);
            backupStep.ClearedRowCount = clearedRowCount;
            backupStep.SummaryMessage =
                $"{backupStep.SummaryMessage}；已删除目标表 {options.TargetTableName} 中公司 {options.TargetCompanyCode.Trim()} 全部 {clearedRowCount} 行";

            if (sourceRowCount == 0)
            {
                await targetDb.Ado.CommitTranAsync().ConfigureAwait(false);
                return BuildResult(columnMapping, sourceRowCount, 0, backupStep);
            }

            if (sameServer)
            {
                clonedRowCount = await CloneByInsertSelectAsync(
                    targetDb,
                    source,
                    target,
                    options,
                    columnMapping).ConfigureAwait(false);
            }
            else
            {
                clonedRowCount = await CloneByBatchAsync(
                    sourceDb,
                    targetDb,
                    options,
                    columnMapping,
                    sourceRowCount,
                    cancellationToken).ConfigureAwait(false);
            }

            await targetDb.Ado.CommitTranAsync().ConfigureAwait(false);
        }
        catch
        {
            await targetDb.Ado.RollbackTranAsync().ConfigureAwait(false);
            throw;
        }

        TaktLogger.Information(
            "[DataClone] 克隆完成: {SourceTenant}/{SourceCompany}/{SourceTable} -> {TargetTenant}/{TargetCompany}/{TargetTable}, Rows={ClonedRowCount}",
            source.TenantCode,
            options.SourceCompanyCode,
            options.SourceTableName,
            target.TenantCode,
            options.TargetCompanyCode,
            options.TargetTableName,
            clonedRowCount);

        return BuildResult(columnMapping, sourceRowCount, clonedRowCount, backupStep);
    }

    /// <summary>
    /// 获取目标公司数据备份预览
    /// </summary>
    public async Task<TaktCloneTargetBackupPreview> GetTargetBackupPreviewAsync(
        TaktDataCloneOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        TaktDatabaseCloneSqlHelper.ValidateTenantCode(options.TargetTenantCode);
        TaktDatabaseCloneSqlHelper.ValidateCompanyCode(options.TargetCompanyCode);
        TaktDatabaseCloneSqlHelper.ValidateDatabaseName(options.TargetDatabaseName);
        TaktDatabaseCloneSqlHelper.ValidateTableName(options.TargetTableName);
        var target = TaktDatabaseCloneSqlHelper.ResolveTenantDatabase(
            _configuration,
            options.TargetTenantCode,
            options.TargetDatabaseName);
        var targetColumns = await _schemaProvider.GetColumnsAsync(target.TenantCode, options.TargetTableName.Trim())
            .ConfigureAwait(false);
        if (targetColumns.Count == 0)
        {
            throw new InvalidOperationException($"未找到目标表 {options.TargetTableName} 的列信息");
        }
        var hasTenantCodeColumn = targetColumns.Any(c =>
            string.Equals(c.DatabaseColumnName, TaktDatabaseCloneSqlHelper.TenantCodeColumn, StringComparison.OrdinalIgnoreCase));
        var hasCompanyCodeColumn = targetColumns.Any(c =>
            string.Equals(c.DatabaseColumnName, TaktDatabaseCloneSqlHelper.CompanyCodeColumn, StringComparison.OrdinalIgnoreCase));
        if (!hasCompanyCodeColumn)
        {
            throw new InvalidOperationException($"表 {options.TargetTableName} 不含 company_code 列，无法按公司克隆");
        }
        using var targetDb = TaktDatabaseCloneSqlHelper.CreateClient(_sugarDbType, target.ConnectionString, target.TenantCode);
        cancellationToken.ThrowIfCancellationRequested();
        var rowCount = await TaktDatabaseCloneSqlHelper.CountCompanyRowsAsync(
            targetDb,
            options.TargetTableName.Trim(),
            options.TargetTenantCode.Trim(),
            options.TargetCompanyCode.Trim(),
            hasTenantCodeColumn).ConfigureAwait(false);
        return TaktDatabaseCloneSqlHelper.BuildCompanyBackupPreview(
            options.TargetTableName.Trim(),
            options.TargetCompanyCode.Trim(),
            rowCount);
    }

    /// <summary>
    /// 校验克隆选项
    /// </summary>
    private static void ValidateOptions(TaktDataCloneOptions options)
    {
        TaktDatabaseCloneSqlHelper.ValidateTenantCode(options.SourceTenantCode);
        TaktDatabaseCloneSqlHelper.ValidateTenantCode(options.TargetTenantCode);
        TaktDatabaseCloneSqlHelper.ValidateCompanyCode(options.SourceCompanyCode);
        TaktDatabaseCloneSqlHelper.ValidateCompanyCode(options.TargetCompanyCode);
        TaktDatabaseCloneSqlHelper.ValidateDatabaseName(options.SourceDatabaseName);
        TaktDatabaseCloneSqlHelper.ValidateDatabaseName(options.TargetDatabaseName);
        TaktDatabaseCloneSqlHelper.ValidateTableName(options.SourceTableName);
        TaktDatabaseCloneSqlHelper.ValidateTableName(options.TargetTableName);
    }

    /// <summary>
    /// 构建列映射上下文
    /// </summary>
    private async Task<DataCloneColumnMappingContext> BuildColumnMappingAsync(
        string sourceTenantCode,
        string targetTenantCode,
        TaktDataCloneOptions options)
    {
        var sourceColumns = await _schemaProvider.GetColumnsAsync(sourceTenantCode, options.SourceTableName).ConfigureAwait(false);
        var targetColumns = await _schemaProvider.GetColumnsAsync(targetTenantCode, options.TargetTableName).ConfigureAwait(false);
        if (sourceColumns.Count == 0)
        {
            throw new InvalidOperationException($"未找到源表 {options.SourceTableName} 的列信息");
        }
        if (targetColumns.Count == 0)
        {
            throw new InvalidOperationException($"未找到目标表 {options.TargetTableName} 的列信息");
        }

        var targetColumnMap = targetColumns
            .GroupBy(c => c.DatabaseColumnName, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);
        var commonColumns = sourceColumns
            .Where(c => targetColumnMap.ContainsKey(c.DatabaseColumnName))
            .Select(c => c.DatabaseColumnName)
            .ToList();
        if (commonColumns.Count == 0)
        {
            throw new InvalidOperationException("源表与目标表没有可映射的同名列");
        }

        return new DataCloneColumnMappingContext
        {
            CommonColumns = commonColumns,
            SkippedSourceColumns = sourceColumns
                .Select(c => c.DatabaseColumnName)
                .Where(name => !targetColumnMap.ContainsKey(name))
                .ToList(),
            SkippedTargetColumns = targetColumns
                .Select(c => c.DatabaseColumnName)
                .Where(name => !commonColumns.Contains(name, StringComparer.OrdinalIgnoreCase))
                .ToList(),
            HasIdentity = sourceColumns.Any(c =>
                c.IsIdentity && commonColumns.Contains(c.DatabaseColumnName, StringComparer.OrdinalIgnoreCase)),
            HasTenantCodeColumn = commonColumns.Contains(TaktDatabaseCloneSqlHelper.TenantCodeColumn, StringComparer.OrdinalIgnoreCase),
            HasCompanyCodeColumn = commonColumns.Contains(TaktDatabaseCloneSqlHelper.CompanyCodeColumn, StringComparer.OrdinalIgnoreCase)
        };
    }

    /// <summary>
    /// 同实例 INSERT…SELECT 克隆（重写 tenant_code / company_code）
    /// </summary>
    private static async Task<int> CloneByInsertSelectAsync(
        SqlSugarClient targetDb,
        (string TenantCode, string ConnectionString, string DatabaseName) source,
        (string TenantCode, string ConnectionString, string DatabaseName) target,
        TaktDataCloneOptions options,
        DataCloneColumnMappingContext mapping)
    {
        var insertColumns = string.Join(", ", mapping.CommonColumns.Select(TaktDatabaseCloneSqlHelper.BracketColumn));
        var selectColumns = string.Join(", ", mapping.CommonColumns.Select(column => BuildSelectExpression(column, options)));
        var sourceQualified = TaktDatabaseCloneSqlHelper.QualifyTable(source.DatabaseName, options.SourceTableName);
        var targetQualified = TaktDatabaseCloneSqlHelper.QualifyTable(target.DatabaseName, options.TargetTableName);
        var whereClause = BuildSourceWhereClause(mapping.HasTenantCodeColumn);
        var parameters = BuildInsertSelectParameters(options);

        if (options.PreserveIdentityValues && mapping.HasIdentity)
        {
            await targetDb.Ado.ExecuteCommandAsync($"SET IDENTITY_INSERT {targetQualified} ON").ConfigureAwait(false);
        }

        var insertSql = $"""
            INSERT INTO {targetQualified} ({insertColumns})
            SELECT {selectColumns}
            FROM {sourceQualified}
            {whereClause}
            """;
        var affected = await targetDb.Ado.ExecuteCommandAsync(insertSql, parameters).ConfigureAwait(false);

        if (options.PreserveIdentityValues && mapping.HasIdentity)
        {
            await targetDb.Ado.ExecuteCommandAsync($"SET IDENTITY_INSERT {targetQualified} OFF").ConfigureAwait(false);
        }

        return affected;
    }

    /// <summary>
    /// 跨实例分批克隆
    /// </summary>
    private static async Task<int> CloneByBatchAsync(
        SqlSugarClient sourceDb,
        SqlSugarClient targetDb,
        TaktDataCloneOptions options,
        DataCloneColumnMappingContext mapping,
        int sourceRowCount,
        CancellationToken cancellationToken)
    {
        var columnList = string.Join(", ", mapping.CommonColumns.Select(TaktDatabaseCloneSqlHelper.BracketColumn));
        var whereClause = BuildSourceWhereClause(mapping.HasTenantCodeColumn);
        var parameters = BuildSourceParameters(options);
        var targetQualified = TaktDatabaseCloneSqlHelper.BracketTable(options.TargetTableName);
        var totalCloned = 0;
        var offset = 0;

        if (options.PreserveIdentityValues && mapping.HasIdentity)
        {
            await targetDb.Ado.ExecuteCommandAsync($"SET IDENTITY_INSERT {targetQualified} ON").ConfigureAwait(false);
        }

        while (offset < sourceRowCount)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var take = Math.Min(TaktDatabaseCloneSqlHelper.BatchSize, sourceRowCount - offset);
            var selectSql = $"""
                SELECT {columnList}
                FROM {TaktDatabaseCloneSqlHelper.BracketTable(options.SourceTableName)}
                {whereClause}
                ORDER BY (SELECT NULL)
                OFFSET {offset} ROWS FETCH NEXT {take} ROWS ONLY
                """;
            var table = await sourceDb.Ado.GetDataTableAsync(selectSql, parameters).ConfigureAwait(false);
            if (table.Rows.Count == 0)
            {
                break;
            }

            ApplyTargetScopeValues(table, mapping.CommonColumns, options);
            totalCloned += await InsertDataTableAsync(targetDb, options.TargetTableName, mapping.CommonColumns, table).ConfigureAwait(false);
            offset += take;
        }

        if (options.PreserveIdentityValues && mapping.HasIdentity)
        {
            await targetDb.Ado.ExecuteCommandAsync($"SET IDENTITY_INSERT {targetQualified} OFF").ConfigureAwait(false);
        }

        return totalCloned;
    }

    /// <summary>
    /// 将 DataTable 批量插入目标表
    /// </summary>
    private static async Task<int> InsertDataTableAsync(
        SqlSugarClient targetDb,
        string targetTableName,
        IReadOnlyList<string> commonColumns,
        DataTable table)
    {
        var rows = new List<Dictionary<string, object?>>(table.Rows.Count);
        foreach (DataRow row in table.Rows)
        {
            var dict = new Dictionary<string, object?>(commonColumns.Count, StringComparer.OrdinalIgnoreCase);
            foreach (var column in commonColumns)
            {
                var value = row[column];
                dict[column] = value == DBNull.Value ? null : value;
            }
            rows.Add(dict);
        }
        if (rows.Count == 0)
        {
            return 0;
        }
        return await targetDb.Insertable(rows).AS(targetTableName).ExecuteCommandAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// 删除目标公司现有数据
    /// </summary>
    private static async Task DeleteTargetCompanyRowsAsync(
        SqlSugarClient targetDb,
        string targetTableName,
        string targetTenantCode,
        string targetCompanyCode,
        bool hasTenantCodeColumn)
    {
        var whereClause = BuildTargetWhereClause(hasTenantCodeColumn);
        var sql = $"""
            DELETE FROM {TaktDatabaseCloneSqlHelper.BracketTable(targetTableName)}
            {whereClause}
            """;
        await targetDb.Ado.ExecuteCommandAsync(sql, BuildTargetParameters(targetTenantCode, targetCompanyCode)).ConfigureAwait(false);
    }

    /// <summary>
    /// 统计源公司匹配行数
    /// </summary>
    private static async Task<int> CountScopedRowsAsync(
        SqlSugarClient db,
        string tableName,
        string sourceTenantCode,
        string sourceCompanyCode,
        bool hasTenantCodeColumn,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var whereClause = BuildSourceWhereClause(hasTenantCodeColumn);
        var sql = $"""
            SELECT COUNT(1)
            FROM {TaktDatabaseCloneSqlHelper.BracketTable(tableName)}
            {whereClause}
            """;
        return await db.Ado.GetIntAsync(sql, BuildSourceParameters(sourceTenantCode, sourceCompanyCode)).ConfigureAwait(false);
    }

    /// <summary>
    /// 构建 SELECT 列表达式（重写 tenant_code / company_code）
    /// </summary>
    private static string BuildSelectExpression(string columnName, TaktDataCloneOptions options)
    {
        if (columnName.Equals(TaktDatabaseCloneSqlHelper.TenantCodeColumn, StringComparison.OrdinalIgnoreCase))
        {
            return "@TargetTenantCode";
        }
        if (columnName.Equals(TaktDatabaseCloneSqlHelper.CompanyCodeColumn, StringComparison.OrdinalIgnoreCase))
        {
            return "@TargetCompanyCode";
        }
        return TaktDatabaseCloneSqlHelper.BracketColumn(columnName);
    }

    /// <summary>
    /// 构建源数据 WHERE 子句
    /// </summary>
    private static string BuildSourceWhereClause(bool hasTenantCodeColumn)
    {
        var conditions = new List<string>
        {
            $"{TaktDatabaseCloneSqlHelper.BracketColumn(TaktDatabaseCloneSqlHelper.CompanyCodeColumn)} = @SourceCompanyCode"
        };
        if (hasTenantCodeColumn)
        {
            conditions.Add($"{TaktDatabaseCloneSqlHelper.BracketColumn(TaktDatabaseCloneSqlHelper.TenantCodeColumn)} = @SourceTenantCode");
        }
        return "WHERE " + string.Join(" AND ", conditions);
    }

    /// <summary>
    /// 构建目标公司 DELETE WHERE 子句
    /// </summary>
    private static string BuildTargetWhereClause(bool hasTenantCodeColumn)
    {
        var conditions = new List<string>
        {
            $"{TaktDatabaseCloneSqlHelper.BracketColumn(TaktDatabaseCloneSqlHelper.CompanyCodeColumn)} = @TargetCompanyCode"
        };
        if (hasTenantCodeColumn)
        {
            conditions.Add($"{TaktDatabaseCloneSqlHelper.BracketColumn(TaktDatabaseCloneSqlHelper.TenantCodeColumn)} = @TargetTenantCode");
        }
        return "WHERE " + string.Join(" AND ", conditions);
    }

    /// <summary>
    /// 构建源范围 SQL 参数
    /// </summary>
    private static List<SugarParameter> BuildSourceParameters(TaktDataCloneOptions options)
    {
        return BuildSourceParameters(options.SourceTenantCode.Trim(), options.SourceCompanyCode.Trim());
    }

    /// <summary>
    /// 构建源范围 SQL 参数
    /// </summary>
    private static List<SugarParameter> BuildSourceParameters(string sourceTenantCode, string sourceCompanyCode)
    {
        return new List<SugarParameter>
        {
            new("@SourceTenantCode", sourceTenantCode),
            new("@SourceCompanyCode", sourceCompanyCode)
        };
    }

    /// <summary>
    /// 构建目标范围 SQL 参数
    /// </summary>
    private static List<SugarParameter> BuildTargetParameters(string targetTenantCode, string targetCompanyCode)
    {
        return new List<SugarParameter>
        {
            new("@TargetTenantCode", targetTenantCode),
            new("@TargetCompanyCode", targetCompanyCode)
        };
    }

    /// <summary>
    /// 构建 INSERT…SELECT 所需完整参数
    /// </summary>
    private static List<SugarParameter> BuildInsertSelectParameters(TaktDataCloneOptions options)
    {
        var parameters = BuildSourceParameters(options);
        parameters.Add(new SugarParameter("@TargetTenantCode", options.TargetTenantCode.Trim()));
        parameters.Add(new SugarParameter("@TargetCompanyCode", options.TargetCompanyCode.Trim()));
        return parameters;
    }

    /// <summary>
    /// 分批模式下重写目标 tenant_code / company_code
    /// </summary>
    private static void ApplyTargetScopeValues(
        DataTable table,
        IReadOnlyList<string> commonColumns,
        TaktDataCloneOptions options)
    {
        var hasTenantCode = commonColumns.Contains(TaktDatabaseCloneSqlHelper.TenantCodeColumn, StringComparer.OrdinalIgnoreCase);
        var hasCompanyCode = commonColumns.Contains(TaktDatabaseCloneSqlHelper.CompanyCodeColumn, StringComparer.OrdinalIgnoreCase);
        if (!hasTenantCode && !hasCompanyCode)
        {
            return;
        }

        foreach (DataRow row in table.Rows)
        {
            if (hasTenantCode)
            {
                row[TaktDatabaseCloneSqlHelper.TenantCodeColumn] = options.TargetTenantCode.Trim();
            }
            if (hasCompanyCode)
            {
                row[TaktDatabaseCloneSqlHelper.CompanyCodeColumn] = options.TargetCompanyCode.Trim();
            }
        }
    }

    /// <summary>
    /// 判断源与目标是否完全相同
    /// </summary>
    private static bool IsSameScope(TaktDataCloneOptions options)
    {
        return string.Equals(options.SourceTenantCode.Trim(), options.TargetTenantCode.Trim(), StringComparison.OrdinalIgnoreCase)
            && string.Equals(options.SourceDatabaseName.Trim(), options.TargetDatabaseName.Trim(), StringComparison.OrdinalIgnoreCase)
            && string.Equals(options.SourceTableName.Trim(), options.TargetTableName.Trim(), StringComparison.OrdinalIgnoreCase)
            && string.Equals(options.SourceCompanyCode.Trim(), options.TargetCompanyCode.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 构建克隆结果
    /// </summary>
    private static TaktDataCloneResult BuildResult(
        DataCloneColumnMappingContext mapping,
        int sourceRowCount,
        int clonedRowCount,
        TaktCloneTargetBackupStepResult backupStep)
    {
        return new TaktDataCloneResult
        {
            SourceRowCount = sourceRowCount,
            ClonedRowCount = clonedRowCount,
            CommonColumnCount = mapping.CommonColumns.Count,
            CommonColumns = mapping.CommonColumns,
            SkippedSourceColumns = mapping.SkippedSourceColumns,
            SkippedTargetColumns = mapping.SkippedTargetColumns,
            BackupTableName = backupStep.BackupTableName,
            BackedUpRowCount = backupStep.BackedUpRowCount,
            ClearedRowCount = backupStep.ClearedRowCount,
            BackupSummaryMessage = backupStep.SummaryMessage
        };
    }

    /// <summary>
    /// 公司级克隆列映射上下文
    /// </summary>
    private sealed class DataCloneColumnMappingContext
    {
        public List<string> CommonColumns { get; init; } = new();
        public List<string> SkippedSourceColumns { get; init; } = new();
        public List<string> SkippedTargetColumns { get; init; } = new();
        public bool HasIdentity { get; init; }
        public bool HasTenantCodeColumn { get; init; }
        public bool HasCompanyCodeColumn { get; init; }
    }
}
