// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：billOfMaterial.types.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Takt物料清单实体（规范化：每个父件在工厂下维护一张BOM抬头，多层结构通过子件物料递归关联其自身BOM实现）
 * 对应前端 TaktBillOfMaterialDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 BillOfMaterial
 * @description 对应后端 TaktBillOfMaterialDto
 */
export interface BillOfMaterial extends CompanyDtoBase {
  /**
   * BillOfMaterialID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  billOfMaterialId: string;

  /**
   * BOM编码（业务单据号，便于检索，非唯一键）
   */
  bomCode: string;

  /**
   * BOM名称
   */
  bomName: string;

  /**
   * 父物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  parentMaterialCode: string;

  /**
   * 父物料描述（冗余：按 ParentMaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  parentMaterialDescription?: string;

  /**
   * BOM版本号
   */
  bomVersion: string;

  /**
   * BOM类型/用途（字典 logistics_manufacturing_bom_type；0=标准，1=工程，2=制造，3=成本，4=销售）
   */
  bomType: number;

  /**
   * 备选BOM编码（对应，如01/02）
   */
  alternativeBomNumber: string;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 失效日期（为空表示永久有效）
   */
  expiryDate?: string;

  /**
   * 父物料单位（字典 logistics_materials_unit_of_measure_code）
   */
  parentMaterialUnit: string;

  /**
   * 基本数量（BOM基数，对应）
   */
  parentMaterialQuantity: number;

  /**
   * BOM组编码（选项 TaktBomGroups/options?bomGroupCategory=1；DictValue=BomGroupCode）
   */
  bomGroupCode?: string;

  /**
   * BOM组名称（冗余：按 BomGroupCode 取 TaktBomGroups.BomGroupName 联动）
   */
  bomGroupName?: string;

  /**
   * BOM描述
   */
  bomDescription?: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * BOM状态（字典 logistics_manufacturing_bom_status；0=草稿，1=已发布，2=已停用）
   */
  bomStatus: number;

  /**
   * BOM组成件明细（扁平单层；多层通过子件物料关联其BOM头递归展开） （子表：TaktBillOfMaterialItem）
   */
  items?: BillOfMaterialItem[];

}


/**
 * BillOfMaterial 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 BillOfMaterialQuery
 * @description 对应后端 TaktBillOfMaterialQueryDto
 */
export interface BillOfMaterialQuery extends TaktPagedQuery {
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
   * BOM编码（业务单据号，便于检索，非唯一键）
   */
  bomCode?: string;

  /**
   * BOM名称
   */
  bomName?: string;

  /**
   * 父物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  parentMaterialCode?: string;

  /**
   * 父物料描述（冗余：按 ParentMaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  parentMaterialDescription?: string;

  /**
   * BOM版本号
   */
  bomVersion?: string;

  /**
   * BOM类型/用途（字典 logistics_manufacturing_bom_type；0=标准，1=工程，2=制造，3=成本，4=销售）
   */
  bomType?: number;

  /**
   * 备选BOM编码（对应，如01/02）
   */
  alternativeBomNumber?: string;

  /**
   * 生效日期（范围查询-开始）
   */
  effectiveDateStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  effectiveDateEnd?: string;

  /**
   * 失效日期（为空表示永久有效）（范围查询-开始）
   */
  expiryDateStart?: string;

  /**
   * 失效日期（为空表示永久有效）（范围查询-结束）
   */
  expiryDateEnd?: string;

  /**
   * 父物料单位（字典 logistics_materials_unit_of_measure_code）
   */
  parentMaterialUnit?: string;

  /**
   * 基本数量（BOM基数，对应）
   */
  parentMaterialQuantity?: number;

  /**
   * BOM组编码（选项 TaktBomGroups/options?bomGroupCategory=1；DictValue=BomGroupCode）
   */
  bomGroupCode?: string;

  /**
   * BOM组名称（冗余：按 BomGroupCode 取 TaktBomGroups.BomGroupName 联动）
   */
  bomGroupName?: string;

  /**
   * BOM描述
   */
  bomDescription?: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder?: number;

  /**
   * BOM状态（字典 logistics_manufacturing_bom_status；0=草稿，1=已发布，2=已停用）
   */
  bomStatus?: number;

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
 * 创建BillOfMaterial DTO
 * 对应前端 BillOfMaterialCreate
 * @description 对应后端 TaktBillOfMaterialCreateDto
 */
export interface BillOfMaterialCreate {
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
   * BOM编码（业务单据号，便于检索，非唯一键）
   */
  bomCode: string;

