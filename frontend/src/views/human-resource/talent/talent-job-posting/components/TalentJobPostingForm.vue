<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/talent/talent-job-posting/components -->
<!-- 文件名称：TalentJobPostingForm.vue -->
<!-- 功能描述：职位发布维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
    <a-tabs
      v-model:active-key="activeTab"
      class="talent-job-posting-form-tabs"
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
                :label="t('entity.talentJobPosting.staffingrequirementid')"
                name="staffingRequirementId"
              >
                <a-input
                  v-model:value="formState.staffingRequirementId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentJobPosting.staffingrequirementid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentJobPosting.postingcode')"
                name="postingCode"
              >
                <a-input
                  v-model:value="formState.postingCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentJobPosting.postingcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentJobPosting.title')"
                name="title"
              >
                <a-input
                  v-model:value="formState.title"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentJobPosting.title') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentJobPosting.postingstatus')"
                name="postingStatus"
              >
                <a-input-number
                  v-model:value="formState.postingStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentJobPosting.postingstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentJobPosting.publishdate')"
                name="publishDate"
              >
                <a-date-picker
                  v-model:value="formState.publishDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentJobPosting.publishdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentJobPosting.opendate')"
                name="openDate"
              >
                <a-date-picker
                  v-model:value="formState.openDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentJobPosting.opendate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentJobPosting.closedate')"
                name="closeDate"
              >
                <a-date-picker
                  v-model:value="formState.closeDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentJobPosting.closedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
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
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentJobPosting.publishchannel')"
                name="publishChannel"
              >
                <a-input-number
                  v-model:value="formState.publishChannel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentJobPosting.publishchannel') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.talentJobPosting.reason')"
                name="reason"
              >
                <a-input
                  v-model:value="formState.reason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentJobPosting.reason') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.extfield')"
                name="ExtField"
              >
                <a-input
                  v-model:value="formState.ExtField"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfield') })"
                  size="small"
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
                  :rows="2"
                  size="small"
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
 * 职位发布维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/human-resource/talent/talent-job-posting/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { TalentJobPostingCreate } from '@/types/human-resource/talent/talentJobPosting.types'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

const { t } = useI18n()

const tenantStore = useTenantStore()
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
  if (force || !target.plantCode) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }

}
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
const activeTab = ref('tab-0')
const formFields = ["tenantCode","companyCode","cultureCode","staffingRequirementId","postingCode","title","postingStatus","publishDate","openDate","closeDate","publishChannel","reason","ExtField","remark"]

interface Props {
  formData?: Partial<TalentJobPostingCreate & { talentJobPostingId?: string }> | null
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false
})

const formRef = ref()
const formState = reactive<Record<string, any>>({})

watch(
  () => props.formData,
  (val) => {
    const next = val ? { ...val } : {}
    Object.keys(formState).forEach((k) => delete formState[k])

    applyScopeDefaults(next)
    Object.assign(formState, next)
  },
  { immediate: true, deep: true }
)

watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.talentJobPostingId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  staffingRequirementId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.talentJobPosting.staffingrequirementid') }),
      trigger: 'blur'
    }
  ],
  postingCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.talentJobPosting.postingcode') }),
      trigger: 'blur'
    }
  ],
  title: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.talentJobPosting.title') }),
      trigger: 'blur'
    }
  ],
  postingStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.talentJobPosting.postingstatus') }),
      trigger: 'change'
    }
  ],
  publishDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.talentJobPosting.publishdate') }),
      trigger: 'change'
    }
  ],
  openDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.talentJobPosting.opendate') }),
      trigger: 'change'
    }
  ],
  publishChannel: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.talentJobPosting.publishchannel') }),
      trigger: 'change'
    }
  ],
}))

async function validate() {
  await formRef.value?.validate()
  return formState
}

function getValues(): Record<string, any> {
  return { ...formState }
}

function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])

  activeTab.value = 'tab-0'
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
