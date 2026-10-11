/**
 * BOM 计算独立类型（对应 TaktBomCalculateDtos）
 */

/**
 * 计算成本 / 重算成本查询（须单个核算月）
 */
export interface BomCalculateQuery {
  /**
   * 工厂代码（必填；空则前端拦截）
   */
  plantCode?: string
  /**
   * 物料类型（查询栏所选；与主表 MaterialType 一致）
   */
  materialType?: string
  /**
   * 机种编码（可选；空=该类型下全部机种）
   */
  modelCode?: string
  /**
   * 产品编码（可选）
   */
  productCode?: string
  /**
   * 核算日期起（须与止同月）
   */
  costingDateStart?: string
  /**
   * 核算日期止（须与起同月）
   */
  costingDateEnd?: string
  /**
   * 处理工厂+产品组上限（0=全部）
   */
  processRecordCount?: number
}

/**
 * 提交后台计算/重算回执
 */
export interface BomCalculateSubmitted {
  /**
   * 核算月份标签（yyyy-MM）
   */
  processedMonth: string
  /**
   * 是否强制重算（归档旧成本后重写）
   */
  forceRecalculate: boolean
}

/**
 * 计算成本 / 重算成本结果
 */
export interface BomCalculateCostResult {
  /**
   * 扫描明细行数
   */
  scannedRowCount: number
  /**
   * 实际同步的工厂+产品组数
   */
  refreshedGroupCount: number
  /**
   * 跳过组数（类型不匹配 / 机种过滤 / 处理上限）
   */
  skippedGroupCount: number
  /**
   * 强制重算时计入的重置组数
   */
  resetGroupCount: number
  /**
   * 处理的核算月数
   */
  processedMonthCount: number
  /**
   * 处理的核算月份（yyyy-MM）
   */
  processedMonth: string
}

/**
 * 计算平均成本查询
 */
export interface BomCalculateAverageQuery {
  /**
   * 工厂代码（必填）
   */
  plantCode: string
  /**
   * 核算期间 yyyy-MM（必填）
   */
  costingPeriod: string
  /**
   * 物料类型（已忽略：后端始终处理全部类型；保留仅兼容旧调用）
   */
  materialType?: string
  /**
   * 机种编码（可选；传入则仅处理该机种）
   */
  modelCode?: string
}

/**
 * 计算平均成本结果
 */
export interface BomCalculateAverageResult {
  /**
   * 扫描主表行数
   */
  scannedRowCount: number
  /**
   * 机种编码更新行数
   */
  modelCodeUpdatedCount: number
  /**
   * 物料类型更新行数
   */
  materialTypeUpdatedCount: number
  /**
   * 机种月平均成本更新行数
   */
  averageUpdatedCount: number
  /**
   * 刷新的机种组数
   */
  modelGroupCount: number
  /**
   * 扫描行中产品月成本 &gt; 0 的行数
   */
  positiveProductCostRowCount: number
  /**
   * 组内至少有一行产品月成本&gt;0 的机种组数
   */
  groupsWithProductCostCount: number
  /**
   * 组内全部产品月成本为 0 的机种组数
   */
  groupsWithoutProductCostCount: number
  /**
   * 处理的核算期间
   */
  costingPeriod: string
}

/**
 * 回填 BOM 明细采购价结果
 */
export interface BomCalculatePurchaseInfoRecordBackfillResult {
  /**
   * 扫描明细行数
   */
  scannedRowCount: number
  /**
   * 命中采购价格并写回的行数
   */
  updatedRowCount: number
  /**
   * 无匹配采购价格而跳过的行数
   */
  skippedNoPriceCount: number
  /**
   * 字段未变化而跳过的行数
   */
  unchangedRowCount: number
  /**
   * 处理的核算月份（yyyy-MM）
   */
  processedMonth: string
}
