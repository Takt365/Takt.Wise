// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：customerComplaintHandling.types.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/complaint 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 客诉处理记录实体
 * 对应前端 TaktCustomerComplaintHandlingDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CustomerComplaintHandling
 * @description 对应后端 TaktCustomerComplaintHandlingDto
 */
export interface CustomerComplaintHandling extends CompanyDtoBase {
  /**
   * CustomerComplaintHandlingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  customerComplaintHandlingId: string;

  /**
   * 客诉处理记录编码（唯一索引）
   */
  complaintHandlingCode: string;

  /**
   * 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  complaintId: string;

  /**
   * 客诉 名称（填充字段）
   */
  complaintName?: string;

  /**
   * 客诉单号（冗余：按对应 Id 取主数据名称联动）
   */
  complaintCode: string;

  /**
   * 客诉明细 ID（选项 TaktCustomerComplaintItems/options；DictValue=Id）
   */
  complaintItemId?: string;

  /**
   * 客诉明细 名称（填充字段）
   */
  complaintItemName?: string;

  /**
   * 处理阶段（字典 logistics_quality_complaint_handling_stage）
   */
  handlingStage: number;

  /**
   * 处理方式（字典 logistics_quality_complaint_handling_method）
   */
  handlingMethod: number;

  /**
   * 处理说明
   */
  handlingDescription: string;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策/纠正措施
   */
  correctiveAction?: string;

  /**
   * 预防措施
   */
  preventiveAction?: string;

  /**
   * 责任部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  responsibleDeptId?: string;

  /**
   * 责任部门名称（冗余：按 ResponsibleDeptId 取 TaktDept.DeptName1 联动）
   */
  responsibleDeptName?: string;

  /**
   * 责任组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=1；DictValue=ComplaintGroupCode）
   */
  responsibleGroupCode?: string;

  /**
   * 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  responsibleGroupName?: string;

  /**
   * 处理组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=2；DictValue=ComplaintGroupCode）
   */
  handlerGroupCode?: string;

  /**
   * 处理组名称（冗余：按 HandlerGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  handlerGroupName?: string;

  /**
   * 处理时间
   */
  handlingAt?: string;

  /**
   * 计划完成日期
   */
  plannedCompletionDate?: string;

  /**
   * 实际完成日期
   */
  actualCompletionDate?: string;

  /**
   * 处理成本/损失金额
   */
  handlingCost?: number;

  /**
   * 客户反馈
   */
  customerFeedback?: string;

  /**
   * 客户满意度（字典 logistics_quality_customer_satisfaction）
   */
  customerSatisfaction?: number;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 处理状态（字典 logistics_quality_complaint_handling_status）
   */
  handlingStatus: number;

  /**
   * 客诉主表 （主表：TaktCustomerComplaint）
   */
  complaint?: CustomerComplaint;

}


/**
 * CustomerComplaintHandling 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CustomerComplaintHandlingQuery
 * @description 对应后端 TaktCustomerComplaintHandlingQueryDto
 */
export interface CustomerComplaintHandlingQuery extends TaktPagedQuery {
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
   * 客诉处理记录编码（唯一索引）
   */
  complaintHandlingCode?: string;

  /**
   * 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  complaintId?: string;

  /**
   * 客诉单号（冗余：按对应 Id 取主数据名称联动）
   */
  complaintCode?: string;

  /**
   * 客诉明细 ID（选项 TaktCustomerComplaintItems/options；DictValue=Id）
   */
  complaintItemId?: string;

  /**
   * 处理阶段（字典 logistics_quality_complaint_handling_stage）
   */
  handlingStage?: number;

  /**
   * 处理方式（字典 logistics_quality_complaint_handling_method）
   */
  handlingMethod?: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策/纠正措施
   */
  correctiveAction?: string;

  /**
   * 预防措施
   */
  preventiveAction?: string;

  /**
   * 责任部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  responsibleDeptId?: string;

  /**
   * 责任部门名称（冗余：按 ResponsibleDeptId 取 TaktDept.DeptName1 联动）
   */
  responsibleDeptName?: string;

