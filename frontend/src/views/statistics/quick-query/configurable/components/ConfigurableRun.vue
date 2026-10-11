<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/statistics/quick-query/configurable/components -->
<!-- 文件名称：ConfigurableRun.vue -->
<!-- 功能描述：定制报表独立执行页（筛选条件 + 查询结果、查询/导出/重置） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="configurable-run flex h-full flex-col p-4">
    <TaktToolsBar
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="false"
      :show-export="false"
      :show-advanced-query="false"
      :show-column-setting="false"
      :show-fullscreen="false"
      :show-refresh="false"
      :show-transpose="false"
      :show-expand="false"
      :show-create-row="false"
      :show-delete-row="false"
    >
      <template #left>
        <div class="flex min-w-0 flex-1 flex-wrap items-center justify-between gap-3">
          <span class="truncate text-base font-medium">{{ pageTitle }}</span>
          <a-space class="shrink-0">
            <a-button
              class="takt-button-query"
              :loading="queryLoading"
              :disabled="!configurableId"
              @click="handleQueryAndShowResult"
            >
              <template #icon>
                <RiSearchLine class="takt-remix-icon" />
              </template>
              {{ t('statistics.quickquery.configurable.page.query') }}
            </a-button>
            <a-button
              class="takt-button-reset"
              :disabled="!configurableId || screenLoading"
              @click="handleReset"
            >
              <template #icon>
                <RiRefreshLine class="takt-remix-icon" />
              </template>
              {{ t('common.page.button.reset') }}
            </a-button>
            <a-button
              v-permission="'statistics:quickquery:configurable:run'"
              class="takt-button-export"
              :loading="exportLoading"
              :disabled="!configurableId"
              @click="handleExport"
            >
              <template #icon>
                <RiExportLine class="takt-remix-icon" />
              </template>
              {{ t('statistics.quickquery.configurable.page.exportdata') }}
            </a-button>
            <a-button class="takt-button-return" @click="handleBack">
              <template #icon>
                <RiArrowLeftLine class="takt-remix-icon" />
              </template>
              {{ t('statistics.quickquery.configurable.page.runpage.backtolist') }}
            </a-button>
          </a-space>
        </div>
      </template>
    </TaktToolsBar>
    <a-spin :spinning="screenLoading" class="min-h-0 flex-1">
      <a-radio-group
        v-model:value="activeTabKey"
        button-style="solid"
        class="mb-4"
      >
        <a-radio-button value="selection">
          {{ t('statistics.quickquery.configurable.page.selectionscreen') }}
        </a-radio-button>
        <a-radio-button value="result">
          {{ t('statistics.quickquery.configurable.page.resulttitle') }}
        </a-radio-button>
      </a-radio-group>
      <div v-show="activeTabKey === 'selection'">
        <div v-if="screen" class="flex max-w-4xl flex-col gap-4">
          <div class="overflow-hidden rounded border border-border text-sm">
            <div
              class="grid grid-cols-[minmax(8rem,10rem)_minmax(8rem,9rem)_1fr] border-b border-border bg-page font-medium text-text"
            >
              <div class="border-r border-border px-3 py-2">
                {{ t('statistics.quickquery.configurable.page.runpage.col.label') }}
              </div>
              <div class="border-r border-border px-3 py-2">
                {{ t('statistics.quickquery.configurable.page.runpage.col.operator') }}
              </div>
              <div class="px-3 py-2">
                {{ t('statistics.quickquery.configurable.page.runpage.col.input') }}
              </div>
            </div>
            <div
              v-for="record in runtimeSelectionRows"
              :key="record.formKey"
              class="grid grid-cols-[minmax(8rem,10rem)_minmax(8rem,9rem)_1fr] border-b border-border last:border-b-0"
            >
              <div
                v-if="selectionForm[record.formKey]"
                class="flex items-center border-r border-border px-3 py-2"
              >
                <span class="truncate" :title="selectionFieldLabel(record)">
                  {{ selectionFieldLabel(record) }}
                </span>
              </div>
              <div
                v-if="selectionForm[record.formKey]"
                class="border-r border-border px-3 py-2"
              >
                <select
                  v-model.number="selectionForm[record.formKey].filterOperator"
                  class="w-full min-w-[8rem] rounded-md border border-border bg-container px-2 py-1.5 text-sm text-text outline-none focus:border-primary"
                >
                  <option
                    v-for="opt in filterOperatorOptions"
                    :key="opt.value"
                    :value="opt.value"
                  >
                    {{ opt.label }}
                  </option>
                </select>
              </div>
              <div
                v-if="selectionForm[record.formKey]"
                class="px-3 py-2"
              >
                <div
                  v-if="selectionForm[record.formKey].filterOperator === 8"
                  class="flex min-w-0 items-center gap-2"
                >
                  <input
                    v-model="selectionForm[record.formKey].value"
                    type="text"
                    class="min-w-0 flex-1 rounded-md border border-border bg-container px-2 py-1.5 text-sm text-text outline-none focus:border-primary"
                  />
                  <span class="shrink-0 text-sm text-text-secondary">~</span>
                  <input
                    v-model="selectionForm[record.formKey].valueTo"
                    type="text"
                    class="min-w-0 flex-1 rounded-md border border-border bg-container px-2 py-1.5 text-sm text-text outline-none focus:border-primary"
                    :placeholder="t('statistics.quickquery.configurable.page.valueto')"
                  />
                </div>
                <input
                  v-else
                  v-model="selectionForm[record.formKey].value"
                  type="text"
                  class="w-full rounded-md border border-border bg-container px-2 py-1.5 text-sm text-text outline-none focus:border-primary"
                />
              </div>
            </div>
          </div>
          <div class="flex flex-wrap items-center gap-3">
            <span class="text-sm text-text">
              {{ t('statistics.quickquery.configurable.page.rowlimit') }}
            </span>
            <input
              v-model.number="rowLimit"
              type="number"
              min="1"
              :max="maxRowLimit"
              class="w-40 rounded-md border border-border bg-container px-2 py-1.5 text-sm text-text outline-none focus:border-primary"
            />
          </div>
        </div>
      </div>
      <div v-if="activeTabKey === 'result'">
        <div v-if="queryResult" class="flex min-h-0 flex-col">
          <a-table
            :columns="resultColumns"
            :data-source="queryResult.rows"
            :pagination="false"
            :scroll="{ x: 'max-content', y: resultTableScrollY }"
            size="small"
            bordered
            :row-key="resultRowKey"
          />
          <TaktPagination
            v-if="paginationReady"
            v-model:current="resultPageIndex"
            v-model:page-size="resultPageSize"
            :total="queryResult.total"
            @change="handleResultPageChange"
            @show-size-change="handleResultPageSizeChange"
          />
        </div>
        <a-empty v-else :description="t('statistics.quickquery.configurable.page.runpage.resultempty')" />
      </div>
    </a-spin>
  </div>
