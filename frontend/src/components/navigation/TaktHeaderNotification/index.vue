<!-- ========================================
项目名称:TaktWise
命名空间:@/components/navigation/TaktHeaderNotification
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:通知消息组件,显示系统通知列表

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <a-badge 
    v-if="dot || count > 0"
    :dot="dot"
    :count="dot ? undefined : count"
    :overflow-count="overflowCount"
    :show-zero="false"
  >
    <a-button
      type="text"
      class="takt-header-notification"
      @click="handleClick"
    >
      <template #icon>
        <RiNotificationLine class="takt-remix-icon" />
      </template>
    </a-button>
  </a-badge>
  <a-button 
    v-else
    type="text" 
    class="takt-header-notification" 
    @click="handleClick"
  >
    <template #icon>
      <RiNotificationLine class="takt-remix-icon" />
    </template>
  </a-button>
  <a-drawer
    v-model:open="visible"
    :title="$t('components.navigation.page.systemsetting.notification')"
    placement="right"
    :width="400"
    class="takt-header-notification-drawer"
  >
    <template #extra>
      <a-button
        type="text"
        @click="handleClearAll"
      >
        {{ $t('common.page.button.emptyall') }}
      </a-button>
    </template>
    <a-list
      :data-source="notifications"
      :loading="loading"
      :pagination="pagination"
    >
      <template #renderItem="{ item }">
        <a-list-item>
          <a-list-item-meta>
            <template #title>
              <span :class="{ 'unread': !item.read }">{{ item.title }}</span>
            </template>
            <template #description>
              <div>{{ item.content }}</div>
              <div class="notification-time">
                {{ item.time }}
              </div>
            </template>
          </a-list-item-meta>
          <template #actions>
            <a-button
              v-if="!item.read"
              type="text"
              size="small"
              @click="handleMarkRead(item.id)"
            >
              {{ $t('common.page.button.read') }}
            </a-button>
            <a-button
              type="text"
              size="small"
              danger
              @click="handleDelete(item.id)"
            >
              {{ $t('common.page.button.delete') }}
            </a-button>
          </template>
        </a-list-item>
      </template>
    </a-list>
  </a-drawer>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageSize } from '@/utils/taktPaged'
import { RiNotificationLine } from '@remixicon/vue'

const { t } = useI18n()

interface Notification {
  id: string
  title: string
  content: string
  time: string
  read: boolean
}

interface Props {
  notifications?: Notification[]
  dot?: boolean
  overflowCount?: number
}

const props = withDefaults(defineProps<Props>(), {
  notifications: () => [],
  dot: false,
  overflowCount: 99
})

const emit = defineEmits<{
  'click': []
  'read': [id: string]
  'delete': [id: string]
  'clear-all': []
}>()

const visible = ref(false)
const loading = ref(false)

const notifications = computed(() => props.notifications)

const count = computed(() => {
  return notifications.value.filter(n => !n.read).length
})

const pagination = computed(() => ({
  pageSize: getTaktDefaultPageSize(),
  showSizeChanger: false,
  showTotal: (total: number) => t('common.table.total', { total })
}))

const handleClick = () => {
  visible.value = true
  emit('click')
}

const handleMarkRead = (id: string) => {
  emit('read', id)
}

const handleDelete = (id: string) => {
  emit('delete', id)
}

const handleClearAll = () => {
  emit('clear-all')
}
</script>

<style scoped>
.takt-header-notification-drawer {
  :deep(.ant-drawer-content-wrapper) {
    right: 16px;
  }
}
</style>
