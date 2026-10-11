// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/maintenance
// 文件名称：notification.types.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/maintenance 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 维护通知单实体（流程起点：发现异常 → 开通知单 → 转/建维护工单）。FlowInstanceId 由业务在发起流程后写入；流程引擎通过 BusinessKey/BusinessType 与本模块对接。
 * 对应前端 TaktMaintenanceNotificationDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 MaintenanceNotification
 * @description 对应后端 TaktMaintenanceNotificationDto
 */
export interface MaintenanceNotification extends ApprovalDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 工厂代码
   */
  plantCode?: string;

  /**
   * 通知单号
   */
  notificationCode?: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId?: string;

  /**
   * 设备编码（冗余，便于查询）
   */
  EquipCode?: string;

  /**
   * 设备名称（冗余）
   */
  equipmentName?: string;

  /**
   * 维护类别（字典 logistics_maintenance_category）
   */
  maintenanceCategory?: number;

  /**
   * 优先级（1=低，2=中，3=高，4=紧急）
   */
  priority?: number;

  /**
   * 通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）
   */
  notificationStatus?: number;

  /**
   * 异常/故障描述
   */
  faultDescription?: string;

  /**
   * 发现时间
   */
  discoveredAt?: string;

  /**
   * 故障开始时间
   */
  breakdownStartTime?: string;

  /**
   * 故障结束时间
   */
  breakdownEndTime?: string;

  /**
   * 报告人（人员编码）
   */
  reportedBy?: string;

  /**
   * 责任成本中心ID（序列化为string以避免Javascript精度问题）
   */
  costCenterId?: string;

  /**
   * 责任成本中心编码（冗余）
   */
  costCenterCode?: string;

  /**
   * 关联维护工单ID（转工单后回填，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId?: string;

  /**
   * 关联维护工单号（冗余）
   */
  maintenanceWorkOrderCode?: string;

  /**
   * 通知图片（JSON格式，存储图片URL列表）
   */
  notificationImages?: string;

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
 * MaintenanceNotification 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaintenanceNotificationExport
 * @description 对应后端 TaktMaintenanceNotificationExportDto
 */
export interface MaintenanceNotificationExport {
  /**
   * MaintenanceNotificationID
   */
  maintenanceNotificationId: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 通知单号
   */
  notificationCode: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId: string;

  /**
   * 设备编码（冗余，便于查询）
   */
  EquipCode: string;

  /**
   * 设备名称（冗余）
   */
  equipmentName: string;

  /**
   * 维护类别（字典 logistics_maintenance_category）
   */
  maintenanceCategory: number;

  /**
   * 优先级（1=低，2=中，3=高，4=紧急）
   */
  priority: number;

  /**
   * 通知单状态（0=新建，1=已转工单，2=已关闭，3=已取消）
   */
  notificationStatus: number;

  /**
   * 异常/故障描述
   */
  faultDescription: string;

  /**
   * 发现时间
   */
  discoveredAt: string;

  /**
   * 故障开始时间
   */
  breakdownStartTime?: string;

  /**
   * 故障结束时间
   */
  breakdownEndTime?: string;

  /**
   * 报告人（人员编码）
   */
  reportedBy?: string;

  /**
   * 责任成本中心ID（序列化为string以避免Javascript精度问题）
   */
  costCenterId?: string;

  /**
   * 责任成本中心编码（冗余）
   */
  costCenterCode?: string;

  /**
   * 关联维护工单ID（转工单后回填，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId?: string;

  /**
   * 关联维护工单号（冗余）
   */
  maintenanceWorkOrderCode?: string;

  /**
   * 通知图片（JSON格式，存储图片URL列表）
   */
  notificationImages?: string;

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

