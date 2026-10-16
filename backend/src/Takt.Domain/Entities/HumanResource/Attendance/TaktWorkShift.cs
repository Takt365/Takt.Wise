// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Attendance
// 文件名称：TaktWorkShift.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Cursor AI)
// 功能描述：班次定义实体（排班管理-班次库）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Attendance;

/// <summary>
/// 班次定义（如早班、中班、夜班）
/// </summary>
[SugarTable("takt_human_resource_attendance_work_shift", "班次信息表")]
[SugarIndex("ix_work_shift_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_work_shift_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_work_shift_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ShiftCode), OrderByType.Asc, true)]
public class TaktWorkShift : TaktCompanyEntityBase
{
    /// <summary>
    /// 班次编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 班次编码规则生成并展示，非手输；单据类型菜单：班次管理）
    /// </summary>
    [SugarColumn(ColumnName = "shift_code", ColumnDescription = "班次编码", ColumnDataType = "nvarchar", Length = 64, IsNullable = false)]
    public string ShiftCode { get; set; } = string.Empty;
    /// <summary>
    /// 班次名称
    /// </summary>
    [SugarColumn(ColumnName = "shift_name", ColumnDescription = "班次名称", ColumnDataType = "nvarchar", Length = 128, IsNullable = false)]
    public string ShiftName { get; set; } = string.Empty;
    /// <summary>
    /// 当班开始时间（HH:mm）
    /// </summary>
    [SugarColumn(ColumnName = "start_time", ColumnDescription = "开始时间", ColumnDataType = "nvarchar", Length = 8, IsNullable = false)]
    public string StartTime { get; set; } = string.Empty;
    /// <summary>
    /// 当班结束时间（HH:mm）
    /// </summary>
    [SugarColumn(ColumnName = "end_time", ColumnDescription = "结束时间", ColumnDataType = "nvarchar", Length = 8, IsNullable = false)]
    public string EndTime { get; set; } = string.Empty;
    /// <summary>
    /// 是否跨自然日（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "cross_midnight", ColumnDescription = "是否跨日", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CrossMidnight { get; set; }
    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; }
}
