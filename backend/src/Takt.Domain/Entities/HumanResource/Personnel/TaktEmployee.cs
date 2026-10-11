// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Personnel
// 文件名称：TaktEmployee.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：员工主档实体（人事档案核心表）；教育/地址/家庭/上岗/离职/合同/调动等明细一律走同域子表，主档不存投影摘要
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities.HumanResource.Organization;

namespace Takt.Domain.Entities.HumanResource.Personnel;

/// <summary>
/// 员工实体（人事主档，公司级档案非审批单）
/// 仅保留身份与档案基本属性；明细见导航子表：
/// 教育→Education；地址→Address；家庭/紧急联系人→Family；
/// 上岗日期/试用/转正/主部门岗位→Joined；离职→Resignation；
/// 合同→Contract；调动→Reassignment；技能→Skill；履历→Experience；
/// 附件→Attachment；代理→Delegation；入职待办→Onboarding
/// </summary>
[SugarTable("takt_human_resource_personnel_employee", "员工表")]
[SugarIndex("ix_employee_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_employee_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_employee_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeCode), OrderByType.Asc, true)]
[SugarIndex("ix_employee_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeStatus), OrderByType.Asc, false)]
public class TaktEmployee : TaktCompanyEntityBase
{
    /// <summary>
    /// 员工编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 员工编码规则生成并展示，非手输；单据类型菜单：员工档案）
    /// </summary>
    [SugarColumn(ColumnName = "employee_code", ColumnDescription = "员工编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string EmployeeCode { get; set; } = string.Empty;
    /// <summary>
    /// 姓名（2–80 位）
    /// </summary>
    [SugarColumn(ColumnName = "employee_name", ColumnDescription = "姓名", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string EmployeeName { get; set; } = string.Empty;
    /// <summary>
    /// 性别（字典 sys_user_gender；0=未知 1=男 2=女）
    /// </summary>
    [SugarColumn(ColumnName = "gender", ColumnDescription = "性别", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int Gender { get; set; } = 0;
    /// <summary>
    /// 出生日期（人事档案必填）
    /// </summary>
    [SugarColumn(ColumnName = "birth_date", ColumnDescription = "出生日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime BirthDate { get; set; }
    /// <summary>
    /// 身份证号（人事档案必填）
    /// </summary>
    [SugarColumn(ColumnName = "id_card_code", ColumnDescription = "身份证号", ColumnDataType = "varchar", Length = 18, IsNullable = false)]
    public string IdCardCode { get; set; } = string.Empty;
    /// <summary>
    /// 手机号码（人事档案必填）
    /// </summary>
    [SugarColumn(ColumnName = "mobile", ColumnDescription = "手机号码", ColumnDataType = "varchar", Length = 11, IsNullable = false)]
    public string Mobile { get; set; } = string.Empty;
    /// <summary>
    /// 电子邮箱
    /// </summary>
    [SugarColumn(ColumnName = "email", ColumnDescription = "电子邮箱", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
    public string? Email { get; set; }
    /// <summary>
    /// 籍贯（字典 humanresource_personnel_native_place_code；列存 6 位 GB 行政区划代码，人事档案必填；与住址子表无关）
    /// </summary>
    [SugarColumn(ColumnName = "native_place", ColumnDescription = "籍贯", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string NativePlace { get; set; } = string.Empty;
    /// <summary>
    /// 民族（字典 humanresource_personnel_ethnic_code；DictValue 1～56）
    /// </summary>
    [SugarColumn(ColumnName = "ethnicity", ColumnDescription = "民族", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int Ethnicity { get; set; } = 1;
    /// <summary>
    /// 政治面貌（字典 humanresource_personnel_political_affiliation；0～12；人事档案必填）
    /// </summary>
    [SugarColumn(ColumnName = "political_affiliation", ColumnDescription = "政治面貌", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PoliticalAffiliation { get; set; } = 0;
    /// <summary>
    /// 婚姻状况（字典 humanresource_personnel_marital_status；0=未婚 1=已婚 2=离异 3=丧偶；人事档案必填）
    /// </summary>
    [SugarColumn(ColumnName = "marital_status", ColumnDescription = "婚姻状况", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int MaritalStatus { get; set; } = 0;
    /// <summary>
    /// 员工状态（字典 humanresource_personnel_employee_status；1=试用期 2=正式 3=离职 4=退休）
    /// </summary>
    [SugarColumn(ColumnName = "employee_status", ColumnDescription = "员工状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int EmployeeStatus { get; set; } = 1;
    /// <summary>
    /// 内置（字典 sys_yes_no；0=否 1=是；种子员工不可删）
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBuiltIn { get; set; } = 0;
    /// <summary>
    /// 头像URL（展示用；档案附件明细见 EmployeeAttachments）
    /// </summary>
    [SugarColumn(ColumnName = "avatar", ColumnDescription = "头像URL", ColumnDataType = "varchar", Length = 500, IsNullable = true)]
    public string? Avatar { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 员工部门关联（RBAC，表 takt_human_resource_organization_employeedept）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEmployeeDept.EmployeeId))]
    public List<TaktEmployeeDept>? EmployeeDepts { get; set; }

    /// <summary>
    /// 员工岗位关联（RBAC，表 takt_human_resource_organization_employeepost）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEmployeePost.EmployeeId))]
    public List<TaktEmployeePost>? EmployeePosts { get; set; }

    /// <summary>
    /// 员工地址（家庭/工作/常住）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEmployeeAddress.EmployeeId))]
    public List<TaktEmployeeAddress>? EmployeeAddresses { get; set; }

    /// <summary>
    /// 教育经历（含最高学历 IsHighest）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEmployeeEducation.EmployeeId))]
    public List<TaktEmployeeEducation>? EmployeeEducations { get; set; }

    /// <summary>
    /// 家庭成员（含紧急联系人 IsEmergencyContact）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEmployeeFamily.EmployeeId))]
    public List<TaktEmployeeFamily>? EmployeeFamilies { get; set; }

    /// <summary>
    /// 外部工作经历
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEmployeeExperience.EmployeeId))]
    public List<TaktEmployeeExperience>? EmployeeExperiences { get; set; }

    /// <summary>
    /// 技能与证书
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEmployeeSkill.EmployeeId))]
    public List<TaktEmployeeSkill>? EmployeeSkills { get; set; }

    /// <summary>
    /// 劳动合同
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEmployeeContract.EmployeeId))]
    public List<TaktEmployeeContract>? EmployeeContracts { get; set; }

    /// <summary>
    /// 入职上岗办理（实际上岗日/试用/转正/部门岗位）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEmployeeJoined.EmployeeId))]
    public List<TaktEmployeeJoined>? EmployeeJoineds { get; set; }

    /// <summary>
    /// 入职待办
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEmployeeOnboarding.EmployeeId))]
    public List<TaktEmployeeOnboarding>? EmployeeOnboardings { get; set; }

    /// <summary>
    /// 调动记录
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEmployeeReassignment.EmployeeId))]
    public List<TaktEmployeeReassignment>? EmployeeReassignments { get; set; }

    /// <summary>
    /// 离职办理
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEmployeeResignation.EmployeeId))]
    public List<TaktEmployeeResignation>? EmployeeResignations { get; set; }

    /// <summary>
    /// 档案附件
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEmployeeAttachment.EmployeeId))]
    public List<TaktEmployeeAttachment>? EmployeeAttachments { get; set; }

    /// <summary>
    /// 员工代理（被代理人视角；外键 OriginalEmployeeId）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEmployeeDelegation.OriginalEmployeeId))]
    public List<TaktEmployeeDelegation>? EmployeeDelegations { get; set; }

}
