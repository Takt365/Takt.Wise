// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：manufacturerMaterial.types.ts
// 创建时间：2026-08-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/procurement 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantCoreDtoBase
} from '@/types/common.types';

/**
 * Takt制造商物料实体（租户内共享）
 * 对应前端 TaktManufacturerMaterialDto
 * 继承 TaktTenantCoreDtoBase（组合 4）
 * 对应前端 ManufacturerMaterial
 * @description 对应后端 TaktManufacturerMaterialDto
 */
export interface ManufacturerMaterial extends TenantCoreDtoBase {
  /**
   * ManufacturerMaterialID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  manufacturerMaterialId: string;

  /**
   * 经销商编码（选项 TaktVendors/options；DictValue=VendorCode；可空）
   */
  vendorCode?: string;

  /**
   * 经销商简称（冗余）
   */
  vendorShortName?: string;

  /**
   * 供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；可空）
   */
  supplierCode?: string;

  /**
   * 供货商简称（冗余）
   */
  supplierShortName?: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
   */
  materialType: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup: string;

  /**
   * 内部物料编码（物料编码后缀区分多制造商/多来源，如物料编码+1、+2、+3）
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
   * 制造商物料编码（制造商内部的物料编码）
   */
  manufacturerMaterialCode: string;

  /**
   * 制造商物料描述
   */
  manufacturerMaterialDescription: string;

  /**
   * 制造商物料规格
   */
  manufacturerMaterialSpecification?: string;

}

/**
 * ManufacturerMaterial 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ManufacturerMaterialQuery
 * @description 对应后端 TaktManufacturerMaterialQueryDto
 */
export interface ManufacturerMaterialQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 经销商编码（选项 TaktVendors/options；DictValue=VendorCode；可空）
   */
  vendorCode?: string;

  /**
   * 经销商简称（冗余）
   */
  vendorShortName?: string;

  /**
   * 供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；可空）
   */
  supplierCode?: string;

  /**
   * 供货商简称（冗余）
   */
  supplierShortName?: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
   */
  materialType?: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup?: string;

  /**
   * 内部物料编码（物料编码后缀区分多制造商/多来源，如物料编码+1、+2、+3）
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
   * 制造商物料编码（制造商内部的物料编码）
   */
  manufacturerMaterialCode?: string;

  /**
   * 制造商物料描述
   */
  manufacturerMaterialDescription?: string;

  /**
   * 制造商物料规格
   */
  manufacturerMaterialSpecification?: string;

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
 * 创建ManufacturerMaterial DTO
 * 对应前端 ManufacturerMaterialCreate
 * @description 对应后端 TaktManufacturerMaterialCreateDto
 */
export interface ManufacturerMaterialCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 经销商编码（选项 TaktVendors/options；DictValue=VendorCode；可空）
   */
  vendorCode?: string;

  /**
   * 经销商简称（冗余）
   */
  vendorShortName?: string;

  /**
   * 供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；可空）
   */
  supplierCode?: string;

  /**
   * 供货商简称（冗余）
   */
  supplierShortName?: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
   */
  materialType: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup: string;

  /**
   * 内部物料编码（物料编码后缀区分多制造商/多来源，如物料编码+1、+2、+3）
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
   * 制造商物料编码（制造商内部的物料编码）
   */
  manufacturerMaterialCode: string;

  /**
   * 制造商物料描述
   */
  manufacturerMaterialDescription: string;

  /**
   * 制造商物料规格
   */
  manufacturerMaterialSpecification?: string;

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
 * 更新ManufacturerMaterial DTO
 * 继承 TaktManufacturerMaterialCreateDto，添加 ManufacturerMaterialId 字段
 * 对应前端 ManufacturerMaterialUpdate
 * @description 对应后端 TaktManufacturerMaterialUpdateDto
 */
export interface ManufacturerMaterialUpdate extends ManufacturerMaterialCreate {
  /**
   * ManufacturerMaterialID（标识要更新的实体）
   */
  manufacturerMaterialId: string;

}

/**
 * ManufacturerMaterial 导入模板行 DTO
 * 对应前端 ManufacturerMaterialTemplate
 * @description 对应后端 TaktManufacturerMaterialTemplateDto
 */
export interface ManufacturerMaterialTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 经销商编码（选项 TaktVendors/options；DictValue=VendorCode；可空）
   */
  vendorCode?: string;

  /**
   * 经销商简称（冗余）
   */
  vendorShortName?: string;

  /**
   * 供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；可空）
   */
  supplierCode?: string;

  /**
   * 供货商简称（冗余）
   */
  supplierShortName?: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
   */
  materialType?: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup?: string;

  /**
   * 内部物料编码（物料编码后缀区分多制造商/多来源，如物料编码+1、+2、+3）
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
   * 制造商物料编码（制造商内部的物料编码）
   */
  manufacturerMaterialCode?: string;

  /**
   * 制造商物料描述
   */
  manufacturerMaterialDescription?: string;

  /**
   * 制造商物料规格
   */
  manufacturerMaterialSpecification?: string;

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
 * ManufacturerMaterial 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ManufacturerMaterialImport
 * @description 对应后端 TaktManufacturerMaterialImportDto
 */
export interface ManufacturerMaterialImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 经销商编码（选项 TaktVendors/options；DictValue=VendorCode；可空）
   */
  vendorCode?: string;

  /**
   * 经销商简称（冗余）
   */
  vendorShortName?: string;

  /**
   * 供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；可空）
   */
  supplierCode?: string;

  /**
   * 供货商简称（冗余）
   */
  supplierShortName?: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
   */
  materialType?: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup?: string;

  /**
   * 内部物料编码（物料编码后缀区分多制造商/多来源，如物料编码+1、+2、+3）
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
   * 制造商物料编码（制造商内部的物料编码）
   */
  manufacturerMaterialCode?: string;

  /**
   * 制造商物料描述
   */
  manufacturerMaterialDescription?: string;

  /**
   * 制造商物料规格
   */
  manufacturerMaterialSpecification?: string;

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
 * ManufacturerMaterial 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ManufacturerMaterialExport
 * @description 对应后端 TaktManufacturerMaterialExportDto
 */
export interface ManufacturerMaterialExport {
  /**
   * ManufacturerMaterialID
   */
  manufacturerMaterialId: string;

  /**
   * 经销商编码（选项 TaktVendors/options；DictValue=VendorCode；可空）
   */
  vendorCode?: string;

  /**
   * 经销商简称（冗余）
   */
  vendorShortName?: string;

  /**
   * 供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；可空）
   */
  supplierCode?: string;

  /**
   * 供货商简称（冗余）
   */
  supplierShortName?: string;

  /**
   * 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
   */
  materialType: string;

  /**
   * 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
   */
  materialGroup: string;

  /**
   * 内部物料编码（物料编码后缀区分多制造商/多来源，如物料编码+1、+2、+3）
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
   * 制造商物料编码（制造商内部的物料编码）
   */
  manufacturerMaterialCode: string;

  /**
   * 制造商物料描述
   */
  manufacturerMaterialDescription: string;

  /**
   * 制造商物料规格
   */
  manufacturerMaterialSpecification?: string;

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

