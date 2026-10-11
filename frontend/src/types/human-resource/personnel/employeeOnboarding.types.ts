// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employeeOnboarding.types.ts
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/personnel 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 入职待办（办理待办单，非审批单；状态见 TodoStatus）
 * 对应前端 TaktEmployeeOnboardingDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmployeeOnboarding
 * @description 对应后端 TaktEmployeeOnboardingDto
 */
export interface EmployeeOnboarding extends CompanyDtoBase {
  /**
   * EmployeeOnboardingID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  employeeOnboardingId: string;

  /**
   * 录用信息（选项 TaktTalentOffers/options；DictValue=Id）
   */
  offerId: string;

  /**
   * 录用信息（选项 TaktTalentOffers/options；DictValue=Id）
   */
  offerName?: string;

  /**
   * 待办单号（租户+公司内业务编码）
   */
  todoCode: string;

  /**
   * 计划上岗日期（JoinedDate 计划值）
   */
  plannedJoinedDate: string;

  /**
   * 候选人姓名（快照）
   */
  candidateName: string;

  /**
   * 候选人手机（快照）
   */
  mobile?: string;

  /**
   * 关联员工（选项 TaktEmployees/options；建档后回填，可空，DictValue=Id）
   */
  employeeId?: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐；建档回填后可写，建档前可空）
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐；建档回填后可写，建档前可空）
   */
  employeeName?: string;

  /**
   * 入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）
   */
  employeeJoinedId?: string;

  /**
   * 入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）
   */
  employeeJoinedName?: string;

  /**
   * 待办说明
   */
  reason?: string;

  /**
   * 待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）
   */
  todoStatus: number;

  /**
   * 员工主档（多对一；建档回填后可有值） （主表：TaktEmployee）
   */
  employee?: Employee;

  /**
   * 录用信息 （主表：TaktTalentOffer）
   */
  offer?: TalentOffer;

  /**
   * 入职上岗单 （主表：TaktEmployeeJoined）
   */
  employeeJoined?: EmployeeJoined;

}


/**
 * EmployeeOnboarding 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EmployeeOnboardingQuery
 * @description 对应后端 TaktEmployeeOnboardingQueryDto
 */
export interface EmployeeOnboardingQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 录用信息（选项 TaktTalentOffers/options；DictValue=Id）
   */
  offerId?: string;

  /**
   * 待办单号（租户+公司内业务编码）
   */
  todoCode?: string;

  /**
   * 计划上岗日期（JoinedDate 计划值）（范围查询-开始）
   */
  plannedJoinedDateStart?: string;

  /**
   * 计划上岗日期（JoinedDate 计划值）（范围查询-结束）
   */
  plannedJoinedDateEnd?: string;

  /**
   * 候选人姓名（快照）
   */
  candidateName?: string;

  /**
   * 候选人手机（快照）
   */
  mobile?: string;

  /**
   * 关联员工（选项 TaktEmployees/options；建档后回填，可空，DictValue=Id）
   */
  employeeId?: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐；建档回填后可写，建档前可空）
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐；建档回填后可写，建档前可空）
   */
  employeeName?: string;

  /**
   * 入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）
   */
  employeeJoinedId?: string;

  /**
   * 待办说明
   */
  reason?: string;

  /**
   * 待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）
   */
  todoStatus?: number;

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
 * 创建EmployeeOnboarding DTO
 * 对应前端 EmployeeOnboardingCreate
 * @description 对应后端 TaktEmployeeOnboardingCreateDto
 */
export interface EmployeeOnboardingCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode: string;

  /**
   * 录用信息（选项 TaktTalentOffers/options；DictValue=Id）
   */
  offerId: string;

  /**
   * 待办单号（租户+公司内业务编码）
   */
  todoCode: string;

  /**
   * 计划上岗日期（JoinedDate 计划值）
   */
  plannedJoinedDate: string;

  /**
   * 候选人姓名（快照）
   */
  candidateName: string;

  /**
   * 候选人手机（快照）
   */
  mobile?: string;

  /**
   * 关联员工（选项 TaktEmployees/options；建档后回填，可空，DictValue=Id）
   */
  employeeId?: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐；建档回填后可写，建档前可空）
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐；建档回填后可写，建档前可空）
   */
  employeeName?: string;

  /**
   * 入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）
   */
  employeeJoinedId?: string;

  /**
   * 待办说明
   */
  reason?: string;

  /**
   * 待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）
   */
  todoStatus: number;

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
 * 更新EmployeeOnboarding DTO
 * 继承 TaktEmployeeOnboardingCreateDto，添加 EmployeeOnboardingId 字段
 * 对应前端 EmployeeOnboardingUpdate
 * @description 对应后端 TaktEmployeeOnboardingUpdateDto
 */
