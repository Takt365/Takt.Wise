<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/message/components -->
<!-- 文件名称：MessageForm.vue -->
<!-- 功能描述：在线消息新增表单；标题自动拼接（{messageType}-{messageGroup}:{内容前40字}）；两步提交：① createMessage 落库 ② sendMessageById SignalR 推送；defineExpose validate/getValues/resetFields/submitCreateAndPushAsync -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-row :gutter="24">
      <a-col :span="12">
        <a-form-item
          :label="t('entity.message.fromuserid')"
          name="fromUserId"
        >
          <TaktSelect
            v-model:value="formState.fromUserId"
            api-url="TaktUsers/options"
            :field-names="{ label: 'dictLabel', value: 'dictValue' }"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.message.fromuserid') })"
            disabled
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.message.fromusername')"
          name="fromUserName"
        >
          <a-input
            v-model:value="formState.fromUserName"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.message.fromusername') })"
            size="small"
            disabled
          />
        </a-form-item>
      </a-col>
      <a-col
        v-if="isCreate"
        :span="24"
      >
        <a-form-item
          :label="t('entity.message.tousername')"
          name="recipientMode"
        >
          <a-radio-group
            v-model:value="formState.recipientMode"
            :disabled="loading"
          >
            <a-radio
              v-if="canUseSendToAll"
              value="all"
            >
              {{ t('foundation.message.page.recipient.all') }}
            </a-radio>
            <a-radio value="list">
              {{ t('foundation.message.page.recipient.list.label') }}
            </a-radio>
          </a-radio-group>
        </a-form-item>
      </a-col>
      <a-col
        v-if="isCreate && formState.recipientMode === 'list'"
        :span="24"
      >
        <a-form-item
          :label="t('foundation.message.page.recipient.list.select')"
          name="toUserIds"
        >
          <TaktSelect
            v-model="formState.toUserIds"
            api-url="TaktUsers/options"
            :field-names="{ label: 'dictLabel', value: 'dictValue' }"
            :placeholder="t('foundation.message.page.recipient.list.placeholder')"
            size="small"
            show-search
            multiple
            :max-tag-count="5"
            @change="handleToUserListChange"
          />
        </a-form-item>
      </a-col>
      <a-col
        v-if="!isCreate"
        :span="12"
      >
        <a-form-item
          :label="t('entity.message.touserid')"
          name="toUserId"
        >
          <TaktSelect
            v-model:value="formState.toUserId"
            api-url="TaktUsers/options"
            :field-names="{ label: 'dictLabel', value: 'dictValue' }"
            :placeholder="t('common.page.form.placeholder.select', { field: t('entity.message.touserid') })"
            allow-clear
            @change="handleToUserChange"
          />
        </a-form-item>
      </a-col>
      <a-col
        v-if="!isCreate"
        :span="12"
      >
        <a-form-item
          :label="t('entity.message.tousername')"
          name="toUserName"
        >
          <a-input
            v-model:value="formState.toUserName"
            size="small"
            disabled
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.message.type')"
          name="messageType"
        >
          <TaktSelect
            v-model:value="formState.messageType"
            dict-type="sys_message_type"
            :placeholder="t('common.page.form.placeholder.select', { field: t('entity.message.type') })"
            size="small"
          />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item
          :label="t('entity.message.group')"
          name="messageGroup"
        >
          <TaktSelect
            v-model:value="formState.messageGroup"
            dict-type="sys_message_group"
            :placeholder="t('common.page.form.placeholder.select', { field: t('entity.message.group') })"
            size="small"
          />
        </a-form-item>
      </a-col>
      <a-col
        v-if="needsFileUpload"
        :span="12"
      >
        <a-form-item
          :label="t('entity.message.filename')"
          name="fileName"
        >
          <a-input
            v-model:value="formState.fileName"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.message.filename') })"
            size="small"
            show-count
            :maxlength="200"
            disabled
          />
        </a-form-item>
      </a-col>
      <a-col
        v-if="needsFileUpload"
        :span="24"
      >
        <a-form-item
          :label="t('entity.message.accessurl')"
          name="accessUrl"
        >
          <TaktUploadFile
            tabs-type="files"
            :files-auto-upload="true"
            :files-multiple="false"
            :files-max-count="1"
            :files-disabled="!!loading || fileUploading"
            :files-max-size="taktFileMaxSizeMb"
            :files-accept="uploadAccept"
            :files-hint="uploadHintText"
            :files-custom-request="handleFilesCustomRequest"
            v-model:files-file-list="filesFileList"
            @files:remove="handleFileRemove"
          />
          <a-input
            v-if="formState.accessUrl"
            v-model:value="formState.accessUrl"
            class="mt-2"
            size="small"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.message.accessurl') })"
            show-count
            :maxlength="1000"
            disabled
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('entity.message.content')"
          name="messageContent"
        >
          <a-textarea
            v-model:value="formState.messageContent"
            :placeholder="messageContentPlaceholder"
            :rows="3"
            size="small"
          />
        </a-form-item>
      </a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 在线消息新增表单（两步：先落库，再 SignalR 推送）
 * @module views/foundation/message/components
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import type { UploadFile } from 'ant-design-vue'
import type { Rule } from 'ant-design-vue/es/form'
import type { Message, MessageCreate, MessageBatchCreate } from '@/types/foundation/message.types'
import { createAndSendMessages } from '@/api/foundation/message.api'
import { getFileById } from '@/api/foundation/file.api'
import { useUserStore } from '@/stores/identity/useUserStore'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import { uploadTaktFileSmart } from '@/utils/taktFileChunkUpload'

