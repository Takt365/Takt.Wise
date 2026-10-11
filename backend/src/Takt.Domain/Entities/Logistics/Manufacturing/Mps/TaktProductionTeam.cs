#nullable enable
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Mps
// 文件名称：TaktProductionTeam.cs
// 创建时间：2026-03-16
// 创建人：Takt365(Cursor AI)
// 功能描述：生产班组实体，用于替代 team_code_category 字典管理生产班组主数据
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Mps;

/// <summary>
/// 生产班组实体（生产线班组主数据）
/// </summary>
[SugarTable("takt_logistics_manufacturing_mps_production_team", "生产班组表")]
[SugarIndex("ix_production_team_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_production_team_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_mps_production_team_team_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(TeamCode), OrderByType.Asc, nameof(TeamCategory), OrderByType.Asc, true)]
public class TaktProductionTeam : TaktCompanyEntityBase
{

    /// <summary>
    /// 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
    /// </summary>
    [SugarColumn(ColumnName = "team_code", ColumnDescription = "班组编码", ColumnDataType = "nvarchar", Length = 8, IsNullable = false)]
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 班组名称（显示名称，如：SMT一班、手插二班等）
    /// </summary>
    [SugarColumn(ColumnName = "team_name", ColumnDescription = "班组名称", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string TeamName { get; set; } = string.Empty;

    /// <summary>
    /// 班组分类（字典 logistics_manufacturing_team_category；存 DictValue；A=组立 P=PCBA Q=质检 O=其他；PCBA 线体如 SMT/AI/手插须维护设备组）
    /// </summary>
    [SugarColumn(ColumnName = "team_category", ColumnDescription = "班组分类编码", ColumnDataType = "nvarchar", Length = 2, IsNullable = false, DefaultValue = "A")]
    public string TeamCategory { get; set; } = "A";

    /// <summary>
    /// 班组长（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "team_leader_employee_id", ColumnDescription = "班组长ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TeamLeaderEmployeeId { get; set; }
    /// <summary>
    /// 班组长名称（冗余：按 TeamLeaderEmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "team_leader_name", ColumnDescription = "班组长名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? TeamLeaderName { get; set; }

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
    /// </summary>
    [SugarColumn(ColumnName = "shift_no", ColumnDescription = "班次", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ShiftNo { get; set; } = 1;

    /// <summary>
    /// 启用状态（字典 sys_normal_disable；0=禁用，1=启用）
    /// </summary>
    [SugarColumn(ColumnName = "team_status", ColumnDescription = "启用状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int TeamStatus { get; set; } = 1;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 设备组明细（PCBA 线体 SMT/AI/手插等生产设备及台数）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktProductionTeamEquipment.ProdTeamId))]
    public List<TaktProductionTeamEquipment> TeamEquipmentList { get; set; } = new();

}

