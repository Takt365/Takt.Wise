// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecTeSourceInput.types.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变来源录入相关类型
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery } from '@/types/common.types';

/**
 * 来源设变录入：公司代码与工厂代码映射结果
 */
export interface EcTeSourcePlantCode {
  /** 公司代码 */
  companyCode: string;
  /** 映射后的工厂代码 */
  plantCode: string;
}

/**
 * 未导入来源设变列表项
 */
export interface EcTeSourceEcInputItem {
  /** 设变来源主表 ID */
  sourceEcId: string;
  /** 设变号码 */
  sourceEcCode: string;
  /** 机种 */
  sourceModel: string;
  /** 标题 */
  sourceTitle: string;
  /** 发行日期 */
  sourceIssueDate: string;
  /** 来源状态（来源 PLM 英文；导入时映射为设变变更状态） */
  sourceStatus: string;
  /** TCJ担当 */
  sourceTcjOwner?: string;
  /** 来源明细行数 */
  detailCount: number;
}

/**
 * 未导入来源设变分页查询
 */
export interface EcTeSourceEcInputQuery extends TaktPagedQuery {
  /** 目标工厂代码（可选；服务端按当前公司代码 1:1 映射） */
  plantCode?: string;
  /** 设变号码（模糊） */
  sourceEcCode?: string;
  /** 标题（模糊） */
  sourceTitle?: string;
  /** 排序字段（sourceEcCode、detailCount、sourceIssueDate） */
  sortField?: string;
  /** 是否降序 */
  sortDesc?: boolean;
}

/**
 * 从来源设变导入请求
 */
export interface EcTeImportFromSource {
  /** 目标工厂代码（可选；服务端按来源设变公司代码 1:1 映射） */
  plantCode?: string;
  /** 区域文化编码（写入设变主表 CultureCode；优先于 companyDefaultCulture） */
  cultureCode?: string;
  /** @deprecated 请传 cultureCode；兼容旧入参 */
  companyDefaultCulture?: string;
  /** 待导入来源设变 ID 列表 */
  sourceEcIds: string[];
}

/**
 * 从来源设变导入结果
 */
export interface EcTeImportFromSourceResult {
  /** 成功条数 */
  successCount: number;
  /** 失败条数 */
  failCount: number;
  /** 错误信息 */
  errors: string[];
  /** 新创建的设变 ID 列表 */
  createdEcTeIds: string[];
}

/**
 * 从来源设变构建创建草稿请求（不落库）
 */
export interface EcTeDraftFromSource {
  /** 目标工厂代码（可选；服务端按来源设变公司代码 1:1 映射） */
  plantCode?: string;
  /** 来源设变主表 ID */
  sourceEcId: string;
  /** 区域文化编码（写入设变主表 CultureCode；优先于 companyDefaultCulture） */
  cultureCode?: string;
  /** @deprecated 请传 cultureCode；兼容旧入参 */
  companyDefaultCulture?: string;
}
