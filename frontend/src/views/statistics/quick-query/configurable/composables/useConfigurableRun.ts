// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/views/statistics/quick-query/configurable/composables
// 文件名称：useConfigurableRun.ts
// 创建时间：2026-06-19
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表运行时：加载筛选屏、查询、导出
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { Ref } from 'vue'
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import {
  getConfigurableRuntimeScreen,
  executeConfigurableQuery,
  exportConfigurableData,
} from '@/api/statistics/quick-query/configurable.api'
import type {
  ConfigurableRuntimeScreen,
  ConfigurableQueryResult,
  ConfigurableRuntimeSelection,
  ConfigurableRuntimeSelectionValue,
} from '@/types/statistics/quick-query/configurable.types'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { taktExcelEntityNames } from '@/utils/naming'

/** 筛选项定义（含独立 formKey） */
export interface RuntimeSelectionRow extends ConfigurableRuntimeSelection {
  /** 表单绑定唯一键 */
  formKey: string
}

/** 筛选项表单状态项 */
export interface SelectionFormItem {
  /** 比较运算符（code_generator_query_type SortOrder，默认 7=like） */
  filterOperator: number
  /** 筛选值 */
  value: string
  /** 区间结束值 */
  valueTo: string
}

/** 全局行数上限 */
const GLOBAL_ROW_LIMIT = 50000
/** 默认最大命中数 */
export const CONFIGURABLE_RUN_DEFAULT_ROW_LIMIT = 500
/** 默认比较符：模糊 like */
const DEFAULT_FILTER_OPERATOR = 7

/**
 * 定制报表运行时组合式（筛选 + 查询 + 导出）
 * @param configurableId 定制报表主键
 * @param configurableName 定制报表名称（标题展示）
 * @returns 运行时状态与方法
 */
