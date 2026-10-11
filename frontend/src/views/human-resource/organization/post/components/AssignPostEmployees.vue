<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：@/views/human-resource/organization/post/components -->
<!-- 文件名称：AssignPostEmployees.vue -->
<!-- 功能描述：分配岗位员工弹窗；Transfer + getEmployeeOptions / getPostEmployeeIds / assignPostEmployees。 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-modal
    v-model:open="visible"
    :title="t('common.dialog.title.allocate', { entity: t('entity.employee._self') })"
    :width="'33.333vw'"
    :confirm-loading="loading"
    :centered="true"
    @ok="handleSubmit"
    @cancel="handleCancel"
  >
    <a-form
      :label-col="{ span: 4 }"
      :wrapper-col="{ span: 20 }"
      layout="horizontal"
    >
      <a-form-item :label="t('entity.post._self')">
        <a-input
          :value="postInfo"
          disabled
        />
      </a-form-item>
      <a-row :gutter="24">
        <a-col :span="24">
          <a-form-item
            :label="t('entity.employee._self')"
            :label-col="{ span: 24 }"
            :wrapper-col="{ span: 24 }"
          >
            <TaktTransfer
              v-model:target-keys="targetKeys"
              :data-source="dataSource"
              :titles="[t('common.tip.transfer.unassigned'), t('common.tip.transfer.assigned')]"
              :loading="optionsLoading"
            />
          </a-form-item>
        </a-col>
      </a-row>
    </a-form>
  </a-modal>
</template>

<script setup lang="ts">
/**
 * 分配岗位员工弹窗：员工 Transfer，提交 assignPostEmployees（employeeId 列表）。
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import { getEmployeeOptions } from '@/api/human-resource/personnel/employee.api'
import { getPostEmployeeIds, assignPostEmployees } from '@/api/identity/rbac.api'
import type { Post } from '@/types/human-resource/organization/post.types'
import type { EmployeePost } from '@/types/human-resource/organization/employeePost.types'
import type { TaktSelectOption } from '@/types/common.types'

/**
 * 从异常对象提取可展示消息
 * @param error 捕获的异常
 * @returns {string | undefined} 错误文案
 */
function getErrorMessage(error: unknown): string | undefined {
  if (error instanceof Error) return error.message
  if (typeof error === 'object' && error !== null && 'message' in error) {
    const msg = (error as { message?: unknown }).message
    return typeof msg === 'string' ? msg : undefined
  }
  return undefined
}

/** 组件入参 */
interface Props {
  /** 是否显示对话框 */
  open?: boolean
  /** 目标岗位 */
  post?: Post | null
}

const props = withDefaults(defineProps<Props>(), {
  open: false,
  post: null
})

const emit = defineEmits<{
  'update:open': [value: boolean]
  'success': []
}>()

const { t } = useI18n()
const logger = createLogger('AssignPostEmployees')

/** 弹窗显隐 */
const visible = ref(false)
/** 提交 loading */
const loading = ref(false)
/** 选项 loading */
const optionsLoading = ref(false)
/** 已选 employeeId */
const targetKeys = ref<string[]>([])
/** 全量员工选项 */
const allOptions = ref<TaktSelectOption[]>([])
/** 岗位 id */
const postId = ref('')
/** 岗位只读展示 */
const postInfo = ref('')

/** Transfer 数据源 */
const dataSource = computed(() =>
  allOptions.value.map((item) => ({
    key: String(item.dictValue),
    title: item.dictLabel ?? '',
    description: String(item.dictValue ?? ''),
  }))
)

watch(() => props.open, (val) => {
  visible.value = val
  if (val && props.post) {
    loadPostEmployees()
  }
})

watch(visible, (val) => {
  emit('update:open', val)
})

/**
 * 加载员工选项与岗位已绑 employeeId
 * @returns {Promise<void>}
 */
async function loadPostEmployees() {
  const post = props.post
  if (!post?.postId) return
  try {
    loading.value = true
    optionsLoading.value = true
    postId.value = String(post.postId)
    postInfo.value = `${post.postName ?? ''}${post.postCode ? `（${post.postCode}）` : ''}`
    const [allEmployees, employeePosts] = await Promise.all([
      getEmployeeOptions(),
      getPostEmployeeIds(postId.value)
    ])
    allOptions.value = allEmployees
    targetKeys.value = employeePosts
      .map((row: EmployeePost) => String(row.employeeId || ''))
      .filter((id: string) => id)
  } catch (error: unknown) {
    logger.error('[AssignPostEmployees] 加载失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.load.failed', { target: t('entity.post._self') + t('entity.employee._self') }))
  } finally {
    loading.value = false
    optionsLoading.value = false
  }
}

/**
 * 提交 assignPostEmployees
 * @returns {Promise<void>}
 */
async function handleSubmit() {
  if (!postId.value) {
    message.error(t('common.validation.not.found', { field: t('entity.post._self') }))
    return
  }
  try {
    loading.value = true
    await assignPostEmployees(postId.value, targetKeys.value)
    message.success(t('common.feedback.assign.success', { target: t('entity.employee._self') }))
    emit('success')
    handleCancel()
  } catch (error: unknown) {
    logger.error('[AssignPostEmployees] 分配失败', undefined, error)
    message.error(getErrorMessage(error) || t('common.feedback.assign.failed', { target: t('entity.employee._self') }))
  } finally {
    loading.value = false
  }
}

/** 关闭并重置 */
function handleCancel() {
  visible.value = false
  postId.value = ''
  targetKeys.value = []
  allOptions.value = []
  postInfo.value = ''
}
</script>
