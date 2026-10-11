// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/constants/logistics
// 文件名称：ecScope.constants.ts
// 创建时间：2026-09-09
// 创建人：Takt365(Cursor AI)
// 功能描述：设变实施范围字典值（对齐后端 TaktEcScopeConstants / logistics_manufacturing_ec_scope_category）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { TaktEcDeptCodes } from '@/constants/logistics/ecDeptCodes.constants'

/** 实施范围：1=全仕向 2=部管 3=内部 4=技术 */
export const TaktEcScope = {
  /** 全仕向 */
  AllDestination: 1,
  /** 部管 */
  MaterialControl: 2,
  /** 内部 */
  Internal: 3,
  /** 技术 */
  Technical: 4,
} as const

/** 部门实施范围：1=按成品 2=按机种 3=按设变 */
export const TaktEcDeptScope = {
  /** 按成品 */
  ByFinishedProduct: 1,
  /** 按机种 */
  ByModel: 2,
  /** 按设变 */
  ByEngineeringChange: 3,
} as const

/**
 * 主表制二/制一/品管实施范围可选项
 * 全仕向：按成品/按机种/按设变，默认按机种；部管/内部/技术：仅按设变
 * @param ecScope 实施范围
 * @returns {allowed: readonly number[]; defaultValue: number} 可选项与默认值
 */
export function taktEcDeptScopeChoices(ecScope: number): { allowed: readonly number[]; defaultValue: number } {
  if (ecScope === TaktEcScope.MaterialControl || ecScope === TaktEcScope.Internal || ecScope === TaktEcScope.Technical) {
    return {
      allowed: [TaktEcDeptScope.ByEngineeringChange],
      defaultValue: TaktEcDeptScope.ByEngineeringChange,
    }
  }
  return {
    allowed: [TaktEcDeptScope.ByFinishedProduct, TaktEcDeptScope.ByModel, TaktEcDeptScope.ByEngineeringChange],
    defaultValue: TaktEcDeptScope.ByModel,
  }
}

/**
 * 部门行展示：字典值，或自动填写
 * @param deptCode 部门编码
 * @param ecScope 实施范围
 * @param masterDeptScope 主表制二/制一/品管实施范围
 * @returns {{ mode: 'dict'; value: number } | { mode: 'auto' }} 展示
 */
export function taktEcDeptDedupDisplay(
  deptCode: string,
  ecScope: number,
  masterDeptScope: number,
): { mode: 'dict'; value: number } | { mode: 'auto' } {
  if (deptCode === TaktEcDeptCodes.Te) {
    return { mode: 'dict', value: TaktEcDeptScope.ByModel }
  }
  if (ecScope === TaktEcScope.Internal || ecScope === TaktEcScope.Technical) {
    return { mode: 'auto' }
  }
  if (ecScope === TaktEcScope.MaterialControl) {
    if (deptCode === TaktEcDeptCodes.Pcba || deptCode === TaktEcDeptCodes.Assy || deptCode === TaktEcDeptCodes.Qa) {
      return { mode: 'auto' }
    }
    return { mode: 'dict', value: TaktEcDeptScope.ByEngineeringChange }
  }
  const master = taktNormalizeEcDeptScope(ecScope, masterDeptScope)
  if (deptCode === TaktEcDeptCodes.Mp || deptCode === TaktEcDeptCodes.Iqc || deptCode === TaktEcDeptCodes.Mc || deptCode === TaktEcDeptCodes.Smt) {
    return { mode: 'dict', value: TaktEcDeptScope.ByEngineeringChange }
  }
  return { mode: 'dict', value: master }
}

/**
 * 部门实施范围不在当前实施范围允许集合内时，改为该范围的默认项
 * @param ecScope 实施范围
 * @param ecDeptScope 当前部门实施范围
 * @returns {number} 修正后的部门实施范围
 */
