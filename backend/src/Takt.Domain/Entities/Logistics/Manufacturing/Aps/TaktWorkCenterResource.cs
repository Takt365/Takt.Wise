// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Aps
// 文件名称：TaktWorkCenterResource.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：工作中心资源，设备/人员/模具等可排程资源
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Aps;

/// <summary>
/// 工作中心资源（设备/人员/模具等）
/// </summary>
[SugarTable("takt_logistics_manufacturing_aps_work_center_resource", "工作中心资源表")]
[SugarIndex("ix_work_center_resource_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_work_center_resource_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_work_center_resource_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(WorkCenterId), OrderByType.Asc, nameof(ResourceCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_work_center_resource_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktWorkCenterResource : TaktCompanyEntityBase
{
    /// <summary>
    /// 工作中心 ID（主子表关系，关联 TaktWorkCenter.Id，选项 TaktWorkCenters/options）
    /// </summary>
    [SugarColumn(ColumnName = "work_center_id", ColumnDescription = "工作中心ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WorkCenterId { get; set; }


    /// <summary>
    /// 工作中心编码（关联 TaktWorkCenter.WorkCenterCode，冗余；选项 TaktWorkCenters/options，DictValue=WorkCenterCode）
    /// </summary>
    [SugarColumn(ColumnName = "work_center_code", ColumnDescription = "工作中心编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string WorkCenterCode { get; set; } = string.Empty;

    /// <summary>
    /// 资源编码（前端表单选择编码规则后自动通过 TaktNumbering 资源编码规则生成并展示，非手输；单据类型菜单：工作中心）
    /// </summary>
    [SugarColumn(ColumnName = "resource_code", ColumnDescription = "资源编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string ResourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 资源名称
    /// </summary>
    [SugarColumn(ColumnName = "resource_name", ColumnDescription = "资源名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ResourceName { get; set; } = string.Empty;

    /// <summary>
    /// 资源类型（字典 work_center_resource_type；0=设备，1=人员，2=模具）
    /// </summary>
    [SugarColumn(ColumnName = "resource_type", ColumnDescription = "资源类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ResourceType { get; set; } = 0;

    /// <summary>
    /// 并行能力（可同时加工任务数）
    /// </summary>
    [SugarColumn(ColumnName = "parallel_capacity", ColumnDescription = "并行能力", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ParallelCapacity { get; set; } = 1;

    /// <summary>
    /// 效率系数（1.0=标准）
    /// </summary>
    [SugarColumn(ColumnName = "efficiency_rate", ColumnDescription = "效率系数", ColumnDataType = "decimal", Length = 8, DecimalDigits = 4, IsNullable = false, DefaultValue = "1")]
    public decimal EfficiencyRate { get; set; } = 1;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "resource_status", ColumnDescription = "资源状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ResourceStatus { get; set; } = 1;
}
