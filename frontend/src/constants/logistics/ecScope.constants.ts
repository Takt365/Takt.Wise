// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/constants/logistics
// 文件名称：ecScope.constants.ts
// 创建时间：2026-09-09
// 创建人：Takt365(Cursor AI)
// 功能描述：实施范围字典值（对齐后端 TaktEcScopeConstants / TaktEcControlScopeConstants.ResolveForDept）
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

/** 控制范围：1=按成品 2=按机种 3=按设变 */
export const TaktEcControlScope = {
  /** 按成品 */
  ByFinishedProduct: 1,
  /** 按机种 */
  ByModel: 2,
  /** 按设变 */
  ByEngineeringChange: 3,
} as const

/**
 * 主表控制范围可选项。全仕向可选按成品/按机种/按设变；部管仅按成品；内部/技术仅按设变
 * @param ecImplScope 实施范围
 * @returns {allowed: readonly number[]; defaultValue: number} 可选项与默认值
 */
export function taktEcControlScopeChoices(ecImplScope: number): { allowed: readonly number[]; defaultValue: number } {
  if (ecImplScope === TaktEcScope.MaterialControl) {
    return {
      allowed: [TaktEcControlScope.ByFinishedProduct],
      defaultValue: TaktEcControlScope.ByFinishedProduct,
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
    defaultValue: TaktEcControlScope.ByFinishedProduct,
  }
}

/**
 * 控制范围不在当前实施范围允许集合内时，改为该范围的默认项（对齐后端 Normalize）
 * @param ecImplScope 实施范围
 * @param ecControlScope 当前控制范围
 * @returns {number} 修正后的控制范围
 */
export function taktNormalizeEcControlScope(ecImplScope: number, ecControlScope: unknown): number {
  const { allowed, defaultValue } = taktEcControlScopeChoices(ecImplScope)
  const num = typeof ecControlScope === 'number' ? ecControlScope : Number(ecControlScope)
  return allowed.includes(num) ? num : defaultValue
}

/**
 * 执行部门实际控制范围（对齐后端 ResolveForDept：采购/受检固定按设变；制技固定按机种；其余跟随 Normalize）
 * @param deptCode 部门编码
 * @param ecImplScope 实施范围
 * @param masterDeptScope 主表控制范围
 * @returns {number} 该部门控制范围
 */
export function taktResolveControlScopeForDept(
  deptCode: string,
  ecImplScope: number,
  masterDeptScope: number,
): number {
  if (deptCode === TaktEcDeptCodes.Pur || deptCode === TaktEcDeptCodes.Iqc) {
    return TaktEcControlScope.ByEngineeringChange
  }
  if (deptCode === TaktEcDeptCodes.Me) {
    return TaktEcControlScope.ByModel
  }
  return taktNormalizeEcControlScope(ecImplScope, masterDeptScope)
}

/**
 * 部门行控制范围展示（对齐 ResolveForDept；一律字典值）
 * @param deptCode 部门编码
 * @param ecImplScope 实施范围
 * @param masterDeptScope 主表控制范围
 * @returns {{ mode: 'dict'; value: number }} 展示
 */
export function taktEcDeptDedupDisplay(
  deptCode: string,
  ecImplScope: number,
  masterDeptScope: number,
): { mode: 'dict'; value: number } {
  return {
    mode: 'dict',
    value: taktResolveControlScopeForDept(deptCode, ecImplScope, masterDeptScope),
  }
}

/**
 * 部门物料编码组回填说明（locale scopeDepts.* 键名；无则不展示）
 * 生管/制二/制一/品管：旧+新；部管：新物料非C003采购件；物料：新物料C003采购件；采购/受检：新物料采购/检验
 * @param deptCode 部门编码
 * @returns {string | null} locale 键后缀
 */
export function taktEcDeptMaterialGroupHintKey(deptCode: string): string | null {
  switch (deptCode) {
    case TaktEcDeptCodes.Pc:
    case TaktEcDeptCodes.Mfg2:
    case TaktEcDeptCodes.Mfg1:
    case TaktEcDeptCodes.Qc:
      return 'oldNewMaterialGroup'
    case TaktEcDeptCodes.Mc:
      return 'mcNewMaterialGroup'
    case TaktEcDeptCodes.Mtl:
      return 'mtlNewMaterialGroup'
    case TaktEcDeptCodes.Pur:
      return 'purNewMaterialGroup'
    case TaktEcDeptCodes.Iqc:
      return 'iqcNewMaterialGroup'
    default:
      return null
  }
}

/** 技术课表单「实施范围」Tab 的部门顺序（生管、采购、受检、部管、物料、制二、制一、品管、制技） */
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
 * 实施范围=部管时，允许按 Z0 填报更新的执行部门 slug（采购/生管/受检/部管/物料）
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
export function isEcEolExecContent(content: unknown): boolean {
  const value = String(content ?? '').trim()
  if (!value) {
    return false
  }
  return value.toUpperCase().endsWith('-EOL')
}
