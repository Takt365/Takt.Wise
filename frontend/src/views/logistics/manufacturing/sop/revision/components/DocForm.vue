<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/sop/revision/components -->
<!-- 文件名称：DocForm.vue -->
<!-- 功能描述：SOP 文档头实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form doc-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="doc-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
              <a-col :span="12">
                <a-form-item
                  :label="t('common.page.entity.culturecode')"
                  name="cultureCode"
                >
                  <a-input
                    v-model:value="formState.cultureCode"
                    disabled
                    :placeholder="t('common.page.form.placeholder.input')"
                  />
                </a-form-item>
              </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopdoc.sopcode')"
                name="sopCode"
              >
                <a-input
                  v-model:value="formState.sopCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopdoc.sopcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.sopDocId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopdoc.sopname')"
                name="sopName"
              >
                <a-input
                  v-model:value="formState.sopName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopdoc.sopname') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopdoc.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopdoc.materialcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.sopDocId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopdoc.routingitemid')"
                name="routingItemId"
              >
                <a-input
                  v-model:value="formState.routingItemId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopdoc.routingitemid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopdoc.workstationid')"
                name="workstationId"
              >
                <a-input
                  v-model:value="formState.workstationId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopdoc.workstationid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopdoc.currentrevisionid')"
                name="currentRevisionId"
              >
                <a-input
                  v-model:value="formState.currentRevisionId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopdoc.currentrevisionid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.sopdoc.sopstatus')"
                name="sopStatus"
              >
                <TaktSelect
                  v-model:value="formState.sopStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopdoc.sopstatus') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                name="extField"
                class="takt-form-item-ext-field"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t('common.page.entity.extfieldhint')"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ t('common.page.entity.extfield') }}</span>
                  </span>
                </template>
                <a-textarea
                  v-model:value="formState.extField"
                  :placeholder="t('common.page.form.placeholder.extfield')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('common.page.entity.remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
    <!-- 下：子表 revisions -->
    <TaktEditableTable
      ref="sopRevisionTableRef"
      v-model="childSopRevisionRows"
      :columns="sopRevisionFormColumns"
      :title="t('entity.soprevision._self')"
      :add-button-entity="t('entity.soprevision._self')"
      id-field="sopRevisionId"
      :default-row="createDefaultSopRevisionRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * SOP 文档头实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/sop/revision/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SopDocCreate } from '@/types/logistics/manufacturing/sop/doc.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或公司切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (formFields.includes('tenantCode') && (force || !target.tenantCode)) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (formFields.includes('companyCode') && (force || !target.companyCode)) {
    target.companyCode = tenantStore.companyCode
  }
  if (formFields.includes('cultureCode') && (force || !target.cultureCode)) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","sopCode","sopName","materialCode","routingItemId","workstationId","currentRevisionId","sopStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'

const childSopRevisionRows = ref<Record<string, unknown>[]>([])
const sopRevisionTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 sopRevision 可编辑列 */
const sopRevisionFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'sopId',
    title: t('entity.soprevision.sopid'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'revision',
    title: t('entity.soprevision.revision'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'fileUrl',
    title: t('entity.soprevision.fileurl'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.soprevision.fileurl') }),
  },
  {
    key: 'changeDesc',
    title: t('entity.soprevision.changedesc'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.soprevision.changedesc') }),
  },
  {
    key: 'ecnId',
    title: t('entity.soprevision.ecnid'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.soprevision.ecnid') }),
  },
  {
    key: 'isLocked',
    title: t('entity.soprevision.islocked'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'forceLeaderAck',
    title: t('entity.soprevision.forceleaderack'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'revisionStatus',
    title: t('entity.soprevision.revisionstatus'),
    editor: 'inputNumber',
    width: 140,
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SopDocCreate & { sopDocId?: string }> | null | undefined) {
  childSopRevisionRows.value = ((val as any)?.revisions ?? []) as Record<string, unknown>[]
}

function createDefaultSopRevisionRow(): Record<string, unknown> {
  return {
    sopId: '',
    revision: '',
    fileUrl: '',
    changeDesc: '',
    ecnId: '',
    isLocked: 0,
    forceLeaderAck: 0,
    revisionStatus: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.sopDocId ?? ''
  return {
    ...formState,
    revisions: sopRevisionTableRef.value?.getRows?.() ?? childSopRevisionRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      sopDocId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SopDocCreate & { sopDocId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 sopDocId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.sopDocId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).revisions
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.sopDocId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  sopCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopdoc.sopcode') }),
      trigger: 'blur'
    }
  ],
  sopName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopdoc.sopname') }),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopdoc.materialcode') }),
      trigger: 'blur'
    }
  ],
  routingItemId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopdoc.routingitemid') }),
      trigger: 'blur'
    }
  ],
  cultureCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopdoc.culturecode') }),
      trigger: 'blur'
    }
  ],
  sopStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sopdoc.sopstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sopdoc.sopstatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await sopRevisionTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('sopStatus' in payload) {
    const rawsopStatus = payload.sopStatus
    payload.sopStatus = typeof rawsopStatus === 'number' ? rawsopStatus : Number(rawsopStatus)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.sopDocId)
  childSopRevisionRows.value = []
  sopRevisionTableRef.value?.resetRows?.()
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
