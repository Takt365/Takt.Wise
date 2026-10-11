<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/workspace/modules -->
<!-- 文件名称：TodoModule.vue -->
<!-- 功能描述：工作台待办模块（流程引擎待办摘要，跳转待办页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="todo-module">
    <a-spin :spinning="loading">
      <a-empty
        v-if="!loading && items.length === 0"
        :description="t('dashboard.workspace.page.todoplaceholder')"
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
              class="todo-module__item"
              @click="goTodoPage"
            >
              <a-list-item-meta>
                <template #title>
                  <span class="todo-module__title">{{ item.processTitle || item.processName }}</span>
                </template>
                <template #description>
                  <span class="todo-module__desc">
                    <template v-if="item.taskName">{{ item.taskName }} · </template>
                    <template v-if="item.startUserName">{{ item.startUserName }} · </template>
                    {{ formatDateTime(item.startTime) }}
                  </span>
                </template>
              </a-list-item-meta>
            </a-list-item>
          </template>
        </a-list>
        <div
          v-if="total > 0"
          class="todo-module__footer"
        >
          <a-button
            type="link"
            size="small"
            @click="goTodoPage"
          >
            {{ t('dashboard.workspace.page.viewall') }}
            <span v-if="total > items.length"> ({{ total }})</span>
          </a-button>
        </div>
      </template>
    </a-spin>
  </div>
</template>

<script setup lang="ts">
/**
 * 工作台待办模块：拉取当前用户流程待办摘要并跳转待办列表页
 */
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { Empty } from 'ant-design-vue'
import dayjs from 'dayjs'
import { getFlowEngineTodoList } from '@/api/workflow/flowEngine.api'
import type { FlowTodoItem } from '@/types/workflow/flowEngine.types'
import { getTaktDefaultPageIndex } from '@/utils/taktPaged'

/** 工作台模块列表条数上限 */
const WORKSPACE_MODULE_LIST_SIZE = 8

const TODO_LIST_ROUTE = '/workflow/todo'

const router = useRouter()
const { t } = useI18n()

/** 列表 loading */
const loading = ref(false)
/** 待办条目 */
const items = ref<FlowTodoItem[]>([])
/** 服务端总数 */
const total = ref(0)

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
 * 加载待办摘要
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  loading.value = true
  try {
    const res = await getFlowEngineTodoList({
      pageIndex: getTaktDefaultPageIndex(),
      pageSize: WORKSPACE_MODULE_LIST_SIZE,
    })
    items.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    logger.error('[TodoModule] 加载待办失败', { error })
    items.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/**
 * 跳转待办列表页
 */
function goTodoPage(): void {
  router.push(TODO_LIST_ROUTE)
}

onMounted(() => {
  void loadData()
})

useTableRefresh(loadData)
</script>

<style scoped lang="css">
.todo-module {
  padding: 0;
  min-height: 128px;
}
.todo-module__item {
  cursor: pointer;
  padding-inline: 0 !important;
  transition: background-color 0.2s;
  border-radius: 6px;
  &:hover {
    background: var(--ant-color-fill-tertiary);
  }
}
.todo-module__title {
  display: block;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  color: var(--ant-color-text);
}
.todo-module__desc {
  display: block;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  font-size: 12px;
}
.todo-module__footer {
  margin-top: 4px;
  text-align: right;
}
</style>