/** 两步提交结果：批量落库 + SignalR 推送 */
export interface MessageFormCreateResult {
  /** 已落库消息（首条） */
  created?: Message
  /** 批量落库结果 */
  createdList: Message[]
  /** 是否已完成推送（batch-send 成功即 true） */
  pushed: boolean
  /** 推送失败 */
  pushFailed?: boolean
}

/** 接收者模式：全员广播 / 指定用户列表 */
type MessageRecipientMode = 'all' | 'list'

/** 解析后的接收者（用户 ID + 登录名） */
interface MessageRecipientItem {
  id: string
  userName: string
}

/** 列表模式最多可选接收者数 */
const MAX_RECIPIENT_LIST_SIZE = 5

/** 自动标题：消息内容截取最大字符数 */
const MESSAGE_TITLE_CONTENT_MAX = 40

/** 自动标题：落库字段上限（与 TaktMessage.MessageTitle 对齐） */
const MESSAGE_TITLE_MAX_LENGTH = 200

/** 超级管理员 UserType（与 TaktUserType.SuperAdmin 对齐） */
const SUPER_ADMIN_USER_TYPE = 2

/** i18n 翻译函数 */
const { t } = useI18n()
/** Pinia：用户上下文 */
const userStore = useUserStore()
/** Pinia：按钮权限（第二步 SignalR 发送） */
const permissionStore = usePermissionStore()
/** CreateDto 字段名列表（与 formState 键对齐；messageTitle 由提交时自动生成） */
const formFields = ['fromUserId', 'fromUserName', 'toUserName', 'recipientMode', 'toUserIds', 'toUserId', 'isCc', 'messageContent', 'messageType', 'messageGroup', 'fileName', 'accessUrl']
/** TaktYesNo：是 */
const DEFAULT_IS_CC = 1

/**
 * 新增态：发送者默认为当前登录用户（只读展示）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖
 */
