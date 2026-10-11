// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/attendance
// 文件名称：workShift.types.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/attendance 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 班次定义（如早班、中班、夜班）
 * 对应前端 TaktWorkShiftDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 WorkShift
 * @description 对应后端 TaktWorkShiftDto
 */
export interface WorkShift extends CompanyDtoBase {

}


/**
 * WorkShift 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 WorkShiftQuery
 * @description 对应后端 TaktWorkShiftQueryDto
 */
export interface WorkShiftQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 班次编码（租户+公司内唯一）
   */
  shiftCode?: string;

  /**
   * 班次名称
   */
  shiftName?: string;

  /**
   * 当班开始时间（HH:mm）
   */
  startTime?: string;

  /**
   * 当班结束时间（HH:mm）
   */
  endTime?: string;

  /**
   * 是否跨自然日（字典 sys_yes_no；0=否 1=是）
   */
  crossMidnight?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}


/**
 * 创建WorkShift DTO
 * 对应前端 WorkShiftCreate
 * @description 对应后端 TaktWorkShiftCreateDto
 */
export interface WorkShiftCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 班次编码（租户+公司内唯一）
   */
  shiftCode: string;

  /**
   * 班次名称
   */
  shiftName: string;

  /**
   * 当班开始时间（HH:mm）
   */
  startTime: string;

  /**
   * 当班结束时间（HH:mm）
   */
  endTime: string;

  /**
   * 是否跨自然日（字典 sys_yes_no；0=否 1=是）
   */
  crossMidnight: number;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

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
 * 更新WorkShift DTO
 * 继承 TaktWorkShiftCreateDto，添加 WorkShiftId 字段
 * 对应前端 WorkShiftUpdate
 * @description 对应后端 TaktWorkShiftUpdateDto
 */
export interface WorkShiftUpdate extends WorkShiftCreate {
  /**
   * WorkShiftID（标识要更新的实体）
   */
  workShiftId: string;

}


/**
 * WorkShift 排序更新 DTO
 * 对应前端 WorkShiftSort
 * @description 对应后端 TaktWorkShiftSortDto
 */
export interface WorkShiftSort {
  /**
   * WorkShiftID
   */
  workShiftId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}


/**
 * WorkShift 导入模板行 DTO
 * 对应前端 WorkShiftTemplate
 * @description 对应后端 TaktWorkShiftTemplateDto
 */
export interface WorkShiftTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 班次编码（租户+公司内唯一）
   */
  shiftCode?: string;

  /**
   * 班次名称
   */
  shiftName?: string;

  /**
   * 当班开始时间（HH:mm）
   */
  startTime?: string;

  /**
   * 当班结束时间（HH:mm）
   */
  endTime?: string;

  /**
   * 是否跨自然日（字典 sys_yes_no；0=否 1=是）
   */
  crossMidnight?: number;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

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
 * WorkShift 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 WorkShiftImport
 * @description 对应后端 TaktWorkShiftImportDto
 */
export interface WorkShiftImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 班次编码（租户+公司内唯一）
   */
  shiftCode?: string;

  /**
   * 班次名称
   */
  shiftName?: string;

  /**
   * 当班开始时间（HH:mm）
   */
  startTime?: string;

  /**
   * 当班结束时间（HH:mm）
   */
  endTime?: string;

  /**
   * 是否跨自然日（字典 sys_yes_no；0=否 1=是）
   */
  crossMidnight?: number;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

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
 * WorkShift 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 WorkShiftExport
 * @description 对应后端 TaktWorkShiftExportDto
 */
export interface WorkShiftExport {
  /**
   * WorkShiftID
   */
  workShiftId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 班次编码（租户+公司内唯一）
   */
  shiftCode: string;

  /**
   * 班次名称
   */
  shiftName: string;

  /**
   * 当班开始时间（HH:mm）
   */
  startTime: string;

  /**
   * 当班结束时间（HH:mm）
   */
  endTime: string;

  /**
   * 是否跨自然日（字典 sys_yes_no；0=否 1=是）
   */
  crossMidnight: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

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

