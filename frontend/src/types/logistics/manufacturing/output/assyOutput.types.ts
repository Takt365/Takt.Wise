// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：assyOutput.types.ts
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/output 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 组立日报（产出）主表实体 <para>业务唯一键：TenantCode+CompanyCode+PlantCode+ProdDate+ProdOrderCode。</para> 达成率(%) = 明细实际生产数量合计 ÷ 主表标准产能合计 × 100%。
 * 对应前端 TaktAssyOutputDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 AssyOutput
 * @description 对应后端 TaktAssyOutputDto
 */
export interface AssyOutput extends CompanyDtoBase {
  /**
   * AssyOutputID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  assyOutputId: string;

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
   * 直接人员
   */
  directLabor: number;

  /**
   * 间接人员
   */
  indirectLabor: number;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）
   */
  prodOrderCode: string;

  /**
   * 机种（回填：随工单）
   */
  modelCode: string;

  /**
   * 物料编码（回填：随工单）
   */
  materialCode: string;

  /**
   * 批次（回填：随工单）
   */
  batchCode?: string;

  /**
   * 工单数量（回填：随工单）
   */
  prodOrderQty: number;

  /**
   * 序列号（回填：随工单）
   */
  serialCode?: string;

  /**
   * 标准工时(分钟)（回填：按 MaterialCode 查询 TaktStandardOperationTime 汇总转换工时）
   */
  stdMinutes: number;

  /**
   * 标准产能（计算结果：利用标准生产稼动率计算出小时产能，DirectLabor人数*60分钟/StdMinutes标准工时*标准生产稼动率）
   */
  stdCapacity: number;

  /**
   * 组立日报明细列表 （子表：TaktAssyOutputDetail）
   */
  assyOutputDetails?: AssyOutputDetail[];

}


/**
 * AssyOutput 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 AssyOutputQuery
 * @description 对应后端 TaktAssyOutputQueryDto
 */
export interface AssyOutputQuery extends TaktPagedQuery {
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
   * 直接人员
   */
  directLabor?: number;

  /**
   * 间接人员
   */
  indirectLabor?: number;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）
   */
  prodOrderCode?: string;

  /**
   * 机种（回填：随工单）
   */
  modelCode?: string;

  /**
   * 物料编码（回填：随工单）
   */
  materialCode?: string;

  /**
   * 批次（回填：随工单）
   */
  batchCode?: string;

  /**
   * 工单数量（回填：随工单）
   */
  prodOrderQty?: number;

  /**
   * 序列号（回填：随工单）
   */
  serialCode?: string;

  /**
   * 标准工时(分钟)（回填：按 MaterialCode 查询 TaktStandardOperationTime 汇总转换工时）
   */
  stdMinutes?: number;

  /**
   * 标准产能（计算结果：利用标准生产稼动率计算出小时产能，DirectLabor人数*60分钟/StdMinutes标准工时*标准生产稼动率）
   */
  stdCapacity?: number;

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
 * 创建AssyOutput DTO
 * 对应前端 AssyOutputCreate
 * @description 对应后端 TaktAssyOutputCreateDto
 */
export interface AssyOutputCreate {
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
   * 直接人员
   */
  directLabor: number;

  /**
   * 间接人员
   */
  indirectLabor: number;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）
   */
  prodOrderCode: string;

  /**
   * 机种（回填：随工单）
   */
  modelCode: string;

  /**
   * 物料编码（回填：随工单）
   */
  materialCode: string;

  /**
   * 批次（回填：随工单）
   */
  batchCode?: string;

  /**
   * 工单数量（回填：随工单）
   */
  prodOrderQty: number;

  /**
   * 序列号（回填：随工单）
   */
  serialCode?: string;

  /**
   * 标准工时(分钟)（回填：按 MaterialCode 查询 TaktStandardOperationTime 汇总转换工时）
   */
  stdMinutes: number;

  /**
   * 标准产能（计算结果：利用标准生产稼动率计算出小时产能，DirectLabor人数*60分钟/StdMinutes标准工时*标准生产稼动率）
   */
  stdCapacity: number;

