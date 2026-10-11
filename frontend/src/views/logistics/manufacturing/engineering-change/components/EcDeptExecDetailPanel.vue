<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/components -->
<!-- 文件名称：EcDeptExecDetailPanel.vue -->
<!-- 功能描述：执行部门右栏：按选中执行主表加载视图子表设变明细（EcXxxId）；defineExpose reload -->
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
      :show-update="false"
      :show-delete="false"
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
        :data-source="pagedRows"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getEcDetailId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="filteredTotal"
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
 * 执行部门右栏设变明细面板（视图主从子表）
 */
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { measureMasterDetailLrTableScrollY } from '@/composables/useTaktMasterDetailLrScrollY'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import type { EcDetail } from '@/types/logistics/manufacturing/engineering-change/ecDetail.types'
import {
  ECDETAIL_DEPT_MASTER_DEFAULT_VISIBLE_COLUMN_KEYS,
  buildEcDetailTableColumns,
  useEcDetailI18n,
} from '../ec-gijutsu/composables/useEcDetailI18n'
import { useEcDeptExecMasterContext } from '../composables/useEcDeptExecMasterContext'

const props = defineProps<{
  /** 执行行主键字段（同明细视图外键，如 ecBukanId） */
  idField: string
  /** 按执行主键取行（须含 ecDetails） */
  getMasterById: (id: string) => Promise<{ ecDetails?: EcDetail[] } | null | undefined>
}>()

const { t } = useI18n()
const pi = useEcDetailI18n()
const { selectedMasterRow } = useEcDeptExecMasterContext()

/**
 * 当前选中执行主表主键
 * @returns {string} 执行 Id
 */
function selectedMasterId(): string {
  return String(selectedMasterRow.value?.[props.idField] ?? '').trim()
}

/** 是否已选主表执行行 */
const hasMasterSelection = computed(() => !!selectedMasterId())
/** 列表 loading */
const loading = ref(false)
/** 全量明细（来自 GetById.EcDetails） */
const allRows = ref<EcDetail[]>([])
/** 当前页 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 关键词 */
const queryKeyword = ref('')
/** 列设置 */
const columnSettingVisible = ref(false)
/** 子表滚动容器 */
const detailTableWrapRef = ref<HTMLElement | null>(null)
/** 子表 scroll.y */
const detailTableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let detailTableScrollResizeObserver: ResizeObserver | null = null

/** 列定义 */
const columns = computed<TableColumnsType>(() =>
  buildEcDetailTableColumns((field) => pi.columnLabel(field)),
)
/** 可见列 */
const visibleColumnKeys = ref<string[]>([...ECDETAIL_DEPT_MASTER_DEFAULT_VISIBLE_COLUMN_KEYS])

/**
 * 客户端关键词过滤
 * @param rows 明细行
 * @returns {EcDetail[]} 过滤结果
 */
function filterRows(rows: EcDetail[]): EcDetail[] {
  const kw = queryKeyword.value.trim().toLowerCase()
  if (!kw) {
    return rows
  }
  return rows.filter((row) => {
    const bag = [
      row.ecCode,
      row.ecModelCode,
      row.ecRootMaterialCode,
      row.ecRootMaterialDescription,
      row.ecParentMaterialCode,
      row.ecNewMaterialCode,
      row.ecOldMaterialCode,
      String(row.lineNumber ?? ''),
    ]
      .filter(Boolean)
      .join(' ')
      .toLowerCase()
    return bag.includes(kw)
  })
}

/** 过滤后总数 */
const filteredTotal = computed(() => filterRows(allRows.value).length)

/** 当前页数据 */
const pagedRows = computed(() => {
  const filtered = filterRows(allRows.value)
  const start = (currentPage.value - 1) * pageSize.value
  return filtered.slice(start, start + pageSize.value)
})

/**
 * 明细主键
 * @param record 明细行
 * @returns {string} 主键
 */
function getEcDetailId(record: EcDetail | Record<string, unknown>) {
  return String((record as EcDetail).ecDetailId ?? '')
}

/**
 * 重算子表 scroll.y
 */
function recalcDetailTableScrollY(): void {
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollY.value = measureMasterDetailLrTableScrollY(wrap)
}

/**
 * 加载设变明细（视图子表）
 */
async function loadData() {
  const masterId = selectedMasterId()
  if (!masterId) {
    allRows.value = []
    return
  }
  loading.value = true
  try {
    const res = await props.getMasterById(masterId)
    allRows.value = res?.ecDetails ?? []
  } catch (error: unknown) {
    logger.error('[EcDeptExec] 加载设变明细失败', { error })
    allRows.value = []
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
}

/** 重置 */
function handleQueryReset() {
  queryKeyword.value = ''
  currentPage.value = getTaktDefaultPageIndex()
}

/** 刷新当前 */
function handleRefresh() {
  void loadData()
}

/** 分页 */
function handlePaginationChange() {}

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
  () => selectedMasterRow.value?.[props.idField],
  () => {
    reload()
  },
)

onMounted(() => {
  nextTick(() => {
    recalcDetailTableScrollY()
    const wrap = detailTableWrapRef.value
    if (wrap && typeof ResizeObserver !== 'undefined') {
      detailTableScrollResizeObserver = new ResizeObserver(() => recalcDetailTableScrollY())
      detailTableScrollResizeObserver.observe(wrap)
    }
  })
})

onBeforeUnmount(() => {
  detailTableScrollResizeObserver?.disconnect()
  detailTableScrollResizeObserver = null
})

defineExpose({ reload, loadData })
</script>
