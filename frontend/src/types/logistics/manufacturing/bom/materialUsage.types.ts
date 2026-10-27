// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：materialUsage.types.ts
// 创建时间：2026-10-21
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 通用物料 / 专用物料查询与行类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common.types'

/**
 * 通用物料 / 专用物料查询
 */
export interface BomMaterialUsageQuery extends TaktPagedQuery {
  /**
   * 工厂代码
   */
  plantCode: string

  /**
   * 组件物料编码（与机种互斥；空=不按物料收窄）
   */
  componentCode?: string

  /**
   * 机种编码（空=全部机种；已选物料时不传）
   */
  modelCode?: string

  /**
   * 核算年月 yyyy-MM
   */
  costingPeriod?: string
}

/**
 * 通用物料或专用物料行
 */
export interface BomMaterialUsageRow {
  /**
   * 行键（机种|组件）
   */
  usageKey: string

  /**
   * 工厂代码
   */
  plantCode: string

  /**
   * 对比月 yyyy-MM
   */
  costingPeriod: string

  /**
   * 机种编码
   */
  modelCode: string

  /**
   * 组件编码
   */
  componentCode: string

  /**
   * 组件描述
   */
  componentDescription: string

  /**
   * 用到该组件的成品数
   */
  productCount: number

  /**
   * 用量清单（Net 与成品:用量-行成本）
   */
  usageText: string

  /**
   * 行成本合计
   */
  lineCost: number
}

/**
 * 分页结果
 */
export type BomMaterialUsageResult = TaktPagedResult<BomMaterialUsageRow>
