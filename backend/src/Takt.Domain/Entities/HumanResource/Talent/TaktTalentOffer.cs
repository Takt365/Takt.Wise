// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Talent
// 文件名称：TaktTalentOffer.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：录用信息（人才链路：用人需求→职位发布→录用）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Domain.Entities.HumanResource.Personnel;

namespace Takt.Domain.Entities.HumanResource.Talent;

/// <summary>
/// 录用信息（审批单；审批态见基类 ApprovalStatus，字典 sys_approval_status）
/// </summary>
[SugarTable("takt_human_resource_talent_offer", "录用信息表")]
[SugarIndex("ix_talent_offer_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_talent_offer_job_posting", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(JobPostingId), OrderByType.Asc, false)]
[SugarIndex("ix_talent_offer_approval", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ApprovalStatus), OrderByType.Asc, false)]
public class TaktTalentOffer : TaktApprovalEntityBase
{
    /// <summary>
    /// 职位发布（选项 TaktTalentJobPostings/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "job_posting_id", ColumnDescription = "职位发布ID", ColumnDataType = "bigint", IsNullable = false)]
    public long JobPostingId { get; set; }
    /// <summary>
    /// 录用编码（租户+公司内业务编码；前端表单选择编码规则后自动通过 TaktNumbering 录用编码规则生成并展示，非手输；单据类型菜单：录用）
    /// </summary>
    [SugarColumn(ColumnName = "offer_code", ColumnDescription = "录用编码", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string OfferCode { get; set; } = string.Empty;
    /// <summary>
    /// 录用日期（确认录用/发 offer）
    /// </summary>
    [SugarColumn(ColumnName = "hire_date", ColumnDescription = "录用日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime HireDate { get; set; }
    /// <summary>
    /// 关联员工（选项 TaktEmployees/options；录用通过并建档后回填，可空，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "关联员工ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? EmployeeId { get; set; }
    /// <summary>
    /// 员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "employee_name", ColumnDescription = "员工名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? EmployeeName { get; set; }
    /// <summary>
    /// 拟录用部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
    /// </summary>
    [SugarColumn(ColumnName = "dept_id", ColumnDescription = "拟录用部门ID", ColumnDataType = "bigint", IsNullable = false)]
    public long DeptId { get; set; }
    /// <summary>
    /// 拟录用部门名称
    /// </summary>
    [SugarColumn(ColumnName = "dept_name", ColumnDescription = "拟录用部门名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string DeptName { get; set; } = string.Empty;
    /// <summary>
    /// 拟录用岗位（选项 TaktPosts/options，可空，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "post_id", ColumnDescription = "拟录用岗位ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? PostId { get; set; }
    /// <summary>
    /// 拟录用岗位名称
    /// </summary>
    [SugarColumn(ColumnName = "post_name", ColumnDescription = "拟录用岗位名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? PostName { get; set; }
    /// <summary>
    /// 录用说明
    /// </summary>
    [SugarColumn(ColumnName = "reason", ColumnDescription = "录用说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? Reason { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 职位发布
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(JobPostingId))]
    public TaktTalentJobPosting? JobPosting { get; set; }

    /// <summary>
    /// 入职待办
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktEmployeeOnboarding.OfferId))]
    public List<TaktEmployeeOnboarding>? EmployeeOnboardings { get; set; }
}
