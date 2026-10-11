// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Personnel
// 文件名称：TaktEmployeeSkill.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：员工技能/证书实体（人事-技能管理）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.HumanResource.Personnel;

/// <summary>
/// 员工技能与证书
/// </summary>
[SugarTable("takt_human_resource_personnel_employee_skill", "员工技能表")]
[SugarIndex("ix_employee_skill_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_employee_skill_employee", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, false)]
public class TaktEmployeeSkill : TaktCompanyEntityBase
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
    /// 技能名称
    /// </summary>
    [SugarColumn(ColumnName = "skill_name", ColumnDescription = "技能名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string SkillName { get; set; } = string.Empty;
    /// <summary>
    /// 技能等级（字典 humanresource_personnel_employee_skill_level；0=入门 1=熟练 2=精通 3=专家）
    /// </summary>
    [SugarColumn(ColumnName = "skill_level", ColumnDescription = "技能等级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SkillLevel { get; set; }
    /// <summary>
    /// 证书名称
    /// </summary>
    [SugarColumn(ColumnName = "certificate_name", ColumnDescription = "证书名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? CertificateName { get; set; }
    /// <summary>
    /// 证书编码
    /// </summary>
    [SugarColumn(ColumnName = "certificate_code", ColumnDescription = "证书编码", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
    public string? CertificateCode { get; set; }
    /// <summary>
    /// 取得日期
    /// </summary>
    [SugarColumn(ColumnName = "obtained_date", ColumnDescription = "取得日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ObtainedDate { get; set; }
    /// <summary>
    /// 到期日期
    /// </summary>
    [SugarColumn(ColumnName = "expiry_date", ColumnDescription = "到期日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ExpiryDate { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 员工主档（多对一）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EmployeeId))]
    public TaktEmployee? Employee { get; set; }
}
