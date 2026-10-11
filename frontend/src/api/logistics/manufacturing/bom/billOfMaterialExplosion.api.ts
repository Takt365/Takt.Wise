// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：billOfMaterialExplosion.api.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  BillOfMaterialExplosion,
  BillOfMaterialExplosionQuery
} from '@/types/logistics/manufacturing/bom/billOfMaterialExplosion.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktBillOfMaterialExplosions
 */
const BILL_OF_MATERIAL_EXPLOSION_API_BASE = 'TaktBillOfMaterialExplosions';

// ========================================
// 基础 CRUD
// ========================================

/**
 * BOM 递归展开（运行时多层展开，单层存储）
 * @param {BillOfMaterialExplosionQuery} queryDto 展开参数
 * @returns {Promise<BillOfMaterialExplosion>} 展开结果
 */
export function getBillOfMaterialExplosion(queryDto: BillOfMaterialExplosionQuery): Promise<BillOfMaterialExplosion> {
  return request<BillOfMaterialExplosion>({
    url: `${BILL_OF_MATERIAL_EXPLOSION_API_BASE}`,
    method: 'get',
    params: queryDto,
  });
}
