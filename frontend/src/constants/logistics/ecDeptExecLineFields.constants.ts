// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/constants/logistics
// 文件名称：ecDeptExecLineFields.constants.ts
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：各部门执行主表列顺序（对齐 Domain：行号→单号→停产→机种/根物料→业务上下文→实施→本课字段→根物料描述→部门；部管/SMT 为根物料编码组）
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
 * 部管/SMT：停产状态 → 机种编码组 → 根物料编码组 → 根物料描述
 */
const EC_DEPT_EXEC_LINE_HEAD_DISC_MODEL_GROUP = [
  'discontinuedStatus',
  'ecModelCodeGroup',
  'ecRootMaterialCodeGroup',
  'ecRootMaterialDescription',
] as const

/**
 * 实施区：是否实施
 */
const EC_DEPT_EXEC_LINE_EXEC_HEAD = ['isImplemented'] as const

/**
 * 生管冗余：预定日期、预定批次（部管/制一/制二/SMT 只读展示）
 */
const EC_DEPT_EXEC_LINE_SCHEDULED_REDUNDANT = ['scheduledDate', 'scheduledBatch'] as const

/** 含生管 scheduled 冗余展示字段的部门 slug（除采购/受检） */
const EC_DEPT_EXEC_HAS_SCHEDULED_REDUNDANT = new Set([
  'ecmc',
  'ecmtl',
  'ecmfg1',
  'ecmfg2',
  'ecqc',
])

/**
 * 执行内容
 */
const EC_DEPT_EXEC_LINE_EXEC_CONTENT = ['execContent'] as const

/**
 * 根物料描述（实体中位于本课业务字段之后、部门之前）
 */
const EC_DEPT_EXEC_LINE_ROOT_DESC = ['ecRootMaterialDescription'] as const

/**
 * 实施范围、部门实施范围（各部门执行表都有）
 */
const EC_DEPT_EXEC_LINE_SCOPE = ['ecImplScope', 'ecControlScope'] as const

/**
 * 尾段：部门编码、部门名称
 */
const EC_DEPT_EXEC_LINE_TAIL_DEPT = ['deptCode', 'deptName'] as const

/** 各课实施/业务字段（实体 ExecContent 之后、根物料描述之前） */
const EC_DEPT_EXEC_LINE_IMPL = {
  /** TaktEcPur：采购订单发行日期、供应商、采购订单号码（旧品处理本表可改，见下方展示段） */
  ecpur: ['purchaseOrderIssueDate', 'supplier', 'purchaseOrderCode'],
  /** TaktEcPc：Po残（可空，有值须正则）、结余数量（scheduled 在实施头段；旧品处理本表可改） */
  ecpc: ['poRemainder', 'balanceQuantity'],
  /** TaktEcIqc：受检单号、检验日期 */
  eciqc: ['iqcOrderCode', 'inspectionDate'],
  /** TaktEcMc：出库批次、出库日期 */
  ecmc: ['outboundBatch', 'outboundDate'],
  /** TaktEcMtl：出库批次、出库日期 */
  ecmtl: ['outboundBatch', 'outboundDate'],
  /** TaktEcMfg2：生产班组、生产日期、实施批次 */
  ecmfg2: ['productionTeam', 'productionDate', 'implementationBatch'],
  /** TaktEcMfg1：生产班组、生产日期、实施批次 */
  ecmfg1: ['productionTeam', 'productionDate', 'implementationBatch'],
  /** TaktEcQc：生产班组、检验日期、检验批次、抽样号码 */
  ecqc: ['productionTeam', 'inspectionDate', 'inspectionBatch', 'samplingCode'],
  /** TaktEcMe：更新SOP、SOP日期（担当在列上单独展示，可空） */
  ecme: ['isSopUpdated', 'sopDate'],
} as const

/**
 * 更新表单前端必填：公共实施字段 + 本课填报字段（后端可空，前端强制非空）
 * 生管另含可编辑的预定日期/预定批次；不含 remark
 * @param slug 实体 slug
 * @returns {readonly string[]} 必填字段
 */
export function getEcDeptExecFillRequiredFields(slug: string): readonly string[] {
  const impl =
    slug in EC_DEPT_EXEC_LINE_IMPL
      ? EC_DEPT_EXEC_LINE_IMPL[slug as keyof typeof EC_DEPT_EXEC_LINE_IMPL]
      : []
  const pcSchedule = slug === 'ecpc' ? (['scheduledDate', 'scheduledBatch'] as const) : []
  const implemented = slug === 'ecme' ? [] : (['isImplemented'] as const)
  // Po残可空，有值时由正则校验；不列入必填
  const fillImpl = impl.filter((field) => field !== 'poRemainder')
  return [...implemented, 'execContent', ...pcSchedule, ...fillImpl]
}

/**
 * 旧品处理列：生管、采购各自可改（去重粒不同，互不带入）
 */
const EC_DEPT_EXEC_LINE_OLD_PART_REDUNDANT = ['ecOldPartDisposition'] as const

