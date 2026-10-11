// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/organization
// 文件名称：employeeDept.types.ts
// 创建时间：2026-05-28
// 创建人：Takt365(Cursor AI)
// 功能描述：TaktEmployeeDept RBAC 关联类型（仅列表，分配见 rbac.ts）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CompanyDtoBase } from '@/types/common.types';

/**
 * 员工-部门关联列表（对应后端 TaktEmployeeDeptDto）
 */
export interface EmployeeDept extends CompanyDtoBase {
  /** 关联主键 */
  employeeDeptId: string;
  /** 员工ID */
  employeeId: string;
  /** 员工姓名（填充） */
  employeeName?: string;
  /** 部门ID */
  deptId: string;
  /** 部门名称（填充） */
  deptName?: string;
}