function applySenderDefaults(target: Record<string, unknown>, force = false) {
  const currentUserName = userStore.userName || userStore.userInfo?.userName || ''
  const currentUserId = userStore.userId || (userStore.userInfo?.userId != null ? String(userStore.userInfo.userId) : '')
  if (formFields.includes('fromUserName') && (force || !target.fromUserName)) {
    target.fromUserName = currentUserName
  }
  if (formFields.includes('fromUserId') && (force || !target.fromUserId)) {
    target.fromUserId = currentUserId
  }
}

/**
 * 新增态业务默认值
 * @param target 表单数据
 */
function applyCreateDefaults(target: Record<string, unknown>) {
  applySenderDefaults(target, true)
  const canSendToAll = userStore.userType === SUPER_ADMIN_USER_TYPE
  if (target.recipientMode === 'all' && !canSendToAll) {
    target.recipientMode = 'list'
  }
  if (target.recipientMode !== 'all' && target.recipientMode !== 'list') {
    target.recipientMode = 'list'
  }
  if (!Array.isArray(target.toUserIds)) {
    target.toUserIds = []
  }
  if (target.messageType === undefined || target.messageType === null || target.messageType === '') {
    target.messageType = 'system'
  }
  if (target.messageGroup === undefined || target.messageGroup === null || target.messageGroup === '') {
    target.messageGroup = 'message'
  }
  target.isCc = DEFAULT_IS_CC
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MessageCreate & { messageId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false,
})

/** 是否新增态（无 messageId） */
const isCreate = computed(() => !props.formData?.messageId)

/** 是否可使用「当前公司全部用户」接收模式（仅超级管理员） */
const canUseSendToAll = computed(() => userStore.userType === SUPER_ADMIN_USER_TYPE)

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 附件上传中 */
const fileUploading = ref(false)
/** takt-upload-file 文件列表 */
const filesFileList = ref<UploadFile[]>([])
/** 上传体积上限 MB */
const taktFileMaxSizeMb = ref(100)

/** 当前消息类型 dictValue */
/** 当前消息类型（sys_message_type DictValue，来自 TaktSelect） */
const messageTypeDictValue = computed(() => String(formState.messageType ?? ''))

/** 多媒体类型须上传附件 */
const needsFileUpload = computed(() => messageTypeDictValue.value === 'multimedia')

/** 上传 accept（多媒体：图片/音视频） */
const uploadAccept = computed(() => {
  if (messageTypeDictValue.value === 'multimedia') {
    return 'image/*,video/*,audio/*'
  }
  return ''
})

/** 上传区提示文案 */
const uploadHintText = computed(() => t('foundation.message.page.upload.multimedia.hint'))

/** 消息内容占位：附件类型可为选填说明 */
const messageContentPlaceholder = computed(() => {
  if (needsFileUpload.value) {
    return t('foundation.message.page.upload.content.optional')
  }
  return t('common.page.form.placeholder.required', { field: t('entity.message.content') })
})

/** 已解析的列表模式接收者（与 toUserIds 同步） */
const recipientUsers = ref<MessageRecipientItem[]>([])

/** 编辑态灌入 formData；新增态 reset 并注入默认值 */
watch(
  () => props.formData,
  (val) => {
    const next = val ? { ...val } : {}
    Object.keys(formState).forEach((k) => delete formState[k])
    const isCreateMode = !props.formData?.messageId
    if (isCreateMode) {
      applyCreateDefaults(next)
    }
    Object.assign(formState, next)
    recipientUsers.value = []
    syncFilesFileListFromFormState()
  },
  { immediate: true, deep: true }
)

/** 切换为文本/系统消息时清空附件 */
watch(messageTypeDictValue, (next, prev) => {
  const nextNeeds = next === 'multimedia'
  const prevNeeds = prev != null && prev === 'multimedia'
  if (prevNeeds && !nextNeeds) {
    clearUploadedFile()
  }
  if (nextNeeds && prev != null && next !== prev) {
    clearUploadedFile()
  }
})

