// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Quality
// 文件名称：TaktSamplingScheme.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt抽样方案实体，定义抽样方案领域模型
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Operation;

/// <summary>
/// Takt抽样方案实体
/// </summary>
[SugarTable("takt_logistics_quality_sampling_scheme", "抽样方案表")]
[SugarIndex("ix_sampling_scheme_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sampling_scheme_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_sampling_scheme_ss_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(SamplingSchemeCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_sampling_scheme_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_sampling_scheme_inspection_level", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(InspectionLevel), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_sampling_scheme_sampling_scheme_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SamplingSchemeStatus), OrderByType.Asc, false)]
public class TaktSamplingScheme : TaktCompanyEntityBase
{
    /// <summary>
    /// 抽样方案编码（唯一索引）
    /// </summary>
    [SugarColumn(ColumnName = "sampling_scheme_code", ColumnDescription = "抽样方案编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string SamplingSchemeCode { get; set; } = string.Empty;
    /// <summary>
    /// 抽样方案名称
    /// </summary>
    [SugarColumn(ColumnName = "sampling_scheme_name", ColumnDescription = "抽样方案名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string SamplingSchemeName { get; set; } = string.Empty;
    /// <summary>
    /// 抽样方案类型（字典 logistics_quality_sampling_scheme_type）
    /// </summary>
    [SugarColumn(ColumnName = "sampling_scheme_type", ColumnDescription = "抽样方案类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SamplingSchemeType { get; set; } = 0;
    /// <summary>
    /// 抽样标准（字典 logistics_quality_sampling_standard）
    /// </summary>
    [SugarColumn(ColumnName = "sampling_standard", ColumnDescription = "抽样标准", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SamplingStandard { get; set; } = 0;
    /// <summary>
    /// 检验水平（字典 logistics_quality_inspection_level）
    /// </summary>
    [SugarColumn(ColumnName = "inspection_level", ColumnDescription = "检验水平", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int InspectionLevel { get; set; } = 1;
    /// <summary>
    /// AQL值（可接受质量水平，0.010-1000，存储为小数）
    /// </summary>
    [SugarColumn(ColumnName = "aql_value", ColumnDescription = "AQL值", ColumnDataType = "decimal", Length = 10, DecimalDigits = 3, IsNullable = false, DefaultValue = "2.5")]
    public decimal AqlValue { get; set; } = 2.5m;
    /// <summary>
    /// 批量范围最小值
    /// </summary>
    [SugarColumn(ColumnName = "lot_size_min", ColumnDescription = "批量范围最小值", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LotSizeMin { get; set; } = 0;
    /// <summary>
    /// 批量范围最大值（0表示无上限）
    /// </summary>
    [SugarColumn(ColumnName = "lot_size_max", ColumnDescription = "批量范围最大值", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LotSizeMax { get; set; } = 0;
    /// <summary>
    /// 样本量（抽样数量）
    /// </summary>
    [SugarColumn(ColumnName = "sample_size", ColumnDescription = "样本量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SampleSize { get; set; } = 0;
    /// <summary>
    /// 接收数（Ac，Acceptance Number）
    /// </summary>
    [SugarColumn(ColumnName = "acceptance_number", ColumnDescription = "接收数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int AcceptanceNumber { get; set; } = 0;
    /// <summary>
    /// 拒收数（Re，Rejection Number）
    /// </summary>
    [SugarColumn(ColumnName = "rejection_number", ColumnDescription = "拒收数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int RejectionNumber { get; set; } = 0;
    /// <summary>
    /// 检验严格度（字典 logistics_quality_inspection_strictness）
    /// </summary>
    [SugarColumn(ColumnName = "inspection_strictness", ColumnDescription = "检验严格度", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int InspectionStrictness { get; set; } = 0;
    /// <summary>
    /// 是否支持转移规则（0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_transfer_rule_enabled", ColumnDescription = "是否支持转移规则", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsTransferRuleEnabled { get; set; } = 0;
    /// <summary>
    /// 转移规则配置（JSON格式，存储正常/加严/放宽检验的转移条件）
    /// </summary>
    [SugarColumn(ColumnName = "transfer_rule_config", ColumnDescription = "转移规则配置", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? TransferRuleConfig { get; set; }
    /// <summary>
    /// 抽样方案描述
    /// </summary>
    [SugarColumn(ColumnName = "scheme_description", ColumnDescription = "抽样方案描述", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? SchemeDescription { get; set; }
    /// <summary>
    /// 抽样方案状态（字典 logistics_quality_standard_status）
    /// </summary>
    [SugarColumn(ColumnName = "sampling_scheme_status", ColumnDescription = "抽样方案状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SamplingSchemeStatus { get; set; } = 0;

    /// <summary>
    /// 检验标准列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktInspectionStandard.SamplingSchemeCode), nameof(SamplingSchemeCode))]
    public List<TaktInspectionStandard>? InspectionStandards { get; set; }
}