  /**
   * BOM名称
   */
  bomName: string;

  /**
   * 父物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  parentMaterialCode: string;

  /**
   * 父物料描述（冗余：按 ParentMaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  parentMaterialDescription?: string;

  /**
   * BOM版本号
   */
  bomVersion: string;

  /**
   * BOM类型/用途（字典 logistics_manufacturing_bom_type；0=标准，1=工程，2=制造，3=成本，4=销售）
   */
  bomType: number;

  /**
   * 备选BOM编码（对应，如01/02）
   */
  alternativeBomNumber: string;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 失效日期（为空表示永久有效）
   */
  expiryDate?: string;

  /**
   * 父物料单位（字典 logistics_materials_unit_of_measure_code）
   */
  parentMaterialUnit: string;

  /**
   * 基本数量（BOM基数，对应）
   */
  parentMaterialQuantity: number;

  /**
   * BOM组编码（选项 TaktBomGroups/options?bomGroupCategory=1；DictValue=BomGroupCode）
   */
  bomGroupCode?: string;

  /**
   * BOM组名称（冗余：按 BomGroupCode 取 TaktBomGroups.BomGroupName 联动）
   */
  bomGroupName?: string;

  /**
   * BOM描述
   */
  bomDescription?: string;

  /**
   * BOM状态（字典 logistics_manufacturing_bom_status；0=草稿，1=已发布，2=已停用）
   */
  bomStatus: number;

  /**
   * BOM组成件明细（扁平单层；多层通过子件物料关联其BOM头递归展开）（子表，级联保存）
   */
  items?: BillOfMaterialItemCreate[];

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
 * 更新BillOfMaterial DTO
 * 继承 TaktBillOfMaterialCreateDto，添加 BillOfMaterialId 字段
 * 对应前端 BillOfMaterialUpdate
 * @description 对应后端 TaktBillOfMaterialUpdateDto
 */
export interface BillOfMaterialUpdate extends BillOfMaterialCreate {
  /**
   * BillOfMaterialID（标识要更新的实体）
   */
  billOfMaterialId: string;

  /**
   * BOM组成件明细（扁平单层；多层通过子件物料关联其BOM头递归展开）（子表，级联保存）
   */
  items?: any;

}


/**
 * BillOfMaterial 状态更新 DTO
 * 对应前端 BillOfMaterialStatus
 * @description 对应后端 TaktBillOfMaterialStatusDto
 */
export interface BillOfMaterialStatus {
  /**
   * BillOfMaterialID
   */
  billOfMaterialId: string;

  /**
   * BOM状态（字典 logistics_manufacturing_bom_status；0=草稿，1=已发布，2=已停用）
   */
  bomStatus: number;

}


/**
 * BillOfMaterial 排序更新 DTO
 * 对应前端 BillOfMaterialSort
 * @description 对应后端 TaktBillOfMaterialSortDto
 */
export interface BillOfMaterialSort {
  /**
   * BillOfMaterialID
   */
  billOfMaterialId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}


/**
 * BillOfMaterial 导入模板行 DTO
 * 对应前端 BillOfMaterialTemplate
 * @description 对应后端 TaktBillOfMaterialTemplateDto
 */
export interface BillOfMaterialTemplate {
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
   * BOM编码（业务单据号，便于检索，非唯一键）
   */
  bomCode?: string;

  /**
   * BOM名称
   */
  bomName?: string;

  /**
   * 父物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  parentMaterialCode?: string;

  /**
   * 父物料描述（冗余：按 ParentMaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  parentMaterialDescription?: string;

  /**
   * BOM版本号
   */
  bomVersion?: string;

  /**
   * BOM类型/用途（字典 logistics_manufacturing_bom_type；0=标准，1=工程，2=制造，3=成本，4=销售）
   */
  bomType?: number;

  /**
   * 备选BOM编码（对应，如01/02）
   */
  alternativeBomNumber?: string;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期（为空表示永久有效）
   */
  expiryDate?: string;

  /**
   * 父物料单位（字典 logistics_materials_unit_of_measure_code）
   */
  parentMaterialUnit?: string;

  /**
   * 基本数量（BOM基数，对应）
   */
  parentMaterialQuantity?: number;

