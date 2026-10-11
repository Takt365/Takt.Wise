// ========================================
// 项目名称:节拍智控 ·TaktWise (TDF)
// 命名空间:Takt.Domain.Entities.Logistics.Quality.Cost
// 文件名称:TaktQualityAssuranceCalibration.cs
// 创建时间:2026-05-08
// 创建人:Takt365(Qoder AI)
// 功能描述:品质业务明细 - 测定器校正费用
//
// 版权信息:Copyright (c) 2025 Takt  All rights reserved.
// 免责声明:此软件使用 MIT License,作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json;
using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Cost;

/// <summary>
/// 品质业务明细 - 测定器校正费用
/// </summary>
[SugarTable("takt_logistics_quality_assurance_calibration", "品质业务设备校正费用明细表")]
[SugarIndex("ix_quality_assurance_calibration_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_quality_assurance_calibration_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_assurance_calibration_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(QualityAssuranceId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_assurance_calibration_line_number", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, false)]
public class TaktQualityAssuranceCalibration : TaktCompanyEntityBase
{
    /// <summary>
    /// 品质业务主表 ID（选项 TaktQualityAssurances/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "quality_assurance_id", ColumnDescription = "品质业务主表ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityAssuranceId { get; set; }

    /// <summary>
    /// 品质业务编码（冗余字段,便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "quality_assurance_code", ColumnDescription = "品质业务编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string QualityAssuranceCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 测定器校正业务费用(元)
    /// </summary>
    [SugarColumn(ColumnName = "calibration_cost", ColumnDescription = "测定器校正业务费用", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal CalibrationCost { get; set; } = 0;

    /// <summary>
    /// 校正作业时间(分钟)
    /// </summary>
    [SugarColumn(ColumnName = "work_time_minutes", ColumnDescription = "校正作业时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int WorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 外部委托费、运搬费(元)
    /// </summary>
    [SugarColumn(ColumnName = "external_agent_service_fee", ColumnDescription = "外部委托费运搬费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ExternalAgentServiceFee { get; set; } = 0;

    /// <summary>
    /// 校正其他费用(元)
    /// </summary>
    [SugarColumn(ColumnName = "other_expenses", ColumnDescription = "校正其他费用", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal OtherExpenses { get; set; } = 0;

    /// <summary>
    /// 校正备注
    /// </summary>
    [SugarColumn(ColumnName = "calibration_note", ColumnDescription = "校正备注", ColumnDataType = "ntext", IsNullable = true)]
    public string? CalibrationNote { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 品质业务主表(导航属性)
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(QualityAssuranceId))]
    public TaktQualityAssurance? Operation { get; set; }
}
