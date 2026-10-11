// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/benefits
// 文件名称：empBenefitPlan.types.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/benefits 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 员工福利方案（非现金福利参与配置）
 * 对应前端 TaktEmpBenefitPlanDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmpBenefitPlan
 * @description 对应后端 TaktEmpBenefitPlanDto
 */
export interface EmpBenefitPlan extends CompanyDtoBase {
  /**
   * EmpBenefitPlanID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  empBenefitPlanId: string;

  /**
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 福利项目（选项 TaktBenefitItems/options；DictValue=Id）
   */
  benefitItemId: string;

  /**
   * 福利项目（选项 TaktBenefitItems/options；DictValue=Id）
   */
  benefitItemName?: string;

  /**
   * 方案编码
   */
  planCode: string;

  /**
   * 参保/参与日期
   */
  enrollmentDate: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）
   */
  empBenefitStatus: number;

}


/**
 * EmpBenefitPlan 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EmpBenefitPlanQuery
 * @description 对应后端 TaktEmpBenefitPlanQueryDto
 */
export interface EmpBenefitPlanQuery extends TaktPagedQuery {
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
   * 员工姓名
   */
  employeeName?: string;

  /**
   * 福利项目（选项 TaktBenefitItems/options；DictValue=Id）
   */
  benefitItemId?: string;

  /**
   * 方案编码
   */
  planCode?: string;

  /**
   * 参保/参与日期（范围查询-开始）
   */
  enrollmentDateStart?: string;

  /**
   * 参保/参与日期（范围查询-结束）
   */
  enrollmentDateEnd?: string;

  /**
   * 失效日期（范围查询-开始）
   */
  expiryDateStart?: string;

  /**
   * 失效日期（范围查询-结束）
   */
  expiryDateEnd?: string;

  /**
   * 状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）
   */
  empBenefitStatus?: number;

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
 * 创建EmpBenefitPlan DTO
 * 对应前端 EmpBenefitPlanCreate
 * @description 对应后端 TaktEmpBenefitPlanCreateDto
 */
export interface EmpBenefitPlanCreate {
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
   * 员工姓名
   */
  employeeName: string;

  /**
   * 福利项目（选项 TaktBenefitItems/options；DictValue=Id）
   */
  benefitItemId: string;

  /**
   * 方案编码
   */
  planCode: string;

  /**
   * 参保/参与日期
   */
  enrollmentDate: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）
   */
  empBenefitStatus: number;

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
 * 更新EmpBenefitPlan DTO
 * 继承 TaktEmpBenefitPlanCreateDto，添加 EmpBenefitPlanId 字段
 * 对应前端 EmpBenefitPlanUpdate
 * @description 对应后端 TaktEmpBenefitPlanUpdateDto
 */
export interface EmpBenefitPlanUpdate extends EmpBenefitPlanCreate {
  /**
   * EmpBenefitPlanID（标识要更新的实体）
   */
  empBenefitPlanId: string;

}


/**
 * EmpBenefitPlan 状态更新 DTO
 * 对应前端 EmpBenefitPlanStatus
 * @description 对应后端 TaktEmpBenefitPlanStatusDto
 */
export interface EmpBenefitPlanStatus {
  /**
   * EmpBenefitPlanID
   */
  empBenefitPlanId: string;

  /**
   * 状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）
   */
  empBenefitStatus: number;

}


/**
 * EmpBenefitPlan 导入模板行 DTO
 * 对应前端 EmpBenefitPlanTemplate
 * @description 对应后端 TaktEmpBenefitPlanTemplateDto
 */
export interface EmpBenefitPlanTemplate {
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
   * 员工姓名
   */
  employeeName?: string;

  /**
   * 福利项目（选项 TaktBenefitItems/options；DictValue=Id）
   */
  benefitItemId?: string;

  /**
   * 方案编码
   */
  planCode?: string;

  /**
   * 参保/参与日期
   */
  enrollmentDate?: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）
   */
  empBenefitStatus?: number;

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
 * EmpBenefitPlan 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EmpBenefitPlanImport
 * @description 对应后端 TaktEmpBenefitPlanImportDto
 */
export interface EmpBenefitPlanImport {
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
   * 员工姓名
   */
  employeeName?: string;

  /**
   * 福利项目（选项 TaktBenefitItems/options；DictValue=Id）
   */
  benefitItemId?: string;

  /**
   * 方案编码
   */
  planCode?: string;

  /**
   * 参保/参与日期
   */
  enrollmentDate?: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）
   */
  empBenefitStatus?: number;

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
 * EmpBenefitPlan 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmpBenefitPlanExport
 * @description 对应后端 TaktEmpBenefitPlanExportDto
 */
export interface EmpBenefitPlanExport {
  /**
   * EmpBenefitPlanID
   */
  empBenefitPlanId: string;

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
   * 员工姓名
   */
  employeeName: string;

  /**
   * 福利项目（选项 TaktBenefitItems/options；DictValue=Id）
   */
  benefitItemId: string;

  /**
   * 方案编码
   */
  planCode: string;

  /**
   * 参保/参与日期
   */
  enrollmentDate: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）
   */
  empBenefitStatus: number;

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

