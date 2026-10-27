<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-mtl/components -->
<!-- 文件名称：EcDeptViewForm.vue -->
<!-- 功能描述：设变 E-MTL 表单；冗余字段 disabled；IsImplemented/ExecContent/课别填报可编辑；defineExpose validate/getValues/resetFields -->
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
    <TaktFormTabs v-model:active-key="activeTab">
      <a-tab-pane key="tab-0" :tab="pi.t('common.page.form.tabs.basicinfo') + ' (1/2)'" force-render>
        <a-row :gutter="24">
      <a-col :span="12"><a-form-item :label="pi.label('ecMtlId')"><a-input v-model:value="formState.ecMtlId" disabled /></a-form-item></a-col>
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
      <a-col :span="24"><a-form-item :label="pi.label('ecModelCodeGroup')"><a-textarea v-model:value="formState.ecModelCodeGroup" :rows="2" disabled /></a-form-item></a-col>
      <a-col :span="24"><a-form-item :label="pi.label('ecRootMaterialCodeGroup')"><a-textarea v-model:value="formState.ecRootMaterialCodeGroup" :rows="2" disabled /></a-form-item></a-col>
      <a-col v-if="showRootMaterialDescription" :span="12"><a-form-item :label="pi.label('ecRootMaterialDescription')"><a-input v-model:value="formState.ecRootMaterialDescription" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecParentMaterialCode')"><a-input v-model:value="formState.ecParentMaterialCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecParentMaterialDescription')"><a-input v-model:value="formState.ecParentMaterialDescription" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecNewMaterialCode')"><a-input v-model:value="formState.ecNewMaterialCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecNewMaterialDescription')"><a-input v-model:value="formState.ecNewMaterialDescription" disabled /></a-form-item></a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('ecNewPurchaseType')">
          <TaktSelect v-model:value="formState.ecNewPurchaseType" dict-type="logistics_procurement_type" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('ecNewWarehouse')">
          <TaktSelect v-model:value="formState.ecNewWarehouse" api-url="TaktWarehouses/options" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="12"><a-form-item name="isImplemented" :label="pi.label('isImplemented')"><TaktSelect v-model:value="formState.isImplemented" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('scheduledDate')"><a-date-picker v-model:value="formState.scheduledDate" value-format="YYYY-MM-DD" class="w-full" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('scheduledBatch')"><a-input v-model:value="formState.scheduledBatch" disabled /></a-form-item></a-col>
      <a-col :span="24">
        <a-form-item name="execContent" :label="pi.label('execContent')">
          <a-textarea v-model:value="formState.execContent" :rows="3" />
        </a-form-item>
      </a-col>
      <a-col :span="12"><a-form-item name="outboundBatch" :label="pi.label('outboundBatch')"><a-input :value="formState.outboundBatch" :maxlength="20" @update:value="(value) => { formState.outboundBatch = taktDeptBatchInput(value) }" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="outboundDate" :label="pi.label('outboundDate')"><a-date-picker v-model:value="formState.outboundDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
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
        <a-form-item :label="pi.label('ecImplScope')">
          <TaktSelect v-model:value="formState.ecImplScope" dict-type="logistics_manufacturing_ec_impl_scope_category" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('ecControlScope')">
          <TaktSelect v-model:value="formState.ecControlScope" dict-type="logistics_manufacturing_ec_control_scope_category" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('isObsolete')">
          <TaktSelect v-model:value="formState.isObsolete" dict-type="sys_yes_no" disabled />
        </a-form-item>
      </a-col>
        </a-row>
      </a-tab-pane>
    </TaktFormTabs>
  </a-form>
</template>

<script setup lang="ts">
import type { EcMtl, EcMtlUpdate } from '@/types/logistics/manufacturing/engineering-change/ecMtl.types'
import { ecDeptImplementedFormValue, taktDeptBatchInput } from '@/constants/logistics/ecDeptExecLineFields.constants'
import { taktShowEcRootMaterialDescription } from '@/constants/logistics/ecScope.constants'
import { useEcDeptViewI18n } from '../../composables/useEcDeptViewI18n'
import { useEcDeptViewFormRules } from '../../composables/useEcDeptViewFormRules'

