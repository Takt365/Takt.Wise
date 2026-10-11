// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/maintenance
// 文件名称：equipment.types.ts
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
 * Takt工厂设备实体
 * 对应前端 TaktEquipmentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Equipment
 * @description 对应后端 TaktEquipmentDto
 */
export interface Equipment extends CompanyDtoBase {

  /**
   * 设备编码（唯一索引：租户+公司+工厂内唯一，见 ix_equipment_code_unique）
   */
  EquipCode?: string;

  /**
   * 设备名称
   */
  equipmentName?: string;

  /**
   * 登录设备（0=生产设备，1=检测设备，2=辅助设备，3=办公设备，4=其他设备）
   */
  equipmentType?: number;

  /**
   * 设备型号
   */
  equipmentModel?: string;

  /**
   * 设备规格
   */
  EquipSpecification?: string;

  /**
   * 设备品牌
   */
  EquipBrand?: string;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 经销商
   */
  dealerBy?: string;

  /**
   * 序列号/出厂编码
   */
  serialNumber?: string;

  /**
   * 所属车间
   */
  workshopBy?: string;

  /**
   * 所属产线
   */
  productionLineBy?: string;

  /**
   * 所属工位
   */
  workstationBy?: string;

  /**
   * 所属部门
   */
  deptBy?: string;

  /**
   * 设备位置（详细位置描述）
   */
  equipmentLocation?: string;

  /**
   * 负责人
   */
  responsibleUserBy?: string;

  /**
   * 操作人
   */
  operatorBy?: string;

  /**
   * 购买日期
   */
  purchaseDate?: string;

  /**
   * 安装日期
   */
  installationDate?: string;

  /**
   * 启用日期
   */
  startDate?: string;

  /**
   * 保修开始日期
   */
  warrantyStartDate?: string;

  /**
   * 保修结束日期
   */
  warrantyEndDate?: string;

  /**
   * 设备原值（精确到分，存储为整数，单位为分）
   */
  equipmentOriginalValue?: number;

  /**
   * 设备技术参数（JSON格式，存储设备技术参数配置）
   */
  technicalParameters?: string;

  /**
   * 设备图片（JSON格式，存储设备图片URL列表）
   */
  equipmentImages?: string;

  /**
   * 设备文档（JSON格式，存储设备文档ID列表）
   */
  equipmentDocuments?: string;

  /**
   * 是否关键设备（0=否，1=是）
   */
  isCritical?: number;

  /**
   * 保修状态（0=无保修，1=保修期内，2=保修期外，3=延保中）
   */
  warrantyStatus?: number;

  /**
   * 设备状态（字典 sys_equipment_status）
   */
  equipmentStatus?: number;

  /**
   * 维护通知单列表（子表，级联保存）
   */
  maintenanceNotifications?: MaintenanceNotificationCreate[];

  /**
   * 维护工单列表（子表，级联保存）
   */
  maintenanceWorkOrders?: MaintenanceWorkOrderCreate[];

  /**
   * 维护履历列表（由维护工单完工归档生成，只读）（子表，级联保存）
   */
  maintenanceHistories?: MaintenanceHistoryCreate[];

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
 * Equipment 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EquipmentExport
 * @description 对应后端 TaktEquipmentExportDto
 */
export interface EquipmentExport {
  /**
   * EquipmentID
   */
  equipmentId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（不可空）
   */
  plantCode: string;

  /**
   * 设备编码（唯一索引：租户+公司+工厂内唯一，见 ix_equipment_code_unique）
   */
  EquipCode: string;

  /**
   * 设备名称
   */
  equipmentName: string;

  /**
   * 登录设备（0=生产设备，1=检测设备，2=辅助设备，3=办公设备，4=其他设备）
   */
  equipmentType: number;

  /**
   * 设备型号
   */
  equipmentModel?: string;

  /**
   * 设备规格
   */
  EquipSpecification?: string;

  /**
   * 设备品牌
   */
  EquipBrand?: string;

  /**
   * 制造商
   */
  manufacturer?: string;

  /**
   * 经销商
   */
  dealerBy?: string;

  /**
   * 序列号/出厂编码
   */
  serialNumber?: string;

  /**
   * 所属车间
   */
  workshopBy?: string;

  /**
   * 所属产线
   */
  productionLineBy?: string;

  /**
   * 所属工位
   */
  workstationBy?: string;

  /**
   * 所属部门
   */
  deptBy?: string;

  /**
   * 设备位置（详细位置描述）
   */
  equipmentLocation?: string;

  /**
   * 负责人
   */
  responsibleUserBy?: string;

  /**
   * 操作人
   */
  operatorBy?: string;

  /**
   * 购买日期
   */
  purchaseDate?: string;

  /**
   * 安装日期
   */
  installationDate?: string;

  /**
   * 启用日期
   */
  startDate?: string;

  /**
   * 保修开始日期
   */
  warrantyStartDate?: string;

  /**
   * 保修结束日期
   */
  warrantyEndDate?: string;

  /**
   * 设备原值（精确到分，存储为整数，单位为分）
   */
  equipmentOriginalValue: number;

  /**
   * 设备技术参数（JSON格式，存储设备技术参数配置）
   */
  technicalParameters?: string;

  /**
   * 设备图片（JSON格式，存储设备图片URL列表）
   */
  equipmentImages?: string;

  /**
   * 设备文档（JSON格式，存储设备文档ID列表）
   */
  equipmentDocuments?: string;

  /**
   * 是否关键设备（0=否，1=是）
   */
  isCritical: number;

  /**
   * 保修状态（0=无保修，1=保修期内，2=保修期外，3=延保中）
   */
  warrantyStatus: number;

  /**
   * 设备状态（字典 sys_equipment_status）
   */
  equipmentStatus: number;

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

