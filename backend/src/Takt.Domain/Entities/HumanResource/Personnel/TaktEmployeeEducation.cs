// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Personnel
// 文件名称：TaktEmployeeEducation.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：员工教育经历实体（人事-教育背景明细）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.HumanResource.Personnel;

/// <summary>
/// 员工教育经历
/// </summary>
[SugarTable("takt_human_resource_personnel_employee_education", "员工教育经历表")]
[SugarIndex("ix_employee_education_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_employee_education_employee", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, false)]
public class TaktEmployeeEducation : TaktCompanyEntityBase
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
    /// 学校名称
    /// </summary>
    [SugarColumn(ColumnName = "school_name", ColumnDescription = "学校名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string SchoolName { get; set; } = string.Empty;
    /// <summary>
    /// 学历层次（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
    /// </summary>
    [SugarColumn(ColumnName = "education_level", ColumnDescription = "学历层次", ColumnDataType = "int", IsNullable = true)]
    public int? EducationLevel { get; set; }
    /// <summary>
    /// 学位层次（字典 humanresource_personnel_degree_level；0=无 1=学士 2=硕士 3=博士）
    /// </summary>
    [SugarColumn(ColumnName = "degree_level", ColumnDescription = "学位层次", ColumnDataType = "int", IsNullable = true)]
    public int? DegreeLevel { get; set; }
    /// <summary>
    /// 专业名称
    /// </summary>
    [SugarColumn(ColumnName = "major_name", ColumnDescription = "专业名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? MajorName { get; set; }
    /// <summary>
    /// 证书编码
    /// </summary>
    [SugarColumn(ColumnName = "certificate_code", ColumnDescription = "证书编码", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
    public string? CertificateCode { get; set; }
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
    /// 是否最高学历（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_highest", ColumnDescription = "是否最高学历", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsHighest { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 员工主档（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EmployeeId))]
    public TaktEmployee? Employee { get; set; }
}