</template>

<script setup lang="ts">
/**
 * 定制报表独立执行页：筛选条件与查询结果分视图展示（路由 query.id / query.name）
 */
import { useRoute, useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { RiSearchLine, RiRefreshLine, RiExportLine, RiArrowLeftLine } from '@remixicon/vue'
import { useConfigurableRun } from '../composables/useConfigurableRun'
import { ensureTaktPaginationConfigAsync } from '@/utils/taktPaged'

/** 路由 */
const route = useRoute()
/** 路由导航 */
const router = useRouter()
/** i18n */
const { t } = useI18n()

/**
 * 解析路由 query 字符串
 * @param raw 原始 query 值
 * @returns {string | undefined} 去空白后的字符串
 */
function parseRouteQueryString(raw: unknown): string | undefined {
  if (Array.isArray(raw)) {
    return raw[0]?.trim() || undefined
  }
  return typeof raw === 'string' ? raw.trim() || undefined : undefined
}

/** 定制报表主键（来自路由 query.id） */
const configurableId = computed(() => parseRouteQueryString(route.query.id))
/** 定制报表名称（来自路由 query.name） */
const configurableName = computed(() => parseRouteQueryString(route.query.name))
/** 当前视图（selection | result） */
const activeTabKey = ref<'selection' | 'result'>('selection')
/** 分页平台配置已就绪（TaktPagination 依赖） */
const paginationReady = ref(false)
/** 结果表格纵向滚动高度 */
const resultTableScrollY = computed(() => Math.max(320, window.innerHeight - 320))

const {
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
} = useConfigurableRun(configurableId, configurableName)

/**
 * 重置筛选条件并回到筛选视图
 */
function handleReset(): void {
  resetSelectionForm()
  activeTabKey.value = 'selection'
}

/**
 * 确保分页配置已加载
 */
async function ensurePaginationReady(): Promise<void> {
  await ensureTaktPaginationConfigAsync()
  paginationReady.value = true
}

/**
 * 查询并切换到结果视图
 */
async function handleQueryAndShowResult(): Promise<void> {
  await ensurePaginationReady()
  const ok = await handleQuery()
  if (ok) {
    activeTabKey.value = 'result'
  }
}

/**
 * 返回报表列表
 */
function handleBack(): void {
  router.push('/statistics/quick-query/configurable')
}

watch(
  () => configurableId.value,
  (id) => {
    if (id) {
      activeTabKey.value = 'selection'
      void loadRuntimeScreen()
    }
  }
)

onMounted(async () => {
  if (!configurableId.value) {
    router.replace('/statistics/quick-query/configurable')
    return
  }
  await ensurePaginationReady()
  void loadRuntimeScreen()
})
</script>
