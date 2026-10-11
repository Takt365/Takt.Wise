<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/materials/packaging-material/components -->
<!-- 文件名称：PackagingMaterialForm.vue -->
<!-- 功能描述：Takt包装物料实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="packaging-material-form-tabs"
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
                :label="pi.label('packagingMaterialCode')"
                name="packagingMaterialCode"
              >
                <a-input
                  v-model:value="formState.packagingMaterialCode"
                  :placeholder="pi.ph('packagingMaterialCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.packagingMaterialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialCode')"
                name="materialCode"
              >
                <TaktSelect
                  v-model:value="formState.materialCode"
                  api-url="TaktMaterialPlants/options"
                  :placeholder="pi.ph('materialCode')"
                  :disabled="!!formData?.packagingMaterialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('materialDescription')"
                name="materialDescription"
              >
                <a-textarea
                  v-model:value="formState.materialDescription"
                  :placeholder="pi.ph('materialDescription')"
                  :rows="2"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('hsCode')"
                name="hsCode"
              >
                <a-input
                  v-model:value="formState.hsCode"
                  :placeholder="pi.ph('hsCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.packagingMaterialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('hsName')"
                name="hsName"
              >
                <a-input
                  v-model:value="formState.hsName"
                  :placeholder="pi.ph('hsName')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('additionalCode')"
                name="additionalCode"
              >
                <a-input
                  v-model:value="formState.additionalCode"
                  :placeholder="pi.ph('additionalCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.packagingMaterialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('originCountryRegionCode')"
                name="originCountryRegionCode"
              >
                <TaktSelect
                  v-model:value="formState.originCountryRegionCode"
                  dict-type="sys_country_code"
                  :placeholder="pi.ph('originCountryRegionCode')"
                  :disabled="!!formData?.packagingMaterialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('originCountryRegionName')"
                name="originCountryRegionName"
              >
                <a-input
                  v-model:value="formState.originCountryRegionName"
                  :placeholder="pi.ph('originCountryRegionName')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('destinationCountryRegionCode')"
                name="destinationCountryRegionCode"
              >
                <TaktSelect
                  v-model:value="formState.destinationCountryRegionCode"
                  dict-type="sys_country_code"
                  :placeholder="pi.ph('destinationCountryRegionCode')"
                  :disabled="!!formData?.packagingMaterialId"
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
                :label="pi.label('destinationCountryRegionName')"
                name="destinationCountryRegionName"
              >
                <a-input
                  v-model:value="formState.destinationCountryRegionName"
                  :placeholder="pi.ph('destinationCountryRegionName')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('regulatoryConditionCode')"
                name="regulatoryConditionCode"
              >
                <a-input
                  v-model:value="formState.regulatoryConditionCode"
                  :placeholder="pi.ph('regulatoryConditionCode')"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.packagingMaterialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('tariffRateType')"
                name="tariffRateType"
              >
                <a-input
                  v-model:value="formState.tariffRateType"
                  :placeholder="pi.ph('tariffRateType')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('grossWeight')"
                name="grossWeight"
              >
                <a-input-number
                  v-model:value="formState.grossWeight"
                  :placeholder="pi.ph('grossWeight')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('netWeight')"
                name="netWeight"
              >
                <a-input-number
                  v-model:value="formState.netWeight"
                  :placeholder="pi.ph('netWeight')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('weightUnit')"
                name="weightUnit"
              >
                <TaktSelect
                  v-model:value="formState.weightUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('weightUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('businessVolume')"
                name="businessVolume"
              >
                <a-input-number
                  v-model:value="formState.businessVolume"
                  :placeholder="pi.ph('businessVolume')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('volumeUnit')"
                name="volumeUnit"
              >
                <TaktSelect
                  v-model:value="formState.volumeUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('volumeUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sizeDimension')"
                name="sizeDimension"
              >
                <a-input
                  v-model:value="formState.sizeDimension"
                  :placeholder="pi.ph('sizeDimension')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('packagingType')"
                name="packagingType"
              >
                <TaktSelect
                  v-model:value="formState.packagingType"
                  dict-type="logistics_materials_material_type"
                  :placeholder="pi.ph('packagingType')"
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
                :label="pi.label('packingUnit')"
                name="packingUnit"
              >
                <TaktSelect
                  v-model:value="formState.packingUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('packingUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('quantityPerPacking')"
                name="quantityPerPacking"
              >
                <a-input-number
                  v-model:value="formState.quantityPerPacking"
                  :placeholder="pi.ph('quantityPerPacking')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('packagingSpec')"
                name="packagingSpec"
              >
                <a-input
                  v-model:value="formState.packagingSpec"
                  :placeholder="pi.ph('packagingSpec')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('packagingDescription')"
                name="packagingDescription"
              >
                <a-textarea
                  v-model:value="formState.packagingDescription"
                  :placeholder="pi.ph('packagingDescription')"
                  :rows="2"
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
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="pi.ph('companyCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('cultureCode')"
                name="cultureCode"
              >
                <a-input
                  v-model:value="formState.cultureCode"
                  :placeholder="pi.ph('cultureCode')"
                  show-count
                  :maxlength="20"
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
 * Takt包装物料实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/packaging-material/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePackagingMaterialI18n } from '../composables/usePackagingMaterialI18n'

/** 实体字段 i18n */
const pi = usePackagingMaterialI18n()
import type { PackagingMaterialCreate } from '@/types/logistics/materials/packagingMaterial.types'
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
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }
}
/** 表单内容区高度 class（多 Tab 大表单固定 10 行高度） */
const formContentClass = 'takt-form-content-rows-10'
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PackagingMaterialCreate & { packagingMaterialId?: string }> | null
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
  packagingType: "ROH"
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 packagingMaterialId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.packagingMaterialId) {
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
    if (!props.formData?.packagingMaterialId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  packagingMaterialCode: [
    {
      required: true,
      message: pi.ph('packagingMaterialCode'),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'change'
    }
  ],
  weightUnit: [
    {
      required: true,
      message: pi.ph('weightUnit'),
      trigger: 'change'
    }
  ],
  volumeUnit: [
    {
      required: true,
      message: pi.ph('volumeUnit'),
      trigger: 'change'
    }
  ],
  packagingType: [
    {
      required: true,
      message: pi.ph('packagingType'),
      trigger: 'change'
    }
  ],
  packingUnit: [
    {
      required: true,
      message: pi.ph('packingUnit'),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('grossWeight' in payload) {
    const rawgrossWeight = payload.grossWeight
    payload.grossWeight = typeof rawgrossWeight === 'number' ? rawgrossWeight : Number(rawgrossWeight)
  }
  if ('netWeight' in payload) {
    const rawnetWeight = payload.netWeight
    payload.netWeight = typeof rawnetWeight === 'number' ? rawnetWeight : Number(rawnetWeight)
  }
  if ('businessVolume' in payload) {
    const rawbusinessVolume = payload.businessVolume
    payload.businessVolume = typeof rawbusinessVolume === 'number' ? rawbusinessVolume : Number(rawbusinessVolume)
  }
  if ('quantityPerPacking' in payload) {
    const rawquantityPerPacking = payload.quantityPerPacking
    payload.quantityPerPacking = typeof rawquantityPerPacking === 'number' ? rawquantityPerPacking : Number(rawquantityPerPacking)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.packagingMaterialId)

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
