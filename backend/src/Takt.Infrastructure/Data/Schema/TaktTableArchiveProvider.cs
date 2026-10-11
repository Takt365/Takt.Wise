// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Schema
// 文件名称：TaktTableArchiveProvider.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：同库按年数据归档（创建归档表、分批 DELETE OUTPUT INTO）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using SqlSugar;
using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Code;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Schema;

/// <summary>
/// 同库按年数据归档提供者
/// </summary>
public class TaktTableArchiveProvider : ITaktTableArchiveProvider
{
    /// <summary>
    /// 单次归档上限（与数据克隆同量级）
    /// </summary>
    public const int MaxArchiveRowCount = 50000;

    /// <summary>
    /// 分批行数
    /// </summary>
    public const int BatchSize = 1000;

    private readonly IConfiguration _configuration;
    private readonly DbType _sugarDbType;

    /// <summary>
    /// 初始化归档提供者
    /// </summary>
    /// <param name="configuration">应用配置</param>
    public TaktTableArchiveProvider(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _sugarDbType = configuration.GetSugarDbType();
    }

    /// <summary>
    /// 预览将归档行数（不迁移）
    /// </summary>
    /// <param name="options">归档选项</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>预览结果</returns>
    public async Task<TaktTableArchivePreview> PreviewAsync(
        TaktTableArchiveOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();
        var normalized = NormalizeAndValidate(options);
        var resolved = TaktDatabaseCloneSqlHelper.ResolveTenantDatabase(
            _configuration,
            normalized.TargetTenantCode,
            normalized.TargetDatabaseName);
        using var db = TaktDatabaseCloneSqlHelper.CreateClient(_sugarDbType, resolved.ConnectionString, resolved.TenantCode);
        EnsureSourceTableExists(db, normalized.TableName);
        EnsureShardColumnExists(db, normalized.TableName, normalized.ArchiveKeyColumn);
        var archiveTableName = TaktTableArchiveKeyKindHelper.BuildArchiveTableNameForYear(
            normalized.TableName,
            normalized.ArchiveKeyKind,
            normalized.ArchiveYear);
        var hasCompany = HasColumn(db, normalized.TableName, TaktDatabaseCloneSqlHelper.CompanyCodeColumn);
        var count = await CountArchiveRowsAsync(db, normalized, hasCompany).ConfigureAwait(false);
        return new TaktTableArchivePreview
        {
            TableName = normalized.TableName,
            ArchiveTableName = archiveTableName,
            ArchiveYear = normalized.ArchiveYear,
            SourceRowCount = count
        };
    }

