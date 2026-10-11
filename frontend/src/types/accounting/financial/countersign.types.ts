// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：countersign.types.ts
// 创建时间：2026-06-30
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 会签单实体
 * 对应前端 TaktCountersignDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 Countersign
 * @description 对应后端 TaktCountersignDto
 */
export interface Countersign extends ApprovalDtoBase {
  /**
   * CountersignID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  countersignId: string;

  /**
   * 会签编码
   */
  countersignCode: string;

  /**
   * 来源采购询价（选项 TaktPurchaseInquiries/options；DictValue=Id）
   */
  purchaseInquiryId?: string;

  /**
   * 来源采购询价编码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.PurchaseInquiryCode 联动）
   */
  purchaseInquiryCode?: string;

  /**
   * 会签业务类型（字典 accounting_financial_countersign_business_type：inquiry/pr/expense/standalone）
   */
  businessType: string;

  /**
   * 会签业务键（如 inquiry:123、pr:456、expense:789）
   */
  businessKey?: string;

  /**
   * 会签步骤序号（询价=1，PR=2，报销=3）
   */
  stepNo: number;

  /**
   * 会签部门（选项 TaktDepts/tree-options；DictValue=Id；多选 JSON）
   */
  countersignDepts?: string;

  /**
   * 财务部门 JSON
   */
  financeDept?: string;

  /**
   * 预算审核意见
   */
  budgetReviewComment?: string;

  /**
   * 总经室 JSON
   */
  executiveOffice?: string;

  /**
   * 申请人（选项 TaktEmployees/options；DictValue=Id）
   */
  applicantBy: string;

  /**
   * 申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）
   */
  applicantName?: string;

  /**
   * 申请部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  applicationDeptId?: string;

  /**
   * 申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）
   */
  applicationDeptName?: string;

  /**
   * 经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  costBearerDeptId?: string;

  /**
   * 经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）
   */
  costBearerDeptName?: string;

  /**
   * 预算否（字典 sys_yes_no）
   */
  isBudget: number;

  /**
   * 预算项目（选项 TaktBudgetActuals/options；DictValue=Id）
   */
  budgetItemId?: string;

  /**
   * 预算项目名称（冗余：按 BudgetItemId 取 TaktBudgetActual.BudgetItemName 联动）
   */
  budgetItem?: string;

  /**
   * 预算金额
   */
  budgetAmount: number;

  /**
   * 申请金额
   */
  applicationAmount: number;

  /**
   * 标题
   */
  countersignTitle?: string;

  /**
   * 申请原因
   */
  applicationReason?: string;

  /**
   * 预算使用说明
   */
  budgetUsageDescription?: string;

  /**
   * 目标与预期效益
   */
  targetAndExpectedBenefit?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
   */
  countersignStatus: number;

  /**
   * 会签单明细列表（主子表关系） （子表：TaktCountersignDetail）
   */
  countersignDetails?: CountersignDetail[];

}


/**
 * Countersign 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CountersignQuery
 * @description 对应后端 TaktCountersignQueryDto
 */
