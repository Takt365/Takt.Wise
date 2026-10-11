// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/workflow
// 文件名称：flowForm.types.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：workflow 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 流程表单定义实体
 * 对应前端 TaktFlowFormDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 FlowForm
 * @description 对应后端 TaktFlowFormDto
 */
export interface FlowForm extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 表单编码（公司内唯一）
   */
  formCode?: string;

  /**
   * 表单名称
   */
  formName?: string;

  /**
   * 表单分类（字典 sys_form_category）
   */
  formCategory?: number;

  /**
   * 表单类型（字典 sys_form_type）
   */
  formType?: number;

  /**
   * 表单设计 JSON
   */
  formConfig?: string;

  /**
   * 表单模板 JSON
   */
  formTemplate?: string;

  /**
   * 表单版本标签
   */
  formVersion?: string;

  /**
   * 是否绑定数据源
   */
  isDatasource?: number;

  /**
   * 关联库名
   */
  relatedDataBaseName?: string;

  /**
   * 关联表名
   */
  relatedTableName?: string;

  /**
   * 关联映射 JSON
   */
  relatedFormField?: string;

  /**
   * 排序号
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
 * FlowForm 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FlowFormExport
 * @description 对应后端 TaktFlowFormExportDto
 */
export interface FlowFormExport {
  /**
   * FlowFormID
   */
  flowFormId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 表单编码（公司内唯一）
   */
  formCode: string;

  /**
   * 编码规则编码（按表单分类选规则取号；对应 TaktNumbering.RuleCode；不落库）
   */
  numberingRuleCode?: string;

  /**
   * 表单名称
   */
  formName: string;

  /**
   * 表单分类（字典 sys_form_category）
   */
  formCategory: number;

  /**
   * 表单类型（字典 sys_form_type）
   */
  formType: number;

  /**
   * 表单设计 JSON
   */
  formConfig?: string;

  /**
   * 表单模板 JSON
   */
  formTemplate?: string;

  /**
   * 表单版本标签
   */
  formVersion: string;

  /**
   * 是否绑定数据源
   */
  isDatasource: number;

  /**
   * 关联库名
   */
  relatedDataBaseName?: string;

  /**
   * 关联表名
   */
  relatedTableName?: string;

  /**
   * 关联映射 JSON
   */
  relatedFormField?: string;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 表单状态
   */
  formStatus: number;

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

