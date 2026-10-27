// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecNotification.types.ts
// 创建时间：2026-06-30
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/engineering-change 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 工程变更通知单（技术阶段一 ④，隶属 TaktEcTe）。技术完成 ①主表 ②附件 ③明细 保存后由 TaktEcTeService 自动生成并派发； 各部门确认后在 TaktEcExec* 执行，技术通过看板/批次监控。FlowInstanceId 由通知审批流程写入（可选）。
 * 对应前端 TaktEcNotificationDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 EcNotification
 * @description 对应后端 TaktEcNotificationDto
 */
export interface EcNotification extends ApprovalDtoBase {

  /**
   * 通知单号（唯一，如：EC-2026-0001）
   */
  ecNotificationCode?: string;

  /**
   * 关联的设变主表ID（序列化为string以避免Javascript精度问题）
   */
  ecTeId?: string;

  /**
   * 设变单号（冗余字段，便于查询）
   */
  ecCode?: string;

  /**
   * 设变标题（冗余字段）
   */
  ecTitle?: string;

  /**
   * 通知日期
   */
  ecNotificationDate?: string;

  /**
   * 通知部门编码（多个部门用逗号分隔，如：Mfg1,PCBA,QC）
   */
  ecNotificationDeptCodes?: string;

  /**
   * 通知部门名称（多个部门用逗号分隔）
   */
  ecNotificationDeptNames?: string;

  /**
   * 通知组编码（选项 TaktEcGroups/options；DictValue=EcGroupCode，DictLabel=EcGroupName）
   */
  ecNotificationNotifierGroupCode?: string;

  /**
   * 通知组名称（冗余：按 EcNotificationNotifierGroupCode 取 TaktEcGroups.EcGroupName 联动）
   */
  ecNotificationNotifierGroupName?: string;

  /**
   * 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
   */
  ecNotificationMethod?: number;

  /**
   * 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
   */
  ecNotificationStatus?: number;

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
 * EcNotification 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcNotificationExport
 * @description 对应后端 TaktEcNotificationExportDto
 */
export interface EcNotificationExport {
  /**
   * EcNotificationID
   */
  ecNotificationId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 通知单号（唯一，如：EC-2026-0001）
   */
  ecNotificationCode: string;

  /**
   * 关联的设变主表ID（序列化为string以避免Javascript精度问题）
   */
  ecTeId: string;

  /**
   * 设变单号（冗余字段，便于查询）
   */
  ecCode: string;

  /**
   * 设变标题（冗余字段）
   */
  ecTitle?: string;

  /**
   * 通知日期
   */
  ecNotificationDate: string;

  /**
   * 通知部门编码（多个部门用逗号分隔，如：Mfg1,PCBA,QC）
   */
  ecNotificationDeptCodes?: string;

  /**
   * 通知部门名称（多个部门用逗号分隔）
   */
  ecNotificationDeptNames?: string;

  /**
   * 通知组编码（选项 TaktEcGroups/options；DictValue=EcGroupCode，DictLabel=EcGroupName）
   */
  ecNotificationNotifierGroupCode?: string;

  /**
   * 通知组名称（冗余：按 EcNotificationNotifierGroupCode 取 TaktEcGroups.EcGroupName 联动）
   */
  ecNotificationNotifierGroupName?: string;

  /**
   * 通知方式（1=系统通知 2=邮件 3=纸质 4=会议）
   */
  ecNotificationMethod: number;

  /**
   * 通知状态（0=待通知 1=已通知 2=已确认 3=已驳回 4=已过期）
   */
  ecNotificationStatus: number;

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

