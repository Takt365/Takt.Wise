// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/defect
// 文件名称：pcbaInspection.types.ts
// 创建时间：2026-07-09
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
 * PCBA检查日报实体 不良率(%) = 明细不良数量合计 ÷ 明细检查数量合计 × 100%；直行率(%) = (检查数量 - 不良数量) ÷ 检查数量 × 100%。
 * 对应前端 TaktPcbaInspectionDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PcbaInspection
 * @description 对应后端 TaktPcbaInspectionDto
 */
export interface PcbaInspection extends CompanyDtoBase {

  /**
   * 生产类别（字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory?: string;

  /**
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）
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
   * PCBA检查明细列表（子表，级联保存）
   */
  pcbaInspectionDetails?: PcbaInspectionDetailCreate[];

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
 * PcbaInspection 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PcbaInspectionExport
 * @description 对应后端 TaktPcbaInspectionExportDto
 */
export interface PcbaInspectionExport {
  /**
   * PcbaInspectionID
   */
  pcbaInspectionId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（回填：随工单）
   */
  plantCode: string;

  /**
   * 生产类别（字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory: string;

  /**
   * 工单类别（回填：随工单）
   */
  prodOrderType?: string;

  /**
   * 工单号（选项 TaktProductionOrders/options，按 PlantCode 过滤）
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

