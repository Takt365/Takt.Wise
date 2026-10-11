#nullable enable
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Sop
// 文件名称：TaktSopWorkstation.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP 工位主数据（产线工位编码、工作中心、工艺段与启用状态）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP 工位主数据实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_sop_workstation", "SOP工位主数据表")]
[SugarIndex("ix_sop_workstation_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sop_workstation_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_workstation_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(WorkstationCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_workstation_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(WorkstationStatus), OrderByType.Asc, false)]
public class TaktSopWorkstation : TaktCompanyEntityBase
{

    /// <summary>
    /// 工位编码（工厂内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "workstation_code", ColumnDescription = "工位编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string WorkstationCode { get; set; } = string.Empty;

    /// <summary>
    /// 工位名称
    /// </summary>
    [SugarColumn(ColumnName = "workstation_name", ColumnDescription = "工位名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string WorkstationName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    [SugarColumn(ColumnName = "work_center", ColumnDescription = "工作中心", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? WorkCenter { get; set; }

    /// <summary>
    /// 生产班组
    /// </summary>
    [SugarColumn(ColumnName = "production_line", ColumnDescription = "生产班组", ColumnDataType = "nvarchar", Length = 8, IsNullable = true)]
    public string? ProductionLine { get; set; }

    /// <summary>
    /// 工位类型（字典 sys_workstation_type；1=装配，2=检验，3=包装，4=测试，5=其他）
    /// </summary>
    [SugarColumn(ColumnName = "workstation_type", ColumnDescription = "工位类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int WorkstationType { get; set; } = 1;

    /// <summary>
    /// 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
    /// </summary>
    [SugarColumn(ColumnName = "process_segment_type", ColumnDescription = "工艺段类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ProcessSegmentType { get; set; } = 1;

    /// <summary>
    /// 启用状态（字典 sys_normal_disable；0=禁用，1=启用，2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "workstation_status", ColumnDescription = "启用状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int WorkstationStatus { get; set; } = 1;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;
}
