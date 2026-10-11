<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/routine/help-desk/ticket/components -->
<!-- 文件名称：TicketWorkflowDrawer.vue -->
<!-- 功能描述：工单 ITSM 工作流抽屉（状态操作、会话回复） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-drawer
    v-model:open="visible"
    :title="t('routine.help-desk.ticket.page.workflow.title')"
    width="640"
    destroy-on-close
    @close="handleClose"
  >
    <a-spin :spinning="loading">
      <template v-if="ticket">
        <a-descriptions :column="2" size="small" bordered class="mb-4">
          <a-descriptions-item :label="t('entity.ticket.no')">{{ ticket.ticketCode }}</a-descriptions-item>
          <a-descriptions-item :label="t('entity.ticket.status')">
            <a-tag :color="statusColor(ticket.ticketStatus)">{{ statusLabel(ticket.ticketStatus) }}</a-tag>
          </a-descriptions-item>
          <a-descriptions-item :label="t('entity.ticket.title')" :span="2">{{ ticket.title }}</a-descriptions-item>
          <a-descriptions-item :label="t('entity.ticket.submittername')">{{ ticket.submitterName }}</a-descriptions-item>
          <a-descriptions-item :label="t('entity.ticket.assigneename')">{{ ticket.assigneeName || '—' }}</a-descriptions-item>
        </a-descriptions>

        <!-- 工作流操作 -->
        <div class="mb-4 flex flex-wrap gap-2">
          <a-button
            v-if="!props.portalMode && canPick"
            v-permission="props.workflowPermission"
            type="primary"
            :loading="actionLoading"
            @click="handlePick(true)"
          >
            {{ t('routine.help-desk.ticket.page.action.pick') }}
          </a-button>
          <a-button
            v-if="!props.portalMode && canStart"
            v-permission="props.workflowPermission"
            :loading="actionLoading"
            @click="handleStart"
          >
            {{ t('routine.help-desk.ticket.page.action.start') }}
          </a-button>
          <a-button
            v-if="!props.portalMode && canWait"
            v-permission="props.workflowPermission"
            :loading="actionLoading"
            @click="handleWait"
          >
            {{ t('routine.help-desk.ticket.page.action.wait') }}
          </a-button>
          <a-button
            v-if="!props.portalMode && canResolve"
            v-permission="props.workflowPermission"
            :loading="actionLoading"
            @click="handleResolve"
          >
            {{ t('routine.help-desk.ticket.page.action.resolve') }}
          </a-button>
          <a-button
            v-if="canConfirmClose"
            v-permission="props.workflowPermission"
            type="primary"
            :loading="actionLoading"
            @click="handleConfirmClose"
          >
            {{ t('routine.help-desk.ticket.page.action.confirm.close') }}
          </a-button>
          <a-button
            v-if="!props.portalMode && canReopen"
            v-permission="props.workflowPermission"
            danger
            :loading="actionLoading"
            @click="handleReopen"
          >
            {{ t('routine.help-desk.ticket.page.action.reopen') }}
          </a-button>
        </div>

        <!-- 回复区 -->
        <div class="mb-2 text-sm font-medium">{{ t('routine.help-desk.ticket.page.replies') }}</div>
        <a-list
          v-if="replies.length"
          size="small"
          bordered
          :data-source="replies"
          class="mb-4 max-h-64 overflow-y-auto"
        >
          <template #renderItem="{ item }">
            <a-list-item>
              <a-list-item-meta
                :title="`${item.authorName || item.authorId} · ${authorTypeLabel(item.authorType)}`"
                :description="item.createdAt"
              />
              <div class="whitespace-pre-wrap text-sm">{{ item.content }}</div>
            </a-list-item>
          </template>
        </a-list>
        <a-empty v-else class="mb-4" />

        <a-form layout="vertical">
          <a-form-item :label="t('routine.help-desk.ticket.page.reply.placeholder')">
            <a-textarea v-model:value="replyContent" :rows="3" :disabled="actionLoading" />
          </a-form-item>
          <a-form-item v-if="props.showInternalNote" v-permission="props.workflowPermission">
            <a-checkbox v-model:checked="isInternal">{{ t('routine.help-desk.ticket.page.internal.note') }}</a-checkbox>
          </a-form-item>
          <a-button
            v-permission="props.workflowPermission"
            type="primary"
            :loading="actionLoading"
            :disabled="!replyContent.trim()"
            @click="handleReply"
          >
            {{ t('routine.help-desk.ticket.page.action.reply') }}
          </a-button>
        </a-form>
      </template>
    </a-spin>
  </a-drawer>