  /**
   * BOM组编码（选项 TaktBomGroups/options?bomGroupCategory=1；DictValue=BomGroupCode）
   */
  bomGroupCode?: string;

  /**
   * BOM组名称（冗余：按 BomGroupCode 取 TaktBomGroups.BomGroupName 联动）
   */
  bomGroupName?: string;

  /**
   * BOM描述
   */
  bomDescription?: string;

  /**
   * BOM状态（字典 logistics_manufacturing_bom_status；0=草稿，1=已发布，2=已停用）
   */
  bomStatus?: number;

  /**
   * BOM组成件明细（扁平单层；多层通过子件物料关联其BOM头递归展开）（子表，级联保存）
   */
  items?: BillOfMaterialItemCreate[];

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
 * BillOfMaterial 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 BillOfMaterialImport
 * @description 对应后端 TaktBillOfMaterialImportDto
 */
export interface BillOfMaterialImport {
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
   * BOM编码（业务单据号，便于检索，非唯一键）
   */
  bomCode?: string;

  /**
   * BOM名称
   */
  bomName?: string;

  /**
   * 父物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  parentMaterialCode?: string;

  /**
   * 父物料描述（冗余：按 ParentMaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  parentMaterialDescription?: string;

  /**
   * BOM版本号
   */
  bomVersion?: string;

  /**
   * BOM类型/用途（字典 logistics_manufacturing_bom_type；0=标准，1=工程，2=制造，3=成本，4=销售）
   */
  bomType?: number;

  /**
   * 备选BOM编码（对应，如01/02）
   */
  alternativeBomNumber?: string;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期（为空表示永久有效）
   */
  expiryDate?: string;

  /**
   * 父物料单位（字典 logistics_materials_unit_of_measure_code）
   */
  parentMaterialUnit?: string;

  /**
   * 基本数量（BOM基数，对应）
   */
  parentMaterialQuantity?: number;

  /**
   * BOM组编码（选项 TaktBomGroups/options?bomGroupCategory=1；DictValue=BomGroupCode）
   */
  bomGroupCode?: string;

  /**
   * BOM组名称（冗余：按 BomGroupCode 取 TaktBomGroups.BomGroupName 联动）
   */
  bomGroupName?: string;

  /**
   * BOM描述
   */
  bomDescription?: string;

  /**
   * BOM状态（字典 logistics_manufacturing_bom_status；0=草稿，1=已发布，2=已停用）
   */
  bomStatus?: number;

  /**
   * BOM组成件明细（扁平单层；多层通过子件物料关联其BOM头递归展开）（子表，级联保存）
   */
  items?: BillOfMaterialItemCreate[];

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
 * BillOfMaterial 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 BillOfMaterialExport
 * @description 对应后端 TaktBillOfMaterialExportDto
 */
export interface BillOfMaterialExport {
  /**
   * BillOfMaterialID
   */
  billOfMaterialId: string;

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
   * BOM编码（业务单据号，便于检索，非唯一键）
   */
  bomCode: string;

  /**
   * BOM名称
   */
  bomName: string;

  /**
   * 父物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  parentMaterialCode: string;

  /**
   * 父物料描述（冗余：按 ParentMaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  parentMaterialDescription?: string;

  /**
   * BOM版本号
   */
  bomVersion: string;

  /**
   * BOM类型/用途（字典 logistics_manufacturing_bom_type；0=标准，1=工程，2=制造，3=成本，4=销售）
   */
  bomType: number;

  /**
   * 备选BOM编码（对应，如01/02）
   */
  alternativeBomNumber: string;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 失效日期（为空表示永久有效）
   */
  expiryDate?: string;

  /**
   * 父物料单位（字典 logistics_materials_unit_of_measure_code）
   */
  parentMaterialUnit: string;

  /**
   * 基本数量（BOM基数，对应）
   */
  parentMaterialQuantity: number;

  /**
   * BOM组编码（选项 TaktBomGroups/options?bomGroupCategory=1；DictValue=BomGroupCode）
   */
  bomGroupCode?: string;

  /**
   * BOM组名称（冗余：按 BomGroupCode 取 TaktBomGroups.BomGroupName 联动）
   */
  bomGroupName?: string;

  /**
   * BOM描述
   */
  bomDescription?: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * BOM状态（字典 logistics_manufacturing_bom_status；0=草稿，1=已发布，2=已停用）
   */
  bomStatus: number;

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

