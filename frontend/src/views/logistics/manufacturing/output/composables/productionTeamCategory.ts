// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/output/composables
// 文件名称：productionTeamCategory.ts
// 功能描述：生产班组分类 DictValue（与 TaktProductionTeam.TeamCategory / logistics_manufacturing_team_category 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 组立班组（字典 logistics_manufacturing_team_category；A）
 */
export const PRODUCTION_TEAM_CATEGORY_ASSY = 'A'

/**
 * PCBA 班组（字典 logistics_manufacturing_team_category；P）
 */
export const PRODUCTION_TEAM_CATEGORY_PCBA = 'P'

/**
 * 组立日报等：TaktProductionTeams/options 仅拉组立班组（无工厂时仅分类）
 */
export const ASSY_PRODUCTION_TEAM_OPTIONS_PARAMS = {
  teamCategory: PRODUCTION_TEAM_CATEGORY_ASSY,
} as const

/**
 * PCBA 日报等：TaktProductionTeams/options 仅拉 PCBA 班组（无工厂时仅分类）
 */
export const PCBA_PRODUCTION_TEAM_OPTIONS_PARAMS = {
  teamCategory: PRODUCTION_TEAM_CATEGORY_PCBA,
} as const

/**
 * 组立班组 options 查询参数（含工厂过滤）
 * @param plantCode 工厂代码（空则不传 plantCode）
 * @returns api-params
 */
export function buildAssyProductionTeamOptionsParams(
  plantCode?: string | null
): { teamCategory: string; plantCode?: string } {
  const plant = String(plantCode ?? '').trim()
  return plant
    ? { teamCategory: PRODUCTION_TEAM_CATEGORY_ASSY, plantCode: plant }
    : { teamCategory: PRODUCTION_TEAM_CATEGORY_ASSY }
}

/**
 * PCBA 班组 options 查询参数（含工厂过滤）
 * @param plantCode 工厂代码（空则不传 plantCode）
 * @returns api-params
 */
export function buildPcbaProductionTeamOptionsParams(
  plantCode?: string | null
): { teamCategory: string; plantCode?: string } {
  const plant = String(plantCode ?? '').trim()
  return plant
    ? { teamCategory: PRODUCTION_TEAM_CATEGORY_PCBA, plantCode: plant }
    : { teamCategory: PRODUCTION_TEAM_CATEGORY_PCBA }
}