  /**
   * 责任组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=1；DictValue=ComplaintGroupCode）
   */
  responsibleGroupCode?: string;

  /**
   * 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  responsibleGroupName?: string;

  /**
   * 处理组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=2；DictValue=ComplaintGroupCode）
   */
  handlerGroupCode?: string;

  /**
   * 处理组名称（冗余：按 HandlerGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  handlerGroupName?: string;

  /**
   * 处理时间（范围查询-开始）
   */
  handlingAtStart?: string;

  /**
   * 处理时间（范围查询-结束）
   */
  handlingAtEnd?: string;

  /**
   * 计划完成日期（范围查询-开始）
   */
  plannedCompletionDateStart?: string;

  /**
   * 计划完成日期（范围查询-结束）
   */
  plannedCompletionDateEnd?: string;

  /**
   * 实际完成日期（范围查询-开始）
   */
  actualCompletionDateStart?: string;

  /**
   * 实际完成日期（范围查询-结束）
   */
  actualCompletionDateEnd?: string;

  /**
   * 处理成本/损失金额
   */
  handlingCost?: number;

  /**
   * 客户反馈
   */
  customerFeedback?: string;

  /**
   * 客户满意度（字典 logistics_quality_customer_satisfaction）
   */
  customerSatisfaction?: number;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 处理状态（字典 logistics_quality_complaint_handling_status）
   */
  handlingStatus?: number;

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
 * 创建CustomerComplaintHandling DTO
 * 对应前端 CustomerComplaintHandlingCreate
 * @description 对应后端 TaktCustomerComplaintHandlingCreateDto
 */
export interface CustomerComplaintHandlingCreate {
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
   * 客诉处理记录编码（唯一索引）
   */
  complaintHandlingCode: string;

  /**
   * 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  complaintId: string;

  /**
   * 客诉单号（冗余：按对应 Id 取主数据名称联动）
   */
  complaintCode: string;

  /**
   * 客诉明细 ID（选项 TaktCustomerComplaintItems/options；DictValue=Id）
   */
  complaintItemId?: string;

  /**
   * 处理阶段（字典 logistics_quality_complaint_handling_stage）
   */
  handlingStage: number;

  /**
   * 处理方式（字典 logistics_quality_complaint_handling_method）
   */
  handlingMethod: number;

  /**
   * 处理说明
   */
  handlingDescription: string;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策/纠正措施
   */
  correctiveAction?: string;

  /**
   * 预防措施
   */
  preventiveAction?: string;

  /**
   * 责任部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  responsibleDeptId?: string;

  /**
   * 责任部门名称（冗余：按 ResponsibleDeptId 取 TaktDept.DeptName1 联动）
   */
  responsibleDeptName?: string;

  /**
   * 责任组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=1；DictValue=ComplaintGroupCode）
   */
  responsibleGroupCode?: string;

  /**
   * 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  responsibleGroupName?: string;

  /**
   * 处理组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=2；DictValue=ComplaintGroupCode）
   */
  handlerGroupCode?: string;

  /**
   * 处理组名称（冗余：按 HandlerGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  handlerGroupName?: string;

  /**
   * 处理时间
   */
  handlingAt?: string;

  /**
   * 计划完成日期
   */
  plannedCompletionDate?: string;

  /**
   * 实际完成日期
   */
  actualCompletionDate?: string;

  /**
   * 处理成本/损失金额
   */
  handlingCost?: number;

  /**
   * 客户反馈
   */
  customerFeedback?: string;

