// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employeeDelegation.types.ts
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
 * 员工代理关系实体 独立记录所有代理场景（部门代理、岗位代理、审批代理等）
 * 对应前端 TaktEmployeeDelegationDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmployeeDelegation
 * @description 对应后端 TaktEmployeeDelegationDto
 */
export interface EmployeeDelegation extends CompanyDtoBase {
  /**
   * EmployeeDelegationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  employeeDelegationId: string;

  /**
   * 代理人（选项 TaktEmployees/options；DictValue=Id）
   */
  proxyEmployeeId: string;

  /**
   * 代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  proxyEmployeeCode: string;

  /**
   * 代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  proxyEmployeeName: string;

  /**
   * 被代理人（选项 TaktEmployees/options；DictValue=Id）
   */
  originalEmployeeId: string;

  /**
   * 被代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  originalEmployeeCode: string;

  /**
   * 被代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  originalEmployeeName: string;

  /**
   * 代理类型（字典 humanresource_personnel_employee_delegation_type；1=完全代理 2=部分代理 3=审批代理）
   */
  delegationType: number;

  /**
   * 代理范围类型（字典 humanresource_personnel_employee_delegation_scope；1=部门级别 2=岗位级别 3=全局代理 4=特定业务）
   */
  scopeType: number;

  /**
   * 代理范围 ID（ScopeType=1 时关联 TaktDept.Id/TaktDepts/tree-options；=2 时关联 TaktPost.Id/TaktPosts/options；=4 时为业务主键）
   */
  scopeId?: string;

  /**
   * 代理范围 名称（填充字段）
   */
  scopeName?: string;

  /**
   * 代理原因（如休假、出差、培训、岗位空缺、病假等）
   */
  reason: string;

  /**
   * 代理开始时间
   */
  startDate: string;

  /**
   * 代理结束时间（null=长期有效，直到手动删除）
   */
  endDate?: string;

  /**
   * 被代理人（多对一；外键 OriginalEmployeeId，非 EmployeeId） （主表：TaktEmployee）
   */
  originalEmployee?: Employee;

  /**
   * 代理人（多对一；外键 ProxyEmployeeId，非 EmployeeId） （主表：TaktEmployee）
   */
  proxyEmployee?: Employee;

}


/**
 * EmployeeDelegation 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EmployeeDelegationQuery
 * @description 对应后端 TaktEmployeeDelegationQueryDto
 */
export interface EmployeeDelegationQuery extends TaktPagedQuery {
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
   * 代理人（选项 TaktEmployees/options；DictValue=Id）
   */
  proxyEmployeeId?: string;

  /**
   * 代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  proxyEmployeeCode?: string;

  /**
   * 代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  proxyEmployeeName?: string;

  /**
   * 被代理人（选项 TaktEmployees/options；DictValue=Id）
   */
  originalEmployeeId?: string;

  /**
   * 被代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  originalEmployeeCode?: string;

  /**
   * 被代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  originalEmployeeName?: string;

  /**
   * 代理类型（字典 humanresource_personnel_employee_delegation_type；1=完全代理 2=部分代理 3=审批代理）
   */
  delegationType?: number;

  /**
   * 代理范围类型（字典 humanresource_personnel_employee_delegation_scope；1=部门级别 2=岗位级别 3=全局代理 4=特定业务）
   */
  scopeType?: number;

  /**
   * 代理范围 ID（ScopeType=1 时关联 TaktDept.Id/TaktDepts/tree-options；=2 时关联 TaktPost.Id/TaktPosts/options；=4 时为业务主键）
   */
  scopeId?: string;

  /**
   * 代理原因（如休假、出差、培训、岗位空缺、病假等）
   */
  reason?: string;

  /**
   * 代理开始时间（范围查询-开始）
   */
  startDateStart?: string;

  /**
   * 代理开始时间（范围查询-结束）
   */
  startDateEnd?: string;

  /**
   * 代理结束时间（null=长期有效，直到手动删除）（范围查询-开始）
   */
  endDateStart?: string;

  /**
   * 代理结束时间（null=长期有效，直到手动删除）（范围查询-结束）
   */
  endDateEnd?: string;

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
 * 创建EmployeeDelegation DTO
 * 对应前端 EmployeeDelegationCreate
 * @description 对应后端 TaktEmployeeDelegationCreateDto
 */
export interface EmployeeDelegationCreate {
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
   * 代理人（选项 TaktEmployees/options；DictValue=Id）
   */
  proxyEmployeeId: string;

  /**
   * 代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  proxyEmployeeCode: string;

  /**
   * 代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  proxyEmployeeName: string;

  /**
   * 被代理人（选项 TaktEmployees/options；DictValue=Id）
   */
  originalEmployeeId: string;

  /**
   * 被代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  originalEmployeeCode: string;

  /**
   * 被代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  originalEmployeeName: string;

  /**
   * 代理类型（字典 humanresource_personnel_employee_delegation_type；1=完全代理 2=部分代理 3=审批代理）
   */
  delegationType: number;

  /**
   * 代理范围类型（字典 humanresource_personnel_employee_delegation_scope；1=部门级别 2=岗位级别 3=全局代理 4=特定业务）
   */
  scopeType: number;

  /**
   * 代理范围 ID（ScopeType=1 时关联 TaktDept.Id/TaktDepts/tree-options；=2 时关联 TaktPost.Id/TaktPosts/options；=4 时为业务主键）
   */
  scopeId?: string;

  /**
   * 代理原因（如休假、出差、培训、岗位空缺、病假等）
   */
  reason: string;

