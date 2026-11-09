// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecStat.types.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：设变统计查询与行类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery } from '@/types/common.types'

/**
 * 设变统计查询
 */
export interface EcStatQuery extends TaktPagedQuery {
  /** 执行部门页签 */
  statTab: string
  /** 实施页签：all、done、open */
  implStatus?: string
  /** 工厂代码 */
  plantCode: string
  /** 发行期间起（yyyy-MM-dd） */
  periodDateStart?: string
  /** 发行期间止（yyyy-MM-dd） */
  periodDateEnd?: string
  /** 录入期间起（yyyy-MM-dd） */
  entryPeriodDateStart?: string
  /** 录入期间止（yyyy-MM-dd） */
  entryPeriodDateEnd?: string
  /** 设变单号 */
  ecCode?: string
}

/**
 * 设变统计行。新物料编码组对应执行部门 EcNewMaterialCodeGroup
 */
export interface EcStatRow {
  /** 执行行主键 */
  id: string
  /** 设变单号 */
  ecCode: string
  /** 机种编码组 */
  ecModelCodeGroup?: string | null
  /** 根物料编码组 */
  ecRootMaterialCodeGroup?: string | null
  /** 新物料编码组 */
  ecNewMaterialCodeGroup?: string | null
  /** 新品仓库 */
  ecNewWarehouse?: string | null
  /** 新品检验 */
  ecNewRequiresInspection?: number | null
  /** 实施 */
  isImplemented?: number | null
  /** 更新 SOP */
  isSopUpdated?: number | null
  /** 预定日期 */
  scheduledDate?: string | null
  /** 预定批次 */
  scheduledBatch?: string | null
  /** 生产日期 */
  productionDate?: string | null
  /** 生产批次 */
  productionBatch?: string | null
  /** 进货日期 */
  inboundDate?: string | null
  /** 检验日期 */
  inspectionDate?: string | null
  /** 检验批次 */
  inspectionBatch?: string | null
  /** SOP 日期 */
  sopDate?: string | null
}

/**
 * 设变统计结果
 */
export interface EcStatResult {
  /** 分页 */
  paged: {
    /** 行 */
    data: EcStatRow[]
    /** 总数 */
    total: number
  }
  /** 行数 */
  rowCount: number
}
