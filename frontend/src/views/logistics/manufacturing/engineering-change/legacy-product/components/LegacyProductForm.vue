<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/legacy-product/components -->
<!-- 文件名称：LegacyProductForm.vue -->
<!-- 功能描述：旧品管制编辑表单；defineExpose validate/getValues/resetFields -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
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
    <a-tabs v-model:active-key="activeTab">
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item :label="pi.label('tenantCode')" name="tenantCode">
                <a-input v-model:value="formState.tenantCode" disabled :placeholder="pi.ph('tenantCode')" />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item :label="pi.label('companyCode')" name="companyCode">
                <a-input v-model:value="formState.companyCode" disabled :placeholder="pi.ph('companyCode')" />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item :label="pi.label('cultureCode')" name="cultureCode">
                <a-input v-model:value="formState.cultureCode" disabled :placeholder="pi.ph('cultureCode')" />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item :label="pi.label('plantCode')" name="plantCode">
                <a-input v-model:value="formState.plantCode" disabled :placeholder="pi.ph('plantCode')" show-count :maxlength="4" />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item :label="pi.label('ecCode')" name="ecCode">
                <a-input v-model:value="formState.ecCode" disabled :placeholder="pi.ph('ecCode')" show-count :maxlength="10" />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item :label="pi.label('lineNumber')" name="lineNumber">
                <a-input-number v-model:value="formState.lineNumber" disabled :placeholder="pi.ph('lineNumber')" style="width: 100%" />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item :label="pi.label('ecModelCode')" name="ecModelCode">
                <a-input v-model:value="formState.ecModelCode" disabled :placeholder="pi.ph('ecModelCode')" show-count :maxlength="40" />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item :label="pi.label('ecOldMaterialCode')" name="ecOldMaterialCode">
                <a-input v-model:value="formState.ecOldMaterialCode" disabled :placeholder="pi.ph('ecOldMaterialCode')" show-count :maxlength="20" />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item :label="pi.label('ecOldMaterialDescription')" name="ecOldMaterialDescription">
                <a-input v-model:value="formState.ecOldMaterialDescription" disabled :placeholder="pi.ph('ecOldMaterialDescription')" show-count :maxlength="40" />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item :label="pi.label('ecOldUsageQuantity')" name="ecOldUsageQuantity">
                <a-input-number v-model:value="formState.ecOldUsageQuantity" disabled :placeholder="pi.ph('ecOldUsageQuantity')" style="width: 100%" />
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
              <a-form-item :label="pi.label('ecNewMaterialCode')" name="ecNewMaterialCode">
                <a-input v-model:value="formState.ecNewMaterialCode" disabled :placeholder="pi.ph('ecNewMaterialCode')" show-count :maxlength="20" />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item :label="pi.label('ecIsCompatible')" name="ecIsCompatible">
                <a-input
                  v-model:value="formState.ecIsCompatible"
                  :placeholder="pi.ph('ecIsCompatible')"
                  disabled
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item :label="pi.label('ec2ndVendor')" name="ec2ndVendor">
                <TaktSelect
                  v-model:value="formState.ec2ndVendor"
                  dict-type="logistics_manufacturing_ec_2nd_vendor"
                  :placeholder="pi.ph('ec2ndVendor')"
                  disabled
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item :label="pi.label('ecInstruction')" name="ecInstruction">
                <TaktSelect
                  v-model:value="formState.ecInstruction"
                  dict-type="logistics_manufacturing_ec_source_instruction"
                  :placeholder="pi.ph('ecInstruction')"
                  disabled
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item :label="pi.label('ecOldPartDisposition')" name="ecOldPartDisposition">
                <TaktSelect
                  v-model:value="formState.ecOldPartDisposition"
                  dict-type="logistics_manufacturing_ec_old_part_disposition"
                  :placeholder="pi.ph('ecOldPartDisposition')"
                  disabled
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item :label="pi.label('discontinuedStatus')" name="discontinuedStatus">
                <TaktSelect
                  v-model:value="formState.discontinuedStatus"
                  dict-type="logistics_materials_material_discontinued_status"
                  :placeholder="pi.ph('discontinuedStatus')"
                  :disabled="loading"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item :label="pi.label('oldProductHandling')" name="oldProductHandling">
                <TaktSelect
                  v-model:value="formState.oldProductHandling"
                  dict-type="logistics_manufacturing_ec_old_part_disposition"
                  :placeholder="pi.ph('oldProductHandling')"
                  :disabled="loading"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item :label="pi.label('remark')" name="remark">
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
                  :disabled="loading"
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
 * 旧品管制编辑表单（无新增；隔离字段与料号只读）
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { EcLegacyProduct, EcLegacyProductUpdate } from '@/types/logistics/manufacturing/engineering-change/legacyProduct.types'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useEcLegacyProductI18n } from '../composables/useEcLegacyProductI18n'

