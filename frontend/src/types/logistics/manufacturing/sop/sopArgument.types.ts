// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：sopArgument.types.ts
// 创建时间：2026-06-15
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/sop 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * SOP 作业参数实体
 * 对应前端 TaktSopArgumentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopArgument
 * @description 对应后端 TaktSopArgumentDto
 */
export interface SopArgument extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 执行追溯 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  execId?: string;

  /**
   * 工步执行明细 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  execStepId?: string;

  /**
   * 工序参数定义 ID（关联 TaktRoutingItemArgument，序列化为 string 以避免 Javascript 精度问题）
   */
  routingItemParameterId?: string;

  /**
   * 参数编码
   */
  paramCode?: string;

  /**
   * 是否超差（字典 sys_yes_no，0=否，1=是）
   */
  isOutOfRange?: number;

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
 * SopArgument 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopArgumentExport
 * @description 对应后端 TaktSopArgumentExportDto
 */
export interface SopArgumentExport {
  /**
   * SopArgumentID
   */
  sopArgumentId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 执行追溯 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  execId: string;

  /**
   * 工步执行明细 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  execStepId?: string;

  /**
   * 工序参数定义 ID（关联 TaktRoutingItemArgument，序列化为 string 以避免 Javascript 精度问题）
   */
  routingItemParameterId?: string;

  /**
   * 参数编码
   */
  paramCode: string;

  /**
   * 实际值
   */
  actualValue: number;

  /**
   * 是否超差（字典 sys_yes_no，0=否，1=是）
   */
  isOutOfRange: number;

  /**
   * 记录时间
   */
  recordedAt: string;

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