</template>

<script setup lang="ts">
/**
 * 工单 ITSM 工作流抽屉
 * @module views/routine/help-desk/ticket/components/ticket-workflow-drawer
 */
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import {
  getTicketById,
  getTicketReplyList,
  getMyTicketById,
  getMyTicketReplyList,
  assignTicket,
  startTicketProgress,
  waitForRequester,
  resolveTicket,
  confirmCloseTicket,
  reopenTicket,
  replyTicket,
  replyMyTicket,
} from '@/api/routine/help-desk/ticket.api'
import type { Ticket } from '@/types/routine/help-desk/ticket.types'
import type { TicketReply } from '@/types/routine/help-desk/ticketReply.types'

/** 抽屉 open（v-model） */
const visible = defineModel<boolean>('open', { default: false })

/** 工单 ID */
const props = withDefaults(
  defineProps<{
    ticketId?: string | null
    /** 工作流/回复按钮权限码（门户页传 routine:help:desk:my:ticket:list） */
    workflowPermission?: string
    /** 是否显示内部备注勾选项（客服端 true，门户 false） */
    showInternalNote?: boolean
    /** 门户模式：走 my-tickets 专用 API */
    portalMode?: boolean
  }>(),
  {
    workflowPermission: 'routine:help:desk:ticket:update',
    showInternalNote: true,
    portalMode: false,
  },
)

/** 工作流变更后通知父级刷新列表 */
const emit = defineEmits<{
  changed: []
}>()

const { t } = useI18n()

/** 详情 loading */
const loading = ref(false)
/** 动作 loading */
const actionLoading = ref(false)
/** 当前工单 */
const ticket = ref<Ticket | null>(null)
/** 回复列表 */
const replies = ref<TicketReply[]>([])
/** 回复输入 */
const replyContent = ref('')
/** 内部备注 */
const isInternal = ref(false)

/** 状态 → 文案键后缀 */
const STATUS_KEY: Record<number, string> = {
  0: 'open',
  1: 'assigned',
  2: 'inprogress',
  3: 'waiting',
  4: 'resolved',
  5: 'closed',
  6: 'cancelled',
  7: 'reopened',
}

/**
 * 服务台工单状态规范化（旧版 6=重新打开 → 7）
 * @param {number} status 状态值
 * @returns {number} 规范化状态
 */
function normalizeHelpDeskTicketStatus(status: number): number {
  return status === 6 ? 7 : status
}

/**
 * 状态展示文案
 * @param {number} status 状态值
 * @returns {string} 文案
 */
function statusLabel(status: number): string {
  const normalized = normalizeHelpDeskTicketStatus(status)
  const key = STATUS_KEY[normalized] ?? 'open'
  return t(`routine.help-desk.ticket.page.status.${key}`)
}

/**
 * 状态 Tag 颜色
 * @param {number} status 状态值
 * @returns {string} 颜色
 */
function statusColor(status: number): string {
  const normalized = normalizeHelpDeskTicketStatus(status)
  const map: Record<number, string> = {
    0: 'blue',
    1: 'cyan',
    2: 'processing',
    3: 'orange',
    4: 'green',
    5: 'default',
    6: 'default',
    7: 'red',
  }
  return map[normalized] ?? 'default'
}

/**
 * 作者类型文案
 * @param {number} type 作者类型
 * @returns {string} 文案
 */
function authorTypeLabel(type: number): string {
  if (type === 1) return t('routine.help-desk.ticket.page.author.requester')
  if (type === 2) return t('routine.help-desk.ticket.page.author.system')
  return t('routine.help-desk.ticket.page.author.agent')
}

