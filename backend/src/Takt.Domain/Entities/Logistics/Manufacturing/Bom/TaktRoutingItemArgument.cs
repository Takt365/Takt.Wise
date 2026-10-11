#nullable enable
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingItemArgument.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：工艺路线工序参数定义（工艺定义层 ParamDef，非 ESOP 实体）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Bom;

/// <summary>
/// 工艺路线工序参数定义实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_bom_routing_item_parameter", "工艺路线工序参数表")]
[SugarIndex("ix_routing_item_parameter_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_routing_item_parameter_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_routing_item_parameter_item", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RoutingItemId), OrderByType.Asc, false)]
public class TaktRoutingItemArgument : TaktCompanyEntityBase
{
    /// <summary>
    /// 工艺路线明细 ID（序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "routing_item_id", ColumnDescription = "工序ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemId { get; set; }

    /// <summary>
    /// 参数编码
    /// </summary>
    [SugarColumn(ColumnName = "param_code", ColumnDescription = "参数编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string ParamCode { get; set; } = string.Empty;

    /// <summary>
    /// 参数名称
    /// </summary>
    [SugarColumn(ColumnName = "param_name", ColumnDescription = "参数名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string ParamName { get; set; } = string.Empty;

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    [SugarColumn(ColumnName = "param_unit", ColumnDescription = "单位", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? ParamUnit { get; set; }

    /// <summary>
    /// 标准值
    /// </summary>
    [SugarColumn(ColumnName = "standard_value", ColumnDescription = "标准值", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = true)]
    public decimal? StandardValue { get; set; }

    /// <summary>
    /// 下限
    /// </summary>
    [SugarColumn(ColumnName = "lower_limit", ColumnDescription = "下限", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = true)]
    public decimal? LowerLimit { get; set; }

    /// <summary>
    /// 上限
    /// </summary>
    [SugarColumn(ColumnName = "upper_limit", ColumnDescription = "上限", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = true)]
    public decimal? UpperLimit { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 工序
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(RoutingItemId))]
    public TaktRoutingItem? RoutingItem { get; set; }
}
