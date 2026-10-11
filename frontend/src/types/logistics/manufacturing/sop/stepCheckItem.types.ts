// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：stepCheckItem.types.ts
// 创建时间：2026-08-12
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/sop 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * SOP 工步检验项目实体
 * 对应前端 TaktSopStepCheckItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopStepCheckItem
 * @description 对应后端 TaktSopStepCheckItemDto
 */
export interface SopStepCheckItem extends CompanyDtoBase {
  /**
   * SopStepCheckItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sopStepCheckItemId: string;

  /**
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId: string;

  /**
   * 工步 名称（填充字段）
   */
  stepName?: string;

  /**
   * 检验项目名称
   */
  checkItemName: string;

  /**
   * 检验方法
   */
  checkMethod?: string;

  /**
   * 检验标准
   */
  checkStandard?: string;

  /**
   * 是否必检（字典 sys_yes_no；0=否，1=是）
   */
  isRequired: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 工步 （主表：TaktSopStep）
   */
  step?: SopStep;

}


/**
 * SopStepCheckItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SopStepCheckItemQuery
 * @description 对应后端 TaktSopStepCheckItemQueryDto
 */
export interface SopStepCheckItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
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
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId?: string;

  /**
   * 检验项目名称
   */
  checkItemName?: string;

  /**
   * 检验方法
   */
  checkMethod?: string;

  /**
   * 检验标准
   */
  checkStandard?: string;

  /**
   * 是否必检（字典 sys_yes_no；0=否，1=是）
   */
  isRequired?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

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
 * 创建SopStepCheckItem DTO
 * 对应前端 SopStepCheckItemCreate
 * @description 对应后端 TaktSopStepCheckItemCreateDto
 */
export interface SopStepCheckItemCreate {
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
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId: string;

  /**
   * 检验项目名称
   */
  checkItemName: string;

  /**
   * 检验方法
   */
  checkMethod?: string;

  /**
   * 检验标准
   */
  checkStandard?: string;

  /**
   * 是否必检（字典 sys_yes_no；0=否，1=是）
   */
  isRequired: number;

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
 * 更新SopStepCheckItem DTO
 * 继承 TaktSopStepCheckItemCreateDto，添加 SopStepCheckItemId 字段
 * 对应前端 SopStepCheckItemUpdate
 * @description 对应后端 TaktSopStepCheckItemUpdateDto
 */
export interface SopStepCheckItemUpdate extends SopStepCheckItemCreate {
  /**
   * SopStepCheckItemID（标识要更新的实体）
   */
  sopStepCheckItemId: string;

}


/**
 * SopStepCheckItem 排序更新 DTO
 * 对应前端 SopStepCheckItemSort
 * @description 对应后端 TaktSopStepCheckItemSortDto
 */
export interface SopStepCheckItemSort {
  /**
   * SopStepCheckItemID
   */
  sopStepCheckItemId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * SopStepCheckItem 导入模板行 DTO
 * 对应前端 SopStepCheckItemTemplate
 * @description 对应后端 TaktSopStepCheckItemTemplateDto
 */
export interface SopStepCheckItemTemplate {
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
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId?: string;

  /**
   * 检验项目名称
   */
  checkItemName?: string;

  /**
   * 检验方法
   */
  checkMethod?: string;

  /**
   * 检验标准
   */
  checkStandard?: string;

  /**
   * 是否必检（字典 sys_yes_no；0=否，1=是）
   */
  isRequired?: number;

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
 * SopStepCheckItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SopStepCheckItemImport
 * @description 对应后端 TaktSopStepCheckItemImportDto
 */
export interface SopStepCheckItemImport {
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
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId?: string;

  /**
   * 检验项目名称
   */
  checkItemName?: string;

  /**
   * 检验方法
   */
  checkMethod?: string;

  /**
   * 检验标准
   */
  checkStandard?: string;

  /**
   * 是否必检（字典 sys_yes_no；0=否，1=是）
   */
  isRequired?: number;

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
 * SopStepCheckItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopStepCheckItemExport
 * @description 对应后端 TaktSopStepCheckItemExportDto
 */
export interface SopStepCheckItemExport {
  /**
   * SopStepCheckItemID
   */
  sopStepCheckItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
   */
  stepId: string;

  /**
   * 检验项目名称
   */
  checkItemName: string;

  /**
   * 检验方法
   */
  checkMethod?: string;

  /**
   * 检验标准
   */
  checkStandard?: string;

  /**
   * 是否必检（字典 sys_yes_no；0=否，1=是）
   */
  isRequired: number;

  /**
   * 排序号
   */
  sortOrder: number;

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

