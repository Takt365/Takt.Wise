// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Schema
// 文件名称：TaktTableCloneProvider.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：跨租户整表数据克隆（仅允许源/目标租户不同，同名列映射）
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
/// 跨租户整表数据克隆提供者
/// </summary>
public class TaktTableCloneProvider : ITaktTableCloneProvider
{
    private readonly IConfiguration _configuration;
    private readonly SqlSugar.DbType _sugarDbType;
    private readonly ITaktDatabaseSchemaProvider _schemaProvider;

    /// <summary>
    /// 初始化数据表克隆提供者
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <param name="schemaProvider">Schema 元数据提供者</param>
    public TaktTableCloneProvider(
        IConfiguration configuration,
        ITaktDatabaseSchemaProvider schemaProvider)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _sugarDbType = configuration.GetSugarDbType();
        _schemaProvider = schemaProvider ?? throw new ArgumentNullException(nameof(schemaProvider));
    }

    /// <summary>
    /// 将源表数据克隆到目标表（仅跨租户）
    /// </summary>
    /// <param name="options">克隆选项</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>克隆结果</returns>
    public async Task<TaktTableCloneResult> CloneTableAsync(
        TaktTableCloneOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        TaktDatabaseCloneSqlHelper.ValidateTableName(options.SourceTableName);
        TaktDatabaseCloneSqlHelper.ValidateTableName(options.TargetTableName);
        TaktDatabaseCloneSqlHelper.ValidateDatabaseName(options.SourceDatabaseName);
        TaktDatabaseCloneSqlHelper.ValidateDatabaseName(options.TargetDatabaseName);
        TaktDatabaseCloneSqlHelper.ValidateTenantCode(options.SourceTenantCode);
        TaktDatabaseCloneSqlHelper.ValidateTenantCode(options.TargetTenantCode);

        if (IsSameTenant(options))
        {
            throw new InvalidOperationException("数据表克隆仅支持跨租户，同租户内不可克隆");
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
        using var sourceDb = TaktDatabaseCloneSqlHelper.CreateClient(_sugarDbType, source.ConnectionString, source.TenantCode);
        using var targetDb = TaktDatabaseCloneSqlHelper.CreateClient(_sugarDbType, target.ConnectionString, target.TenantCode);

        var sourceRowCount = await CountRowsAsync(sourceDb, options.SourceTableName, cancellationToken).ConfigureAwait(false);
        if (sourceRowCount > TaktDatabaseCloneSqlHelper.MaxCloneRowCount)
        {
            throw new InvalidOperationException(
                $"源表行数 {sourceRowCount} 超过克隆上限 {TaktDatabaseCloneSqlHelper.MaxCloneRowCount}");
        }

        var clonedRowCount = 0;
        var sameServer = TaktDatabaseCloneSqlHelper.IsSameServer(source.ConnectionString, target.ConnectionString);
        await targetDb.Ado.BeginTranAsync().ConfigureAwait(false);
        TaktCloneTargetBackupStepResult backupStep;
        try
        {
            backupStep = await TaktDatabaseCloneSqlHelper.BackupFullTableAsync(targetDb, options.TargetTableName)
                .ConfigureAwait(false);
            var clearedRowCount = await TaktDatabaseCloneSqlHelper.TruncateTableAsync(targetDb, options.TargetTableName)
                .ConfigureAwait(false);
            backupStep.ClearedRowCount = clearedRowCount;
            backupStep.SummaryMessage =
                $"{backupStep.SummaryMessage}；已 TRUNCATE 清空目标表 {options.TargetTableName} 全部 {clearedRowCount} 行";

            if (sourceRowCount == 0)
            {
                await targetDb.Ado.CommitTranAsync().ConfigureAwait(false);
                return BuildResult(columnMapping, sourceRowCount, 0, backupStep);
            }

            if (sameServer)
            {
                clonedRowCount = await CloneByInsertSelectAsync(
                    targetDb,
                    source.DatabaseName,
                    target.DatabaseName,
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
            "[TableClone] 跨租户克隆完成: {SourceTenant}/{SourceTable} -> {TargetTenant}/{TargetTable}, Rows={ClonedRowCount}",
            source.TenantCode,
            options.SourceTableName,
            target.TenantCode,
            options.TargetTableName,
            clonedRowCount);

        return BuildResult(columnMapping, sourceRowCount, clonedRowCount, backupStep);
    }

    /// <summary>
    /// 获取目标整表备份预览
    /// </summary>
    public async Task<TaktCloneTargetBackupPreview> GetTargetBackupPreviewAsync(
        TaktTableCloneOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        TaktDatabaseCloneSqlHelper.ValidateTableName(options.TargetTableName);
        TaktDatabaseCloneSqlHelper.ValidateDatabaseName(options.TargetDatabaseName);
        TaktDatabaseCloneSqlHelper.ValidateTenantCode(options.TargetTenantCode);
        var target = TaktDatabaseCloneSqlHelper.ResolveTenantDatabase(
            _configuration,
            options.TargetTenantCode,
            options.TargetDatabaseName);
        using var targetDb = TaktDatabaseCloneSqlHelper.CreateClient(_sugarDbType, target.ConnectionString, target.TenantCode);
        cancellationToken.ThrowIfCancellationRequested();
        var rowCount = await TaktDatabaseCloneSqlHelper.CountTableRowsAsync(targetDb, options.TargetTableName).ConfigureAwait(false);
        return TaktDatabaseCloneSqlHelper.BuildFullTableBackupPreview(options.TargetTableName.Trim(), rowCount);
    }

    /// <summary>
    /// 构建源/目标同名列映射
    /// </summary>
    private async Task<ColumnMappingContext> BuildColumnMappingAsync(
        string sourceTenantCode,
        string targetTenantCode,
        TaktTableCloneOptions options)
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

        return new ColumnMappingContext
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
            HasTenantCodeColumn = commonColumns.Contains(TaktDatabaseCloneSqlHelper.TenantCodeColumn, StringComparer.OrdinalIgnoreCase)
        };
    }

    /// <summary>
    /// 构建 SELECT 列表达式（跨租户时重写 tenant_code）
    /// </summary>
    private static string BuildSelectExpression(string columnName, string targetTenantCode, bool hasTenantCodeColumn)
    {
        if (hasTenantCodeColumn
            && columnName.Equals(TaktDatabaseCloneSqlHelper.TenantCodeColumn, StringComparison.OrdinalIgnoreCase))
        {
            return "@TargetTenantCode";
        }
        return TaktDatabaseCloneSqlHelper.BracketColumn(columnName);
    }

    /// <summary>
    /// 分批模式下重写目标 tenant_code
    /// </summary>
    private static void ApplyTargetTenantCode(DataTable table, string targetTenantCode, bool hasTenantCodeColumn)
    {
        if (!hasTenantCodeColumn)
        {
            return;
        }
        foreach (DataRow row in table.Rows)
        {
            row[TaktDatabaseCloneSqlHelper.TenantCodeColumn] = targetTenantCode;
        }
    }

    /// <summary>
    /// 同 SQL Server 实例内使用 INSERT…SELECT 克隆（重写 tenant_code）
    /// </summary>
    private static async Task<int> CloneByInsertSelectAsync(
        SqlSugarClient targetDb,
        string sourceDatabaseName,
        string targetDatabaseName,
        TaktTableCloneOptions options,
        ColumnMappingContext mapping)
    {
        var insertColumns = string.Join(", ", mapping.CommonColumns.Select(TaktDatabaseCloneSqlHelper.BracketColumn));
        var selectColumns = string.Join(
            ", ",
            mapping.CommonColumns.Select(column =>
                BuildSelectExpression(column, options.TargetTenantCode.Trim(), mapping.HasTenantCodeColumn)));
        var sourceQualified = TaktDatabaseCloneSqlHelper.QualifyTable(sourceDatabaseName, options.SourceTableName);
        var targetQualified = TaktDatabaseCloneSqlHelper.QualifyTable(targetDatabaseName, options.TargetTableName);
        var parameters = mapping.HasTenantCodeColumn
            ? new List<SugarParameter> { new("@TargetTenantCode", options.TargetTenantCode.Trim()) }
            : null;
        if (options.PreserveIdentityValues && mapping.HasIdentity)
        {
            await targetDb.Ado.ExecuteCommandAsync($"SET IDENTITY_INSERT {targetQualified} ON").ConfigureAwait(false);
        }
        var insertSql = $"""
            INSERT INTO {targetQualified} ({insertColumns})
            SELECT {selectColumns} FROM {sourceQualified}
            """;
        var affected = parameters == null
            ? await targetDb.Ado.ExecuteCommandAsync(insertSql).ConfigureAwait(false)
            : await targetDb.Ado.ExecuteCommandAsync(insertSql, parameters).ConfigureAwait(false);
        if (options.PreserveIdentityValues && mapping.HasIdentity)
        {
            await targetDb.Ado.ExecuteCommandAsync($"SET IDENTITY_INSERT {targetQualified} OFF").ConfigureAwait(false);
        }
        return affected;
    }

    /// <summary>
    /// 跨实例时分批读取源表并写入目标表（重写 tenant_code）
    /// </summary>
    private static async Task<int> CloneByBatchAsync(
        SqlSugarClient sourceDb,
        SqlSugarClient targetDb,
        TaktTableCloneOptions options,
        ColumnMappingContext mapping,
        int sourceRowCount,
        CancellationToken cancellationToken)
    {
        var columnList = string.Join(", ", mapping.CommonColumns.Select(TaktDatabaseCloneSqlHelper.BracketColumn));
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
                ORDER BY (SELECT NULL)
                OFFSET {offset} ROWS FETCH NEXT {take} ROWS ONLY
                """;
            var table = await sourceDb.Ado.GetDataTableAsync(selectSql).ConfigureAwait(false);
            if (table.Rows.Count == 0)
            {
                break;
            }
            ApplyTargetTenantCode(table, options.TargetTenantCode.Trim(), mapping.HasTenantCodeColumn);
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
    /// 统计源表行数
    /// </summary>
    private static async Task<int> CountRowsAsync(SqlSugarClient db, string tableName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var sql = $"SELECT COUNT(1) FROM {TaktDatabaseCloneSqlHelper.BracketTable(tableName)}";
        return await db.Ado.GetIntAsync(sql).ConfigureAwait(false);
    }

    /// <summary>
    /// 判断是否为同一租户
    /// </summary>
    private static bool IsSameTenant(TaktTableCloneOptions options)
    {
        return string.Equals(
            options.SourceTenantCode.Trim(),
            options.TargetTenantCode.Trim(),
            StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 构建克隆结果
    /// </summary>
    private static TaktTableCloneResult BuildResult(
        ColumnMappingContext mapping,
        int sourceRowCount,
        int clonedRowCount,
        TaktCloneTargetBackupStepResult backupStep)
    {
        return new TaktTableCloneResult
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
    /// 列映射上下文
    /// </summary>
    private sealed class ColumnMappingContext
    {
        public List<string> CommonColumns { get; init; } = new();
        public List<string> SkippedSourceColumns { get; init; } = new();
        public List<string> SkippedTargetColumns { get; init; } = new();
        public bool HasIdentity { get; init; }
        public bool HasTenantCodeColumn { get; init; }
    }
}
