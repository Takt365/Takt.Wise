// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employeeJoined.types.ts
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/personnel 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 员工入职上岗办理记录（审批单，Joined=实际上班；审批态见基类 ApprovalStatus，字典 sys_approval_status）
 * 对应前端 TaktEmployeeJoinedDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 EmployeeJoined
 * @description 对应后端 TaktEmployeeJoinedDto
 */
export interface EmployeeJoined extends ApprovalDtoBase {
  /**
   * EmployeeJoinedID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  employeeJoinedId: string;

  /**
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName: string;

  /**
   * 入职待办（选项 TaktEmployeeOnboardings/options；DictValue=Id）
   */
  onboardingId?: string;

  /**
   * 入职待办（选项 TaktEmployeeOnboardings/options；DictValue=Id）
   */
  onboardingName?: string;

  /**
   * 实际上岗日期（JoinedDate：我去上班）
   */
  joinedDate: string;

  /**
   * 试用期结束日期
   */
  probationEndDate?: string;

  /**
   * 转正日期
   */
  regularDate?: string;

  /**
   * 上岗部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId: string;

  /**
   * 上岗部门名称
   */
  deptName: string;

  /**
   * 上岗岗位（选项 TaktPosts/options；DictValue=Id）
   */
  postId?: string;

  /**
   * 上岗岗位名称
   */
  postName?: string;

  /**
   * 职务/职称
   */
  jobTitle?: string;

  /**
   * 工作性质（字典 humanresource_personnel_employee_work_nature；0=全职 1=兼职 2=实习 3=外包 4=其他）
   */
  workNature: number;

  /**
   * 任职类型（字典 humanresource_personnel_employee_employment_type；0=主职 1=兼职 2=借调 3=挂职）
   */
  employmentType: number;

  /**
   * 直属上级（选项 TaktEmployees/options；DictValue=Id）
   */
  directManagerId?: string;

  /**
   * 直属上级姓名
   */
  directManagerName?: string;

  /**
   * 员工主档（多对一） （主表：TaktEmployee）
   */
  employee?: Employee;

  /**
   * 直属上级员工（多对一） （主表：TaktEmployee）
   */
  directManager?: Employee;

}


/**
 * EmployeeJoined 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EmployeeJoinedQuery
 * @description 对应后端 TaktEmployeeJoinedQueryDto
 */
export interface EmployeeJoinedQuery extends TaktPagedQuery {
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
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId?: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName?: string;

  /**
   * 入职待办（选项 TaktEmployeeOnboardings/options；DictValue=Id）
   */
  onboardingId?: string;

  /**
   * 实际上岗日期（JoinedDate：我去上班）（范围查询-开始）
   */
  joinedDateStart?: string;

  /**
   * 实际上岗日期（JoinedDate：我去上班）（范围查询-结束）
   */
  joinedDateEnd?: string;

  /**
   * 试用期结束日期（范围查询-开始）
   */
  probationEndDateStart?: string;

  /**
   * 试用期结束日期（范围查询-结束）
   */
  probationEndDateEnd?: string;

  /**
   * 转正日期（范围查询-开始）
   */
  regularDateStart?: string;

  /**
   * 转正日期（范围查询-结束）
   */
  regularDateEnd?: string;

  /**
   * 上岗部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId?: string;

  /**
   * 上岗部门名称
   */
  deptName?: string;

  /**
   * 上岗岗位（选项 TaktPosts/options；DictValue=Id）
   */
  postId?: string;

  /**
   * 上岗岗位名称
   */
  postName?: string;

  /**
   * 职务/职称
   */
  jobTitle?: string;

  /**
   * 工作性质（字典 humanresource_personnel_employee_work_nature；0=全职 1=兼职 2=实习 3=外包 4=其他）
   */
  workNature?: number;

  /**
   * 任职类型（字典 humanresource_personnel_employee_employment_type；0=主职 1=兼职 2=借调 3=挂职）
   */
  employmentType?: number;

  /**
   * 直属上级（选项 TaktEmployees/options；DictValue=Id）
   */
  directManagerId?: string;

  /**
   * 直属上级姓名
   */
  directManagerName?: string;

  /**
   * 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
   */
  approvalStatus?: number;

  /**
   * 发起人ID
   */
  initiatorId?: string;

  /**
   * 发起时间（范围查询-开始）
   */
  initiatedAtStart?: string;

  /**
   * 发起时间（范围查询-结束）
   */
  initiatedAtEnd?: string;

  /**
   * 最终审批人ID
   */
  approvedBy?: string;

  /**
   * 最终审批时间（范围查询-开始）
   */
  approvedAtStart?: string;

  /**
   * 最终审批时间（范围查询-结束）
   */
  approvedAtEnd?: string;

  /**
   * 流程实例 ID
   */
  flowInstanceId?: string;

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
 * 创建EmployeeJoined DTO
 * 对应前端 EmployeeJoinedCreate
 * @description 对应后端 TaktEmployeeJoinedCreateDto
 */
export interface EmployeeJoinedCreate {
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
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName: string;

  /**
   * 入职待办（选项 TaktEmployeeOnboardings/options；DictValue=Id）
   */
  onboardingId?: string;

  /**
   * 实际上岗日期（JoinedDate：我去上班）
   */
  joinedDate: string;

  /**
   * 试用期结束日期
   */
  probationEndDate?: string;

  /**
   * 转正日期
   */
  regularDate?: string;

  /**
   * 上岗部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId: string;

  /**
   * 上岗部门名称
   */
  deptName: string;

  /**
   * 上岗岗位（选项 TaktPosts/options；DictValue=Id）
   */
  postId?: string;

  /**
   * 上岗岗位名称
   */
  postName?: string;

