// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：materialDocument.types.ts
// 创建时间：2026-09-20
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Takt物料凭证主表实体（公司级）
 * 对应前端 TaktMaterialDocumentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MaterialDocument
 * @description 对应后端 TaktMaterialDocumentDto
 */
export interface MaterialDocument extends CompanyDtoBase {
  /**
   * MaterialDocumentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  materialDocumentId: string;

  /**
   * 物料凭证
   */
  materialDocumentCode: string;

  /**
   * 物料凭证的年份
   */
  materialDocumentYear: string;

  /**
   * 交易/事件类型（字典 logistics_material_document_transaction_event_type）
   */
  transactionEventType?: string;

  /**
   * 凭证类型（字典 logistics_material_document_type）
   */
  documentType?: string;

  /**
   * 凭证类型重新评估
   */
  revaluationType?: string;

  /**
   * 凭证日期
   */
  documentDate: string;

  /**
   * 过帐日期
   */
  postingDate: string;

  /**
   * 参照（最长 16，故 Length=16）
   */
  referenceCode?: string;

  /**
   * 凭证抬头文本（最长 25，故 Length=25）
   */
  headerText?: string;

  /**
   * 提货单（最长 16，故 Length=16）
   */
  billOfLadingCode?: string;

  /**
   * 交货单
   */
  deliveryCode?: string;

  /**
   * 事务代码
   */
  transactionCode?: string;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

  /**
   * 物料凭证行项目列表（主子表关系） （子表：TaktMaterialDocumentItem）
   */
  items?: MaterialDocumentItem[];

}


/**
 * MaterialDocument 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaterialDocumentQuery
 * @description 对应后端 TaktMaterialDocumentQueryDto
 */
export interface MaterialDocumentQuery extends TaktPagedQuery {
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
   * 物料凭证
   */
  materialDocumentCode?: string;

  /**
   * 物料凭证的年份
   */
  materialDocumentYear?: string;

  /**
   * 交易/事件类型（字典 logistics_material_document_transaction_event_type）
   */
  transactionEventType?: string;

  /**
   * 凭证类型（字典 logistics_material_document_type）
   */
  documentType?: string;

  /**
   * 凭证类型重新评估
   */
  revaluationType?: string;

  /**
   * 凭证日期（范围查询-开始）
   */
  documentDateStart?: string;

  /**
   * 凭证日期（范围查询-结束）
   */
  documentDateEnd?: string;

  /**
   * 过帐日期（范围查询-开始）
   */
  postingDateStart?: string;

  /**
   * 过帐日期（范围查询-结束）
   */
  postingDateEnd?: string;

  /**
   * 参照（最长 16，故 Length=16）
   */
  referenceCode?: string;

  /**
   * 凭证抬头文本（最长 25，故 Length=25）
   */
  headerText?: string;

  /**
   * 提货单（最长 16，故 Length=16）
   */
  billOfLadingCode?: string;

  /**
   * 交货单
   */
  deliveryCode?: string;

  /**
   * 事务代码
   */
  transactionCode?: string;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

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
 * 创建MaterialDocument DTO
 * 对应前端 MaterialDocumentCreate
 * @description 对应后端 TaktMaterialDocumentCreateDto
 */
export interface MaterialDocumentCreate {
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
   * 物料凭证
   */
  materialDocumentCode: string;

  /**
   * 物料凭证的年份
   */
  materialDocumentYear: string;

  /**
   * 交易/事件类型（字典 logistics_material_document_transaction_event_type）
   */
  transactionEventType?: string;

  /**
   * 凭证类型（字典 logistics_material_document_type）
   */
  documentType?: string;

  /**
   * 凭证类型重新评估
   */
  revaluationType?: string;

  /**
   * 凭证日期
   */
  documentDate: string;

  /**
   * 过帐日期
   */
  postingDate: string;

  /**
   * 参照（最长 16，故 Length=16）
   */
  referenceCode?: string;

  /**
   * 凭证抬头文本（最长 25，故 Length=25）
   */
  headerText?: string;

  /**
   * 提货单（最长 16，故 Length=16）
   */
  billOfLadingCode?: string;

  /**
   * 交货单
   */
  deliveryCode?: string;

  /**
   * 事务代码
   */
  transactionCode?: string;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

