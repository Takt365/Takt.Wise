// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktQuartzRelationIssueService.cs
// 创建时间：2026-11-04
// 创建人：Takt365(Cursor AI)
// 功能描述：同步前用 SqlSugar Ado 对源表做 LEFT HASH JOIN，只投影编码/行号/主键；有问题则中止同步，清单分页与分批导出
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Text;
using SqlSugar;
using Takt.Application.Dtos.Foundation;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 同步脚本主子表关系核对（哈希反连接，禁止嵌套 NOT EXISTS）
/// </summary>
public interface ITaktQuartzRelationIssueService
{
    /// <summary>
    /// 同步开始前核对。有孤儿明细或空头凭证时抛出，消息以 QUARTZ_RELATION|脚本|库名| 开头
    /// </summary>
    /// <param name="sourceDatabase">源库；设变脚本可空</param>
    /// <param name="scriptPath">任务 SqlScript，如 Quartz/sync_po.sql</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>任务</returns>
    Task ThrowIfBrokenAsync(string? sourceDatabase, string? scriptPath, CancellationToken cancellationToken = default);

    /// <summary>
    /// 分页读取某一类问题行（服务端 OFFSET，每页不超过分页上限）
    /// </summary>
    /// <param name="query">脚本、源库、类型、页码</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>分页结果</returns>
    Task<TaktPagedResult<TaktQuartzRelationIssueRowDto>> GetRelationIssuePageAsync(
        TaktQuartzRelationIssueQueryDto query,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 按主键分批写出两类问题行的 CSV（不把全量装入内存）
    /// </summary>
    /// <param name="script">脚本文件名</param>
    /// <param name="sourceDatabase">源库</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>文件名与临时文件路径（调用方读完后删除）</returns>
    Task<(string FileName, string TempPath)> ExportRelationIssueCsvAsync(
        string script,
        string? sourceDatabase,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// 用当前 SqlSugar 连接对同实例源库执行 LEFT HASH JOIN
/// </summary>
public sealed class TaktQuartzRelationIssueService : ITaktQuartzRelationIssueService
{
    private const int ProbeTimeoutSeconds = 180;
    private const int ExportBatchSize = 20000;
    private const string Marker = "QUARTZ_RELATION";

    private static readonly RelSpec[] Specs =
    [
        new("sync_po.sql", "takt_logistics_procurement_purchase_order", "takt_logistics_procurement_purchase_order_item", "purchase_order_code", "line_number", null),
        new("sync_so.sql", "takt_logistics_sales_order", "takt_logistics_sales_order_item", "sales_order_code", "line_number", null),
        new("sync_miro.sql", "takt_logistics_procurement_purchase_invoice", "takt_logistics_procurement_purchase_invoice_item", "purchase_invoice_code", "line_number", null),
        new("sync_billing.sql", "takt_logistics_sales_invoice", "takt_logistics_sales_invoice_item", "billing_document_code", "line_number", null),
        new("sync_pup.sql", "takt_logistics_procurement_purchase_condition", "takt_logistics_procurement_purchase_condition_item", "purchase_condition_code", "purchase_condition_seq", null),
        new("sync_sp.sql", "takt_logistics_sales_condition", "takt_logistics_sales_condition_item", "sales_condition_code", "sales_condition_seq", null),
        new("sync_matdoc.sql", "takt_logistics_materials_material_document", "takt_logistics_materials_material_document_item", "material_document_code", "line_number", null),
        new("sync_ec.sql", "PP_SapEcn", "PP_SapEcnSub", "D_SAP_ZPABD_Z001", null, "D_SAP_ZPABD_S001", "Sap_Data", false),
    ];

    private readonly ISqlSugarClient _db;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="db">当前租户 SqlSugar 客户端（与源库同一实例，跨库三段名访问）</param>
    public TaktQuartzRelationIssueService(ISqlSugarClient db)
    {
        ArgumentNullException.ThrowIfNull(db);
        _db = db;
    }

    /// <inheritdoc />
    public async Task ThrowIfBrokenAsync(string? sourceDatabase, string? scriptPath, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!TryResolve(scriptPath, sourceDatabase, out var spec, out var database))
        {
            return;
        }
        var previous = _db.Ado.CommandTimeOut;
        _db.Ado.CommandTimeOut = ProbeTimeoutSeconds;
        try
        {
            var orphan = await CountAsync(spec, database, "orphan", cancellationToken).ConfigureAwait(false);
            var empty = await CountAsync(spec, database, "empty", cancellationToken).ConfigureAwait(false);
            if (orphan == 0 && empty == 0)
            {
                return;
            }
            var orphanSample = orphan > 0
                ? await SampleAsync(spec, database, "orphan", cancellationToken).ConfigureAwait(false)
                : string.Empty;
            var emptySample = empty > 0
                ? await SampleAsync(spec, database, "empty", cancellationToken).ConfigureAwait(false)
                : string.Empty;
            throw new InvalidOperationException(BuildMessage(spec.ScriptFile, database, orphan, orphanSample, empty, emptySample));
        }
        finally
        {
            _db.Ado.CommandTimeOut = previous;
        }
    }

    /// <inheritdoc />
    public async Task<TaktPagedResult<TaktQuartzRelationIssueRowDto>> GetRelationIssuePageAsync(
        TaktQuartzRelationIssueQueryDto query,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);
        cancellationToken.ThrowIfCancellationRequested();
        var kind = NormalizeKind(query.IssueKind);
        if (!TryResolve(query.Script, query.SourceDatabase, out var spec, out var database))
        {
            throw new ArgumentException($"不支持的同步脚本: {query.Script}");
        }
        var pageIndex = TaktPagedClamp.NormalizePageIndex(query.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(query.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var previous = _db.Ado.CommandTimeOut;
        _db.Ado.CommandTimeOut = ProbeTimeoutSeconds;
        try
        {
            var total = await CountAsync(spec, database, kind, cancellationToken).ConfigureAwait(false);
            if (total > int.MaxValue)
            {
                throw new InvalidOperationException("清单行数超过分页整数上限");
            }
            var rows = total == 0
                ? []
                : await ReadPageAsync(spec, database, kind, skip, pageSize, cancellationToken).ConfigureAwait(false);
            return TaktPagedResult<TaktQuartzRelationIssueRowDto>.Create(rows, (int)total, pageIndex, pageSize);
        }
        finally
        {
            _db.Ado.CommandTimeOut = previous;
        }
    }

    /// <inheritdoc />
    public async Task<(string FileName, string TempPath)> ExportRelationIssueCsvAsync(
        string script,
        string? sourceDatabase,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(script);
        cancellationToken.ThrowIfCancellationRequested();
        if (!TryResolve(script, sourceDatabase, out var spec, out var database))
        {
            throw new ArgumentException($"不支持的同步脚本: {script}");
        }
        var fileName = spec.ScriptFile.Replace(".sql", "", StringComparison.OrdinalIgnoreCase) + "_relation.csv";
        var path = Path.Combine(Path.GetTempPath(), "takt-relation-" + Guid.NewGuid().ToString("N") + ".csv");
        var previous = _db.Ado.CommandTimeOut;
        _db.Ado.CommandTimeOut = ProbeTimeoutSeconds;
        try
        {
            await using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
            await using var writer = new StreamWriter(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
            await writer.WriteLineAsync("issue_kind,business_code,line_number,row_id").ConfigureAwait(false);
            await WriteKindAsync(writer, spec, database, "orphan", cancellationToken).ConfigureAwait(false);
            await WriteKindAsync(writer, spec, database, "empty", cancellationToken).ConfigureAwait(false);
            await writer.FlushAsync(cancellationToken).ConfigureAwait(false);
            return (fileName, path);
        }
        catch
        {
            TryDelete(path);
            throw;
        }
        finally
        {
            _db.Ado.CommandTimeOut = previous;
        }
    }

    /// <summary>
    /// 按主键向后分批写入一类问题行
    /// </summary>
    private async Task WriteKindAsync(
        StreamWriter writer,
        RelSpec spec,
        string database,
        string kind,
        CancellationToken cancellationToken)
    {
        if (!spec.HasRowId)
        {
            var skip = 0;
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var page = await ReadPageAsync(spec, database, kind, skip, ExportBatchSize, cancellationToken).ConfigureAwait(false);
                if (page.Count == 0)
                {
                    return;
                }
                foreach (var row in page)
                {
                    await WriteCsvRowAsync(writer, kind, row).ConfigureAwait(false);
                }
                if (page.Count < ExportBatchSize)
                {
                    return;
                }
                skip = checked(skip + page.Count);
            }
        }
        long lastId = 0;
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var batch = await ReadKeysetAsync(spec, database, kind, lastId, ExportBatchSize, cancellationToken).ConfigureAwait(false);
            if (batch.Count == 0)
            {
                return;
            }
            foreach (var row in batch)
            {
                await WriteCsvRowAsync(writer, kind, row).ConfigureAwait(false);
            }
            if (!spec.HasRowId || batch.Count < ExportBatchSize)
            {
                return;
            }
            if (!long.TryParse(batch[^1].RowId, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) || parsed <= lastId)
            {
                return;
            }
            lastId = parsed;
        }
    }

    /// <summary>
    /// 统计一类问题行
    /// </summary>
    private async Task<long> CountAsync(RelSpec spec, string database, string kind, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var sql = BuildSql(spec, database, kind, "count");
        var value = await _db.Ado.GetScalarAsync(sql).ConfigureAwait(false);
        return value == null || value == DBNull.Value ? 0 : Convert.ToInt64(value, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 取一条样例键
    /// </summary>
    private async Task<string> SampleAsync(RelSpec spec, string database, string kind, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var table = await _db.Ado.GetDataTableAsync(BuildSql(spec, database, kind, "sample")).ConfigureAwait(false);
        if (table.Rows.Count == 0)
        {
            return string.Empty;
        }
        var code = Convert.ToString(table.Rows[0]["business_code"], CultureInfo.InvariantCulture) ?? string.Empty;
        var line = Convert.ToString(table.Rows[0]["line_number"], CultureInfo.InvariantCulture) ?? string.Empty;
        return string.IsNullOrEmpty(line) ? code : code + "/" + line;
    }

    /// <summary>
    /// 读取一页
    /// </summary>
    private async Task<List<TaktQuartzRelationIssueRowDto>> ReadPageAsync(
        RelSpec spec,
        string database,
        string kind,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var sql = BuildSql(spec, database, kind, "page");
        var table = await _db.Ado.GetDataTableAsync(
            sql,
            new SugarParameter("@skip", skip),
            new SugarParameter("@take", take)).ConfigureAwait(false);
        return MapRows(table, kind);
    }

    /// <summary>
    /// 主键大于 lastId 的下一批
    /// </summary>
    private async Task<List<TaktQuartzRelationIssueRowDto>> ReadKeysetAsync(
        RelSpec spec,
        string database,
        string kind,
        long lastId,
        int take,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!spec.HasRowId)
        {
            return lastId > 0
                ? []
                : await ReadPageAsync(spec, database, kind, 0, take, cancellationToken).ConfigureAwait(false);
        }
        var sql = BuildSql(spec, database, kind, "keyset");
        var table = await _db.Ado.GetDataTableAsync(
            sql,
            new SugarParameter("@lastId", lastId),
            new SugarParameter("@take", take)).ConfigureAwait(false);
        return MapRows(table, kind);
    }

    /// <summary>
    /// 解析脚本与库名。未登记的脚本返回 false
    /// </summary>
    private static bool TryResolve(string? scriptPath, string? sourceDatabase, out RelSpec spec, out string database)
    {
        spec = null!;
        database = string.Empty;
        var file = Path.GetFileName((scriptPath ?? string.Empty).Trim().Replace('\\', '/'));
        var found = Specs.FirstOrDefault(x => string.Equals(x.ScriptFile, file, StringComparison.OrdinalIgnoreCase));
        if (found == null)
        {
            return false;
        }
        spec = found;
        if (!string.IsNullOrWhiteSpace(spec.FixedDatabase))
        {
            database = spec.FixedDatabase;
            TaktQuartzSyncExecuteParamsHelper.ValidateDatabaseName(database);
            return true;
        }
        if (string.IsNullOrWhiteSpace(sourceDatabase))
        {
            return false;
        }
        TaktQuartzSyncExecuteParamsHelper.ValidateDatabaseName(sourceDatabase);
        database = sourceDatabase.Trim();
        return true;
    }

    /// <summary>
    /// 规范化问题类型
    /// </summary>
    private static string NormalizeKind(string? kind)
    {
        var value = (kind ?? string.Empty).Trim();
        if (value.Equals("orphan", StringComparison.OrdinalIgnoreCase) || value.Equals("empty", StringComparison.OrdinalIgnoreCase))
        {
            return value.ToLowerInvariant();
        }
        throw new ArgumentException("IssueKind 只能是 orphan 或 empty");
    }

    /// <summary>
    /// 拼失败消息。标记放在最前，避免落库截断后前端无法打开清单
    /// </summary>
    private static string BuildMessage(string script, string database, long orphan, string orphanSample, long empty, string emptySample)
    {
        var text = new StringBuilder();
        text.Append(Marker).Append('|').Append(script).Append('|').Append(database).Append('|');
        if (orphan > 0)
        {
            text.Append("孤儿明细: n=").Append(orphan.ToString(CultureInfo.InvariantCulture)).Append(", sample=").Append(orphanSample);
        }
        if (orphan > 0 && empty > 0)
        {
            text.Append('；');
        }
        if (empty > 0)
        {
            text.Append("空头凭证: n=").Append(empty.ToString(CultureInfo.InvariantCulture)).Append(", sample=").Append(emptySample);
        }
        return text.ToString();
    }

    /// <summary>
    /// 生成哈希反连接 SQL。mode：count / sample / page / keyset
    /// </summary>
    private static string BuildSql(RelSpec spec, string database, string kind, string mode)
    {
        var db = Bracket(database);
        var header = db + ".[dbo]." + Bracket(spec.HeaderTable);
        var item = db + ".[dbo]." + Bracket(spec.ItemTable);
        var headerCode = Bracket(spec.HeaderCodeColumn);
        var itemCode = Bracket(spec.ItemCodeColumn ?? spec.HeaderCodeColumn);
        var fromJoin = kind == "orphan"
            ? item + " AS i LEFT HASH JOIN " + header + " AS h ON h." + headerCode + " = i." + itemCode
            : header + " AS h LEFT HASH JOIN " + item + " AS i ON i." + itemCode + " = h." + headerCode;
        var missing = kind == "orphan" ? "h." + headerCode + " IS NULL" : "i." + itemCode + " IS NULL";
        var side = kind == "orphan" ? "i" : "h";
        var lineExpr = spec.LineColumn == null || kind == "empty"
            ? "CAST(N'' AS NVARCHAR(40))"
            : "CAST(" + side + "." + Bracket(spec.LineColumn) + " AS NVARCHAR(40))";
        var idExpr = spec.HasRowId
            ? "CAST(" + side + ".[id] AS BIGINT)"
            : "CAST(0 AS BIGINT)";
        var order = spec.HasRowId ? side + ".[id]" : side + "." + (kind == "orphan" ? itemCode : headerCode);
        var projection = side + "." + (kind == "orphan" ? itemCode : headerCode) + " AS [business_code], " + lineExpr + " AS [line_number], " + idExpr + " AS [row_id]";
        if (mode == "count")
        {
            return "SELECT COUNT_BIG(1) FROM " + fromJoin + " WHERE " + missing;
        }
        if (mode == "sample")
        {
            return "SELECT TOP (1) " + projection + " FROM " + fromJoin + " WHERE " + missing + " ORDER BY " + order;
        }
        if (mode == "page")
        {
            return "SELECT " + projection + " FROM " + fromJoin + " WHERE " + missing + " ORDER BY " + order + " OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";
        }
        var keyset = spec.HasRowId ? " AND " + side + ".[id] > @lastId" : string.Empty;
        return "SELECT TOP (@take) " + projection + " FROM " + fromJoin + " WHERE " + missing + keyset + " ORDER BY " + order;
    }

    /// <summary>
    /// 结果表转 DTO。主键以字符串返回
    /// </summary>
    private static List<TaktQuartzRelationIssueRowDto> MapRows(System.Data.DataTable table, string kind)
    {
        var rows = new List<TaktQuartzRelationIssueRowDto>(table.Rows.Count);
        foreach (System.Data.DataRow dataRow in table.Rows)
        {
            rows.Add(new TaktQuartzRelationIssueRowDto
            {
                IssueKind = kind,
                BusinessCode = Convert.ToString(dataRow["business_code"], CultureInfo.InvariantCulture) ?? string.Empty,
                LineNumber = Convert.ToString(dataRow["line_number"], CultureInfo.InvariantCulture) ?? string.Empty,
                RowId = Convert.ToString(dataRow["row_id"], CultureInfo.InvariantCulture) ?? string.Empty,
            });
        }
        return rows;
    }

    /// <summary>
    /// 标识符加括号。只允许字母数字下划线
    /// </summary>
    private static string Bracket(string identifier)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
        foreach (var ch in identifier)
        {
            if (!char.IsAsciiLetterOrDigit(ch) && ch != '_')
            {
                throw new InvalidOperationException("非法标识符: " + identifier);
            }
        }
        return "[" + identifier + "]";
    }

    /// <summary>
    /// 写一行 CSV
    /// </summary>
    private static Task WriteCsvRowAsync(StreamWriter writer, string kind, TaktQuartzRelationIssueRowDto row)
    {
        return writer.WriteLineAsync(string.Join(',',
            Csv(kind),
            Csv(row.BusinessCode),
            Csv(row.LineNumber),
            Csv(row.RowId)));
    }

    /// <summary>
    /// CSV 字段转义
    /// </summary>
    private static string Csv(string? value)
    {
        var text = value ?? string.Empty;
        if (text.Contains('"') || text.Contains(',') || text.Contains('\n') || text.Contains('\r'))
        {
            return "\"" + text.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";
        }
        return text;
    }

    /// <summary>
    /// 删除临时导出文件。失败时忽略
    /// </summary>
    private static void TryDelete(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }

    /// <summary>
    /// 一张主子表的关联键
    /// </summary>
    /// <param name="ScriptFile">脚本文件名</param>
    /// <param name="HeaderTable">主表</param>
    /// <param name="ItemTable">明细表</param>
    /// <param name="HeaderCodeColumn">主表业务编码列</param>
    /// <param name="ItemCodeColumn">明细业务编码列；空则与主表同名</param>
    /// <param name="LineColumn">明细行号列；空头凭证不输出</param>
    /// <param name="FixedDatabase">固定库名；空则用源库参数</param>
    /// <param name="HasRowId">是否有 bigint id，用于键集分页</param>
    private sealed record RelSpec(
        string ScriptFile,
        string HeaderTable,
        string ItemTable,
        string HeaderCodeColumn,
        string? LineColumn,
        string? ItemCodeColumn,
        string? FixedDatabase = null,
        bool HasRowId = true);
}
