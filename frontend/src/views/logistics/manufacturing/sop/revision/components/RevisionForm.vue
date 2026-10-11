<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/sop/revision/components -->
<!-- 文件名称：RevisionForm.vue -->
<!-- 功能描述：SOP 版本实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form revision-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="revision-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cultureCode')"
                name="cultureCode"
              >
                <TaktSelect
                  v-model:value="formState.cultureCode"
                  dict-type="sys_culture_code"
                  :placeholder="pi.ph('cultureCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sopId')"
                name="sopId"
              >
                <TaktSelect
                  v-model:value="formState.sopId"
                  api-url="TaktSopDocs/options"
                  :placeholder="pi.ph('sopId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('revision')"
                name="revision"
              >
                <a-input
                  v-model:value="formState.revision"
                  :placeholder="pi.ph('revision')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fileUrl')"
                name="fileUrl"
              >
                <a-input
                  v-model:value="formState.fileUrl"
                  :placeholder="pi.ph('fileUrl')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('changeDesc')"
                name="changeDesc"
              >
                <a-input
                  v-model:value="formState.changeDesc"
                  :placeholder="pi.ph('changeDesc')"
                  show-count
                  :maxlength="1000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecnId')"
                name="ecnId"
              >
                <TaktSelect
                  v-model:value="formState.ecnId"
                  api-url="TaktEcs/options"
                  :placeholder="pi.ph('ecnId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isLocked')"
                name="isLocked"
              >
                <TaktSelect
                  v-model:value="formState.isLocked"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isLocked')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('forceLeaderAck')"
                name="forceLeaderAck"
              >
                <TaktSelect
                  v-model:value="formState.forceLeaderAck"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('forceLeaderAck')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('revisionStatus')"
                name="revisionStatus"
              >
                <TaktSelect
                  v-model:value="formState.revisionStatus"
                  dict-type="sys_lifecycle_status"
                  :placeholder="pi.ph('revisionStatus')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('effectiveRule')"
                name="effectiveRule"
              >
                <TaktSelect
                  v-model:value="formState.effectiveRule"
                  dict-type="logistics_sop_effective_rule"
                  :placeholder="pi.ph('effectiveRule')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="pi.ph('tenantCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
              >
                <TaktSelect
                  v-model:value="formState.companyCode"
                  api-url="TaktCompanies/options"
                  :placeholder="pi.ph('companyCode')"
                  disabled
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
                    <span>{{ pi.label('extField') }}</span>
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
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
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
    <!-- 下：子表 contents -->
    <TaktEditableTable
      ref="sopContentTableRef"
      v-model="childSopContentRows"
      :columns="sopContentFormColumns"
      :title="sopContentPi.self()"
      :add-button-entity="sopContentPi.self()"
      id-field="sopContentId"
      :default-row="createDefaultSopContentRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-sopId="{ record }">
        <TaktSelect
          v-model:value="record.sopId"
          api-url="TaktSopDocs/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="sopContentPi.queryPh('sopId', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * SOP 版本实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/sop/revision/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useSopRevisionI18n } from '../composables/useRevisionI18n'

/** 实体字段 i18n */
const pi = useSopRevisionI18n()

import type { SopRevisionCreate } from '@/types/logistics/manufacturing/sop/revision.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文（当前公司 CultureCode 注入源） */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / CultureCode / PlantCode（登录或公司切换注入；工厂可选改）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (force || !target.companyCode) {
    target.companyCode = tenantStore.companyCode
  }
  if (force || !target.cultureCode) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    const nextPlant = tenantStore.currentCompanyRelatedPlant || ''
    if (nextPlant) {
      target.plantCode = nextPlant
    }
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","sopId","revision","fileUrl","changeDesc","ecnId","isLocked","forceLeaderAck","revisionStatus","effectiveRule","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { useSopContentI18n } from '../composables/useContentI18n'

