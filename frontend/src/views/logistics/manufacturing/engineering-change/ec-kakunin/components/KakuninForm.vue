<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-kakunin/components -->
<!-- 文件名称：KakuninForm.vue -->
<!-- 功能描述：物料确认编辑表单；defineExpose validate/getValues/resetFields -->
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
      <a-col :span="12"><a-form-item :label="pi.label('ecCode')"><a-input v-model:value="formState.ecCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecModelCode')"><a-input v-model:value="formState.ecModelCode" disabled /></a-form-item></a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('ecOldRequiresInspection')">
          <TaktSelect v-model:value="formState.ecOldRequiresInspection" dict-type="sys_yes_no" :placeholder="pi.ph('ecOldRequiresInspection')" />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('ecOldPurchaseType')">
          <TaktSelect v-model:value="formState.ecOldPurchaseType" dict-type="sys_yes_no" :placeholder="pi.ph('ecOldPurchaseType')" />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('ecNewRequiresInspection')">
          <TaktSelect v-model:value="formState.ecNewRequiresInspection" dict-type="sys_yes_no" :placeholder="pi.ph('ecNewRequiresInspection')" />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('ecNewPurchaseType')">
          <TaktSelect v-model:value="formState.ecNewPurchaseType" dict-type="sys_yes_no" :placeholder="pi.ph('ecNewPurchaseType')" />
        </a-form-item>
      </a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
import type { EcKakunin, EcKakuninUpdate } from '@/types/logistics/manufacturing/engineering-change/ecKakunin.types';
import { useEcDetailI18n } from '@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables/useEcDetailI18n';

const props = defineProps<{ formData?: EcKakunin | null; loading?: boolean }>();
const pi = useEcDetailI18n();
const formRef = ref();
const formState = reactive<EcKakuninUpdate & { ecCode?: string; ecModelCode?: string }>({
  ecDetailId: '',
  ecOldPurchaseType: 0,
  ecOldRequiresInspection: 0,
  ecNewPurchaseType: 0,
  ecNewRequiresInspection: 0,
});

watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
  Object.assign(formState, {
    ecDetailId: val.ecDetailId,
    ecCode: val.ecCode,
    ecModelCode: val.ecModelCode,
    ecOldPurchaseType: val.ecOldPurchaseType ?? 0,
    ecOldRequiresInspection: val.ecOldRequiresInspection ?? 0,
    ecNewPurchaseType: val.ecNewPurchaseType ?? 0,
    ecNewRequiresInspection: val.ecNewRequiresInspection ?? 0,
  });
}, { immediate: true });

async function validate() { await formRef.value?.validate(); }
function getValues(): EcKakuninUpdate {
  const { ecCode, ecModelCode, ...rest } = formState;
  return rest;
}
function resetFields() {
  Object.assign(formState, {
    ecDetailId: '',
    ecOldPurchaseType: 0,
    ecOldRequiresInspection: 0,
    ecNewPurchaseType: 0,
    ecNewRequiresInspection: 0,
    ecCode: '',
    ecModelCode: '',
  });
}
defineExpose({ validate, getValues, resetFields });
</script>
