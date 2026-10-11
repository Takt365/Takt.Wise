// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：sopWorkstation.types.ts
// 创建时间：2026-06-15
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/sop 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * SOP 工位主数据实体
 * 对应前端 TaktSopWorkstationDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopWorkstation
 * @description 对应后端 TaktSopWorkstationDto
 */
export interface SopWorkstation extends CompanyDtoBase {

  /**
   * 工位编码（工厂内唯一）
   */
  workstationCode?: string;

  /**
   * 工位名称
   */
  workstationName?: string;

  /**
   * 工作中心
   */
  workCenter?: string;

  /**
   * 生产线
   */
  productionLine?: string;

  /**
   * 工位类型（1=装配，2=检验，3=包装，4=测试，5=其他；字典 sys_workstation_type）
   */
  workstationType?: number;

  /**
   * 工艺段类型（1=SMT，2=自插，3=手插，4=修正，5=总装；字典 logistics_manufacturing_process_segment_type）
   */
  processSegmentType?: number;

  /**
   * 启用状态（字典 sys_normal_disable，0=停用，1=启用）
   */
  workstationStatus?: number;

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
 * SopWorkstation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopWorkstationExport
 * @description 对应后端 TaktSopWorkstationExportDto
 */
export interface SopWorkstationExport {
  /**
   * SopWorkstationID
   */
  sopWorkstationId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码
   */
  plantCode: string;

  /**
   * 工位编码（工厂内唯一）
   */
  workstationCode: string;

  /**
   * 工位名称
   */
  workstationName: string;

  /**
   * 工作中心
   */
  workCenter?: string;

  /**
   * 生产线
   */
  productionLine?: string;

  /**
   * 工位类型（1=装配，2=检验，3=包装，4=测试，5=其他；字典 sys_workstation_type）
   */
  workstationType: number;

  /**
   * 工艺段类型（1=SMT，2=自插，3=手插，4=修正，5=总装；字典 logistics_manufacturing_process_segment_type）
   */
  processSegmentType: number;

  /**
   * 启用状态（字典 sys_normal_disable，0=停用，1=启用）
   */
  workstationStatus: number;

  /**
   * 排序号
   */
  sortOrder: number;

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

