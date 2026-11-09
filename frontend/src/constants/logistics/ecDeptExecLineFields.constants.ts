// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/constants/logistics
// 文件名称：ecDeptExecLineFields.constants.ts
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：各部门执行主表列顺序（对齐 Domain：行号→单号→停产→机种/根物料→[上阶]/旧/新物料组→实施→本课字段→部门；生管/制一/制二/品管含旧新物料编码组）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 各部门执行主键字段（主表首列 / row-key；与 index.vue id-field 一致）
 */
export const EC_DEPT_EXEC_ID_FIELD: Readonly<Record<string, string>> = {
  ecpur: 'ecPurId',
  eciqc: 'ecIqcId',
  ecmc: 'ecMcId',
  ecmtl: 'ecMtlId',
  ecpc: 'ecPcId',
  ecmfg1: 'ecMfg1Id',
  ecmfg2: 'ecMfg2Id',
  ecqc: 'ecQcId',
  ecme: 'ecMeId',
}

/**
 * 前列：行号、设变单号（对齐实体 LineNumber → EcCode）
 */
const EC_DEPT_EXEC_LINE_IDENTITY = ['lineNumber', 'ecCode'] as const

/**
 * 生管/制技头段：停产状态 → 机种编码组 → 根物料编码组（描述在业务字段之后）
 */
const EC_DEPT_EXEC_LINE_HEAD_DISC_MODEL = [
  'discontinuedStatus',
  'ecModelCodeGroup',
  'ecRootMaterialCodeGroup',
] as const

/**
 * 部管/SMT：停产状态 → 机种编码组 → 根物料编码组
 */
const EC_DEPT_EXEC_LINE_HEAD_DISC_MODEL_GROUP = [
  'discontinuedStatus',
  'ecModelCodeGroup',
  'ecRootMaterialCodeGroup',
] as const

/**
 * 实施区：是否实施
 */
const EC_DEPT_EXEC_LINE_EXEC_HEAD = ['isImplemented'] as const

/**
 * 生管冗余：预定日期、预定批次（部管/制一/制二/SMT 只读展示）
 */
const EC_DEPT_EXEC_LINE_SCHEDULED_REDUNDANT = ['scheduledDate', 'scheduledBatch'] as const

/** 含生管 scheduled 冗余展示字段的部门 slug（除采购/受检/品管） */
const EC_DEPT_EXEC_HAS_SCHEDULED_REDUNDANT = new Set([
  'ecmc',
  'ecmtl',
  'ecmfg1',
  'ecmfg2',
])

/**
 * 执行内容
 */
const EC_DEPT_EXEC_LINE_EXEC_CONTENT = ['execContent'] as const

/**
 * 实施范围、部门实施范围（各部门执行表都有）
 */
const EC_DEPT_EXEC_LINE_SCOPE = ['ecImplScope', 'ecControlScope'] as const

/**
 * 尾段：部门编码、部门名称
 */
const EC_DEPT_EXEC_LINE_TAIL_DEPT = ['deptCode', 'deptName'] as const

/** 各课实施/业务字段（在执行内容之前；根物料描述仍在执行内容之后） */
const EC_DEPT_EXEC_LINE_IMPL = {
  /** TaktEcPur：进货日期（前端表单必填；后端可空）；旧品结余由「旧品是否结余」为是时条件必填 */
  ecpur: ['inboundDate'],
  /** TaktEcPc：旧品结余由「旧品是否结余」为是时条件必填（ntext 多行格式） */
  ecpc: [],
  /** TaktEcIqc：检验日期 */
  eciqc: ['inspectionDate'],
  /** TaktEcMc：实施夹在预定批次与出库日期之间 */
  ecmc: ['isImplemented', 'outboundDate', 'outboundBatch'],
  /** TaktEcMtl：实施夹在预定批次与出库日期之间 */
  ecmtl: ['isImplemented', 'outboundDate', 'outboundBatch'],
  /** TaktEcMfg2：实施夹在预定批次与生产班组之间 */
  ecmfg2: ['isImplemented', 'productionTeam', 'productionDate', 'productionBatch'],
  /** TaktEcMfg1：实施夹在预定批次与生产班组之间 */
  ecmfg1: ['isImplemented', 'productionTeam', 'productionDate', 'productionBatch'],
  /** TaktEcQc：生产班组/日期/批次（制一冗余只读）→ 实施 → 检验日期/检验批次（自填） */
  ecqc: ['productionTeam', 'productionDate', 'productionBatch', 'isImplemented', 'inspectionDate', 'inspectionBatch'],
  /** TaktEcMe：更新SOP、担当、SOP日期（前端表单必填；后端可空） */
  ecme: ['isSopUpdated', 'sopLeader', 'sopDate'],
} as const

