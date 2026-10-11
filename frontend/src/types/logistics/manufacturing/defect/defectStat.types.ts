// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/defect
// 文件名称：defectStat.types.ts
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：不良统计 DTO（数据看板 defect-stat，按生产班组分行）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 不良统计查询
 * @description 对应后端 TaktDefectStatQueryDto
 */
export interface DefectStatQuery {
  /** 生产日期（范围-开始） */
  prodDateStart?: string;
  /** 生产日期（范围-结束） */
  prodDateEnd?: string;
}

/**
 * 不良统计按生产班组行
 * @description 对应后端 TaktDefectStatTeamItemDto
 */
export interface DefectStatTeamItem {
  /** 生产班组编码 */
  teamCode: string;
  /** 生产数（分母） */
  baseQty: number;
  /** 无不良台数 */
  goodQty: number;
  /** 不良数 */
  defectQty: number;
  /** 直行率（%） */
  yieldRatePercent: number;
  /** 不良率（%） */
  defectRatePercent: number;
}

/**
 * 不良统计（组立/PCBA 检查/改修共用）
 * @description 对应后端 TaktDefectStatDto
 */
export interface DefectStat {
  /** 统计月份 yyyy-MM */
  statMonth: string;
  /** 月统计分母合计 */
  monthBaseQty: number;
  /** 月良品/无不良数量合计 */
  monthGoodQty: number;
  /** 月不良数量合计 */
  monthDefectQty: number;
  /** 月不良率（%） */
  monthDefectRatePercent: number;
  /** 月直行率（%） */
  monthYieldRatePercent: number;
  /** 按生产班组分行 */
  teams: DefectStatTeamItem[];
}

export type AssyDefectStat = DefectStat;
export type PcbaInspectionStat = DefectStat;
export type PcbaRepairStat = DefectStat;
