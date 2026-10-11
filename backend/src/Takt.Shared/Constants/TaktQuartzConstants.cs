// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktQuartzConstants.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 调度跨层共享常量（HTTP 客户端名等）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// Quartz 调度跨层共享常量
/// </summary>
public static class TaktQuartzConstants
{
    /// <summary>
    /// Quartz HTTP 任务 IHttpClientFactory 客户端名称
    /// </summary>
    public const string HttpClientName = "TaktQuartzHttp";

    /// <summary>
    /// 默认 Job 分组（字典 sys_quartz_job_group 默认项 default）
    /// </summary>
    public const string DefaultJobGroup = "default";

    /// <summary>
    /// JobGroup 列最大长度
    /// </summary>
    public const int MaxJobGroupLength = 40;

    /// <summary>
    /// Serilog 模块名（TaktLogContext.Module）
    /// </summary>
    public const string LogModuleName = "Quartz";

    /// <summary>
    /// 独立执行日志通道属性名（Filter → logs/quartz-/quartz-.log）
    /// </summary>
    public const string LogChannelPropertyName = "TaktLogChannel";

    /// <summary>
    /// 独立执行日志通道属性值
    /// </summary>
    public const string LogChannelValue = "QuartzJob";

    /// <summary>
    /// SQL 同步脚本结果集标识（与 Quartz/*.sql 末尾 SELECT summary_tag 一致）
    /// </summary>
    public const string SqlSyncSummaryTag = "QUARTZ_SYNC_SUMMARY";

    /// <summary>
    /// SQL 同步分批进度结果集标识（WHILE/MERGE 每批 SELECT；执行器即时写入 quartz 日志）
    /// </summary>
    public const string SqlSyncProgressTag = "QUARTZ_SYNC_PROGRESS";

    /// <summary>
    /// SQL 同步分批进度 InfoMessage 前缀（脚本 RAISERROR … WITH NOWAIT，避免 SET NOCOUNT 下 TDS 缓冲导致进度日志被整批积压）
    /// 格式：QUARTZ_SYNC_PROGRESS|phase|from_rn|to_rn|max_rn|batch_rows|scope
    /// </summary>
    public const string SqlSyncProgressInfoPrefix = "QUARTZ_SYNC_PROGRESS|";

    /// <summary>
    /// 非查询 SQL（含 sync_*.sql MERGE）默认命令超时秒数；0 表示无限制。
    /// 可由配置 Quartz:SqlCommandTimeoutSeconds 覆盖。
    /// </summary>
    public const int DefaultSqlCommandTimeoutSeconds = 7200;

    /// <summary>
    /// Quartz SQL 分批写入行数（MERGE/UPDATE TOP 循环；百万级表禁止单事务全量）
    /// </summary>
    public const int DefaultSqlApplyChunkRows = 20000;

    /// <summary>
    /// 任务执行完成落库消息类型（字典 sys_message_type DictValue）
    /// </summary>
    public const string ExecutedMessageType = "system";

    /// <summary>
    /// 任务执行完成落库消息分组（字典 sys_message_group DictValue）
    /// </summary>
    public const string ExecutedMessageGroup = "reminder";

    /// <summary>
    /// 无触发用户时的系统发送者登录名（与种子 admin / SystemAuditUser 对齐）
    /// </summary>
    public const string SystemSenderUserName = "admin";

    /// <summary>
    /// 规范化 JobGroup（空则 DEFAULT，超长截断至 40）
    /// </summary>
    /// <param name="value">原始 Job 分组</param>
    /// <returns>规范化后的 JobGroup</returns>
    public static string NormalizeJobGroup(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return DefaultJobGroup;
        }
        var trimmed = value.Trim();
        return trimmed.Length <= MaxJobGroupLength ? trimmed : trimmed[..MaxJobGroupLength];
    }
}
