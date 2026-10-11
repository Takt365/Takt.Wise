// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Accounting.Financial
// 文件名称：TaktCountersign.cs
// 创建时间：2025-03-16
// 创建人：Takt365(Cursor AI)
// 功能描述：会签单实体（财务审批业务单，可关联工作流实例；含会签明细 CountersignDetails）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Financial;

/// <summary>
/// 会签单实体。继承审批基类，与 TaktFlowEngine 对接；CountersignStatus 与 ApprovalStatus 取值对齐。
/// </summary>
[SugarTable("takt_accounting_financial_countersign", "会签单表")]
[SugarIndex("ix_countersign_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_countersign_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_countersign_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CountersignCode), OrderByType.Asc, true)]
[SugarIndex("ix_countersign_flow_instance", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
public class TaktCountersign : TaktApprovalEntityBase
{
    /// <summary>
    /// 会签编码
    /// </summary>
    [SugarColumn(ColumnName = "countersign_code", ColumnDescription = "会签编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string CountersignCode { get; set; } = string.Empty;
    /// <summary>
    /// 来源采购询价（选项 TaktPurchaseInquiries/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_inquiry_id", ColumnDescription = "来源采购询价ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? PurchaseInquiryId { get; set; }
    /// <summary>
    /// 来源采购询价编码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.PurchaseInquiryCode 联动）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_inquiry_code", ColumnDescription = "来源采购询价编码", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? PurchaseInquiryCode { get; set; }
    /// <summary>
    /// 会签业务类型（字典 accounting_financial_countersign_business_type：inquiry/pr/expense/standalone）
    /// </summary>
    [SugarColumn(ColumnName = "business_type", ColumnDescription = "会签业务类型", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "standalone")]
    public string BusinessType { get; set; } = "standalone";
    /// <summary>
    /// 会签业务键（如 inquiry:123、pr:456、expense:789）
    /// </summary>
    [SugarColumn(ColumnName = "business_key", ColumnDescription = "会签业务键", ColumnDataType = "varchar", Length = 80, IsNullable = true)]
    public string? BusinessKey { get; set; }
    /// <summary>
    /// 会签步骤序号（询价=1，PR=2，报销=3）
    /// </summary>
    [SugarColumn(ColumnName = "step_no", ColumnDescription = "会签步骤序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int StepNo { get; set; } = 1;
    /// <summary>
    /// 会签部门（选项 TaktDepts/tree-options；DictValue=Id；多选 JSON）
    /// </summary>
    [SugarColumn(ColumnName = "countersign_depts", ColumnDescription = "会签部门", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? CountersignDepts { get; set; }
    /// <summary>
    /// 财务部门 JSON
    /// </summary>
    [SugarColumn(ColumnName = "finance_dept", ColumnDescription = "财务部门", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? FinanceDept { get; set; }
    /// <summary>
    /// 预算审核意见
    /// </summary>
    [SugarColumn(ColumnName = "budget_review_comment", ColumnDescription = "预算审核意见", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? BudgetReviewComment { get; set; }
    /// <summary>
    /// 总经室 JSON
    /// </summary>
    [SugarColumn(ColumnName = "executive_office", ColumnDescription = "总经室", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? ExecutiveOffice { get; set; }
    /// <summary>
    /// 申请人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "applicant_by", ColumnDescription = "申请人ID", ColumnDataType = "bigint", IsNullable = false)]
    public long ApplicantBy { get; set; }
    /// <summary>
    /// 申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "applicant_name", ColumnDescription = "申请人名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? ApplicantName { get; set; }
    /// <summary>
    /// 申请部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "application_dept_id", ColumnDescription = "申请部门ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? ApplicationDeptId { get; set; }
    /// <summary>
    /// 申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    [SugarColumn(ColumnName = "application_dept_name", ColumnDescription = "申请部门名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? ApplicationDeptName { get; set; }
    /// <summary>
    /// 经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "cost_bearer_dept_id", ColumnDescription = "经费负担部门ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? CostBearerDeptId { get; set; }
    /// <summary>
    /// 经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    [SugarColumn(ColumnName = "cost_bearer_dept_name", ColumnDescription = "经费负担部门名称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? CostBearerDeptName { get; set; }
    /// <summary>
    /// 预算否（字典 sys_yes_no）
    /// </summary>
    [SugarColumn(ColumnName = "is_budget", ColumnDescription = "预算否", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBudget { get; set; } = 0;
    /// <summary>
    /// 预算项目（选项 TaktBudgetActuals/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "budget_item_id", ColumnDescription = "预算项目ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? BudgetItemId { get; set; }
    /// <summary>
    /// 预算项目名称（冗余：按 BudgetItemId 取 TaktBudgetActual.BudgetItemName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "budget_item", ColumnDescription = "预算项目名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? BudgetItem { get; set; }
    /// <summary>
    /// 预算金额（按 BudgetItemId 取 TaktBudgetActual.BudgetAmount 联动）
    /// </summary>
    [SugarColumn(ColumnName = "budget_amount", ColumnDescription = "预算金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal BudgetAmount { get; set; }
    /// <summary>
    /// 申请金额
    /// </summary>
    [SugarColumn(ColumnName = "application_amount", ColumnDescription = "申请金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ApplicationAmount { get; set; }
    /// <summary>
    /// 标题
    /// </summary>
    [SugarColumn(ColumnName = "countersign_title", ColumnDescription = "标题", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? CountersignTitle { get; set; }
    /// <summary>
    /// 申请原因
    /// </summary>
    [SugarColumn(ColumnName = "application_reason", ColumnDescription = "申请原因", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? ApplicationReason { get; set; }
    /// <summary>
    /// 预算使用说明
    /// </summary>
    [SugarColumn(ColumnName = "budget_usage_description", ColumnDescription = "预算使用说明", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? BudgetUsageDescription { get; set; }
    /// <summary>
    /// 目标与预期效益
    /// </summary>
    [SugarColumn(ColumnName = "target_and_expected_benefit", ColumnDescription = "目标与预期效益", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? TargetAndExpectedBenefit { get; set; }
    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    [SugarColumn(ColumnName = "file_name", ColumnDescription = "文件名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? FileName { get; set; }
    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    [SugarColumn(ColumnName = "access_url", ColumnDescription = "访问地址", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? AccessUrl { get; set; }
    /// <summary>
    /// 会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    [SugarColumn(ColumnName = "countersign_status", ColumnDescription = "会签单状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CountersignStatus { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 会签单明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktCountersignDetail.CountersignId))]
    public List<TaktCountersignDetail>? CountersignDetails { get; set; }
}
