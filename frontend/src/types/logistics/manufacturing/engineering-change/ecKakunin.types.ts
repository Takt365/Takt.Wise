// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecKakunin.types.ts
// 创建时间：2026-06-22
// 功能描述：设变物料确认类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CompanyDtoBase, TaktPagedQuery } from '@/types/common.types';

export interface EcKakunin extends CompanyDtoBase {
  ecDetailId: string;
  ecCode: string;
  lineNumber: number;
  ecModelCode: string;
  ecOldMaterialCode?: string;
  ecOldMaterialDescription?: string;
  ecOldPurchaseType?: string;
  ecNewMaterialCode?: string;
  ecNewMaterialDescription?: string;
  ecNewPurchaseType?: string;
  ecOldRequiresInspection: number;
  ecNewRequiresInspection: number;
}

export interface EcKakuninQuery extends TaktPagedQuery {
  ecCode?: string;
  cultureCode?: string;
  ecModelCode?: string;
  ecOldMaterialCode?: string;
  ecNewMaterialCode?: string;
  ecOldPurchaseType?: string;
  ecNewPurchaseType?: string;
  ecOldRequiresInspection?: number;
  ecNewRequiresInspection?: number;
}

export interface EcKakuninUpdate {
  ecDetailId: string;
  ecOldPurchaseType: number;
  ecOldRequiresInspection: number;
  ecNewPurchaseType: number;
  ecNewRequiresInspection: number;
}