const props = defineProps<{ formData?: EcMtl | null; loading?: boolean }>();
const pi = useEcDeptViewI18n('ecmtl')
const { rules } = useEcDeptViewFormRules('ecmtl', pi.label)
const formRef = ref();
const activeTab = ref('tab-0');
const formState = reactive<{
  ecMtlId?: string;
  tenantCode?: string;
  companyCode?: string;
  cultureCode?: string;
  plantCode?: string;
  ecCode?: string;
  lineNumber?: number;
  ecModelCodeGroup?: string;
  ecRootMaterialCodeGroup?: string;
  ecRootMaterialDescription?: string;
  ecParentMaterialCode?: string;
  ecParentMaterialDescription?: string;
  discontinuedStatus?: string;
  ecNewMaterialCode?: string;
  ecNewMaterialDescription?: string;
  ecNewPurchaseType?: string;
  ecNewWarehouse?: string;
  isImplemented: number;
  scheduledDate?: string;
  scheduledBatch?: string;
  execContent?: string;
  outboundDate?: string;
  outboundBatch?: string;
  ecDetailId?: string;
  deptCode?: string;
  deptName?: string;
  ecImplScope?: number;
  ecControlScope?: number;
  isObsolete: number;
  remark?: string;
}>({ isImplemented: 1, execContent: '', discontinuedStatus: 'Z0', isObsolete: 0 });

/** 仅全仕向且按成品时显示根物料描述 */
const showRootMaterialDescription = computed(() => taktShowEcRootMaterialDescription(formState.ecImplScope, formState.ecControlScope))

watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
  Object.assign(formState, {
    ecMtlId: val.ecMtlId,
    tenantCode: val.tenantCode,
    companyCode: val.companyCode,
    cultureCode: val.cultureCode,
    plantCode: val.plantCode,
    ecCode: val.ecCode,
    lineNumber: val.lineNumber,
    ecModelCodeGroup: val.ecModelCodeGroup,
    ecRootMaterialCodeGroup: val.ecRootMaterialCodeGroup,
    ecRootMaterialDescription: val.ecRootMaterialDescription,
    ecParentMaterialCode: val.ecParentMaterialCode,
    ecParentMaterialDescription: val.ecParentMaterialDescription,
    discontinuedStatus: val.discontinuedStatus ?? 'Z0',
    ecNewMaterialCode: val.ecNewMaterialCode,
    ecNewMaterialDescription: val.ecNewMaterialDescription,
    ecNewPurchaseType: val.ecNewPurchaseType,
    ecNewWarehouse: val.ecNewWarehouse,
    isImplemented: ecDeptImplementedFormValue(val.isImplemented),
    scheduledDate: val.scheduledDate,
    scheduledBatch: val.scheduledBatch,
    execContent: val.execContent ?? '',
    outboundDate: val.outboundDate,
    outboundBatch: taktDeptBatchInput(val.outboundBatch),
    ecDetailId: val.ecDetailId,
    deptCode: val.deptCode,
    deptName: val.deptName,
    ecImplScope: val.ecImplScope,
    ecControlScope: val.ecControlScope,
    isObsolete: val.isObsolete ?? 0,
    remark: val.remark,
  });
}, { immediate: true });

async function validate() { await formRef.value?.validate(); }
function getValues(): EcMtlUpdate {
  // 冗余字段（ecCode/deptCode/ecModelCodeGroup 等）界面只读但须随提交带回，否则后端 [Required]/Adapt 会当成空
  return { ...formState } as EcMtlUpdate;
}
function resetFields() {
  Object.assign(formState, {
    ecMtlId: '',
    tenantCode: '', companyCode: '', cultureCode: '', plantCode: '',
    ecCode: '', lineNumber: undefined, ecModelCodeGroup: '',
    ecRootMaterialCodeGroup: '',
    ecRootMaterialDescription: '',
    ecParentMaterialCode: '', ecParentMaterialDescription: '',
    discontinuedStatus: 'Z0',
    ecNewMaterialCode: '', ecNewMaterialDescription: '',
    ecNewPurchaseType: undefined, ecNewWarehouse: '',
    isImplemented: 1, execContent: '',
    scheduledDate: undefined, scheduledBatch: undefined,
    outboundDate: undefined, outboundBatch: undefined,
    ecDetailId: '', deptCode: '', deptName: '',
    ecImplScope: undefined, ecControlScope: undefined, isObsolete: 0, remark: undefined,
  });
}
defineExpose({ validate, getValues, resetFields });
</script>
