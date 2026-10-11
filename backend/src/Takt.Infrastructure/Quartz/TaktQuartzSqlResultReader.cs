// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Quartz
// 文件名称：TaktQuartzSqlResultReader.cs
// 创建时间：2026-07-15
// 创建人：Takt365(Cursor AI)
// 功能描述：执行 Quartz SQL 脚本；即时消费 QUARTZ_SYNC_PROGRESS（结果集 + RAISERROR WITH NOWAIT），末尾解析 QUARTZ_SYNC_SUMMARY
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using SqlSugar;
using Takt.Shared.Constants;

namespace Takt.Infrastructure.Quartz;

/// <summary>
/// Quartz SQL 执行与汇总结果解析（分批进度：RAISERROR WITH NOWAIT 即时回调 + 结果集兜底；摘要写入 ExecuteMessage / quartz 日志）
/// </summary>
public static class TaktQuartzSqlResultReader
{
    /// <summary>
    /// 执行非查询脚本，读取全部结果集并格式化为执行摘要
    /// </summary>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="sql">可执行 SQL</param>
    /// <param name="scriptPath">脚本路径（写入摘要）</param>
    /// <param name="onProgress">分批进度（QUARTZ_SYNC_PROGRESS）；空则只记末尾摘要</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>含行数统计的执行摘要</returns>
    public static async Task<string> ExecuteAndFormatSummaryAsync(
        ISqlSugarClient db,
        string sql,
        string? scriptPath,
        Action<string>? onProgress = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(db);
        ArgumentException.ThrowIfNullOrWhiteSpace(sql);
        var scopes = new List<SyncScopeCounts>();
        var legacyPairs = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var loggedProgress = new HashSet<string>(StringComparer.Ordinal);
        void EmitProgress(string text)
        {
            if (string.IsNullOrWhiteSpace(text) || !loggedProgress.Add(text))
            {
                return;
            }
            onProgress?.Invoke(text);
        }
        var connection = db.Ado.Connection as DbConnection
            ?? throw new InvalidOperationException("Quartz SQL 执行失败：Ado.Connection 不是 DbConnection");
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }
        using var infoMessage = AttachSqlInfoMessage(connection, raw =>
        {
            if (TryFormatProgressFromInfoMessage(scriptPath, raw, out var formatted))
            {
                EmitProgress(formatted);
            }
        });
        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;
        command.CommandTimeout = db.Ado.CommandTimeOut >= 0
            ? db.Ado.CommandTimeOut
            : TaktQuartzConstants.DefaultSqlCommandTimeoutSeconds;
        if (db.Ado.Transaction is DbTransaction dbTransaction)
        {
            command.Transaction = dbTransaction;
        }
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        do
        {
            await ConsumeResultSetAsync(
                reader,
                scriptPath,
                scopes,
                legacyPairs,
                EmitProgress,
                cancellationToken).ConfigureAwait(false);
        }
        while (await reader.NextResultAsync(cancellationToken).ConfigureAwait(false));
        return FormatExecuteMessage(scriptPath, scopes, legacyPairs);
    }

    /// <summary>
    /// 消费单个结果集：QUARTZ_SYNC_PROGRESS 即时回调；QUARTZ_SYNC_SUMMARY 记入摘要；否则兼容两列 metric/value
    /// </summary>
    /// <param name="reader">当前结果集</param>
    /// <param name="scriptPath">脚本路径</param>
    /// <param name="scopes">汇总缓冲</param>
    /// <param name="legacyPairs">旧两列指标</param>
    /// <param name="onProgress">分批进度回调</param>
    /// <param name="cancellationToken">取消令牌</param>
    private static async Task ConsumeResultSetAsync(
        DbDataReader reader,
        string? scriptPath,
        List<SyncScopeCounts> scopes,
        Dictionary<string, int> legacyPairs,
        Action<string>? onProgress,
        CancellationToken cancellationToken)
    {
        if (reader.FieldCount == 0)
        {
            return;
        }
        var hasSummaryTag = HasColumn(reader, "summary_tag");
        if (hasSummaryTag)
        {
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                var tag = reader["summary_tag"]?.ToString();
                if (string.Equals(tag, TaktQuartzConstants.SqlSyncProgressTag, StringComparison.OrdinalIgnoreCase))
                {
                    onProgress?.Invoke(FormatProgressMessageFromReader(scriptPath, reader));
                    continue;
                }
                if (!string.Equals(tag, TaktQuartzConstants.SqlSyncSummaryTag, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                var hasMergeStats = HasColumn(reader, "source_count");
                scopes.Add(new SyncScopeCounts
                {
                    Scope = ReadString(reader, "scope"),
                    HasMergeStats = hasMergeStats,
                    SourceRawCount = HasColumn(reader, "source_raw_count")
                        ? ReadInt(reader, "source_raw_count")
                        : HasColumn(reader, "sap_raw_count")
                            ? ReadInt(reader, "sap_raw_count")
                            : null,
                    SourceCount = hasMergeStats ? ReadInt(reader, "source_count") : 0,
                    SkippedEmptyCount = HasColumn(reader, "skipped_empty_count")
                        ? ReadInt(reader, "skipped_empty_count")
                        : null,
                    LeadingZeroStrippedCount = HasColumn(reader, "leading_zero_stripped_count")
                        ? ReadInt(reader, "leading_zero_stripped_count")
                        : null,
                    LeadingZeroHeaderCount = HasColumn(reader, "leading_zero_header_count")
                        ? ReadInt(reader, "leading_zero_header_count")
                        : null,
                    LeadingZeroItemCount = HasColumn(reader, "leading_zero_item_count")
                        ? ReadInt(reader, "leading_zero_item_count")
                        : null,
                    LeadingZeroClashSkippedCount = HasColumn(reader, "leading_zero_clash_skipped_count")
                        ? ReadInt(reader, "leading_zero_clash_skipped_count")
                        : null,
                    ModelBackfilledCount = HasColumn(reader, "model_backfilled_count")
                        ? ReadInt(reader, "model_backfilled_count")
                        : null,
                    MaterialTypeBackfilledCount = HasColumn(reader, "material_type_backfilled_count")
                        ? ReadInt(reader, "material_type_backfilled_count")
                        : null,
                    ModelClashSkippedCount = HasColumn(reader, "model_clash_skipped_count")
                        ? ReadInt(reader, "model_clash_skipped_count")
                        : null,
                    AverageUpdatedCount = HasColumn(reader, "average_updated_count")
                        ? ReadInt(reader, "average_updated_count")
                        : null,
                    DedupeDroppedCount = HasColumn(reader, "dedupe_dropped")
                        ? ReadInt(reader, "dedupe_dropped")
                        : null,
                    TargetBefore = ReadInt(reader, "target_before"),
                    TargetAfter = ReadInt(reader, "target_after"),
                    TargetPhysical = ReadInt(reader, "target_physical"),
                    SoftDeleted = ReadInt(reader, "soft_deleted"),
                    InsertCount = ReadInt(reader, "insert_count"),
                    UpdateCount = ReadInt(reader, "update_count"),
                    UnchangedCount = HasColumn(reader, "unchanged_count")
                        ? ReadInt(reader, "unchanged_count")
                        : hasMergeStats
                            ? Math.Max(0, ReadInt(reader, "source_count")
                                - ReadInt(reader, "insert_count")
                                - ReadInt(reader, "update_count"))
                            : 0,
                    DeleteCount = ReadInt(reader, "delete_count"),
                    SoftDeletedKeys = HasColumn(reader, "soft_deleted_keys")
                        ? ReadString(reader, "soft_deleted_keys")
                        : string.Empty,
                });
            }
            return;
        }
        if (reader.FieldCount >= 2)
        {
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                if (reader.IsDBNull(0) || reader.IsDBNull(1))
                {
                    continue;
                }
                var key = Convert.ToString(reader.GetValue(0), CultureInfo.InvariantCulture);
                if (string.IsNullOrWhiteSpace(key))
                {
                    continue;
                }
                if (int.TryParse(
                        Convert.ToString(reader.GetValue(1), CultureInfo.InvariantCulture),
                        NumberStyles.Integer,
                        CultureInfo.InvariantCulture,
                        out var value))
                {
                    legacyPairs[key.Trim()] = value;
                }
            }
        }
    }

    /// <summary>
    /// 格式化执行摘要文案（供落库 ExecuteMessage 与 quartz 日志）
    /// </summary>
    private static string FormatExecuteMessage(
        string? scriptPath,
        IReadOnlyList<SyncScopeCounts> scopes,
        IReadOnlyDictionary<string, int> legacyPairs)
    {
        var path = string.IsNullOrWhiteSpace(scriptPath) ? "(inline)" : scriptPath.Trim();
        if (scopes.Count > 0)
        {
            var sb = new StringBuilder();
            sb.Append("SQL 同步完成，路径=").Append(path);
            foreach (var scope in scopes)
            {
                sb.Append("；");
                if (!string.IsNullOrWhiteSpace(scope.Scope))
                {
                    sb.Append('[').Append(MapScopeLabel(scope.Scope)).Append(']');
                }
                var wrotePart = false;
                AppendScopePart(sb, ref wrotePart, FormatLeadingZeroPart(scope));
                if (scope.ModelBackfilledCount.HasValue)
                {
                    AppendScopePart(sb, ref wrotePart,
                        "回填机种=" + scope.ModelBackfilledCount.Value.ToString(CultureInfo.InvariantCulture));
                }
                if (scope.MaterialTypeBackfilledCount.HasValue)
                {
                    AppendScopePart(sb, ref wrotePart,
                        "回填物料类型=" + scope.MaterialTypeBackfilledCount.Value.ToString(CultureInfo.InvariantCulture));
                }
                if (scope.LeadingZeroClashSkippedCount is > 0)
                {
                    AppendScopePart(sb, ref wrotePart,
                        "去前导撞键跳过=" + scope.LeadingZeroClashSkippedCount.Value.ToString(CultureInfo.InvariantCulture));
                }
                if (scope.ModelClashSkippedCount is > 0)
                {
                    AppendScopePart(sb, ref wrotePart,
                        "机种撞键跳过=" + scope.ModelClashSkippedCount.Value.ToString(CultureInfo.InvariantCulture));
                }
                if (!scope.HasMergeStats)
                {
                    continue;
                }
                // source_raw=源表物理行；装入=过滤空键+业务键去重后；真正更新=业务字段有差；未变=MERGE 未写
                if (wrotePart)
                {
                    sb.Append("，");
                }
                if (scope.SourceRawCount.HasValue)
                {
                    sb.Append("源表=").Append(scope.SourceRawCount.Value.ToString(CultureInfo.InvariantCulture));
                    sb.Append("，装入=");
                }
                else
                {
                    sb.Append("装入=");
                }
                sb.Append(scope.SourceCount.ToString(CultureInfo.InvariantCulture));
                if (scope.SkippedEmptyCount is > 0)
                {
                    sb.Append("，跳过空键=").Append(scope.SkippedEmptyCount.Value.ToString(CultureInfo.InvariantCulture));
                }
                if (scope.AverageUpdatedCount is > 0)
                {
                    sb.Append("，重算月均=").Append(scope.AverageUpdatedCount.Value.ToString(CultureInfo.InvariantCulture));
                }
                if (scope.DedupeDroppedCount is > 0)
                {
                    sb.Append("，业务键去重=").Append(scope.DedupeDroppedCount.Value.ToString(CultureInfo.InvariantCulture));
                }
                sb.Append("，有效行(更新前)=").Append(scope.TargetBefore.ToString(CultureInfo.InvariantCulture));
                sb.Append("，有效行(更新后)=").Append(scope.TargetAfter.ToString(CultureInfo.InvariantCulture));
                sb.Append("，物理行=").Append(scope.TargetPhysical.ToString(CultureInfo.InvariantCulture));
                sb.Append("，软删合计(is_deleted=1)=").Append(scope.SoftDeleted.ToString(CultureInfo.InvariantCulture));
                sb.Append("，真正新增=").Append(scope.InsertCount.ToString(CultureInfo.InvariantCulture));
                sb.Append("，真正更新=").Append(scope.UpdateCount.ToString(CultureInfo.InvariantCulture));
                sb.Append("，未变=").Append(scope.UnchangedCount.ToString(CultureInfo.InvariantCulture));
                sb.Append("，本轮软删=").Append(scope.DeleteCount.ToString(CultureInfo.InvariantCulture));
                if (!string.IsNullOrWhiteSpace(scope.SoftDeletedKeys))
                {
                    sb.Append("，软删明细(id|工厂/物料/工作中心)=").Append(scope.SoftDeletedKeys);
                }
            }
            return sb.ToString();
        }
        if (legacyPairs.Count > 0)
        {
            var parts = legacyPairs
                .OrderBy(x => x.Key, StringComparer.OrdinalIgnoreCase)
                .Select(x => $"{x.Key}={x.Value.ToString(CultureInfo.InvariantCulture)}");
            return $"SQL 脚本执行成功，路径={path}，{string.Join("，", parts)}";
        }
        return $"SQL 脚本执行成功，路径={path}（无行数汇总结果集；请确认脚本末尾输出 summary_tag={TaktQuartzConstants.SqlSyncSummaryTag}）";
    }

    /// <summary>
    /// 将 scope 码转为中文标签
    /// </summary>
    private static string MapScopeLabel(string scope)
    {
        return scope.Trim().ToLowerInvariant() switch
        {
            "main" => "主表",
            "detail" => "子表",
            _ => scope.Trim(),
        };
    }

    /// <summary>
    /// 将分批 phase 转为中文
    /// </summary>
    /// <param name="phase">脚本 phase 列</param>
    /// <returns>展示名</returns>
    private static string MapProgressPhaseLabel(string phase)
    {
        return phase.Trim().ToLowerInvariant() switch
        {
            "start" => "开始",
            "loadstart" => "开始装入源表",
            "load" => "装入源表",
            "norm" => "键列规范化",
            "idmap" => "源键对齐",
            "merge" => "MERGE",
            "soft" => "软删",
            _ => string.IsNullOrWhiteSpace(phase) ? "进度" : phase.Trim(),
        };
    }

    /// <summary>
    /// 从结果集行格式化 QUARTZ_SYNC_PROGRESS
    /// </summary>
    /// <param name="scriptPath">脚本路径</param>
    /// <param name="reader">当前行</param>
    /// <returns>过程摘要</returns>
    private static string FormatProgressMessageFromReader(string? scriptPath, DbDataReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);
        return FormatProgressMessage(
            scriptPath,
            ReadString(reader, "phase"),
            HasColumn(reader, "from_rn") ? ReadInt(reader, "from_rn") : 0,
            HasColumn(reader, "to_rn") ? ReadInt(reader, "to_rn") : 0,
            HasColumn(reader, "max_rn") ? ReadInt(reader, "max_rn") : 0,
            HasColumn(reader, "batch_rows") ? ReadInt(reader, "batch_rows") : 0,
            HasColumn(reader, "scope") ? ReadString(reader, "scope") : string.Empty);
    }

    /// <summary>
    /// 格式化 QUARTZ_SYNC_PROGRESS（结果集与 RAISERROR InfoMessage 共用）
    /// </summary>
    /// <param name="scriptPath">脚本路径</param>
    /// <param name="phase">脚本 phase</param>
    /// <param name="fromRn">本批起始 rn</param>
    /// <param name="toRn">本批结束 rn</param>
    /// <param name="maxRn">最大 rn</param>
    /// <param name="batchRows">本批行数</param>
    /// <param name="scope">main/detail；空则不加标签</param>
    /// <returns>过程摘要</returns>
    private static string FormatProgressMessage(
        string? scriptPath,
        string phase,
        int fromRn,
        int toRn,
        int maxRn,
        int batchRows,
        string? scope)
    {
        var path = string.IsNullOrWhiteSpace(scriptPath) ? "(inline)" : scriptPath.Trim();
        var label = MapProgressPhaseLabel(phase);
        var scopeTag = FormatProgressScopeTag(scope);
        if (string.Equals(phase, "start", StringComparison.OrdinalIgnoreCase))
        {
            return "SQL 分批开始，路径=" + path;
        }
        if (string.Equals(phase, "loadstart", StringComparison.OrdinalIgnoreCase))
        {
            return "SQL 分批开始装入源表，路径=" + path + scopeTag;
        }
        if (string.Equals(phase, "load", StringComparison.OrdinalIgnoreCase))
        {
            return "SQL 分批装入源表，路径="
                + path
                + scopeTag
                + "，行数="
                + batchRows.ToString(CultureInfo.InvariantCulture);
        }
        if (maxRn > 0)
        {
            return "SQL 分批"
                + label
                + "，路径="
                + path
                + scopeTag
                + "，"
                + fromRn.ToString(CultureInfo.InvariantCulture)
                + "-"
                + toRn.ToString(CultureInfo.InvariantCulture)
                + "/"
                + maxRn.ToString(CultureInfo.InvariantCulture)
                + "，本批="
                + batchRows.ToString(CultureInfo.InvariantCulture);
        }
        return "SQL 分批"
            + label
            + "，路径="
            + path
            + scopeTag
            + "，累计="
            + toRn.ToString(CultureInfo.InvariantCulture)
            + "，本批="
            + batchRows.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 进度日志的 scope 标签（[主表]/[子表]）
    /// </summary>
    /// <param name="scope">脚本 scope 列</param>
    /// <returns>前导逗号+标签；空 scope 则空串</returns>
    private static string FormatProgressScopeTag(string? scope)
    {
        if (string.IsNullOrWhiteSpace(scope))
        {
            return string.Empty;
        }
        return "，[" + MapScopeLabel(scope) + "]";
    }

    /// <summary>
    /// 解析脚本 RAISERROR 进度（QUARTZ_SYNC_PROGRESS|phase|from|to|max|rows|scope）
    /// </summary>
    /// <param name="scriptPath">脚本路径</param>
    /// <param name="message">InfoMessage 文本</param>
    /// <param name="formatted">格式化后的过程摘要</param>
    /// <returns>可解析则为 true</returns>
    private static bool TryFormatProgressFromInfoMessage(
        string? scriptPath,
        string message,
        out string formatted)
    {
        formatted = string.Empty;
        if (string.IsNullOrWhiteSpace(message)
            || !message.StartsWith(TaktQuartzConstants.SqlSyncProgressInfoPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        var parts = message.Split('|');
        if (parts.Length < 6)
        {
            return false;
        }
        if (!int.TryParse(parts[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out var fromRn)
            || !int.TryParse(parts[3], NumberStyles.Integer, CultureInfo.InvariantCulture, out var toRn)
            || !int.TryParse(parts[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out var maxRn)
            || !int.TryParse(parts[5], NumberStyles.Integer, CultureInfo.InvariantCulture, out var batchRows))
        {
            return false;
        }
        var scope = parts.Length >= 7 ? parts[6] : string.Empty;
        formatted = FormatProgressMessage(scriptPath, parts[1], fromRn, toRn, maxRn, batchRows, scope);
        return true;
    }

    /// <summary>
    /// 订阅 SqlConnection.InfoMessage，使 RAISERROR WITH NOWAIT 在 ExecuteReader 阻塞期间即可写日志
    /// </summary>
    /// <param name="connection">当前连接</param>
    /// <param name="onMessage">原始 InfoMessage 文本</param>
    /// <returns>可释放订阅；非 SQL Server 连接则为 null</returns>
    private static IDisposable? AttachSqlInfoMessage(DbConnection connection, Action<string> onMessage)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(onMessage);
        var eventInfo = connection.GetType().GetEvent("InfoMessage");
        if (eventInfo?.EventHandlerType == null)
        {
            return null;
        }
        var handler = TryCreateInfoMessageHandler(eventInfo.EventHandlerType, onMessage);
        if (handler == null)
        {
            return null;
        }
        // 仅订阅 InfoMessage：RAISERROR severity=10（进度）本就会走 InfoMessage。
        // ❌ 禁止 FireInfoMessageEventOnUserErrors=true —— 会把 THROW/severity≤16 改成 InfoMessage，
        //    导致脚本中途失败仍被当成「执行成功、无 SUMMARY」。
        eventInfo.AddEventHandler(connection, handler);
        return new SqlInfoMessageSubscription(connection, eventInfo, handler);
    }

    /// <summary>
    /// 按连接上 InfoMessage 委托类型编译回调（兼容 Microsoft.Data.SqlClient / System.Data.SqlClient）
    /// </summary>
    /// <param name="handlerType">事件委托类型</param>
    /// <param name="onMessage">原始消息</param>
    /// <returns>事件处理委托；无法绑定时为 null</returns>
    private static Delegate? TryCreateInfoMessageHandler(Type handlerType, Action<string> onMessage)
    {
        var invoke = handlerType.GetMethod("Invoke");
        var parameters = invoke?.GetParameters();
        if (parameters is not { Length: 2 })
        {
            return null;
        }
        var messageProp = parameters[1].ParameterType.GetProperty("Message");
        if (messageProp == null)
        {
            return null;
        }
        var senderParam = Expression.Parameter(parameters[0].ParameterType, "sender");
        var argsParam = Expression.Parameter(parameters[1].ParameterType, "e");
        var messageAccess = Expression.Coalesce(
            Expression.Property(argsParam, messageProp),
            Expression.Constant(string.Empty));
        var body = Expression.Invoke(Expression.Constant(onMessage), messageAccess);
        return Expression.Lambda(handlerType, body, senderParam, argsParam).Compile();
    }

    /// <summary>
    /// 追加一段指标（第一段无前导逗号）
    /// </summary>
    /// <param name="sb">摘要缓冲</param>
    /// <param name="wrotePart">是否已写过指标</param>
    /// <param name="text">指标文案；空则跳过</param>
    private static void AppendScopePart(StringBuilder sb, ref bool wrotePart, string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return;
        }
        if (wrotePart)
        {
            sb.Append("，");
        }
        sb.Append(text);
        wrotePart = true;
    }

    /// <summary>
    /// 去前导 0 数量（全量回填脚本；含主表/明细拆分）
    /// </summary>
    /// <param name="scope">单 scope 汇总</param>
    /// <returns>指标文案；无对应列则 null</returns>
    private static string? FormatLeadingZeroPart(SyncScopeCounts scope)
    {
        if (!scope.LeadingZeroStrippedCount.HasValue
            && !scope.LeadingZeroHeaderCount.HasValue
            && !scope.LeadingZeroItemCount.HasValue)
        {
            return null;
        }
        var total = scope.LeadingZeroStrippedCount
            ?? checked((scope.LeadingZeroHeaderCount ?? 0) + (scope.LeadingZeroItemCount ?? 0));
        var text = "去前导=" + total.ToString(CultureInfo.InvariantCulture);
        if (scope.LeadingZeroHeaderCount.HasValue || scope.LeadingZeroItemCount.HasValue)
        {
            text += "（主表="
                + (scope.LeadingZeroHeaderCount ?? 0).ToString(CultureInfo.InvariantCulture)
                + "，明细="
                + (scope.LeadingZeroItemCount ?? 0).ToString(CultureInfo.InvariantCulture)
                + "）";
        }
        return text;
    }

    private static bool HasColumn(DbDataReader reader, string name)
    {
        for (var i = 0; i < reader.FieldCount; i++)
        {
            if (string.Equals(reader.GetName(i), name, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    private static string ReadString(DbDataReader reader, string column)
    {
        if (!HasColumn(reader, column) || reader[column] is DBNull or null)
        {
            return string.Empty;
        }
        return Convert.ToString(reader[column], CultureInfo.InvariantCulture)?.Trim() ?? string.Empty;
    }

    private static int ReadInt(DbDataReader reader, string column)
    {
        if (!HasColumn(reader, column) || reader[column] is DBNull or null)
        {
            return 0;
        }
        return Convert.ToInt32(reader[column], CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 单 scope 行数汇总
    /// </summary>
    private sealed class SyncScopeCounts
    {
        public string Scope { get; init; } = string.Empty;
        public bool HasMergeStats { get; init; }
        public int? SourceRawCount { get; init; }
        public int SourceCount { get; init; }
        public int? SkippedEmptyCount { get; init; }
        public int? LeadingZeroStrippedCount { get; init; }
        public int? LeadingZeroHeaderCount { get; init; }
        public int? LeadingZeroItemCount { get; init; }
        public int? LeadingZeroClashSkippedCount { get; init; }
        public int? ModelBackfilledCount { get; init; }
        public int? MaterialTypeBackfilledCount { get; init; }
        public int? ModelClashSkippedCount { get; init; }
        public int? AverageUpdatedCount { get; init; }
        public int? DedupeDroppedCount { get; init; }
        public int TargetBefore { get; init; }
        public int TargetAfter { get; init; }
        public int TargetPhysical { get; init; }
        public int SoftDeleted { get; init; }
        public int InsertCount { get; init; }
        public int UpdateCount { get; init; }
        public int UnchangedCount { get; init; }
        public int DeleteCount { get; init; }
        public string SoftDeletedKeys { get; init; } = string.Empty;
    }

    /// <summary>
    /// SqlConnection.InfoMessage 订阅释放器
    /// </summary>
    private sealed class SqlInfoMessageSubscription : IDisposable
    {
        private readonly object _connection;
        private readonly EventInfo _eventInfo;
        private readonly Delegate _handler;
        private bool _disposed;

        /// <summary>
        /// 创建订阅释放器
        /// </summary>
        /// <param name="connection">SQL 连接</param>
        /// <param name="eventInfo">InfoMessage 事件</param>
        /// <param name="handler">已绑定的委托</param>
        public SqlInfoMessageSubscription(object connection, EventInfo eventInfo, Delegate handler)
        {
            ArgumentNullException.ThrowIfNull(connection);
            ArgumentNullException.ThrowIfNull(eventInfo);
            ArgumentNullException.ThrowIfNull(handler);
            _connection = connection;
            _eventInfo = eventInfo;
            _handler = handler;
        }

        /// <summary>
        /// 取消 InfoMessage 订阅
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _eventInfo.RemoveEventHandler(_connection, _handler);
            _disposed = true;
        }
    }
}
