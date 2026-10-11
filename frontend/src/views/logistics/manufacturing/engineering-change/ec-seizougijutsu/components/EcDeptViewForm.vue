<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-seizougijutsu/components -->
<!-- 文件名称：EcDeptViewForm.vue -->
<!-- 功能描述：设变制造技术课表单；冗余字段 disabled；IsSopUpdated/IsImplemented/ExecContent/课别填报可编辑；defineExpose validate/getValues/resetFields -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
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
      <a-tab-pane key="tab-0" :tab="pi.t('common.page.form.tabs.basicinfo') + ' (1/2)'" force-render>
        <a-row :gutter="24">
      <a-col :span="12"><a-form-item :label="pi.label('ecSeizougijutsuId')"><a-input v-model:value="formState.ecSeizougijutsuId" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('tenantCode')"><a-input v-model:value="formState.tenantCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('companyCode')"><a-input v-model:value="formState.companyCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('cultureCode')"><a-input v-model:value="formState.cultureCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('plantCode')"><a-input v-model:value="formState.plantCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('lineNumber')"><a-input-number v-model:value="formState.lineNumber" class="w-full" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecCode')"><a-input v-model:value="formState.ecCode" disabled /></a-form-item></a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('discontinuedStatus')">
          <TaktSelect v-model:value="formState.discontinuedStatus" dict-type="logistics_materials_material_discontinued_status" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecModelCode')"><a-input v-model:value="formState.ecModelCode" disabled /></a-form-item></a-col>
      <a-col :span="24"><a-form-item :label="pi.label('ecRootMaterialCodeGroup')"><a-textarea v-model:value="formState.ecRootMaterialCodeGroup" :rows="2" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecRootMaterialDescription')"><a-input v-model:value="formState.ecRootMaterialDescription" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="isSopUpdated" :label="pi.label('isSopUpdated')"><TaktSelect v-model:value="formState.isSopUpdated" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="isImplemented" :label="pi.label('isImplemented')"><TaktSelect v-model:value="formState.isImplemented" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="sopDate" :label="pi.label('sopDate')"><a-date-picker v-model:value="formState.sopDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
      <a-col :span="24">
        <a-form-item name="execContent" :label="pi.label('execContent')">
          <a-textarea v-model:value="formState.execContent" :rows="3" />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item :label="pi.label('remark')">
          <a-textarea v-model:value="formState.remark" :rows="2" />
        </a-form-item>
      </a-col>
        </a-row>
      </a-tab-pane>
      <a-tab-pane key="tab-1" :tab="pi.t('common.page.form.tabs.basicinfo') + ' (2/2)'" force-render>
        <a-row :gutter="24">
      <a-col :span="12"><a-form-item :label="pi.label('deptCode')"><a-input v-model:value="formState.deptCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('deptName')"><a-input v-model:value="formState.deptName" disabled /></a-form-item></a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('ecScope')">
          <TaktSelect v-model:value="formState.ecScope" dict-type="logistics_manufacturing_ec_scope_category" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecDetailId')"><a-input v-model:value="formState.ecDetailId" disabled /></a-form-item></a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('isObsolete')">
          <TaktSelect v-model:value="formState.isObsolete" dict-type="sys_yes_no" disabled />
        </a-form-item>
      </a-col>
        </a-row>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
import type { EcSeizougijutsu, EcSeizougijutsuUpdate } from '@/types/logistics/manufacturing/engineering-change/ecSeizougijutsu.types'
import { useEcDeptViewI18n } from '../../composables/useEcDeptViewI18n'
import { useEcDeptViewFormRules } from '../../composables/useEcDeptViewFormRules'

const props = defineProps<{ formData?: EcSeizougijutsu | null; loading?: boolean }>();
const pi = useEcDeptViewI18n('ecseizougijutsu')
const { rules } = useEcDeptViewFormRules('ecseizougijutsu', pi.label)
const formRef = ref();
const activeTab = ref('tab-0');
const formState = reactive<{
  ecSeizougijutsuId?: string;
  tenantCode?: string;
  companyCode?: string;
  cultureCode?: string;
  plantCode?: string;
  ecCode?: string;
  lineNumber?: number;
  ecModelCode?: string;
  ecRootMaterialCodeGroup?: string;
  ecRootMaterialDescription?: string;
  discontinuedStatus?: string;
  isImplemented: number;
  execContent?: string;
  sopDate?: string;
  isSopUpdated: number;
  ecDetailId?: string;
  deptCode?: string;
  deptName?: string;
  ecScope?: number;
  isObsolete: number;
  remark?: string;
}>({ isImplemented: 1, isSopUpdated: 0, execContent: '', discontinuedStatus: 'Z0', isObsolete: 0 });

watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
  Object.assign(formState, {
    ecSeizougijutsuId: val.ecSeizougijutsuId,
    tenantCode: val.tenantCode,
    companyCode: val.companyCode,
    cultureCode: val.cultureCode,
    plantCode: val.plantCode,
    ecCode: val.ecCode,
    lineNumber: val.lineNumber,
    ecModelCode: val.ecModelCode,
    ecRootMaterialCodeGroup: val.ecRootMaterialCodeGroup,
    ecRootMaterialDescription: val.ecRootMaterialDescription,
    discontinuedStatus: val.discontinuedStatus ?? 'Z0',
    isImplemented: val.isImplemented ?? 1,
    execContent: val.execContent ?? '',
    sopDate: val.sopDate,
    isSopUpdated: val.isSopUpdated ?? 0,
    ecDetailId: val.ecDetailId,
    deptCode: val.deptCode,
    deptName: val.deptName,
    ecScope: val.ecScope,
    isObsolete: val.isObsolete ?? 0,
    remark: val.remark,
  });
}, { immediate: true });

async function validate() { await formRef.value?.validate(); }
function getValues(): EcSeizougijutsuUpdate {
  // 冗余字段（ecCode/deptCode/ecModelCode 等）界面只读但须随提交带回，否则后端 [Required]/Adapt 会当成空
  return { ...formState } as EcSeizougijutsuUpdate;
}
function resetFields() {
  Object.assign(formState, {
    ecSeizougijutsuId: '',
    tenantCode: '', companyCode: '', cultureCode: '', plantCode: '',
    ecCode: '', lineNumber: undefined, ecModelCode: '',
    ecRootMaterialCodeGroup: '', ecRootMaterialDescription: '',
    discontinuedStatus: 'Z0', isImplemented: 1, execContent: '',
    sopDate: undefined, isSopUpdated: 0,
    ecDetailId: '', deptCode: '', deptName: '',
    ecScope: undefined, isObsolete: 0, remark: undefined,
  });
}
defineExpose({ validate, getValues, resetFields });
</script>
