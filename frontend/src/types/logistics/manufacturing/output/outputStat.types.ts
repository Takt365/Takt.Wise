// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/output
// 文件名称：outputStat.types.ts
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：产出生产统计 DTO（数据看板 production-stat / OPH，按生产班组分行）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 生产统计查询
 * @description 对应后端 TaktOutputProductionStatQueryDto
 */
export interface OutputProductionStatQuery {
  /** 生产日期（范围-开始） */
  prodDateStart?: string;
  /** 生产日期（范围-结束） */
  prodDateEnd?: string;
}

/**
 * 生产统计按生产班组行
 * @description 对应后端 TaktOutputProductionStatTeamItemDto
 */
export interface OutputProductionStatTeamItem {
  /** 生产班组编码 */
  teamCode: string;
  /** 计划数（标准产能） */
  stdCapacity: number;
  /** 生产数（实际产量） */
  prodActualQty: number;
  /** 达成率（%） */
  achievementRate: number;
}

/**
 * 生产统计（组立/PCBA 共用）
 * @description 对应后端 TaktOutputProductionStatDto
 */
export interface OutputProductionStat {
  /** 统计月份 yyyy-MM */
  statMonth: string;
  /** 月标准产能合计 */
  monthStdCapacity: number;
  /** 月实际产量合计 */
  monthProdActualQty: number;
  /** 月达成率（%） */
  monthAchievementRate: number;
  /** 月停线损失（分钟） */
  monthDowntimeMinutes: number;
  /** 月投入工时（分钟） */
  monthInputMinutes: number;
  /** 月生产工时（分钟） */
  monthProdMinutes: number;
  /** 月实际工时（分钟） */
  monthActualMinutes: number;
  /** 按生产班组分行 */
  teams: OutputProductionStatTeamItem[];
}

/** 组立生产统计 */
export type AssyOutputProductionStat = OutputProductionStat;
/** PCBA 生产统计 */
export type PcbaOutputProductionStat = OutputProductionStat;
