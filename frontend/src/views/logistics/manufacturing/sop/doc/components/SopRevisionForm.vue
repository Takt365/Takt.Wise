<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/sop/doc/components -->
<!-- 文件名称：SopRevisionForm.vue -->
<!-- 功能描述：SOP 文档头实体子表 sopRevision 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form sop-revision-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="sop-revision-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo')"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.soprevision.sopid')"
                name="sopId"
              >
                <a-input
                  v-model:value="formState.sopId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.soprevision.sopid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.soprevision.revision')"
                name="revision"
              >
                <a-input
                  v-model:value="formState.revision"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.soprevision.revision') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.soprevision.fileurl')"
                name="fileUrl"
              >
                <a-input
                  v-model:value="formState.fileUrl"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.soprevision.fileurl') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.soprevision.changedesc')"
                name="changeDesc"
              >
                <a-input
                  v-model:value="formState.changeDesc"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.soprevision.changedesc') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.soprevision.ecnid')"
                name="ecnId"
              >
                <a-input
                  v-model:value="formState.ecnId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.soprevision.ecnid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.soprevision.islocked')"
                name="isLocked"
              >
                <TaktSelect
                  v-model:value="formState.isLocked"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.soprevision.islocked') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.soprevision.forceleaderack')"
                name="forceLeaderAck"
              >
                <TaktSelect
                  v-model:value="formState.forceLeaderAck"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.soprevision.forceleaderack') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.soprevision.revisionstatus')"
                name="revisionStatus"
              >
                <TaktSelect
                  v-model:value="formState.revisionStatus"
                  dict-type="sys_lifecycle_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.soprevision.revisionstatus') })"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * SOP 文档头实体子表 sopRevision 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/sop/doc/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SopRevisionCreate } from '@/types/logistics/manufacturing/sop/sopRevision.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["sopId","revision","fileUrl","changeDesc","ecnId","isLocked","forceLeaderAck","revisionStatus"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SopRevisionCreate & { sopRevisionId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  masterId: '',
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  revisionStatus: 1
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 sopRevisionId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.sopRevisionId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  sopId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.soprevision.sopid') }),
      trigger: 'blur'
    }
  ],
  revision: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.soprevision.revision') }),
      trigger: 'blur'
    }
  ],
  isLocked: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.soprevision.islocked') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.soprevision.islocked') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  forceLeaderAck: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.soprevision.forceleaderack') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.soprevision.forceleaderack') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  revisionStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.soprevision.revisionstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.soprevision.revisionstatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 sopDocId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('isLocked' in payload) {
    const rawisLocked = payload.isLocked
    payload.isLocked = typeof rawisLocked === 'number' ? rawisLocked : Number(rawisLocked)
  }
  if ('forceLeaderAck' in payload) {
    const rawforceLeaderAck = payload.forceLeaderAck
    payload.forceLeaderAck = typeof rawforceLeaderAck === 'number' ? rawforceLeaderAck : Number(rawforceLeaderAck)
  }
  if ('revisionStatus' in payload) {
    const rawrevisionStatus = payload.revisionStatus
    payload.revisionStatus = typeof rawrevisionStatus === 'number' ? rawrevisionStatus : Number(rawrevisionStatus)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.sopDocId = props.masterId
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>
