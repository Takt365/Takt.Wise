// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：incidentItem.types.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/cost 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 品质事故明细 - 废弃零件明细行
 * 对应前端 TaktQualityIncidentItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityIncidentItem
 * @description 对应后端 TaktQualityIncidentItemDto
 */
export interface QualityIncidentItem extends CompanyDtoBase {
  /**
   * QualityIncidentItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityIncidentItemId: string;

  /**
   * 品质事故主表 ID（关联 TaktQualityIncident.Id，选项 TaktQualityIncidents/options）
   */
  qualityIncidentId: string;

  /**
   * 品质事故主表 名称（填充字段）
   */
  qualityIncidentName?: string;

  /**
   * 品质事故编码（冗余字段，便于查询）
   */
  qualityIncidentCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options，DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料描述
   */
  materialDescription: string;

  /**
   * 废弃费用(元)
   */
  scrapCost: number;

  /**
   * 废弃数量
   */
  scrapSize: number;

  /**
   * 零件单价(元)
   */
  partPrice: number;

  /**
   * 废弃处理费用(元)
   */
  scrapReasonCost: number;

  /**
   * 运费(元)
   */
  freightCharges: number;

  /**
   * 其他费用(元)
   */
  otherExpenses: number;

  /**
   * 处理作业时间(分钟)
   */
  reasonWorkTimeMinutes: number;

  /**
   * 关税(元)
   */
  tax: number;

  /**
   * 处理发生其他费用(元)
   */
  reasonOtherExpenses: number;

  /**
   * 废弃备注
   */
  scrapNote?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 品质事故主表(导航属性) （主表：TaktQualityIncident）
   */
  incident?: QualityIncident;

}


/**
 * QualityIncidentItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityIncidentItemQuery
 * @description 对应后端 TaktQualityIncidentItemQueryDto
 */
export interface QualityIncidentItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 品质事故主表 ID（关联 TaktQualityIncident.Id，选项 TaktQualityIncidents/options）
   */
  qualityIncidentId?: string;

  /**
   * 品质事故编码（冗余字段，便于查询）
   */
  qualityIncidentCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options，DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 废弃费用(元)
   */
  scrapCost?: number;

  /**
   * 废弃数量
   */
  scrapSize?: number;

  /**
   * 零件单价(元)
   */
  partPrice?: number;

  /**
   * 废弃处理费用(元)
   */
  scrapReasonCost?: number;

  /**
   * 运费(元)
   */
  freightCharges?: number;

  /**
   * 其他费用(元)
   */
  otherExpenses?: number;

  /**
   * 处理作业时间(分钟)
   */
  reasonWorkTimeMinutes?: number;

  /**
   * 关税(元)
   */
  tax?: number;

  /**
   * 处理发生其他费用(元)
   */
  reasonOtherExpenses?: number;

  /**
   * 废弃备注
   */
  scrapNote?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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
 * 创建QualityIncidentItem DTO
 * 对应前端 QualityIncidentItemCreate
 * @description 对应后端 TaktQualityIncidentItemCreateDto
 */
export interface QualityIncidentItemCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 品质事故主表 ID（关联 TaktQualityIncident.Id，选项 TaktQualityIncidents/options）
   */
  qualityIncidentId: string;

  /**
   * 品质事故编码（冗余字段，便于查询）
   */
  qualityIncidentCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options，DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料描述
   */
  materialDescription: string;

  /**
   * 废弃费用(元)
   */
  scrapCost: number;

  /**
   * 废弃数量
   */
  scrapSize: number;

  /**
   * 零件单价(元)
   */
  partPrice: number;

  /**
   * 废弃处理费用(元)
   */
  scrapReasonCost: number;

  /**
   * 运费(元)
   */
  freightCharges: number;

  /**
   * 其他费用(元)
   */
  otherExpenses: number;

  /**
   * 处理作业时间(分钟)
   */
  reasonWorkTimeMinutes: number;

  /**
   * 关税(元)
   */
  tax: number;

  /**
   * 处理发生其他费用(元)
   */
  reasonOtherExpenses: number;

  /**
   * 废弃备注
   */
  scrapNote?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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
 * 更新QualityIncidentItem DTO
 * 继承 TaktQualityIncidentItemCreateDto，添加 QualityIncidentItemId 字段
 * 对应前端 QualityIncidentItemUpdate
 * @description 对应后端 TaktQualityIncidentItemUpdateDto
 */
