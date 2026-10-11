// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Accounting.Financial
// 文件名称：TaktCountersignDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：Countersign 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktCountersign 生成，请按需审阅）
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
// Countersign 响应 DTO
// ========================================

/// <summary>
/// 会签单实体。继承审批基类，与 TaktFlowEngine 对接；CountersignStatus 与 ApprovalStatus 取值对齐。
/// 对应前端 TaktCountersignDto
/// 继承 TaktApprovalDtoBase
/// </summary>
public class TaktCountersignDto : TaktApprovalDtoBase
{
    /// <summary>
    /// CountersignID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CountersignId { get; set; }

    /// <summary>
    /// 会签编码
    /// </summary>
    public string CountersignCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购询价（选项 TaktPurchaseInquiries/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInquiryId { get; set; }

    /// <summary>
    /// 来源采购询价编码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.PurchaseInquiryCode 联动）
    /// </summary>
    public string? PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 会签业务类型（字典 accounting_financial_countersign_business_type：inquiry/pr/expense/standalone）
    /// </summary>
    public string BusinessType { get; set; } = string.Empty;

    /// <summary>
    /// 会签业务键（如 inquiry:123、pr:456、expense:789）
    /// </summary>
    public string? BusinessKey { get; set; } = string.Empty;

    /// <summary>
    /// 会签步骤序号（询价=1，PR=2，报销=3）
    /// </summary>
    public int StepNo { get; set; } = 0;

    /// <summary>
    /// 会签部门（选项 TaktDepts/tree-options；DictValue=Id；多选 JSON）
    /// </summary>
    public string? CountersignDepts { get; set; } = string.Empty;

    /// <summary>
    /// 财务部门 JSON
    /// </summary>
    public string? FinanceDept { get; set; } = string.Empty;

    /// <summary>
    /// 预算审核意见
    /// </summary>
    public string? BudgetReviewComment { get; set; } = string.Empty;

    /// <summary>
    /// 总经室 JSON
    /// </summary>
    public string? ExecutiveOffice { get; set; } = string.Empty;

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
    /// 预算否（字典 sys_yes_no）
    /// </summary>
    public int IsBudget { get; set; } = 0;

    /// <summary>
    /// 预算项目（选项 TaktBudgetActuals/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BudgetItemId { get; set; }

    /// <summary>
    /// 预算项目名称（冗余：按 BudgetItemId 取 TaktBudgetActual.BudgetItemName 联动）
    /// </summary>
    public string? BudgetItem { get; set; } = string.Empty;

    /// <summary>
    /// 预算金额（按 BudgetItemId 取 TaktBudgetActual.BudgetAmount 联动）
    /// </summary>
    public decimal BudgetAmount { get; set; }