/** 实体字段 i18n */
const pi = useEcLegacyProductI18n()
/** i18n 翻译函数 */
const { t } = useI18n()
/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()
/** Pinia：字典（兼容存中文标签时回写成 DictValue，否则 TaktSelect 对不上选项） */
const dictDataStore = useDictDataStore()

interface Props {
  /** 父级传入的编辑 DTO */
  formData?: Partial<EcLegacyProduct> | null
  /** 父级提交 loading */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

/** Create/展示字段名（与 formState 键对齐） */
const formFields = [
  'tenantCode',
  'companyCode',
  'cultureCode',
  'plantCode',
  'ecCode',
  'lineNumber',
  'ecModelCode',
  'ecOldMaterialCode',
  'ecOldMaterialDescription',
  'ecOldUsageQuantity',
  'ecIsCompatible',
  'ec2ndVendor',
  'ecInstruction',
  'ecOldPartDisposition',
  'ecNewMaterialCode',
  'oldProductHandling',
  'discontinuedStatus',
  'remark',
]

/** 表单内容区高度 class */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** a-form 实例 */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, unknown>>({})

/**
 * 上下文隔离字段：租户 / 公司 / 区域文化 / 工厂
 * @param target 表单数据
 * @param force 为 true 时强制覆盖
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

const LEGACY_PRODUCT_DICT_FIELDS = [
  { field: 'ec2ndVendor', dictType: 'logistics_manufacturing_ec_2nd_vendor' },
  { field: 'ecInstruction', dictType: 'logistics_manufacturing_ec_source_instruction' },
  { field: 'ecOldPartDisposition', dictType: 'logistics_manufacturing_ec_old_part_disposition' },
  { field: 'oldProductHandling', dictType: 'logistics_manufacturing_ec_old_part_disposition' },
  { field: 'discontinuedStatus', dictType: 'logistics_materials_material_discontinued_status' },
] as const

/**
 * 将字典列的中文标签规范为 DictValue，供 TaktSelect 匹配选项
 * @param target 表单数据
 */
function normalizeLegacyProductDictValues(target: Record<string, unknown>) {
  for (const item of LEGACY_PRODUCT_DICT_FIELDS) {
    const raw = target[item.field]
    if (raw == null || raw === '') {
      continue
    }
    const option = dictDataStore.getDictOption(raw as string | number, item.dictType, false)
    if (option?.dictValue != null) {
      target[item.field] = String(option.dictValue)
    }
  }
}

/** 编辑态灌入 formData */
watch(
  () => props.formData,
  (val) => {
    Object.keys(formState).forEach((k) => delete formState[k])
    if (val?.ecDetailId) {
      const next = { ...val } as Record<string, unknown>
      applyScopeDefaults(next)
      normalizeLegacyProductDictValues(next)
      Object.assign(formState, next)
    } else {
      formState.discontinuedStatus = 'Z0'
      applyScopeDefaults(formState, true)
    }
    formRef.value?.clearValidate()
  },
  { immediate: true },
)

/** 表单校验规则 */
const rules = computed<Record<string, Rule[]>>(() => ({
  discontinuedStatus: [
    {
      required: true,
      message: pi.ph('discontinuedStatus'),
      trigger: 'change',
    },
  ],
}))

/**
 * 校验表单
 * @returns {Promise<void>}
 */
async function validate() {
  await formRef.value?.validate()
}

/**
 * 映射为 Update DTO
 * @returns {EcLegacyProductUpdate} 更新入参
 */
function getValues(): EcLegacyProductUpdate {
  return {
    ecDetailId: String(formState.ecDetailId ?? ''),
    oldProductHandling: (formState.oldProductHandling as string | undefined) ?? '',
    discontinuedStatus: String(formState.discontinuedStatus ?? 'Z0'),
    remark: (formState.remark as string | undefined) ?? '',
  }
}

/** 重置表单 */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.ecDetailId)
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>
