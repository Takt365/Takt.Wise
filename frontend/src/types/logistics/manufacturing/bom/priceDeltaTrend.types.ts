// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：priceDeltaTrend.types.ts
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：成本差异推移类型（独立模块）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common.types'

/** 成本差异推移查询 */
export interface BomPriceDeltaTrendQuery extends TaktPagedQuery {
  plantCode: string
  /** 物料类型（空则后端默认 FERT） */
  materialType?: string
  modelCode?: string
  productCode?: string
  costingDateStart?: string
  costingDateEnd?: string
  /** 基准月 yyyy-MM（关注月；须在核算期间列内） */
  basePeriod?: string
  /** 比较月 yyyy-MM（基期；须在核算期间列内，可与基准月不相邻） */
  comparePeriod?: string
  /**
   * 差异选项：all=全部（默认）；gt1/gt5/gt10/gt50/gt100=仅 |差异|≥该值的产品行。界面阈值为 >=1 等符号（四语不翻译）
   */
  priceDeltaOption?: string
  /** 兼容旧参数；空则用 basePeriod */
  focusPeriod?: string
}

/** 成本差异推移行 */
export interface BomPriceDeltaTrend {
  plantCode: string
  modelCode: string
  productCode: string
  productDescription: string
  periodCosts: Record<string, number>
  /** 差异：价格差异组 Summary Var + 组件差异 Summary Var */
  priceDelta?: number | null
  /** 0价格组 物料:用量:可替代:替代价, …；用量=成本明细期间末日快照按键去重后合计 */
  zeroPriceGroup: string
  /** 建议替代价格 零价组件:用量→源替代:单价×用量, …；用量同合并合计口径 */
  replaceComponentGroup: string
  /** 价格差异组 组件:用量:基期价→关注价,Diff:差价×用量, …,Summary Var:Diff 合计 */
  priceDeltaTrend: string
  /** 组件差异 …→remove/new/version, …,Summary Var:N-{新增}-R-{删除}={净值}；与价格组 Summary 之和=差异 */
  componentDeltaGroup: string
  /**
   * 改修：基准月内实际开始、工单类别 ZDTB、物料编码匹配产品编码的工单号清单（逗号分隔）
   */
  reworkOrderGroup: string
  basePeriod?: string | null
  comparePeriod?: string | null
}

/** 成本差异推移结果 */
export interface BomPriceDeltaTrendResult {
  paged: TaktPagedResult<BomPriceDeltaTrend>
  periodOrder: string[]
  basePeriod?: string | null
  comparePeriod?: string | null
}
