// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：customerComplaintItem.types.ts
// 创建时间：2026-09-04
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
 * 客诉明细实体
 * 对应前端 TaktCustomerComplaintItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CustomerComplaintItem
 * @description 对应后端 TaktCustomerComplaintItemDto
 */
export interface CustomerComplaintItem extends CompanyDtoBase {
  /**
   * CustomerComplaintItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  customerComplaintItemId: string;

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
  customerComplaintCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  productCode?: string;

  /**
   * 产品名称
   */
  productName?: string;

  /**
   * 批次号
   */
  batchCode?: string;

  /**
   * 不良项目类型（字典 logistics_quality_complaint_item_type）
   */
  itemType: number;

  /**
   * 不良现象描述
   */
  defectDescription: string;

  /**
   * 缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）
   */
  defectLevel: string;

  /**
   * 不良数量
   */
  defectQuantity: number;

  /**
   * 不良率（%）
   */
  defectRate?: number;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策
   */
  improvementAction?: string;

  /**
   * 改善组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=3；DictValue=ComplaintGroupCode）
   */
  improvementResponsibleGroupCode?: string;

  /**
   * 改善组名称（冗余：按 ImprovementResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  improvementResponsibleGroupName?: string;

  /**
   * 计划完成日期
   */
  plannedCompletionDate?: string;

  /**
   * 实际完成日期
   */
  actualCompletionDate?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 改善状态（字典 logistics_quality_improvement_status）
   */
  improvementStatus: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 客诉主表 （主表：TaktCustomerComplaint）
   */
  complaint?: CustomerComplaint;

}


/**
 * CustomerComplaintItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CustomerComplaintItemQuery
 * @description 对应后端 TaktCustomerComplaintItemQueryDto
 */
export interface CustomerComplaintItemQuery extends TaktPagedQuery {
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
   * 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  complaintId?: string;

  /**
   * 客诉单号（冗余：按对应 Id 取主数据名称联动）
   */
  customerComplaintCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  productCode?: string;

  /**
   * 产品名称
   */
  productName?: string;

  /**
   * 批次号
   */
  batchCode?: string;

  /**
   * 不良项目类型（字典 logistics_quality_complaint_item_type）
   */
  itemType?: number;

  /**
   * 不良现象描述
   */
  defectDescription?: string;

  /**
   * 缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）
   */
  defectLevel?: string;

  /**
   * 不良数量
   */
  defectQuantity?: number;

  /**
   * 不良率（%）
   */
  defectRate?: number;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策
   */
  improvementAction?: string;

  /**
   * 改善组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=3；DictValue=ComplaintGroupCode）
   */
  improvementResponsibleGroupCode?: string;

  /**
   * 改善组名称（冗余：按 ImprovementResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  improvementResponsibleGroupName?: string;

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
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 改善状态（字典 logistics_quality_improvement_status）
   */
  improvementStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * 创建CustomerComplaintItem DTO
 * 对应前端 CustomerComplaintItemCreate
 * @description 对应后端 TaktCustomerComplaintItemCreateDto
 */
export interface CustomerComplaintItemCreate {
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
   * 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  complaintId: string;

  /**
   * 客诉单号（冗余：按对应 Id 取主数据名称联动）
   */
  customerComplaintCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  productCode?: string;

  /**
   * 产品名称
   */
  productName?: string;

  /**
   * 批次号
   */
  batchCode?: string;

  /**
   * 不良项目类型（字典 logistics_quality_complaint_item_type）
   */
  itemType: number;

  /**
   * 不良现象描述
   */
  defectDescription: string;

  /**
   * 缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）
   */
  defectLevel: string;

  /**
   * 不良数量
   */
  defectQuantity: number;

  /**
   * 不良率（%）
   */
  defectRate?: number;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策
   */
  improvementAction?: string;

  /**
   * 改善组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=3；DictValue=ComplaintGroupCode）
   */
  improvementResponsibleGroupCode?: string;

  /**
   * 改善组名称（冗余：按 ImprovementResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  improvementResponsibleGroupName?: string;

  /**
   * 计划完成日期
   */
  plannedCompletionDate?: string;

  /**
   * 实际完成日期
   */
  actualCompletionDate?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 改善状态（字典 logistics_quality_improvement_status）
   */
  improvementStatus: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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
 * 更新CustomerComplaintItem DTO
 * 继承 TaktCustomerComplaintItemCreateDto，添加 CustomerComplaintItemId 字段
 * 对应前端 CustomerComplaintItemUpdate
 * @description 对应后端 TaktCustomerComplaintItemUpdateDto
 */
export interface CustomerComplaintItemUpdate extends CustomerComplaintItemCreate {
  /**
   * CustomerComplaintItemID（标识要更新的实体）
   */
  customerComplaintItemId: string;

}


/**
 * CustomerComplaintItem 状态更新 DTO
 * 对应前端 CustomerComplaintItemStatus
 * @description 对应后端 TaktCustomerComplaintItemStatusDto
 */
export interface CustomerComplaintItemStatus {
  /**
   * CustomerComplaintItemID
   */
  customerComplaintItemId: string;

