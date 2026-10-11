// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/defect
// 文件名称：assyDefect.types.ts
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/defect 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 组立不良日报实体 不良率(%) = (生实实绩 - 无不良数量) ÷ 生实实绩 × 100%；直行率(%) = 无不良数量 ÷ 生实实绩 × 100%。
 * 对应前端 TaktAssyDefectDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 AssyDefect
 * @description 对应后端 TaktAssyDefectDto
 */
export interface AssyDefect extends CompanyDtoBase {
  /**
   * AssyDefectID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  assyDefectId: string;

  /**
   * 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory: string;

  /**
   * 生产日期
   */
  prodDate: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
   */
  teamCode: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktAssyOutputs/prod-order-options，来源组立日报；同日同工单已存在不良日报则不再展示）
   */
  prodOrderCode: string;

  /**
   * 工单数量
   */
  prodOrderQty: number;

  /**
   * 机种
   */
  modelCode: string;

  /**
   * 批次
   */
  batchCode?: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 生实实绩
   */
  prodActualQty: number;

  /**
   * 无不良数量
   */
  goodQuantity: number;

  /**
   * 组立不良明细列表 （子表：TaktAssyDefectDetail）
   */
  assyDefectDetails?: AssyDefectDetail[];

}


/**
 * AssyDefect 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 AssyDefectQuery
 * @description 对应后端 TaktAssyDefectQueryDto
 */
export interface AssyDefectQuery extends TaktPagedQuery {
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
   * 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory?: string;

  /**
   * 生产日期（范围查询-开始）
   */
  prodDateStart?: string;

  /**
   * 生产日期（范围查询-结束）
   */
  prodDateEnd?: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
   */
  teamCode?: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktAssyOutputs/prod-order-options，来源组立日报；同日同工单已存在不良日报则不再展示）
   */
  prodOrderCode?: string;

  /**
   * 工单数量
   */
  prodOrderQty?: number;

  /**
   * 机种
   */
  modelCode?: string;

  /**
   * 批次
   */
  batchCode?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 生实实绩
   */
  prodActualQty?: number;

  /**
   * 无不良数量
   */
  goodQuantity?: number;

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
 * 创建AssyDefect DTO
 * 对应前端 AssyDefectCreate
 * @description 对应后端 TaktAssyDefectCreateDto
 */
export interface AssyDefectCreate {
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
   * 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory: string;

  /**
   * 生产日期
   */
  prodDate: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
   */
  teamCode: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktAssyOutputs/prod-order-options，来源组立日报；同日同工单已存在不良日报则不再展示）
   */
  prodOrderCode: string;

  /**
   * 工单数量
   */
  prodOrderQty: number;

  /**
   * 机种
   */
  modelCode: string;

  /**
   * 批次
   */
  batchCode?: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 生实实绩
   */
  prodActualQty: number;

  /**
   * 无不良数量
   */
  goodQuantity: number;

  /**
   * 组立不良明细列表（子表，级联保存）
   */
  assyDefectDetails?: AssyDefectDetailCreate[];

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
 * 更新AssyDefect DTO
 * 继承 TaktAssyDefectCreateDto，添加 AssyDefectId 字段
 * 对应前端 AssyDefectUpdate
 * @description 对应后端 TaktAssyDefectUpdateDto
 */
export interface AssyDefectUpdate extends AssyDefectCreate {
  /**
   * AssyDefectID（标识要更新的实体）
   */
  assyDefectId: string;

  /**
   * 组立不良明细列表（子表，级联保存）
   */
  assyDefectDetails?: any;

}


/**
 * AssyDefect 导入模板行 DTO
 * 对应前端 AssyDefectTemplate
 * @description 对应后端 TaktAssyDefectTemplateDto
 */
export interface AssyDefectTemplate {
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
   * 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory?: string;

  /**
   * 生产日期
   */
  prodDate?: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
   */
  teamCode?: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktAssyOutputs/prod-order-options，来源组立日报；同日同工单已存在不良日报则不再展示）
   */
  prodOrderCode?: string;

  /**
   * 工单数量
   */
  prodOrderQty?: number;

  /**
   * 机种
   */
  modelCode?: string;

  /**
   * 批次
   */
  batchCode?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 生实实绩
   */
  prodActualQty?: number;

  /**
   * 无不良数量
   */
  goodQuantity?: number;

  /**
   * 组立不良明细列表（子表，级联保存）
   */
  assyDefectDetails?: AssyDefectDetailCreate[];

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
 * AssyDefect 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 AssyDefectImport
 * @description 对应后端 TaktAssyDefectImportDto
 */
export interface AssyDefectImport {
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
   * 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory?: string;

  /**
   * 生产日期
   */
  prodDate?: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
   */
  teamCode?: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktAssyOutputs/prod-order-options，来源组立日报；同日同工单已存在不良日报则不再展示）
   */
  prodOrderCode?: string;

  /**
   * 工单数量
   */
  prodOrderQty?: number;

  /**
   * 机种
   */
  modelCode?: string;

  /**
   * 批次
   */
  batchCode?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 生实实绩
   */
  prodActualQty?: number;

  /**
   * 无不良数量
   */
  goodQuantity?: number;

  /**
   * 组立不良明细列表（子表，级联保存）
   */
  assyDefectDetails?: AssyDefectDetailCreate[];

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
 * AssyDefect 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 AssyDefectExport
 * @description 对应后端 TaktAssyDefectExportDto
 */
export interface AssyDefectExport {
  /**
   * AssyDefectID
   */
  assyDefectId: string;

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
   * 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory: string;

  /**
   * 生产日期
   */
  prodDate: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options；DictValue=TeamCode，ExtValue=PlantCode）
   */
  teamCode: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktAssyOutputs/prod-order-options，来源组立日报；同日同工单已存在不良日报则不再展示）
   */
  prodOrderCode: string;

  /**
   * 工单数量
   */
  prodOrderQty: number;

  /**
   * 机种
   */
  modelCode: string;

  /**
   * 批次
   */
  batchCode?: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 生实实绩
   */
  prodActualQty: number;

  /**
   * 无不良数量
   */
  goodQuantity: number;

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

