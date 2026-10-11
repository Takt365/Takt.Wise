// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/identity
// 文件名称：userRole.types.ts
// 创建时间：2026-05-28
// 创建人：Takt365(Cursor AI)
// 功能描述：TaktUserRole RBAC 关联类型（仅列表，分配见 rbac.ts）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================
import type { CompanyDtoBase } from '@/types/common.types';

/**
 * 用户-角色关联列表（对应后端 TaktUserRoleDto）
 */
export interface UserRole extends CompanyDtoBase {
  /** 关联主键 */
  userRoleId: string;
  /** 用户ID */
  userId: string;
  /** 用户名称（填充） */
  userName?: string;
  /** 角色ID */
  roleId: string;
  /** 角色名称（填充） */
  roleName?: string;
}
