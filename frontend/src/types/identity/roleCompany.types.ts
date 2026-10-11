// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/identity
// 文件名称：roleCompany.types.ts
// 创建时间：2026-05-28
// 创建人：Takt365(Cursor AI)
// 功能描述：TaktRoleCompany RBAC 关联类型（仅列表，分配见 rbac.ts）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何风险。
// ========================================
import type { CompanyDtoBase } from '@/types/common.types';

/**
 * 角色-公司关联列表（对应后端 TaktRoleCompanyDto）
 */
export interface RoleCompany extends CompanyDtoBase {
  /** 关联主键 */
  roleCompanyId: string;
  /** 角色ID */
  roleId: string;
  /** 角色名称（填充） */
  roleName?: string;
}