  /**
   * 客户满意度（字典 logistics_quality_customer_satisfaction）
   */
  customerSatisfaction?: number;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 处理状态（字典 logistics_quality_complaint_handling_status）
   */
  handlingStatus: number;

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
 * 更新CustomerComplaintHandling DTO
 * 继承 TaktCustomerComplaintHandlingCreateDto，添加 CustomerComplaintHandlingId 字段
 * 对应前端 CustomerComplaintHandlingUpdate
 * @description 对应后端 TaktCustomerComplaintHandlingUpdateDto
 */
export interface CustomerComplaintHandlingUpdate extends CustomerComplaintHandlingCreate {
  /**
   * CustomerComplaintHandlingID（标识要更新的实体）
   */
  customerComplaintHandlingId: string;

}


/**
 * CustomerComplaintHandling 状态更新 DTO
 * 对应前端 CustomerComplaintHandlingStatus
 * @description 对应后端 TaktCustomerComplaintHandlingStatusDto
 */
export interface CustomerComplaintHandlingStatus {
  /**
   * CustomerComplaintHandlingID
   */
  customerComplaintHandlingId: string;

  /**
   * 处理状态（字典 logistics_quality_complaint_handling_status）
   */
  handlingStatus: number;

}


/**
 * CustomerComplaintHandling 导入模板行 DTO
 * 对应前端 CustomerComplaintHandlingTemplate
 * @description 对应后端 TaktCustomerComplaintHandlingTemplateDto
 */
export interface CustomerComplaintHandlingTemplate {
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
   * 客诉处理记录编码（唯一索引）
   */
  complaintHandlingCode?: string;

  /**
   * 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  complaintId?: string;

  /**
   * 客诉单号（冗余：按对应 Id 取主数据名称联动）
   */
  complaintCode?: string;

  /**
   * 客诉明细 ID（选项 TaktCustomerComplaintItems/options；DictValue=Id）
   */
  complaintItemId?: string;

  /**
   * 处理阶段（字典 logistics_quality_complaint_handling_stage）
   */
  handlingStage?: number;

  /**
   * 处理方式（字典 logistics_quality_complaint_handling_method）
   */
  handlingMethod?: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策/纠正措施
   */
  correctiveAction?: string;

  /**
   * 预防措施
   */
  preventiveAction?: string;

  /**
   * 责任部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  responsibleDeptId?: string;

  /**
   * 责任部门名称（冗余：按 ResponsibleDeptId 取 TaktDept.DeptName1 联动）
   */
  responsibleDeptName?: string;

  /**
   * 责任组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=1；DictValue=ComplaintGroupCode）
   */
  responsibleGroupCode?: string;

  /**
   * 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  responsibleGroupName?: string;

  /**
   * 处理组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=2；DictValue=ComplaintGroupCode）
   */
  handlerGroupCode?: string;

  /**
   * 处理组名称（冗余：按 HandlerGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  handlerGroupName?: string;

  /**
   * 处理时间
   */
  handlingAt?: string;

  /**
   * 计划完成日期
   */
  plannedCompletionDate?: string;

  /**
   * 实际完成日期
   */
  actualCompletionDate?: string;

  /**
   * 处理成本/损失金额
   */
  handlingCost?: number;

  /**
   * 客户反馈
   */
  customerFeedback?: string;

  /**
   * 客户满意度（字典 logistics_quality_customer_satisfaction）
   */
  customerSatisfaction?: number;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 处理状态（字典 logistics_quality_complaint_handling_status）
   */
  handlingStatus?: number;

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
 * CustomerComplaintHandling 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CustomerComplaintHandlingImport
 * @description 对应后端 TaktCustomerComplaintHandlingImportDto
 */
export interface CustomerComplaintHandlingImport {
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
   * 客诉处理记录编码（唯一索引）
   */
  complaintHandlingCode?: string;

  /**
   * 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  complaintId?: string;

  /**
   * 客诉单号（冗余：按对应 Id 取主数据名称联动）
   */
  complaintCode?: string;

  /**
   * 客诉明细 ID（选项 TaktCustomerComplaintItems/options；DictValue=Id）
   */
  complaintItemId?: string;

  /**
   * 处理阶段（字典 logistics_quality_complaint_handling_stage）
   */
  handlingStage?: number;

