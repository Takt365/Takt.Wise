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

/**
 * 设变统计查询
 */
export interface EcStatQuery {
  /** 执行部门页签 */
  statTab?: string
  /** 实施页签：all / done / open */
  implStatus?: string
  /** 工厂 */
  plantCode?: string
  /** 发行期间起（技术主表发行日期） */
  periodDateStart?: string
  /** 发行期间止（技术主表发行日期） */
  periodDateEnd?: string
  /** 录入期间起（技术主表录入日期） */
  entryPeriodDateStart?: string
  /** 录入期间止（技术主表录入日期） */
  entryPeriodDateEnd?: string
  /** 设变单号 */
  ecCode?: string
  /** 关键词（设变单号、标题、担当） */
  keyWords?: string
  /** 页码 */
  pageIndex?: number
  /** 页大小 */
  pageSize?: number
}

/**
 * 设变统计行。字段来自当前执行部门实施表，未使用的部门字段为空
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
  /** 新物料编码 */
  ecNewMaterialCode?: string | null
  /** 新品仓库 */
  ecNewWarehouse?: string | null
  /** 新品检验（sys_yes_no） */
  ecNewRequiresInspection?: number | null
  /** 实施（sys_yes_no） */
  isImplemented?: number | null
  /** 更新 SOP（sys_yes_no）。仅制造技术课 */
  isSopUpdated?: number | null
  /** 预定日期 */
  scheduledDate?: string | null
  /** 预定批次 */
  scheduledBatch?: string | null
  /** 生产日期 */
  productionDate?: string | null
  /** 实施批次 */
  implementationBatch?: string | null
  /** 检验日期 */
  inspectionDate?: string | null
  /** 检验批次 */
  inspectionBatch?: string | null
  /** 采购订单发行日期 */
  purchaseOrderIssueDate?: string | null
  /** 受检单号 */
  iqcOrderCode?: string | null
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
