// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/views/foundation/quartz-task/utils
// 文件名称：quartzSyncDbFields.ts
// 创建时间：2026-08-07
// 创建人：Takt365(Cursor AI)
// 功能描述：同步 SQL 立即执行弹窗：源/目标库识别与 ExecuteParams 组装
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 源库日链脚本（源库固定，仅选目标库） */
const SAP_DATA_SYNC_SCRIPTS = new Set([
  'quartz/sync_matplt.sql',
  'quartz/sync_mdl.sql',
  'quartz/sync_st.sql',
  'quartz/sync_ec.sql',
  'quartz/sync_mo.sql',
])

/** 源库日链 JobName（小写；列表缺 sqlScript 时兜底） */
const SAP_DATA_SYNC_JOB_NAMES = new Set([
  'sync_matplt',
  'sync_mdl',
  'sync_st',
  'sync_ec',
  'sync_mo',
])

/** 仅目标库回填 / 目标库派生 JobName（小写；仅弹目标库，无核算月） */
const TARGET_ONLY_BACKFILL_JOB_NAMES = new Set([
  'sync_pup_bk',
  'sync_sp_bk',
  'sync_bv_bk',
  'sync_pph_bk',
  'sync_pph',
])

/** 仅目标库（无跨库源占位：日链 + 回填 *_bk；不含需核算月的 pcb_sect / bc_bk） */
const TARGET_ONLY_SYNC_SCRIPTS = new Set([
  ...SAP_DATA_SYNC_SCRIPTS,
  'quartz/sync_pup_bk.sql',
  'quartz/sync_sp_bk.sql',
  'quartz/sync_bv_bk.sql',
  'quartz/sync_pph_bk.sql',
])

/** BOM 立即执行须选目标库+核算月的 JobName（小写） */
const BOM_DB_AND_MONTH_JOB_NAMES = new Set([
  'bom_material_cost_sum',
  'bom_material_cost_recalc',
  'bom_model_avg_cost',
  'bom_zero_price_mp_bk',
  'bom_pcb_sect',
  'sync_bc_pcb_sect_bk',
  'sync_bc_bk',
])

/** 默认暂存源库名（三部分标识；列表中可能无 Tenant_900） */
export const DEFAULT_STAGING_SOURCE_DATABASE = 'zTakt_900_Dev'

/**
 * 规范化 SqlScript 路径为小写正斜杠
 * @param sqlScript 任务 SqlScript
 * @returns {string} 规范化路径
 */
export function normalizeQuartzSqlScriptPath(sqlScript: string | null | undefined): string {
  return String(sqlScript ?? '')
    .trim()
    .replace(/\\/g, '/')
    .toLowerCase()
}

/**
 * 是否为需选库的同步脚本（排除手工 DDL）
 * @param sqlScript 任务 SqlScript
 * @returns {boolean} 是否同步脚本
 */
export function isQuartzSyncSqlScript(sqlScript: string | null | undefined): boolean {
  const path = normalizeQuartzSqlScriptPath(sqlScript)
  if (!path.startsWith('quartz/sync_') || !path.endsWith('.sql')) {
    return false
  }
  return path !== 'quartz/sync_data_create_tables.sql'
}

/**
 * 是否仅需目标库（源库日链 + sync_pup/sp/bv_bk 回填；不含 pcb_sect / bc_bk）
 * @param sqlScript 任务 SqlScript
 * @returns {boolean} 仅目标
 */
export function needsSyncTargetOnlyPicker(sqlScript: string | null | undefined): boolean {
  return TARGET_ONLY_SYNC_SCRIPTS.has(normalizeQuartzSqlScriptPath(sqlScript))
}

/**
 * 是否仅需目标库（按任务行：JobName / TaskCode / SqlScript；绝不含核算月）
 * @param record 任务行
 * @returns {boolean} 仅目标库弹窗
 */
export function needsSyncTargetOnlyFromTask(record: {
  taskCode?: string | null
  jobName?: string | null
  sqlScript?: string | null
}): boolean {
  // PCB SECT 走目标库+核算月，不进本分支
  if (needsBomDbAndMonthPicker(record)) {
    return false
  }
  const jobName = String(record.jobName ?? '')
    .trim()
    .toLowerCase()
  if (SAP_DATA_SYNC_JOB_NAMES.has(jobName) || TARGET_ONLY_BACKFILL_JOB_NAMES.has(jobName)) {
    return true
  }
  const taskCode = String(record.taskCode ?? '').toUpperCase()
  if (
    taskCode === 'QT_SYNC_PUP_BK'
    || taskCode === 'QT_SYNC_SP_BK'
    || taskCode === 'QT_SYNC_BV_BK'
    || taskCode === 'QT_SYNC_PPH_BK'
  ) {
    return true
  }
  return needsSyncTargetOnlyPicker(record.sqlScript)
}

