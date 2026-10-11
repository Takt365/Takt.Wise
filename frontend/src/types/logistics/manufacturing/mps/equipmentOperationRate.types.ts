// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/mps
// 文件名称：equipmentOperationRate.types.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mps 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 机器稼动率实体（生产设备运行效率记录） 时间稼动率(%) = 稼动时间 ÷ 负荷时间 × 100%；为 OEE（设备综合效率）基础之一。
 * 对应前端 TaktEquipmentOperationRateDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EquipmentOperationRate
 * @description 对应后端 TaktEquipmentOperationRateDto
 */
export interface EquipmentOperationRate extends CompanyDtoBase {

  /**
   * 时间类别（1=天，2=周，3=月）
   */
  timeCategory?: number;

  /**
   * 开始日期
   */
  startDate?: string;

  /**
   * 结束日期
   */
  endDate?: string;

  /**
   * 周数（1-53）
   */
  weekNumber?: number;

  /**
   * 月份（1-12）
   */
  monthNumber?: number;

  /**
   * 设备编码（关联 TaktProductionEquipment.ProdEquipCode，选项 TaktProductionEquipments/options）
   */
  EquipCode?: string;

  /**
   * 设备名称
   */
  equipmentName?: string;

  /**
   * 登录设备（字典 logistics_maintenance_equipment_type；0=生产设备 1=检测设备 2=包装设备 3=物流设备 4=辅助设备）
   */
  equipmentType?: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode?: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。
   */
  plannedRuntime?: number;

  /**
   * 稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。
   */
  actualRuntime?: number;

  /**
   * 停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。
   */
  downtime?: number;

  /**
   * 时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。
   */
  equipmentOperationRate?: number;

  /**
   * 计划产量
   */
  plannedOutput?: number;

  /**
   * 实际产量
   */
  actualOutput?: number;

  /**
   * 合格品数量
   */
  qualifiedQuantity?: number;

  /**
   * 不良品数量
   */
  defectiveQuantity?: number;

  /**
   * 良品率（%）
   */
  yieldRate?: number;

  /**
   * 停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）
   */
  downtimeReasonType?: number;

  /**
   * 停机原因描述（自由文本，与 DowntimeReasonType 配合）
   */
  downtimeReason?: string;

  /**
   * 设备操作员（选项 TaktEmployees/options，存员工姓名或工号）
   */
  equipmentOperator?: string;

  /**
   * 设备维护员（选项 TaktEmployees/options，存员工姓名或工号）
   */
  equipmentMaintainer?: string;

  /**
   * 班组长（选项 TaktEmployees/options，存员工姓名或工号）
   */
  teamLeader?: string;

  /**
   * 状态（0=正常，1=停用）
   */
  rateStatus?: number;

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
 * EquipmentOperationRate 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EquipmentOperationRateExport
 * @description 对应后端 TaktEquipmentOperationRateExportDto
 */
export interface EquipmentOperationRateExport {
  /**
   * EquipmentOperationRateID
   */
  equipmentOperationRateId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode: string;

  /**
   * 时间类别（1=天，2=周，3=月）
   */
  timeCategory: number;

  /**
   * 开始日期
   */
  startDate: string;

  /**
   * 结束日期
   */
  endDate: string;

  /**
   * 周数（1-53）
   */
  weekNumber?: number;

  /**
   * 月份（1-12）
   */
  monthNumber?: number;

  /**
   * 设备编码（关联 TaktProductionEquipment.ProdEquipCode，选项 TaktProductionEquipments/options）
   */
  EquipCode: string;

  /**
   * 设备名称
   */
  equipmentName: string;

  /**
   * 登录设备（字典 logistics_maintenance_equipment_type；0=生产设备 1=检测设备 2=包装设备 3=物流设备 4=辅助设备）
   */
  equipmentType: number;

  /**
   * 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 过滤）
   */
  TeamCode?: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 负荷时间（分钟）。设备在计划内应运行的总时间，即 计划作业时间 - 计划停机时间。
   */
  plannedRuntime: number;

  /**
   * 稼动时间（分钟）。设备实际用于生产的时间，即 负荷时间 - 停线损失时间。
   */
  actualRuntime: number;

  /**
   * 停线损失时间（分钟）。换模/换线、故障、品质异常、缺料等导致的停机。
   */
  downtime: number;

  /**
   * 时间稼动率（%）。计算公式：稼动时间 ÷ 负荷时间 × 100%。
   */
  equipmentOperationRate: number;

  /**
   * 计划产量
   */
  plannedOutput: number;

  /**
   * 实际产量
   */
  actualOutput: number;

  /**
   * 合格品数量
   */
  qualifiedQuantity: number;

  /**
   * 不良品数量
   */
  defectiveQuantity: number;

  /**
   * 良品率（%）
   */
  yieldRate: number;

  /**
   * 停机原因类型（1=设备故障，2=换型调试，3=缺料，4=人员不足，5=其他）
   */
  downtimeReasonType?: number;

  /**
   * 停机原因描述（自由文本，与 DowntimeReasonType 配合）
   */
  downtimeReason?: string;

  /**
   * 设备操作员（选项 TaktEmployees/options，存员工姓名或工号）
   */
  equipmentOperator?: string;

  /**
   * 设备维护员（选项 TaktEmployees/options，存员工姓名或工号）
   */
  equipmentMaintainer?: string;

  /**
   * 班组长（选项 TaktEmployees/options，存员工姓名或工号）
   */
  teamLeader?: string;

  /**
   * 状态（0=正常，1=停用）
   */
  rateStatus: number;

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

