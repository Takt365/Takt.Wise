<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/routine/news-center/news/components -->
<!-- 文件名称：NewsEngagementPanel.vue -->
<!-- 功能描述：新闻右侧互动明细（点赞/收藏/分享）：按选中 newsId 分页列表与删除；defineExpose reload -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="news-engagement-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktToolsBar
      :delete-permission="deletePermission"
      :show-create="false"
      :show-update="false"
      :show-delete="true"
      :show-import="false"
      :show-export="false"
      :show-expand="false"
      :show-advanced-query="false"
      :show-column-setting="false"
      :show-fullscreen="false"
      :show-refresh="true"
      :delete-disabled="deleteDisabled"
      :delete-loading="loading"
      :refresh-loading="loading"
      @delete="handleDelete"
      @refresh="handleRefresh"
    />
    <div
      ref="detailTableWrapRef"
      class="news-engagement-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getRowId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="masterDetailDetail"
        :scroll="{ y: detailTableScrollY }"
        :show-row-selection="true"
        @change="handleTableChange"
        @pagination-change="handlePaginationChange"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 新闻互动记录面板（点赞 / 收藏 / 分享）
 * @module views/routine/news-center/news/components/news-engagement-panel
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { measureMasterDetailLrTableScrollY } from '@/composables/useTaktMasterDetailLrScrollY'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'
import { RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import { useNewsMasterContext } from '../composables/useNewsMasterContext'
import {
  getNewsLikeList,
  deleteNewsLikeById,
  deleteNewsLikeBatch,
} from '@/api/routine/news-center/newsLike.api'
import {
  getNewsFavoriteList,
  deleteNewsFavoriteById,
  deleteNewsFavoriteBatch,
} from '@/api/routine/news-center/newsFavorite.api'
import {
  getNewsShareList,
  deleteNewsShareById,
  deleteNewsShareBatch,
} from '@/api/routine/news-center/newsShare.api'
import type { NewsLike } from '@/types/routine/news-center/newsLike.types'
import type { NewsFavorite } from '@/types/routine/news-center/newsFavorite.types'
import type { NewsShare } from '@/types/routine/news-center/newsShare.types'

/** 互动类型 */
export type NewsEngagementKind = 'like' | 'favorite' | 'share'

const props = defineProps<{
  /** 互动类型：点赞 / 收藏 / 分享 */
  engagement: NewsEngagementKind
}>()

const { t } = useI18n()
const { selectedMasterRow } = useNewsMasterContext()

/** 行数据类型（三类互动记录并集） */
type EngagementRow = NewsLike | NewsFavorite | NewsShare | Record<string, unknown>

/** 列表 loading */
const loading = ref(false)
/** 分页数据 */
const dataSource = ref<EngagementRow[]>([])
/** 当前页 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 页大小 */
const pageSize = ref(getTaktDefaultPageSize())
/** 总数 */
const total = ref(0)
/** 多选 keys */
const selectedRowKeys = ref<(string | number)[]>([])
/** 多选行 */
const selectedRows = ref<EngagementRow[]>([])
/** 单选当前行 */
const selectedRow = ref<EngagementRow | null>(null)

/** 子表滚动容器 */
const detailTableWrapRef = ref<HTMLElement | null>(null)
/** 子表 scroll.y */
const detailTableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let detailTableScrollResizeObserver: ResizeObserver | null = null

/** 当前选中新闻 Id（string） */
const masterNewsId = computed(() => {
  const row = selectedMasterRow.value as Record<string, unknown> | null
  const id = row?.newsId ?? row?.id
  return id != null && String(id).length > 0 ? String(id) : ''
})

/** 是否已选中主表行 */
const hasMasterSelection = computed(() => masterNewsId.value.length > 0)

/** 删除按钮禁用 */
const deleteDisabled = computed(
  () => !hasMasterSelection.value || (selectedRowKeys.value.length === 0 && !selectedRow.value),
)

/** 按互动类型使用附属权限码（TaktMenuOtherSeedData） */
const deletePermission = computed(() => `routine:news:center:${props.engagement}:delete`)

/**
 * 取行主键
 * @param {EngagementRow} record 行
 * @returns {string} Id
 */
function getRowId(record: EngagementRow): string {
  const r = record as Record<string, unknown>
  if (props.engagement === 'like') {
    return String(r.newsLikeId ?? r.id ?? '')
  }
  if (props.engagement === 'favorite') {
    return String(r.newsFavoriteId ?? r.id ?? '')
  }
  return String(r.newsShareId ?? r.id ?? '')
}

/**
 * 互动发生时间字段值
 * @param {EngagementRow} record 行
 * @returns {string} 时间
 */
function getEngagementTime(record: EngagementRow): string {
  const r = record as Record<string, unknown>
  if (props.engagement === 'like') {
    return String(r.likeTime ?? '')
  }
  if (props.engagement === 'favorite') {
    return String(r.favoriteTime ?? '')
  }
  return String(r.shareTime ?? '')
}

/** 列定义 */
const columns = computed<TableColumnsType>(() => {
  const cols: TableColumnsType = [
    {
      title: t('entity.user.userName'),
      dataIndex: 'userName',
      key: 'userName',
      width: 140,
      ellipsis: true,
    },
    {
      title: t('routine.news.center.news.page.engagement.time'),
      key: 'engagementTime',
      width: 180,
      customRender: ({ record }) => getEngagementTime(record as EngagementRow),
    },
  ]
  if (props.engagement === 'share') {
    cols.push({
      title: t('routine.news.center.news.page.engagement.channel'),
      dataIndex: 'shareChannel',
      key: 'shareChannel',
      width: 140,
      ellipsis: true,
    })
  }
  cols.push(
    CreateActionColumn({
      title: t('common.page.column.action'),
      width: 88,
      actions: [
        {
          key: 'delete',
          icon: RiDeleteBinLine,
          permission: `routine:news:center:${props.engagement}:delete`,
          confirm: true,
          confirmTitle: () => t('common.dialog.title.delete', { entity: tabLabel.value }),
          confirmIcon: RiQuestionLine,
          onClick: (record: EngagementRow) => handleDeleteRow(record),
        },
      ],
    }) as any,
  )
  return cols
})

/** Tab/实体短名（删除确认） */
const tabLabel = computed(() => {
  if (props.engagement === 'like') {
    return t('routine.news.center.news.page.tabs.like')
  }
  if (props.engagement === 'favorite') {
    return t('routine.news.center.news.page.tabs.favorite')
  }
  return t('routine.news.center.news.page.tabs.share')
})

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EngagementRow[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
  },
}))

