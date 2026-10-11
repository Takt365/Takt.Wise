#nullable enable
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Sop
// 文件名称：TaktSopEsdCheck.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP 工位 ESD 防静电检查（未达标锁屏）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP ESD 检查实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_sop_esd_check", "SOP ESD检查表")]
[SugarIndex("ix_sop_esd_check_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sop_esd_check_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_esd_check_workstation", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(WorkstationId), OrderByType.Asc, false)]
public class TaktSopEsdCheck : TaktCompanyEntityBase
{

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "workstation_id", ColumnDescription = "工位ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WorkstationId { get; set; }

    /// <summary>
    /// 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "exec_id", ColumnDescription = "执行追溯ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecId { get; set; }

    /// <summary>
    /// 员工 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "员工ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }
    /// <summary>
    /// 员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "employee_name", ColumnDescription = "员工名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? EmployeeName { get; set; }

    /// <summary>
    /// 监测设备编码
    /// </summary>
    [SugarColumn(ColumnName = "device_code", ColumnDescription = "监测设备编码", ColumnDataType = "nvarchar", Length = 18, IsNullable = true)]
    public string? DeviceCode { get; set; }

    /// <summary>
    /// 阻值（兆欧）
    /// </summary>
    [SugarColumn(ColumnName = "resistance_value", ColumnDescription = "阻值兆欧", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = true)]
    public decimal? ResistanceValue { get; set; }

    /// <summary>
    /// 达标（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_compliant", ColumnDescription = "达标", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsCompliant { get; set; } = 1;

    /// <summary>
    /// 锁屏（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "lock_screen_triggered", ColumnDescription = "锁屏", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LockScreenTriggered { get; set; } = 0;

    /// <summary>
    /// 检查时间
    /// </summary>
    [SugarColumn(ColumnName = "checked_at", ColumnDescription = "检查时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime CheckedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 工位
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(WorkstationId))]
    public TaktSopWorkstation? Workstation { get; set; }
}
