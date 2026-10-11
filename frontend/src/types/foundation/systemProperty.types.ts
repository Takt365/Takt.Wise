// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/foundation
// 文件名称：systemProperty.types.ts
// 创建时间：2026-09-16
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 系统属性实体 存储系统的各种配置参数，支持租户级配置隔离
 * 对应前端 TaktSystemPropertyDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SystemProperty
 * @description 对应后端 TaktSystemPropertyDto
 */
export interface SystemProperty extends CompanyDtoBase {
  /**
   * SystemPropertyID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  systemPropertyId: string;

  /**
   * 属性键（唯一索引：租户+公司内唯一，见 ix_system_property_key_unique；如 system.siteName, upload.maxSize）
   */
  systemPropertyKey: string;

  /**
   * 属性值（字符串形式，复杂对象用JSON；超长配置用 ntext）
   */
  systemPropertyValue?: string;

  /**
   * 属性名称（显示名称，如：站点名称、最大上传大小）
   */
  systemPropertyName: string;

  /**
   * 属性描述
   */
  systemPropertyDescription?: string;

  /**
   * 属性类别（字典 sys_resource_type；frontend=前端 backend=后端）
   */
  systemPropertyGroup: string;

  /**
   * 值类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
   */
  valueType: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是）
   */
  isBuiltIn: number;

  /**
   * 只读（字典 sys_yes_no；0=否 1=是）
   */
  isReadonly: number;

  /**
   * 加密（字典 sys_yes_no；0=否 1=是）
   */
  isEncrypted: number;

  /**
   * 排序号（回填）
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  systemPropertyStatus: number;

}


/**
 * SystemProperty 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SystemPropertyQuery
 * @description 对应后端 TaktSystemPropertyQueryDto
 */
export interface SystemPropertyQuery extends TaktPagedQuery {
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
   * 属性键（唯一索引：租户+公司内唯一，见 ix_system_property_key_unique；如 system.siteName, upload.maxSize）
   */
  systemPropertyKey?: string;

  /**
   * 属性值（字符串形式，复杂对象用JSON；超长配置用 ntext）
   */
  systemPropertyValue?: string;

  /**
   * 属性名称（显示名称，如：站点名称、最大上传大小）
   */
  systemPropertyName?: string;

  /**
   * 属性描述
   */
  systemPropertyDescription?: string;

  /**
   * 属性类别（字典 sys_resource_type；frontend=前端 backend=后端）
   */
  systemPropertyGroup?: string;

  /**
   * 值类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
   */
  valueType?: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是）
   */
  isBuiltIn?: number;

  /**
   * 只读（字典 sys_yes_no；0=否 1=是）
   */
  isReadonly?: number;

  /**
   * 加密（字典 sys_yes_no；0=否 1=是）
   */
  isEncrypted?: number;

  /**
   * 排序号（回填）
   */
  sortOrder?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  systemPropertyStatus?: number;

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
 * 创建SystemProperty DTO
 * 对应前端 SystemPropertyCreate
 * @description 对应后端 TaktSystemPropertyCreateDto
 */
export interface SystemPropertyCreate {
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
   * 属性键（唯一索引：租户+公司内唯一，见 ix_system_property_key_unique；如 system.siteName, upload.maxSize）
   */
  systemPropertyKey: string;

  /**
   * 属性值（字符串形式，复杂对象用JSON；超长配置用 ntext）
   */
  systemPropertyValue?: string;

  /**
   * 属性名称（显示名称，如：站点名称、最大上传大小）
   */
  systemPropertyName: string;

  /**
   * 属性描述
   */
  systemPropertyDescription?: string;

  /**
   * 属性类别（字典 sys_resource_type；frontend=前端 backend=后端）
   */
  systemPropertyGroup: string;

  /**
   * 值类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
   */
  valueType: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是）
   */
  isBuiltIn: number;

  /**
   * 只读（字典 sys_yes_no；0=否 1=是）
   */
  isReadonly: number;

  /**
   * 加密（字典 sys_yes_no；0=否 1=是）
   */
  isEncrypted: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  systemPropertyStatus: number;

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
 * 更新SystemProperty DTO
 * 继承 TaktSystemPropertyCreateDto，添加 SystemPropertyId 字段
 * 对应前端 SystemPropertyUpdate
 * @description 对应后端 TaktSystemPropertyUpdateDto
 */
export interface SystemPropertyUpdate extends SystemPropertyCreate {
  /**
   * SystemPropertyID（标识要更新的实体）
   */
  systemPropertyId: string;

}


/**
 * SystemProperty 状态更新 DTO
 * 对应前端 SystemPropertyStatus
 * @description 对应后端 TaktSystemPropertyStatusDto
 */
export interface SystemPropertyStatus {
  /**
   * SystemPropertyID
   */
  systemPropertyId: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  systemPropertyStatus: number;

}


/**
 * SystemProperty 排序更新 DTO
 * 对应前端 SystemPropertySort
 * @description 对应后端 TaktSystemPropertySortDto
 */
export interface SystemPropertySort {
  /**
   * SystemPropertyID
   */
  systemPropertyId: string;

