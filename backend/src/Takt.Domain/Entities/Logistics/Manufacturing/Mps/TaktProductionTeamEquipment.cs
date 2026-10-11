// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Mps
// 文件名称：TaktProductionTeamEquipment.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：生产班组设备组明细（PCBA 线体 SMT/AI/手插等与生产设备主数据关联及台数）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Mps;

/// <summary>
/// 生产班组设备组明细（主子表；PCBA 线体生产设备及台数）
/// </summary>
[SugarTable("takt_logistics_manufacturing_mps_production_team_equipment", "生产班组设备组表")]
[SugarIndex("ix_production_team_equipment_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_production_team_equipment_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mps_production_team_equipment_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(ProdTeamId), OrderByType.Asc, nameof(ProdEquipId), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_mps_production_team_equipment_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdTeamId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, nameof(ProdEquipCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_mps_production_team_equipment_team_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TeamCode), OrderByType.Asc, false)]
public class TaktProductionTeamEquipment : TaktCompanyEntityBase
{
    /// <summary>
    /// 生产班组主键（主子表关系，关联 TaktProductionTeam.Id）
    /// </summary>
    [SugarColumn(ColumnName = "prod_team_id", ColumnDescription = "生产班组主键", ColumnDataType = "bigint", IsNullable = false)]
    public long ProdTeamId { get; set; }
    /// <summary>
    /// 班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）
    /// </summary>
    [SugarColumn(ColumnName = "team_code", ColumnDescription = "班组编码", ColumnDataType = "nvarchar", Length = 8, IsNullable = false)]
    public string TeamCode { get; set; } = string.Empty;
    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;
    /// <summary>
    /// 生产设备主键（关联 TaktProductionEquipment.Id）
    /// </summary>
    [SugarColumn(ColumnName = "prod_equip_id", ColumnDescription = "生产设备主键", ColumnDataType = "bigint", IsNullable = false)]
    public long ProdEquipId { get; set; }
    /// <summary>
    /// 生产设备编码（冗余快照，与 TaktProductionEquipment.ProdEquipCode 一致）
    /// </summary>
    [SugarColumn(ColumnName = "prod_equip_code", ColumnDescription = "生产设备编码", ColumnDataType = "nvarchar", Length = 18, IsNullable = false)]
    public string ProdEquipCode { get; set; } = string.Empty;
    /// <summary>
    /// 设备台数（同型号多台时 &gt;1）
    /// </summary>
    [SugarColumn(ColumnName = "equip_quantity", ColumnDescription = "设备台数", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int EquipQuantity { get; set; } = 1;
    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "team_equip_status", ColumnDescription = "班组设备状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int TeamEquipStatus { get; set; } = 1;
    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;
}