/** 含旧品处理本表字段的部门 slug */
const EC_DEPT_EXEC_HAS_OLD_PART_REDUNDANT = new Set(['ecpur', 'ecpc'])

/**
 * 采购：停产状态 + 新物料、新品仓库、新采购类型（无机种/根物料编码）
 */
const EC_DEPT_EXEC_LINE_MID_MP = [
  'discontinuedStatus',
  'ecModelCodeGroup',
  'ecRootMaterialCodeGroup',
  'ecRootMaterialDescription',
  'ecNewMaterialCode',
  'ecNewMaterialDescription',
  'ecNewWarehouse',
  'ecNewPurchaseType',
] as const

/**
 * 受检：停产状态 + 新物料、新品仓库、新品检验
 */
const EC_DEPT_EXEC_LINE_MID_IQC = [
  'discontinuedStatus',
  'ecModelCodeGroup',
  'ecRootMaterialCodeGroup',
  'ecRootMaterialDescription',
  'ecNewMaterialCode',
  'ecNewMaterialDescription',
  'ecNewWarehouse',
  'ecNewRequiresInspection',
] as const

/**
 * 部管：新物料 / 新采购类型 / 新品仓库（停产与机种/根物料编码已在头段）
 */
const EC_DEPT_EXEC_LINE_MID_MC = [
  'ecNewMaterialCode',
  'ecNewMaterialDescription',
  'ecNewPurchaseType',
  'ecNewWarehouse',
] as const

/**
 * SMT：上阶 + 新物料（停产与机种/根物料编码已在头段）
 */
const EC_DEPT_EXEC_LINE_MID_SMT = [
  'ecParentMaterialCode',
  'ecParentMaterialDescription',
  'ecNewMaterialCode',
  'ecNewMaterialDescription',
  'ecNewPurchaseType',
  'ecNewWarehouse',
] as const

/**
 * 制二：停产 + 机种编码组 + 根物料编码组 + 新物料/上阶（无机种单码）
 */
const EC_DEPT_EXEC_LINE_MID_PCBA = [
  'discontinuedStatus',
  'ecModelCodeGroup',
  'ecRootMaterialCodeGroup',
  'ecNewMaterialCode',
  'ecNewMaterialCodeGroup',
  'ecNewMaterialDescription',
  'ecParentMaterialCode',
  'ecParentMaterialCodeGroup',
  'ecParentMaterialDescription',
] as const

/**
 * 品管/制一：停产 + 机种编码组 + 根物料编码组（无机种单码）
 */
const EC_DEPT_EXEC_LINE_MID_QA = [
  'discontinuedStatus',
  'ecModelCodeGroup',
  'ecRootMaterialCodeGroup',
] as const

/**
 * 生管/制技：头段之后无额外物料上下文
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
  ecpc: EC_DEPT_EXEC_LINE_MID_EMPTY,
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
/** 部门批次输入上限（与实体批次列 Length 一致） */
export const EC_DEPT_BATCH_MAX_LENGTH = 20

/**
 * 部门批次输入：只保留大写字母，并截断到 20 位
 * @param value 输入值
 * @returns {string} 规范化后的批次
 */
export function taktDeptBatchInput(value: string | null | undefined): string {
  return String(value ?? '').toUpperCase().replace(/[^A-Z]/g, '').slice(0, EC_DEPT_BATCH_MAX_LENGTH)
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
 * 顺序：主键 → 行号/单号 → [停产/机种/根物料编码] → 物料上下文 → 实施/执行内容 → 本课业务 → [根物料描述] → 部门
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
  return [
    ...(idField ? [idField] : []),
    ...EC_DEPT_EXEC_LINE_IDENTITY,
    ...(hasRootGroup ? EC_DEPT_EXEC_LINE_HEAD_DISC_MODEL_GROUP : []),
    ...(hasDiscModel ? EC_DEPT_EXEC_LINE_HEAD_DISC_MODEL : []),
    ...mid,
    ...(slug === 'ecme' ? [] : EC_DEPT_EXEC_LINE_EXEC_HEAD),
    ...(EC_DEPT_EXEC_HAS_SCHEDULED_REDUNDANT.has(slug) ? EC_DEPT_EXEC_LINE_SCHEDULED_REDUNDANT : []),
    ...(slug === 'ecpc' ? EC_DEPT_EXEC_LINE_SCHEDULED_REDUNDANT : []),
    ...EC_DEPT_EXEC_LINE_EXEC_CONTENT,
    ...impl,
    ...(slug === 'ecme' ? (['sopLeader', 'techLeader'] as const) : []),
    ...(EC_DEPT_EXEC_HAS_OLD_PART_REDUNDANT.has(slug) ? EC_DEPT_EXEC_LINE_OLD_PART_REDUNDANT : []),
    ...(hasDiscModel || slug === 'ecqc' || slug === 'ecmfg1' || slug === 'ecmfg2' ? EC_DEPT_EXEC_LINE_ROOT_DESC : []),
    ...EC_DEPT_EXEC_LINE_SCOPE,
    ...EC_DEPT_EXEC_LINE_TAIL_DEPT,
  ]
}
