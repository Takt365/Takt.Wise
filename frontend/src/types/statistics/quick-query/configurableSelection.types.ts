// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/statistics/quick-query
// 文件名称：configurableSelection.types.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：statistics/quick-query 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 定制报表筛选条件（运行时筛选项）
 * 对应前端 TaktConfigurableSelectionDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ConfigurableSelection
 * @description 对应后端 TaktConfigurableSelectionDto
 */
export interface ConfigurableSelection extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 关联定制报表主表 ID（主子表关系）
   */
  configurableId?: string;

  /**
   * 数据源别名
   */
  sourceAlias?: string;

  /**
   * 列名
   */
  columnName?: string;

  /**
   * 显示名称（筛选项 标签）
   */
  displayName?: string;

  /**
   * 比较运算符
   */
  filterOperator?: number;

  /**
   * 默认值（单值或 IN 列表逗号分隔）
   */
  defaultValue?: string;

  /**
   * 区间结束值（BETWEEN 时使用）
   */
  defaultValueTo?: string;

  /**
   * 是否必填（0=否 1=是）
   */
  isRequired?: number;

  /**
   * 排序号（筛选项 展示顺序）
   */
  sortOrder?: number;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}

/**
 * ConfigurableSelection 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ConfigurableSelectionExport
 * @description 对应后端 TaktConfigurableSelectionExportDto
 */
export interface ConfigurableSelectionExport {
  /**
   * ConfigurableSelectionID
   */
  configurableSelectionId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 关联定制报表主表 ID（主子表关系）
   */
  configurableId: string;

  /**
   * 数据源别名
   */
  sourceAlias: string;

  /**
   * 列名
   */
  columnName: string;

  /**
   * 显示名称（筛选项 标签）
   */
  displayName: string;

  /**
   * 比较运算符
   */
  filterOperator: number;

  /**
   * 默认值（单值或 IN 列表逗号分隔）
   */
  defaultValue?: string;

  /**
   * 区间结束值（BETWEEN 时使用）
   */
  defaultValueTo?: string;

  /**
   * 是否必填（0=否 1=是）
   */
  isRequired: number;

  /**
   * 排序号（筛选项 展示顺序）
   */
  sortOrder: number;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

