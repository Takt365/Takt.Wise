<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-batch/components -->
<!-- 文件名称：BatchForm.vue -->
<!-- 功能描述：投入批次编辑表单；defineExpose validate/getValues/resetFields -->
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
      <a-col :span="12"><a-form-item :label="seikan.label('scheduledBatch')"><a-input v-model:value="formState.scheduledBatch" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="nika.label('productionBatch')"><a-input v-model:value="formState.productionBatch" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="seikan.label('scheduledDate')"><a-date-picker v-model:value="formState.scheduledDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="ikka.label('productionDate')"><a-date-picker v-model:value="formState.productionDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
import type { EcBatch, EcBatchUpdate } from '@/types/logistics/manufacturing/engineering-change/ecBatch.types';
import { useEntityFieldI18n } from '@/composables/useEntityFieldI18n';
import { useEcDetailI18n } from '@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables/useEcDetailI18n';

const props = defineProps<{ formData?: EcBatch | null; loading?: boolean }>();
const pi = useEcDetailI18n();
const seikan = useEntityFieldI18n('ecseikan');
const nika = useEntityFieldI18n('ecseizounika');
const ikka = useEntityFieldI18n('ecseizouikka');
const formRef = ref();
const formState = reactive<EcBatchUpdate & { ecCode?: string; ecModelCode?: string }>({
  ecDetailId: '',
  scheduledBatch: '',
  productionBatch: '',
});

watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
  Object.assign(formState, {
    ecDetailId: val.ecDetailId,
    ecCode: val.ecCode,
    ecModelCode: val.ecModelCode,
    scheduledBatch: val.scheduledBatch ?? '',
    productionBatch: val.productionBatch ?? '',
    scheduledDate: val.scheduledDate,
    productionDate: val.productionDate,
  });
}, { immediate: true });

async function validate() { await formRef.value?.validate(); }
function getValues(): EcBatchUpdate {
  const { ecCode, ecModelCode, ...rest } = formState;
  return rest;
}
function resetFields() {
  Object.assign(formState, { ecDetailId: '', scheduledBatch: '', productionBatch: '', ecCode: '', ecModelCode: '' });
}
defineExpose({ validate, getValues, resetFields });
</script>