/** 采购单/受检检验单缺省编码（生管 Po残、采购执行、受检执行共用；仅表单默认） */
export const EC_DEPT_PO_DEFAULT_ORDER = '4300000000'

/** 采购单/受检检验单缺省序号 4 位（生管 Po残、采购执行、受检执行共用；仅表单默认） */
export const EC_DEPT_PO_DEFAULT_LINE = '0010'

/** 采购执行内容缺省供应商（仅表单默认） */
export const EC_PUR_DEFAULT_SUPPLIER = 'H200000'

/** 生管 Po残缺省数量（仅表单默认） */
export const EC_PC_PO_DEFAULT_QTY = '0'

/** 生管旧品结余缺省结余数量（仅表单默认） */
export const EC_PC_OLD_PART_DEFAULT_QTY = '0'

/** 生管旧品结余缺省处理方式（字典 logistics_manufacturing_ec_old_part_disposition；4=消耗） */
export const EC_PC_OLD_PART_DEFAULT_DISPOSITION = '4'

/**
 * 更新表单前端必填：公共实施字段 + 本课填报字段（实体列可空，前端强制非空；不含 remark）
 * 生管另含可编辑的预定日期/预定批次
 * @param slug 实体 slug
 * @returns {readonly string[]} 必填字段
 */
export function getEcDeptExecFillRequiredFields(slug: string): readonly string[] {
  const impl =
    slug in EC_DEPT_EXEC_LINE_IMPL
      ? EC_DEPT_EXEC_LINE_IMPL[slug as keyof typeof EC_DEPT_EXEC_LINE_IMPL]
      : []
  const pcSchedule = slug === 'ecpc' ? (['scheduledDate', 'scheduledBatch'] as const) : []
  // 品管/制一/制二/部管/物料：实施已夹在预定批次与出库/生产（或生产批次与检验日期）之间，不再前置
  const implemented =
    slug === 'ecme' || slug === 'ecqc' || slug === 'ecmfg1' || slug === 'ecmfg2' || slug === 'ecmc' || slug === 'ecmtl'
      ? []
      : (['isImplemented'] as const)
  // 品管班组/生产日期/生产批次冗余自制一（只读），不列入本课必填；实施/检验日期/检验批次为本课必填
  const qcMfg1Redundant = new Set(['productionTeam', 'productionDate', 'productionBatch'])
  const fillImpl = impl.filter(
    (field) => !(slug === 'ecqc' && qcMfg1Redundant.has(field)),
  )
  // 顺序：实施 → 预定(生管) → 本课填报 → 执行内容
  return [...implemented, ...pcSchedule, ...fillImpl, 'execContent']
}

/**
 * 旧品结余列：生管、采购各自可改（去重粒不同，互不带入）
 */
const EC_DEPT_EXEC_LINE_OLD_PART_REDUNDANT = ['ecOldPartBalance'] as const

/** 含旧品结余本表字段的部门 slug */
const EC_DEPT_EXEC_HAS_OLD_PART_REDUNDANT = new Set(['ecpur', 'ecpc'])

/**
 * 采购：停产 + 机种/根物料/新物料编码组 + 新采购类型（无新品仓库）
 */
const EC_DEPT_EXEC_LINE_MID_MP = [
  'discontinuedStatus',
  'ecModelCodeGroup',
  'ecRootMaterialCodeGroup',
  'ecNewMaterialCodeGroup',
  'ecOldMaterialCodeGroup',
  'ecNewPurchaseType',
] as const

/**
 * 受检：停产 + 机种/根物料/新物料编码组 + 新品检验（无新品仓库）
 */