  /**
   * 处理方式（字典 logistics_quality_complaint_handling_method）
   */
  handlingMethod?: number;

  /**
   * 处理说明
   */
  handlingDescription?: string;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策/纠正措施
   */
  correctiveAction?: string;

  /**
   * 预防措施
   */
  preventiveAction?: string;

  /**
   * 责任部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  responsibleDeptId?: string;

  /**
   * 责任部门名称（冗余：按 ResponsibleDeptId 取 TaktDept.DeptName1 联动）
   */
  responsibleDeptName?: string;

  /**
   * 责任组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=1；DictValue=ComplaintGroupCode）
   */
  responsibleGroupCode?: string;

  /**
   * 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  responsibleGroupName?: string;

  /**
   * 处理组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=2；DictValue=ComplaintGroupCode）
   */
  handlerGroupCode?: string;

  /**
   * 处理组名称（冗余：按 HandlerGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  handlerGroupName?: string;

  /**
   * 处理时间
   */
  handlingAt?: string;

  /**
   * 计划完成日期
   */
  plannedCompletionDate?: string;

  /**
   * 实际完成日期
   */
  actualCompletionDate?: string;

  /**
   * 处理成本/损失金额
   */
  handlingCost?: number;

  /**
   * 客户反馈
   */
  customerFeedback?: string;

  /**
   * 客户满意度（字典 logistics_quality_customer_satisfaction）
   */
  customerSatisfaction?: number;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 处理状态（字典 logistics_quality_complaint_handling_status）
   */
  handlingStatus?: number;

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
 * CustomerComplaintHandling 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CustomerComplaintHandlingExport
 * @description 对应后端 TaktCustomerComplaintHandlingExportDto
 */
export interface CustomerComplaintHandlingExport {
  /**
   * CustomerComplaintHandlingID
   */
  customerComplaintHandlingId: string;

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
   * 客诉处理记录编码（唯一索引）
   */
  complaintHandlingCode: string;

  /**
   * 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  complaintId: string;

  /**
   * 客诉单号（冗余：按对应 Id 取主数据名称联动）
   */
  complaintCode: string;

  /**
   * 客诉明细 ID（选项 TaktCustomerComplaintItems/options；DictValue=Id）
   */
  complaintItemId?: string;

  /**
   * 处理阶段（字典 logistics_quality_complaint_handling_stage）
   */
  handlingStage: number;

  /**
   * 处理方式（字典 logistics_quality_complaint_handling_method）
   */
  handlingMethod: number;

  /**
   * 处理说明
   */
  handlingDescription: string;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策/纠正措施
   */
  correctiveAction?: string;

  /**
   * 预防措施
   */
  preventiveAction?: string;

  /**
   * 责任部门（选项 TaktDepts/tree-options；DictValue=Id）
   */
  responsibleDeptId?: string;

  /**
   * 责任部门名称（冗余：按 ResponsibleDeptId 取 TaktDept.DeptName1 联动）
   */
  responsibleDeptName?: string;

  /**
   * 责任组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=1；DictValue=ComplaintGroupCode）
   */
  responsibleGroupCode?: string;

  /**
   * 责任组名称（冗余：按 ResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  responsibleGroupName?: string;

  /**
   * 处理组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=2；DictValue=ComplaintGroupCode）
   */
  handlerGroupCode?: string;

  /**
   * 处理组名称（冗余：按 HandlerGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  handlerGroupName?: string;

  /**
   * 处理时间
   */
  handlingAt?: string;

  /**
   * 计划完成日期
   */
  plannedCompletionDate?: string;

  /**
   * 实际完成日期
   */
  actualCompletionDate?: string;

  /**
   * 处理成本/损失金额
   */
  handlingCost?: number;

  /**
   * 客户反馈
   */
  customerFeedback?: string;

  /**
   * 客户满意度（字典 logistics_quality_customer_satisfaction）
   */
  customerSatisfaction?: number;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 处理状态（字典 logistics_quality_complaint_handling_status）
   */
  handlingStatus: number;

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

