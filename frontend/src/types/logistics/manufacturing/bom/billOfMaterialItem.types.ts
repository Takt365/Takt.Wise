// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：billOfMaterialItem.types.ts
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
 * Takt物料清单明细实体（扁平BOM行：一头多行，每行一个直接子件；多层BOM通过子件物料关联其BOM头递归展开）
 * 对应前端 TaktBillOfMaterialItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 BillOfMaterialItem
 * @description 对应后端 TaktBillOfMaterialItemDto
 */
export interface BillOfMaterialItem extends CompanyDtoBase {
  /**
   * BillOfMaterialItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  billOfMaterialItemId: string;

  /**
   * 物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）
   */
  billOfMaterialId: string;

  /**
   * 物料清单名称（填充字段）
   */
  billOfMaterialName?: string;

  /**
   * BOM编码（冗余，便于查询）
   */
  bomCode: string;

  /**
   * 行号（项号，步长10：10/20/30…）
   */
  lineNumber: number;

  /**
   * 子项物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 子项物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 用量（quantity）
   */
  usageQuantity: number;

  /**
   * 单位（字典 logistics_materials_unit_of_measure_code）
   */
  materialUnit: string;

  /**
   * 损耗率（0-100，scrap_rate）
   */
  scrapRate: number;

  /**
   * 实际用量（用量 × (1 + 损耗率/100)）
   */
  actualUsageQuantity: number;

  /**
   * 工序号（operation_seq）
   */
  operationSeq: number;

  /**
   * 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
   */
  workCenter?: string;

  /**
   * 位号（position，PCB位号等）
   */
  position?: string;

  /**
   * 替代组号（substitute_group）
   */
  substituteGroup?: string;

  /**
   * 替代优先级（组内越小越优先）
   */
  substitutePriority: number;

  /**
   * 是否可选件（字典 sys_yes_no；0=否，1=是）
   */
  isOptional: number;

  /**
   * 是否虚拟件（字典 sys_yes_no；0=否，1=是）
   */
  isPhantom: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 物料清单（BOM头） （主表：TaktBillOfMaterial）
   */
  bom?: BillOfMaterial;

  /**
   * 替代料明细（一行主件可维护多条替代物料） （子表：TaktBillOfMaterialSubstitute）
   */
  substitutes?: BillOfMaterialSubstitute[];

}


/**
 * BillOfMaterialItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 BillOfMaterialItemQuery
 * @description 对应后端 TaktBillOfMaterialItemQueryDto
 */
export interface BillOfMaterialItemQuery extends TaktPagedQuery {
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
   * 物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）
   */
  billOfMaterialId?: string;

  /**
   * BOM编码（冗余，便于查询）
   */
  bomCode?: string;

  /**
   * 行号（项号，步长10：10/20/30…）
   */
  lineNumber?: number;

  /**
   * 子项物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 子项物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 用量（quantity）
   */
  usageQuantity?: number;

  /**
   * 单位（字典 logistics_materials_unit_of_measure_code）
   */
  materialUnit?: string;

  /**
   * 损耗率（0-100，scrap_rate）
   */
  scrapRate?: number;

  /**
   * 实际用量（用量 × (1 + 损耗率/100)）
   */
  actualUsageQuantity?: number;

  /**
   * 工序号（operation_seq）
   */
  operationSeq?: number;

  /**
   * 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
   */
  workCenter?: string;

  /**
   * 位号（position，PCB位号等）
   */
  position?: string;

  /**
   * 替代组号（substitute_group）
   */
  substituteGroup?: string;

  /**
   * 替代优先级（组内越小越优先）
   */
  substitutePriority?: number;

  /**
   * 是否可选件（字典 sys_yes_no；0=否，1=是）
   */
  isOptional?: number;

  /**
   * 是否虚拟件（字典 sys_yes_no；0=否，1=是）
   */
  isPhantom?: number;

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
 * 创建BillOfMaterialItem DTO
 * 对应前端 BillOfMaterialItemCreate
 * @description 对应后端 TaktBillOfMaterialItemCreateDto
 */
export interface BillOfMaterialItemCreate {
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
   * 物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）
   */
  billOfMaterialId: string;

  /**
   * BOM编码（冗余，便于查询）
   */
  bomCode: string;

  /**
   * 行号（项号，步长10：10/20/30…）
   */
  lineNumber: number;

  /**
   * 子项物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 子项物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 用量（quantity）
   */
  usageQuantity: number;

  /**
   * 单位（字典 logistics_materials_unit_of_measure_code）
   */
  materialUnit: string;

  /**
   * 损耗率（0-100，scrap_rate）
   */
  scrapRate: number;

  /**
   * 实际用量（用量 × (1 + 损耗率/100)）
   */
  actualUsageQuantity: number;

  /**
   * 工序号（operation_seq）
   */
  operationSeq: number;

  /**
   * 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
   */
  workCenter?: string;

  /**
   * 位号（position，PCB位号等）
   */
  position?: string;

  /**
   * 替代组号（substitute_group）
   */
  substituteGroup?: string;

  /**
   * 替代优先级（组内越小越优先）
   */
  substitutePriority: number;

  /**
   * 是否可选件（字典 sys_yes_no；0=否，1=是）
   */
  isOptional: number;

  /**
   * 是否虚拟件（字典 sys_yes_no；0=否，1=是）
   */
  isPhantom: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 替代料明细（一行主件可维护多条替代物料）（子表，级联保存）
   */
  substitutes?: BillOfMaterialSubstituteCreate[];

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
 * 更新BillOfMaterialItem DTO
 * 继承 TaktBillOfMaterialItemCreateDto，添加 BillOfMaterialItemId 字段
 * 对应前端 BillOfMaterialItemUpdate
 * @description 对应后端 TaktBillOfMaterialItemUpdateDto
 */
export interface BillOfMaterialItemUpdate extends BillOfMaterialItemCreate {
  /**
   * BillOfMaterialItemID（标识要更新的实体）
   */
  billOfMaterialItemId: string;