  /**
   * 代理开始时间
   */
  startDate: string;

  /**
   * 代理结束时间（null=长期有效，直到手动删除）
   */
  endDate?: string;

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
 * 更新EmployeeDelegation DTO
 * 继承 TaktEmployeeDelegationCreateDto，添加 EmployeeDelegationId 字段
 * 对应前端 EmployeeDelegationUpdate
 * @description 对应后端 TaktEmployeeDelegationUpdateDto
 */
export interface EmployeeDelegationUpdate extends EmployeeDelegationCreate {
  /**
   * EmployeeDelegationID（标识要更新的实体）
   */
  employeeDelegationId: string;

}


/**
 * EmployeeDelegation 导入模板行 DTO
 * 对应前端 EmployeeDelegationTemplate
 * @description 对应后端 TaktEmployeeDelegationTemplateDto
 */
export interface EmployeeDelegationTemplate {
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
   * 代理人（选项 TaktEmployees/options；DictValue=Id）
   */
  proxyEmployeeId?: string;

  /**
   * 代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  proxyEmployeeCode?: string;

  /**
   * 代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  proxyEmployeeName?: string;

  /**
   * 被代理人（选项 TaktEmployees/options；DictValue=Id）
   */
  originalEmployeeId?: string;

  /**
   * 被代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  originalEmployeeCode?: string;

  /**
   * 被代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  originalEmployeeName?: string;

  /**
   * 代理类型（字典 humanresource_personnel_employee_delegation_type；1=完全代理 2=部分代理 3=审批代理）
   */
  delegationType?: number;

  /**
   * 代理范围类型（字典 humanresource_personnel_employee_delegation_scope；1=部门级别 2=岗位级别 3=全局代理 4=特定业务）
   */
  scopeType?: number;

  /**
   * 代理范围 ID（ScopeType=1 时关联 TaktDept.Id/TaktDepts/tree-options；=2 时关联 TaktPost.Id/TaktPosts/options；=4 时为业务主键）
   */
  scopeId?: string;

  /**
   * 代理原因（如休假、出差、培训、岗位空缺、病假等）
   */
  reason?: string;

  /**
   * 代理开始时间
   */
  startDate?: string;

  /**
   * 代理结束时间（null=长期有效，直到手动删除）
   */
  endDate?: string;

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
 * EmployeeDelegation 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EmployeeDelegationImport
 * @description 对应后端 TaktEmployeeDelegationImportDto
 */
export interface EmployeeDelegationImport {
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
   * 代理人（选项 TaktEmployees/options；DictValue=Id）
   */
  proxyEmployeeId?: string;

  /**
   * 代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  proxyEmployeeCode?: string;

  /**
   * 代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  proxyEmployeeName?: string;

  /**
   * 被代理人（选项 TaktEmployees/options；DictValue=Id）
   */
  originalEmployeeId?: string;

  /**
   * 被代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  originalEmployeeCode?: string;

  /**
   * 被代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  originalEmployeeName?: string;

  /**
   * 代理类型（字典 humanresource_personnel_employee_delegation_type；1=完全代理 2=部分代理 3=审批代理）
   */
  delegationType?: number;

  /**
   * 代理范围类型（字典 humanresource_personnel_employee_delegation_scope；1=部门级别 2=岗位级别 3=全局代理 4=特定业务）
   */
  scopeType?: number;

  /**
   * 代理范围 ID（ScopeType=1 时关联 TaktDept.Id/TaktDepts/tree-options；=2 时关联 TaktPost.Id/TaktPosts/options；=4 时为业务主键）
   */
  scopeId?: string;

  /**
   * 代理原因（如休假、出差、培训、岗位空缺、病假等）
   */
  reason?: string;

  /**
   * 代理开始时间
   */
  startDate?: string;

  /**
   * 代理结束时间（null=长期有效，直到手动删除）
   */
  endDate?: string;

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
 * EmployeeDelegation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeDelegationExport
 * @description 对应后端 TaktEmployeeDelegationExportDto
 */
export interface EmployeeDelegationExport {
  /**
   * EmployeeDelegationID
   */
  employeeDelegationId: string;

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
   * 代理人（选项 TaktEmployees/options；DictValue=Id）
   */
  proxyEmployeeId: string;

  /**
   * 代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  proxyEmployeeCode: string;

  /**
   * 代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  proxyEmployeeName: string;

  /**
   * 被代理人（选项 TaktEmployees/options；DictValue=Id）
   */
  originalEmployeeId: string;

  /**
   * 被代理人编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  originalEmployeeCode: string;

  /**
   * 被代理人姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  originalEmployeeName: string;

  /**
   * 代理类型（字典 humanresource_personnel_employee_delegation_type；1=完全代理 2=部分代理 3=审批代理）
   */
  delegationType: number;

  /**
   * 代理范围类型（字典 humanresource_personnel_employee_delegation_scope；1=部门级别 2=岗位级别 3=全局代理 4=特定业务）
   */
  scopeType: number;

  /**
   * 代理范围 ID（ScopeType=1 时关联 TaktDept.Id/TaktDepts/tree-options；=2 时关联 TaktPost.Id/TaktPosts/options；=4 时为业务主键）
   */
  scopeId?: string;

  /**
   * 代理原因（如休假、出差、培训、岗位空缺、病假等）
   */
  reason: string;

  /**
   * 代理开始时间
   */
  startDate: string;

  /**
   * 代理结束时间（null=长期有效，直到手动删除）
   */
  endDate?: string;

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

