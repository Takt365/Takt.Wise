// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktFlowFormBinding.ts
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：流程表单 RelatedFormField 解析与序列化（对齐后端 TaktFlowFormBindingHelper）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 业务状态列与流程终态映射（对应 RelatedFormField.business） */
export interface TaktFlowFormBusinessBinding {
  businessStatusColumn?: string
  statusInProgress?: number
  statusApproved?: number
  statusRejected?: number
  statusCancelled?: number
  submitAllowedBusinessStatuses?: number[]
}

/** 单字段映射行 */
export interface TaktFlowFormFieldMappingRow {
  dbColumnName?: string
  csharpColumnName?: string
  columnDescription?: string
  dataType?: string
  length?: number
  decimalDigits?: number
  isRequired?: number
  displayType?: string
  dictTypeCode?: string
  csharpType?: string
}

/** RelatedFormField 解析结果 */
export interface TaktFlowFormBindingRoot {
  fields: TaktFlowFormFieldMappingRow[]
  business?: TaktFlowFormBusinessBinding
}

/**
 * 蛇形列名转 camelCase（FrmData 字段名，与后端种子 employeeId 对齐）
 * @param name 数据库列名
 * @returns camelCase 字段名
 */
export function snakeColumnToCamelCase(name: string | undefined): string {
  if (!name?.trim()) return ''
  const parts = name.trim().split(/[_\s]+/).filter(Boolean)
  if (parts.length === 0) return ''
  const first = parts[0].toLowerCase()
  const rest = parts.slice(1).map((p) => p.charAt(0).toUpperCase() + p.slice(1).toLowerCase()).join('')
  return first + rest
}

/**
 * 解析 RelatedFormField JSON（纯数组或 { fields, business }）
 * @param json RelatedFormField 字符串
 * @returns 绑定根对象
 */
export function parseRelatedFormField(json: string | undefined | null): TaktFlowFormBindingRoot {
  if (!json?.trim()) {
    return { fields: [] }
  }
  try {
    const parsed = JSON.parse(json) as unknown
    if (Array.isArray(parsed)) {
      return { fields: parsed as TaktFlowFormFieldMappingRow[] }
    }
    if (parsed && typeof parsed === 'object') {
      const obj = parsed as Record<string, unknown>
      const rawFields = obj.fields ?? obj.Fields
      const rawBusiness = obj.business ?? obj.Business
      const fields = Array.isArray(rawFields) ? (rawFields as TaktFlowFormFieldMappingRow[]) : []
      const business =
        rawBusiness && typeof rawBusiness === 'object'
          ? normalizeBusinessBinding(rawBusiness as Record<string, unknown>)
          : undefined
      return { fields, business }
    }
  } catch {
    return { fields: [] }
  }
  return { fields: [] }
}

/**
 * 序列化 RelatedFormField（有 business 时输出对象，否则仅 fields 数组）
 * @param fields 字段映射
 * @param business 可选业务绑定
 * @returns JSON 字符串
 */
export function buildRelatedFormFieldJson(
  fields: TaktFlowFormFieldMappingRow[],
  business?: TaktFlowFormBusinessBinding | null
): string {
  const normalizedFields = fields ?? []
  const hasBusiness =
    business != null
    && (
      business.businessStatusColumn?.trim()
      || business.statusInProgress != null
      || business.statusApproved != null
      || business.statusRejected != null
      || business.statusCancelled != null
      || (business.submitAllowedBusinessStatuses?.length ?? 0) > 0
    )
  if (!hasBusiness) {
    return JSON.stringify(normalizedFields)
  }
  return JSON.stringify({
    fields: normalizedFields,
    business: {
      businessStatusColumn: business.businessStatusColumn?.trim() || undefined,
      statusInProgress: business.statusInProgress,
      statusApproved: business.statusApproved,
      statusRejected: business.statusRejected,
      statusCancelled: business.statusCancelled,
      submitAllowedBusinessStatuses: business.submitAllowedBusinessStatuses?.length
        ? business.submitAllowedBusinessStatuses
        : undefined
    }
  })
}

/**
 * 规范化 business 绑定对象
 * @param raw 原始对象
 * @returns 业务绑定
 */
function normalizeBusinessBinding(raw: Record<string, unknown>): TaktFlowFormBusinessBinding {
  const col = raw.businessStatusColumn ?? raw.BusinessStatusColumn
  const allowed = raw.submitAllowedBusinessStatuses ?? raw.SubmitAllowedBusinessStatuses
  const binding: TaktFlowFormBusinessBinding = {
    businessStatusColumn: col != null ? String(col) : undefined,
    statusInProgress: parseOptionalInt(raw.statusInProgress ?? raw.StatusInProgress),
    statusApproved: parseOptionalInt(raw.statusApproved ?? raw.StatusApproved),
    statusRejected: parseOptionalInt(raw.statusRejected ?? raw.StatusRejected),
    statusCancelled: parseOptionalInt(raw.statusCancelled ?? raw.StatusCancelled)
  }
  if (Array.isArray(allowed)) {
    binding.submitAllowedBusinessStatuses = allowed
      .map((v) => (typeof v === 'number' ? v : parseInt(String(v), 10)))
      .filter((n) => !Number.isNaN(n))
  }
  return binding
}

/**
 * 解析可选整数
 * @param value 输入
 * @returns 整数或 undefined
 */
function parseOptionalInt(value: unknown): number | undefined {
  if (value == null || value === '') return undefined
  const n = typeof value === 'number' ? value : parseInt(String(value), 10)
  return Number.isNaN(n) ? undefined : n
}