const EC_DEPT_EXEC_LINE_MID_IQC = [
  'discontinuedStatus',
  'ecModelCodeGroup',
  'ecRootMaterialCodeGroup',
  'ecNewMaterialCodeGroup',
  'ecNewRequiresInspection',
] as const

/**
 * 部管：新物料编码组 / 新采购类型 / 新品仓库（停产与机种/根物料编码已在头段）
 */
const EC_DEPT_EXEC_LINE_MID_MC = [
  'ecNewMaterialCodeGroup',
  'ecNewPurchaseType',
  'ecNewWarehouse',
] as const

/**
 * SMT：新物料编码组 → 上阶 → 新采购类型/新品仓库（停产与机种/根物料编码已在头段）
 */
const EC_DEPT_EXEC_LINE_MID_SMT = [
  'ecNewMaterialCodeGroup',
  'ecParentMaterialCode',
  'ecParentMaterialDescription',
  'ecNewPurchaseType',
  'ecNewWarehouse',
] as const

/**
 * 制二：停产 + 机种/根物料 → 上阶 → 旧/新物料编码组（无机种单码、无新物料描述）
 */
const EC_DEPT_EXEC_LINE_MID_PCBA = [
  'discontinuedStatus',
  'ecModelCodeGroup',
  'ecRootMaterialCodeGroup',
  'ecParentMaterialCode',
  'ecParentMaterialDescription',
  'ecOldMaterialCodeGroup',
  'ecNewMaterialCodeGroup',
] as const

/**
 * 品管/制一：停产 + 机种/根物料/新/旧物料编码组（无机种单码、无新物料描述）
 */
const EC_DEPT_EXEC_LINE_MID_QA = [
  'discontinuedStatus',
  'ecModelCodeGroup',
  'ecRootMaterialCodeGroup',
  'ecNewMaterialCodeGroup',
  'ecOldMaterialCodeGroup',
] as const

/**
 * 生管：头段（停产/机种/根物料）之后补新/旧物料编码组
 */
const EC_DEPT_EXEC_LINE_MID_PC = [
  'ecNewMaterialCodeGroup',
  'ecOldMaterialCodeGroup',
] as const

/**
 * 制技：头段之后无额外物料上下文
 */
const EC_DEPT_EXEC_LINE_MID_EMPTY = [] as const

const EC_DEPT_EXEC_LINE_MID_BY_SLUG: Record<string, readonly string[]> = {
  ecpur: EC_DEPT_EXEC_LINE_MID_MP,
  eciqc: EC_DEPT_EXEC_LINE_MID_IQC,
  ecmc: EC_DEPT_EXEC_LINE_MID_MC,
  ecmtl: EC_DEPT_EXEC_LINE_MID_SMT,
  ecmfg2: EC_DEPT_EXEC_LINE_MID_PCBA,
  ecmfg1: EC_DEPT_EXEC_LINE_MID_QA,
  ecqc: EC_DEPT_EXEC_LINE_MID_QA,
  ecme: EC_DEPT_EXEC_LINE_MID_EMPTY,
  ecpc: EC_DEPT_EXEC_LINE_MID_PC,
}

/** 含机种/根物料编码/停产头段的部门（采购/受检、部管/SMT 除外） */
const EC_DEPT_EXEC_HAS_DISC_MODEL = new Set([
  'ecpc',
  'ecme',
])

/** 部管/SMT：机种 + 根物料编码组，无根物料描述 */
const EC_DEPT_EXEC_HAS_ROOT_GROUP = new Set(['ecmc', 'ecmtl'])

/**
 * 编辑表单的实施值。字典 sys_yes_no：1=是。行上 0 是列默认否，打开表单时显示是。
 * @param value 行上的实施值
 * @returns {number} 表单绑定值
 */
/** 部门批次输入上限（预定/检验批次列 Length=20；部管/物料出库批次传 40） */
export const EC_DEPT_BATCH_MAX_LENGTH = 20

/** 生产批次输入上限（prod_batch / production_batch Length=40） */
export const EC_PRODUCTION_BATCH_MAX_LENGTH = 40

/**
 * 部门批次输入：只保留大写字母与数字，并截断到上限
 * @param value 输入值
 * @param maxLength 截断长度；生产批次传 40，其余批次默认 20
 * @returns {string} 规范化后的批次
 */