  /**
   * 改善状态（字典 logistics_quality_improvement_status）
   */
  improvementStatus: number;

}


/**
 * CustomerComplaintItem 作废/撤销作废 DTO
 * 对应前端 CustomerComplaintItemObsolete
 * @description 对应后端 TaktCustomerComplaintItemObsoleteDto
 */
export interface CustomerComplaintItemObsolete {
  /**
   * CustomerComplaintItemID
   */
  customerComplaintItemId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * CustomerComplaintItem 导入模板行 DTO
 * 对应前端 CustomerComplaintItemTemplate
 * @description 对应后端 TaktCustomerComplaintItemTemplateDto
 */
export interface CustomerComplaintItemTemplate {
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
   * 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  complaintId?: string;

  /**
   * 客诉单号（冗余：按对应 Id 取主数据名称联动）
   */
  customerComplaintCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  productCode?: string;

  /**
   * 产品名称
   */
  productName?: string;

  /**
   * 批次号
   */
  batchCode?: string;

  /**
   * 不良项目类型（字典 logistics_quality_complaint_item_type）
   */
  itemType?: number;

  /**
   * 不良现象描述
   */
  defectDescription?: string;

  /**
   * 缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）
   */
  defectLevel?: string;

  /**
   * 不良数量
   */
  defectQuantity?: number;

  /**
   * 不良率（%）
   */
  defectRate?: number;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策
   */
  improvementAction?: string;

  /**
   * 改善组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=3；DictValue=ComplaintGroupCode）
   */
  improvementResponsibleGroupCode?: string;

  /**
   * 改善组名称（冗余：按 ImprovementResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  improvementResponsibleGroupName?: string;

  /**
   * 计划完成日期
   */
  plannedCompletionDate?: string;

  /**
   * 实际完成日期
   */
  actualCompletionDate?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 改善状态（字典 logistics_quality_improvement_status）
   */
  improvementStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * CustomerComplaintItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CustomerComplaintItemImport
 * @description 对应后端 TaktCustomerComplaintItemImportDto
 */
export interface CustomerComplaintItemImport {
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
   * 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  complaintId?: string;

  /**
   * 客诉单号（冗余：按对应 Id 取主数据名称联动）
   */
  customerComplaintCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  productCode?: string;

  /**
   * 产品名称
   */
  productName?: string;

  /**
   * 批次号
   */
  batchCode?: string;

  /**
   * 不良项目类型（字典 logistics_quality_complaint_item_type）
   */
  itemType?: number;

  /**
   * 不良现象描述
   */
  defectDescription?: string;

  /**
   * 缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）
   */
  defectLevel?: string;

  /**
   * 不良数量
   */
  defectQuantity?: number;

  /**
   * 不良率（%）
   */
  defectRate?: number;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策
   */
  improvementAction?: string;

  /**
   * 改善组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=3；DictValue=ComplaintGroupCode）
   */
  improvementResponsibleGroupCode?: string;

  /**
   * 改善组名称（冗余：按 ImprovementResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  improvementResponsibleGroupName?: string;

  /**
   * 计划完成日期
   */
  plannedCompletionDate?: string;

  /**
   * 实际完成日期
   */
  actualCompletionDate?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 改善状态（字典 logistics_quality_improvement_status）
   */
  improvementStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * CustomerComplaintItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CustomerComplaintItemExport
 * @description 对应后端 TaktCustomerComplaintItemExportDto
 */
export interface CustomerComplaintItemExport {
  /**
   * CustomerComplaintItemID
   */
  customerComplaintItemId: string;

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
   * 客诉 ID（选项 TaktCustomerComplaints/options；DictValue=Id）
   */
  complaintId: string;

  /**
   * 客诉单号（冗余：按对应 Id 取主数据名称联动）
   */
  customerComplaintCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  productCode?: string;

  /**
   * 产品名称
   */
  productName?: string;

  /**
   * 批次号
   */
  batchCode?: string;

  /**
   * 不良项目类型（字典 logistics_quality_complaint_item_type）
   */
  itemType: number;

  /**
   * 不良现象描述
   */
  defectDescription: string;

  /**
   * 缺点等级（字典 logistics_quality_defect_severity_code；DictValue=CR/MA/MI）
   */
  defectLevel: string;

  /**
   * 不良数量
   */
  defectQuantity: number;

  /**
   * 不良率（%）
   */
  defectRate?: number;

  /**
   * 原因分析
   */
  causeAnalysis?: string;

  /**
   * 改善对策
   */
  improvementAction?: string;

  /**
   * 改善组编码（选项 TaktComplaintGroups/options?complaintGroupCategory=3；DictValue=ComplaintGroupCode）
   */
  improvementResponsibleGroupCode?: string;

  /**
   * 改善组名称（冗余：按 ImprovementResponsibleGroupCode 取 TaktComplaintGroups.ComplaintGroupName 联动）
   */
  improvementResponsibleGroupName?: string;

  /**
   * 计划完成日期
   */
  plannedCompletionDate?: string;

  /**
   * 实际完成日期
   */
  actualCompletionDate?: string;

  /**
   * 文件名称（原始文件名，长度对齐 TaktFile.FileName）
   */
  fileName?: string;

  /**
   * 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
   */
  accessUrl?: string;

  /**
   * 改善状态（字典 logistics_quality_improvement_status）
   */
  improvementStatus: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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

