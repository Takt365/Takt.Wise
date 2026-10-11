// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Personnel
// 文件名称：TaktEmployeeOnboarding.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：入职待办（人事链路第6步：录用通过后办理入职，完成后关联上岗单）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities.HumanResource.Talent;

namespace Takt.Domain.Entities.HumanResource.Personnel;

/// <summary>
/// 入职待办（办理待办单，非审批单；状态见 TodoStatus）
/// </summary>
[SugarTable("takt_human_resource_personnel_employee_onboarding", "入职待办表")]
[SugarIndex("ix_employee_onboarding_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_employee_onboarding_offer", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(OfferId), OrderByType.Asc, false)]
public class TaktEmployeeOnboarding : TaktCompanyEntityBase
{
    /// <summary>
    /// 录用信息（选项 TaktTalentOffers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "offer_id", ColumnDescription = "录用信息ID", ColumnDataType = "bigint", IsNullable = false)]
    public long OfferId { get; set; }
    /// <summary>
    /// 待办单号（租户+公司内业务编码）
    /// </summary>
    [SugarColumn(ColumnName = "todo_code", ColumnDescription = "待办单号", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string TodoCode { get; set; } = string.Empty;
    /// <summary>
    /// 计划上岗日期（JoinedDate 计划值）
    /// </summary>
    [SugarColumn(ColumnName = "planned_joined_date", ColumnDescription = "计划上岗日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PlannedJoinedDate { get; set; }
    /// <summary>
    /// 候选人姓名（快照）
    /// </summary>
    [SugarColumn(ColumnName = "candidate_name", ColumnDescription = "候选人姓名", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string CandidateName { get; set; } = string.Empty;
    /// <summary>
    /// 候选人手机（快照）
    /// </summary>
    [SugarColumn(ColumnName = "mobile", ColumnDescription = "候选人手机", ColumnDataType = "varchar", Length = 11, IsNullable = true)]
    public string? Mobile { get; set; }
    /// <summary>
    /// 关联员工（选项 TaktEmployees/options；建档后回填，可空，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "关联员工ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? EmployeeId { get; set; }
    /// <summary>
    /// 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐；建档回填后可写，建档前可空）
    /// </summary>
    [SugarColumn(ColumnName = "employee_code", ColumnDescription = "员工编码", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? EmployeeCode { get; set; }
    /// <summary>
    /// 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐；建档回填后可写，建档前可空）
    /// </summary>
    [SugarColumn(ColumnName = "employee_name", ColumnDescription = "员工姓名", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? EmployeeName { get; set; }
    /// <summary>
    /// 入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）
    /// </summary>
    [SugarColumn(ColumnName = "employee_joined_id", ColumnDescription = "入职上岗单ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? EmployeeJoinedId { get; set; }
    /// <summary>
    /// 待办说明
    /// </summary>
    [SugarColumn(ColumnName = "reason", ColumnDescription = "待办说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? Reason { get; set; }
    /// <summary>
    /// 待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）
    /// </summary>
    [SugarColumn(ColumnName = "todo_status", ColumnDescription = "待办状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TodoStatus { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 员工主档（多对一；建档回填后可有值）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EmployeeId))]
    public TaktEmployee? Employee { get; set; }

    /// <summary>
    /// 录用信息
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(OfferId))]
    public TaktTalentOffer? Offer { get; set; }

    /// <summary>
    /// 入职上岗单
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(EmployeeJoinedId))]
    public TaktEmployeeJoined? EmployeeJoined { get; set; }
}
