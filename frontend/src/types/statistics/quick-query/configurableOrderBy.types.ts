// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/statistics/quick-query
// 文件名称：configurableOrderBy.types.ts
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
 * 定制报表排序字段定义
 * 对应前端 TaktConfigurableOrderByDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ConfigurableOrderBy
 * @description 对应后端 TaktConfigurableOrderByDto
 */
export interface ConfigurableOrderBy extends CompanyDtoBase {
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
   * 排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）
   */
  sortDirection?: string;

  /**
   * 排序号（ORDER BY 优先级）
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
 * ConfigurableOrderBy 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ConfigurableOrderByExport
 * @description 对应后端 TaktConfigurableOrderByExportDto
 */
export interface ConfigurableOrderByExport {
  /**
   * ConfigurableOrderByID
   */
  configurableOrderById: string;

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
   * 排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）
   */
  sortDirection: string;

  /**
   * 排序号（ORDER BY 优先级）
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

