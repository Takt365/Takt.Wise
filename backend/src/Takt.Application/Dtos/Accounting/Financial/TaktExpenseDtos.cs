// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Accounting.Financial
// 文件名称：TaktExpenseDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：Expense 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktExpense 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Accounting.Financial;

// ========================================
// Expense 响应 DTO
// ========================================

/// <summary>
/// 费用单实体。继承审批基类，与 TaktFlowEngine 对接；ExpenseStatus 与 ApprovalStatus 取值对齐。
/// 对应前端 TaktExpenseDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktExpenseDto : TaktApprovalDtoBase
{
    /// <summary>
    /// ExpenseID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExpenseId { get; set; }

    /// <summary>
    /// 费用单编码（租户+公司内唯一）
    /// </summary>
    public string ExpenseCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用标题
    /// </summary>
    public string ExpenseTitle { get; set; } = string.Empty;

    /// <summary>
    /// 费用类型（字典 accounting_financial_expense_type：1=月结供应商除原材料外的费用，2=月结供应商货款及公司其他费用，3=杂项购置费用）
    /// </summary>
    public int ExpenseType { get; set; } = 0;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；整单唯一，DictValue=Id）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称（整单唯一）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 申请人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApplicantBy { get; set; }

    /// <summary>
    /// 申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? ApplicantName { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicationDeptId { get; set; }

    /// <summary>
    /// 申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? ApplicationDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostBearerDeptId { get; set; }

    /// <summary>
    /// 经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? CostBearerDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
    /// </summary>
    public string? CostCenter { get; set; } = string.Empty;

    /// <summary>
    /// 关联会签单（选项 TaktCountersigns/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CountersignId { get; set; }

    /// <summary>
    /// 关联会签单（选项 TaktCountersigns/options；DictValue=Id）
    /// </summary>
    public string? CountersignName { get; set; }

    /// <summary>
    /// 来源采购订单编码（选项 TaktPurchaseOrders/options；采购链路自动生成时写入，DictValue=Id）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购申请编码（选项 TaktPurchaseRequests/options；采购链路自动生成时写入，DictValue=Id）
    /// </summary>
    public string? PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用金额
    /// </summary>
    public decimal ExpenseAmount { get; set; }

    /// <summary>
    /// 税率（字典 accounting_financial_tax_rate_param；整单统一税率）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 税额（整单合计）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 费用发生日期
    /// </summary>
    public DateTime ExpenseDate { get; set; }

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? ApplicationReason { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    public int ExpenseStatus { get; set; } = 0;

    /// <summary>
    /// 费用单明细列表（主子表关系）
    /// （子表：TaktExpenseDetail）
    /// </summary>
    public List<TaktExpenseDetailDto>? ExpenseDetails { get; set; }

}

// ========================================
// Expense 查询 DTO
// ========================================

/// <summary>
/// Expense 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktExpenseQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用单编码（租户+公司内唯一）
    /// </summary>
    public string? ExpenseCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用标题
    /// </summary>
    public string? ExpenseTitle { get; set; } = string.Empty;

    /// <summary>
    /// 费用类型（字典 accounting_financial_expense_type：1=月结供应商除原材料外的费用，2=月结供应商货款及公司其他费用，3=杂项购置费用）
    /// </summary>
    public int? ExpenseType { get; set; }

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；整单唯一，DictValue=Id）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称（整单唯一）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 申请人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicantBy { get; set; }

    /// <summary>
    /// 申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? ApplicantName { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicationDeptId { get; set; }

    /// <summary>
    /// 申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? ApplicationDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostBearerDeptId { get; set; }

    /// <summary>
    /// 经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? CostBearerDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
    /// </summary>
    public string? CostCenter { get; set; } = string.Empty;

    /// <summary>
    /// 关联会签单（选项 TaktCountersigns/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CountersignId { get; set; }

    /// <summary>
    /// 来源采购订单编码（选项 TaktPurchaseOrders/options；采购链路自动生成时写入，DictValue=Id）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购申请编码（选项 TaktPurchaseRequests/options；采购链路自动生成时写入，DictValue=Id）
    /// </summary>
    public string? PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用金额
    /// </summary>
    public decimal? ExpenseAmount { get; set; }

    /// <summary>
    /// 税率（字典 accounting_financial_tax_rate_param；整单统一税率）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 税额（整单合计）
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 费用发生日期（范围查询-开始）
    /// </summary>
    public DateTime? ExpenseDateStart { get; set; }

    /// <summary>
    /// 费用发生日期（范围查询-结束）
    /// </summary>
    public DateTime? ExpenseDateEnd { get; set; }

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? ApplicationReason { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    public int? ExpenseStatus { get; set; }

    /// <summary>
    /// 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
    /// </summary>
    public TaktApprovalStatus? ApprovalStatus { get; set; }

    /// <summary>
    /// 发起人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? InitiatorId { get; set; }

    /// <summary>
    /// 发起时间（范围查询-开始）
    /// </summary>
    public DateTime? InitiatedAtStart { get; set; }

    /// <summary>
    /// 发起时间（范围查询-结束）
    /// </summary>
    public DateTime? InitiatedAtEnd { get; set; }

    /// <summary>
    /// 最终审批人ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApprovedBy { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-开始）
    /// </summary>
    public DateTime? ApprovedAtStart { get; set; }

    /// <summary>
    /// 最终审批时间（范围查询-结束）
    /// </summary>
    public DateTime? ApprovedAtEnd { get; set; }

    /// <summary>
    /// 流程实例 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FlowInstanceId { get; set; }

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建Expense DTO
// ========================================

/// <summary>
/// 创建Expense DTO
/// </summary>
public class TaktExpenseCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用单编码（租户+公司内唯一）
    /// </summary>
    [Required(ErrorMessage = "费用单编码（租户+公司内唯一）不能为空")]
    public string ExpenseCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用标题
    /// </summary>
    [Required(ErrorMessage = "费用标题不能为空")]
    public string ExpenseTitle { get; set; } = string.Empty;

    /// <summary>
    /// 费用类型（字典 accounting_financial_expense_type：1=月结供应商除原材料外的费用，2=月结供应商货款及公司其他费用，3=杂项购置费用）
    /// </summary>
    public int ExpenseType { get; set; } = 0;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；整单唯一，DictValue=Id）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称（整单唯一）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 申请人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApplicantBy { get; set; }

    /// <summary>
    /// 申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? ApplicantName { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicationDeptId { get; set; }

    /// <summary>
    /// 申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? ApplicationDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostBearerDeptId { get; set; }

    /// <summary>
    /// 经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? CostBearerDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
    /// </summary>
    public string? CostCenter { get; set; } = string.Empty;

    /// <summary>
    /// 关联会签单（选项 TaktCountersigns/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CountersignId { get; set; }

    /// <summary>
    /// 来源采购订单编码（选项 TaktPurchaseOrders/options；采购链路自动生成时写入，DictValue=Id）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购申请编码（选项 TaktPurchaseRequests/options；采购链路自动生成时写入，DictValue=Id）
    /// </summary>
    public string? PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用金额
    /// </summary>
    public decimal ExpenseAmount { get; set; }

    /// <summary>
    /// 税率（字典 accounting_financial_tax_rate_param；整单统一税率）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 税额（整单合计）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 费用发生日期
    /// </summary>
    public DateTime ExpenseDate { get; set; }

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? ApplicationReason { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    public int ExpenseStatus { get; set; } = 0;

    /// <summary>
    /// 费用单明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktExpenseDetailCreateDto>? ExpenseDetails { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新Expense DTO
// ========================================

/// <summary>
/// 更新Expense DTO
/// 继承 TaktExpenseCreateDto，添加 ExpenseId 字段
/// </summary>
public class TaktExpenseUpdateDto : TaktExpenseCreateDto
{
    /// <summary>
    /// ExpenseID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExpenseId { get; set; }

    /// <summary>
    /// 费用单明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktExpenseDetailUpdateDto>? ExpenseDetails { get; set; }

}

// ========================================
// Expense 状态 DTO
// ========================================

/// <summary>
/// Expense 状态更新 DTO
/// </summary>
public class TaktExpenseStatusDto
{
    /// <summary>
    /// ExpenseID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExpenseId { get; set; }

    /// <summary>
    /// 费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    [Required(ErrorMessage = "费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）不能为空")]
    public int ExpenseStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Expense 导入模板行 DTO
/// </summary>
public class TaktExpenseTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用单编码（租户+公司内唯一）
    /// </summary>
    public string? ExpenseCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用标题
    /// </summary>
    public string? ExpenseTitle { get; set; } = string.Empty;

    /// <summary>
    /// 费用类型（字典 accounting_financial_expense_type：1=月结供应商除原材料外的费用，2=月结供应商货款及公司其他费用，3=杂项购置费用）
    /// </summary>
    public int? ExpenseType { get; set; }

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；整单唯一，DictValue=Id）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称（整单唯一）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 申请人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicantBy { get; set; }

    /// <summary>
    /// 申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? ApplicantName { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicationDeptId { get; set; }

    /// <summary>
    /// 申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? ApplicationDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostBearerDeptId { get; set; }

    /// <summary>
    /// 经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? CostBearerDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
    /// </summary>
    public string? CostCenter { get; set; } = string.Empty;

    /// <summary>
    /// 关联会签单（选项 TaktCountersigns/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CountersignId { get; set; }

    /// <summary>
    /// 来源采购订单编码（选项 TaktPurchaseOrders/options；采购链路自动生成时写入，DictValue=Id）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购申请编码（选项 TaktPurchaseRequests/options；采购链路自动生成时写入，DictValue=Id）
    /// </summary>
    public string? PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用金额
    /// </summary>
    public decimal? ExpenseAmount { get; set; }

    /// <summary>
    /// 税率（字典 accounting_financial_tax_rate_param；整单统一税率）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 税额（整单合计）
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 费用发生日期
    /// </summary>
    public DateTime? ExpenseDate { get; set; }

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? ApplicationReason { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    public int? ExpenseStatus { get; set; }

    /// <summary>
    /// 费用单明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktExpenseDetailCreateDto>? ExpenseDetails { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// Expense 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktExpenseImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用单编码（租户+公司内唯一）
    /// </summary>
    public string? ExpenseCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用标题
    /// </summary>
    public string? ExpenseTitle { get; set; } = string.Empty;

    /// <summary>
    /// 费用类型（字典 accounting_financial_expense_type：1=月结供应商除原材料外的费用，2=月结供应商货款及公司其他费用，3=杂项购置费用）
    /// </summary>
    public int? ExpenseType { get; set; }

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；整单唯一，DictValue=Id）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称（整单唯一）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 申请人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicantBy { get; set; }

    /// <summary>
    /// 申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? ApplicantName { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicationDeptId { get; set; }

    /// <summary>
    /// 申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? ApplicationDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostBearerDeptId { get; set; }

    /// <summary>
    /// 经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? CostBearerDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
    /// </summary>
    public string? CostCenter { get; set; } = string.Empty;

    /// <summary>
    /// 关联会签单（选项 TaktCountersigns/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CountersignId { get; set; }

    /// <summary>
    /// 来源采购订单编码（选项 TaktPurchaseOrders/options；采购链路自动生成时写入，DictValue=Id）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购申请编码（选项 TaktPurchaseRequests/options；采购链路自动生成时写入，DictValue=Id）
    /// </summary>
    public string? PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用金额
    /// </summary>
    public decimal? ExpenseAmount { get; set; }

    /// <summary>
    /// 税率（字典 accounting_financial_tax_rate_param；整单统一税率）
    /// </summary>
    public int? TaxRate { get; set; }

    /// <summary>
    /// 税额（整单合计）
    /// </summary>
    public decimal? TaxAmount { get; set; }

    /// <summary>
    /// 费用发生日期
    /// </summary>
    public DateTime? ExpenseDate { get; set; }

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? ApplicationReason { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    public int? ExpenseStatus { get; set; }

    /// <summary>
    /// 费用单明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktExpenseDetailCreateDto>? ExpenseDetails { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// Expense 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktExpenseExportDto
{
    /// <summary>
    /// ExpenseID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExpenseId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用单编码（租户+公司内唯一）
    /// </summary>
    public string ExpenseCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用标题
    /// </summary>
    public string ExpenseTitle { get; set; } = string.Empty;

    /// <summary>
    /// 费用类型（字典 accounting_financial_expense_type：1=月结供应商除原材料外的费用，2=月结供应商货款及公司其他费用，3=杂项购置费用）
    /// </summary>
    public int ExpenseType { get; set; } = 0;

    /// <summary>
    /// 供应商编码（选项 TaktSuppliers/options；整单唯一，DictValue=Id）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供应商名称（整单唯一）
    /// </summary>
    public string? SupplierName1 { get; set; } = string.Empty;

    /// <summary>
    /// 申请人（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApplicantBy { get; set; }

    /// <summary>
    /// 申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    public string? ApplicantName { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApplicationDeptId { get; set; }

    /// <summary>
    /// 申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? ApplicationDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CostBearerDeptId { get; set; }

    /// <summary>
    /// 经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）
    /// </summary>
    public string? CostBearerDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 成本中心（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
    /// </summary>
    public string? CostCenter { get; set; } = string.Empty;

    /// <summary>
    /// 关联会签单（选项 TaktCountersigns/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CountersignId { get; set; }

    /// <summary>
    /// 来源采购订单编码（选项 TaktPurchaseOrders/options；采购链路自动生成时写入，DictValue=Id）
    /// </summary>
    public string? PurchaseOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购申请编码（选项 TaktPurchaseRequests/options；采购链路自动生成时写入，DictValue=Id）
    /// </summary>
    public string? PurchaseRequestCode { get; set; } = string.Empty;

    /// <summary>
    /// 费用金额
    /// </summary>
    public decimal ExpenseAmount { get; set; }

    /// <summary>
    /// 税率（字典 accounting_financial_tax_rate_param；整单统一税率）
    /// </summary>
    public int TaxRate { get; set; } = 0;

    /// <summary>
    /// 税额（整单合计）
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// 费用发生日期
    /// </summary>
    public DateTime ExpenseDate { get; set; }

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? ApplicationReason { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    public int ExpenseStatus { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
