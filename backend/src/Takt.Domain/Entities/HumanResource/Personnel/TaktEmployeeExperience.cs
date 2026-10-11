// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Personnel
// 文件名称：TaktEmployeeExperience.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：员工外部工作经历实体（入职前履历）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.HumanResource.Personnel;

/// <summary>
/// 员工外部工作经历
/// </summary>
[SugarTable("takt_human_resource_personnel_employee_experience", "员工工作经历表")]
[SugarIndex("ix_employee_experience_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_employee_experience_employee", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, false)]
public class TaktEmployeeExperience : TaktCompanyEntityBase
{
    /// <summary>
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "员工ID", ColumnDataType = "bigint", IsNullable = false)]
    public long EmployeeId { get; set; }
    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "employee_code", ColumnDescription = "员工编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string EmployeeCode { get; set; } = string.Empty;
    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "employee_name", ColumnDescription = "员工姓名", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string EmployeeName { get; set; } = string.Empty;
    /// <summary>
    /// 工作单位名称
    /// </summary>
    [SugarColumn(ColumnName = "company_name", ColumnDescription = "工作单位", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string CompanyName { get; set; } = string.Empty;
    /// <summary>
    /// 职位名称
    /// </summary>
    [SugarColumn(ColumnName = "position_name", ColumnDescription = "职位名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? PositionName { get; set; }
    /// <summary>
    /// 工作内容
    /// </summary>
    [SugarColumn(ColumnName = "job_content", ColumnDescription = "工作内容", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? JobContent { get; set; }
    /// <summary>
    /// 开始日期
    /// </summary>
    [SugarColumn(ColumnName = "start_date", ColumnDescription = "开始日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? StartDate { get; set; }
    /// <summary>
    /// 结束日期
    /// </summary>
    [SugarColumn(ColumnName = "end_date", ColumnDescription = "结束日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? EndDate { get; set; }
    /// <summary>
    /// 证明人姓名
    /// </summary>
    [SugarColumn(ColumnName = "witness_name", ColumnDescription = "证明人姓名", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? WitnessName { get; set; }
    /// <summary>
    /// 证明人电话
    /// </summary>
    [SugarColumn(ColumnName = "witness_phone", ColumnDescription = "证明人电话", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? WitnessPhone { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 员工主档（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EmployeeId))]
    public TaktEmployee? Employee { get; set; }
}
