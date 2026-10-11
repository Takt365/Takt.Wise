<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/routing-item/components -->
<!-- 文件名称：RoutingItemArgumentForm.vue -->
<!-- 功能描述：工艺路线明细表实体子表 routingItemArgument 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form routing-item-argument-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="routing-item-argument-form-tabs"
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
                :label="pi.label('paramCode')"
                name="paramCode"
              >
                <a-input
                  v-model:value="formState.paramCode"
                  :placeholder="pi.ph('paramCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.routingItemArgumentId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('paramName')"
                name="paramName"
              >
                <a-input
                  v-model:value="formState.paramName"
                  :placeholder="pi.ph('paramName')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('paramUnit')"
                name="paramUnit"
              >
                <TaktSelect
                  v-model:value="formState.paramUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('paramUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('standardValue')"
                name="standardValue"
              >
                <a-input-number
                  v-model:value="formState.standardValue"
                  :placeholder="pi.ph('standardValue')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('lowerLimit')"
                name="lowerLimit"
              >
                <a-input-number
                  v-model:value="formState.lowerLimit"
                  :placeholder="pi.ph('lowerLimit')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('upperLimit')"
                name="upperLimit"
              >
                <a-input-number
                  v-model:value="formState.upperLimit"
                  :placeholder="pi.ph('upperLimit')"
                  style="width: 100%"
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
 * 工艺路线明细表实体子表 routingItemArgument 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/bom/routing-item/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useRoutingItemArgumentI18n } from '../composables/useRoutingItemArgumentI18n'

/** 实体字段 i18n */
const pi = useRoutingItemArgumentI18n()

import type { RoutingItemArgumentCreate } from '@/types/logistics/manufacturing/bom/routingItemArgument.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["paramCode","paramName","paramUnit","standardValue","lowerLimit","upperLimit"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<RoutingItemArgumentCreate & { routingItemArgumentId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 routingItemArgumentId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.routingItemArgumentId) {
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
  paramCode: [
    {
      required: true,
      message: pi.ph('paramCode'),
      trigger: 'blur'
    }
  ],
  paramName: [
    {
      required: true,
      message: pi.ph('paramName'),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 routingItemId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('standardValue' in payload) {
    const rawstandardValue = payload.standardValue
    payload.standardValue = typeof rawstandardValue === 'number' ? rawstandardValue : Number(rawstandardValue)
  }
  if ('lowerLimit' in payload) {
    const rawlowerLimit = payload.lowerLimit
    payload.lowerLimit = typeof rawlowerLimit === 'number' ? rawlowerLimit : Number(rawlowerLimit)
  }
  if ('upperLimit' in payload) {
    const rawupperLimit = payload.upperLimit
    payload.upperLimit = typeof rawupperLimit === 'number' ? rawupperLimit : Number(rawupperLimit)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.routingItemId = props.masterId
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
