// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/constants/logistics
// 文件名称：ecScope.constants.ts
// 创建时间：2026-09-09
// 创建人：Takt365(Cursor AI)
// 功能描述：实施范围字典值（对齐后端 TaktEcScopeConstants / logistics_manufacturing_ec_impl_scope_category）
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
export const TaktEcControlScope = {
  /** 按成品 */
  ByFinishedProduct: 1,
  /** 按机种 */
  ByModel: 2,
  /** 按设变 */
  ByEngineeringChange: 3,
} as const

/**
 * 主表部门实施范围可选项。全仕向可选；部管仅按机种；内部/技术仅按设变
 * @param ecImplScope 实施范围
 * @returns {allowed: readonly number[]; defaultValue: number} 可选项与默认值
 */
export function taktEcControlScopeChoices(ecImplScope: number): { allowed: readonly number[]; defaultValue: number } {
  if (ecImplScope === TaktEcScope.MaterialControl) {
    return {
      allowed: [TaktEcControlScope.ByModel],
      defaultValue: TaktEcControlScope.ByModel,
    }
  }
  if (ecImplScope === TaktEcScope.Internal || ecImplScope === TaktEcScope.Technical) {
    return {
      allowed: [TaktEcControlScope.ByEngineeringChange],
      defaultValue: TaktEcControlScope.ByEngineeringChange,
    }
  }
  return {
    allowed: [TaktEcControlScope.ByFinishedProduct, TaktEcControlScope.ByModel, TaktEcControlScope.ByEngineeringChange],
    defaultValue: TaktEcControlScope.ByModel,
  }
}

/**
 * 部门行展示：字典值，或自动填写
 * @param deptCode 部门编码
 * @param ecImplScope 实施范围
 * @param masterDeptScope 主表制二/制一/品管实施范围
 * @returns {{ mode: 'dict'; value: number } | { mode: 'auto' }} 展示
 */
export function taktEcDeptDedupDisplay(
  deptCode: string,
  ecImplScope: number,
  masterDeptScope: number,
): { mode: 'dict'; value: number } | { mode: 'auto' } {
  if (deptCode === TaktEcDeptCodes.Me) {
    return { mode: 'dict', value: TaktEcControlScope.ByModel }
  }
  if (ecImplScope === TaktEcScope.Internal || ecImplScope === TaktEcScope.Technical) {
    return { mode: 'auto' }
  }
  if (ecImplScope === TaktEcScope.MaterialControl) {
    if (deptCode === TaktEcDeptCodes.Mfg2 || deptCode === TaktEcDeptCodes.Mfg1 || deptCode === TaktEcDeptCodes.Qc) {
      return { mode: 'auto' }
    }
    if (deptCode === TaktEcDeptCodes.Pc) {
      return { mode: 'dict', value: TaktEcControlScope.ByModel }
    }
    return { mode: 'dict', value: TaktEcControlScope.ByEngineeringChange }
  }
  const master = taktNormalizeEcControlScope(ecImplScope, masterDeptScope)
  if (deptCode === TaktEcDeptCodes.Pur || deptCode === TaktEcDeptCodes.Iqc || deptCode === TaktEcDeptCodes.Mc || deptCode === TaktEcDeptCodes.Mtl) {
    return { mode: 'dict', value: TaktEcControlScope.ByEngineeringChange }
  }
  return { mode: 'dict', value: master }
}

/**
 * 部门实施范围不在当前实施范围允许集合内时，改为该范围的默认项
 * @param ecImplScope 实施范围
 * @param ecControlScope 当前部门实施范围
 * @returns {number} 修正后的部门实施范围
 */
export function taktNormalizeEcControlScope(ecImplScope: number, ecControlScope: unknown): number {
  const { allowed, defaultValue } = taktEcControlScopeChoices(ecImplScope)
  const num = typeof ecControlScope === 'number' ? ecControlScope : Number(ecControlScope)
  return allowed.includes(num) ? num : defaultValue
}

/** 技术课表单「实施范围」Tab 的部门顺序（生管、采购、受检、部管、SMT、制二、制一、品管、制技） */
export const EC_SCOPE_FORM_DEPTS: readonly { code: string; labelKey: string }[] = [
  { code: TaktEcDeptCodes.Pc, labelKey: 'logistics.manufacturing.engineering-change.ec-te.page.scopeDepts.pc' },
  { code: TaktEcDeptCodes.Pur, labelKey: 'logistics.manufacturing.engineering-change.ec-te.page.scopeDepts.pur' },
  { code: TaktEcDeptCodes.Iqc, labelKey: 'logistics.manufacturing.engineering-change.ec-te.page.scopeDepts.iqc' },
  { code: TaktEcDeptCodes.Mc, labelKey: 'logistics.manufacturing.engineering-change.ec-te.page.scopeDepts.mc' },
  { code: TaktEcDeptCodes.Mtl, labelKey: 'logistics.manufacturing.engineering-change.ec-te.page.scopeDepts.mtl' },
  { code: TaktEcDeptCodes.Mfg2, labelKey: 'logistics.manufacturing.engineering-change.ec-te.page.scopeDepts.mfg2' },
  { code: TaktEcDeptCodes.Mfg1, labelKey: 'logistics.manufacturing.engineering-change.ec-te.page.scopeDepts.mfg1' },
  { code: TaktEcDeptCodes.Qc, labelKey: 'logistics.manufacturing.engineering-change.ec-te.page.scopeDepts.qc' },
  { code: TaktEcDeptCodes.Me, labelKey: 'logistics.manufacturing.engineering-change.ec-te.page.scopeDepts.me' },
]

/**
 * 实施范围=部管时，允许按 Z0 填报更新的执行部门 slug（采购/生管/受检/部管/SMT）
 */
export const EC_SCOPE_MATERIAL_CONTROL_UPDATE_SLUGS = new Set([
  'ecpur',
  'ecpc',
  'eciqc',
  'ecmc',
  'ecmtl',
])

/** 执行内容标准前缀（对齐后端 ExecContentPrefix） */
export const EC_EXEC_CONTENT_PREFIX = '实施范围-'

/**
 * 是否为停产自动执行内容（「实施范围-*-EOL」）
 * @param content 执行内容
 * @returns {boolean} 是否 EOL 类文案
 */
/**
 * 根物料描述是否显示。仅全仕向且部门实施范围为按成品；机种组或物料组不显示。
 * @param ecImplScope 实施范围
 * @param ecControlScope 部门实施范围
 * @returns {boolean} 是否显示根物料描述
 */
export function taktShowEcRootMaterialDescription(ecImplScope: unknown, ecControlScope: unknown): boolean {
  return Number(ecImplScope) === TaktEcScope.AllDestination
    && Number(ecControlScope) === TaktEcControlScope.ByFinishedProduct
}

export function isEcEolExecContent(content: unknown): boolean {
  const value = String(content ?? '').trim()
  if (!value) {
    return false
  }
  return value.toUpperCase().endsWith('-EOL')
}