// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：sopCall.types.ts
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
 * SOP 安灯呼叫实体
 * 对应前端 TaktSopCallDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopCall
 * @description 对应后端 TaktSopCallDto
 */
export interface SopCall extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 工位 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  workstationId?: string;

  /**
   * 执行追溯 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  execId?: string;

  /**
   * 呼叫类型（1=班长，2=维修，3=品质；字典 logistics_manufacturing_sop_andon_type）
   */
  callType?: number;

  /**
   * 呼叫人 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  callerId?: string;

  /**
   * 响应人 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  respondedBy?: string;

  /**
   * 响应时长（秒）
   */
  responseSeconds?: number;

  /**
   * 呼叫状态（1=待响应，2=已响应，3=已关闭；字典 logistics_manufacturing_sop_andon_status）
   */
  callStatus?: number;

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
 * SopCall 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopCallExport
 * @description 对应后端 TaktSopCallExportDto
 */
export interface SopCallExport {
  /**
   * SopCallID
   */
  sopCallId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工位 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  workstationId: string;

  /**
   * 执行追溯 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  execId?: string;

  /**
   * 呼叫类型（1=班长，2=维修，3=品质；字典 logistics_manufacturing_sop_andon_type）
   */
  callType: number;

  /**
   * 呼叫人 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  callerId: string;

  /**
   * 呼叫时间
   */
  calledAt: string;

  /**
   * 响应人 ID（序列化为 string 以避免 Javascript 精度问题）
   */
  respondedBy?: string;

  /**
   * 响应时间
   */
  respondedAt?: string;

  /**
   * 响应时长（秒）
   */
  responseSeconds?: number;

  /**
   * 呼叫状态（1=待响应，2=已响应，3=已关闭；字典 logistics_manufacturing_sop_andon_status）
   */
  callStatus: number;

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

