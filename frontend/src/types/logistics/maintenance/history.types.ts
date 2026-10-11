// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/maintenance
// 文件名称：history.types.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/maintenance 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 设备维护履历实体（TaktEquipment 子表；数据来源于 TaktMaintenanceWorkOrder 完工归档，只读展示）
 * 对应前端 TaktMaintenanceHistoryDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MaintenanceHistory
 * @description 对应后端 TaktMaintenanceHistoryDto
 */
export interface MaintenanceHistory extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 来源维护工单ID（一工单一条履历，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId?: string;

  /**
   * 来源维护工单号（冗余）
   */
  workOrderCode?: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId?: string;

  /**
   * 设备编码（冗余字段,便于查询）
   */
  EquipCode?: string;

  /**
   * 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType?: number;

  /**
   * 维护类别（字典 logistics_maintenance_category）
   */
  maintenanceCategory?: number;

  /**
   * 维护单位
   */
  maintenanceCompany?: string;

  /**
   * 维护技师（人员编码）
   */
  maintenanceTechnician?: string;

  /**
   * 维护日期（归档基准日，通常取工单完工时间）
   */
  maintenanceDate?: string;

  /**
   * 维护开始时间
   */
  maintenanceStartTime?: string;

  /**
   * 维护结束时间
   */
  maintenanceEndTime?: string;

  /**
   * 维护内容描述
   */
  maintenanceContent?: string;

  /**
   * 故障描述
   */
  faultDescription?: string;

  /**
   * 处理方案
   */
  solution?: string;

  /**
   * 使用配件（JSON，由工单领料明细汇总）
   */
  usedParts?: string;

  /**
   * 维护费用（工单总成本快照）
   */
  maintenanceCost?: number;

  /**
   * 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
   */
  maintenanceResult?: number;

  /**
   * 履历状态（固定为 2=已完成，归档写入）
   */
  maintenanceStatus?: number;

  /**
   * 下次维护日期
   */
  nextMaintenanceDate?: string;

  /**
   * 维护周期（天）
   */
  maintenanceCycleDays?: number;

  /**
   * 维护文档（JSON格式，存储维护文档ID列表）
   */
  maintenanceDocuments?: string;

  /**
   * 维护图片（JSON格式，存储维护图片URL列表）
   */
  maintenanceImages?: string;

  /**
   * 验收总结
   */
  acceptedSummary?: string;

  /**
   * 验收人（人员编码）
   */
  acceptedBy?: string;

  /**
   * 验收时间
   */
  acceptedAt?: string;

  /**
   * 归档时间
   */
  archivedAt?: string;

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
 * MaintenanceHistory 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaintenanceHistoryExport
 * @description 对应后端 TaktMaintenanceHistoryExportDto
 */
export interface MaintenanceHistoryExport {
  /**
   * MaintenanceHistoryID
   */
  maintenanceHistoryId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 来源维护工单ID（一工单一条履历，序列化为string以避免Javascript精度问题）
   */
  maintenanceWorkOrderId: string;

  /**
   * 来源维护工单号（冗余）
   */
  workOrderCode: string;

  /**
   * 设备ID（序列化为string以避免Javascript精度问题）
   */
  equipmentId: string;

  /**
   * 设备编码（冗余字段,便于查询）
   */
  EquipCode: string;

  /**
   * 维护类型（字典 logistics_maintenance_type；0=定期保养，1=故障维修，2=大修，3=改造升级，4=其他）
   */
  maintenanceType: number;

  /**
   * 维护类别（字典 logistics_maintenance_category）
   */
  maintenanceCategory: number;

  /**
   * 维护单位
   */
  maintenanceCompany?: string;

  /**
   * 维护技师（人员编码）
   */
  maintenanceTechnician?: string;

  /**
   * 维护日期（归档基准日，通常取工单完工时间）
   */
  maintenanceDate: string;

  /**
   * 维护开始时间
   */
  maintenanceStartTime?: string;

  /**
   * 维护结束时间
   */
  maintenanceEndTime?: string;

  /**
   * 维护内容描述
   */
  maintenanceContent?: string;

  /**
   * 故障描述
   */
  faultDescription?: string;

  /**
   * 处理方案
   */
  solution?: string;

  /**
   * 使用配件（JSON，由工单领料明细汇总）
   */
  usedParts?: string;

  /**
   * 维护费用（工单总成本快照）
   */
  maintenanceCost: number;

  /**
   * 维护结果（0=正常，1=待观察，2=需再次维修，3=已报废）
   */
  maintenanceResult: number;

  /**
   * 履历状态（固定为 2=已完成，归档写入）
   */
  maintenanceStatus: number;

  /**
   * 下次维护日期
   */
  nextMaintenanceDate?: string;

  /**
   * 维护周期（天）
   */
  maintenanceCycleDays: number;

  /**
   * 维护文档（JSON格式，存储维护文档ID列表）
   */
  maintenanceDocuments?: string;

  /**
   * 维护图片（JSON格式，存储维护图片URL列表）
   */
  maintenanceImages?: string;

  /**
   * 验收总结
   */
  acceptedSummary?: string;

  /**
   * 验收人（人员编码）
   */
  acceptedBy?: string;

  /**
   * 验收时间
   */
  acceptedAt?: string;

  /**
   * 归档时间
   */
  archivedAt: string;

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

