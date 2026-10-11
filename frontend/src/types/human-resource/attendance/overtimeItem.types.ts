// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/attendance
// 文件名称：overtimeItem.types.ts
// 创建时间：2026-07-09
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
 * 加班申请明细（一次申请可包含多个人员）
 * 对应前端 TaktOvertimeItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 OvertimeItem
 * @description 对应后端 TaktOvertimeItemDto
 */
export interface OvertimeItem extends CompanyDtoBase {
  /**
   * OvertimeItemID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  overtimeItemId: string;

  /**
   * 加班申请单（关联 TaktOvertime.Id，主子表关系）
   */
  overtimeId: string;

  /**
   * 加班申请单（关联 TaktOvertime.Id，主子表关系）
   */
  overtimeName?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 计划加班小时数
   */
  plannedHours: number;

  /**
   * 实际加班开始时间
   */
  actualStartTime?: string;

  /**
   * 实际加班结束时间
   */
  actualEndTime?: string;

  /**
   * 实际加班小时数
   */
  actualHours?: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 加班主表 （主表：TaktOvertime）
   */
  overtime?: Overtime;

}


/**
 * OvertimeItem 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 OvertimeItemQuery
 * @description 对应后端 TaktOvertimeItemQueryDto
 */
export interface OvertimeItemQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  /**
   * 加班申请单（关联 TaktOvertime.Id，主子表关系）
   */
  overtimeId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  employeeId?: string;

  /**
   * 员工姓名
   */
  employeeName?: string;

  /**
   * 计划加班小时数
   */
  plannedHours?: number;

  /**
   * 实际加班开始时间（范围查询-开始）
   */
  actualStartTimeStart?: string;

  /**
   * 实际加班开始时间（范围查询-结束）
   */
  actualStartTimeEnd?: string;

  /**
   * 实际加班结束时间（范围查询-开始）
   */
  actualEndTimeStart?: string;

  /**
   * 实际加班结束时间（范围查询-结束）
   */
  actualEndTimeEnd?: string;

  /**
   * 实际加班小时数
   */
  actualHours?: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

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
 * 创建OvertimeItem DTO
 * 对应前端 OvertimeItemCreate
 * @description 对应后端 TaktOvertimeItemCreateDto
 */
export interface OvertimeItemCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 加班申请单（关联 TaktOvertime.Id，主子表关系）
   */
  overtimeId: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 计划加班小时数
   */
  plannedHours: number;

  /**
   * 实际加班开始时间
   */
  actualStartTime?: string;

  /**
   * 实际加班结束时间
   */
  actualEndTime?: string;

  /**
   * 实际加班小时数
   */
  actualHours?: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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

}


/**
 * 更新OvertimeItem DTO
 * 继承 TaktOvertimeItemCreateDto，添加 OvertimeItemId 字段
 * 对应前端 OvertimeItemUpdate
 * @description 对应后端 TaktOvertimeItemUpdateDto
 */
export interface OvertimeItemUpdate extends OvertimeItemCreate {
  /**
   * OvertimeItemID（标识要更新的实体）
   */
  overtimeItemId: string;

}


/**
 * OvertimeItem 作废/撤销作废 DTO
 * 对应前端 OvertimeItemObsolete
 * @description 对应后端 TaktOvertimeItemObsoleteDto
 */
export interface OvertimeItemObsolete {
  /**
   * OvertimeItemID
   */
  overtimeItemId: string;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

}


/**
 * OvertimeItem 导入模板行 DTO
 * 对应前端 OvertimeItemTemplate
 * @description 对应后端 TaktOvertimeItemTemplateDto
 */
export interface OvertimeItemTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  /**
   * 加班申请单（关联 TaktOvertime.Id，主子表关系）
   */
  overtimeId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  employeeId?: string;

  /**
   * 员工姓名
   */
  employeeName?: string;

  /**
   * 计划加班小时数
   */
  plannedHours?: number;

  /**
   * 实际加班开始时间
   */
  actualStartTime?: string;

  /**
   * 实际加班结束时间
   */
  actualEndTime?: string;

  /**
   * 实际加班小时数
   */
  actualHours?: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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
 * OvertimeItem 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 OvertimeItemImport
 * @description 对应后端 TaktOvertimeItemImportDto
 */
export interface OvertimeItemImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
   */
  plantCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 加班申请单（关联 TaktOvertime.Id，主子表关系）
   */
  overtimeId?: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber?: number;

  /**
   * 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  employeeId?: string;

  /**
   * 员工姓名
   */
  employeeName?: string;

  /**
   * 计划加班小时数
   */
  plannedHours?: number;

  /**
   * 实际加班开始时间
   */
  actualStartTime?: string;

  /**
   * 实际加班结束时间
   */
  actualEndTime?: string;

  /**
   * 实际加班小时数
   */
  actualHours?: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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
 * OvertimeItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 OvertimeItemExport
 * @description 对应后端 TaktOvertimeItemExportDto
 */
export interface OvertimeItemExport {
  /**
   * OvertimeItemID
   */
  overtimeItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 加班申请单（关联 TaktOvertime.Id，主子表关系）
   */
  overtimeId: string;

  /**
   * 行号（项号/序号，固定步长=10）
   */
  lineNumber: number;

  /**
   * 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 计划加班小时数
   */
  plannedHours: number;

  /**
   * 实际加班开始时间
   */
  actualStartTime?: string;

  /**
   * 实际加班结束时间
   */
  actualEndTime?: string;

  /**
   * 实际加班小时数
   */
  actualHours?: number;

  /**
   * 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
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

