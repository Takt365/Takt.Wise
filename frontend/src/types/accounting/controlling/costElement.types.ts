// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/accounting/controlling
// 文件名称：costElement.types.ts
// 创建时间：2026-08-21
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/controlling 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 成本要素实体
 * 对应前端 TaktCostElementDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CostElement
 * @description 对应后端 TaktCostElementDto
 */
export interface CostElement extends CompanyDtoBase {
  /**
   * CostElementID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  costElementId: string;

  /**
   * 成本要素编码（4位，租户+公司内唯一）
   */
  costElementCode: string;

  /**
   * 成本要素名称
   */
  costElementName: string;

  /**
   * 成本要素类型（字典 accounting_controlling_cost_element_type；0=初级，1=次级）
   */
  costElementType: number;

  /**
   * 成本要素类别（字典 accounting_controlling_cost_element_category-KATYP，整型存 1/3/4/11…）
   */
  costElementCategory: number;

  /**
   * 父级 ID
   */
  parentId: string;

  /**
   * 成本要素层级
   */
  costElementLevel: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 排序号（回填）
   */
  sortOrder: number;

  /**
   * 成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  costElementStatus: number;

}


/**
 * CostElement 树形列表/树选择 DTO（含子节点）
 * 对应 GetCostElementTreeAsync 等接口
 * 对应前端 CostElementTree
 * @description 对应后端 TaktCostElementTreeDto
 */
export interface CostElementTree extends CostElement {
  /**
   * 子节点
   */
  children: CostElementTree[];

}


/**
 * CostElement 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CostElementQuery
 * @description 对应后端 TaktCostElementQueryDto
 */
export interface CostElementQuery extends TaktPagedQuery {
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
   * 成本要素编码（4位，租户+公司内唯一）
   */
  costElementCode?: string;

  /**
   * 成本要素名称
   */
  costElementName?: string;

  /**
   * 成本要素类型（字典 accounting_controlling_cost_element_type；0=初级，1=次级）
   */
  costElementType?: number;

  /**
   * 成本要素类别（字典 accounting_controlling_cost_element_category-KATYP，整型存 1/3/4/11…）
   */
  costElementCategory?: number;

  /**
   * 父级 ID
   */
  parentId?: string;

  /**
   * 成本要素层级
   */
  costElementLevel?: number;

  /**
   * 生效日期（范围查询-开始）
   */
  validFromStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  validFromEnd?: string;

  /**
   * 失效日期（范围查询-开始）
   */
  validToStart?: string;

  /**
   * 失效日期（范围查询-结束）
   */
  validToEnd?: string;

  /**
   * 排序号（回填）
   */
  sortOrder?: number;

  /**
   * 成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  costElementStatus?: number;

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
 * 创建CostElement DTO
 * 对应前端 CostElementCreate
 * @description 对应后端 TaktCostElementCreateDto
 */
export interface CostElementCreate {
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
   * 成本要素编码（4位，租户+公司内唯一）
   */
  costElementCode: string;

  /**
   * 成本要素名称
   */
  costElementName: string;

  /**
   * 成本要素类型（字典 accounting_controlling_cost_element_type；0=初级，1=次级）
   */
  costElementType: number;

  /**
   * 成本要素类别（字典 accounting_controlling_cost_element_category-KATYP，整型存 1/3/4/11…）
   */
  costElementCategory: number;

  /**
   * 父级 ID
   */
  parentId: string;

  /**
   * 成本要素层级
   */
  costElementLevel: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  costElementStatus: number;

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
 * 更新CostElement DTO
 * 继承 TaktCostElementCreateDto，添加 CostElementId 字段
 * 对应前端 CostElementUpdate
 * @description 对应后端 TaktCostElementUpdateDto
 */
export interface CostElementUpdate extends CostElementCreate {
  /**
   * CostElementID（标识要更新的实体）
   */
  costElementId: string;

}


/**
 * CostElement 状态更新 DTO
 * 对应前端 CostElementStatus
 * @description 对应后端 TaktCostElementStatusDto
 */
export interface CostElementStatus {
  /**
   * CostElementID
   */
  costElementId: string;

  /**
   * 成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  costElementStatus: number;

}


/**
 * CostElement 排序更新 DTO
 * 对应前端 CostElementSort
 * @description 对应后端 TaktCostElementSortDto
 */
export interface CostElementSort {
  /**
   * CostElementID
   */
  costElementId: string;

  /**
   * 排序号（回填）
   */
  sortOrder: number;

}


/**
 * CostElement 导入模板行 DTO
 * 对应前端 CostElementTemplate
 * @description 对应后端 TaktCostElementTemplateDto
 */
export interface CostElementTemplate {
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
   * 成本要素编码（4位，租户+公司内唯一）
   */
  costElementCode?: string;

  /**
   * 成本要素名称
   */
  costElementName?: string;

  /**
   * 成本要素类型（字典 accounting_controlling_cost_element_type；0=初级，1=次级）
   */
  costElementType?: number;

  /**
   * 成本要素类别（字典 accounting_controlling_cost_element_category-KATYP，整型存 1/3/4/11…）
   */
  costElementCategory?: number;

  /**
   * 父级 ID
   */
  parentId?: string;

  /**
   * 成本要素层级
   */
  costElementLevel?: number;

  /**
   * 生效日期
   */
  validFrom?: string;

  /**
   * 失效日期
   */
  validTo?: string;

  /**
   * 成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  costElementStatus?: number;

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
 * CostElement 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CostElementImport
 * @description 对应后端 TaktCostElementImportDto
 */
export interface CostElementImport {
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
   * 成本要素编码（4位，租户+公司内唯一）
   */
  costElementCode?: string;

  /**
   * 成本要素名称
   */
  costElementName?: string;

  /**
   * 成本要素类型（字典 accounting_controlling_cost_element_type；0=初级，1=次级）
   */
  costElementType?: number;

  /**
   * 成本要素类别（字典 accounting_controlling_cost_element_category-KATYP，整型存 1/3/4/11…）
   */
  costElementCategory?: number;

  /**
   * 父级 ID
   */
  parentId?: string;

  /**
   * 成本要素层级
   */
  costElementLevel?: number;

  /**
   * 生效日期
   */
  validFrom?: string;

  /**
   * 失效日期
   */
  validTo?: string;

  /**
   * 成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  costElementStatus?: number;

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
 * CostElement 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CostElementExport
 * @description 对应后端 TaktCostElementExportDto
 */
export interface CostElementExport {
  /**
   * CostElementID
   */
  costElementId: string;

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
   * 成本要素编码（4位，租户+公司内唯一）
   */
  costElementCode: string;

  /**
   * 成本要素名称
   */
  costElementName: string;

  /**
   * 成本要素类型（字典 accounting_controlling_cost_element_type；0=初级，1=次级）
   */
  costElementType: number;

  /**
   * 成本要素类别（字典 accounting_controlling_cost_element_category-KATYP，整型存 1/3/4/11…）
   */
  costElementCategory: number;

  /**
   * 父级 ID
   */
  parentId: string;

  /**
   * 成本要素层级
   */
  costElementLevel: number;

  /**
   * 生效日期
   */
  validFrom: string;

  /**
   * 失效日期
   */
  validTo: string;

  /**
   * 排序号（回填）
   */
  sortOrder: number;

  /**
   * 成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  costElementStatus: number;

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