/** 用户资料就绪后，新增态同步发送者 */
watch(
  () => [userStore.userId, userStore.userName, userStore.userInfo?.userId, userStore.userInfo?.userName] as const,
  () => {
    if (isCreate.value) {
      applySenderDefaults(formState, true)
    }
  },
)

/** 非超级管理员不可使用全员模式，自动切回指定用户 */
watch(
  () => userStore.userType,
  () => {
    if (!isCreate.value) {
      return
    }
    if (!canUseSendToAll.value && formState.recipientMode === 'all') {
      formState.recipientMode = 'list'
    }
  },
)

/**
 * 从下拉选项解析登录用户名
 * @param option 选中项
 * @returns {string} 登录用户名
 */
function resolveUserNameFromSelectOption(
  option: { label?: string; dictLabel?: string; extValue?: string | number } | null | undefined,
): string {
  const extUserName = option?.extValue != null ? String(option.extValue).trim() : ''
  if (extUserName) {
    return extUserName
  }
  const displayLabel = (option?.dictLabel ?? option?.label ?? '').trim()
  const parenIndex = displayLabel.indexOf('(')
  return parenIndex > 0 ? displayLabel.slice(0, parenIndex).trim() : displayLabel
}

/**
 * 单条接收者变更：回填 toUserName
 * @param value 用户 Id
 * @param option 选项
 */
function handleToUserChange(
  value: string | number | (string | number)[] | undefined,
  option: { label?: string; dictLabel?: string; extValue?: string | number; dictValue?: string | number; value?: string | number } | { label?: string; dictLabel?: string; extValue?: string | number; dictValue?: string | number; value?: string | number }[] | null,
) {
  if (value === undefined || value === null || value === '') {
    formState.toUserName = ''
    return
  }
  const opt = Array.isArray(option) ? option[0] : option
  formState.toUserName = resolveUserNameFromSelectOption(opt)
}

/**
 * 列表模式接收者多选变更：同步 toUserIds 与 recipientUsers
 * @param value 选中用户 ID 列表
 * @param option 选中项列表
 */
function handleToUserListChange(
  value: string | number | (string | number)[] | undefined,
  option: { label?: string; dictLabel?: string; extValue?: string | number; dictValue?: string | number; value?: string | number } | { label?: string; dictLabel?: string; extValue?: string | number; dictValue?: string | number; value?: string | number }[] | null,
) {
  let ids = Array.isArray(value)
    ? value.map((item) => String(item).trim()).filter(Boolean)
    : value != null && String(value).trim() !== ''
      ? [String(value).trim()]
      : []
  if (ids.length > MAX_RECIPIENT_LIST_SIZE) {
    ids = ids.slice(0, MAX_RECIPIENT_LIST_SIZE)
    message.warning(t('foundation.message.page.recipient.list.max', { max: MAX_RECIPIENT_LIST_SIZE }))
  }
  formState.toUserIds = ids
  const options = Array.isArray(option) ? option : option != null ? [option] : []
  recipientUsers.value = ids.map((id) => {
    const matched = options.find((item) => {
      const optionValue = item?.dictValue ?? item?.value
      return optionValue != null && String(optionValue).trim() === id
    })
    return {
      id,
      userName: resolveUserNameFromSelectOption(matched),
    }
  })
}

/** 切换接收者模式时清空列表选择 */
watch(
  () => formState.recipientMode as MessageRecipientMode | undefined,
  (mode) => {
    if (!isCreate.value) {
      return
    }
    if (mode === 'all') {
      formState.toUserIds = []
      recipientUsers.value = []
    }
  },
)

/**
 * 获取列表模式下的接收者清单（校验后调用）
 * @returns {MessageRecipientItem[]} 接收者列表
 */
function resolveRecipientList(): MessageRecipientItem[] {
  const ids = Array.isArray(formState.toUserIds)
    ? formState.toUserIds.map((item: unknown) => String(item).trim()).filter(Boolean)
    : []
  return ids.map((id) => {
    const cached = recipientUsers.value.find((item) => item.id === id)
    return cached ?? { id, userName: '' }
  })
}