/**
 * 行点击选中
 * @param {EngagementRow} record 行
 * @returns {Record<string, unknown>} 行事件
 */
function onClickRow(record: EngagementRow) {
  return {
    onClick: () => {
      selectedRow.value = record
    },
  }
}

/**
 * 按容器重算 scroll.y
 * @returns {void}
 */
function recalcDetailTableScrollY(): void {
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollY.value = measureMasterDetailLrTableScrollY(wrap)
}

/**
 * 监听子表容器尺寸
 * @returns {void}
 */
function startDetailTableScrollObserve(): void {
  stopDetailTableScrollObserve()
  recalcDetailTableScrollY()
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollResizeObserver = new ResizeObserver(() => {
    recalcDetailTableScrollY()
  })
  detailTableScrollResizeObserver.observe(wrap)
}

/**
 * 停止监听
 * @returns {void}
 */
function stopDetailTableScrollObserve(): void {
  detailTableScrollResizeObserver?.disconnect()
  detailTableScrollResizeObserver = null
}

/**
 * 清空列表选择与数据
 * @returns {void}
 */
function clearListState(): void {
  dataSource.value = []
  total.value = 0
  selectedRowKeys.value = []
  selectedRows.value = []
  selectedRow.value = null
}

/**
 * 加载分页列表（依赖主表 newsId）
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  if (!hasMasterSelection.value) {
    clearListState()
    return
  }
  loading.value = true
  try {
    const query = {
      newsId: masterNewsId.value,
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
    }
    let result: { items?: EngagementRow[]; totalCount?: number; total?: number } | null = null
    if (props.engagement === 'like') {
      result = await getNewsLikeList(query) as any
    } else if (props.engagement === 'favorite') {
      result = await getNewsFavoriteList(query) as any
    } else {
      result = await getNewsShareList(query) as any
    }
    dataSource.value = (result?.items ?? []) as EngagementRow[]
    total.value = Number(result?.totalCount ?? result?.total ?? 0)
  } finally {
    loading.value = false
  }
}

/**
 * 对外刷新
 * @returns {Promise<void>}
 */