  /**
   * 物料凭证行项目列表（主子表关系）（子表，级联保存）
   */
  items?: MaterialDocumentItemCreate[];

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
 * 更新MaterialDocument DTO
 * 继承 TaktMaterialDocumentCreateDto，添加 MaterialDocumentId 字段
 * 对应前端 MaterialDocumentUpdate
 * @description 对应后端 TaktMaterialDocumentUpdateDto
 */
export interface MaterialDocumentUpdate extends MaterialDocumentCreate {
  /**
   * MaterialDocumentID（标识要更新的实体）
   */
  materialDocumentId: string;

  /**
   * 物料凭证行项目列表（主子表关系）（子表，级联保存）
   */
  items?: any;

}


/**
 * MaterialDocument 导入模板行 DTO
 * 对应前端 MaterialDocumentTemplate
 * @description 对应后端 TaktMaterialDocumentTemplateDto
 */
export interface MaterialDocumentTemplate {
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
   * 物料凭证
   */
  materialDocumentCode?: string;

  /**
   * 物料凭证的年份
   */
  materialDocumentYear?: string;

  /**
   * 交易/事件类型（字典 logistics_material_document_transaction_event_type）
   */
  transactionEventType?: string;

  /**
   * 凭证类型（字典 logistics_material_document_type）
   */
  documentType?: string;

  /**
   * 凭证类型重新评估
   */
  revaluationType?: string;

  /**
   * 凭证日期
   */
  documentDate?: string;

  /**
   * 过帐日期
   */
  postingDate?: string;

  /**
   * 参照（最长 16，故 Length=16）
   */
  referenceCode?: string;

  /**
   * 凭证抬头文本（最长 25，故 Length=25）
   */
  headerText?: string;

  /**
   * 提货单（最长 16，故 Length=16）
   */
  billOfLadingCode?: string;

  /**
   * 交货单
   */
  deliveryCode?: string;

  /**
   * 事务代码
   */
  transactionCode?: string;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

  /**
   * 物料凭证行项目列表（主子表关系）（子表，级联保存）
   */
  items?: MaterialDocumentItemCreate[];

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
 * MaterialDocument 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaterialDocumentImport
 * @description 对应后端 TaktMaterialDocumentImportDto
 */
export interface MaterialDocumentImport {
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
   * 物料凭证
   */
  materialDocumentCode?: string;

  /**
   * 物料凭证的年份
   */
  materialDocumentYear?: string;

  /**
   * 交易/事件类型（字典 logistics_material_document_transaction_event_type）
   */
  transactionEventType?: string;

  /**
   * 凭证类型（字典 logistics_material_document_type）
   */
  documentType?: string;

  /**
   * 凭证类型重新评估
   */
  revaluationType?: string;

  /**
   * 凭证日期
   */
  documentDate?: string;

  /**
   * 过帐日期
   */
  postingDate?: string;

  /**
   * 参照（最长 16，故 Length=16）
   */
  referenceCode?: string;

  /**
   * 凭证抬头文本（最长 25，故 Length=25）
   */
  headerText?: string;

  /**
   * 提货单（最长 16，故 Length=16）
   */
  billOfLadingCode?: string;

  /**
   * 交货单
   */
  deliveryCode?: string;

  /**
   * 事务代码
   */
  transactionCode?: string;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

  /**
   * 物料凭证行项目列表（主子表关系）（子表，级联保存）
   */
  items?: MaterialDocumentItemCreate[];

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
 * MaterialDocument 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaterialDocumentExport
 * @description 对应后端 TaktMaterialDocumentExportDto
 */
export interface MaterialDocumentExport {
  /**
   * MaterialDocumentID
   */
  materialDocumentId: string;

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
   * 物料凭证
   */
  materialDocumentCode: string;

  /**
   * 物料凭证的年份
   */
  materialDocumentYear: string;

  /**
   * 交易/事件类型（字典 logistics_material_document_transaction_event_type）
   */
  transactionEventType?: string;

  /**
   * 凭证类型（字典 logistics_material_document_type）
   */
  documentType?: string;

  /**
   * 凭证类型重新评估
   */
  revaluationType?: string;

  /**
   * 凭证日期
   */
  documentDate: string;

  /**
   * 过帐日期
   */
  postingDate: string;

  /**
   * 参照（最长 16，故 Length=16）
   */
  referenceCode?: string;

  /**
   * 凭证抬头文本（最长 25，故 Length=25）
   */
  headerText?: string;

  /**
   * 提货单（最长 16，故 Length=16）
   */
  billOfLadingCode?: string;

  /**
   * 交货单
   */
  deliveryCode?: string;

  /**
   * 事务代码
   */
  transactionCode?: string;

  /**
   * 过账人（当前登录用户对应的 EmployeeCode）
   */
  postedBy?: string;

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

