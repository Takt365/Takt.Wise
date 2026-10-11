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
  eckoubai: 'ecKoubaiId',
  ecukeken: 'ecUkekenId',
  ecbukan: 'ecBukanId',
  ecsmt: 'ecSmtId',
  ecseikan: 'ecSeikanId',
  ecseizouikka: 'ecSeizouikkaId',
  ecseizounika: 'ecSeizounikaId',
  echinkan: 'ecHinkanId',
  ecseizougijutsu: 'ecSeizougijutsuId',
}

/**
 * 前列：行号、设变单号（对齐实体 LineNumber → EcCode）
 */
const EC_DEPT_EXEC_LINE_IDENTITY = ['lineNumber', 'ecCode'] as const

/**
 * 含机种/根物料编码课别的头段：停产状态 → 机种 → 根物料编码（描述在业务字段之后）
 */
const EC_DEPT_EXEC_LINE_HEAD_DISC_MODEL = [
  'discontinuedStatus',
  'ecModelCode',
  'ecRootMaterialCodeGroup',
] as const

/**
 * 部管/SMT：停产状态 → 机种 → 根物料编码组（无根物料描述）
 */
const EC_DEPT_EXEC_LINE_HEAD_DISC_MODEL_GROUP = [
  'discontinuedStatus',
  'ecModelCode',
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

/** 含生管 scheduled 冗余展示字段的部门 slug（除采购/受检） */
const EC_DEPT_EXEC_HAS_SCHEDULED_REDUNDANT = new Set([
  'ecbukan',
  'ecsmt',
  'ecseizouikka',
  'ecseizounika',
  'echinkan',
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
 * 尾段：部门编码、部门名称
 */
const EC_DEPT_EXEC_LINE_TAIL_DEPT = ['deptCode', 'deptName'] as const

/** 各课实施/业务字段（实体 ExecContent 之后、根物料描述之前） */
const EC_DEPT_EXEC_LINE_IMPL = {
  /** TaktEcKoubai：采购订单发行日期、供应商、采购订单号码（旧品处理为明细冗余，见下方展示段） */
  eckoubai: ['purchaseOrderIssueDate', 'supplier', 'purchaseOrderCode'],
  /** TaktEcSeikan：Po残、结余（scheduled 在实施头段；旧品处理为明细冗余） */
  ecseikan: ['poRemainder', 'balance'],
  /** TaktEcUkeken：受检单号、检验日期 */
  ecukeken: ['iqcOrderCode', 'inspectionDate'],
  /** TaktEcBukan：出库批次、出库日期 */
  ecbukan: ['outboundBatch', 'outboundDate'],
  /** TaktEcSmt：出库批次、出库日期 */
  ecsmt: ['outboundBatch', 'outboundDate'],
  /** TaktEcSeizounika：生产班组、生产日期、实施批次 */
  ecseizounika: ['productionTeam', 'productionDate', 'implementationBatch'],
  /** TaktEcSeizouikka：生产班组、生产日期、实施批次 */
  ecseizouikka: ['productionTeam', 'productionDate', 'implementationBatch'],
  /** TaktEcHinkan：生产班组、检验日期、检验批次、抽样号码 */
  echinkan: ['productionTeam', 'inspectionDate', 'inspectionBatch', 'samplingCode'],
  /** TaktEcSeizougijutsu：SOP日期、更新SOP */
  ecseizougijutsu: ['sopDate', 'isSopUpdated'],
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
  const seikanSchedule = slug === 'ecseikan' ? (['scheduledDate', 'scheduledBatch'] as const) : []
  return ['isImplemented', 'execContent', ...seikanSchedule, ...impl]
}

/**
 * 明细旧品处理冗余展示（生管/采购只读）
 */
const EC_DEPT_EXEC_LINE_OLD_PART_REDUNDANT = ['ecOldPartDisposition'] as const

/** 含旧品处理冗余展示的部门 slug */
const EC_DEPT_EXEC_HAS_OLD_PART_REDUNDANT = new Set(['eckoubai', 'ecseikan'])

/**
 * 采购：停产状态 + 新物料、新品仓库、新采购类型（无机种/根物料编码）
 */
const EC_DEPT_EXEC_LINE_MID_KOUBAI = [
  'discontinuedStatus',
  'ecModelCodeGroup',
  'ecRootMaterialCodeGroup',
  'ecNewMaterialCode',
  'ecNewMaterialDescription',
  'ecNewWarehouse',
  'ecNewPurchaseType',
] as const

/**
 * 受检：停产状态 + 新物料、新品仓库、新品检验
 */
const EC_DEPT_EXEC_LINE_MID_UKEKEN = [
  'discontinuedStatus',
  'ecModelCodeGroup',
  'ecRootMaterialCodeGroup',
  'ecNewMaterialCode',
  'ecNewMaterialDescription',
  'ecNewWarehouse',
  'ecNewRequiresInspection',
] as const

/**
 * 部管：新物料 / 新采购类型 / 新品仓库（停产与机种/根物料编码已在头段）
 */
const EC_DEPT_EXEC_LINE_MID_BUKAN = [
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
 * 制二：机种/新物料/上阶编码组与新物料描述（单码机种与根物料组已在头段）
 */
const EC_DEPT_EXEC_LINE_MID_SEIZOUNIKA = [
  'ecModelCodeGroup',
  'ecNewMaterialCodeGroup',
  'ecNewMaterialDescription',
  'ecParentMaterialCodeGroup',
  'ecParentMaterialCode',
  'ecParentMaterialDescription',
] as const

/**
 * 制一/品管：按设变时的机种编码组（单码机种与根物料组已在头段）
 */
const EC_DEPT_EXEC_LINE_MID_MODEL_GROUP = ['ecModelCodeGroup'] as const

/**
 * 生管/制技：头段之后无额外物料上下文
 */
const EC_DEPT_EXEC_LINE_MID_EMPTY = [] as const

const EC_DEPT_EXEC_LINE_MID_BY_SLUG: Record<string, readonly string[]> = {
  eckoubai: EC_DEPT_EXEC_LINE_MID_KOUBAI,
  ecukeken: EC_DEPT_EXEC_LINE_MID_UKEKEN,
  ecbukan: EC_DEPT_EXEC_LINE_MID_BUKAN,
  ecsmt: EC_DEPT_EXEC_LINE_MID_SMT,
  ecseizounika: EC_DEPT_EXEC_LINE_MID_SEIZOUNIKA,
  ecseizouikka: EC_DEPT_EXEC_LINE_MID_MODEL_GROUP,
  echinkan: EC_DEPT_EXEC_LINE_MID_MODEL_GROUP,
  ecseizougijutsu: EC_DEPT_EXEC_LINE_MID_EMPTY,
  ecseikan: EC_DEPT_EXEC_LINE_MID_EMPTY,
}

/** 含机种/根物料编码/停产头段的部门（采购/受检、部管/SMT 除外） */
const EC_DEPT_EXEC_HAS_DISC_MODEL = new Set([
  'ecseikan',
  'ecseizouikka',
  'echinkan',
  'ecseizougijutsu',
  'ecseizounika',
])

/** 部管/SMT：机种 + 根物料编码组，无根物料描述 */
const EC_DEPT_EXEC_HAS_ROOT_GROUP = new Set(['ecbukan', 'ecsmt'])

/** 执行部门实体 slug */
export type EcDeptExecSlug = keyof typeof EC_DEPT_EXEC_LINE_IMPL

/**
 * 取指定部门执行主表字段
 * 顺序：主键 → 行号/单号 → [停产/机种/根物料编码] → 物料上下文 → 实施/执行内容 → 本课业务 → [根物料描述] → 部门
 * @param slug 实体 slug（eckoubai 对应 TaktEcKoubai）
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
    ...EC_DEPT_EXEC_LINE_EXEC_HEAD,
    ...(EC_DEPT_EXEC_HAS_SCHEDULED_REDUNDANT.has(slug) ? EC_DEPT_EXEC_LINE_SCHEDULED_REDUNDANT : []),
    ...(slug === 'ecseikan' ? EC_DEPT_EXEC_LINE_SCHEDULED_REDUNDANT : []),
    ...EC_DEPT_EXEC_LINE_EXEC_CONTENT,
    ...impl,
    ...(EC_DEPT_EXEC_HAS_OLD_PART_REDUNDANT.has(slug) ? EC_DEPT_EXEC_LINE_OLD_PART_REDUNDANT : []),
    ...(hasDiscModel ? EC_DEPT_EXEC_LINE_ROOT_DESC : []),
    ...EC_DEPT_EXEC_LINE_TAIL_DEPT,
  ]
}
