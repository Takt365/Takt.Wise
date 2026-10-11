<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/routine/news-center/news/components -->
<!-- 文件名称：NewsForm.vue -->
<!-- 功能描述：新闻中心主实体 支持分类、置顶、推荐、社交统计维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="news-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
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
                :label="t('common.page.form.numberingRule')"
                name="numberingRuleCode"
              >
                <TaktSelect
                  v-model:value="formState.numberingRuleCode"
                  api-url="TaktNumberings/options"
                  :api-params="{ documentType: '新闻中心' }"
                  :placeholder="t('common.page.form.placeholder.selectonly')"
                  :disabled="!!formData?.newsId || loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('newsCode')"
                name="newsCode"
              >
                <a-input
                  v-model:value="formState.newsCode"
                  :placeholder="t('common.page.form.numberingCodePreview')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('newsCategory')"
                name="newsCategory"
              >
                <TaktSelect
                  v-model:value="formState.newsCategory"
                  dict-type="sys_news_type"
                  :placeholder="pi.ph('newsCategory')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('newsTitle')"
                name="newsTitle"
              >
                <a-input
                  v-model:value="formState.newsTitle"
                  :placeholder="pi.ph('newsTitle')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('newsSummary')"
                name="newsSummary"
              >
                <a-input
                  v-model:value="formState.newsSummary"
                  :placeholder="pi.ph('newsSummary')"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('newsTags')"
                name="newsTags"
              >
                <a-input
                  v-model:value="formState.newsTags"
                  :placeholder="pi.ph('newsTags')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('newsContent')"
                name="newsContent"
              >
                <TaktRichEditor
                  v-model:value="formState.newsContent"
                  :placeholder="pi.ph('newsContent')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('newsCoverImage')"
                name="newsCoverImage"
              >
                <a-input
                  v-model:value="formState.newsCoverImage"
                  :placeholder="pi.ph('newsCoverImage')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('newsIsTop')"
                name="newsIsTop"
              >
                <TaktSelect
                  v-model:value="formState.newsIsTop"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('newsIsTop')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('newsIsRecommended')"
                name="newsIsRecommended"
              >
                <TaktSelect
                  v-model:value="formState.newsIsRecommended"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('newsIsRecommended')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('newsEffectiveTime')"
                name="newsEffectiveTime"
              >
                <a-date-picker
                  v-model:value="formState.newsEffectiveTime"
                  :placeholder="pi.ph('newsEffectiveTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('newsExpireTime')"
                name="newsExpireTime"
              >
                <a-date-picker
                  v-model:value="formState.newsExpireTime"
                  :placeholder="pi.ph('newsExpireTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('newsReadCount')"
                name="newsReadCount"
              >
                <a-input-number
                  v-model:value="formState.newsReadCount"
                  :placeholder="pi.ph('newsReadCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('newsLikeCount')"
                name="newsLikeCount"
              >
                <a-input-number
                  v-model:value="formState.newsLikeCount"
                  :placeholder="pi.ph('newsLikeCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('newsCommentCount')"
                name="newsCommentCount"
              >
                <a-input-number
                  v-model:value="formState.newsCommentCount"
                  :placeholder="pi.ph('newsCommentCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('newsFavoriteCount')"
                name="newsFavoriteCount"
              >
                <a-input-number
                  v-model:value="formState.newsFavoriteCount"
                  :placeholder="pi.ph('newsFavoriteCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('newsShareCount')"
                name="newsShareCount"
              >
                <a-input-number
                  v-model:value="formState.newsShareCount"
                  :placeholder="pi.ph('newsShareCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('newsAttachmentCount')"
                name="newsAttachmentCount"
              >
                <a-input-number
                  v-model:value="formState.newsAttachmentCount"
                  :placeholder="pi.ph('newsAttachmentCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('deptId')"
                name="deptId"
              >
                <TaktSelect
                  v-model:value="formState.deptId"
                  api-url="TaktDepts/tree-options"
                  :placeholder="pi.ph('deptId')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('deptName')"
                name="deptName"
              >
                <a-input
                  v-model:value="formState.deptName"
                  :placeholder="pi.ph('deptName')"
                  show-count
                  :maxlength="100"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('publisherId')"
                name="publisherId"
              >
                <TaktSelect
                  v-model:value="formState.publisherId"
                  api-url="TaktUsers/options"
                  :placeholder="pi.ph('publisherId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('publisherName')"
                name="publisherName"
              >
                <a-input
                  v-model:value="formState.publisherName"
                  :placeholder="pi.ph('publisherName')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('newsPublishTime')"
                name="newsPublishTime"
              >
                <a-date-picker
                  v-model:value="formState.newsPublishTime"
                  :placeholder="pi.ph('newsPublishTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('targetScope')"
                name="targetScope"
              >
                <TaktSelect
                  v-model:value="formState.targetScope"
                  dict-type="sys_publish_scope"
                  :placeholder="pi.ph('targetScope')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('targetDepartments')"
                name="targetDepartments"
              >
                <a-input
                  v-model:value="formState.targetDepartments"
                  :placeholder="pi.ph('targetDepartments')"
                  show-count
                  :maxlength="1000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('targetUsers')"
                name="targetUsers"
              >
                <a-input
                  v-model:value="formState.targetUsers"
                  :placeholder="pi.ph('targetUsers')"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('newsStatus')"
                name="newsStatus"
              >
                <TaktSelect
                  v-model:value="formState.newsStatus"
                  dict-type="sys_publish_status"
                  :placeholder="pi.ph('newsStatus')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/4)'"
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * 新闻中心主实体 支持分类、置顶、推荐、社交统计维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/routine/news-center/news/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useNewsI18n } from '../composables/useNewsI18n'

