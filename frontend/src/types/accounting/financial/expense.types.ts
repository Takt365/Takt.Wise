// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：expense.types.ts
// 创建时间：2026-08-28
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
 * 费用单实体。继承审批基类，与 TaktFlowEngine 对接；ExpenseStatus 与 ApprovalStatus 取值对齐。
 * 对应前端 TaktExpenseDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 Expense
 * @description 对应后端 TaktExpenseDto
 */
export interface Expense extends ApprovalDtoBase {
  /**
   * ExpenseID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  expenseId: string;

  /**
   * 费用单编码（租户+公司内唯一）
   */
  expenseCode: string;

  /**
   * 费用标题
   */
  expenseTitle: string;

  /**
   * 费用类型（字典 accounting_financial_expense_type：1=月结供应商除原材料外的费用，2=月结供应商货款及公司其他费用，3=杂项购置费用）
   */
  expenseType: number;

  /**
   * 供应商编码（选项 TaktSuppliers/options；整单唯一，DictValue=Id）
   */
  supplierCode?: string;

  /**
   * 供应商名称（整单唯一）
   */
  supplierName1?: string;

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
   * 成本中心（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
   */
  costCenter?: string;

  /**
   * 关联会签单（选项 TaktCountersigns/options；DictValue=Id）
   */
  countersignId?: string;

  /**
   * 关联会签单（选项 TaktCountersigns/options；DictValue=Id）
   */
  countersignName?: string;

  /**
   * 来源采购订单编码（选项 TaktPurchaseOrders/options；采购链路自动生成时写入，DictValue=Id）
   */
  purchaseOrderCode?: string;

  /**
   * 来源采购申请编码（选项 TaktPurchaseRequests/options；采购链路自动生成时写入，DictValue=Id）
   */
  purchaseRequestCode?: string;

  /**
   * 费用金额
   */
  expenseAmount: number;

  /**
   * 税率（字典 accounting_financial_tax_rate_param；整单统一税率）
   */
  taxRate: number;

  /**
   * 税额（整单合计）
   */
  taxAmount: number;

  /**
   * 费用发生日期
   */
  expenseDate: string;

  /**
   * 申请原因
   */
  applicationReason?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
   */
  expenseStatus: number;

  /**
   * 费用单明细列表（主子表关系） （子表：TaktExpenseDetail）
   */
  expenseDetails?: ExpenseDetail[];

}


/**
 * Expense 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ExpenseQuery
 * @description 对应后端 TaktExpenseQueryDto
 */
export interface ExpenseQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 费用单编码（租户+公司内唯一）
   */
  expenseCode?: string;

  /**
   * 费用标题
   */
  expenseTitle?: string;

  /**
   * 费用类型（字典 accounting_financial_expense_type：1=月结供应商除原材料外的费用，2=月结供应商货款及公司其他费用，3=杂项购置费用）
   */
  expenseType?: number;

  /**
   * 供应商编码（选项 TaktSuppliers/options；整单唯一，DictValue=Id）
   */
  supplierCode?: string;

  /**
   * 供应商名称（整单唯一）
   */
  supplierName1?: string;

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
   * 成本中心（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
   */
  costCenter?: string;

  /**
   * 关联会签单（选项 TaktCountersigns/options；DictValue=Id）
   */
  countersignId?: string;

  /**
   * 来源采购订单编码（选项 TaktPurchaseOrders/options；采购链路自动生成时写入，DictValue=Id）
   */
  purchaseOrderCode?: string;

  /**
   * 来源采购申请编码（选项 TaktPurchaseRequests/options；采购链路自动生成时写入，DictValue=Id）
   */
  purchaseRequestCode?: string;

  /**
   * 费用金额
   */
  expenseAmount?: number;

  /**
   * 税率（字典 accounting_financial_tax_rate_param；整单统一税率）
   */
  taxRate?: number;

  /**
   * 税额（整单合计）
   */
  taxAmount?: number;

  /**
   * 费用发生日期（范围查询-开始）
   */
  expenseDateStart?: string;

  /**
   * 费用发生日期（范围查询-结束）
   */
  expenseDateEnd?: string;

  /**
   * 申请原因
   */
  applicationReason?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
   */
  expenseStatus?: number;

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
 * 创建Expense DTO
 * 对应前端 ExpenseCreate
 * @description 对应后端 TaktExpenseCreateDto
 */