const sopContentPi = useSopContentI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childSopContentRows = ref<Record<string, unknown>[]>([])
const sopContentTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 sopContent 可编辑列 */
const sopContentFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'sopId',
    title: sopContentPi.label('sopId'),
    width: 140,
  },
  {
    key: 'contentTitle',
    title: sopContentPi.label('contentTitle'),
    editor: 'textarea',
    rows: 1,
    placeholder: sopContentPi.ph('contentTitle'),
    width: 180,
  },
  {
    key: 'steps',
    title: sopContentPi.label('steps'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: sopContentPi.ph('steps'),
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SopRevisionCreate & { sopRevisionId?: string }> | null | undefined) {
  const rows_sopContent = ((val as any)?.contents ?? []) as Record<string, unknown>[]
  childSopContentRows.value = rows_sopContent
}

function createDefaultSopContentRow(): Record<string, unknown> {
  return {
    sopId: '',
    contentTitle: '',
    steps: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.sopRevisionId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    contents: sopContentTableRef.value?.getRows?.() ?? childSopContentRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      revisionId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SopRevisionCreate & { sopRevisionId?: string }> | null
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
    delete (next as any).contents
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
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture, tenantStore.currentCompanyRelatedPlant] as const,
  () => {
    if (!props.formData?.sopRevisionId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  sopId: [
    {
      required: true,
      message: pi.ph('sopId'),
      trigger: 'change'
    }
  ],
  revision: [
    {
      required: true,
      message: pi.ph('revision'),
      trigger: 'blur'
    }
  ],
  isLocked: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isLocked'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isLocked'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  forceLeaderAck: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('forceLeaderAck'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('forceLeaderAck'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  revisionStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('revisionStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('revisionStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  effectiveRule: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('effectiveRule'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('effectiveRule'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await sopContentTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('isLocked' in payload) {
    const rawisLocked = payload.isLocked
    if (rawisLocked === undefined || rawisLocked === null || rawisLocked === '') {
      delete payload.isLocked
    } else {
      const numisLocked = typeof rawisLocked === 'number' ? rawisLocked : Number(rawisLocked)
      if (Number.isFinite(numisLocked)) payload.isLocked = numisLocked
      else delete payload.isLocked
    }
  }
  if ('forceLeaderAck' in payload) {
    const rawforceLeaderAck = payload.forceLeaderAck
    if (rawforceLeaderAck === undefined || rawforceLeaderAck === null || rawforceLeaderAck === '') {
      delete payload.forceLeaderAck
    } else {
      const numforceLeaderAck = typeof rawforceLeaderAck === 'number' ? rawforceLeaderAck : Number(rawforceLeaderAck)
      if (Number.isFinite(numforceLeaderAck)) payload.forceLeaderAck = numforceLeaderAck
      else delete payload.forceLeaderAck
    }
  }
  if ('revisionStatus' in payload) {
    const rawrevisionStatus = payload.revisionStatus
    if (rawrevisionStatus === undefined || rawrevisionStatus === null || rawrevisionStatus === '') {
      delete payload.revisionStatus
    } else {
      const numrevisionStatus = typeof rawrevisionStatus === 'number' ? rawrevisionStatus : Number(rawrevisionStatus)
      if (Number.isFinite(numrevisionStatus)) payload.revisionStatus = numrevisionStatus
      else delete payload.revisionStatus
    }
  }
  if ('effectiveRule' in payload) {
    const raweffectiveRule = payload.effectiveRule
    if (raweffectiveRule === undefined || raweffectiveRule === null || raweffectiveRule === '') {
      delete payload.effectiveRule
    } else {
      const numeffectiveRule = typeof raweffectiveRule === 'number' ? raweffectiveRule : Number(raweffectiveRule)
      if (Number.isFinite(numeffectiveRule)) payload.effectiveRule = numeffectiveRule
      else delete payload.effectiveRule
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }
  if (props.formData?.sopRevisionId) {
    payload.sopRevisionId = props.formData.sopRevisionId
  }
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.sopRevisionId)
  childSopContentRows.value = []
  sopContentTableRef.value?.resetRows?.()
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
