// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/labor-hour
// 文件名称：pcbaRepairLaborHour.types.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/labor-hour 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * PCBA改修工数统计实体
 * 对应前端 TaktPcbaRepairLaborHourDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PcbaRepairLaborHour
 * @description 对应后端 TaktPcbaRepairLaborHourDto
 */
export interface PcbaRepairLaborHour extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 生产日期
   */
  prodDate?: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode?: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 标准产能（统计：TaktPcbaOutput.StdCapacity 合计）
   */
  stdCapacity?: number;

  /**
   * 实际生产数量（统计：TaktPcbaOutputDetail.DailyCompletedQty 合计）
   */
  prodActualQty?: number;

  /**
   * 投入工时(分钟)（统计：TaktPcbaOutputDetail.InputMinutes 合计）
   */
  inputMinutes?: number;

  /**
   * 停线损失工时(分钟)（统计：TaktPcbaOutputDetail.DowntimeMinutes 合计）
   */
  downtimeMinutes?: number;

  /**
   * 报工工时(分钟)（统计：TaktPcbaOutputDetail.ConfirmMinutes 合计）
   */
  confirmMinutes?: number;

  /**
   * 实际工时(分钟)（统计：TaktPcbaOutputDetail.ActualMinutes 合计）
   */
  actualMinutes?: number;

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
 * PcbaRepairLaborHour 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PcbaRepairLaborHourExport
 * @description 对应后端 TaktPcbaRepairLaborHourExportDto
 */
export interface PcbaRepairLaborHourExport {
  /**
   * PcbaRepairLaborHourID
   */
  pcbaRepairLaborHourId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 生产日期
   */
  prodDate: string;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 标准产能（统计：TaktPcbaOutput.StdCapacity 合计）
   */
  stdCapacity: number;

  /**
   * 实际生产数量（统计：TaktPcbaOutputDetail.DailyCompletedQty 合计）
   */
  prodActualQty: number;

  /**
   * 投入工时(分钟)（统计：TaktPcbaOutputDetail.InputMinutes 合计）
   */
  inputMinutes: number;

  /**
   * 停线损失工时(分钟)（统计：TaktPcbaOutputDetail.DowntimeMinutes 合计）
   */
  downtimeMinutes: number;

  /**
   * 报工工时(分钟)（统计：TaktPcbaOutputDetail.ConfirmMinutes 合计）
   */
  confirmMinutes: number;

  /**
   * 实际工时(分钟)（统计：TaktPcbaOutputDetail.ActualMinutes 合计）
   */
  actualMinutes: number;

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

