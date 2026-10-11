// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Accounting.Financial
// 文件名称：TaktExpense.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：费用单实体（财务审批业务单；含费用明细 ExpenseDetails；BusinessType=Expense）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Financial;

/// <summary>
/// 费用单实体。继承审批基类，与 TaktFlowEngine 对接；ExpenseStatus 与 ApprovalStatus 取值对齐。
/// </summary>
[SugarTable("takt_accounting_financial_expense", "费用单表")]
[SugarIndex("ix_expense_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_expense_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_expense_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ExpenseCode), OrderByType.Asc, true)]
[SugarIndex("ix_expense_flow_instance", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(FlowInstanceId), OrderByType.Asc, false)]
[SugarIndex("ix_expense_applicant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ApplicantBy), OrderByType.Asc, false)]
[SugarIndex("ix_expense_date", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ExpenseDate), OrderByType.Desc, false)]
[SugarIndex("ix_expense_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ExpenseStatus), OrderByType.Asc, false)]
public class TaktExpense : TaktApprovalEntityBase
{
    /// <summary>
    /// 费用单编码（租户+公司内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "expense_code", ColumnDescription = "费用单编码", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string ExpenseCode { get; set; } = string.Empty;
    /// <summary>
    /// 费用标题
    /// </summary>
    [SugarColumn(ColumnName = "expense_title", ColumnDescription = "费用标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ExpenseTitle { get; set; } = string.Empty;
    /// <summary>
    /// 费用类型（字典 accounting_financial_expense_type：1=月结供应商除原材料外的费用，2=月结供应商货款及公司其他费用，3=杂项购置费用）
    /// </summary>
    [SugarColumn(ColumnName = "expense_type", ColumnDescription = "费用类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ExpenseType { get; set; } = 1;
    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；整单唯一，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "供应商编码", ColumnDataType = "varchar", Length = 10, IsNullable = true)]
    public string? SupplierCode { get; set; }
    /// <summary>
    /// 供应商名称（整单唯一）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_name1", ColumnDescription = "供应商名称1", ColumnDataType = "nvarchar", Length = 140, IsNullable = true)]
    public string? SupplierName1 { get; set; }
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
    /// 成本中心（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
    /// </summary>
    [SugarColumn(ColumnName = "cost_center", ColumnDescription = "成本中心", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? CostCenter { get; set; }
    /// <summary>
    /// 关联会签单（选项 TaktCountersigns/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "countersign_id", ColumnDescription = "关联会签单ID", ColumnDataType = "bigint", IsNullable = true)]
    public long? CountersignId { get; set; }
    /// <summary>
    /// 来源采购订单编码（选项 TaktPurchaseOrders/options；采购链路自动生成时写入，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_order_code", ColumnDescription = "来源采购订单编码", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? PurchaseOrderCode { get; set; }
    /// <summary>
    /// 来源采购申请编码（选项 TaktPurchaseRequests/options；采购链路自动生成时写入，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_request_code", ColumnDescription = "来源采购申请编码", ColumnDataType = "varchar", Length = 20, IsNullable = true)]
    public string? PurchaseRequestCode { get; set; }
    /// <summary>
    /// 费用金额
    /// </summary>
    [SugarColumn(ColumnName = "expense_amount", ColumnDescription = "费用金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ExpenseAmount { get; set; }
    /// <summary>
    /// 税率（字典 accounting_financial_tax_rate_param；整单统一税率）
    /// </summary>
    [SugarColumn(ColumnName = "tax_rate", ColumnDescription = "税率", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TaxRate { get; set; }
    /// <summary>
    /// 税额（整单合计）
    /// </summary>
    [SugarColumn(ColumnName = "tax_amount", ColumnDescription = "税额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal TaxAmount { get; set; }
    /// <summary>
    /// 费用发生日期
    /// </summary>
    [SugarColumn(ColumnName = "expense_date", ColumnDescription = "费用发生日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ExpenseDate { get; set; }
    /// <summary>
    /// 申请原因
    /// </summary>
    [SugarColumn(ColumnName = "application_reason", ColumnDescription = "申请原因", ColumnDataType = "nvarchar", Length = -1, IsNullable = true)]
    public string? ApplicationReason { get; set; }
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
    /// 费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    [SugarColumn(ColumnName = "expense_status", ColumnDescription = "费用单状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ExpenseStatus { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 费用单明细列表（主子表关系）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktExpenseDetail.ExpenseId))]
    public List<TaktExpenseDetail>? ExpenseDetails { get; set; }
}
