// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/workflow
// 文件名称：flowAddSign.types.ts
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
 * 流程加签记录实体
 * 对应前端 TaktFlowAddSignDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 FlowAddSign
 * @description 对应后端 TaktFlowAddSignDto
 */
export interface FlowAddSign extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 流程实例 ID
   */
  instanceId?: string;

  /**
   * 加签节点 ID
   */
  nodeId?: string;

  /**
   * 加签人 ID
   */
  signUserId?: string;

  /**
   * 加签人姓名
   */
  signUserName?: string;

  /**
   * 加签方式（sequential / all / one，与前端 approveType 一致）
   */
  signType?: string;

  /**
   * 完成后是否回到加签节点
   */
  returnToSignNode?: number;

  /**
   * 加签原因
   */
  reason?: string;

  /**
   * 是否已处理（含减签）
   */
  isHandled?: number;

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
 * FlowAddSign 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FlowAddSignExport
 * @description 对应后端 TaktFlowAddSignExportDto
 */
export interface FlowAddSignExport {
  /**
   * FlowAddSignID
   */
  flowAddSignId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 流程实例 ID
   */
  instanceId: string;

  /**
   * 加签节点 ID
   */
  nodeId: string;

  /**
   * 加签人 ID
   */
  signUserId: string;

  /**
   * 加签人姓名
   */
  signUserName?: string;

  /**
   * 加签方式（sequential / all / one，与前端 approveType 一致）
   */
  signType: string;

  /**
   * 完成后是否回到加签节点
   */
  returnToSignNode: number;

  /**
   * 加签原因
   */
  reason?: string;

  /**
   * 是否已处理（含减签）
   */
  isHandled: number;

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

