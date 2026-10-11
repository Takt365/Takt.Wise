<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/workspace/modules -->
<!-- 文件名称：NoticeModule.vue -->
<!-- 功能描述：工作台通知公告模块（已发布公告摘要与详情预览） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="notice-module">
    <a-spin :spinning="loading">
      <a-empty
        v-if="!loading && items.length === 0"
        :description="t('dashboard.workspace.page.noticeplaceholder')"
        :image="Empty.PRESENTED_IMAGE_SIMPLE"
      />
      <template v-else>
        <a-list
          :data-source="items"
          size="small"
          :split="false"
        >
          <template #renderItem="{ item }">
            <a-list-item
              class="notice-module__item"
              @click="openDetail(item)"
            >
              <a-list-item-meta>
                <template #title>
                  <span class="notice-module__title-row">
                    <a-tag
                      v-if="item.isTop === 1"
                      color="processing"
                      class="notice-module__top-tag"
                    >
                      {{ t('dashboard.workspace.page.toptag') }}
                    </a-tag>
                    <span class="notice-module__title">{{ item.announcementTitle }}</span>
                  </span>
                </template>
                <template #description>
                  <span class="notice-module__desc">
                    {{ resolveSummary(item) }}
                    <template v-if="item.publishTime">
                      · {{ formatDateTime(item.publishTime) }}
                    </template>
                  </span>
                </template>
              </a-list-item-meta>
            </a-list-item>
          </template>
        </a-list>
        <div
          v-if="total > 0"
          class="notice-module__footer"
        >
          <a-button
            type="link"
            size="small"
            @click="goAnnouncementPage"
          >
            {{ t('dashboard.workspace.page.viewall') }}
            <span v-if="total > items.length"> ({{ total }})</span>
          </a-button>
        </div>
      </template>
    </a-spin>
    <a-modal
      v-model:open="detailVisible"
      :title="detailItem?.announcementTitle"
      :footer="null"
      :width="640"
      destroy-on-close
    >
      <div
        v-if="detailItem?.publishTime"
        class="notice-module__detail-meta"
      >
        {{ t('entity.announcement.publishtime') }}：{{ formatDateTime(detailItem.publishTime) }}
      </div>
      <div
        v-if="detailLoading"
        class="notice-module__detail-loading"
      >
        <a-spin />
      </div>
      <div
        v-else-if="detailContent"
        class="notice-module__detail-content"
        v-html="detailContent"
      />
      <div
        v-else
        class="notice-module__detail-empty"
      >
        {{ resolveSummary(detailItem) }}
      </div>
    </a-modal>
  </div>
</template>

<script setup lang="ts">
/**
 * 工作台通知公告模块：展示已发布公告摘要，支持弹窗预览详情
 */
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { Empty } from 'ant-design-vue'
import dayjs from 'dayjs'
import { getAnnouncementList, getAnnouncementById } from '@/api/routine/announcement.api'
import type { Announcement } from '@/types/routine/announcement.types'
import { getTaktDefaultPageIndex } from '@/utils/taktPaged'

/** 工作台模块列表条数上限 */
const WORKSPACE_MODULE_LIST_SIZE = 8

/** sys_publish_status：已发布 */
const ANNOUNCEMENT_STATUS_PUBLISHED = 1

const ANNOUNCEMENT_LIST_ROUTE = '/routine/announcement'

const router = useRouter()
const { t } = useI18n()

/** 列表 loading */
const loading = ref(false)
/** 公告条目 */
const items = ref<Announcement[]>([])
/** 服务端总数 */
const total = ref(0)
/** 详情弹窗可见 */
const detailVisible = ref(false)
/** 详情 loading */
const detailLoading = ref(false)
/** 当前列表项（标题等） */
const detailItem = ref<Announcement | null>(null)
/** 详情 HTML 内容 */
const detailContent = ref('')

/**
 * 格式化日期时间
 * @param value ISO 时间字符串
 * @returns {string} 展示文本
 */
function formatDateTime(value?: string): string {
  if (!value?.trim()) {
    return ''
  }
  const parsed = dayjs(value)
  return parsed.isValid() ? parsed.format('YYYY-MM-DD HH:mm') : value
}

/**
 * 列表摘要文案
 * @param item 公告 DTO
 * @returns {string} 摘要
 */
function resolveSummary(item: Announcement | null | undefined): string {
  if (!item) {
    return ''
  }
  const summary = item.summary?.trim()
  if (summary) {
    return summary
  }
  const plain = item.content?.replace(/<[^>]+>/g, ' ').replace(/\s+/g, ' ').trim()
  if (!plain) {
    return ''
  }
  return plain.length > 80 ? `${plain.slice(0, 80)}…` : plain
}

/**
 * 加载已发布公告摘要
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  loading.value = true
  try {
    const res = await getAnnouncementList({
      pageIndex: getTaktDefaultPageIndex(),
      pageSize: WORKSPACE_MODULE_LIST_SIZE,
      announcementStatus: ANNOUNCEMENT_STATUS_PUBLISHED,
    })
    items.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    logger.error('[NoticeModule] 加载公告失败', { error })
    items.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/**
 * 打开公告详情预览
 * @param item 列表项
 * @returns {Promise<void>}
 */
async function openDetail(item: Announcement): Promise<void> {
  detailItem.value = item
  detailContent.value = item.content ?? ''
  detailVisible.value = true
  if (item.content?.trim()) {
    return
  }
  detailLoading.value = true
  try {
    const detail = await getAnnouncementById(item.announcementId)
    detailItem.value = detail
    detailContent.value = detail.content ?? ''
  } catch (error: unknown) {
    logger.error('[NoticeModule] 加载公告详情失败', { error, id: item.announcementId })
  } finally {
    detailLoading.value = false
  }
}

/**
 * 跳转公告管理页
 */
function goAnnouncementPage(): void {
  router.push(ANNOUNCEMENT_LIST_ROUTE)
}

onMounted(() => {
  void loadData()
})

useTableRefresh(loadData)
</script>

<style scoped lang="css">
.notice-module {
  padding: 0;
  min-height: 128px;
}
.notice-module__item {
  cursor: pointer;
  padding-inline: 0 !important;
  transition: background-color 0.2s;
  border-radius: 6px;
  &:hover {
    background: var(--ant-color-fill-tertiary);
  }
}
.notice-module__title-row {
  display: flex;
  align-items: center;
  gap: 6px;
  min-width: 0;
}
.notice-module__top-tag {
  flex-shrink: 0;
  margin-inline-end: 0;
}
.notice-module__title {
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  color: var(--ant-color-text);
}
.notice-module__desc {
  display: block;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  font-size: 12px;
}
.notice-module__footer {
  margin-top: 4px;
  text-align: right;
}
.notice-module__detail-meta {
  margin-bottom: 12px;
  font-size: 12px;
  color: var(--ant-color-text-secondary);
}
.notice-module__detail-loading {
  display: flex;
  justify-content: center;
  padding: 24px 0;
}
.notice-module__detail-content {
  max-height: 420px;
  overflow: auto;
  line-height: 1.6;
  word-break: break-word;
}
.notice-module__detail-empty {
  color: var(--ant-color-text-secondary);
  font-size: 13px;
}
</style>
