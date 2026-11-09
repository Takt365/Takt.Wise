// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/engineering-change/composables
// 文件名称：ecDeptProductionTeamMulti.ts
// 创建时间：2026-10-28
// 创建人：Takt365(Cursor AI)
// 功能描述：制一/制二生产班组多选：库内分号分隔 TeamCode ↔ TaktSelect multiple 数组；按班组分类过滤 options 与课别默认选中
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import {
  PRODUCTION_TEAM_CATEGORY_ASSY,
  PRODUCTION_TEAM_CATEGORY_PCBA,
} from '@/views/logistics/manufacturing/output/composables/productionTeamCategory'

/**
 * 制一课默认生产班组（班组分类 A；TeamCode=1）
 */
export const EC_MFG1_DEFAULT_PRODUCTION_TEAM_CODES = ['1'] as const

/**
 * 制二课默认生产班组（班组分类 P；SMT1/AI1/MI1/RWC1）
 */
export const EC_MFG2_DEFAULT_PRODUCTION_TEAM_CODES = ['SMT1', 'AI1', 'MI1', 'RWC1'] as const

/**
 * 将库内/接口生产班组串拆成 TeamCode 数组（分号或逗号兼容）
 * @param raw 原始串或已是数组
 * @returns {string[]} TeamCode 列表
 */
export function taktSplitEcProductionTeamCodes(raw: unknown): string[] {
  if (Array.isArray(raw)) {
    return raw.map((item) => String(item).trim()).filter((item) => item.length > 0)
  }
  const text = String(raw ?? '').trim()
  if (!text) {
    return []
  }
  return text
    .split(/[;；,，]/)
    .map((item) => item.trim())
    .filter((item) => item.length > 0)
}

/**
 * TeamCode 数组拼成库内存储串（分号分隔）
 * @param codes 已选 TeamCode
 * @returns {string} 分号串；空选返回空串
 */
export function taktJoinEcProductionTeamCodes(codes: readonly unknown[] | null | undefined): string {
  if (!codes?.length) {
    return ''
  }
  return codes
    .map((item) => String(item).trim())
    .filter((item) => item.length > 0)
    .join(';')
}

/**
 * 打开表单时解析生产班组：已有值保留，空则回填课别默认 TeamCode
 * @param raw 库内/接口串
 * @param defaults 空值时的默认 TeamCode
 * @returns {string[]} TeamCode 列表
 */
export function taktResolveEcProductionTeamCodes(
  raw: unknown,
  defaults: readonly string[],
): string[] {
  const codes = taktSplitEcProductionTeamCodes(raw)
  if (codes.length > 0) {
    return codes
  }
  return defaults.map((item) => String(item).trim()).filter((item) => item.length > 0)
}

/**
 * 制一/品管（组立）班组 options：teamCategory=A + 可选工厂
 * @param plantCode 工厂代码
 * @returns api-params
 */
export function buildEcMfg1ProductionTeamOptionsParams(
  plantCode?: string | null,
): { teamCategory: string; plantCode?: string } {
  const plant = String(plantCode ?? '').trim()
  return plant
    ? { teamCategory: PRODUCTION_TEAM_CATEGORY_ASSY, plantCode: plant }
    : { teamCategory: PRODUCTION_TEAM_CATEGORY_ASSY }
}

/**
 * 制二（PCBA）班组 options：teamCategory=P + 可选工厂
 * @param plantCode 工厂代码
 * @returns api-params
 */
export function buildEcMfg2ProductionTeamOptionsParams(
  plantCode?: string | null,
): { teamCategory: string; plantCode?: string } {
  const plant = String(plantCode ?? '').trim()
  return plant
    ? { teamCategory: PRODUCTION_TEAM_CATEGORY_PCBA, plantCode: plant }
    : { teamCategory: PRODUCTION_TEAM_CATEGORY_PCBA }
}