export interface CountersignQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  /**
   * 会签编码
   */
  countersignCode?: string;

  /**
   * 来源采购询价（选项 TaktPurchaseInquiries/options；DictValue=Id）
   */
  purchaseInquiryId?: string;

  /**
   * 来源采购询价编码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.PurchaseInquiryCode 联动）
   */
  purchaseInquiryCode?: string;

  /**
   * 会签业务类型（字典 accounting_financial_countersign_business_type：inquiry/pr/expense/standalone）
   */
  businessType?: string;

  /**
   * 会签业务键（如 inquiry:123、pr:456、expense:789）
   */
  businessKey?: string;

  /**
   * 会签步骤序号（询价=1，PR=2，报销=3）
   */
  stepNo?: number;

  /**
   * 会签部门（选项 TaktDepts/tree-options；DictValue=Id；多选 JSON）
   */
  countersignDepts?: string;

  /**
   * 财务部门 JSON
   */
  financeDept?: string;

  /**
   * 预算审核意见
   */
  budgetReviewComment?: string;

  /**
   * 总经室 JSON
   */
  executiveOffice?: string;

  /**
   * 申请人（选项 TaktEmployees/options；DictValue=Id）
   */
  applicantBy?: string;

  /**
   * 申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）
   */
  applicantName?: string;

  /**
   * 申请部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  applicationDeptId?: string;

  /**
   * 申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）
   */
  applicationDeptName?: string;

  /**
   * 经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  costBearerDeptId?: string;

  /**
   * 经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）
   */
  costBearerDeptName?: string;

  /**
   * 预算否（字典 sys_yes_no）
   */
  isBudget?: number;

  /**
   * 预算项目（选项 TaktBudgetActuals/options；DictValue=Id）
   */
  budgetItemId?: string;

  /**
   * 预算项目名称（冗余：按 BudgetItemId 取 TaktBudgetActual.BudgetItemName 联动）
   */
  budgetItem?: string;

  /**
   * 预算金额
   */
  budgetAmount?: number;

  /**
   * 申请金额
   */
  applicationAmount?: number;

  /**
   * 标题
   */
  countersignTitle?: string;

  /**
   * 申请原因
   */
  applicationReason?: string;

  /**
   * 预算使用说明
   */
  budgetUsageDescription?: string;

  /**
   * 目标与预期效益
   */
  targetAndExpectedBenefit?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
   */
  countersignStatus?: number;

  /**
   * 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
   */
  approvalStatus?: number;

  /**
   * 发起人ID
   */
  initiatorId?: string;

  /**
   * 发起时间（范围查询-开始）
   */
  initiatedAtStart?: string;

  /**
   * 发起时间（范围查询-结束）
   */
  initiatedAtEnd?: string;

  /**
   * 最终审批人ID
   */
  approvedBy?: string;

  /**
   * 最终审批时间（范围查询-开始）
   */
  approvedAtStart?: string;

  /**
   * 最终审批时间（范围查询-结束）
   */
  approvedAtEnd?: string;

  /**
   * 流程实例 ID
   */
  flowInstanceId?: string;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建Countersign DTO
 * 对应前端 CountersignCreate
 * @description 对应后端 TaktCountersignCreateDto
 */
export interface CountersignCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 会签编码
   */
  countersignCode: string;

  /**
   * 来源采购询价（选项 TaktPurchaseInquiries/options；DictValue=Id）
   */
  purchaseInquiryId?: string;

  /**
   * 来源采购询价编码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.PurchaseInquiryCode 联动）
   */
  purchaseInquiryCode?: string;

  /**
   * 会签业务类型（字典 accounting_financial_countersign_business_type：inquiry/pr/expense/standalone）
   */
  businessType: string;

  /**
   * 会签业务键（如 inquiry:123、pr:456、expense:789）
   */
  businessKey?: string;

  /**
   * 会签步骤序号（询价=1，PR=2，报销=3）
   */
  stepNo: number;

  /**
   * 会签部门（选项 TaktDepts/tree-options；DictValue=Id；多选 JSON）
   */
  countersignDepts?: string;

  /**
   * 财务部门 JSON
   */
  financeDept?: string;

  /**
   * 预算审核意见
   */
  budgetReviewComment?: string;

  /**
   * 总经室 JSON
   */
  executiveOffice?: string;

  /**
   * 申请人（选项 TaktEmployees/options；DictValue=Id）
   */
  applicantBy: string;

  /**
   * 申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）
   */
  applicantName?: string;

  /**
   * 申请部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  applicationDeptId?: string;

  /**
   * 申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）
   */
  applicationDeptName?: string;

  /**
   * 经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  costBearerDeptId?: string;

  /**
   * 经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）
   */
  costBearerDeptName?: string;

  /**
   * 预算否（字典 sys_yes_no）
   */
  isBudget: number;

  /**
   * 预算项目（选项 TaktBudgetActuals/options；DictValue=Id）
   */
  budgetItemId?: string;

  /**
   * 预算项目名称（冗余：按 BudgetItemId 取 TaktBudgetActual.BudgetItemName 联动）
   */
  budgetItem?: string;

  /**
   * 预算金额
   */
  budgetAmount: number;

  /**
   * 申请金额
   */
  applicationAmount: number;

  /**
   * 标题
   */
  countersignTitle?: string;

  /**
   * 申请原因
   */
  applicationReason?: string;

  /**
   * 预算使用说明
   */
  budgetUsageDescription?: string;

  /**
   * 目标与预期效益
   */
  targetAndExpectedBenefit?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
   */
  countersignStatus: number;

  /**
   * 会签单明细列表（主子表关系）（子表，级联保存）
   */
  countersignDetails?: CountersignDetailCreate[];

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * 更新Countersign DTO
 * 继承 TaktCountersignCreateDto，添加 CountersignId 字段
 * 对应前端 CountersignUpdate
 * @description 对应后端 TaktCountersignUpdateDto
 */
