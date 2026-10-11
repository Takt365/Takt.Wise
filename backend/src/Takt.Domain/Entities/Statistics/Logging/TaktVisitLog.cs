// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Statistics.Logging
// 文件名称：TaktVisitLog.cs
// 创建时间：2026-06-25
// 创建人：Takt365(Cursor AI)
// 功能描述：用户日访问量统计实体（与在线时长无关，认证成功时累加）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Statistics.Logging;

/// <summary>
/// 用户日访问量统计实体
/// </summary>
/// <remarks>
/// 租户+公司+用户+统计日唯一一行；VisitCount 在认证成功时 +1，与 TaktOnline 在线态、TaktDurationLog 时长无关。
/// </remarks>
[SugarTable("takt_statistics_logging_visit_log", "用户日访问量表")]
[SugarIndex("ix_visit_log_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_visit_log_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_visit_log_user_stat_date_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserId), OrderByType.Asc, nameof(StatDate), OrderByType.Asc, true)]
[SugarIndex("ix_takt_statistics_logging_visit_log_user_name", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserName), OrderByType.Asc, false)]
[SugarIndex("ix_takt_statistics_logging_visit_log_stat_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(StatDate), OrderByType.Desc, false)]
[SugarIndex("ix_takt_statistics_logging_visit_log_visit_count", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(VisitCount), OrderByType.Desc, false)]
public class TaktVisitLog : TaktCompanyEntityBase
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
    /// 当日访问次数（成功登录/进入系统次数）
    /// </summary>
    [SugarColumn(ColumnName = "visit_count", ColumnDescription = "当日访问次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int VisitCount { get; set; }
}