export function taktDeptBatchInput(value: string | null | undefined, maxLength = EC_DEPT_BATCH_MAX_LENGTH): string {
  const limit = maxLength > 0 ? maxLength : EC_DEPT_BATCH_MAX_LENGTH
  return String(value ?? '').toUpperCase().replace(/[^A-Z0-9]/g, '').slice(0, limit)
}

/**
 * 执行内容前缀：{设变单号}-{根物料组}
 * @param ecCode 设变单号
 * @param rootMaterialGroup 根物料编码组
 * @returns {string} 前缀；皆空则空串
 */
export function taktEcExecContentPrefix(
  ecCode: string | null | undefined,
  rootMaterialGroup: string | null | undefined,
): string {
  const code = String(ecCode ?? '').trim()
  const root = String(rootMaterialGroup ?? '').trim()
  if (!code && !root) {
    return ''
  }
  if (!code) {
    return root
  }
  return `${code}-${root}`
}

/**
 * 生管/制一/制二等执行内容默认：{设变号}-{根物料组}，从{批次}开始实施。；批次空则空串
 * @param ecCode 设变单号
 * @param rootMaterialGroup 根物料编码组
 * @param batch 批次/单号
 * @returns {string} 默认执行内容
 */
export function taktEcDefaultExecContent(
  ecCode: string | null | undefined,
  rootMaterialGroup: string | null | undefined,
  batch: string | null | undefined,
): string {
  const text = String(batch ?? '').trim()
  if (!text) {
    return ''
  }
  const head = taktEcExecContentPrefix(ecCode, rootMaterialGroup)
  return head ? `${head}，从${text}开始实施。` : `从${text}开始实施。`
}

/**
 * 品管执行内容默认：{设变号}-{根物料组}，{检验批次}已实施，判定：PASS。；批次空则空串
 * 例：EA8169-09FRAV4000，FRAV426X已实施，判定：PASS。
 * @param ecCode 设变单号
 * @param rootMaterialGroup 根物料编码组
 * @param inspectionBatch 检验批次
 * @returns {string} 默认执行内容
 */
export function taktEcQcDefaultExecContent(
  ecCode: string | null | undefined,
  rootMaterialGroup: string | null | undefined,
  inspectionBatch: string | null | undefined,
): string {
  const text = String(inspectionBatch ?? '').trim()
  if (!text) {
    return ''
  }
  const head = taktEcExecContentPrefix(ecCode, rootMaterialGroup)
  return head ? `${head}，${text}已实施，判定：PASS。` : `${text}已实施，判定：PASS。`
}

/**
 * 采购/受检/部管/物料执行内容：
 * {设变号}-{根物料组}-{新物料}-{课别字段}，从{日期或批次}开始实施。
 * 课别字段：采购=新采购类型；受检=新品检验；部管/物料=新品仓库
 * fromValue：采购=采购订单发行日期；受检=检验日期；部管/物料=出库批次
 * @param parts 各段取值
 * @returns {string} 默认执行内容；fromValue 空则空串
 */
export function taktEcMaterialScopeExecContent(parts: {
  ecCode?: string | null
  rootMaterialGroup?: string | null
  newMaterialCode?: string | null
  /** 采购=新采购类型；受检=新品检验；部管/物料=新品仓库 */
  scopeValue?: string | number | null
  /** 采购=采购日期；受检=检验日期；部管/物料=出库批次 */
  fromValue?: string | null
}): string {
  const from = String(parts.fromValue ?? '').trim()
  if (!from) {
    return ''
  }
  const code = String(parts.ecCode ?? '').trim()
  const root = String(parts.rootMaterialGroup ?? '').trim()
  const material = String(parts.newMaterialCode ?? '').trim()
  const scopeRaw = parts.scopeValue
  const scope =
    scopeRaw === null || scopeRaw === undefined || scopeRaw === ''
      ? ''
      : String(scopeRaw).trim()
  const head = `${code}-${root}-${material}-${scope}`
  return `${head}，从${from}开始实施。`
}

/**
 * 是否视为自动生成的执行内容，便于改批次时联动覆盖
 * @param content 当前执行内容
 * @returns {boolean} 可被默认文案覆盖则为 true
 */