async function reload(): Promise<void> {
  currentPage.value = getTaktDefaultPageIndex()
  await loadData()
}

/**
 * 工具栏刷新
 * @returns {Promise<void>}
 */
async function handleRefresh(): Promise<void> {
  await loadData()
}

/**
 * 表格排序/筛选变化
 * @returns {void}
 */
function handleTableChange(): void {
  // 服务端分页；排序若后端支持再扩展
}

/**
 * 分页变化
 * @param {number} page 页码
 * @param {number} size 页大小
 * @returns {Promise<void>}
 */
async function handlePaginationChange(page: number, size: number): Promise<void> {
  currentPage.value = page
  pageSize.value = size
  await loadData()
}

/**
 * 删除单行
 * @param {EngagementRow} record 行
 * @returns {Promise<void>}
 */
async function handleDeleteRow(record: EngagementRow): Promise<void> {
  const id = getRowId(record)
  if (!id) {
    return
  }
  loading.value = true
  try {
    if (props.engagement === 'like') {
      await deleteNewsLikeById(id)
    } else if (props.engagement === 'favorite') {
      await deleteNewsFavoriteById(id)
    } else {
      await deleteNewsShareById(id)
    }
    message.success(t('common.feedback.deleted', { target: tabLabel.value }))
    await loadData()
  } finally {
    loading.value = false
  }
}

/**
 * 工具栏删除（单选或批删）
 * @returns {void}
 */
function handleDelete(): void {
  const ids = selectedRowKeys.value.map(String).filter(Boolean)
  if (ids.length === 0 && selectedRow.value) {
    ids.push(getRowId(selectedRow.value))
  }
  if (ids.length === 0) {
    return
  }
  Modal.confirm({
    title: t('common.dialog.title.delete', { entity: tabLabel.value }),
    content: t('common.dialog.content.deleteConfirm', { count: ids.length }),
    okType: 'danger',
    onOk: async () => {
      loading.value = true
      try {
        if (ids.length === 1) {
          if (props.engagement === 'like') {
            await deleteNewsLikeById(ids[0])
          } else if (props.engagement === 'favorite') {
            await deleteNewsFavoriteById(ids[0])
          } else {
            await deleteNewsShareById(ids[0])
          }
        } else if (props.engagement === 'like') {
          await deleteNewsLikeBatch(ids)
        } else if (props.engagement === 'favorite') {
          await deleteNewsFavoriteBatch(ids)
        } else {
          await deleteNewsShareBatch(ids)
        }
        message.success(t('common.feedback.deleted', { target: tabLabel.value }))
        selectedRowKeys.value = []
        selectedRows.value = []
        selectedRow.value = null
        await loadData()
      } finally {
        loading.value = false
      }
    },
  })
}

watch(masterNewsId, async () => {
  currentPage.value = getTaktDefaultPageIndex()
  await loadData()
})

watch(
  () => props.engagement,
  async () => {
    currentPage.value = getTaktDefaultPageIndex()
    await loadData()
  },
)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  startDetailTableScrollObserve()
  await loadData()
})

onBeforeUnmount(() => {
  stopDetailTableScrollObserve()
})

defineExpose({
  reload,
  loadData,
})
</script>
