// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Personnel
// 文件名称：TaktEmployeeFamily.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：员工家庭成员实体（人事-家庭信息）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.HumanResource.Personnel;

/// <summary>
/// 员工家庭成员
/// </summary>
[SugarTable("takt_human_resource_personnel_employee_family", "员工家庭成员表")]
[SugarIndex("ix_employee_family_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_employee_family_employee", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, false)]
public class TaktEmployeeFamily : TaktCompanyEntityBase
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
    /// 成员姓名
    /// </summary>
    [SugarColumn(ColumnName = "member_name", ColumnDescription = "成员姓名", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string MemberName { get; set; } = string.Empty;
    /// <summary>
    /// 与员工关系（字典 humanresource_personnel_employee_family_relation_type；0=配偶 1=子女 2=父母 3=兄弟姐妹 9=其他）
    /// </summary>
    [SugarColumn(ColumnName = "relation_type", ColumnDescription = "关系类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "9")]
    public int RelationType { get; set; }
    /// <summary>
    /// 联系电话
    /// </summary>
    [SugarColumn(ColumnName = "phone_number", ColumnDescription = "联系电话", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? PhoneNumber { get; set; }
    /// <summary>
    /// 工作单位
    /// </summary>
    [SugarColumn(ColumnName = "work_unit", ColumnDescription = "工作单位", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? WorkUnit { get; set; }
    /// <summary>
    /// 职务
    /// </summary>
    [SugarColumn(ColumnName = "job_title", ColumnDescription = "职务", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? JobTitle { get; set; }
    /// <summary>
    /// 出生日期
    /// </summary>
    [SugarColumn(ColumnName = "birth_date", ColumnDescription = "出生日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? BirthDate { get; set; }
    /// <summary>
    /// 是否紧急联系人（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_emergency_contact", ColumnDescription = "是否紧急联系人", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsEmergencyContact { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 员工主档（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EmployeeId))]
    public TaktEmployee? Employee { get; set; }
}
