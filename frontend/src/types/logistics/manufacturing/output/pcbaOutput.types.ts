// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：pcbaOutput.types.ts
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
 * PCBA日报实体 达成率(%) = 明细当日完成数量合计 ÷ 明细人员标准产能合计 × 100%。
 * 对应前端 TaktPcbaOutputDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PcbaOutput
 * @description 对应后端 TaktPcbaOutputDto
 */
export interface PcbaOutput extends CompanyDtoBase {
  /**
   * PcbaOutputID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  pcbaOutputId: string;

  /**
   * 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory: string;

  /**
   * 生产日期
   */
  prodDate: string;

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
   * PCBA明细列表 （子表：TaktPcbaOutputDetail）
   */
  pcbaOutputDetails?: PcbaOutputDetail[];

}


/**
 * PcbaOutput 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 PcbaOutputQuery
 * @description 对应后端 TaktPcbaOutputQueryDto
 */
export interface PcbaOutputQuery extends TaktPagedQuery {
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
 * 创建PcbaOutput DTO
 * 对应前端 PcbaOutputCreate
 * @description 对应后端 TaktPcbaOutputCreateDto
 */
export interface PcbaOutputCreate {
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
   * PCBA明细列表（子表，级联保存）
   */
  pcbaOutputDetails?: PcbaOutputDetailCreate[];

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
 * 更新PcbaOutput DTO
 * 继承 TaktPcbaOutputCreateDto，添加 PcbaOutputId 字段
 * 对应前端 PcbaOutputUpdate
 * @description 对应后端 TaktPcbaOutputUpdateDto
 */
export interface PcbaOutputUpdate extends PcbaOutputCreate {
  /**
   * PcbaOutputID（标识要更新的实体）
   */
  pcbaOutputId: string;

  /**
   * PCBA明细列表（子表，级联保存）
   */
  pcbaOutputDetails?: any;

}


/**
 * PcbaOutput 导入模板行 DTO
 * 对应前端 PcbaOutputTemplate
 * @description 对应后端 TaktPcbaOutputTemplateDto
 */
export interface PcbaOutputTemplate {
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
   * PCBA明细列表（子表，级联保存）
   */
  pcbaOutputDetails?: PcbaOutputDetailCreate[];

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
 * PcbaOutput 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 PcbaOutputImport
 * @description 对应后端 TaktPcbaOutputImportDto
 */
export interface PcbaOutputImport {
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
   * PCBA明细列表（子表，级联保存）
   */
  pcbaOutputDetails?: PcbaOutputDetailCreate[];

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
 * PcbaOutput 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PcbaOutputExport
 * @description 对应后端 TaktPcbaOutputExportDto
 */
export interface PcbaOutputExport {
  /**
   * PcbaOutputID
   */
  pcbaOutputId: string;

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

