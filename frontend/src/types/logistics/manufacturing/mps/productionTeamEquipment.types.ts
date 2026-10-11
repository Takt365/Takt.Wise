// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/mps
// 文件名称：productionTeamEquipment.types.ts
// 创建时间：2026-07-24
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
 * 生产班组设备组明细（主子表；PCBA 线体生产设备及台数）
 * 对应前端 TaktProductionTeamEquipmentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ProductionTeamEquipment
 * @description 对应后端 TaktProductionTeamEquipmentDto
 */
export interface ProductionTeamEquipment extends CompanyDtoBase {

  /**
   * 生产班组主键（主子表关系，关联 TaktProductionTeam.Id）
   */
  prodTeamId?: string;

  /**
   * 班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）
   */
  teamCode?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 生产设备主键（关联 TaktProductionEquipment.Id）
   */
  prodEquipId?: string;

  /**
   * 生产设备编码（冗余快照，与 TaktProductionEquipment.ProdEquipCode 一致）
   */
  prodEquipCode?: string;

  /**
   * 设备台数（同型号多台时 &gt;1）
   */
  equipQuantity?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  teamEquipStatus?: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * ProductionTeamEquipment 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProductionTeamEquipmentExport
 * @description 对应后端 TaktProductionTeamEquipmentExportDto
 */
export interface ProductionTeamEquipmentExport {
  /**
   * ProductionTeamEquipmentID
   */
  productionTeamEquipmentId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 生产班组主键（主子表关系，关联 TaktProductionTeam.Id）
   */
  prodTeamId: string;

  /**
   * 班组编码（冗余快照，与 TaktProductionTeam.TeamCode 一致）
   */
  teamCode: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 生产设备主键（关联 TaktProductionEquipment.Id）
   */
  prodEquipId: string;

  /**
   * 生产设备编码（冗余快照，与 TaktProductionEquipment.ProdEquipCode 一致）
   */
  prodEquipCode: string;

  /**
   * 设备台数（同型号多台时 &gt;1）
   */
  equipQuantity: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  teamEquipStatus: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

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

