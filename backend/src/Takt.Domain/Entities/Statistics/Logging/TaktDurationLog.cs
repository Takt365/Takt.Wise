// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Statistics.Logging
// 文件名称：TaktDurationLog.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：在线时长日志实体，按自然日汇总登录用户在线秒数（统计日志域）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Statistics.Logging;

/// <summary>
/// 在线时长日志实体（日汇总）
/// </summary>
/// <remarks>
/// 租户+公司+用户+统计日唯一一行；DurationSeconds 由 TaktOnlineService 会话/Heartbeat 维护。
/// 访问量见独立实体 TaktVisitLog，与本表无关。
/// </remarks>
[SugarTable("takt_statistics_logging_duration_log", "在线时长日志表")]
[SugarIndex("ix_duration_log_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_duration_log_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_duration_log_user_stat_date_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserId), OrderByType.Asc, nameof(StatDate), OrderByType.Asc, true)]
[SugarIndex("ix_takt_statistics_logging_duration_log_user_name", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserName), OrderByType.Asc, false)]
[SugarIndex("ix_takt_statistics_logging_duration_log_stat_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(StatDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_statistics_logging_duration_log_duration_seconds", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(DurationSeconds), OrderByType.Desc, false)]
public class TaktDurationLog : TaktCompanyEntityBase
{
    /// <summary>
    /// 用户 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户名", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 统计日期（自然日，不含时分秒）
    /// </summary>
    [SugarColumn(ColumnName = "stat_date", ColumnDescription = "统计日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime StatDate { get; set; }

    /// <summary>
    /// 当日累计在线时长（秒）
    /// </summary>
    [SugarColumn(ColumnName = "duration_seconds", ColumnDescription = "当日在线时长秒数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DurationSeconds { get; set; }
}
