<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/routine/help-desk/my-asset -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：服务台「我的资产」页；按当前用户工单的 AssetCode 聚合，关联 TaktAsset -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />
    <TaktSingleTable
      :columns="columns"
      entity-scope="company"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getAssetRowKey"
      :pagination="false"
    />
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 服务台我的资产页（当前用户工单 AssetCode 聚合）
 * @module views/routine/help-desk/my-asset
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { getMyTicketAssetList } from '@/api/routine/help-desk/ticket.api'
import type { TicketMyAsset } from '@/types/routine/help-desk/ticket.types'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 快捷查询占位 */
const searchPlaceholder = computed(() =>
  t('common.page.form.placeholder.search', { keyword: t('entity.ticket.assetcode') })
)
/** 查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 列表数据 */
const dataSource = ref<TicketMyAsset[]>([])
/** 当前页 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 总数 */
const total = ref(0)
/** 表格列 */
const columns = computed<TableColumnsType>(() => [
  { title: t('entity.ticket.assetcode'), dataIndex: 'assetCode', key: 'assetCode', width: 140, ellipsis: true },
  { title: t('entity.asset.name'), dataIndex: 'assetName', key: 'assetName', width: 180, ellipsis: true },
  { title: t('routine.help-desk.my-asset.page.ticket.count'), dataIndex: 'ticketCount', key: 'ticketCount', width: 120 },
  { title: t('routine.help-desk.my-asset.page.last.ticket.at'), dataIndex: 'lastTicketAt', key: 'lastTicketAt', width: 180, ellipsis: true }])
/** row-key */
const getAssetRowKey = (record: TicketMyAsset): string => record.assetCode ?? ''
/**
 * 加载当前用户工单关联的资产汇总
 */
async function loadData() {
  loading.value = true
  try {
    const kw = (queryKeyword.value ?? '').trim()
    const params: Record<string, unknown> = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
    }
    if (kw) {
      params.keyWords = kw
    }
    const res = await getMyTicketAssetList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/** 查询 */
function handleSearch() {
  currentPage.value = 1
  loadData()
}
/** 重置 */
function handleReset() {
  queryKeyword.value = ''
  currentPage.value = 1
  loadData()
}

/** 外置分页：翻页 */
function handlePaginationChange(page: number) {
  currentPage.value = page
  loadData()
}

/** 外置分页：改每页条数时回到第 1 页 */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = 1
  pageSize.value = size
  loadData()
}

onMounted(() => {
  loadData()
})
</script>