export interface ExpenseCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode: string;

  /**
   * 费用单编码（租户+公司内唯一）
   */
  expenseCode: string;

  /**
   * 费用标题
   */
  expenseTitle: string;

  /**
   * 费用类型（字典 accounting_financial_expense_type：1=月结供应商除原材料外的费用，2=月结供应商货款及公司其他费用，3=杂项购置费用）
   */
  expenseType: number;

  /**
   * 供应商编码（选项 TaktSuppliers/options；整单唯一，DictValue=Id）
   */
  supplierCode?: string;

  /**
   * 供应商名称（整单唯一）
   */
  supplierName1?: string;

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
   * 成本中心（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
   */
  costCenter?: string;

  /**
   * 关联会签单（选项 TaktCountersigns/options；DictValue=Id）
   */
  countersignId?: string;

  /**
   * 来源采购订单编码（选项 TaktPurchaseOrders/options；采购链路自动生成时写入，DictValue=Id）
   */
  purchaseOrderCode?: string;

  /**
   * 来源采购申请编码（选项 TaktPurchaseRequests/options；采购链路自动生成时写入，DictValue=Id）
   */
  purchaseRequestCode?: string;

  /**
   * 费用金额
   */
  expenseAmount: number;

  /**
   * 税率（字典 accounting_financial_tax_rate_param；整单统一税率）
   */
  taxRate: number;

  /**
   * 税额（整单合计）
   */
  taxAmount: number;

  /**
   * 费用发生日期
   */
  expenseDate: string;

  /**
   * 申请原因
   */
  applicationReason?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
   */
  expenseStatus: number;

  /**
   * 费用单明细列表（主子表关系）（子表，级联保存）
   */
  expenseDetails?: ExpenseDetailCreate[];

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
 * 更新Expense DTO
 * 继承 TaktExpenseCreateDto，添加 ExpenseId 字段
 * 对应前端 ExpenseUpdate
 * @description 对应后端 TaktExpenseUpdateDto
 */
export interface ExpenseUpdate extends ExpenseCreate {
  /**
   * ExpenseID（标识要更新的实体）
   */
  expenseId: string;

  /**
   * 费用单明细列表（主子表关系）（子表，级联保存）
   */
  expenseDetails?: any;

}


/**
 * Expense 状态更新 DTO
 * 对应前端 ExpenseStatus
 * @description 对应后端 TaktExpenseStatusDto
 */
export interface ExpenseStatus {
  /**
   * ExpenseID
   */
  expenseId: string;

  /**
   * 费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
   */
  expenseStatus: number;

}


/**
 * Expense 导入模板行 DTO
 * 对应前端 ExpenseTemplate
 * @description 对应后端 TaktExpenseTemplateDto
 */
export interface ExpenseTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 费用单编码（租户+公司内唯一）
   */
  expenseCode?: string;

  /**
   * 费用标题
   */
  expenseTitle?: string;

  /**
   * 费用类型（字典 accounting_financial_expense_type：1=月结供应商除原材料外的费用，2=月结供应商货款及公司其他费用，3=杂项购置费用）
   */
  expenseType?: number;

  /**
   * 供应商编码（选项 TaktSuppliers/options；整单唯一，DictValue=Id）
   */
  supplierCode?: string;

  /**
   * 供应商名称（整单唯一）
   */
  supplierName1?: string;

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
   * 成本中心（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
   */
  costCenter?: string;

  /**
   * 关联会签单（选项 TaktCountersigns/options；DictValue=Id）
   */
  countersignId?: string;

  /**
   * 来源采购订单编码（选项 TaktPurchaseOrders/options；采购链路自动生成时写入，DictValue=Id）
   */
  purchaseOrderCode?: string;

  /**
   * 来源采购申请编码（选项 TaktPurchaseRequests/options；采购链路自动生成时写入，DictValue=Id）
   */
  purchaseRequestCode?: string;

  /**
   * 费用金额
   */
  expenseAmount?: number;

  /**
   * 税率（字典 accounting_financial_tax_rate_param；整单统一税率）
   */
  taxRate?: number;

  /**
   * 税额（整单合计）
   */
  taxAmount?: number;

  /**
   * 费用发生日期
   */
  expenseDate?: string;

  /**
   * 申请原因
   */
  applicationReason?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
   */
  expenseStatus?: number;

  /**
   * 费用单明细列表（主子表关系）（子表，级联保存）
   */
  expenseDetails?: ExpenseDetailCreate[];

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
 * Expense 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ExpenseImport
 * @description 对应后端 TaktExpenseImportDto
 */
