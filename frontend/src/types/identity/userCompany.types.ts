// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/identity
// 文件名称：userCompany.types.ts
// 创建时间：2026-05-28
// 创建人：Takt365(Cursor AI)
// 功能描述：TaktUserCompany RBAC 关联类型（仅列表，分配见 rbac.ts）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CompanyDtoBase,TaktYesNo } from '@/types/common.types';

/**
 * 用户-公司关联列表（对应后端 TaktUserCompanyDto）
 */
export interface UserCompany extends CompanyDtoBase {
  /** 关联主键 */
  userCompanyId: string;
  /** 用户ID */
  userId: string;
  /** 用户名称（填充） */
  userName?: string;
  /** 默认公司 */
  isDefault: TaktYesNo;
}
