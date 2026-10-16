// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：taxCode.types.ts
// 创建时间：2026-10-14
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 税收代码实体（公司级；租户内公司 + 过程 + 税码 + 税类别唯一）
 * 对应前端 TaktTaxCodeDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 TaxCode
 * @description 对应后端 TaktTaxCodeDto
 */
export interface TaxCode extends CompanyDtoBase {
  /**
   * TaxCodeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  taxCodeId: string;

  /**
   * 过程（字典 accounting_financial_tax_procedure；DictValue=TAXD～TAXPL）
   */
  taxProcedureCode: string;

  /**
   * 税码（长度 2；同一公司、过程、税类别下唯一）
   */
  taxCode: string;

  /**
   * 税类别（字典 accounting_financial_tax_class；A=销项税 V=进项税）
   */
  taxCategory: string;

  /**
   * 描述（税码文本，如销项增值税、进项税档位说明；Length=50）
   */
  description: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  taxCodeStatus: number;

}


/**
 * TaxCode 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 TaxCodeQuery
 * @description 对应后端 TaktTaxCodeQueryDto
 */
export interface TaxCodeQuery extends TaktPagedQuery {
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
   * 过程（字典 accounting_financial_tax_procedure；DictValue=TAXD～TAXPL）
   */
  taxProcedureCode?: string;

  /**
   * 税码（长度 2；同一公司、过程、税类别下唯一）
   */
  taxCode?: string;

  /**
   * 税类别（字典 accounting_financial_tax_class；A=销项税 V=进项税）
   */
  taxCategory?: string;

  /**
   * 描述（税码文本，如销项增值税、进项税档位说明；Length=50）
   */
  description?: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  taxCodeStatus?: number;

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
 * 创建TaxCode DTO
 * 对应前端 TaxCodeCreate
 * @description 对应后端 TaktTaxCodeCreateDto
 */
export interface TaxCodeCreate {
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
   * 过程（字典 accounting_financial_tax_procedure；DictValue=TAXD～TAXPL）
   */
  taxProcedureCode: string;

  /**
   * 税码（长度 2；同一公司、过程、税类别下唯一）
   */
  taxCode: string;

  /**
   * 税类别（字典 accounting_financial_tax_class；A=销项税 V=进项税）
   */
  taxCategory: string;

  /**
   * 描述（税码文本，如销项增值税、进项税档位说明；Length=50）
   */
  description: string;

  /**
   * 税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  taxCodeStatus: number;

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
 * 更新TaxCode DTO
 * 继承 TaktTaxCodeCreateDto，添加 TaxCodeId 字段
 * 对应前端 TaxCodeUpdate
 * @description 对应后端 TaktTaxCodeUpdateDto
 */
export interface TaxCodeUpdate extends TaxCodeCreate {
  /**
   * TaxCodeID（标识要更新的实体）
   */
  taxCodeId: string;

}


/**
 * TaxCode 状态更新 DTO
 * 对应前端 TaxCodeStatus
 * @description 对应后端 TaktTaxCodeStatusDto
 */
export interface TaxCodeStatus {
  /**
   * TaxCodeID
   */
  taxCodeId: string;

  /**
   * 税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  taxCodeStatus: number;

}


/**
 * TaxCode 排序更新 DTO
 * 对应前端 TaxCodeSort
 * @description 对应后端 TaktTaxCodeSortDto
 */
export interface TaxCodeSort {
  /**
   * TaxCodeID
   */
  taxCodeId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}


/**
 * TaxCode 导入模板行 DTO
 * 对应前端 TaxCodeTemplate
 * @description 对应后端 TaktTaxCodeTemplateDto
 */
export interface TaxCodeTemplate {
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
   * 过程（字典 accounting_financial_tax_procedure；DictValue=TAXD～TAXPL）
   */
  taxProcedureCode?: string;

  /**
   * 税码（长度 2；同一公司、过程、税类别下唯一）
   */
  taxCode?: string;

  /**
   * 税类别（字典 accounting_financial_tax_class；A=销项税 V=进项税）
   */
  taxCategory?: string;

  /**
   * 描述（税码文本，如销项增值税、进项税档位说明；Length=50）
   */
  description?: string;

  /**
   * 税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  taxCodeStatus?: number;

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
 * TaxCode 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 TaxCodeImport
 * @description 对应后端 TaktTaxCodeImportDto
 */
export interface TaxCodeImport {
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
   * 过程（字典 accounting_financial_tax_procedure；DictValue=TAXD～TAXPL）
   */
  taxProcedureCode?: string;

  /**
   * 税码（长度 2；同一公司、过程、税类别下唯一）
   */
  taxCode?: string;

  /**
   * 税类别（字典 accounting_financial_tax_class；A=销项税 V=进项税）
   */
  taxCategory?: string;

  /**
   * 描述（税码文本，如销项增值税、进项税档位说明；Length=50）
   */
  description?: string;

  /**
   * 税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  taxCodeStatus?: number;

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
 * TaxCode 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TaxCodeExport
 * @description 对应后端 TaktTaxCodeExportDto
 */
export interface TaxCodeExport {
  /**
   * TaxCodeID
   */
  taxCodeId: string;

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
   * 过程（字典 accounting_financial_tax_procedure；DictValue=TAXD～TAXPL）
   */
  taxProcedureCode: string;

  /**
   * 税码（长度 2；同一公司、过程、税类别下唯一）
   */
  taxCode: string;

  /**
   * 税类别（字典 accounting_financial_tax_class；A=销项税 V=进项税）
   */
  taxCategory: string;

  /**
   * 描述（税码文本，如销项增值税、进项税档位说明；Length=50）
   */
  description: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 税收代码状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  taxCodeStatus: number;

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

