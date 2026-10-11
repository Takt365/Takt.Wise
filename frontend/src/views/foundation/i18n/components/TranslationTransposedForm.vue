<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/foundation/i18n/components -->
<!-- 文件名称：TranslationTransposedForm.vue -->
<!-- 创建时间：2026-01-29 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：翻译转置表单，用于创建/编辑一个 i18n 键下的所有语言翻译 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="translation-transposed-form">
    <a-form
      ref="formRef"
      class="takt-generated-form"
      :model="formState"
      :rules="formRules"
      layout="horizontal"
      label-align="right"
    >
      <a-form-item
        :label="t('entity.translation.i18nkey')"
        name="i18nKey"
      >
        <a-input
          v-model:value="formState.i18nKey"
          :placeholder="t('common.page.form.placeholder.input', { field: t('entity.translation.i18nkey') })"
          :disabled="isEdit"
        />
      </a-form-item>
      <a-form-item
        :label="t('entity.translation.resourcetype')"
        name="resourceType"
      >
        <TaktSelect
          v-model:value="formState.resourceType"
          dict-type="sys_resource_type"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.translation.resourcetype') })"
          :disabled="props.loading"
        />
      </a-form-item>
      <a-form-item
        :label="t('entity.translation.resourcegroup')"
        name="resourceGroup"
      >
        <TaktTreeSelect
          v-model:value="formState.resourceGroup"
          api-url="TaktMenus/tree-options"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.translation.resourcegroup') })"
          allow-clear
          :field-names="{ label: 'dictLabel', value: 'dictValue' }"
          :disabled="props.loading"
        />
      </a-form-item>
      <a-divider orientation="left">
        {{ t('entity.culture.translationlist') }}
      </a-divider>
      <a-form-item
        v-for="culture in cultureList"
        :key="culture.cultureCode"
        :label="culture.label"
        :name="['translations', culture.cultureCode]"
      >
        <a-input
          :value="formState.translations[culture.cultureCode] ?? ''"
          @update:value="(val) => { formState.translations[culture.cultureCode] = val }"
          :placeholder="t('common.page.form.placeholder.required', { field: culture.label })"
        />
      </a-form-item>
      <a-form-item
        :label="t('common.page.entity.remark')"
        name="remark"
      >
        <a-textarea
          v-model:value="formState.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
          :rows="2"
        />
      </a-form-item>
    </a-form>
  </div>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { getCultureOptions } from '@/api/foundation/culture.api'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import type { Translation } from '@/types/foundation/translation.types'

/** 区域文化选项（转置表单按语言列展开） */
interface CultureOptionItem {
  cultureId: string
  cultureCode: string
  label: string
}

/** 转置表单 getFormData 返回值 */
export interface TranslationTransposedFormData {
  i18nKey: string
  resourceType: string
  resourceGroup: string
  remark: string
  translations: Record<string, string>
  translationIds: Record<string, string>
  cultureIds: Record<string, string>
}

interface Props {
  /** 编辑模式下，传入该 i18n 键下的所有翻译 */
  formData?: Translation[] | null
  /** 提交 loading */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false
})

const { t } = useI18n()
/** 字典缓存（sys_resource_type 等） */
const dictDataStore = useDictDataStore()

/** 表单 ref */
const formRef = ref()
/** 区域文化列表 */
const cultureList = ref<CultureOptionItem[]>([])
/** 区域文化列表 loading */
const cultureListLoading = ref(false)

/** 表单绑定状态 */
const formState = reactive<TranslationTransposedFormData>({
  i18nKey: '',
  resourceType: 'frontend',
  resourceGroup: '',
  remark: '',
  translations: {},
  translationIds: {},
  cultureIds: {}
})

/** 是否编辑模式 */
const isEdit = computed(() => Boolean(props.formData && props.formData.length > 0))

