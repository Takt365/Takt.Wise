// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/statistics/quick-query
// 文件名称：configurable.types.ts
// 创建时间：2026-06-24
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
 * 定制报表主实体（快速查询定义）
 * 对应前端 TaktConfigurableDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Configurable
 * @description 对应后端 TaktConfigurableDto
 */
export interface Configurable extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 定制报表编码（租户+公司内唯一）
   */
  configurableCode?: string;

  /**
   * 定制报表名称
   */
  configurableName?: string;

  /**
   * 定制报表业务域（TaktModule 整型，与一级目录菜单 MenuCode 映射；展示名取自菜单 i18n）
   */
  configurableDomain?: number;

  /**
   * 定制报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
   */
  configurableSubCategory?: string;

  /**
   * 是否去重行（SELECT DISTINCT）
   */
  distinctRows?: number;

  /**
   * 单次导出最大行数（Excel 上限，防止 OOM）
   */
  maxExportRows?: number;

  /**
   * 单次查询最大行数（预览/分页上限）
   */
  maxQueryRows?: number;

  /**
   * 公开（字典 sys_public_type；0=公开，1=私有）
   */
  isPublic?: number;

  /**
   * 定制报表状态（0=禁用 1=启用）
   */
  configurableStatus?: number;

  /**
   * 定制报表描述
   */
  configurableDescription?: string;

  /**
   * 数据源表列表（FROM）（子表，级联保存）
   */
  sources?: ConfigurableSourceCreate[];

  /**
   * 多表关联列表（JOIN）（子表，级联保存）
   */
  joins?: ConfigurableJoinCreate[];

  /**
   * 输出字段列表（SELECT）（子表，级联保存）
   */
  fields?: ConfigurableFieldCreate[];

  /**
   * 筛选条件列表（WHERE）（子表，级联保存）
   */
  selections?: ConfigurableSelectionCreate[];

  /**
   * 分组字段列表（GROUP BY）（子表，级联保存）
   */
  groupBys?: ConfigurableGroupByCreate[];

  /**
   * 排序字段列表（ORDER BY）（子表，级联保存）
   */
  orderBys?: ConfigurableOrderByCreate[];

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}

/**
 * Configurable 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ConfigurableExport
 * @description 对应后端 TaktConfigurableExportDto
 */
export interface ConfigurableExport {
  /**
   * ConfigurableID
   */
  configurableId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 定制报表编码（租户+公司内唯一）
   */
  configurableCode: string;

  /**
   * 编码规则编码（表单选规则后取号；对应 TaktNumbering.RuleCode；不落库）
   */
  numberingRuleCode?: string;

  /**
   * 定制报表名称
   */
  configurableName: string;

  /**
   * 定制报表业务域（TaktModule 整型，与一级目录菜单 MenuCode 映射；展示名取自菜单 i18n）
   */
  configurableDomain: number;

  /**
   * 定制报表子分类（与菜单末级路由段对齐，如 management、controlling、material）
   */
  configurableSubCategory?: string;

  /**
   * 是否去重行（SELECT DISTINCT）
   */
  distinctRows: number;

  /**
   * 单次导出最大行数（Excel 上限，防止 OOM）
   */
  maxExportRows: number;

  /**
   * 单次查询最大行数（预览/分页上限）
   */
  maxQueryRows: number;

  /**
   * 公开（字典 sys_public_type；0=公开，1=私有）
   */
  isPublic: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 定制报表状态（0=禁用 1=启用）
   */
  configurableStatus: number;

  /**
   * 定制报表描述
   */
  configurableDescription?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}