  /**
   * 组立日报明细列表（子表，级联保存）
   */
  assyOutputDetails?: AssyOutputDetailCreate[];

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
 * 更新AssyOutput DTO
 * 继承 TaktAssyOutputCreateDto，添加 AssyOutputId 字段
 * 对应前端 AssyOutputUpdate
 * @description 对应后端 TaktAssyOutputUpdateDto
 */
export interface AssyOutputUpdate extends AssyOutputCreate {
  /**
   * AssyOutputID（标识要更新的实体）
   */
  assyOutputId: string;

  /**
   * 组立日报明细列表（子表，级联保存）
   */
  assyOutputDetails?: any;

}


/**
 * AssyOutput 导入模板行 DTO
 * 对应前端 AssyOutputTemplate
 * @description 对应后端 TaktAssyOutputTemplateDto
 */
export interface AssyOutputTemplate {
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
   * 直接人员
   */
  directLabor?: number;

  /**
   * 间接人员
   */
  indirectLabor?: number;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）
   */
  prodOrderCode?: string;

  /**
   * 机种（回填：随工单）
   */
  modelCode?: string;

  /**
   * 物料编码（回填：随工单）
   */
  materialCode?: string;

  /**
   * 批次（回填：随工单）
   */
  batchCode?: string;

  /**
   * 工单数量（回填：随工单）
   */
  prodOrderQty?: number;

  /**
   * 序列号（回填：随工单）
   */
  serialCode?: string;

  /**
   * 标准工时(分钟)（回填：按 MaterialCode 查询 TaktStandardOperationTime 汇总转换工时）
   */
  stdMinutes?: number;

  /**
   * 标准产能（计算结果：利用标准生产稼动率计算出小时产能，DirectLabor人数*60分钟/StdMinutes标准工时*标准生产稼动率）
   */
  stdCapacity?: number;

  /**
   * 组立日报明细列表（子表，级联保存）
   */
  assyOutputDetails?: AssyOutputDetailCreate[];

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
 * AssyOutput 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 AssyOutputImport
 * @description 对应后端 TaktAssyOutputImportDto
 */
export interface AssyOutputImport {
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
   * 直接人员
   */
  directLabor?: number;

  /**
   * 间接人员
   */
  indirectLabor?: number;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）
   */
  prodOrderCode?: string;

  /**
   * 机种（回填：随工单）
   */
  modelCode?: string;

  /**
   * 物料编码（回填：随工单）
   */
  materialCode?: string;

  /**
   * 批次（回填：随工单）
   */
  batchCode?: string;

  /**
   * 工单数量（回填：随工单）
   */
  prodOrderQty?: number;

  /**
   * 序列号（回填：随工单）
   */
  serialCode?: string;

  /**
   * 标准工时(分钟)（回填：按 MaterialCode 查询 TaktStandardOperationTime 汇总转换工时）
   */
  stdMinutes?: number;

  /**
   * 标准产能（计算结果：利用标准生产稼动率计算出小时产能，DirectLabor人数*60分钟/StdMinutes标准工时*标准生产稼动率）
   */
  stdCapacity?: number;

  /**
   * 组立日报明细列表（子表，级联保存）
   */
  assyOutputDetails?: AssyOutputDetailCreate[];

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
 * AssyOutput 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 AssyOutputExport
 * @description 对应后端 TaktAssyOutputExportDto
 */
export interface AssyOutputExport {
  /**
   * AssyOutputID
   */
  assyOutputId: string;

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
   * 直接人员
   */
  directLabor: number;

  /**
   * 间接人员
   */
  indirectLabor: number;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktProductionOrders/options；DictValue=ProdOrderCode，ExtValue=PlantCode）
   */
  prodOrderCode: string;

  /**
   * 机种（回填：随工单）
   */
  modelCode: string;

  /**
   * 物料编码（回填：随工单）
   */
  materialCode: string;

  /**
   * 批次（回填：随工单）
   */
  batchCode?: string;

  /**
   * 工单数量（回填：随工单）
   */
  prodOrderQty: number;

  /**
   * 序列号（回填：随工单）
   */
  serialCode?: string;

  /**
   * 标准工时(分钟)（回填：按 MaterialCode 查询 TaktStandardOperationTime 汇总转换工时）
   */
  stdMinutes: number;

  /**
   * 标准产能（计算结果：利用标准生产稼动率计算出小时产能，DirectLabor人数*60分钟/StdMinutes标准工时*标准生产稼动率）
   */
  stdCapacity: number;

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