export interface QualityIncidentItemUpdate extends QualityIncidentItemCreate {
  /**
   * QualityIncidentItemID（标识要更新的实体）
   */
  qualityIncidentItemId: string;

}


/**
 * QualityIncidentItem 作废/撤销作废 DTO
 * 对应前端 QualityIncidentItemObsolete
 * @description 对应后端 TaktQualityIncidentItemObsoleteDto
 */
export interface QualityIncidentItemObsolete {
  /**
   * QualityIncidentItemID
   */
  qualityIncidentItemId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * QualityIncidentItem 导入模板行 DTO
 * 对应前端 QualityIncidentItemTemplate
 * @description 对应后端 TaktQualityIncidentItemTemplateDto
 */
export interface QualityIncidentItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 品质事故主表 ID（关联 TaktQualityIncident.Id，选项 TaktQualityIncidents/options）
   */
  qualityIncidentId?: string;

  /**
   * 品质事故编码（冗余字段，便于查询）
   */
  qualityIncidentCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options，DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 废弃费用(元)
   */
  scrapCost?: number;

  /**
   * 废弃数量
   */
  scrapSize?: number;

  /**
   * 零件单价(元)
   */
  partPrice?: number;

  /**
   * 废弃处理费用(元)
   */
  scrapReasonCost?: number;

  /**
   * 运费(元)
   */
  freightCharges?: number;

  /**
   * 其他费用(元)
   */
  otherExpenses?: number;

  /**
   * 处理作业时间(分钟)
   */
  reasonWorkTimeMinutes?: number;

  /**
   * 关税(元)
   */
  tax?: number;

  /**
   * 处理发生其他费用(元)
   */
  reasonOtherExpenses?: number;

  /**
   * 废弃备注
   */
  scrapNote?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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
 * QualityIncidentItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityIncidentItemImport
 * @description 对应后端 TaktQualityIncidentItemImportDto
 */
export interface QualityIncidentItemImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 品质事故主表 ID（关联 TaktQualityIncident.Id，选项 TaktQualityIncidents/options）
   */
  qualityIncidentId?: string;

  /**
   * 品质事故编码（冗余字段，便于查询）
   */
  qualityIncidentCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options，DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 废弃费用(元)
   */
  scrapCost?: number;

  /**
   * 废弃数量
   */
  scrapSize?: number;

  /**
   * 零件单价(元)
   */
  partPrice?: number;

  /**
   * 废弃处理费用(元)
   */
  scrapReasonCost?: number;

  /**
   * 运费(元)
   */
  freightCharges?: number;

  /**
   * 其他费用(元)
   */
  otherExpenses?: number;

  /**
   * 处理作业时间(分钟)
   */
  reasonWorkTimeMinutes?: number;

  /**
   * 关税(元)
   */
  tax?: number;

  /**
   * 处理发生其他费用(元)
   */
  reasonOtherExpenses?: number;

  /**
   * 废弃备注
   */
  scrapNote?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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
 * QualityIncidentItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityIncidentItemExport
 * @description 对应后端 TaktQualityIncidentItemExportDto
 */
export interface QualityIncidentItemExport {
  /**
   * QualityIncidentItemID
   */
  qualityIncidentItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 品质事故主表 ID（关联 TaktQualityIncident.Id，选项 TaktQualityIncidents/options）
   */
  qualityIncidentId: string;

  /**
   * 品质事故编码（冗余字段，便于查询）
   */
  qualityIncidentCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options，DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料描述
   */
  materialDescription: string;

  /**
   * 废弃费用(元)
   */
  scrapCost: number;

  /**
   * 废弃数量
   */
  scrapSize: number;

  /**
   * 零件单价(元)
   */
  partPrice: number;

  /**
   * 废弃处理费用(元)
   */
  scrapReasonCost: number;

  /**
   * 运费(元)
   */
  freightCharges: number;

  /**
   * 其他费用(元)
   */
  otherExpenses: number;

  /**
   * 处理作业时间(分钟)
   */
  reasonWorkTimeMinutes: number;

  /**
   * 关税(元)
   */
  tax: number;

  /**
   * 处理发生其他费用(元)
   */
  reasonOtherExpenses: number;

  /**
   * 废弃备注
   */
  scrapNote?: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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