/** 实体字段 i18n */
const pi = useNewsI18n()
import type { NewsCreate } from '@/types/routine/news-center/news.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'
import { useTaktFormNumbering } from '@/composables/useTaktFormNumbering'

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
/** 表单内容区高度 class（多 Tab 大表单固定 10 行高度） */
const formContentClass = 'takt-form-content-rows-10'
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<NewsCreate & { newsId?: string }> | null
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
  newsStatus: 0,
  targetScope: 0,
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 newsId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.newsId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      Object.assign(formState, next)
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
    if (!props.formData?.newsId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 是否编辑态 */
const isEditMode = computed(() => !!props.formData?.newsId)

useTaktFormNumbering({
  formState,
  isEdit: isEditMode,
  businessCodeField: 'newsCode',
})

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  numberingRuleCode: [{
    validator: async (_rule, value) => {
      if (isEditMode.value) {
        return Promise.resolve()
      }
      if (!String(value ?? '').trim()) {
        return Promise.reject(t('common.page.form.numberingRuleRequired'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  newsCode: [{
    validator: async (_rule, value) => {
      if (isEditMode.value) {
        return Promise.resolve()
      }
      if (!String(value ?? '').trim()) {
        return Promise.reject(t('common.page.form.numberingCodePreview'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  newsCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('newsCategory'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('newsCategory'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  newsTitle: [
    {
      required: true,
      message: pi.ph('newsTitle'),
      trigger: 'blur'
    }
  ],
  newsContent: [
    {
      required: true,
      message: pi.ph('newsContent'),
      trigger: 'blur'
    }
  ],
  newsIsTop: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('newsIsTop'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('newsIsTop'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  newsIsRecommended: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('newsIsRecommended'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('newsIsRecommended'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  newsReadCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('newsReadCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('newsReadCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  newsLikeCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('newsLikeCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('newsLikeCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  newsCommentCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('newsCommentCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('newsCommentCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  newsFavoriteCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('newsFavoriteCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('newsFavoriteCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  newsShareCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('newsShareCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('newsShareCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  newsAttachmentCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('newsAttachmentCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('newsAttachmentCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  publisherId: [
    {
      required: true,
      message: pi.ph('publisherId'),
      trigger: 'change'
    }
  ],
  targetScope: [
    {
      required: true,
      message: pi.ph('targetScope'),
      trigger: 'change',
    },
  ],
  newsStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('newsStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('newsStatus'))
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

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('newsCategory' in payload) {
    const rawnewsCategory = payload.newsCategory
    if (rawnewsCategory === undefined || rawnewsCategory === null || rawnewsCategory === '') {
      delete payload.newsCategory
    } else {
      const numnewsCategory = typeof rawnewsCategory === 'number' ? rawnewsCategory : Number(rawnewsCategory)
      if (Number.isFinite(numnewsCategory)) payload.newsCategory = numnewsCategory
      else delete payload.newsCategory
    }
  }
  if ('newsIsTop' in payload) {
    const rawnewsIsTop = payload.newsIsTop
    if (rawnewsIsTop === undefined || rawnewsIsTop === null || rawnewsIsTop === '') {
      delete payload.newsIsTop
    } else {
      const numnewsIsTop = typeof rawnewsIsTop === 'number' ? rawnewsIsTop : Number(rawnewsIsTop)
      if (Number.isFinite(numnewsIsTop)) payload.newsIsTop = numnewsIsTop
      else delete payload.newsIsTop
    }
  }
  if ('newsIsRecommended' in payload) {
    const rawnewsIsRecommended = payload.newsIsRecommended
    if (rawnewsIsRecommended === undefined || rawnewsIsRecommended === null || rawnewsIsRecommended === '') {
      delete payload.newsIsRecommended
    } else {
      const numnewsIsRecommended = typeof rawnewsIsRecommended === 'number' ? rawnewsIsRecommended : Number(rawnewsIsRecommended)
      if (Number.isFinite(numnewsIsRecommended)) payload.newsIsRecommended = numnewsIsRecommended
      else delete payload.newsIsRecommended
    }
  }
  if ('newsReadCount' in payload) {
    const rawnewsReadCount = payload.newsReadCount
    if (rawnewsReadCount === undefined || rawnewsReadCount === null || rawnewsReadCount === '') {
      delete payload.newsReadCount
    } else {
      const numnewsReadCount = typeof rawnewsReadCount === 'number' ? rawnewsReadCount : Number(rawnewsReadCount)
      if (Number.isFinite(numnewsReadCount)) payload.newsReadCount = numnewsReadCount
      else delete payload.newsReadCount
    }
  }
  if ('newsLikeCount' in payload) {
    const rawnewsLikeCount = payload.newsLikeCount
    if (rawnewsLikeCount === undefined || rawnewsLikeCount === null || rawnewsLikeCount === '') {
      delete payload.newsLikeCount
    } else {
      const numnewsLikeCount = typeof rawnewsLikeCount === 'number' ? rawnewsLikeCount : Number(rawnewsLikeCount)
      if (Number.isFinite(numnewsLikeCount)) payload.newsLikeCount = numnewsLikeCount
      else delete payload.newsLikeCount
    }
  }
  if ('newsCommentCount' in payload) {
    const rawnewsCommentCount = payload.newsCommentCount
    if (rawnewsCommentCount === undefined || rawnewsCommentCount === null || rawnewsCommentCount === '') {
      delete payload.newsCommentCount
    } else {
      const numnewsCommentCount = typeof rawnewsCommentCount === 'number' ? rawnewsCommentCount : Number(rawnewsCommentCount)
      if (Number.isFinite(numnewsCommentCount)) payload.newsCommentCount = numnewsCommentCount
      else delete payload.newsCommentCount
    }
  }
  if ('newsFavoriteCount' in payload) {
    const rawnewsFavoriteCount = payload.newsFavoriteCount
    if (rawnewsFavoriteCount === undefined || rawnewsFavoriteCount === null || rawnewsFavoriteCount === '') {
      delete payload.newsFavoriteCount
    } else {
      const numnewsFavoriteCount = typeof rawnewsFavoriteCount === 'number' ? rawnewsFavoriteCount : Number(rawnewsFavoriteCount)
      if (Number.isFinite(numnewsFavoriteCount)) payload.newsFavoriteCount = numnewsFavoriteCount
      else delete payload.newsFavoriteCount
    }
  }
  if ('newsShareCount' in payload) {
    const rawnewsShareCount = payload.newsShareCount
    if (rawnewsShareCount === undefined || rawnewsShareCount === null || rawnewsShareCount === '') {
      delete payload.newsShareCount
    } else {
      const numnewsShareCount = typeof rawnewsShareCount === 'number' ? rawnewsShareCount : Number(rawnewsShareCount)
      if (Number.isFinite(numnewsShareCount)) payload.newsShareCount = numnewsShareCount
      else delete payload.newsShareCount
    }
  }
  if ('newsAttachmentCount' in payload) {
    const rawnewsAttachmentCount = payload.newsAttachmentCount
    if (rawnewsAttachmentCount === undefined || rawnewsAttachmentCount === null || rawnewsAttachmentCount === '') {
      delete payload.newsAttachmentCount
    } else {
      const numnewsAttachmentCount = typeof rawnewsAttachmentCount === 'number' ? rawnewsAttachmentCount : Number(rawnewsAttachmentCount)
      if (Number.isFinite(numnewsAttachmentCount)) payload.newsAttachmentCount = numnewsAttachmentCount
      else delete payload.newsAttachmentCount
    }
  }
  if ('targetScope' in payload) {
    const rawTargetScope = payload.targetScope
    if (rawTargetScope === undefined || rawTargetScope === null || rawTargetScope === '') {
      delete payload.targetScope
    } else {
      const numTargetScope = typeof rawTargetScope === 'number' ? rawTargetScope : Number(rawTargetScope)
      if (Number.isFinite(numTargetScope)) payload.targetScope = numTargetScope
      else delete payload.targetScope
    }
  }
  if ('newsStatus' in payload) {
    const rawnewsStatus = payload.newsStatus
    if (rawnewsStatus === undefined || rawnewsStatus === null || rawnewsStatus === '') {
      delete payload.newsStatus
    } else {
      const numnewsStatus = typeof rawnewsStatus === 'number' ? rawnewsStatus : Number(rawnewsStatus)
      if (Number.isFinite(numnewsStatus)) payload.newsStatus = numnewsStatus
      else delete payload.newsStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }
  if (props.formData?.newsId) {
    payload.newsId = props.formData.newsId
    delete payload.numberingRuleCode
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.newsId)

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