export function useConfigurableRun(
  configurableId: Ref<string | undefined>,
  configurableName: Ref<string | undefined>
) {
  const { t } = useI18n()
  const dictDataStore = useDictDataStore()
  const excelNames = taktExcelEntityNames('TaktConfigurableData')
  /** 页面标题 */
  const pageTitle = computed(() => {
    const name = configurableName.value?.trim()
    if (name) {
      return `${t('statistics.quickquery.configurable.page.run')} - ${name}`
    }
    return t('statistics.quickquery.configurable.page.run')
  })
  /** 运行时屏幕定义 */
  const screen = ref<ConfigurableRuntimeScreen | null>(null)
  /** 规范化后的筛选项行 */
  const runtimeSelectionRows = ref<RuntimeSelectionRow[]>([])
  /** 加载屏幕定义 loading */
  const screenLoading = ref(false)
  /** 查询 loading */
  const queryLoading = ref(false)
  /** 导出 loading */
  const exportLoading = ref(false)
  /** 查询结果 */
  const queryResult = ref<ConfigurableQueryResult | null>(null)
  /** 结果页码 */
  const resultPageIndex = ref(1)
  /** 结果每页条数 */
  const resultPageSize = ref(20)
  /** 本次查询/导出最大命中数 */
  const rowLimit = ref(CONFIGURABLE_RUN_DEFAULT_ROW_LIMIT)
  /** 按唯一键索引的筛选表单 */
  const selectionForm = reactive<Record<string, SelectionFormItem>>({})

  /** 筛选项比较符下拉（code_generator_query_type，value 为 SortOrder 1～8） */
  const filterOperatorOptions = computed(() => {
    const opts = dictDataStore.getDictOptionsForSelect('code_generator_query_type', {
      labelField: 'dictLabel',
      valueField: 'sortOrder',
    })
    return opts.map((opt) => {
      const i18nKey = opt.i18nKey?.trim()
      const label = i18nKey ? t(i18nKey) : String(opt.label ?? opt.dictLabel ?? '')
      const raw = opt.sortOrder ?? opt.value
      return {
        label,
        value: typeof raw === 'number' ? raw : Number(raw),
      }
    })
  })

  /**
   * 规范化报表配置的最大命中数
   * @param configured 报表配置值
   * @returns {number} 合法上限
   */
  function normalizeConfiguredRowLimit(configured: number | undefined): number {
    const value = configured ?? CONFIGURABLE_RUN_DEFAULT_ROW_LIMIT
    return Math.min(value > 0 ? value : CONFIGURABLE_RUN_DEFAULT_ROW_LIMIT, GLOBAL_ROW_LIMIT)
  }

  /** 最大命中数输入上限 */
  const maxRowLimit = computed(() => {
    const queryCap = normalizeConfiguredRowLimit(screen.value?.maxQueryRows)
    const exportCap = normalizeConfiguredRowLimit(screen.value?.maxExportRows)
    return Math.min(queryCap, exportCap)
  })

  /** 结果表列 */
  const resultColumns = computed<TableColumnsType>(() => {
    if (!queryResult.value?.columns?.length) {
      return []
    }
    return queryResult.value.columns.map((col) => ({
      title: col.label,
      dataIndex: col.key,
      key: col.key,
      ellipsis: true,
    }))
  })

  /**
   * 筛选项表单唯一键
   * @param sel 筛选项定义
   * @param index 数组序号
   * @returns {string} 表单键
   */
  function resolveSelectionFormKey(sel: ConfigurableRuntimeSelection, index: number): string {
    const id = sel.configurableSelectionId?.trim()
    if (id) {
      return `id-${id}`
    }
    return `idx-${index}`
  }

  /**
   * 筛选项表单标签（仅字段名）
   * @param sel 筛选项定义
   * @returns {string} 标签文案
   */
  function selectionFieldLabel(sel: ConfigurableRuntimeSelection): string {
    const name = sel.displayName?.trim() || sel.columnName?.trim()
    return name || String(sel.sortOrder)
  }

  /**
   * 为筛选项附加表单绑定键
   * @param selections 原始筛选项
   * @returns 运行时筛选项行
   */
  function buildRuntimeSelectionRows(
    selections: ConfigurableRuntimeSelection[]
  ): RuntimeSelectionRow[] {
    const ordered = [...selections].sort((a, b) => a.sortOrder - b.sortOrder)
    return ordered.map((sel, index) => ({
      ...sel,
      formKey: resolveSelectionFormKey(sel, index),
    }))
  }

  /**
   * 结果行 row-key
   * @param record 行数据
   * @returns {string} 行键
   */
  function resultRowKey(record: Record<string, unknown>): string {
    const cols = queryResult.value?.columns
    if (!cols?.length) {
      return 'row-empty'
    }
    return cols.map((col) => `${col.key}=${String(record[col.key] ?? '')}`).join('|')
  }

  /**
   * 写入筛选项表单默认值（加载屏幕时同步行集合）
   * @param rows 运行时筛选项行
   */
  function syncSelectionForm(rows: RuntimeSelectionRow[]): void {
    const rowKeys = new Set(rows.map((row) => row.formKey))
    for (const key of Object.keys(selectionForm)) {
      if (!rowKeys.has(key)) {
        delete selectionForm[key]
      }
    }
    for (const sel of rows) {
      applySelectionFormDefaults(sel)
    }
  }

  /**
   * 将单条筛选项恢复为定义默认值
   * @param sel 筛选项定义
   */
  function applySelectionFormDefaults(sel: RuntimeSelectionRow): void {
    const op = sel.filterOperator
    const defaults: SelectionFormItem = {
      filterOperator: op >= 1 && op <= 8 ? op : DEFAULT_FILTER_OPERATOR,
      value: sel.defaultValue?.trim() ?? '',
      valueTo: sel.defaultValueTo?.trim() ?? '',
    }
    const existing = selectionForm[sel.formKey]
    if (existing) {
      existing.filterOperator = defaults.filterOperator
      existing.value = defaults.value
      existing.valueTo = defaults.valueTo
      return
    }
    selectionForm[sel.formKey] = defaults
  }

  /**
   * 重置筛选条件与最大命中数（就地更新，避免 delete 触发 Ant Design 组件 slot 警告）
   */
  function resetSelectionForm(): void {
    for (const sel of runtimeSelectionRows.value) {
      applySelectionFormDefaults(sel)
    }
    rowLimit.value = CONFIGURABLE_RUN_DEFAULT_ROW_LIMIT
    queryResult.value = null
    resultPageIndex.value = 1
  }

  /**
   * 加载运行时屏幕定义
   */
  async function loadRuntimeScreen(): Promise<void> {
    const id = configurableId.value?.trim()
    if (!id) {
      return
    }
    screenLoading.value = true
    try {
      await dictDataStore.loadAllDictDataAsync()
      const data = await getConfigurableRuntimeScreen(id)
      runtimeSelectionRows.value = data.selections?.length
        ? buildRuntimeSelectionRows(data.selections)
        : []
      screen.value = data
      syncSelectionForm(runtimeSelectionRows.value)
      queryResult.value = null
      resultPageIndex.value = 1
      rowLimit.value = CONFIGURABLE_RUN_DEFAULT_ROW_LIMIT
    } catch (error: unknown) {
      const err = error as { message?: string }
      logger.error('[ConfigurableRun] 加载运行时屏幕失败', { error })
      message.error(err?.message || t('common.feedback.load.data.failed'))
      screen.value = null
      runtimeSelectionRows.value = []
    } finally {
      screenLoading.value = false
    }
  }

  /**
   * 组装筛选值列表
   * @returns {ConfigurableRuntimeSelectionValue[]} 筛选值
   */
  function buildSelectionValues(): ConfigurableRuntimeSelectionValue[] {
    const list: ConfigurableRuntimeSelectionValue[] = []
    for (const sel of runtimeSelectionRows.value) {
      const form = selectionForm[sel.formKey]
      if (!form) {
        continue
      }
      const { filterOperator, value, valueTo } = form
      if (!value?.trim()) {
        continue
      }
      if (filterOperator === 8 && !valueTo?.trim()) {
        continue
      }
      list.push({
        ...(sel.configurableSelectionId?.trim()
          ? { configurableSelectionId: sel.configurableSelectionId.trim() }
          : {}),
        sortOrder: sel.sortOrder,
        filterOperator,
        value: value.trim(),
        valueTo: filterOperator === 8 ? valueTo.trim() : undefined,
      })
    }
    return list
  }

  /**
   * 执行查询
   * @returns {Promise<boolean>} 是否查询成功
   */
  async function handleQuery(): Promise<boolean> {
    const id = configurableId.value?.trim()
    if (!id) {
      return false
    }
    queryLoading.value = true
    try {
      const selectionValues = buildSelectionValues()
      const result = await executeConfigurableQuery(id, {
        pageIndex: resultPageIndex.value,
        pageSize: resultPageSize.value,
        rowLimit: rowLimit.value,
        selectionValues,
      })
      queryResult.value = result
      resultPageIndex.value = result.pageIndex
      resultPageSize.value = result.pageSize
      return true
    } catch (error: unknown) {
      const err = error as { message?: string }
      logger.error('[ConfigurableRun] 查询失败', { error })
      message.error(err?.message || t('common.feedback.load.data.failed'))
      return false
    } finally {
      queryLoading.value = false
    }
  }

  /**
   * 导出数据
   */
  async function handleExport(): Promise<void> {
    const id = configurableId.value?.trim()
    if (!id) {
      return
    }
    exportLoading.value = true
    try {
      const selectionValues = buildSelectionValues()
      const exportMeta = await exportConfigurableData(
        id,
        { selectionValues, rowLimit: rowLimit.value },
        excelNames.sheet,
        `${screen.value?.configurableCode ?? 'configurable'}_data`
      )
      const ts = new Date()
      const pad = (n: number, w = 2) => String(n).padStart(w, '0')
      const fallbackBase = `${screen.value?.configurableCode ?? 'configurable'}_data_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
      const fileName = resolveExportDownloadFileName({
        contentDisposition: exportMeta.contentDisposition ?? null,
        contentType: exportMeta.contentType ?? null,
        fallbackBase,
      })
      const url = window.URL.createObjectURL(exportMeta.blob)
      const link = document.createElement('a')
      link.href = url
      link.download = fileName
      link.style.display = 'none'
      document.body.appendChild(link)
      link.click()
      document.body.removeChild(link)
      setTimeout(() => window.URL.revokeObjectURL(url), 100)
      message.success(
        t('common.feedback.export.success', { target: t('entity.configurable._self') })
      )
    } catch (error: unknown) {
      const err = error as { message?: string }
      logger.error('[ConfigurableRun] 导出失败', { error })
      message.error(
        err?.message || t('common.feedback.export.failed', { target: t('entity.configurable._self') })
      )
    } finally {
      exportLoading.value = false
    }
  }

  /**
   * 结果分页变更
   */
  function handleResultPageChange(page: number): void {
    resultPageIndex.value = page
    void handleQuery()
  }

  /**
   * 结果每页条数变更
   */
  function handleResultPageSizeChange(_current: number, size: number): void {
    resultPageIndex.value = 1
    resultPageSize.value = size
    void handleQuery()
  }

  return {
    pageTitle,
    screen,
    runtimeSelectionRows,
    screenLoading,
    queryLoading,
    exportLoading,
    queryResult,
    resultPageIndex,
    resultPageSize,
    rowLimit,
    selectionForm,
    filterOperatorOptions,
    maxRowLimit,
    resultColumns,
    selectionFieldLabel,
    resultRowKey,
    loadRuntimeScreen,
    resetSelectionForm,
    handleQuery,
    handleExport,
    handleResultPageChange,
    handleResultPageSizeChange,
  }
}
