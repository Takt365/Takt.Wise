// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/defect
// 文件名称：defectOrderStat.types.ts
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：不良统计（工单 / 批次）查询与行类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 不良统计查询
 */
export interface DefectOrderStatQuery {
  /** 页签 */
  statTab?: string
  /** 工厂 */
  plantCode?: string
  /** 期间起 */
  periodDateStart?: string
  /** 期间止 */
  periodDateEnd?: string
  /** 工单 */
  prodOrderCode?: string
  /** 批次 */
  batchCode?: string
  /** 机种 */
  modelCode?: string
  /** 物料 */
  materialCode?: string
  /** 生产类别（字典 logistics_manufacturing_prod_category） */
  prodCategory?: string
  /** 页码 */
  pageIndex?: number
  /** 页大小 */
  pageSize?: number
}

/**
 * 不良统计行。组立按实体全列，SMT 检查与 PCBA 修理按明细
 */
export interface DefectOrderStatRow {
  /** 行键 */
  rowKey?: string
  /** 工厂 */
  plantCode: string
  /** 生产类别 */
  prodCategory?: string
  /** 工单类别 */
  prodOrderType?: string
  /** 备注 */
  remark?: string
  /** 行号 */
  lineNumber?: number
  /** 明细生产班组 */
  detailTeamCode?: string
  /** 工单 */
  prodOrderCode: string
  /** 生产日期组 */
  prodDateGroup?: string
  /** 生产工单组 */
  prodOrderGroup?: string
  /** 批次 */
  batchCode: string
  /** 机种 */
  modelCode: string
  /** 物料 */
  materialCode: string
  /** 生产物料组 */
  materialGroup?: string
  /** 工单数量 / LOT台数 / 订单台数 */
  prodOrderQty?: number
  /** 批次工单总数量 */
  batchOrderQty?: number
  /** 订单数量组 */
  prodOrderQtyGroup?: string
  /** 累计生实实绩 / 生产实绩 */
  prodActualQty?: number
  /** 实绩 */
  actualQty: number
  /** 无不良数量 */
  goodQty: number
  /** 不良数量 */
  defectQty: number
  /** 不良率 */
  defectRatePercent: number
  /** 直行率 */
  yieldRatePercent: number
  /** 最近生产日期 */
  lastProdDate?: string | null
  /** 日报笔数 */
  reportCount?: number
  /** 工单状态 */
  orderStatus?: number
  /** 批次状态 */
  batchStatus?: number
  /** 检查日期 */
  inspectionDate?: string | null
  /** 板别 */
  pcbaBoardType?: string
  /** 目视线别 */
  visualInspectionLine?: string
  /** AOI线别 */
  aoiLine?: string
  /** B面实装 */
  bSideAssemblyDate?: string | null
  /** T面实装 */
  tSideAssemblyDate?: string | null
  /** 生产班别 */
  shiftNo?: number
  /** 检查员 */
  inspectorName?: string
  /** 当日完成数 */
  dailyCompletedQty?: number
  /** 检查台数 */
  inspectionQty?: number
  /** 检查状态 */
  inspectionStatus?: number
  /** 生产线别 / 生产组别 */
  teamCode?: string
  /** 检查工数 */
  inspectionWorkHours?: number
  /** AOI工数 */
  aoiWorkHours?: number
  /** 手贴部品 */
  handPlacement?: string
  /** 序列号 */
  serialNumber?: string
  /** 内容 */
  content?: string
  /** 个所区分 */
  defectLocation?: string
  /** 生产日期 */
  prodDate?: string | null
  /** 卡号 */
  cardCode?: string
  /** 不良症状 */
  defectSymptom?: string
  /** 检出工程 */
  defectEngineering?: string
  /** 不良原因 */
  defectReason?: string
  /** 责任归属 */
  defectResponsibility?: string
  /** 不良性质 */
  defectNature?: string
  /** 修理 */
  repairOperatorName?: string
}

/**
 * 不良统计结果
 */
export interface DefectOrderStatResult {
  /** 分页 */
  paged: {
    /** 行 */
    data: DefectOrderStatRow[]
    /** 总数 */
    total: number
  }
  /** 行数 */
  rowCount: number
}
