// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/defect
// 文件名称：assyBatchDefect.types.ts
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
 * 组立批量不良统计实体（统计维度：生产类别+批次）
 * 对应前端 TaktAssyBatchDefectDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 AssyBatchDefect
 * @description 对应后端 TaktAssyBatchDefectDto
 */
export interface AssyBatchDefect extends CompanyDtoBase {

  /**
   * 生产类别（统计维度，字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory?: string;

  /**
   * 批次（统计维度）
   */
  batchCode?: string;

  /**
   * 生产日期组（与生产工单组一一对应，yyyy-MM-dd 逗号分隔，取同工单最早生产日期）
   */
  prodDateGroup?: string;

  /**
   * 生产工单组（同批次 Distinct 工单号逗号分隔，与生产日期组、生产物料组、订单数量组一一对应）
   */
  prodOrderGroup?: string;

  /**
   * 机种（取最近日报）
   */
  modelCode?: string;

  /**
   * 生产物料组（与生产工单组一一对应，逗号分隔，同工单取最近日报物料编码）
   */
  materialGroup?: string;

  /**
   * 批次工单总数量（同批次下各生产工单订单数量汇总：同工单取最大订单数量再合计）
   */
  batchOrderQty?: number;

  /**
   * 订单数量组（与生产工单组一一对应，逗号分隔，同工单取最大订单数量）
   */
  prodOrderQtyGroup?: string;

  /**
   * 累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）
   */
  prodActualQty?: number;

  /**
   * 累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）
   */
  goodQuantity?: number;

  /**
   * 累计不良数量（计算：累计生实实绩 - 累计无不良数量）
   */
  defectQty?: number;

  /**
   * 不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）
   */
  defectRatePercent?: number;

  /**
   * 直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）
   */
  yieldRatePercent?: number;

  /**
   * 最近生产日期（关联日报最大 ProdDate）
   */
  lastProdDate?: string;

  /**
   * 关联组立不良日报笔数
   */
  reportCount?: number;

  /**
   * 批次状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）
   */
  batchStatus?: number;

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
 * AssyBatchDefect 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 AssyBatchDefectExport
 * @description 对应后端 TaktAssyBatchDefectExportDto
 */
export interface AssyBatchDefectExport {
  /**
   * AssyBatchDefectID
   */
  assyBatchDefectId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（取最近日报，关联 TaktPlant.PlantCode）
   */
  plantCode: string;

  /**
   * 生产类别（统计维度，字典 logistics_manufacturing_prod_category，存 DictValue：EPP/FPP/RWP/MDP/CPP）
   */
  prodCategory: string;

  /**
   * 批次（统计维度）
   */
  batchCode: string;

  /**
   * 生产日期组（与生产工单组一一对应，yyyy-MM-dd 逗号分隔，取同工单最早生产日期）
   */
  prodDateGroup?: string;

  /**
   * 生产工单组（同批次 Distinct 工单号逗号分隔，与生产日期组、生产物料组、订单数量组一一对应）
   */
  prodOrderGroup?: string;

  /**
   * 机种（取最近日报）
   */
  modelCode: string;

  /**
   * 生产物料组（与生产工单组一一对应，逗号分隔，同工单取最近日报物料编码）
   */
  materialGroup?: string;

  /**
   * 批次工单总数量（同批次下各生产工单订单数量汇总：同工单取最大订单数量再合计）
   */
  batchOrderQty: number;

  /**
   * 订单数量组（与生产工单组一一对应，逗号分隔，同工单取最大订单数量）
   */
  prodOrderQtyGroup?: string;

  /**
   * 累计生实实绩（汇总 TaktAssyDefect.ProdActualQty）
   */
  prodActualQty: number;

  /**
   * 累计无不良数量（汇总 TaktAssyDefect.GoodQuantity）
   */
  goodQuantity: number;

  /**
   * 累计不良数量（计算：累计生实实绩 - 累计无不良数量）
   */
  defectQty: number;

  /**
   * 不良率（%，计算：累计不良数量 ÷ 累计生实实绩 × 100）
   */
  defectRatePercent: number;

  /**
   * 直行率（%，计算：累计无不良数量 ÷ 累计生实实绩 × 100）
   */
  yieldRatePercent: number;

  /**
   * 最近生产日期（关联日报最大 ProdDate）
   */
  lastProdDate?: string;

  /**
   * 关联组立不良日报笔数
   */
  reportCount: number;

  /**
   * 批次状态（字典 logistics_manufacturing_prod_status；1=进行中 2=已完成；批次工单总数量与累计生实实绩相等时为已完成）
   */
  batchStatus: number;

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