/**
 * 按 fileName / accessUrl 同步上传列表展示
 */
function syncFilesFileListFromFormState() {
  const url = String(formState.accessUrl ?? '').trim()
  if (!url) {
    filesFileList.value = []
    return
  }
  filesFileList.value = [{
    uid: '-1',
    name: String(formState.fileName ?? url.split('/').pop() ?? 'file'),
    status: 'done',
    url,
  }]
}

/**
 * 将 TaktFile 上传结果回填至表单（文件名由上传结果回填，禁止手输）
 * @param file 本地文件
 * @param result 上传结果
 */
async function applyUploadResultToForm(file: globalThis.File, result: Awaited<ReturnType<typeof uploadTaktFileSmart>>) {
  let accessUrl = result.accessUrl?.trim() ?? ''
  if (!accessUrl && result.fileId) {
    const detail = await getFileById(result.fileId)
    accessUrl = detail.accessUrl?.trim() ?? ''
  }
  if (!accessUrl) {
    throw new Error('accessUrl empty')
  }
  formState.accessUrl = accessUrl
  formState.fileName = result.fileOriginalName?.trim()
    || result.fileName?.trim()
    || file.name
  formRef.value?.validateFields(['accessUrl', 'fileName']).catch(() => undefined)
}

/**
 * takt-upload-file 自定义上传：落库 TaktFile 后回写 accessUrl / fileName
 * @param options 上传选项
 */
async function handleFilesCustomRequest(options: { file: globalThis.File | Blob; onSuccess?: (body: unknown) => void; onError?: (e: Error) => void }) {
  const file = options.file as globalThis.File
  if (props.loading || fileUploading.value) {
    options.onError?.(new Error('busy'))
    return
  }
  fileUploading.value = true
  try {
    const result = await uploadTaktFileSmart(file)
    await applyUploadResultToForm(file, result)
    syncFilesFileListFromFormState()
    options.onSuccess?.(result)
    message.success(t('foundation.message.page.upload.success'))
  } catch (e) {
    options.onError?.(e instanceof Error ? e : new Error(String(e)))
    message.error(t('foundation.message.page.upload.failed'))
  } finally {
    fileUploading.value = false
  }
}

/**
 * 清空已上传附件
 */
function clearUploadedFile() {
  formState.accessUrl = ''
  formState.fileName = ''
  filesFileList.value = []
}

/**
 * 移除已上传附件
 */
function handleFileRemove() {
  clearUploadedFile()
}

watch(
  () => [formState.fileName, formState.accessUrl],
  () => {
    syncFilesFileListFromFormState()
  }
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  fromUserName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.message.fromusername') }),
      trigger: 'blur'
    }
  ],
  toUserIds: [
    {
      validator: async () => {
        if (!isCreate.value || formState.recipientMode !== 'list') {
          return Promise.resolve()
        }
        const ids = Array.isArray(formState.toUserIds) ? formState.toUserIds : []
        if (ids.length === 0) {
          return Promise.reject(t('foundation.message.page.recipient.list.required'))
        }
        if (ids.length > MAX_RECIPIENT_LIST_SIZE) {
          return Promise.reject(t('foundation.message.page.recipient.list.max', { max: MAX_RECIPIENT_LIST_SIZE }))
        }
        return Promise.resolve()
      },
      trigger: 'change',
    }],
  messageContent: needsFileUpload.value
    ? []
    : [
        {
          required: true,
          message: t('common.page.form.placeholder.required', { field: t('entity.message.content') }),
          trigger: 'blur',
        }],
  accessUrl: needsFileUpload.value
    ? [
        {
          validator: async () => {
            if (String(formState.accessUrl ?? '').trim()) {
              return Promise.resolve()
            }
            return Promise.reject(t('foundation.message.page.upload.required'))
          },
          trigger: 'change',
        }]
    : [],
  messageType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.message.type') }),
      trigger: 'change'
    }
  ],
  messageGroup: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.message.group') }),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射消息正文为批量创建 DTO（不含接收者） */
