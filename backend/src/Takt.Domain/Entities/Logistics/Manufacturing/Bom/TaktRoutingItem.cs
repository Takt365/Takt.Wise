#nullable enable
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingItem.cs
// 创建时间：2026-05-12
// 创建人：Takt365(Cursor AI)
// 功能描述：工艺路线明细表实体，定义工艺路线的工序序列
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Bom;

/// <summary>
/// 工艺路线明细表实体（工序序列）
/// </summary>
[SugarTable("takt_logistics_manufacturing_bom_routing_item", "工艺路线明细表")]
[SugarIndex("ix_routing_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_routing_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RoutingId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
public class TaktRoutingItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 工艺路线主表ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "routing_id", ColumnDescription = "工艺路线ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingId { get; set; }

    /// <summary>
    /// 工艺路线编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "routing_code", ColumnDescription = "工艺路线编码", ColumnDataType = "nvarchar", Length = 8, IsNullable = false)]
    public string RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 作业/工序计量单位（字典 logistics_materials_unit_of_measure_code；DictValue=PC/EA 等；默认 PC）
    /// </summary>
    [SugarColumn(ColumnName = "base_unit", ColumnDescription = "计量单位", ColumnDataType = "nvarchar", Length = 10, IsNullable = false, DefaultValue = "PC")]
    public string BaseUnit { get; set; } = "PC";

    /// <summary>
    /// 基本数量
    /// </summary>
    [SugarColumn(ColumnName = "base_quantity", ColumnDescription = "基本数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "1")]
    public decimal BaseQuantity { get; set; } = 1;

    /// <summary>
    /// 标准工时（分钟）
    /// </summary>
    [SugarColumn(ColumnName = "standard_minutes", ColumnDescription = "标准工时", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal StandardMinutes { get; set; } = 0;

    /// <summary>
    /// 工时单位（字典 logistics_manufacturing_time_unit；DictValue=MIN/H/S；MIN=分钟，H=小时，S=秒；默认 MIN）
    /// </summary>
    [SugarColumn(ColumnName = "time_unit", ColumnDescription = "工时单位", ColumnDataType = "nvarchar", Length = 3, IsNullable = false, DefaultValue = "MIN")]
    public string TimeUnit { get; set; } = "MIN";

    /// <summary>
    /// 标准点数
    /// </summary>
    [SugarColumn(ColumnName = "standard_shorts", ColumnDescription = "标准点数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int StandardShorts { get; set; } = 0;

    /// <summary>
    /// 点数单位（字典 logistics_manufacturing_points_unit；DictValue=SHORT；SHORT=点数；默认 SHORT）
    /// </summary>
    [SugarColumn(ColumnName = "points_unit", ColumnDescription = "点数单位", ColumnDataType = "nvarchar", Length = 5, IsNullable = false, DefaultValue = "SHORT")]
    public string PointsUnit { get; set; } = "SHORT";

    /// <summary>
    /// 点数转分钟汇率（字典 logistics_manufacturing_points_to_minutes_rate；DictValue=1/0.028/0.045；普通=1，AI=0.028，SMT=0.045；ConvertedMinutes = StandardShorts × rate ÷ BaseQuantity）
    /// </summary>
    [SugarColumn(ColumnName = "points_to_minutes_rate", ColumnDescription = "转换汇率", ColumnDataType = "nvarchar", Length = 10, IsNullable = false, DefaultValue = "1")]
    public string PointsToMinutesRate { get; set; } = "1";

    /// <summary>
    /// 转换后标准工时（分钟）
    /// </summary>
    [SugarColumn(ColumnName = "converted_minutes", ColumnDescription = "转换工时", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ConvertedMinutes { get; set; } = 0;

    /// <summary>
    /// 准备时间（分钟），如换模、调试等
    /// </summary>
    [SugarColumn(ColumnName = "setup_minutes", ColumnDescription = "准备时间", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal SetupMinutes { get; set; } = 0;

    /// <summary>
    /// 清理时间（分钟），如清洁、整理等
    /// </summary>
    [SugarColumn(ColumnName = "teardown_minutes", ColumnDescription = "清理时间", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TeardownMinutes { get; set; } = 0;

    /// <summary>
    /// 检验（字典 sys_yes_no：0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_inspection", ColumnDescription = "检验", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsInspection { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 工序说明
    /// </summary>
    [SugarColumn(ColumnName = "process_description", ColumnDescription = "工序说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ProcessDescription { get; set; }

    /// <summary>
    /// 工艺段类型（字典 logistics_manufacturing_process_segment_type：1=SMT，2=自插，3=手插，4=修正，5=总装）
    /// </summary>
    [SugarColumn(ColumnName = "process_segment_type", ColumnDescription = "工艺段类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ProcessSegmentType { get; set; } = 1;

    /// <summary>
    /// 工序扩展 JSON（五段工艺差异化参数，如钢网/Feeder/扭矩/烙铁温度）
    /// </summary>
    [SugarColumn(ColumnName = "ext_json", ColumnDescription = "工序扩展JSON", ColumnDataType = "nvarchar", Length = 4000, IsNullable = true)]
    public string? ExtJson { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 工艺路线主表（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(RoutingId))]
    public TaktRouting? Routing { get; set; }

    /// <summary>
    /// 工序参数定义
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktRoutingItemArgument.RoutingItemId))]
    public List<TaktRoutingItemArgument>? Arguments { get; set; }
}
