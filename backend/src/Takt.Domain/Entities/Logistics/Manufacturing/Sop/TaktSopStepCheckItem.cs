#nullable enable
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Sop
// 文件名称：TaktSopStepCheckItem.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP 工步检验项目定义
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP 工步检验项目实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_sop_step_check_item", "SOP工步检验项目表")]
[SugarIndex("ix_sop_step_check_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sop_step_check_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_step_check_item_step", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(StepId), OrderByType.Asc, false)]
public class TaktSopStepCheckItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "step_id", ColumnDescription = "工步ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StepId { get; set; }

    /// <summary>
    /// 检验项目名称
    /// </summary>
    [SugarColumn(ColumnName = "check_item_name", ColumnDescription = "检验项目名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string CheckItemName { get; set; } = string.Empty;

    /// <summary>
    /// 检验方法
    /// </summary>
    [SugarColumn(ColumnName = "check_method", ColumnDescription = "检验方法", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? CheckMethod { get; set; }

    /// <summary>
    /// 检验标准
    /// </summary>
    [SugarColumn(ColumnName = "check_standard", ColumnDescription = "检验标准", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? CheckStandard { get; set; }

    /// <summary>
    /// 是否必检（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_required", ColumnDescription = "是否必检", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsRequired { get; set; } = 1;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 工步
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(StepId))]
    public TaktSopStep? Step { get; set; }
}