function getMessageBody(): Omit<MessageBatchCreate, 'sendToAll' | 'toUserIds'> {
  const fromUserId = formState.fromUserId != null && String(formState.fromUserId).trim() !== ''
    ? String(formState.fromUserId).trim()
    : undefined
  const fileName = formState.fileName != null && String(formState.fileName).trim() !== ''
    ? String(formState.fileName).trim()
    : undefined
  const accessUrl = formState.accessUrl != null && String(formState.accessUrl).trim() !== ''
    ? String(formState.accessUrl).trim()
    : undefined
  return {
    fromUserName: formState.fromUserName,
    fromUserId,
    messageTitle: buildAutoMessageTitle(),
    messageContent: formState.messageContent ?? '',
    fileName,
    accessUrl,
    messageType: String(formState.messageType ?? 'system'),
    messageGroup: String(formState.messageGroup ?? 'message'),
    isCc: DEFAULT_IS_CC,
  }
}

/**
 * 自动生成消息标题：{messageType DictValue}-{messageGroup DictValue}:{消息内容前 40 字符}
 * @returns {string} 落库标题（最长 200 字符）
 */
function buildAutoMessageTitle(): string {
  const messageType = String(formState.messageType ?? 'system').trim() || 'system'
  const messageGroup = String(formState.messageGroup ?? 'message').trim() || 'message'
  const contentPrefix = String(formState.messageContent ?? '').trim().slice(0, MESSAGE_TITLE_CONTENT_MAX)
  const title = `${messageType}-${messageGroup}:${contentPrefix}`
  if (title.length <= MESSAGE_TITLE_MAX_LENGTH) {
    return title
  }
  return title.slice(0, MESSAGE_TITLE_MAX_LENGTH)
}

/** 映射为 Create DTO（字典值转后端枚举；不含表单只读隔离字段） */
function getValues(recipient?: MessageRecipientItem): Record<string, any> {
  const body = getMessageBody()
  const toUserId = recipient?.id
    ?? (formState.toUserId != null && String(formState.toUserId).trim() !== ''
      ? String(formState.toUserId).trim()
      : undefined)
  const toUserName = recipient?.userName
    ?? (formState.toUserName != null ? String(formState.toUserName).trim() : '')
  return {
    ...body,
    toUserName,
    toUserId,
  }
}

/** 重置表单 */
function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])
  recipientUsers.value = []
  clearUploadedFile()
}

/**
 * 批量落库并推送：全员或指定用户列表（服务端逐人 Create + Send）
 * @returns {Promise<MessageFormCreateResult>} 落库结果与推送状态
 */
async function submitCreateAndPushAsync(): Promise<MessageFormCreateResult> {
  await validate()
  const mode = formState.recipientMode as MessageRecipientMode
  if (mode === 'all' && !canUseSendToAll.value) {
    throw new Error(t('foundation.message.page.recipient.send.to.all.forbidden'))
  }
  const body = getMessageBody()
  const payload: MessageBatchCreate = {
    ...body,
    sendToAll: mode === 'all',
    toUserIds: mode === 'list'
      ? resolveRecipientList().map((item) => item.id)
      : [],
  }
  if (!payload.sendToAll && (!payload.toUserIds || payload.toUserIds.length === 0)) {
    throw new Error(t('foundation.message.page.recipient.list.required'))
  }
  if (!permissionStore.hasPermission('foundation:message:send')) {
    return { createdList: [], pushed: false, pushFailed: true }
  }
  const createdList = await createAndSendMessages(payload)
  return {
    created: createdList[0],
    createdList,
    pushed: true,
  }
}

defineExpose({ validate, getValues, resetFields, submitCreateAndPushAsync })
</script>