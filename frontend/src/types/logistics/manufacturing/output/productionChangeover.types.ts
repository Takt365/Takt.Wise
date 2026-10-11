// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：productionChangeover.types.ts
// 创建时间：2026-07-06
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
 * 生产切换记录实体
 * 对应前端 TaktProductionChangeoverDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ProductionChangeover
 * @description 对应后端 TaktProductionChangeoverDto
 */
export interface ProductionChangeover extends CompanyDtoBase {

  /**
   * 生产类别（字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory?: string;

  /**
   * 切换类别（字典 logistics_manufacturing_changeover_category，存 DictValue：ASSY/PCBA）
   */
  changeoverCategory?: string;

  /**
   * 生产日期
   */
  prodDate?: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 按工厂过滤）
   */
  TeamCode?: string;

  /**
   * 当前工单（切换前工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤）
   */
  currentProdOrderCode?: string;

  /**
   * 当前机种（回填：随工单）
   */
  currentModelCode?: string;

  /**
   * 切换后工单（切换目标工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤）
   */
  changeoverProdOrderCode?: string;

  /**
   * 切换后机种（回填：随工单）
   */
  changeoverModelCode?: string;

  /**
   * 切换次数
   */
  changeoverCount?: number;

  /**
   * 切换时间（单次，单位：分钟）
   */
  changeoverTime?: number;

  /**
   * 仪设时间（仪器/设备设置耗时，单位：分钟）
   */
  instrumentSetupTime?: number;

  /**
   * 切换总时间（单位：分钟）
   */
  totalChangeoverTime?: number;

  /**
   * 读取SOP时间（单位：分钟）
   */
  readSopTime?: number;

  /**
   * 学习时间（切换学习/培训耗时，单位：分钟）
   */
  learningTime?: number;

  /**
   * 人数（参与切换人数）
   */
  personCount?: number;

  /**
   * 学习总时间（单位：分钟）
   */
  totalLearningTime?: number;

  /**
   * SOP总时间（单位：分钟）
   */
  totalSopTime?: number;

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
 * ProductionChangeover 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProductionChangeoverExport
 * @description 对应后端 TaktProductionChangeoverExportDto
 */
export interface ProductionChangeoverExport {
  /**
   * ProductionChangeoverID
   */
  productionChangeoverId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 生产工厂（回填：随工单）
   */
  plantCode: string;

  /**
   * 生产类别（字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory?: string;

  /**
   * 切换类别（字典 logistics_manufacturing_changeover_category，存 DictValue：ASSY/PCBA）
   */
  changeoverCategory: string;

  /**
   * 生产日期
   */
  prodDate: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 按工厂过滤）
   */
  TeamCode?: string;

  /**
   * 当前工单（切换前工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤）
   */
  currentProdOrderCode: string;

  /**
   * 当前机种（回填：随工单）
   */
  currentModelCode: string;

  /**
   * 切换后工单（切换目标工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤）
   */
  changeoverProdOrderCode: string;

  /**
   * 切换后机种（回填：随工单）
   */
  changeoverModelCode: string;

  /**
   * 切换次数
   */
  changeoverCount: number;

  /**
   * 切换时间（单次，单位：分钟）
   */
  changeoverTime: number;

  /**
   * 仪设时间（仪器/设备设置耗时，单位：分钟）
   */
  instrumentSetupTime: number;

  /**
   * 切换总时间（单位：分钟）
   */
  totalChangeoverTime: number;

  /**
   * 读取SOP时间（单位：分钟）
   */
  readSopTime: number;

  /**
   * 学习时间（切换学习/培训耗时，单位：分钟）
   */
  learningTime: number;

  /**
   * 人数（参与切换人数）
   */
  personCount: number;

  /**
   * 学习总时间（单位：分钟）
   */
  totalLearningTime: number;

  /**
   * SOP总时间（单位：分钟）
   */
  totalSopTime: number;

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