export interface CountersignUpdate extends CountersignCreate {
  /**
   * CountersignID（标识要更新的实体）
   */
  countersignId: string;

}


/**
 * Countersign 状态更新 DTO
 * 对应前端 CountersignStatus
 * @description 对应后端 TaktCountersignStatusDto
 */
export interface CountersignStatus {
  /**
   * CountersignID
   */
  countersignId: string;

  /**
   * 会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
   */
  countersignStatus: number;

}


/**
 * Countersign 导入模板行 DTO
 * 对应前端 CountersignTemplate
 * @description 对应后端 TaktCountersignTemplateDto
 */
export interface CountersignTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  /**
   * 会签编码
   */
  countersignCode?: string;

  /**
   * 来源采购询价（选项 TaktPurchaseInquiries/options；DictValue=Id）
   */
  purchaseInquiryId?: string;

  /**
   * 来源采购询价编码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.PurchaseInquiryCode 联动）
   */
  purchaseInquiryCode?: string;

  /**
   * 会签业务类型（字典 accounting_financial_countersign_business_type：inquiry/pr/expense/standalone）
   */
  businessType?: string;

  /**
   * 会签业务键（如 inquiry:123、pr:456、expense:789）
   */
  businessKey?: string;

  /**
   * 会签步骤序号（询价=1，PR=2，报销=3）
   */
  stepNo?: number;

  /**
   * 会签部门（选项 TaktDepts/tree-options；DictValue=Id；多选 JSON）
   */
  countersignDepts?: string;

  /**
   * 财务部门 JSON
   */
  financeDept?: string;

  /**
   * 预算审核意见
   */
  budgetReviewComment?: string;

  /**
   * 总经室 JSON
   */
  executiveOffice?: string;

  /**
   * 申请人（选项 TaktEmployees/options；DictValue=Id）
   */
  applicantBy?: string;

  /**
   * 申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）
   */
  applicantName?: string;

  /**
   * 申请部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  applicationDeptId?: string;

  /**
   * 申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）
   */
  applicationDeptName?: string;

  /**
   * 经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  costBearerDeptId?: string;

  /**
   * 经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）
   */
  costBearerDeptName?: string;

  /**
   * 预算否（字典 sys_yes_no）
   */
  isBudget?: number;

  /**
   * 预算项目（选项 TaktBudgetActuals/options；DictValue=Id）
   */
  budgetItemId?: string;

  /**
   * 预算项目名称（冗余：按 BudgetItemId 取 TaktBudgetActual.BudgetItemName 联动）
   */
  budgetItem?: string;

  /**
   * 预算金额
   */
  budgetAmount?: number;

  /**
   * 申请金额
   */
  applicationAmount?: number;

  /**
   * 标题
   */
  countersignTitle?: string;

  /**
   * 申请原因
   */
  applicationReason?: string;

  /**
   * 预算使用说明
   */
  budgetUsageDescription?: string;

  /**
   * 目标与预期效益
   */
  targetAndExpectedBenefit?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
   */
  countersignStatus?: number;

  /**
   * 会签单明细列表（主子表关系）（子表，级联保存）
   */
  countersignDetails?: CountersignDetailCreate[];

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * Countersign 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CountersignImport
 * @description 对应后端 TaktCountersignImportDto
 */