  /**
   * 职务/职称
   */
  jobTitle?: string;

  /**
   * 工作性质（字典 humanresource_personnel_employee_work_nature；0=全职 1=兼职 2=实习 3=外包 4=其他）
   */
  workNature: number;

  /**
   * 任职类型（字典 humanresource_personnel_employee_employment_type；0=主职 1=兼职 2=借调 3=挂职）
   */
  employmentType: number;

  /**
   * 直属上级（选项 TaktEmployees/options；DictValue=Id）
   */
  directManagerId?: string;

  /**
   * 直属上级姓名
   */
  directManagerName?: string;

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
 * 更新EmployeeJoined DTO
 * 继承 TaktEmployeeJoinedCreateDto，添加 EmployeeJoinedId 字段
 * 对应前端 EmployeeJoinedUpdate
 * @description 对应后端 TaktEmployeeJoinedUpdateDto
 */
export interface EmployeeJoinedUpdate extends EmployeeJoinedCreate {
  /**
   * EmployeeJoinedID（标识要更新的实体）
   */
  employeeJoinedId: string;

}


/**
 * EmployeeJoined 导入模板行 DTO
 * 对应前端 EmployeeJoinedTemplate
 * @description 对应后端 TaktEmployeeJoinedTemplateDto
 */
export interface EmployeeJoinedTemplate {
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
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId?: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName?: string;

  /**
   * 入职待办（选项 TaktEmployeeOnboardings/options；DictValue=Id）
   */
  onboardingId?: string;

  /**
   * 实际上岗日期（JoinedDate：我去上班）
   */
  joinedDate?: string;

  /**
   * 试用期结束日期
   */
  probationEndDate?: string;

  /**
   * 转正日期
   */
  regularDate?: string;

  /**
   * 上岗部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId?: string;

  /**
   * 上岗部门名称
   */
  deptName?: string;

  /**
   * 上岗岗位（选项 TaktPosts/options；DictValue=Id）
   */
  postId?: string;

  /**
   * 上岗岗位名称
   */
  postName?: string;

  /**
   * 职务/职称
   */
  jobTitle?: string;

  /**
   * 工作性质（字典 humanresource_personnel_employee_work_nature；0=全职 1=兼职 2=实习 3=外包 4=其他）
   */
  workNature?: number;

  /**
   * 任职类型（字典 humanresource_personnel_employee_employment_type；0=主职 1=兼职 2=借调 3=挂职）
   */
  employmentType?: number;

  /**
   * 直属上级（选项 TaktEmployees/options；DictValue=Id）
   */
  directManagerId?: string;

  /**
   * 直属上级姓名
   */
  directManagerName?: string;

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
 * EmployeeJoined 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EmployeeJoinedImport
 * @description 对应后端 TaktEmployeeJoinedImportDto
 */
export interface EmployeeJoinedImport {
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
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId?: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName?: string;

  /**
   * 入职待办（选项 TaktEmployeeOnboardings/options；DictValue=Id）
   */
  onboardingId?: string;

  /**
   * 实际上岗日期（JoinedDate：我去上班）
   */
  joinedDate?: string;

  /**
   * 试用期结束日期
   */
  probationEndDate?: string;

  /**
   * 转正日期
   */
  regularDate?: string;

  /**
   * 上岗部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId?: string;

  /**
   * 上岗部门名称
   */
  deptName?: string;

  /**
   * 上岗岗位（选项 TaktPosts/options；DictValue=Id）
   */
  postId?: string;

  /**
   * 上岗岗位名称
   */
  postName?: string;

  /**
   * 职务/职称
   */
  jobTitle?: string;

  /**
   * 工作性质（字典 humanresource_personnel_employee_work_nature；0=全职 1=兼职 2=实习 3=外包 4=其他）
   */
  workNature?: number;

  /**
   * 任职类型（字典 humanresource_personnel_employee_employment_type；0=主职 1=兼职 2=借调 3=挂职）
   */
  employmentType?: number;

  /**
   * 直属上级（选项 TaktEmployees/options；DictValue=Id）
   */
  directManagerId?: string;

  /**
   * 直属上级姓名
   */
  directManagerName?: string;

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
 * EmployeeJoined 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeJoinedExport
 * @description 对应后端 TaktEmployeeJoinedExportDto
 */
export interface EmployeeJoinedExport {
  /**
   * EmployeeJoinedID
   */
  employeeJoinedId: string;

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
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName: string;

  /**
   * 入职待办（选项 TaktEmployeeOnboardings/options；DictValue=Id）
   */
  onboardingId?: string;

  /**
   * 实际上岗日期（JoinedDate：我去上班）
   */
  joinedDate: string;

  /**
   * 试用期结束日期
   */
  probationEndDate?: string;

  /**
   * 转正日期
   */
  regularDate?: string;

  /**
   * 上岗部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId: string;

  /**
   * 上岗部门名称
   */
  deptName: string;

  /**
   * 上岗岗位（选项 TaktPosts/options；DictValue=Id）
   */
  postId?: string;

  /**
   * 上岗岗位名称
   */
  postName?: string;

  /**
   * 职务/职称
   */
  jobTitle?: string;

  /**
   * 工作性质（字典 humanresource_personnel_employee_work_nature；0=全职 1=兼职 2=实习 3=外包 4=其他）
   */
  workNature: number;

  /**
   * 任职类型（字典 humanresource_personnel_employee_employment_type；0=主职 1=兼职 2=借调 3=挂职）
   */
  employmentType: number;

  /**
   * 直属上级（选项 TaktEmployees/options；DictValue=Id）
   */
  directManagerId?: string;

  /**
   * 直属上级姓名
   */
  directManagerName?: string;

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

