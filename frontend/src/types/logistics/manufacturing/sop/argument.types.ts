// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：argument.types.ts
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
 * SOP 作业参数实体
 * 对应前端 TaktSopArgumentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopArgument
 * @description 对应后端 TaktSopArgumentDto
 */
export interface SopArgument extends CompanyDtoBase {
  /**
   * SopArgumentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sopArgumentId: string;

  /**
   * 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
   */
  execId: string;

  /**
   * 执行追溯 名称（填充字段）
   */
  execName?: string;

  /**
   * 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
   */
  execStepId?: string;

  /**
   * 工步执行明细 名称（填充字段）
   */
  execStepName?: string;

  /**
   * 工序参数定义 ID（选项 TaktRoutingItemArguments/options；DictValue=Id）
   */
  routingItemParameterId?: string;

  /**
   * 工序参数定义 名称（填充字段）
   */
  routingItemParameterName?: string;

  /**
   * 参数编码
   */
  paramCode: string;

  /**
   * 实际值
   */
  actualValue: number;

  /**
   * 是否超差（字典 sys_yes_no；0=否，1=是）
   */
  isOutOfRange: number;

  /**
   * 记录时间
   */
  recordedAt: string;

  /**
   * 执行追溯 （主表：TaktSopExec）
   */
  exec?: SopExec;

}


/**
 * SopArgument 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SopArgumentQuery
 * @description 对应后端 TaktSopArgumentQueryDto
 */
export interface SopArgumentQuery extends TaktPagedQuery {
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
   * 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
   */
  execId?: string;

  /**
   * 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
   */
  execStepId?: string;

  /**
   * 工序参数定义 ID（选项 TaktRoutingItemArguments/options；DictValue=Id）
   */
  routingItemParameterId?: string;

  /**
   * 参数编码
   */
  paramCode?: string;

  /**
   * 实际值
   */
  actualValue?: number;

  /**
   * 是否超差（字典 sys_yes_no；0=否，1=是）
   */
  isOutOfRange?: number;

  /**
   * 记录时间（范围查询-开始）
   */
  recordedAtStart?: string;

  /**
   * 记录时间（范围查询-结束）
   */
  recordedAtEnd?: string;

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
 * 创建SopArgument DTO
 * 对应前端 SopArgumentCreate
 * @description 对应后端 TaktSopArgumentCreateDto
 */
export interface SopArgumentCreate {
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
   * 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
   */
  execId: string;

  /**
   * 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
   */
  execStepId?: string;

  /**
   * 工序参数定义 ID（选项 TaktRoutingItemArguments/options；DictValue=Id）
   */
  routingItemParameterId?: string;

  /**
   * 参数编码
   */
  paramCode: string;

  /**
   * 实际值
   */
  actualValue: number;

  /**
   * 是否超差（字典 sys_yes_no；0=否，1=是）
   */
  isOutOfRange: number;

  /**
   * 记录时间
   */
  recordedAt: string;

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
 * 更新SopArgument DTO
 * 继承 TaktSopArgumentCreateDto，添加 SopArgumentId 字段
 * 对应前端 SopArgumentUpdate
 * @description 对应后端 TaktSopArgumentUpdateDto
 */
export interface SopArgumentUpdate extends SopArgumentCreate {
  /**
   * SopArgumentID（标识要更新的实体）
   */
  sopArgumentId: string;

}


/**
 * SopArgument 导入模板行 DTO
 * 对应前端 SopArgumentTemplate
 * @description 对应后端 TaktSopArgumentTemplateDto
 */
export interface SopArgumentTemplate {
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
   * 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
   */
  execId?: string;

  /**
   * 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
   */
  execStepId?: string;

  /**
   * 工序参数定义 ID（选项 TaktRoutingItemArguments/options；DictValue=Id）
   */
  routingItemParameterId?: string;

  /**
   * 参数编码
   */
  paramCode?: string;

  /**
   * 实际值
   */
  actualValue?: number;

  /**
   * 是否超差（字典 sys_yes_no；0=否，1=是）
   */
  isOutOfRange?: number;

  /**
   * 记录时间
   */
  recordedAt?: string;

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
 * SopArgument 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SopArgumentImport
 * @description 对应后端 TaktSopArgumentImportDto
 */
export interface SopArgumentImport {
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
   * 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
   */
  execId?: string;

  /**
   * 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
   */
  execStepId?: string;

  /**
   * 工序参数定义 ID（选项 TaktRoutingItemArguments/options；DictValue=Id）
   */
  routingItemParameterId?: string;

  /**
   * 参数编码
   */
  paramCode?: string;

  /**
   * 实际值
   */
  actualValue?: number;

  /**
   * 是否超差（字典 sys_yes_no；0=否，1=是）
   */
  isOutOfRange?: number;

  /**
   * 记录时间
   */
  recordedAt?: string;

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
 * SopArgument 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopArgumentExport
 * @description 对应后端 TaktSopArgumentExportDto
 */
export interface SopArgumentExport {
  /**
   * SopArgumentID
   */
  sopArgumentId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
   */
  execId: string;

  /**
   * 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
   */
  execStepId?: string;

  /**
   * 工序参数定义 ID（选项 TaktRoutingItemArguments/options；DictValue=Id）
   */
  routingItemParameterId?: string;

  /**
   * 参数编码
   */
  paramCode: string;

  /**
   * 实际值
   */
  actualValue: number;

  /**
   * 是否超差（字典 sys_yes_no；0=否，1=是）
   */
  isOutOfRange: number;

  /**
   * 记录时间
   */
  recordedAt: string;

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

