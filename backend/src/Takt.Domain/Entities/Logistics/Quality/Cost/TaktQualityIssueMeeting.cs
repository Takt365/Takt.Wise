// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Quality.Cost
// 文件名称：TaktQualityIssueMeeting.cs
// 创建时间：2026-05-07
// 创建人：Takt365(Qoder AI)
// 功能描述:品质问题应对明细 - 会议/调查/试验费用
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json;
using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Cost;

/// <summary>
/// 品质问题应对明细 - 会议/调查/试验费用
/// </summary>
[SugarTable("takt_logistics_quality_issue_meeting", "质量问题会议调查试验费用明细表")]
[SugarIndex("ix_quality_issue_meeting_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_quality_issue_meeting_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_issue_meeting_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(QualityIssueId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_issue_meeting_line_number", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, false)]
public class TaktQualityIssueMeeting : TaktCompanyEntityBase
{
    /// <summary>
    /// 品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "quality_issue_id", ColumnDescription = "品质问题主表ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIssueId { get; set; }

    /// <summary>
    /// 品质问题编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "quality_issue_code", ColumnDescription = "品质问题编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 直接人员费率（元/分钟）
    /// </summary>
    [SugarColumn(ColumnName = "direct_manpower_cost_per_minute", ColumnDescription = "直接人员费率", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal DirectManpowerCostPerMinute { get; set; } = 0;

    /// <summary>
    /// 间接人员费率（元/分钟）
    /// </summary>
    [SugarColumn(ColumnName = "indirect_manpower_cost_per_minute", ColumnDescription = "间接人员费率", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal IndirectManpowerCostPerMinute { get; set; } = 0;

    /// <summary>
    /// 讨论调查试验内容(会议记录)
    /// </summary>
    [SugarColumn(ColumnName = "meeting_investigation_content", ColumnDescription = "讨论调查试验内容", ColumnDataType = "ntext", IsNullable = true)]
    public string? MeetingInvestigationContent { get; set; }

    /// <summary>
    /// 讨论调查试验费用(元)
    /// </summary>
    [SugarColumn(ColumnName = "meeting_investigation_cost", ColumnDescription = "讨论调查试验费用", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal MeetingInvestigationCost { get; set; } = 0;

    /// <summary>
    /// 讨论会使用时间(分钟)
    /// </summary>
    [SugarColumn(ColumnName = "meeting_time_minutes", ColumnDescription = "检讨会使用时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int MeetingTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 直接人员参加人数
    /// </summary>
    [SugarColumn(ColumnName = "direct_participant_count", ColumnDescription = "直接人员参加人数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DirectParticipantCount { get; set; } = 0;

    /// <summary>
    /// 间接人员参加人数
    /// </summary>
    [SugarColumn(ColumnName = "indirect_participant_count", ColumnDescription = "间接人员参加人数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IndirectParticipantCount { get; set; } = 0;

    /// <summary>
    /// 调查评价试验工作时间（分钟）
    /// </summary>
    [SugarColumn(ColumnName = "investigation_work_time_minutes", ColumnDescription = "调查评价试验工作时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int InvestigationWorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 交通费、旅费（元）
    /// </summary>
    [SugarColumn(ColumnName = "travel_cost", ColumnDescription = "交通费旅费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TravelCost { get; set; } = 0;

    /// <summary>
    /// 其他费用（元）
    /// </summary>
    [SugarColumn(ColumnName = "other_expenses", ColumnDescription = "其他费用", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal OtherExpenses { get; set; } = 0;

    /// <summary>
    /// 其他作业時間（分钟）
    /// </summary>
    [SugarColumn(ColumnName = "other_work_time_minutes", ColumnDescription = "其他作业时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int OtherWorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 其他设备购入费、工程费、搬运费等（元）
    /// </summary>
    [SugarColumn(ColumnName = "other_apparatus_cost", ColumnDescription = "其他设备工程搬运费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal OtherApparatusCost { get; set; } = 0;

    /// <summary>
    /// 对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "response_group_code", ColumnDescription = "对应组编码", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? ResponseGroupCode { get; set; }

    /// <summary>
    /// 对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "response_group_name", ColumnDescription = "对应组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? ResponseGroupName { get; set; }


    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 质量问题主表（导航属性）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(QualityIssueId))]
    public TaktQualityIssue? Issue { get; set; }
}