    /// <summary>
    /// 执行按年归档（基表 DELETE OUTPUT INTO 年分表 {table}_{yyyy}，分批）
    /// </summary>
    /// <param name="options">归档选项</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>执行结果</returns>
    public async Task<TaktTableArchiveResult> ArchiveAsync(
        TaktTableArchiveOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        cancellationToken.ThrowIfCancellationRequested();
        var normalized = NormalizeAndValidate(options);
        var resolved = TaktDatabaseCloneSqlHelper.ResolveTenantDatabase(
            _configuration,
            normalized.TargetTenantCode,
            normalized.TargetDatabaseName);
        using var db = TaktDatabaseCloneSqlHelper.CreateClient(_sugarDbType, resolved.ConnectionString, resolved.TenantCode);
        EnsureSourceTableExists(db, normalized.TableName);
        EnsureShardColumnExists(db, normalized.TableName, normalized.ArchiveKeyColumn);
        var archiveTableName = TaktTableArchiveKeyKindHelper.BuildArchiveTableNameForYear(
            normalized.TableName,
            normalized.ArchiveKeyKind,
            normalized.ArchiveYear);
        var hasCompany = HasColumn(db, normalized.TableName, TaktDatabaseCloneSqlHelper.CompanyCodeColumn);
        var sourceRowCount = await CountArchiveRowsAsync(db, normalized, hasCompany).ConfigureAwait(false);
        if (sourceRowCount > MaxArchiveRowCount)
        {
            throw new InvalidOperationException(
                $"待归档行数 {sourceRowCount} 超过单次上限 {MaxArchiveRowCount}");
        }
        await EnsureArchiveTableAsync(db, normalized.TableName, archiveTableName).ConfigureAwait(false);
        var archived = 0;
        while (archived < sourceRowCount)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var affected = await MoveBatchAsync(db, normalized, archiveTableName, hasCompany).ConfigureAwait(false);
            if (affected <= 0)
            {
                break;
            }
            archived = checked(archived + affected);
        }
        return new TaktTableArchiveResult
        {
            TableName = normalized.TableName,
            ArchiveTableName = archiveTableName,
            ArchiveYear = normalized.ArchiveYear,
            SourceRowCount = sourceRowCount,
            ArchivedRowCount = archived,
            DeletedRowCount = archived
        };
    }

    /// <summary>
    /// 按年份列表预建年分表（SELECT TOP 0 * INTO {table}_{year}，已存在则跳过）
    /// </summary>
    /// <param name="options">建表选项（含基表与租户库）</param>
    /// <param name="years">年份列表</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>已创建或已存在的年分表名</returns>
    public async Task<IReadOnlyList<string>> EnsureYearTablesAsync(
        TaktTableArchiveOptions options,
        IReadOnlyList<int> years,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(years);
        if (years.Count == 0)
        {
            throw new ArgumentException("请至少选择一个年份", nameof(years));
        }
        if (years.Count > 30)
        {
            throw new ArgumentException("单次最多创建 30 个年分表", nameof(years));
        }
        cancellationToken.ThrowIfCancellationRequested();
        var normalized = NormalizeAndValidate(options, requireArchiveYear: false);
        var resolved = TaktDatabaseCloneSqlHelper.ResolveTenantDatabase(
            _configuration,
            normalized.TargetTenantCode,
            normalized.TargetDatabaseName);
        using var db = TaktDatabaseCloneSqlHelper.CreateClient(_sugarDbType, resolved.ConnectionString, resolved.TenantCode);
        EnsureSourceTableExists(db, normalized.TableName);
        EnsureShardColumnExists(db, normalized.TableName, normalized.ArchiveKeyColumn);
        var created = new List<string>();
        foreach (var year in years.Distinct().OrderBy(y => y))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (year < 1970 || year > 2100)
            {
                throw new ArgumentOutOfRangeException(nameof(years), $"非法年份: {year}");
            }
            var yearTable = TaktTableArchiveKeyKindHelper.BuildArchiveTableNameForYear(
                normalized.TableName,
                normalized.ArchiveKeyKind,
                year);
            await EnsureArchiveTableAsync(db, normalized.TableName, yearTable).ConfigureAwait(false);
            created.Add(yearTable);
        }
        return created;
    }

    /// <summary>
    /// 规范化并校验选项
    /// </summary>
    /// <param name="options">原始选项</param>
    /// <param name="requireArchiveYear">是否校验归档年（建年表时可跳过）</param>
    private static TaktTableArchiveOptions NormalizeAndValidate(TaktTableArchiveOptions options, bool requireArchiveYear = true)
    {
        TaktDatabaseCloneSqlHelper.ValidateTenantCode(options.TargetTenantCode);
        TaktDatabaseCloneSqlHelper.ValidateDatabaseName(options.TargetDatabaseName);
        TaktDatabaseCloneSqlHelper.ValidateTableName(options.TableName);
        TaktDatabaseCloneSqlHelper.ValidateColumnName(options.ArchiveKeyColumn);
        if (requireArchiveYear && (options.ArchiveYear < 1970 || options.ArchiveYear > 2100))
        {
            throw new ArgumentOutOfRangeException(nameof(options.ArchiveYear), "归档年份无效");
        }
        if (options.ArchiveKeyKind is < 1 or > 3)
        {
            throw new ArgumentOutOfRangeException(nameof(options.ArchiveKeyKind), "归档键类型须为 1/2/3");
        }
        // 归档表名按 ArchiveKeyKind：…_yyyy / …_yyyyMM / …_yyyyMMddHHmmss
        if (!string.IsNullOrWhiteSpace(options.CompanyCode))
        {
            TaktDatabaseCloneSqlHelper.ValidateCompanyCode(options.CompanyCode);
        }
        return new TaktTableArchiveOptions
        {
            TargetTenantCode = options.TargetTenantCode.Trim(),
            TargetDatabaseName = options.TargetDatabaseName.Trim(),
            TableName = options.TableName.Trim().ToLowerInvariant(),
            ArchiveKeyColumn = options.ArchiveKeyColumn.Trim().ToLowerInvariant(),
            ArchiveKeyKind = options.ArchiveKeyKind,
            ArchiveYear = options.ArchiveYear,
            CompanyCode = options.CompanyCode?.Trim() ?? string.Empty
        };
    }

    /// <summary>
    /// 生成归档物理表名（按键类型格式码）
    /// </summary>
    internal static string BuildYearTableName(string tableName, int year) =>
        TaktTableArchiveKeyKindHelper.BuildArchiveTableNameForYear(tableName, TaktTableArchiveKeyKindHelper.Yyyy, year);

    /// <summary>
    /// 按策略键类型生成归档物理表名
    /// </summary>
    internal static string BuildArchiveTableName(string tableName, int archiveKeyKind, int year) =>
        TaktTableArchiveKeyKindHelper.BuildArchiveTableNameForYear(tableName, archiveKeyKind, year);

    /// <summary>
    /// 兼容旧调用：忽略 suffix，按年格式生成
    /// </summary>
    internal static string BuildArchiveTableName(string tableName, string suffix, int year) =>
        BuildYearTableName(tableName, year);

    /// <summary>
    /// 源表必须存在
    /// </summary>
    private static void EnsureSourceTableExists(SqlSugarClient db, string tableName)
    {
        if (!db.DbMaintenance.IsAnyTable(tableName, false))
        {
            throw new InvalidOperationException($"源表不存在: {tableName}");
        }
    }

    /// <summary>
    /// 归档键列必须存在
    /// </summary>
    private static void EnsureShardColumnExists(SqlSugarClient db, string tableName, string columnName)
    {
        if (!HasColumn(db, tableName, columnName))
        {
            throw new InvalidOperationException($"表 {tableName} 不存在归档键列 {columnName}");
        }
    }

    /// <summary>
    /// 列是否存在
    /// </summary>
    private static bool HasColumn(SqlSugarClient db, string tableName, string columnName)
    {
        var columns = db.DbMaintenance.GetColumnInfosByTableName(tableName, false);
        return columns != null && columns.Any(c =>
            string.Equals(c.DbColumnName, columnName, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// 归档表不存在则按热表结构创建（SELECT TOP 0 * INTO）
    /// </summary>
    private static async Task EnsureArchiveTableAsync(SqlSugarClient db, string sourceTable, string archiveTable)
    {
        if (db.DbMaintenance.IsAnyTable(archiveTable, false))
        {
            return;
        }
        var sql =
            $"SELECT TOP 0 * INTO {TaktDatabaseCloneSqlHelper.BracketTable(archiveTable)} FROM {TaktDatabaseCloneSqlHelper.BracketTable(sourceTable)}";
        await db.Ado.ExecuteCommandAsync(sql).ConfigureAwait(false);
    }

    /// <summary>
    /// 统计待归档行数
    /// </summary>
    private static async Task<int> CountArchiveRowsAsync(
        SqlSugarClient db,
        TaktTableArchiveOptions options,
        bool hasCompanyColumn)
    {
        var (whereSql, parameters) = BuildWhere(options, hasCompanyColumn);
        var sql = $"SELECT COUNT(1) FROM {TaktDatabaseCloneSqlHelper.BracketTable(options.TableName)} WHERE {whereSql}";
        var result = await db.Ado.GetScalarAsync(sql, parameters).ConfigureAwait(false);
        return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
    }

    /// <summary>
    /// 分批 DELETE OUTPUT INTO 归档表
    /// </summary>
    private static async Task<int> MoveBatchAsync(
        SqlSugarClient db,
        TaktTableArchiveOptions options,
        string archiveTableName,
        bool hasCompanyColumn)
    {
        var (whereSql, parameters) = BuildWhere(options, hasCompanyColumn);
        var sql = $"""
            DELETE TOP ({BatchSize}) FROM {TaktDatabaseCloneSqlHelper.BracketTable(options.TableName)}
            OUTPUT DELETED.* INTO {TaktDatabaseCloneSqlHelper.BracketTable(archiveTableName)}
            WHERE {whereSql}
            """;
        return await db.Ado.ExecuteCommandAsync(sql, parameters).ConfigureAwait(false);
    }

    /// <summary>
    /// 构建年份过滤 WHERE 与参数
    /// </summary>
    private static (string WhereSql, List<SugarParameter> Parameters) BuildWhere(
        TaktTableArchiveOptions options,
        bool hasCompanyColumn)
    {
        var col = TaktDatabaseCloneSqlHelper.BracketColumn(options.ArchiveKeyColumn);
        var parameters = new List<SugarParameter>();
        string yearPredicate = options.ArchiveKeyKind switch
        {
            1 => $"YEAR({col}) = @archiveYear",
            2 => $"LEFT(CONVERT(varchar(40), {col}), 4) = @archiveYearText",
            3 => $"{col} = @archiveYear",
            _ => throw new ArgumentOutOfRangeException(nameof(options.ArchiveKeyKind))
        };
        parameters.Add(new SugarParameter("@archiveYear", options.ArchiveYear));
        if (options.ArchiveKeyKind == 2)
        {
            parameters.Add(new SugarParameter("@archiveYearText", options.ArchiveYear.ToString()));
        }
        if (hasCompanyColumn && !string.IsNullOrWhiteSpace(options.CompanyCode))
        {
            yearPredicate += $" AND {TaktDatabaseCloneSqlHelper.BracketColumn(TaktDatabaseCloneSqlHelper.CompanyCodeColumn)} = @companyCode";
            parameters.Add(new SugarParameter("@companyCode", options.CompanyCode));
        }
        return (yearPredicate, parameters);
    }
}
