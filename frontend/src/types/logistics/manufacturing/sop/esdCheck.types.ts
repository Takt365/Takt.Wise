// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：esdCheck.types.ts
// 创建时间：2026-06-30
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
 * SOP ESD 检查实体
 * 对应前端 TaktSopEsdCheckDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopEsdCheck
 * @description 对应后端 TaktSopEsdCheckDto
 */
export interface SopEsdCheck extends CompanyDtoBase {
  /**
   * SopEsdCheckID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sopEsdCheckId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 工位 ID（关联 TaktSopWorkstation.Id，选项 TaktSopWorkstations/options）
   */
  workstationId: string;

  /**
   * 工位 名称（填充字段）
   */
  workstationName?: string;

  /**
   * 执行追溯 ID（关联 TaktSopExec.Id，选项 TaktSopExecs/options）
   */
  execId?: string;

  /**
   * 执行追溯 名称（填充字段）
   */
  execName?: string;

  /**
   * 员工 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  employeeId: string;

  /**
   * 员工 名称（填充字段）
   */
  employeeName?: string;

  /**
   * 监测设备编码
   */
  deviceCode?: string;

  /**
   * 阻值（兆欧）
   */
  resistanceValue?: number;

  /**
   * 达标（字典 sys_yes_no；0=否，1=是）
   */
  isCompliant: number;

  /**
   * 锁屏（字典 sys_yes_no；0=否，1=是）
   */
  lockScreenTriggered: number;

  /**
   * 检查时间
   */
  checkedAt: string;

  /**
   * 工位 （主表：TaktSopWorkstation）
   */
  workstation?: SopWorkstation;

}


/**
 * SopEsdCheck 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SopEsdCheckQuery
 * @description 对应后端 TaktSopEsdCheckQueryDto
 */
export interface SopEsdCheckQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 工位 ID（关联 TaktSopWorkstation.Id，选项 TaktSopWorkstations/options）
   */
  workstationId?: string;

  /**
   * 执行追溯 ID（关联 TaktSopExec.Id，选项 TaktSopExecs/options）
   */
  execId?: string;

  /**
   * 员工 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  employeeId?: string;

  /**
   * 监测设备编码
   */
  deviceCode?: string;

  /**
   * 阻值（兆欧）
   */
  resistanceValue?: number;

  /**
   * 达标（字典 sys_yes_no；0=否，1=是）
   */
  isCompliant?: number;

  /**
   * 锁屏（字典 sys_yes_no；0=否，1=是）
   */
  lockScreenTriggered?: number;

  /**
   * 检查时间（范围查询-开始）
   */
  checkedAtStart?: string;

  /**
   * 检查时间（范围查询-结束）
   */
  checkedAtEnd?: string;

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
 * 创建SopEsdCheck DTO
 * 对应前端 SopEsdCheckCreate
 * @description 对应后端 TaktSopEsdCheckCreateDto
 */
export interface SopEsdCheckCreate {
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
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 工位 ID（关联 TaktSopWorkstation.Id，选项 TaktSopWorkstations/options）
   */
  workstationId: string;

  /**
   * 执行追溯 ID（关联 TaktSopExec.Id，选项 TaktSopExecs/options）
   */
  execId?: string;

  /**
   * 员工 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  employeeId: string;

  /**
   * 监测设备编码
   */
  deviceCode?: string;

  /**
   * 阻值（兆欧）
   */
  resistanceValue?: number;

  /**
   * 达标（字典 sys_yes_no；0=否，1=是）
   */
  isCompliant: number;

  /**
   * 锁屏（字典 sys_yes_no；0=否，1=是）
   */
  lockScreenTriggered: number;

  /**
   * 检查时间
   */
  checkedAt: string;

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
 * 更新SopEsdCheck DTO
 * 继承 TaktSopEsdCheckCreateDto，添加 SopEsdCheckId 字段
 * 对应前端 SopEsdCheckUpdate
 * @description 对应后端 TaktSopEsdCheckUpdateDto
 */
export interface SopEsdCheckUpdate extends SopEsdCheckCreate {
  /**
   * SopEsdCheckID（标识要更新的实体）
   */
  sopEsdCheckId: string;

}


/**
 * SopEsdCheck 导入模板行 DTO
 * 对应前端 SopEsdCheckTemplate
 * @description 对应后端 TaktSopEsdCheckTemplateDto
 */
export interface SopEsdCheckTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 工位 ID（关联 TaktSopWorkstation.Id，选项 TaktSopWorkstations/options）
   */
  workstationId?: string;

  /**
   * 执行追溯 ID（关联 TaktSopExec.Id，选项 TaktSopExecs/options）
   */
  execId?: string;

  /**
   * 员工 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  employeeId?: string;

  /**
   * 监测设备编码
   */
  deviceCode?: string;

  /**
   * 阻值（兆欧）
   */
  resistanceValue?: number;

  /**
   * 达标（字典 sys_yes_no；0=否，1=是）
   */
  isCompliant?: number;

  /**
   * 锁屏（字典 sys_yes_no；0=否，1=是）
   */
  lockScreenTriggered?: number;

  /**
   * 检查时间
   */
  checkedAt?: string;

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
 * SopEsdCheck 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SopEsdCheckImport
 * @description 对应后端 TaktSopEsdCheckImportDto
 */
export interface SopEsdCheckImport {
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
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 工位 ID（关联 TaktSopWorkstation.Id，选项 TaktSopWorkstations/options）
   */
  workstationId?: string;

  /**
   * 执行追溯 ID（关联 TaktSopExec.Id，选项 TaktSopExecs/options）
   */
  execId?: string;

  /**
   * 员工 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  employeeId?: string;

  /**
   * 监测设备编码
   */
  deviceCode?: string;

  /**
   * 阻值（兆欧）
   */
  resistanceValue?: number;

  /**
   * 达标（字典 sys_yes_no；0=否，1=是）
   */
  isCompliant?: number;

  /**
   * 锁屏（字典 sys_yes_no；0=否，1=是）
   */
  lockScreenTriggered?: number;

  /**
   * 检查时间
   */
  checkedAt?: string;

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
 * SopEsdCheck 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopEsdCheckExport
 * @description 对应后端 TaktSopEsdCheckExportDto
 */
export interface SopEsdCheckExport {
  /**
   * SopEsdCheckID
   */
  sopEsdCheckId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 工位 ID（关联 TaktSopWorkstation.Id，选项 TaktSopWorkstations/options）
   */
  workstationId: string;

  /**
   * 执行追溯 ID（关联 TaktSopExec.Id，选项 TaktSopExecs/options）
   */
  execId?: string;

  /**
   * 员工 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  employeeId: string;

  /**
   * 监测设备编码
   */
  deviceCode?: string;

  /**
   * 阻值（兆欧）
   */
  resistanceValue?: number;

  /**
   * 达标（字典 sys_yes_no；0=否，1=是）
   */
  isCompliant: number;

  /**
   * 锁屏（字典 sys_yes_no；0=否，1=是）
   */
  lockScreenTriggered: number;

  /**
   * 检查时间
   */
  checkedAt: string;

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

