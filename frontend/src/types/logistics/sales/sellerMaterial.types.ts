// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：sellerMaterial.types.ts
// 创建时间：2026-08-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/sales 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantCoreDtoBase
} from '@/types/common.types';

/**
 * Takt销售商物料实体（租户内共享） 组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase；仅租户）
 * 对应前端 TaktSellerMaterialDto
 * 继承 TaktTenantCoreDtoBase
 * 对应前端 SellerMaterial
 * @description 对应后端 TaktSellerMaterialDto
 */
export interface SellerMaterial extends TenantCoreDtoBase {
  /**
   * SellerMaterialID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sellerMaterialId: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode；可空）
   */
  customerCode?: string;

  /**
   * 客户简称（冗余）
   */
  customerShortName?: string;

  /**
   * 客户端编码（选项 TaktClients/options；DictValue=ClientCode；可空）
   */
  clientCode?: string;

  /**
   * 客户端简称（冗余）
   */
  clientShortName?: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
   */
  materialType: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup: string;

  /**
   * 内部物料编码（物料编码后缀区分多销售商/多来源，如物料编码+1、+2、+3）
   */
  internalMaterialCode: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription: string;

  /**
   * 销售商物料编码（销售商内部的物料编码）
   */
  sellerMaterialCode: string;

  /**
   * 销售商物料描述
   */
  sellerMaterialDescription: string;

  /**
   * 销售商物料规格
   */
  sellerMaterialSpecification?: string;

}


/**
 * SellerMaterial 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SellerMaterialQuery
 * @description 对应后端 TaktSellerMaterialQueryDto
 */
export interface SellerMaterialQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode；可空）
   */
  customerCode?: string;

  /**
   * 客户简称（冗余）
   */
  customerShortName?: string;

  /**
   * 客户端编码（选项 TaktClients/options；DictValue=ClientCode；可空）
   */
  clientCode?: string;

  /**
   * 客户端简称（冗余）
   */
  clientShortName?: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
   */
  materialType?: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup?: string;

  /**
   * 内部物料编码（物料编码后缀区分多销售商/多来源，如物料编码+1、+2、+3）
   */
  internalMaterialCode?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription?: string;

  /**
   * 销售商物料编码（销售商内部的物料编码）
   */
  sellerMaterialCode?: string;

  /**
   * 销售商物料描述
   */
  sellerMaterialDescription?: string;

  /**
   * 销售商物料规格
   */
  sellerMaterialSpecification?: string;

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
 * 创建SellerMaterial DTO
 * 对应前端 SellerMaterialCreate
 * @description 对应后端 TaktSellerMaterialCreateDto
 */
export interface SellerMaterialCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode；可空）
   */
  customerCode?: string;

  /**
   * 客户简称（冗余）
   */
  customerShortName?: string;

  /**
   * 客户端编码（选项 TaktClients/options；DictValue=ClientCode；可空）
   */
  clientCode?: string;

  /**
   * 客户端简称（冗余）
   */
  clientShortName?: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
   */
  materialType: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup: string;

  /**
   * 内部物料编码（物料编码后缀区分多销售商/多来源，如物料编码+1、+2、+3）
   */
  internalMaterialCode: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription: string;

  /**
   * 销售商物料编码（销售商内部的物料编码）
   */
  sellerMaterialCode: string;

  /**
   * 销售商物料描述
   */
  sellerMaterialDescription: string;

  /**
   * 销售商物料规格
   */
  sellerMaterialSpecification?: string;

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
 * 更新SellerMaterial DTO
 * 继承 TaktSellerMaterialCreateDto，添加 SellerMaterialId 字段
 * 对应前端 SellerMaterialUpdate
 * @description 对应后端 TaktSellerMaterialUpdateDto
 */
export interface SellerMaterialUpdate extends SellerMaterialCreate {
  /**
   * SellerMaterialID（标识要更新的实体）
   */
  sellerMaterialId: string;

}


/**
 * SellerMaterial 导入模板行 DTO
 * 对应前端 SellerMaterialTemplate
 * @description 对应后端 TaktSellerMaterialTemplateDto
 */
export interface SellerMaterialTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode；可空）
   */
  customerCode?: string;

  /**
   * 客户简称（冗余）
   */
  customerShortName?: string;

  /**
   * 客户端编码（选项 TaktClients/options；DictValue=ClientCode；可空）
   */
  clientCode?: string;

  /**
   * 客户端简称（冗余）
   */
  clientShortName?: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
   */
  materialType?: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup?: string;

  /**
   * 内部物料编码（物料编码后缀区分多销售商/多来源，如物料编码+1、+2、+3）
   */
  internalMaterialCode?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription?: string;

  /**
   * 销售商物料编码（销售商内部的物料编码）
   */
  sellerMaterialCode?: string;

  /**
   * 销售商物料描述
   */
  sellerMaterialDescription?: string;

  /**
   * 销售商物料规格
   */
  sellerMaterialSpecification?: string;

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
 * SellerMaterial 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SellerMaterialImport
 * @description 对应后端 TaktSellerMaterialImportDto
 */
export interface SellerMaterialImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode；可空）
   */
  customerCode?: string;

  /**
   * 客户简称（冗余）
   */
  customerShortName?: string;

  /**
   * 客户端编码（选项 TaktClients/options；DictValue=ClientCode；可空）
   */
  clientCode?: string;

  /**
   * 客户端简称（冗余）
   */
  clientShortName?: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
   */
  materialType?: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup?: string;

  /**
   * 内部物料编码（物料编码后缀区分多销售商/多来源，如物料编码+1、+2、+3）
   */
  internalMaterialCode?: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription?: string;

  /**
   * 销售商物料编码（销售商内部的物料编码）
   */
  sellerMaterialCode?: string;

  /**
   * 销售商物料描述
   */
  sellerMaterialDescription?: string;

  /**
   * 销售商物料规格
   */
  sellerMaterialSpecification?: string;

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
 * SellerMaterial 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SellerMaterialExport
 * @description 对应后端 TaktSellerMaterialExportDto
 */
export interface SellerMaterialExport {
  /**
   * SellerMaterialID
   */
  sellerMaterialId: string;

  /**
   * 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode；可空）
   */
  customerCode?: string;

  /**
   * 客户简称（冗余）
   */
  customerShortName?: string;

  /**
   * 客户端编码（选项 TaktClients/options；DictValue=ClientCode；可空）
   */
  clientCode?: string;

  /**
   * 客户端简称（冗余）
   */
  clientShortName?: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
   */
  materialType: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup: string;

  /**
   * 内部物料编码（物料编码后缀区分多销售商/多来源，如物料编码+1、+2、+3）
   */
  internalMaterialCode: string;

  /**
   * 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 物料描述（回填：随物料）
   */
  materialDescription: string;

  /**
   * 销售商物料编码（销售商内部的物料编码）
   */
  sellerMaterialCode: string;

  /**
   * 销售商物料描述
   */
  sellerMaterialDescription: string;

  /**
   * 销售商物料规格
   */
  sellerMaterialSpecification?: string;

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