    /// <summary>
    /// 申请金额
    /// </summary>
    public decimal ApplicationAmount { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string? CountersignTitle { get; set; } = string.Empty;

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? ApplicationReason { get; set; } = string.Empty;

    /// <summary>
    /// 预算使用说明
    /// </summary>
    public string? BudgetUsageDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标与预期效益
    /// </summary>
    public string? TargetAndExpectedBenefit { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    public int CountersignStatus { get; set; } = 0;

    /// <summary>
    /// 会签单明细列表（主子表关系）
    /// （子表：TaktCountersignDetail）
    /// </summary>
    public List<TaktCountersignDetailDto>? CountersignDetails { get; set; }

}

// ========================================
// Countersign 查询 DTO
// ========================================

/// <summary>
/// Countersign 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktCountersignQueryDto : TaktPagedQuery
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
    /// 会签编码
    /// </summary>
    public string? CountersignCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购询价（选项 TaktPurchaseInquiries/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInquiryId { get; set; }

    /// <summary>
    /// 来源采购询价编码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.PurchaseInquiryCode 联动）
    /// </summary>
    public string? PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 会签业务类型（字典 accounting_financial_countersign_business_type：inquiry/pr/expense/standalone）
    /// </summary>
    public string? BusinessType { get; set; } = string.Empty;

    /// <summary>
    /// 会签业务键（如 inquiry:123、pr:456、expense:789）
    /// </summary>
    public string? BusinessKey { get; set; } = string.Empty;

    /// <summary>
    /// 会签步骤序号（询价=1，PR=2，报销=3）
    /// </summary>
    public int? StepNo { get; set; }

    /// <summary>
    /// 会签部门（选项 TaktDepts/tree-options；DictValue=Id；多选 JSON）
    /// </summary>
    public string? CountersignDepts { get; set; } = string.Empty;

    /// <summary>
    /// 财务部门 JSON
    /// </summary>
    public string? FinanceDept { get; set; } = string.Empty;

    /// <summary>
    /// 预算审核意见
    /// </summary>
    public string? BudgetReviewComment { get; set; } = string.Empty;

    /// <summary>
    /// 总经室 JSON
    /// </summary>
    public string? ExecutiveOffice { get; set; } = string.Empty;

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
    /// 预算否（字典 sys_yes_no）
    /// </summary>
    public int? IsBudget { get; set; }

    /// <summary>
    /// 预算项目（选项 TaktBudgetActuals/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BudgetItemId { get; set; }

    /// <summary>
    /// 预算项目名称（冗余：按 BudgetItemId 取 TaktBudgetActual.BudgetItemName 联动）
    /// </summary>
    public string? BudgetItem { get; set; } = string.Empty;

    /// <summary>
    /// 预算金额（按 BudgetItemId 取 TaktBudgetActual.BudgetAmount 联动）
    /// </summary>
    public decimal? BudgetAmount { get; set; }

    /// <summary>
    /// 申请金额
    /// </summary>
    public decimal? ApplicationAmount { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string? CountersignTitle { get; set; } = string.Empty;

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? ApplicationReason { get; set; } = string.Empty;

    /// <summary>
    /// 预算使用说明
    /// </summary>
    public string? BudgetUsageDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标与预期效益
    /// </summary>
    public string? TargetAndExpectedBenefit { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    public int? CountersignStatus { get; set; }

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
// 创建Countersign DTO
// ========================================

/// <summary>
/// 创建Countersign DTO
/// </summary>
public class TaktCountersignCreateDto
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
    /// 会签编码
    /// </summary>
    [Required(ErrorMessage = "会签编码不能为空")]
    public string CountersignCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购询价（选项 TaktPurchaseInquiries/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInquiryId { get; set; }

    /// <summary>
    /// 来源采购询价编码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.PurchaseInquiryCode 联动）
    /// </summary>
    public string? PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 会签业务类型（字典 accounting_financial_countersign_business_type：inquiry/pr/expense/standalone）
    /// </summary>
    [Required(ErrorMessage = "会签业务类型（字典 accounting_financial_countersign_business_type：inquiry/pr/expense/standalone）不能为空")]
    public string BusinessType { get; set; } = string.Empty;

    /// <summary>
    /// 会签业务键（如 inquiry:123、pr:456、expense:789）
    /// </summary>
    public string? BusinessKey { get; set; } = string.Empty;

    /// <summary>
    /// 会签步骤序号（询价=1，PR=2，报销=3）
    /// </summary>
    public int StepNo { get; set; } = 0;

    /// <summary>
    /// 会签部门（选项 TaktDepts/tree-options；DictValue=Id；多选 JSON）
    /// </summary>
    public string? CountersignDepts { get; set; } = string.Empty;

    /// <summary>
    /// 财务部门 JSON
    /// </summary>
    public string? FinanceDept { get; set; } = string.Empty;

    /// <summary>
    /// 预算审核意见
    /// </summary>
    public string? BudgetReviewComment { get; set; } = string.Empty;

    /// <summary>
    /// 总经室 JSON
    /// </summary>
    public string? ExecutiveOffice { get; set; } = string.Empty;

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
    /// 预算否（字典 sys_yes_no）
    /// </summary>
    public int IsBudget { get; set; } = 0;

    /// <summary>
    /// 预算项目（选项 TaktBudgetActuals/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BudgetItemId { get; set; }

    /// <summary>
    /// 预算项目名称（冗余：按 BudgetItemId 取 TaktBudgetActual.BudgetItemName 联动）
    /// </summary>
    public string? BudgetItem { get; set; } = string.Empty;

    /// <summary>
    /// 预算金额（按 BudgetItemId 取 TaktBudgetActual.BudgetAmount 联动）
    /// </summary>
    public decimal BudgetAmount { get; set; }

    /// <summary>
    /// 申请金额
    /// </summary>
    public decimal ApplicationAmount { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string? CountersignTitle { get; set; } = string.Empty;

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? ApplicationReason { get; set; } = string.Empty;

    /// <summary>
    /// 预算使用说明
    /// </summary>
    public string? BudgetUsageDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标与预期效益
    /// </summary>
    public string? TargetAndExpectedBenefit { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    public int CountersignStatus { get; set; } = 0;

    /// <summary>
    /// 会签单明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktCountersignDetailCreateDto>? CountersignDetails { get; set; }

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
// 更新Countersign DTO
// ========================================

/// <summary>
/// 更新Countersign DTO
/// 继承 TaktCountersignCreateDto，添加 CountersignId 字段
/// </summary>
public class TaktCountersignUpdateDto : TaktCountersignCreateDto
{
    /// <summary>
    /// CountersignID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CountersignId { get; set; }

    /// <summary>
    /// 会签单明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public new List<TaktCountersignDetailUpdateDto>? CountersignDetails { get; set; }

}

// ========================================
// Countersign 状态 DTO
// ========================================

/// <summary>
/// Countersign 状态更新 DTO
/// </summary>
public class TaktCountersignStatusDto
{
    /// <summary>
    /// CountersignID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CountersignId { get; set; }

    /// <summary>
    /// 会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    [Required(ErrorMessage = "会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）不能为空")]
    public int CountersignStatus { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Countersign 导入模板行 DTO
/// </summary>
public class TaktCountersignTemplateDto
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
    /// 会签编码
    /// </summary>
    public string? CountersignCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购询价（选项 TaktPurchaseInquiries/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInquiryId { get; set; }

    /// <summary>
    /// 来源采购询价编码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.PurchaseInquiryCode 联动）
    /// </summary>
    public string? PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 会签业务类型（字典 accounting_financial_countersign_business_type：inquiry/pr/expense/standalone）
    /// </summary>
    public string? BusinessType { get; set; } = string.Empty;

    /// <summary>
    /// 会签业务键（如 inquiry:123、pr:456、expense:789）
    /// </summary>
    public string? BusinessKey { get; set; } = string.Empty;

    /// <summary>
    /// 会签步骤序号（询价=1，PR=2，报销=3）
    /// </summary>
    public int? StepNo { get; set; }

    /// <summary>
    /// 会签部门（选项 TaktDepts/tree-options；DictValue=Id；多选 JSON）
    /// </summary>
    public string? CountersignDepts { get; set; } = string.Empty;

    /// <summary>
    /// 财务部门 JSON
    /// </summary>
    public string? FinanceDept { get; set; } = string.Empty;

    /// <summary>
    /// 预算审核意见
    /// </summary>
    public string? BudgetReviewComment { get; set; } = string.Empty;

    /// <summary>
    /// 总经室 JSON
    /// </summary>
    public string? ExecutiveOffice { get; set; } = string.Empty;

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
    /// 预算否（字典 sys_yes_no）
    /// </summary>
    public int? IsBudget { get; set; }

    /// <summary>
    /// 预算项目（选项 TaktBudgetActuals/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BudgetItemId { get; set; }

    /// <summary>
    /// 预算项目名称（冗余：按 BudgetItemId 取 TaktBudgetActual.BudgetItemName 联动）
    /// </summary>
    public string? BudgetItem { get; set; } = string.Empty;

    /// <summary>
    /// 预算金额（按 BudgetItemId 取 TaktBudgetActual.BudgetAmount 联动）
    /// </summary>
    public decimal? BudgetAmount { get; set; }

    /// <summary>
    /// 申请金额
    /// </summary>
    public decimal? ApplicationAmount { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string? CountersignTitle { get; set; } = string.Empty;

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? ApplicationReason { get; set; } = string.Empty;

    /// <summary>
    /// 预算使用说明
    /// </summary>
    public string? BudgetUsageDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标与预期效益
    /// </summary>
    public string? TargetAndExpectedBenefit { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    public int? CountersignStatus { get; set; }

    /// <summary>
    /// 会签单明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktCountersignDetailCreateDto>? CountersignDetails { get; set; }

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
/// Countersign 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktCountersignImportDto
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
    /// 会签编码
    /// </summary>
    public string? CountersignCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购询价（选项 TaktPurchaseInquiries/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInquiryId { get; set; }

    /// <summary>
    /// 来源采购询价编码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.PurchaseInquiryCode 联动）
    /// </summary>
    public string? PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 会签业务类型（字典 accounting_financial_countersign_business_type：inquiry/pr/expense/standalone）
    /// </summary>
    public string? BusinessType { get; set; } = string.Empty;

    /// <summary>
    /// 会签业务键（如 inquiry:123、pr:456、expense:789）
    /// </summary>
    public string? BusinessKey { get; set; } = string.Empty;

    /// <summary>
    /// 会签步骤序号（询价=1，PR=2，报销=3）
    /// </summary>
    public int? StepNo { get; set; }

    /// <summary>
    /// 会签部门（选项 TaktDepts/tree-options；DictValue=Id；多选 JSON）
    /// </summary>
    public string? CountersignDepts { get; set; } = string.Empty;

    /// <summary>
    /// 财务部门 JSON
    /// </summary>
    public string? FinanceDept { get; set; } = string.Empty;

    /// <summary>
    /// 预算审核意见
    /// </summary>
    public string? BudgetReviewComment { get; set; } = string.Empty;

    /// <summary>
    /// 总经室 JSON
    /// </summary>
    public string? ExecutiveOffice { get; set; } = string.Empty;

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
    /// 预算否（字典 sys_yes_no）
    /// </summary>
    public int? IsBudget { get; set; }

    /// <summary>
    /// 预算项目（选项 TaktBudgetActuals/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BudgetItemId { get; set; }

    /// <summary>
    /// 预算项目名称（冗余：按 BudgetItemId 取 TaktBudgetActual.BudgetItemName 联动）
    /// </summary>
    public string? BudgetItem { get; set; } = string.Empty;

    /// <summary>
    /// 预算金额（按 BudgetItemId 取 TaktBudgetActual.BudgetAmount 联动）
    /// </summary>
    public decimal? BudgetAmount { get; set; }

    /// <summary>
    /// 申请金额
    /// </summary>
    public decimal? ApplicationAmount { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string? CountersignTitle { get; set; } = string.Empty;

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? ApplicationReason { get; set; } = string.Empty;

    /// <summary>
    /// 预算使用说明
    /// </summary>
    public string? BudgetUsageDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标与预期效益
    /// </summary>
    public string? TargetAndExpectedBenefit { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    public int? CountersignStatus { get; set; }

    /// <summary>
    /// 会签单明细列表（主子表关系）（子表，级联保存）
    /// </summary>
    public List<TaktCountersignDetailCreateDto>? CountersignDetails { get; set; }

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
/// Countersign 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktCountersignExportDto
{
    /// <summary>
    /// CountersignID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long CountersignId { get; set; }

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
    /// 会签编码
    /// </summary>
    public string CountersignCode { get; set; } = string.Empty;

    /// <summary>
    /// 来源采购询价（选项 TaktPurchaseInquiries/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PurchaseInquiryId { get; set; }

    /// <summary>
    /// 来源采购询价编码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.PurchaseInquiryCode 联动）
    /// </summary>
    public string? PurchaseInquiryCode { get; set; } = string.Empty;

    /// <summary>
    /// 会签业务类型（字典 accounting_financial_countersign_business_type：inquiry/pr/expense/standalone）
    /// </summary>
    public string BusinessType { get; set; } = string.Empty;

    /// <summary>
    /// 会签业务键（如 inquiry:123、pr:456、expense:789）
    /// </summary>
    public string? BusinessKey { get; set; } = string.Empty;

    /// <summary>
    /// 会签步骤序号（询价=1，PR=2，报销=3）
    /// </summary>
    public int StepNo { get; set; } = 0;

    /// <summary>
    /// 会签部门（选项 TaktDepts/tree-options；DictValue=Id；多选 JSON）
    /// </summary>
    public string? CountersignDepts { get; set; } = string.Empty;

    /// <summary>
    /// 财务部门 JSON
    /// </summary>
    public string? FinanceDept { get; set; } = string.Empty;

    /// <summary>
    /// 预算审核意见
    /// </summary>
    public string? BudgetReviewComment { get; set; } = string.Empty;

    /// <summary>
    /// 总经室 JSON
    /// </summary>
    public string? ExecutiveOffice { get; set; } = string.Empty;

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
    /// 预算否（字典 sys_yes_no）
    /// </summary>
    public int IsBudget { get; set; } = 0;

    /// <summary>
    /// 预算项目（选项 TaktBudgetActuals/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BudgetItemId { get; set; }

    /// <summary>
    /// 预算项目名称（冗余：按 BudgetItemId 取 TaktBudgetActual.BudgetItemName 联动）
    /// </summary>
    public string? BudgetItem { get; set; } = string.Empty;

    /// <summary>
    /// 预算金额（按 BudgetItemId 取 TaktBudgetActual.BudgetAmount 联动）
    /// </summary>
    public decimal BudgetAmount { get; set; }

    /// <summary>
    /// 申请金额
    /// </summary>
    public decimal ApplicationAmount { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string? CountersignTitle { get; set; } = string.Empty;

    /// <summary>
    /// 申请原因
    /// </summary>
    public string? ApplicationReason { get; set; } = string.Empty;

    /// <summary>
    /// 预算使用说明
    /// </summary>
    public string? BudgetUsageDescription { get; set; } = string.Empty;

    /// <summary>
    /// 目标与预期效益
    /// </summary>
    public string? TargetAndExpectedBenefit { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
    /// </summary>
    public int CountersignStatus { get; set; } = 0;

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
