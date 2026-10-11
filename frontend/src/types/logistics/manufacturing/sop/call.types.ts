// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：call.types.ts
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
 * SOP 安灯呼叫实体
 * 对应前端 TaktSopCallDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopCall
 * @description 对应后端 TaktSopCallDto
 */
export interface SopCall extends CompanyDtoBase {
  /**
   * SopCallID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sopCallId: string;

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
   * 呼叫类型（字典 logistics_manufacturing_sop_andon_type；1=班长，2=维修，3=品质）
   */
  callType: number;

  /**
   * 呼叫人 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  callerId: string;

  /**
   * 呼叫人 名称（填充字段）
   */
  callerName?: string;

  /**
   * 呼叫时间
   */
  calledAt: string;

  /**
   * 响应人 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  respondedBy?: string;

  /**
   * 响应时间
   */
  respondedAt?: string;

  /**
   * 响应时长（秒）
   */
  responseSeconds?: number;

  /**
   * 呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）
   */
  callStatus: number;

  /**
   * 工位 （主表：TaktSopWorkstation）
   */
  workstation?: SopWorkstation;

}


/**
 * SopCall 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SopCallQuery
 * @description 对应后端 TaktSopCallQueryDto
 */
export interface SopCallQuery extends TaktPagedQuery {
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
   * 呼叫类型（字典 logistics_manufacturing_sop_andon_type；1=班长，2=维修，3=品质）
   */
  callType?: number;

  /**
   * 呼叫人 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  callerId?: string;

  /**
   * 呼叫时间（范围查询-开始）
   */
  calledAtStart?: string;

  /**
   * 呼叫时间（范围查询-结束）
   */
  calledAtEnd?: string;

  /**
   * 响应人 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  respondedBy?: string;

  /**
   * 响应时间（范围查询-开始）
   */
  respondedAtStart?: string;

  /**
   * 响应时间（范围查询-结束）
   */
  respondedAtEnd?: string;

  /**
   * 响应时长（秒）
   */
  responseSeconds?: number;

  /**
   * 呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）
   */
  callStatus?: number;

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
 * 创建SopCall DTO
 * 对应前端 SopCallCreate
 * @description 对应后端 TaktSopCallCreateDto
 */
export interface SopCallCreate {
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
   * 呼叫类型（字典 logistics_manufacturing_sop_andon_type；1=班长，2=维修，3=品质）
   */
  callType: number;

  /**
   * 呼叫人 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  callerId: string;

  /**
   * 呼叫时间
   */
  calledAt: string;

  /**
   * 响应人 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  respondedBy?: string;

  /**
   * 响应时间
   */
  respondedAt?: string;

  /**
   * 响应时长（秒）
   */
  responseSeconds?: number;

  /**
   * 呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）
   */
  callStatus: number;

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
 * 更新SopCall DTO
 * 继承 TaktSopCallCreateDto，添加 SopCallId 字段
 * 对应前端 SopCallUpdate
 * @description 对应后端 TaktSopCallUpdateDto
 */
export interface SopCallUpdate extends SopCallCreate {
  /**
   * SopCallID（标识要更新的实体）
   */
  sopCallId: string;

}


/**
 * SopCall 状态更新 DTO
 * 对应前端 SopCallStatus
 * @description 对应后端 TaktSopCallStatusDto
 */
export interface SopCallStatus {
  /**
   * SopCallID
   */
  sopCallId: string;

  /**
   * 呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）
   */
  callStatus: number;

}


/**
 * SopCall 导入模板行 DTO
 * 对应前端 SopCallTemplate
 * @description 对应后端 TaktSopCallTemplateDto
 */
export interface SopCallTemplate {
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
   * 呼叫类型（字典 logistics_manufacturing_sop_andon_type；1=班长，2=维修，3=品质）
   */
  callType?: number;

  /**
   * 呼叫人 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  callerId?: string;

  /**
   * 呼叫时间
   */
  calledAt?: string;

  /**
   * 响应人 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  respondedBy?: string;

  /**
   * 响应时间
   */
  respondedAt?: string;

  /**
   * 响应时长（秒）
   */
  responseSeconds?: number;

  /**
   * 呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）
   */
  callStatus?: number;

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
 * SopCall 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SopCallImport
 * @description 对应后端 TaktSopCallImportDto
 */
export interface SopCallImport {
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
   * 呼叫类型（字典 logistics_manufacturing_sop_andon_type；1=班长，2=维修，3=品质）
   */
  callType?: number;

  /**
   * 呼叫人 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  callerId?: string;

  /**
   * 呼叫时间
   */
  calledAt?: string;

  /**
   * 响应人 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  respondedBy?: string;

  /**
   * 响应时间
   */
  respondedAt?: string;

  /**
   * 响应时长（秒）
   */
  responseSeconds?: number;

  /**
   * 呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）
   */
  callStatus?: number;

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
 * SopCall 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopCallExport
 * @description 对应后端 TaktSopCallExportDto
 */
export interface SopCallExport {
  /**
   * SopCallID
   */
  sopCallId: string;

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
   * 呼叫类型（字典 logistics_manufacturing_sop_andon_type；1=班长，2=维修，3=品质）
   */
  callType: number;

  /**
   * 呼叫人 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  callerId: string;

  /**
   * 呼叫时间
   */
  calledAt: string;

  /**
   * 响应人 ID（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  respondedBy?: string;

  /**
   * 响应时间
   */
  respondedAt?: string;

  /**
   * 响应时长（秒）
   */
  responseSeconds?: number;

  /**
   * 呼叫状态（字典 logistics_manufacturing_sop_andon_status；1=待响应，2=已响应，3=已关闭）
   */
  callStatus: number;

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