export function taktEcIsAutoExecContent(content: string | null | undefined): boolean {
  const text = String(content ?? '').trim()
  return (
    !text
    || /^(?:.+，)?从.+开始实施。$/.test(text)
    || /^(?:.+，)?.+已实施，判定：PASS。$/.test(text)
    || /^(?:.+，)?.+已通过检验：PASS。$/.test(text)
    || /^.+-.+-.+-.*，从.+开始实施。$/.test(text)
    || /^从.+开始$/.test(text)
  )
}

/**
 * 预定日期：有值回填，无值清空（不沿用已保存的阶段日期）
 * @param scheduledDate 预定日期
 * @returns {string | undefined} 目标日期
 */
export function taktEcDateFromScheduled(scheduledDate: string | null | undefined): string | undefined {
  const text = String(scheduledDate ?? '').trim()
  return text || undefined
}

/**
 * 预定批次：有值规范化回填，无值清空
 * @param scheduledBatch 预定批次
 * @returns {string} 目标批次
 */
export function taktEcBatchFromScheduled(scheduledBatch: string | null | undefined): string {
  return taktDeptBatchInput(scheduledBatch)
}

export function ecDeptImplementedFormValue(value: number | null | undefined): number {
  if (value == null || value === 0) {
    return 1
  }
  return value
}

/** 执行部门实体 slug */
export type EcDeptExecSlug = keyof typeof EC_DEPT_EXEC_LINE_IMPL

/**
 * 取指定部门执行主表字段
 * 顺序：主键 → 行号/单号 → [停产/机种/根物料…] → 物料上下文 → 实施 → 本课业务 → 执行内容 → 部门编码/名称 → 实施/控制范围
 * 品管/制一/制二/部管/物料特例：实施夹在预定批次与出库/生产（或生产批次与检验日期）之间，不走前置 EXEC_HEAD
 * @param slug 实体 slug（ecpur 对应 TaktEcPur）
 * @returns {readonly string[]} 主表列字段
 */
export function getEcDeptExecLineFields(slug: string): readonly string[] {
  const impl =
    slug in EC_DEPT_EXEC_LINE_IMPL
      ? EC_DEPT_EXEC_LINE_IMPL[slug as EcDeptExecSlug]
      : []
  const mid = EC_DEPT_EXEC_LINE_MID_BY_SLUG[slug] ?? EC_DEPT_EXEC_LINE_MID_EMPTY
  const hasDiscModel = EC_DEPT_EXEC_HAS_DISC_MODEL.has(slug)
  const hasRootGroup = EC_DEPT_EXEC_HAS_ROOT_GROUP.has(slug)
  const idField = EC_DEPT_EXEC_ID_FIELD[slug]
  const skipExecHead =
    slug === 'ecme' || slug === 'ecqc' || slug === 'ecmfg1' || slug === 'ecmfg2' || slug === 'ecmc' || slug === 'ecmtl'
  return [
    ...(idField ? [idField] : []),
    ...EC_DEPT_EXEC_LINE_IDENTITY,
    ...(hasRootGroup ? EC_DEPT_EXEC_LINE_HEAD_DISC_MODEL_GROUP : []),
    ...(hasDiscModel ? EC_DEPT_EXEC_LINE_HEAD_DISC_MODEL : []),
    ...mid,
    ...(skipExecHead ? [] : EC_DEPT_EXEC_LINE_EXEC_HEAD),
    ...(EC_DEPT_EXEC_HAS_SCHEDULED_REDUNDANT.has(slug) ? EC_DEPT_EXEC_LINE_SCHEDULED_REDUNDANT : []),
    ...(slug === 'ecpc' ? EC_DEPT_EXEC_LINE_SCHEDULED_REDUNDANT : []),
    ...impl,
    ...(slug === 'ecme' ? (['sopLeader'] as const) : []),
    ...(EC_DEPT_EXEC_HAS_OLD_PART_REDUNDANT.has(slug) ? EC_DEPT_EXEC_LINE_OLD_PART_REDUNDANT : []),
    // 执行内容紧挨部门编码之前（对齐 Domain）
    ...EC_DEPT_EXEC_LINE_EXEC_CONTENT,
    ...EC_DEPT_EXEC_LINE_TAIL_DEPT,
    ...EC_DEPT_EXEC_LINE_SCOPE,
  ]
}
