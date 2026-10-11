// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/bom
// 文件名称：materialZeroPrice.api.ts
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 组件零价格清单 API（独立控制器 TaktBomMaterialZeroPrices）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request'
import type {
  BomMaterialZeroPriceQuery,
  BomMaterialZeroPriceResult,
  BomMaterialZeroPriceMovingBackfillDto,
  BomMaterialZeroPriceMovingBackfillResult,
  BomMaterialZeroPriceManualMovingDto,
  BomMaterialZeroPricePcbSectMarkDto,
  BomMaterialZeroPricePcbSectMarkResult,
  BomMaterialZeroPriceStripLeadingZeroDto,
  BomMaterialZeroPriceStripLeadingZeroResult,
} from '@/types/logistics/manufacturing/bom/materialZeroPrice.types'

/** API 路由前缀（对应 TaktBomMaterialZeroPricesController） */
const BOM_MATERIAL_ZERO_PRICE_API_BASE = 'TaktBomMaterialZeroPrices'

/**
 * 获取组件零价格合并清单（工厂+核算月；仅 FERT）
 * @param queryDto 查询
 * @returns 合并结果
 */
export function getBomMaterialZeroPriceList(
  queryDto: BomMaterialZeroPriceQuery,
): Promise<BomMaterialZeroPriceResult> {
  return request<BomMaterialZeroPriceResult>({
    url: `${BOM_MATERIAL_ZERO_PRICE_API_BASE}/list`,
    method: 'get',
    params: queryDto,
  })
}

/**
 * 导出组件零价格合并清单
 * @param query 查询
 * @param sheetName 工作表名
 * @param exportName 导出文件名
 * @returns Excel（含 contentDisposition 元数据）
 */
export function exportBomMaterialZeroPriceData(
  query: BomMaterialZeroPriceQuery,
  sheetName?: string,
  exportName?: string,
): Promise<Blob> {
  return request<Blob>({
    url: `${BOM_MATERIAL_ZERO_PRICE_API_BASE}/export`,
    method: 'get',
    params: {
      ...query,
      sheetName,
      exportName,
    },
    responseType: 'blob',
    returnBinaryMeta: true,
  })
}

/**
 * 回填移动平均价/单位/货币（操作列或批量；逆推源价写入明细 + ExtField._bk.mp；同步刷产品/机种月成本）
 * @param dto 工厂+核算月+组件（空=批量）
 * @returns 回填统计
 */
export function backfillBomMaterialZeroPriceMoving(
  dto: BomMaterialZeroPriceMovingBackfillDto,
): Promise<BomMaterialZeroPriceMovingBackfillResult> {
  return request<BomMaterialZeroPriceMovingBackfillResult>({
    url: `${BOM_MATERIAL_ZERO_PRICE_API_BASE}/backfill-moving-price`,
    method: 'post',
    data: dto,
    timeout: 600000,
  })
}

/**
 * 手工替换更新移动平均价（原组件 ← 新组件价/单位/币种）
 * @param dto 工厂+核算月+原/新组件+价
 * @returns 更新统计
 */
export function manualUpdateBomMaterialZeroPriceMoving(
  dto: BomMaterialZeroPriceManualMovingDto,
): Promise<BomMaterialZeroPriceMovingBackfillResult> {
  return request<BomMaterialZeroPriceMovingBackfillResult>({
    url: `${BOM_MATERIAL_ZERO_PRICE_API_BASE}/manual-moving-price`,
    method: 'post',
    data: dto,
    timeout: 600000,
  })
}

/**
 * PCB SECT 整树 ExtField 打标（pcbSect=X；工厂+核算月；机种可选）
 * @param dto 工厂+核算月+机种
 * @returns 打标统计
 */
export function markBomMaterialZeroPricePcbSect(
  dto: BomMaterialZeroPricePcbSectMarkDto,
): Promise<BomMaterialZeroPricePcbSectMarkResult> {
  return request<BomMaterialZeroPricePcbSectMarkResult>({
    url: `${BOM_MATERIAL_ZERO_PRICE_API_BASE}/mark-pcb-sect`,
    method: 'post',
    data: dto,
    timeout: 600000,
  })
}

/**
 * 去掉当前工厂纯数字产品/组件编码的前导 0（含字母的编码不改）
 * @param dto 工厂
 * @returns 更新统计
 */
export function stripBomMaterialZeroPriceLeadingZeros(
  dto: BomMaterialZeroPriceStripLeadingZeroDto,
): Promise<BomMaterialZeroPriceStripLeadingZeroResult> {
  return request<BomMaterialZeroPriceStripLeadingZeroResult>({
    url: `${BOM_MATERIAL_ZERO_PRICE_API_BASE}/strip-leading-zeros`,
    method: 'post',
    data: dto,
    timeout: 600000,
  })
}
