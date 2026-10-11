// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/statistics/quick-query
// 文件名称：configurableSource.types.ts
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
 * 定制报表数据源（单表及别名）
 * 对应前端 TaktConfigurableSourceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ConfigurableSource
 * @description 对应后端 TaktConfigurableSourceDto
 */
export interface ConfigurableSource extends CompanyDtoBase {
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
   * 数据源别名（如 A、B、C，用于 JOIN 与字段引用）
   */
  sourceAlias?: string;

  /**
   * 物理表名（须为 takt_ 前缀业务表，运行时白名单校验）
   */
  tableName?: string;

  /**
   * 是否主表（驱动 FROM 的第一张表）
   */
  isPrimary?: number;

  /**
   * 排序号（多表 FROM 顺序）
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
 * ConfigurableSource 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ConfigurableSourceExport
 * @description 对应后端 TaktConfigurableSourceExportDto
 */
export interface ConfigurableSourceExport {
  /**
   * ConfigurableSourceID
   */
  configurableSourceId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 关联定制报表主表 ID（主子表关系）
   */
  configurableId: string;

  /**
   * 数据源别名（如 A、B、C，用于 JOIN 与字段引用）
   */
  sourceAlias: string;

  /**
   * 物理表名（须为 takt_ 前缀业务表，运行时白名单校验）
   */
  tableName: string;

  /**
   * 是否主表（驱动 FROM 的第一张表）
   */
  isPrimary: number;

  /**
   * 排序号（多表 FROM 顺序）
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