/**
 * 是否需源库+目标库（zTakt_900 暂存族）
 * @param sqlScript 任务 SqlScript
 * @returns {boolean} 源+目标
 */
export function needsSyncSourceTargetPicker(sqlScript: string | null | undefined): boolean {
  const path = normalizeQuartzSqlScriptPath(sqlScript)
  // pcb_sect / bc_bk 由 BOM 合并窗处理，勿当源+目标
  if (path === 'quartz/sync_bc_pcb_sect_bk.sql' || path === 'quartz/sync_bc_bk.sql') {
    return false
  }
  return isQuartzSyncSqlScript(sqlScript) && !needsSyncTargetOnlyPicker(sqlScript)
}

/**
 * 从任务 ExecuteParams JSON 解析源/目标库
 * @param executeParams 任务配置或上次参数
 * @returns {{ sourceDatabase?: string, targetDatabase?: string }}
 */
export function parseSyncExecuteDatabaseParams(executeParams: string | null | undefined): {
  sourceDatabase?: string
  targetDatabase?: string
} {
  if (!executeParams?.trim()) {
    return {}
  }
  try {
    const raw = JSON.parse(executeParams.trim()) as Record<string, unknown>
    if (!raw || typeof raw !== 'object') {
      return {}
    }
    const source =
      typeof raw.sourceDatabase === 'string'
        ? raw.sourceDatabase.trim()
        : typeof raw.SourceDatabase === 'string'
          ? raw.SourceDatabase.trim()
          : undefined
    const target =
      typeof raw.targetDatabase === 'string'
        ? raw.targetDatabase.trim()
        : typeof raw.TargetDatabase === 'string'
          ? raw.TargetDatabase.trim()
          : undefined
    return {
      sourceDatabase: source || undefined,
      targetDatabase: target || undefined,
    }
  } catch {
    return {}
  }
}

/**
 * 组装同步任务立即执行参数 JSON
 * @param options 源/目标库
 * @param options.sourceDatabase 源库（可选）
 * @param options.targetDatabase 目标库
 * @returns {string} executeParams JSON
 */
export function buildSyncExecuteParams(options: {
  sourceDatabase?: string
  targetDatabase: string
}): string {
  const targetDatabase = options.targetDatabase.trim()
  const sourceDatabase = options.sourceDatabase?.trim()
  if (sourceDatabase) {
    return JSON.stringify({ sourceDatabase, targetDatabase })
  }
  return JSON.stringify({ targetDatabase })
}

/**
 * 是否为 BOM 类立即执行（须同时选目标库 + 核算月份）
 * @param record 任务行（含 taskCode / jobName / className / sqlScript）
 * @returns {boolean} 是否弹合并窗
 */
export function needsBomDbAndMonthPicker(record: {
  taskCode?: string | null
  jobName?: string | null
  className?: string | null
  sqlScript?: string | null
}): boolean {
  const jobName = String(record.jobName ?? '')
    .trim()
    .toLowerCase()
  if (BOM_DB_AND_MONTH_JOB_NAMES.has(jobName)) {
    return true
  }
  const taskCode = String(record.taskCode ?? '').toUpperCase()
  if (
    taskCode.startsWith('QT_BOM')
    || taskCode === 'QT_SYNC_BC_PCB_SECT_BK'
    || taskCode === 'QT_SYNC_BC_BK'
  ) {
    return true
  }
  const className = String(record.className ?? '')
  if (/Bom.*(Cost|Avg|PcbSect|ZeroPrice)/i.test(className)) {
    return true
  }
  const path = normalizeQuartzSqlScriptPath(record.sqlScript)
  return path === 'quartz/sync_bc_pcb_sect_bk.sql' || path === 'quartz/sync_bc_bk.sql'
}

/**
 * 组装 BOM / PCB SECT 立即执行参数（目标库 + 核算月）
 * @param options 目标库与核算月
 * @param options.targetDatabase 目标库
 * @param options.costingPeriod 核算月 yyyy-MM
 * @returns {string} executeParams JSON
 */
export function buildBomDbAndMonthExecuteParams(options: {
  targetDatabase: string
  costingPeriod: string
}): string {
  const targetDatabase = options.targetDatabase.trim()
  const costingPeriod = options.costingPeriod.trim()
  return JSON.stringify({ targetDatabase, costingPeriod })
}