export interface EmployeeOnboardingUpdate extends EmployeeOnboardingCreate {
  /**
   * EmployeeOnboardingID（标识要更新的实体）
   */
  employeeOnboardingId: string;

}


/**
 * EmployeeOnboarding 状态更新 DTO
 * 对应前端 EmployeeOnboardingStatus
 * @description 对应后端 TaktEmployeeOnboardingStatusDto
 */
export interface EmployeeOnboardingStatus {
  /**
   * EmployeeOnboardingID
   */
  employeeOnboardingId: string;

  /**
   * 待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）
   */
  todoStatus: number;

}


/**
 * EmployeeOnboarding 导入模板行 DTO
 * 对应前端 EmployeeOnboardingTemplate
 * @description 对应后端 TaktEmployeeOnboardingTemplateDto
 */
export interface EmployeeOnboardingTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 录用信息（选项 TaktTalentOffers/options；DictValue=Id）
   */
  offerId?: string;

  /**
   * 待办单号（租户+公司内业务编码）
   */
  todoCode?: string;

  /**
   * 计划上岗日期（JoinedDate 计划值）
   */
  plannedJoinedDate?: string;

  /**
   * 候选人姓名（快照）
   */
  candidateName?: string;

  /**
   * 候选人手机（快照）
   */
  mobile?: string;

  /**
   * 关联员工（选项 TaktEmployees/options；建档后回填，可空，DictValue=Id）
   */
  employeeId?: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐；建档回填后可写，建档前可空）
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐；建档回填后可写，建档前可空）
   */
  employeeName?: string;

  /**
   * 入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）
   */
  employeeJoinedId?: string;

  /**
   * 待办说明
   */
  reason?: string;

  /**
   * 待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）
   */
  todoStatus?: number;

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
 * EmployeeOnboarding 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EmployeeOnboardingImport
 * @description 对应后端 TaktEmployeeOnboardingImportDto
 */
export interface EmployeeOnboardingImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 录用信息（选项 TaktTalentOffers/options；DictValue=Id）
   */
  offerId?: string;

  /**
   * 待办单号（租户+公司内业务编码）
   */
  todoCode?: string;

  /**
   * 计划上岗日期（JoinedDate 计划值）
   */
  plannedJoinedDate?: string;

  /**
   * 候选人姓名（快照）
   */
  candidateName?: string;

  /**
   * 候选人手机（快照）
   */
  mobile?: string;

  /**
   * 关联员工（选项 TaktEmployees/options；建档后回填，可空，DictValue=Id）
   */
  employeeId?: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐；建档回填后可写，建档前可空）
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐；建档回填后可写，建档前可空）
   */
  employeeName?: string;

  /**
   * 入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）
   */
  employeeJoinedId?: string;

  /**
   * 待办说明
   */
  reason?: string;

  /**
   * 待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）
   */
  todoStatus?: number;

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
 * EmployeeOnboarding 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeOnboardingExport
 * @description 对应后端 TaktEmployeeOnboardingExportDto
 */
export interface EmployeeOnboardingExport {
  /**
   * EmployeeOnboardingID
   */
  employeeOnboardingId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 录用信息（选项 TaktTalentOffers/options；DictValue=Id）
   */
  offerId: string;

  /**
   * 待办单号（租户+公司内业务编码）
   */
  todoCode: string;

  /**
   * 计划上岗日期（JoinedDate 计划值）
   */
  plannedJoinedDate: string;

  /**
   * 候选人姓名（快照）
   */
  candidateName: string;

  /**
   * 候选人手机（快照）
   */
  mobile?: string;

  /**
   * 关联员工（选项 TaktEmployees/options；建档后回填，可空，DictValue=Id）
   */
  employeeId?: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐；建档回填后可写，建档前可空）
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐；建档回填后可写，建档前可空）
   */
  employeeName?: string;

  /**
   * 入职上岗单（关联 TaktEmployeeJoined.Id；待办完成后回填，可空）
   */
  employeeJoinedId?: string;

  /**
   * 待办说明
   */
  reason?: string;

  /**
   * 待办状态（字典 humanresource_personnel_onboarding_status；0=待办理 1=办理中 2=已完成 3=已取消）
   */
  todoStatus: number;

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