const canPick = computed(() => ticket.value && [0, 7].includes(normalizeHelpDeskTicketStatus(ticket.value.ticketStatus)))
const canStart = computed(() => ticket.value?.ticketStatus === 1)
const canWait = computed(() => ticket.value?.ticketStatus === 2)
const canResolve = computed(() => ticket.value?.ticketStatus === 2)
const canConfirmClose = computed(() => ticket.value?.ticketStatus === 4)
const canReopen = computed(() => ticket.value && [4, 5].includes(ticket.value.ticketStatus))

/**
 * 加载工单详情与回复
 * @returns {Promise<void>}
 */
async function loadDetail(): Promise<void> {
  if (!props.ticketId) {
    ticket.value = null
    replies.value = []
    return
  }
  loading.value = true
  try {
    if (props.portalMode) {
      ticket.value = await getMyTicketById(props.ticketId)
      const page = await getMyTicketReplyList(props.ticketId, {
        pageIndex: 1,
        pageSize: 50,
      })
      replies.value = page.data ?? []
    } else {
      ticket.value = await getTicketById(props.ticketId)
      const page = await getTicketReplyList({
        ticketId: props.ticketId,
        pageIndex: 1,
        pageSize: 50,
        includeInternal: true,
      })
      replies.value = page.data ?? []
    }
  } finally {
    loading.value = false
  }
}

/**
 * 工作流动作包装
 * @param {() => Promise<void>} fn 动作
 * @returns {Promise<void>}
 */
async function runAction(fn: () => Promise<void>): Promise<void> {
  actionLoading.value = true
  try {
    await fn()
    message.success(t('common.feedback.updated'))
    emit('changed')
    await loadDetail()
  } finally {
    actionLoading.value = false
  }
}

/**
 * 领取并开始
 * @param {boolean} startImmediately 是否立即处理
 * @returns {Promise<void>}
 */
async function handlePick(startImmediately: boolean): Promise<void> {
  if (!props.ticketId) return
  await runAction(async () => {
    await assignTicket({
      ticketId: props.ticketId!,
      startImmediately,
    })
  })
}

/** 开始处理 */
async function handleStart(): Promise<void> {
  if (!props.ticketId) return
  await runAction(async () => {
    await startTicketProgress({ ticketId: props.ticketId! })
  })
}

/** 等待用户 */
async function handleWait(): Promise<void> {
  if (!props.ticketId) return
  await runAction(async () => {
    await waitForRequester({ ticketId: props.ticketId! })
  })
}

/** 标记解决 */
async function handleResolve(): Promise<void> {
  if (!props.ticketId) return
  await runAction(async () => {
    await resolveTicket({ ticketId: props.ticketId! })
  })
}

/** 确认关闭 */
async function handleConfirmClose(): Promise<void> {
  if (!props.ticketId) return
  await runAction(async () => {
    await confirmCloseTicket({ ticketId: props.ticketId! })
  })
}

/** 重新打开 */
async function handleReopen(): Promise<void> {
  if (!props.ticketId) return
  await runAction(async () => {
    await reopenTicket({ ticketId: props.ticketId! })
  })
}

/** 发送回复 */
async function handleReply(): Promise<void> {
  if (!props.ticketId || !replyContent.value.trim()) return
  await runAction(async () => {
    if (props.portalMode) {
      await replyMyTicket(props.ticketId!, {
        content: replyContent.value.trim(),
      })
    } else {
      await replyTicket({
        ticketId: props.ticketId!,
        content: replyContent.value.trim(),
        isInternal: isInternal.value ? 1 : 0,
      })
    }
    replyContent.value = ''
    isInternal.value = false
  })
}

/** 关闭抽屉 */
function handleClose(): void {
  replyContent.value = ''
  isInternal.value = false
}

watch(
  () => [visible.value, props.ticketId] as const,
  ([open, id]) => {
    if (open && id) {
      loadDetail()
    }
  },
)
</script>
