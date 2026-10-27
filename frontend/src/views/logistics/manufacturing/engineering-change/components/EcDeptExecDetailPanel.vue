<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/components -->
<!-- 文件名称：EcDeptExecDetailPanel.vue -->
<!-- 功能描述：执行部门右栏明细页签：按左侧设变主表加载计划物料（Z0）且根物料类型为 FERT 的明细；defineExpose reload -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="ec-dept-exec-detail-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="t('common.page.form.placeholder.search')"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      :show-create="false"
      :show-create-row="false"
      :show-update="false"
      :show-delete="false"
      :show-delete-row="false"
      :show-import="false"
      :show-export="false"
      :show-expand="false"
      :show-refresh="true"
      :show-advanced-query="false"
      :show-column-setting="true"
      :show-fullscreen="true"
      :refresh-loading="loading"
      :refresh-disabled="!hasMasterSelection"
      @column-setting="columnSettingVisible = true"
      @refresh="handleRefresh"
    />
    <div
      ref="detailTableWrapRef"
      class="ec-dept-exec-detail-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        entity-scope="company"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="ecDetailId"
        table-mode="masterDetailDetail"
        scroll-layout="masterDetailLr"
        :scroll="{ y: detailTableScrollY }"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getEcDetailId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        @change="handleTableChange"
        @pagination-change="handlePaginationChange"
        @resize-column="handleResizeColumn"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'discontinuedStatus'">
            <TaktDictTag dict-type="logistics_materials_material_discontinued_status" :value="record.discontinuedStatus" />
          </template>
          <template v-else-if="column.key === 'ecNewPurchaseType'">
            <TaktDictTag dict-type="logistics_procurement_type" :value="record.ecNewPurchaseType" />
          </template>
          <template v-else-if="column.key === 'ecOldPartDisposition'">
            <TaktDictTag dict-type="logistics_manufacturing_ec_old_part_disposition" :value="record.ecOldPartDisposition" />
          </template>
          <template v-else-if="column.key === 'ecNewRequiresInspection' || column.key === 'isObsolete'">
            <TaktDictTag dict-type="sys_yes_no" :value="record[String(column.key)]" />
          </template>
          <template v-else-if="isCalendarDateColumn(column.key)">
            {{ formatCalendarDate(record[String(column.key)]) }}
          </template>
        </template>
      </TaktSingleTable>
    </div>
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="ecDetailId"
      entity-scope="company"
      table-mode="masterDetailDetail"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 执行部门右栏设变明细页签（按设变主表分页）
 */
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getEcDetailList } from '@/api/logistics/manufacturing/engineering-change/ecDetail.api'
import { formatCalendarDate } from '@/utils/datetime'
import { measureConstrainedTableScrollY } from '@/utils/tableScroll'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import type { EcDetail } from '@/types/logistics/manufacturing/engineering-change/ecDetail.types'
import {
  ECDETAIL_DEPT_MASTER_DEFAULT_VISIBLE_COLUMN_KEYS,
  buildEcDetailTableColumns,
  useEcDetailI18n,
} from '../ec-te/composables/useEcDetailI18n'

const props = defineProps<{
  /** 左侧选中的设变主表主键 */
  ecTeId?: string
  /** 页签内表格 scroll.y（标准左右表高度减去页签栏） */
  scrollY: number
}>()

const { t } = useI18n()
const pi = useEcDetailI18n()

/**
 * 列是否为日历日（字段名以 Date 结尾）
 * @param key 列 key
 * @returns {boolean} 是否按 yyyy-MM-dd 展示
 */
function isCalendarDateColumn(key: unknown): boolean {
  return String(key ?? '').endsWith('Date')
}

