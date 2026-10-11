// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/mps
// 文件名称：productionTeam.types.ts
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
 * 生产班组实体（生产线班组主数据）
 * 对应前端 TaktProductionTeamDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ProductionTeam
 * @description 对应后端 TaktProductionTeamDto
 */
export interface ProductionTeam extends CompanyDtoBase {

  /**
   * 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
   */
  teamCode?: string;

  /**
   * 班组名称（显示名称，如：SMT一班、手插二班等）
   */
  teamName?: string;

  /**
   * 班组分类（字典 logistics_manufacturing_team_category；存 DictValue；A=组立 P=PCBA Q=质检 O=其他；PCBA 线体如 SMT/AI/手插须维护设备组）
   */
  teamCategory?: string;

  /**
   * 班组长姓名（选项 TaktEmployees/options，存员工姓名或工号）
   */
  teamLeaderName?: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo?: number;

  /**
   * 启用状态（字典 sys_normal_disable；0=禁用，1=启用）
   */
  teamStatus?: number;

  /**
   * 设备组明细（PCBA 线体 SMT/AI/手插等生产设备及台数）（子表，级联保存）
   */
  teamEquipmentList?: ProductionTeamEquipmentCreate[];

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
 * ProductionTeam 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ProductionTeamExport
 * @description 对应后端 TaktProductionTeamExportDto
 */
export interface ProductionTeamExport {
  /**
   * ProductionTeamID
   */
  productionTeamId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=Id）
   */
  plantCode: string;

  /**
   * 班组编码（唯一标识，例如：1、1SMT1、1SMT2、2自插A 等）
   */
  teamCode: string;

  /**
   * 班组名称（显示名称，如：SMT一班、手插二班等）
   */
  teamName: string;

  /**
   * 班组分类（字典 logistics_manufacturing_team_category；存 DictValue；A=组立 P=PCBA Q=质检 O=其他；PCBA 线体如 SMT/AI/手插须维护设备组）
   */
  teamCategory: string;

  /**
   * 班组长姓名（选项 TaktEmployees/options，存员工姓名或工号）
   */
  teamLeaderName?: string;

  /**
   * 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班）
   */
  shiftNo: number;

  /**
   * 启用状态（字典 sys_normal_disable；0=禁用，1=启用）
   */
  teamStatus: number;

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

