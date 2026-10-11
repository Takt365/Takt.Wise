// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/workflow
// 文件名称：flowInstance.types.ts
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
 * 流程实例实体
 * 对应前端 TaktFlowInstanceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 FlowInstance
 * @description 对应后端 TaktFlowInstanceDto
 */
export interface FlowInstance extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 实例编码（对外业务单号）
   */
  instanceCode?: string;

  /**
   * 流程定义 ID（TaktFlowScheme Id）
   */
  processDefinitionId?: string;

  /**
   * 流程键（冗余）
   */
  processKey?: string;

  /**
   * 流程名称（冗余）
   */
  processName?: string;

  /**
   * 发起时锁定的定义版本号
   */
  definitionVersion?: number;

  /**
   * 申请标题
   */
  processTitle?: string;

  /**
   * 实例状态
   */
  instanceStatus?: number;

  /**
   * 当前节点 ID（设计器 nodeId）
   */
  currentActivityId?: string;

  /**
   * 当前节点名称
   */
  currentActivityName?: string;

  /**
   * 发起人 ID
   */
  startUserId?: string;

  /**
   * 发起人姓名
   */
  startUserName?: string;

  /**
   * 历时毫秒
   */
  durationMs?: string;

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
 * FlowInstance 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FlowInstanceExport
 * @description 对应后端 TaktFlowInstanceExportDto
 */
export interface FlowInstanceExport {
  /**
   * FlowInstanceID
   */
  flowInstanceId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 实例编码（对外业务单号）
   */
  instanceCode: string;

  /**
   * 流程定义 ID（TaktFlowScheme Id）
   */
  processDefinitionId: string;

  /**
   * 流程键（冗余）
   */
  processKey: string;

  /**
   * 流程名称（冗余）
   */
  processName: string;

  /**
   * 发起时锁定的定义版本号
   */
  definitionVersion: number;

  /**
   * 申请标题
   */
  processTitle?: string;

  /**
   * 实例状态
   */
  instanceStatus: number;

  /**
   * 当前节点 ID（设计器 nodeId）
   */
  currentActivityId?: string;

  /**
   * 当前节点名称
   */
  currentActivityName?: string;

  /**
   * 发起人 ID
   */
  startUserId: string;

  /**
   * 发起人姓名
   */
  startUserName?: string;

  /**
   * 开始时间
   */
  startTime?: string;

  /**
   * 结束时间
   */
  endTime?: string;

  /**
   * 历时毫秒
   */
  durationMs?: string;

  /**
   * 业务主键（关联业务单据 Id 等）
   */
  businessKey?: string;

  /**
   * 业务类型（由业务模块约定，用于回写）
   */
  businessType?: string;

  /**
   * 父流程实例 ID（子流程场景）
   */
  superInstanceId?: string;

  /**
   * 终止原因
   */
  deleteReason?: string;

  /**
   * 表单数据 JSON（前端 frmData；细粒度字段可同步至 TaktFlowVariable）
   */
  frmData?: string;

  /**
   * 关联表单 ID
   */
  formId?: string;

  /**
   * 关联表单编码
   */
  formCode?: string;

  /**
   * 流程设计快照（启动时复制，避免定义变更影响在途实例）
   */
  processContentSnapshot?: string;

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