  /**
   * 替代料明细（一行主件可维护多条替代物料）（子表，级联保存）
   */
  substitutes?: any;

}


/**
 * BillOfMaterialItem 作废/撤销作废 DTO
 * 对应前端 BillOfMaterialItemObsolete
 * @description 对应后端 TaktBillOfMaterialItemObsoleteDto
 */
export interface BillOfMaterialItemObsolete {
  /**
   * BillOfMaterialItemID
   */
  billOfMaterialItemId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * BillOfMaterialItem 导入模板行 DTO
 * 对应前端 BillOfMaterialItemTemplate
 * @description 对应后端 TaktBillOfMaterialItemTemplateDto
 */
export interface BillOfMaterialItemTemplate {
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
   * 物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）
   */
  billOfMaterialId?: string;

  /**
   * BOM编码（冗余，便于查询）
   */
  bomCode?: string;

  /**
   * 行号（项号，步长10：10/20/30…）
   */
  lineNumber?: number;

  /**
   * 子项物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 子项物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 用量（quantity）
   */
  usageQuantity?: number;

  /**
   * 单位（字典 logistics_materials_unit_of_measure_code）
   */
  materialUnit?: string;

  /**
   * 损耗率（0-100，scrap_rate）
   */
  scrapRate?: number;

  /**
   * 实际用量（用量 × (1 + 损耗率/100)）
   */
  actualUsageQuantity?: number;

  /**
   * 工序号（operation_seq）
   */
  operationSeq?: number;

  /**
   * 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
   */
  workCenter?: string;

  /**
   * 位号（position，PCB位号等）
   */
  position?: string;

  /**
   * 替代组号（substitute_group）
   */
  substituteGroup?: string;

  /**
   * 替代优先级（组内越小越优先）
   */
  substitutePriority?: number;

  /**
   * 是否可选件（字典 sys_yes_no；0=否，1=是）
   */
  isOptional?: number;

  /**
   * 是否虚拟件（字典 sys_yes_no；0=否，1=是）
   */
  isPhantom?: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 替代料明细（一行主件可维护多条替代物料）（子表，级联保存）
   */
  substitutes?: BillOfMaterialSubstituteCreate[];

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
 * BillOfMaterialItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 BillOfMaterialItemImport
 * @description 对应后端 TaktBillOfMaterialItemImportDto
 */
export interface BillOfMaterialItemImport {
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
   * 物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）
   */
  billOfMaterialId?: string;

  /**
   * BOM编码（冗余，便于查询）
   */
  bomCode?: string;

  /**
   * 行号（项号，步长10：10/20/30…）
   */
  lineNumber?: number;

  /**
   * 子项物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode?: string;

  /**
   * 子项物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 用量（quantity）
   */
  usageQuantity?: number;

  /**
   * 单位（字典 logistics_materials_unit_of_measure_code）
   */
  materialUnit?: string;

  /**
   * 损耗率（0-100，scrap_rate）
   */
  scrapRate?: number;

  /**
   * 实际用量（用量 × (1 + 损耗率/100)）
   */
  actualUsageQuantity?: number;

  /**
   * 工序号（operation_seq）
   */
  operationSeq?: number;

  /**
   * 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
   */
  workCenter?: string;

  /**
   * 位号（position，PCB位号等）
   */
  position?: string;

  /**
   * 替代组号（substitute_group）
   */
  substituteGroup?: string;

  /**
   * 替代优先级（组内越小越优先）
   */
  substitutePriority?: number;

  /**
   * 是否可选件（字典 sys_yes_no；0=否，1=是）
   */
  isOptional?: number;

  /**
   * 是否虚拟件（字典 sys_yes_no；0=否，1=是）
   */
  isPhantom?: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 替代料明细（一行主件可维护多条替代物料）（子表，级联保存）
   */
  substitutes?: BillOfMaterialSubstituteCreate[];

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
 * BillOfMaterialItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 BillOfMaterialItemExport
 * @description 对应后端 TaktBillOfMaterialItemExportDto
 */
export interface BillOfMaterialItemExport {
  /**
   * BillOfMaterialItemID
   */
  billOfMaterialItemId: string;

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
   * 物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）
   */
  billOfMaterialId: string;

  /**
   * BOM编码（冗余，便于查询）
   */
  bomCode: string;

  /**
   * 行号（项号，步长10：10/20/30…）
   */
  lineNumber: number;

  /**
   * 子项物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
   */
  materialCode: string;

  /**
   * 子项物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
   */
  materialDescription?: string;

  /**
   * 用量（quantity）
   */
  usageQuantity: number;

  /**
   * 单位（字典 logistics_materials_unit_of_measure_code）
   */
  materialUnit: string;

  /**
   * 损耗率（0-100，scrap_rate）
   */
  scrapRate: number;

  /**
   * 实际用量（用量 × (1 + 损耗率/100)）
   */
  actualUsageQuantity: number;

  /**
   * 工序号（operation_seq）
   */
  operationSeq: number;

  /**
   * 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
   */
  workCenter?: string;

  /**
   * 位号（position，PCB位号等）
   */
  position?: string;

  /**
   * 替代组号（substitute_group）
   */
  substituteGroup?: string;

  /**
   * 替代优先级（组内越小越优先）
   */
  substitutePriority: number;

  /**
   * 是否可选件（字典 sys_yes_no；0=否，1=是）
   */
  isOptional: number;

  /**
   * 是否虚拟件（字典 sys_yes_no；0=否，1=是）
   */
  isPhantom: number;

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

