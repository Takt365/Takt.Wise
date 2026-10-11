// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/api/logistics/manufacturing/aps
// 文件名称：productionOrderFormFill.api.ts
// 功能描述：生产工单表单回填 API（独立非实体 CRUD；generate-from-backend 不会覆盖）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request'
import type {
  ProductionOrderFormFill,
  ProductionOrderFormFillQuery,
} from '@/types/logistics/manufacturing/aps/productionOrderFormFill.types'

/**
 * API 路径前缀（对应 TaktProductionOrderFormFillsController）
 */
const PRODUCTION_ORDER_FORM_FILL_API_BASE = 'TaktProductionOrderFormFills'

/**
 * 按工单号获取表单回填（工单类别/机种/物料/批次/数量/序号/标准工时等）
 * @param query 查询参数
 * @returns 回填 DTO；工单不存在时为 null
 */
export function getProductionOrderFormFill(
  query: ProductionOrderFormFillQuery
): Promise<ProductionOrderFormFill | null> {
  const prodOrderCode = query.prodOrderCode?.trim()
  if (!prodOrderCode) {
    return Promise.resolve(null)
  }
  return request<ProductionOrderFormFill | null>({
    url: `${PRODUCTION_ORDER_FORM_FILL_API_BASE}/by-code`,
    method: 'get',
    params: {
      prodOrderCode,
      plantCode: query.plantCode?.trim() || undefined,
      prodDate: query.prodDate?.trim().slice(0, 10) || undefined,
      directLabor: query.directLabor,
      includeDefaultDetails: query.includeDefaultDetails === true,
    },
  })
}