export interface CountersignImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 会签编码
   */
  countersignCode?: string;

  /**
   * 来源采购询价（选项 TaktPurchaseInquiries/options；DictValue=Id）
   */
  purchaseInquiryId?: string;

  /**
   * 来源采购询价编码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.PurchaseInquiryCode 联动）
   */
  purchaseInquiryCode?: string;

  /**
   * 会签业务类型（字典 accounting_financial_countersign_business_type：inquiry/pr/expense/standalone）
   */
  businessType?: string;

  /**
   * 会签业务键（如 inquiry:123、pr:456、expense:789）
   */
  businessKey?: string;

  /**
   * 会签步骤序号（询价=1，PR=2，报销=3）
   */
  stepNo?: number;

  /**
   * 会签部门（选项 TaktDepts/tree-options；DictValue=Id；多选 JSON）
   */
  countersignDepts?: string;

  /**
   * 财务部门 JSON
   */
  financeDept?: string;

  /**
   * 预算审核意见
   */
  budgetReviewComment?: string;

  /**
   * 总经室 JSON
   */
  executiveOffice?: string;

  /**
   * 申请人（选项 TaktEmployees/options；DictValue=Id）
   */
  applicantBy?: string;

  /**
   * 申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）
   */
  applicantName?: string;

  /**
   * 申请部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  applicationDeptId?: string;

  /**
   * 申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）
   */
  applicationDeptName?: string;

  /**
   * 经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  costBearerDeptId?: string;

  /**
   * 经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）
   */
  costBearerDeptName?: string;

  /**
   * 预算否（字典 sys_yes_no）
   */
  isBudget?: number;

  /**
   * 预算项目（选项 TaktBudgetActuals/options；DictValue=Id）
   */
  budgetItemId?: string;

  /**
   * 预算项目名称（冗余：按 BudgetItemId 取 TaktBudgetActual.BudgetItemName 联动）
   */
  budgetItem?: string;

  /**
   * 预算金额
   */
  budgetAmount?: number;

  /**
   * 申请金额
   */
  applicationAmount?: number;

  /**
   * 标题
   */
  countersignTitle?: string;

  /**
   * 申请原因
   */
  applicationReason?: string;

  /**
   * 预算使用说明
   */
  budgetUsageDescription?: string;

  /**
   * 目标与预期效益
   */
  targetAndExpectedBenefit?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
   */
  countersignStatus?: number;

  /**
   * 会签单明细列表（主子表关系）（子表，级联保存）
   */
  countersignDetails?: CountersignDetailCreate[];

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}


/**
 * Countersign 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CountersignExport
 * @description 对应后端 TaktCountersignExportDto
 */
export interface CountersignExport {
  /**
   * CountersignID
   */
  countersignId: string;

  /**
   * 会签编码
   */
  countersignCode: string;

  /**
   * 来源采购询价（选项 TaktPurchaseInquiries/options；DictValue=Id）
   */
  purchaseInquiryId?: string;

  /**
   * 来源采购询价编码（冗余：按 PurchaseInquiryId 取 TaktPurchaseInquiry.PurchaseInquiryCode 联动）
   */
  purchaseInquiryCode?: string;

  /**
   * 会签业务类型（字典 accounting_financial_countersign_business_type：inquiry/pr/expense/standalone）
   */
  businessType: string;

  /**
   * 会签业务键（如 inquiry:123、pr:456、expense:789）
   */
  businessKey?: string;

  /**
   * 会签步骤序号（询价=1，PR=2，报销=3）
   */
  stepNo: number;

  /**
   * 会签部门（选项 TaktDepts/tree-options；DictValue=Id；多选 JSON）
   */
  countersignDepts?: string;

  /**
   * 财务部门 JSON
   */
  financeDept?: string;

  /**
   * 预算审核意见
   */
  budgetReviewComment?: string;

  /**
   * 总经室 JSON
   */
  executiveOffice?: string;

  /**
   * 申请人（选项 TaktEmployees/options；DictValue=Id）
   */
  applicantBy: string;

  /**
   * 申请人名称（冗余：按 ApplicantBy 取 TaktEmployee.EmployeeName 联动）
   */
  applicantName?: string;

  /**
   * 申请部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  applicationDeptId?: string;

  /**
   * 申请部门名称（冗余：按 ApplicationDeptId 取 TaktDept.DeptName1 联动）
   */
  applicationDeptName?: string;

  /**
   * 经费负担部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  costBearerDeptId?: string;

  /**
   * 经费负担部门名称（冗余：按 CostBearerDeptId 取 TaktDept.DeptName1 联动）
   */
  costBearerDeptName?: string;

  /**
   * 预算否（字典 sys_yes_no）
   */
  isBudget: number;

  /**
   * 预算项目（选项 TaktBudgetActuals/options；DictValue=Id）
   */
  budgetItemId?: string;

  /**
   * 预算项目名称（冗余：按 BudgetItemId 取 TaktBudgetActual.BudgetItemName 联动）
   */
  budgetItem?: string;

  /**
   * 预算金额
   */
  budgetAmount: number;

  /**
   * 申请金额
   */
  applicationAmount: number;

  /**
   * 标题
   */
  countersignTitle?: string;

  /**
   * 申请原因
   */
  applicationReason?: string;

  /**
   * 预算使用说明
   */
  budgetUsageDescription?: string;

  /**
   * 目标与预期效益
   */
  targetAndExpectedBenefit?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 会签单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
   */
  countersignStatus: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

