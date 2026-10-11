// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/compensation
// 文件名称：empSalary.types.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/compensation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 员工薪酬档案（现金报酬定薪记录）
 * 对应前端 TaktEmpSalaryDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmpSalary
 * @description 对应后端 TaktEmpSalaryDto
 */
export interface EmpSalary extends CompanyDtoBase {

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
 * EmpSalary 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmpSalaryExport
 * @description 对应后端 TaktEmpSalaryExportDto
 */
export interface EmpSalaryExport {
  /**
   * EmpSalaryID
   */
  empSalaryId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 关联薪酬体系 ID
   */
  payrollId?: string;

  /**
   * 关联薪级 ID
   */
  payScaleId?: string;

  /**
   * 基本工资（元）
   */
  baseSalary: number;

  /**
   * 岗位工资（元）
   */
  positionSalary: number;

  /**
   * 津贴合计（元）
   */
  allowanceTotal: number;

  /**
   * 关联薪资项目 ID（如股权激励项，对应 TaktSalaryItem 中 item_type 为股权激励的记录）
   */
  salaryItemId?: string;

  /**
   * 授予股数/份数（股权激励定薪时使用）
   */
  empSalaryShareCount: number;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 状态（字典 sys_normal_disable）
   */
  empSalaryStatus: number;

  /**
   * 关联工厂
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

