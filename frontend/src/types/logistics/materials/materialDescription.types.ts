// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：materialDescription.types.ts
// 创建时间：2026-08-05
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantDtoBase
} from '@/types/common.types';

/**
 * Takt物料多语言描述实体（租户级）
 * 对应前端 TaktMaterialDescriptionDto
 * 继承 TaktTenantDtoBase
 * 对应前端 MaterialDescription
 * @description 对应后端 TaktMaterialDescriptionDto
 */
export interface MaterialDescription extends TenantDtoBase {
  /**
   * MaterialDescriptionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  materialDescriptionId: string;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料描述
   */
  materialDescription: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 物料型号
   */
  materialModel?: string;

  /**
   * 物料长描述
   */
  materialLongDescription?: string;

  /**
   * 语言（区域文化编码；选项 TaktCultures/options，DictValue=CultureCode；存 BCP47 如 zh-CN）
   */

}

/**
 * MaterialDescription 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaterialDescriptionQuery
 * @description 对应后端 TaktMaterialDescriptionQueryDto
 */
export interface MaterialDescriptionQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 物料型号
   */
  materialModel?: string;

  /**
   * 物料长描述
   */
  materialLongDescription?: string;

  /**
   * 语言（区域文化编码；选项 TaktCultures/options，DictValue=CultureCode；存 BCP47 如 zh-CN）
   */
  cultureCode?: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant?: string;

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
 * 创建MaterialDescription DTO
 * 对应前端 MaterialDescriptionCreate
 * @description 对应后端 TaktMaterialDescriptionCreateDto
 */
export interface MaterialDescriptionCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料描述
   */
  materialDescription: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 物料型号
   */
  materialModel?: string;

  /**
   * 物料长描述
   */
  materialLongDescription?: string;

  /**
   * 语言（区域文化编码；选项 TaktCultures/options，DictValue=CultureCode；存 BCP47 如 zh-CN）
   */
  cultureCode: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant: string;

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
 * 更新MaterialDescription DTO
 * 继承 TaktMaterialDescriptionCreateDto，添加 MaterialDescriptionId 字段
 * 对应前端 MaterialDescriptionUpdate
 * @description 对应后端 TaktMaterialDescriptionUpdateDto
 */
export interface MaterialDescriptionUpdate extends MaterialDescriptionCreate {
  /**
   * MaterialDescriptionID（标识要更新的实体）
   */
  materialDescriptionId: string;

}

/**
 * MaterialDescription 导入模板行 DTO
 * 对应前端 MaterialDescriptionTemplate
 * @description 对应后端 TaktMaterialDescriptionTemplateDto
 */
export interface MaterialDescriptionTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 物料型号
   */
  materialModel?: string;

  /**
   * 物料长描述
   */
  materialLongDescription?: string;

  /**
   * 语言（区域文化编码；选项 TaktCultures/options，DictValue=CultureCode；存 BCP47 如 zh-CN）
   */
  cultureCode?: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant?: string;

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
 * MaterialDescription 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaterialDescriptionImport
 * @description 对应后端 TaktMaterialDescriptionImportDto
 */
export interface MaterialDescriptionImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
   */
  materialCode?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 物料型号
   */
  materialModel?: string;

  /**
   * 物料长描述
   */
  materialLongDescription?: string;

  /**
   * 语言（区域文化编码；选项 TaktCultures/options，DictValue=CultureCode；存 BCP47 如 zh-CN）
   */
  cultureCode?: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant?: string;

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
 * MaterialDescription 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaterialDescriptionExport
 * @description 对应后端 TaktMaterialDescriptionExportDto
 */
export interface MaterialDescriptionExport {
  /**
   * MaterialDescriptionID
   */
  materialDescriptionId: string;

  /**
   * 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
   */
  materialCode: string;

  /**
   * 物料描述
   */
  materialDescription: string;

  /**
   * 物料规格
   */
  materialSpecification?: string;

  /**
   * 物料型号
   */
  materialModel?: string;

  /**
   * 物料长描述
   */
  materialLongDescription?: string;

  /**
   * 语言（区域文化编码；选项 TaktCultures/options，DictValue=CultureCode；存 BCP47 如 zh-CN）
   */
  cultureCode: string;

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