  /**
   * 排序号（回填）
   */
  sortOrder: number;

}


/**
 * SystemProperty 内置更新 DTO
 * 对应前端 SystemPropertyBuiltIn
 * @description 对应后端 TaktSystemPropertyBuiltInDto
 */
export interface SystemPropertyBuiltIn {
  /**
   * SystemPropertyID
   */
  systemPropertyId: string;

  /**
   * 内置（字典 sys_yes_no；1=是，0=否）
   */
  isBuiltIn: number;

}


/**
 * SystemProperty 导入模板行 DTO
 * 对应前端 SystemPropertyTemplate
 * @description 对应后端 TaktSystemPropertyTemplateDto
 */
export interface SystemPropertyTemplate {
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
   * 属性键（唯一索引：租户+公司内唯一，见 ix_system_property_key_unique；如 system.siteName, upload.maxSize）
   */
  systemPropertyKey?: string;

  /**
   * 属性值（字符串形式，复杂对象用JSON；超长配置用 ntext）
   */
  systemPropertyValue?: string;

  /**
   * 属性名称（显示名称，如：站点名称、最大上传大小）
   */
  systemPropertyName?: string;

  /**
   * 属性描述
   */
  systemPropertyDescription?: string;

  /**
   * 属性类别（字典 sys_resource_type；frontend=前端 backend=后端）
   */
  systemPropertyGroup?: string;

  /**
   * 值类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
   */
  valueType?: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是）
   */
  isBuiltIn?: number;

  /**
   * 只读（字典 sys_yes_no；0=否 1=是）
   */
  isReadonly?: number;

  /**
   * 加密（字典 sys_yes_no；0=否 1=是）
   */
  isEncrypted?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  systemPropertyStatus?: number;

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
 * SystemProperty 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SystemPropertyImport
 * @description 对应后端 TaktSystemPropertyImportDto
 */
export interface SystemPropertyImport {
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
   * 属性键（唯一索引：租户+公司内唯一，见 ix_system_property_key_unique；如 system.siteName, upload.maxSize）
   */
  systemPropertyKey?: string;

  /**
   * 属性值（字符串形式，复杂对象用JSON；超长配置用 ntext）
   */
  systemPropertyValue?: string;

  /**
   * 属性名称（显示名称，如：站点名称、最大上传大小）
   */
  systemPropertyName?: string;

  /**
   * 属性描述
   */
  systemPropertyDescription?: string;

  /**
   * 属性类别（字典 sys_resource_type；frontend=前端 backend=后端）
   */
  systemPropertyGroup?: string;

  /**
   * 值类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
   */
  valueType?: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是）
   */
  isBuiltIn?: number;

  /**
   * 只读（字典 sys_yes_no；0=否 1=是）
   */
  isReadonly?: number;

  /**
   * 加密（字典 sys_yes_no；0=否 1=是）
   */
  isEncrypted?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  systemPropertyStatus?: number;

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
 * SystemProperty 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SystemPropertyExport
 * @description 对应后端 TaktSystemPropertyExportDto
 */
export interface SystemPropertyExport {
  /**
   * SystemPropertyID
   */
  systemPropertyId: string;

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
   * 属性键（唯一索引：租户+公司内唯一，见 ix_system_property_key_unique；如 system.siteName, upload.maxSize）
   */
  systemPropertyKey: string;

  /**
   * 属性值（字符串形式，复杂对象用JSON；超长配置用 ntext）
   */
  systemPropertyValue?: string;

  /**
   * 属性名称（显示名称，如：站点名称、最大上传大小）
   */
  systemPropertyName: string;

  /**
   * 属性描述
   */
  systemPropertyDescription?: string;

  /**
   * 属性类别（字典 sys_resource_type；frontend=前端 backend=后端）
   */
  systemPropertyGroup: string;

  /**
   * 值类型（字典 code_generator_display_type；input=文本框 select=下拉框 switch=开关 等）
   */
  valueType: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是）
   */
  isBuiltIn: number;

  /**
   * 只读（字典 sys_yes_no；0=否 1=是）
   */
  isReadonly: number;

  /**
   * 加密（字典 sys_yes_no；0=否 1=是）
   */
  isEncrypted: number;

  /**
   * 排序号（回填）
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  systemPropertyStatus: number;

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

