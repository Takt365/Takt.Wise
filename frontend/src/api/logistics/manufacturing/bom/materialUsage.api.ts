// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：materialUsage.api.ts
// 创建时间：2026-10-21
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 通用物料 / 专用物料 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request'
import type {
  BomMaterialUsageQuery,
  BomMaterialUsageResult,
} from '@/types/logistics/manufacturing/bom/materialUsage.types'

/** API 路由前缀（对应 TaktBomMaterialUsagesController） */
const BOM_MATERIAL_USAGE_API_BASE = 'TaktBomMaterialUsages'

/**
 * 通用物料页物料下拉（明细 ComponentCode）
 * @returns {string} 选项地址
 */
export function getBomCommonMaterialComponentOptionsUrl(): string {
  return `${BOM_MATERIAL_USAGE_API_BASE}/common-material/component-options`
}

/**
 * 专用物料页物料下拉（明细 ComponentCode）
 * @returns {string} 选项地址
 */
export function getBomDedicatedMaterialComponentOptionsUrl(): string {
  return `${BOM_MATERIAL_USAGE_API_BASE}/dedicated-material/component-options`
}

/**
 * 通用物料分页
 * @param queryDto 查询条件
 * @returns 分页结果
 */
export function getBomCommonMaterialList(
  queryDto: BomMaterialUsageQuery,
): Promise<BomMaterialUsageResult> {
  return request<BomMaterialUsageResult>({
    url: `${BOM_MATERIAL_USAGE_API_BASE}/common-material`,
    method: 'get',
    params: queryDto,
    timeout: 120000,
  })
}

/**
 * 专用物料分页
 * @param queryDto 查询条件
 * @returns 分页结果
 */
export function getBomDedicatedMaterialList(
  queryDto: BomMaterialUsageQuery,
): Promise<BomMaterialUsageResult> {
  return request<BomMaterialUsageResult>({
    url: `${BOM_MATERIAL_USAGE_API_BASE}/dedicated-material`,
    method: 'get',
    params: queryDto,
    timeout: 120000,
  })
}

/**
 * 导出通用物料
 * @param query 查询条件
 * @param sheetName 工作表名
 * @param exportName 导出文件名
 * @returns Excel 文件
 */
export function exportBomCommonMaterial(
  query: BomMaterialUsageQuery,
  sheetName?: string,
  exportName?: string,
): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_USAGE_API_BASE}/common-material/export`,
    method: 'get',
    params: { ...query, sheetName, exportName },
    responseType: 'blob',
    returnBinaryMeta: true,
    timeout: 300000,
  })
}

/**
 * 导出专用物料
 * @param query 查询条件
 * @param sheetName 工作表名
 * @param exportName 导出文件名
 * @returns Excel 文件
 */
export function exportBomDedicatedMaterial(
  query: BomMaterialUsageQuery,
  sheetName?: string,
  exportName?: string,
): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_USAGE_API_BASE}/dedicated-material/export`,
    method: 'get',
    params: { ...query, sheetName, exportName },
    responseType: 'blob',
    returnBinaryMeta: true,
    timeout: 300000,
  })
}
