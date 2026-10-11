// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：customerComplaintTrend.types.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/complaint 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 顾客投诉月度推移分析查询 DTO
 * 对应前端 CustomerComplaintMonthlyTrendQuery
 * @description 对应后端 TaktCustomerComplaintMonthlyTrendQueryDto
 */
export interface CustomerComplaintMonthlyTrendQuery extends TaktPagedQuery {
  /**
   * 工厂代码（必填；映射实体 PlantCode）
   */
  plantCode: string;

  /**
   * 投诉期间起（当月首日语义）
   */
  periodDateStart?: string;

  /**
   * 投诉期间止（当月首日语义）
   */
  periodDateEnd?: string;

  /**
   * 关注期间 yyyy-MM（可选）；缺省取期间末月，相对上月算环比
   */
  focusPeriod?: string;

  /**
   * 客户编码（可选）
   */
  customerCode?: string;

  /**
   * 投诉类型（字典 logistics_quality_complaint_type；可选）
   */
  complaintType?: number;

  /**
   * 投诉等级（字典 logistics_quality_complaint_level；可选）
   */
  complaintLevel?: number;

  /**
   * 涨跌筛选：空=全部；up/down/flat/none；changed=仅涨或跌
   */
  trendFilter?: string;

}


/**
 * 顾客投诉月度推移转置行（行=工厂+客户，列=各月投诉件数）
 * 对应前端 CustomerComplaintMonthlyTrend
 * @description 对应后端 TaktCustomerComplaintMonthlyTrendDto
 */
export interface CustomerComplaintMonthlyTrend {
  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 客户编码（空串表示无编码）
   */
  customerCode: string;

  /**
   * 客户名称
   */
  customerName: string;

  /**
   * 各期间投诉件数（键 yyyy-MM）
   */
  periodValues: Record<string, number>;

  /**
   * 环比涨跌：none / up / down / flat
   */
  trend: string;

  /**
   * 环比基准期间
   */
  basePeriod?: string;

  /**
   * 环比对比期间
   */
  comparePeriod?: string;

  /**
   * 环比差额（对比件数 - 基准件数）
   */
  varianceAmount?: number;

  /**
   * 环比变动率（小数比率，保留 4 位）
   */
  variancePercent?: number;

}


/**
 * 顾客投诉月度推移分析结果
 * 对应前端 CustomerComplaintMonthlyTrendResult
 * @description 对应后端 TaktCustomerComplaintMonthlyTrendResultDto
 */
export interface CustomerComplaintMonthlyTrendResult {
  /**
   * 分页行
   */
  paged: number;

  /**
   * 期间列顺序 yyyy-MM
   */
  periodOrder: string[];

  /**
   * 客户行总数（分页前，已应用涨跌筛选）
   */
  customerCount: number;

  /**
   * 环比基准期间
   */
  basePeriod?: string;

  /**
   * 环比对比期间（关注月）
   */
  comparePeriod?: string;

  /**
   * 上涨行数（筛选前全量统计）
   */
  upCount: number;

  /**
   * 下跌行数（筛选前全量统计）
   */
  downCount: number;

  /**
   * 持平行数（筛选前全量统计）
   */
  flatCount: number;

  /**
   * 无法比较行数（筛选前全量统计）
   */
  noneCount: number;

}

