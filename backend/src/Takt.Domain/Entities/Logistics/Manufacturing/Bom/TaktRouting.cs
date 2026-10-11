#nullable enable
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Bom
// 文件名称：TaktRouting.cs
// 创建时间：2026-05-12
// 创建人：Takt365(Cursor AI)
// 功能描述：工艺路线主表实体，定义产品的工序流程
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Bom;

/// <summary>
/// 工艺路线主表实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_bom_routing", "工艺路线主表")]
[SugarIndex("ix_routing_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_routing_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_routing_plant_code_version_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(RoutingCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_routing_approval_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ApprovalStatus), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_routing_material_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_routing_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_routing_routing_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(RoutingStatus), OrderByType.Asc, false)]
public class TaktRouting : TaktApprovalEntityBase
{

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "work_center", ColumnDescription = "工作中心", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string WorkCenter { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    [SugarColumn(ColumnName = "routing_code", ColumnDescription = "工艺路线编码", ColumnDataType = "nvarchar", Length = 8, IsNullable = false)]
    public string RoutingCode { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线名称
    /// </summary>
    [SugarColumn(ColumnName = "routing_name", ColumnDescription = "工艺路线名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string RoutingName { get; set; } = string.Empty;

    /// <summary>
    /// 工艺路线组编码（选项 TaktBomGroups/options?bomGroupCategory=2；DictValue=BomGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "routing_group_code", ColumnDescription = "工艺路线组编码", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? RoutingGroupCode { get; set; }

    /// <summary>
    /// 工艺路线组名称（冗余：按 RoutingGroupCode 取 TaktBomGroups.BomGroupName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "routing_group_name", ColumnDescription = "工艺路线组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? RoutingGroupName { get; set; }

    /// <summary>
    /// 用途（字典 logistics_manufacturing_routing_purpose：1=生产，2=工程/设计，3=万能，4=工厂维护）
    /// </summary>
    [SugarColumn(ColumnName = "purpose", ColumnDescription = "用途", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int Purpose { get; set; } = 1;

    /// <summary>
    /// 适用物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 版本号
    /// </summary>
    [SugarColumn(ColumnName = "version", ColumnDescription = "版本号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false, DefaultValue = "V1.0")]
    public string Version { get; set; } = "V1.0";

    /// <summary>
    /// 状态（字典 logistics_manufacturing_routing_status：1=生成的，2=对订单下达，3=对成本核算下达，4=下达的）
    /// </summary>
    [SugarColumn(ColumnName = "routing_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "4")]
    public int RoutingStatus { get; set; } = 4;

    /// <summary>
    /// 生效日期
    /// </summary>
    [SugarColumn(ColumnName = "effective_date", ColumnDescription = "生效日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期
    /// </summary>
    [SugarColumn(ColumnName = "expiry_date", ColumnDescription = "失效日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 工艺路线说明
    /// </summary>
    [SugarColumn(ColumnName = "routing_description", ColumnDescription = "工艺路线说明", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? RoutingDescription { get; set; }

    /// <summary>
    /// 工艺路线明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktRoutingItem.RoutingId))]
    public List<TaktRoutingItem>? Items { get; set; }
}