/** 是否已选设变主表 */
const hasMasterSelection = computed(() => !!props.ecTeId?.trim())
/** 列表 loading */
const loading = ref(false)
/** 当前页明细 */
const dataSource = ref<EcDetail[]>([])
/** 服务端总数 */
const total = ref(0)
/** 当前页 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 关键词 */
const queryKeyword = ref('')
/** 列设置 */
const columnSettingVisible = ref(false)
/** 明细表外壳 */
const detailTableWrapRef = ref<HTMLElement | null>(null)
/** 明细表按外壳实测的 scroll.y */
const detailFittedScrollY = ref(0)
/** 明细表高度监听 */
let detailTableResizeObserver: ResizeObserver | null = null
/**
 * 明细表 scroll.y：不超过基础表减页签，且落在外壳内以保留横向滚动条
 */
const detailTableScrollY = computed(() => {
  if (detailFittedScrollY.value > 0) {
    return Math.min(props.scrollY, detailFittedScrollY.value)
  }
  return props.scrollY
})

/**
 * 按明细表外壳重算 scroll.y
 */
function syncDetailTableScrollY(): void {
  const fitted = measureConstrainedTableScrollY(detailTableWrapRef.value, { reservePagination: true })
  if (fitted > 0) {
    detailFittedScrollY.value = fitted
  }
}

/** 列定义 */
const columns = computed<TableColumnsType>(() =>
  buildEcDetailTableColumns((field) => pi.columnLabel(field)),
)
/** 可见列 */
const visibleColumnKeys = ref<string[]>([...ECDETAIL_DEPT_MASTER_DEFAULT_VISIBLE_COLUMN_KEYS])

/**
 * 明细主键
 * @param record 明细行
 * @returns {string} 主键
 */
function getEcDetailId(record: EcDetail | Record<string, unknown>) {
  return String((record as EcDetail).ecDetailId ?? '')
}

/**
 * 加载设变明细（按左侧设变主表分页，仅计划物料且根物料类型为 FERT）
 */
async function loadData() {
  const ecTeId = props.ecTeId?.trim()
  if (!ecTeId) {
    dataSource.value = []
    total.value = 0
    return
  }
  loading.value = true
  try {
    const res = await getEcDetailList({
      ecTeId,
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      keyWords: queryKeyword.value.trim() || undefined,
      discontinuedStatus: 'Z0',
      ecRootMaterialType: 'FERT',
    })
    dataSource.value = (res.data ?? []).map((row) => {
      const next = { ...row } as Record<string, unknown>
      for (const key of Object.keys(next)) {
        if (!key.endsWith('Date')) {
          continue
        }
        const formatted = formatCalendarDate(next[key] as string | null | undefined)
        next[key] = formatted || undefined
      }
      return next as EcDetail
    })
    total.value = res.total ?? 0
  } catch (error: unknown) {
    logger.error('[EcDeptExec] 加载设变明细失败', { error })
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 刷新（重置到第一页） */
function reload() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 搜索 */
function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 重置 */
function handleQueryReset() {
  queryKeyword.value = ''
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 刷新当前 */
function handleRefresh() {
  void loadData()
}

/** 分页 */
function handlePaginationChange() {
  void loadData()
}

/** 表格变化 */
function handleTableChange() {}

/** 列宽 */
function handleResizeColumn() {}

/**
 * 列显隐
 * @param keys 可见列
 */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/** 列重置 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = [...ECDETAIL_DEPT_MASTER_DEFAULT_VISIBLE_COLUMN_KEYS]
}

watch(
  () => props.ecTeId,
  () => {
    reload()
  },
)

onMounted(() => {
  reload()
  nextTick(() => {
    syncDetailTableScrollY()
    const wrap = detailTableWrapRef.value
    if (wrap && typeof ResizeObserver !== 'undefined') {
      detailTableResizeObserver = new ResizeObserver(() => syncDetailTableScrollY())
      detailTableResizeObserver.observe(wrap)
    }
  })
})

onBeforeUnmount(() => {
  detailTableResizeObserver?.disconnect()
  detailTableResizeObserver = null
})

defineExpose({ reload, loadData })
</script>