export interface ExpenseImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 费用单编码（租户+公司内唯一）
   */
  expenseCode?: string;

  /**
   * 费用标题
   */
  expenseTitle?: string;

  /**
   * 费用类型（字典 accounting_financial_expense_type：1=月结供应商除原材料外的费用，2=月结供应商货款及公司其他费用，3=杂项购置费用）
   */
  expenseType?: number;

  /**
   * 供应商编码（选项 TaktSuppliers/options；整单唯一，DictValue=Id）
   */
  supplierCode?: string;

  /**
   * 供应商名称（整单唯一）
   */
  supplierName1?: string;

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
   * 成本中心（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
   */
  costCenter?: string;

  /**
   * 关联会签单（选项 TaktCountersigns/options；DictValue=Id）
   */
  countersignId?: string;

  /**
   * 来源采购订单编码（选项 TaktPurchaseOrders/options；采购链路自动生成时写入，DictValue=Id）
   */
  purchaseOrderCode?: string;

  /**
   * 来源采购申请编码（选项 TaktPurchaseRequests/options；采购链路自动生成时写入，DictValue=Id）
   */
  purchaseRequestCode?: string;

  /**
   * 费用金额
   */
  expenseAmount?: number;

  /**
   * 税率（字典 accounting_financial_tax_rate_param；整单统一税率）
   */
  taxRate?: number;

  /**
   * 税额（整单合计）
   */
  taxAmount?: number;

  /**
   * 费用发生日期
   */
  expenseDate?: string;

  /**
   * 申请原因
   */
  applicationReason?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
   */
  expenseStatus?: number;

  /**
   * 费用单明细列表（主子表关系）（子表，级联保存）
   */
  expenseDetails?: ExpenseDetailCreate[];

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
 * Expense 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ExpenseExport
 * @description 对应后端 TaktExpenseExportDto
 */
export interface ExpenseExport {
  /**
   * ExpenseID
   */
  expenseId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 费用单编码（租户+公司内唯一）
   */
  expenseCode: string;

  /**
   * 费用标题
   */
  expenseTitle: string;

  /**
   * 费用类型（字典 accounting_financial_expense_type：1=月结供应商除原材料外的费用，2=月结供应商货款及公司其他费用，3=杂项购置费用）
   */
  expenseType: number;

  /**
   * 供应商编码（选项 TaktSuppliers/options；整单唯一，DictValue=Id）
   */
  supplierCode?: string;

  /**
   * 供应商名称（整单唯一）
   */
  supplierName1?: string;

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
   * 成本中心（关联 TaktCostCenter.CostCenterCode，选项 TaktCostCenters/tree-options）
   */
  costCenter?: string;

  /**
   * 关联会签单（选项 TaktCountersigns/options；DictValue=Id）
   */
  countersignId?: string;

  /**
   * 来源采购订单编码（选项 TaktPurchaseOrders/options；采购链路自动生成时写入，DictValue=Id）
   */
  purchaseOrderCode?: string;

  /**
   * 来源采购申请编码（选项 TaktPurchaseRequests/options；采购链路自动生成时写入，DictValue=Id）
   */
  purchaseRequestCode?: string;

  /**
   * 费用金额
   */
  expenseAmount: number;

  /**
   * 税率（字典 accounting_financial_tax_rate_param；整单统一税率）
   */
  taxRate: number;

  /**
   * 税额（整单合计）
   */
  taxAmount: number;

  /**
   * 费用发生日期
   */
  expenseDate: string;

  /**
   * 申请原因
   */
  applicationReason?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 费用单状态（字典 sys_approval_status；与 ApprovalStatus 取值一致）
   */
  expenseStatus: number;

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

