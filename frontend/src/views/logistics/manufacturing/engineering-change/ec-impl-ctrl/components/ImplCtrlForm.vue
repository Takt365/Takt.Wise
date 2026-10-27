<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-impl-ctrl/components -->
<!-- 文件名称：ImplCtrlForm.vue -->
<!-- 功能描述：控制范围编辑；单列展示设变号、机种编码组、根物料编码组，以及当前部门控制范围；defineExpose validate/getValues/resetFields -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    layout="horizontal"
    label-align="right"
  >
    <a-row :gutter="24">
      <a-col :span="24"><a-form-item :label="pi.label('ecCode')"><a-input v-model:value="formState.ecCode" disabled /></a-form-item></a-col>
      <a-col :span="24">
        <a-form-item :label="t('entity.ecqc.ecmodelcodegroup')">
          <a-input :value="formState.ecModelCodeGroup" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item :label="t('entity.ecqc.ecrootmaterialcodegroup')">
          <a-input :value="formState.ecRootMaterialCodeGroup" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item :label="t('entity.ecte.eccontrolscope')">
          <TaktSelect
            v-model:value="formState.ecControlScope"
            dict-type="logistics_manufacturing_ec_control_scope_category"
            :disabled="loading"
          />
        </a-form-item>
      </a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 控制范围编辑：当前部门选择按成品、按机种、按设变或不管理
 */
import { useI18n } from 'vue-i18n'
import type { EcImplCtrl, EcImplCtrlUpdate } from '@/types/logistics/manufacturing/engineering-change/ecImplCtrl.types'
import { useEcDetailI18n } from '@/views/logistics/manufacturing/engineering-change/ec-te/composables/useEcDetailI18n'

const props = defineProps<{
  formData?: EcImplCtrl | null
  loading?: boolean
  /** pc / assy / pcba / qa 只更新该部门执行表 */
  deptTab?: 'pc' | 'mfg2' | 'mfg1' | 'qc'
}>()
const { t } = useI18n()
const pi = useEcDetailI18n()
const formRef = ref()
const formState = reactive<EcImplCtrlUpdate & { ecCode?: string; ecModelCode?: string; ecModelCodeGroup?: string; ecRootMaterialCodeGroup?: string }>({
  ecDetailId: '',
  ecControlScope: 2,
})

/**
 * 只接受 1～4，缺省按机种
 * @param value 已保存的控制范围
 * @returns {number} 1～4
 */
function asControlScope(value: number | undefined): number {
  return value === 1 || value === 2 || value === 3 || value === 4 ? value : 2
}

watch(() => props.formData, (val) => {
  if (!val) {
    resetFields()
    return
  }
  Object.assign(formState, {
    ecDetailId: val.ecDetailId,
    ecCode: val.ecCode,
    ecModelCode: val.ecModelCode,
    ecModelCodeGroup: val.ecModelCodeGroup,
    ecRootMaterialCodeGroup: val.ecRootMaterialCodeGroup,
    ecControlScope: asControlScope(val.ecControlScope),
  })
}, { immediate: true })

/** 校验表单 */
async function validate() {
  await formRef.value?.validate()
}

/**
 * 提交值
 * @returns {EcImplCtrlUpdate} 当前部门的控制范围
 */
function getValues(): EcImplCtrlUpdate {
  return {
    ecDetailId: formState.ecDetailId,
    ecControlScope: asControlScope(formState.ecControlScope),
    deptTab: props.deptTab,
  }
}

/** 清空表单 */
function resetFields() {
  Object.assign(formState, {
    ecDetailId: '',
    ecCode: '',
    ecModelCode: '',
    ecModelCodeGroup: '',
    ecRootMaterialCodeGroup: '',
    ecControlScope: 2,
  })
}

defineExpose({ validate, getValues, resetFields })
</script>