/** 校验规则 */
const formRules = computed<Record<string, Rule[]>>(() => ({
  i18nKey: [{ required: true, message: t('common.page.form.placeholder.required', { field: t('entity.translation.i18nkey') }), trigger: 'blur' }],
  resourceType: [{ required: true, message: t('common.page.form.placeholder.select', { field: t('entity.translation.resourcetype') }), trigger: 'change' }],
  resourceGroup: [{ required: true, message: t('common.page.form.placeholder.select', { field: t('entity.translation.resourcegroup') }), trigger: 'change' }]
}))

/**
 * 解析资源类别（字典 sys_resource_type：frontend/backend）
 * @param value 原始值
 * @returns {string} 资源类别
 */
function parseResourceType(value: unknown): string {
  const text = value == null ? '' : String(value).trim()
  return text || 'frontend'
}

/**
 * 解析资源分组
 * @param value 原始值
 * @returns {string} 资源分组
 */
function parseResourceGroup(value: unknown): string {
  if (value == null) return ''
  return String(value).trim()
}

/**
 * 初始化各语言翻译输入槽位
 * @returns {void}
 */
function initTranslationSlots() {
  cultureList.value.forEach((culture) => {
    if (!(culture.cultureCode in formState.translations)) {
      formState.translations[culture.cultureCode] = ''
    }
    if (!(culture.cultureCode in formState.cultureIds)) {
      formState.cultureIds[culture.cultureCode] = culture.cultureId
    }
  })
}

/**
 * 加载区域文化列表
 * @returns {Promise<void>}
 */
async function loadCultureList() {
  try {
    cultureListLoading.value = true
    const listRaw = (await getCultureOptions()) as unknown[]
    cultureList.value = listRaw.map((x) => {
      const r = x as Record<string, unknown>
      return {
        cultureId: String(r['extValue'] ?? ''),
        cultureCode: String(r['dictValue'] ?? ''),
        label: `${String(r['dictLabel'] ?? '')} (${String(r['extLabel'] ?? r['dictValue'] ?? '')})`
      }
    })
    initTranslationSlots()
  } finally {
    cultureListLoading.value = false
  }
}

watch(
  () => props.formData,
  (v) => {
    if (v && v.length > 0) {
      const first = v[0]!
      formState.i18nKey = first.i18nKey ?? ''
      formState.resourceType = parseResourceType(first.resourceType)
      formState.resourceGroup = parseResourceGroup(first.resourceGroup)
      formState.remark = first.remark ?? ''
      formState.translations = {}
      formState.translationIds = {}
      formState.cultureIds = {}
      v.forEach((tr) => {
        if (tr.cultureCode) {
          formState.translations[tr.cultureCode] = tr.translationText ?? ''
          formState.translationIds[tr.cultureCode] = tr.translationId ?? ''
          formState.cultureIds[tr.cultureCode] = tr.cultureId ?? ''
        }
      })
      cultureList.value.forEach((culture) => {
        if (!(culture.cultureCode in formState.translations)) {
          formState.translations[culture.cultureCode] = ''
          formState.cultureIds[culture.cultureCode] = culture.cultureId
        }
      })
    } else {
      formState.i18nKey = ''
      formState.resourceType = 'frontend'
      formState.resourceGroup = ''
      formState.remark = ''
      formState.translations = {}
      formState.translationIds = {}
      formState.cultureIds = {}
      initTranslationSlots()
    }
  },
  { immediate: true, deep: true }
)

onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
  loadCultureList()
})

/**
 * 校验表单
 * @returns {Promise<void>}
 */
async function validate() {
  await formRef.value?.validate()
}

/**
 * 获取提交数据
 * @returns {TranslationTransposedFormData} 转置表单数据
 */
function getFormData(): TranslationTransposedFormData {
  return {
    i18nKey: formState.i18nKey,
    resourceType: formState.resourceType,
    resourceGroup: formState.resourceGroup,
    remark: formState.remark,
    translations: { ...formState.translations },
    translationIds: { ...formState.translationIds },
    cultureIds: { ...formState.cultureIds }
  }
}

defineExpose({ validate, getFormData })
</script>

<style scoped lang="css">
.translation-transposed-form {
  padding: 0;
  max-height: 60vh;
  overflow-y: auto;
}
</style>
