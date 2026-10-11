// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：qualityInspectionStat.types.ts
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：IQC/IPQC/FQC 检验统计（数据看板 inspection-stat）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 质量检验统计（IQC/IPQC/FQC 共用）
 * @description 对应后端 TaktQualityInspectionStatDto
 */
export interface QualityInspectionStat {
  /** 统计月份 yyyy-MM */
  statMonth: string;
  /** 月检验单数量 */
  monthOrderCount: number;
  /** 月抽样数量合计 */
  monthSampleQuantity: number;
  /** 月合格数量合计 */
  monthQualifiedQuantity: number;
  /** 月不合格数量合计 */
  monthUnqualifiedQuantity: number;
  /** 月合格率（%） */
  monthPassRatePercent: number;
}

/** IQC 检验统计 */
export type IqcOrderStat = QualityInspectionStat;

/** IPQC 检验统计 */
export type IpqcOrderStat = QualityInspectionStat;

/** FQC 检验统计 */
export type FqcOrderStat = QualityInspectionStat;