export function taktNormalizeEcDeptScope(ecScope: number, ecDeptScope: unknown): number {
  const { allowed, defaultValue } = taktEcDeptScopeChoices(ecScope)
  const num = typeof ecDeptScope === 'number' ? ecDeptScope : Number(ecDeptScope)
  return allowed.includes(num) ? num : defaultValue
}

/** 技术课表单「实施范围」Tab 的部门顺序（生管、采购、受检、部管、SMT、制二、制一、品管、制技） */
export const EC_SCOPE_FORM_DEPTS: readonly { code: string; labelKey: string }[] = [
  { code: TaktEcDeptCodes.Pmc, labelKey: 'logistics.manufacturing.engineering-change.ec-gijutsu.page.scopeDepts.pmc' },
  { code: TaktEcDeptCodes.Mp, labelKey: 'logistics.manufacturing.engineering-change.ec-gijutsu.page.scopeDepts.mp' },
  { code: TaktEcDeptCodes.Iqc, labelKey: 'logistics.manufacturing.engineering-change.ec-gijutsu.page.scopeDepts.iqc' },
  { code: TaktEcDeptCodes.Mc, labelKey: 'logistics.manufacturing.engineering-change.ec-gijutsu.page.scopeDepts.mc' },
  { code: TaktEcDeptCodes.Smt, labelKey: 'logistics.manufacturing.engineering-change.ec-gijutsu.page.scopeDepts.smt' },
  { code: TaktEcDeptCodes.Pcba, labelKey: 'logistics.manufacturing.engineering-change.ec-gijutsu.page.scopeDepts.pcba' },
  { code: TaktEcDeptCodes.Assy, labelKey: 'logistics.manufacturing.engineering-change.ec-gijutsu.page.scopeDepts.assy' },
  { code: TaktEcDeptCodes.Qa, labelKey: 'logistics.manufacturing.engineering-change.ec-gijutsu.page.scopeDepts.qa' },
  { code: TaktEcDeptCodes.Te, labelKey: 'logistics.manufacturing.engineering-change.ec-gijutsu.page.scopeDepts.te' },
]

/**
 * 实施范围=部管时，允许按 Z0 填报更新的执行部门 slug（采购/生管/受检/部管/SMT）
 */
export const EC_SCOPE_MATERIAL_CONTROL_UPDATE_SLUGS = new Set([
  'eckoubai',
  'ecseikan',
  'ecukeken',
  'ecbukan',
  'ecsmt',
])

/** 执行内容标准前缀（对齐后端 ExecContentPrefix） */
export const EC_EXEC_CONTENT_PREFIX = '实施范围-'

/** 历史执行内容前缀（读入时规范为 EC_EXEC_CONTENT_PREFIX） */
export const EC_LEGACY_EXEC_CONTENT_PREFIX = '管理区分-'

/**
 * 历史前缀「管理区分-」统一为「实施范围-」（对齐后端 ReplaceLegacyExecContentPrefix）
 * @param content 执行内容
 * @returns {string} 前缀规范后的文案
 */
export function replaceLegacyEcExecContentPrefix(content: unknown): string {
  const value = String(content ?? '').trim()
  if (!value) {
    return ''
  }
  if (value.startsWith(EC_LEGACY_EXEC_CONTENT_PREFIX)) {
    return EC_EXEC_CONTENT_PREFIX + value.slice(EC_LEGACY_EXEC_CONTENT_PREFIX.length)
  }
  return value
}

/**
 * 是否为停产自动执行内容（裸 EOL 或「实施范围-*-EOL」；历史「管理区分-」先规范）
 * @param content 执行内容
 * @returns {boolean} 是否 EOL 类文案
 */
export function isEcEolExecContent(content: unknown): boolean {
  const value = replaceLegacyEcExecContentPrefix(content)
  if (!value) {
    return false
  }
  const upper = value.toUpperCase()
  return upper === 'EOL' || upper.endsWith('-EOL')
}