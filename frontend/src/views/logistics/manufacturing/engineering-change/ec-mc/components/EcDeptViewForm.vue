<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-mc/components -->
<!-- 文件名称：EcDeptViewForm.vue -->
<!-- 功能描述：设变部管部门表单；冗余字段 disabled；IsImplemented/ExecContent/课别填报可编辑；defineExpose validate/getValues/resetFields -->
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
    <TaktFormTabs v-model:active-key="activeTab">
      <a-tab-pane key="tab-0" :tab="pi.t('common.page.form.tabs.basicinfo') + ' (1/3)'" force-render>
        <a-row :gutter="24">
      <a-col :span="12"><a-form-item :label="pi.label('plantCode')"><a-input v-model:value="formState.plantCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecCode')"><a-input v-model:value="formState.ecCode" disabled /></a-form-item></a-col>
      <a-col :span="24"><a-form-item :label="pi.label('ecModelCodeGroup')"><a-textarea v-model:value="formState.ecModelCodeGroup" :rows="2" disabled /></a-form-item></a-col>
      <a-col :span="24"><a-form-item :label="pi.label('ecRootMaterialCodeGroup')"><a-textarea v-model:value="formState.ecRootMaterialCodeGroup" :rows="2" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('scheduledDate')"><a-date-picker v-model:value="formState.scheduledDate" value-format="YYYY-MM-DD" class="w-full" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('scheduledBatch')"><a-input v-model:value="formState.scheduledBatch" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="isImplemented" :label="pi.label('isImplemented')"><TaktSelect v-model:value="formState.isImplemented" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="outboundDate" :label="pi.label('outboundDate')"><a-date-picker v-model:value="formState.outboundDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="outboundBatch" :label="pi.label('outboundBatch')"><a-input :value="formState.outboundBatch" :maxlength="40" @update:value="(value) => { formState.outboundBatch = taktDeptBatchInput(value, 40) }" /></a-form-item></a-col>
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
      <a-tab-pane key="tab-1" :tab="pi.t('common.page.form.tabs.basicinfo') + ' (2/3)'" force-render>
        <a-row :gutter="24">
      <a-col :span="24"><a-form-item :label="pi.label('ecNewMaterialCodeGroup')"><a-textarea v-model:value="formState.ecNewMaterialCodeGroup" :rows="2" disabled /></a-form-item></a-col>
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
        </a-row>
      </a-tab-pane>
      <a-tab-pane key="tab-2" :tab="pi.t('common.page.form.tabs.basicinfo') + ' (3/3)'" force-render>
        <a-row :gutter="24">
      <a-col :span="12"><a-form-item :label="pi.label('ecMcId')"><a-input v-model:value="formState.ecMcId" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('tenantCode')"><a-input v-model:value="formState.tenantCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('companyCode')"><a-input v-model:value="formState.companyCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('cultureCode')"><a-input v-model:value="formState.cultureCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('lineNumber')"><a-input-number v-model:value="formState.lineNumber" class="w-full" disabled /></a-form-item></a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('discontinuedStatus')">
          <TaktSelect v-model:value="formState.discontinuedStatus" dict-type="logistics_materials_material_discontinued_status" disabled />
        </a-form-item>
      </a-col>
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
import type { EcMc, EcMcUpdate } from '@/types/logistics/manufacturing/engineering-change/ecMc.types'
import {
  ecDeptImplementedFormValue,
  taktDeptBatchInput,
  taktEcBatchFromScheduled,
  taktEcDateFromScheduled,
  taktEcIsAutoExecContent,
  taktEcMaterialScopeExecContent,
} from '@/constants/logistics/ecDeptExecLineFields.constants'
import { useEcDeptViewI18n } from '../../composables/useEcDeptViewI18n'
import { useEcDeptViewFormRules } from '../../composables/useEcDeptViewFormRules'

const props = defineProps<{ formData?: EcMc | null; loading?: boolean }>();
const pi = useEcDeptViewI18n('ecmc')
const { rules } = useEcDeptViewFormRules('ecmc', pi.label)
const formRef = ref();
const activeTab = ref('tab-0');
const formState = reactive<{
  ecMcId?: string;
  tenantCode?: string;
  companyCode?: string;
  cultureCode?: string;
  plantCode?: string;
  ecCode?: string;
  lineNumber?: number;
  ecModelCodeGroup?: string;
  ecRootMaterialCodeGroup?: string;
  discontinuedStatus?: string;
  ecNewMaterialCodeGroup?: string;
    ecNewPurchaseType?: string;
  ecNewWarehouse?: string;
  scheduledDate?: string;
  scheduledBatch?: string;
  isImplemented: number;
  outboundDate?: string;
  outboundBatch?: string;
  execContent?: string;
  ecGrainKey?: string;
  ecDetailId?: string;
  deptCode?: string;
  deptName?: string;
  ecImplScope?: number;
  ecControlScope?: number;
  isObsolete: number;
  remark?: string;
}>({ isImplemented: 1, execContent: '', discontinuedStatus: 'Z0', isObsolete: 0 });


watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
  // 预定非空 → 出库=预定；预定空 → 出库空
  // 执行内容：{设变号}-{根物料组}-{新物料}-{新品仓库}，从{出库批次}开始实施。
  const outboundBatch = taktEcBatchFromScheduled(val.scheduledBatch)
  const outboundDate = taktEcDateFromScheduled(val.scheduledDate)
  Object.assign(formState, {
    ecMcId: val.ecMcId,
    tenantCode: val.tenantCode,
    companyCode: val.companyCode,
    cultureCode: val.cultureCode,
    plantCode: val.plantCode,
    ecCode: val.ecCode,
    lineNumber: val.lineNumber,
    ecModelCodeGroup: val.ecModelCodeGroup,
    ecRootMaterialCodeGroup: val.ecRootMaterialCodeGroup,
    discontinuedStatus: val.discontinuedStatus ?? 'Z0',
    ecNewMaterialCodeGroup: val.ecNewMaterialCodeGroup,
        ecNewPurchaseType: val.ecNewPurchaseType,
    ecNewWarehouse: val.ecNewWarehouse,
    isImplemented: ecDeptImplementedFormValue(val.isImplemented),
    scheduledDate: val.scheduledDate,
    scheduledBatch: val.scheduledBatch,
    outboundDate,
    outboundBatch,
    execContent: taktEcIsAutoExecContent(val.execContent)
      ? taktEcMaterialScopeExecContent({
          ecCode: val.ecCode,
          rootMaterialGroup: val.ecRootMaterialCodeGroup,
          newMaterialCode: val.ecNewMaterialCodeGroup,
          scopeValue: val.ecNewWarehouse,
          fromValue: outboundBatch,
        })
      : (val.execContent ?? ''),
    ecGrainKey: (val as { ecGrainKey?: string }).ecGrainKey ?? '',
    ecDetailId: val.ecDetailId,
    deptCode: val.deptCode,
    deptName: val.deptName,
    ecImplScope: val.ecImplScope,
    ecControlScope: val.ecControlScope,
    isObsolete: val.isObsolete ?? 0,
    remark: val.remark,
  });
}, { immediate: true });

/** 手改出库批次时，若执行内容仍是自动文案则联动 */
watch(
  () => formState.outboundBatch,
  (batch) => {
    if (taktEcIsAutoExecContent(formState.execContent)) {
      formState.execContent = taktEcMaterialScopeExecContent({
        ecCode: formState.ecCode,
        rootMaterialGroup: formState.ecRootMaterialCodeGroup,
        newMaterialCode: formState.ecNewMaterialCodeGroup,
        scopeValue: formState.ecNewWarehouse,
        fromValue: batch,
      })
    }
  },
)

async function validate() { await formRef.value?.validate(); }
function getValues(): EcMcUpdate {
  // 冗余字段（ecCode/deptCode/ecModelCodeGroup 等）界面只读但须随提交带回，否则后端 [Required]/Adapt 会当成空
  return { ...formState } as EcMcUpdate;
}
function resetFields() {
  Object.assign(formState, {
    ecMcId: '',
    tenantCode: '', companyCode: '', cultureCode: '', plantCode: '',
    ecCode: '', lineNumber: undefined, ecModelCodeGroup: '', ecRootMaterialCodeGroup: '',
    discontinuedStatus: 'Z0',
    ecNewMaterialCodeGroup: '',
    ecNewPurchaseType: undefined, ecNewWarehouse: undefined,
    isImplemented: 1, execContent: '',
    scheduledDate: undefined, scheduledBatch: undefined,
    outboundDate: undefined, outboundBatch: undefined,
    ecGrainKey: '', ecDetailId: '', deptCode: '', deptName: '',
    ecImplScope: undefined, ecControlScope: undefined, isObsolete: 0, remark: undefined,
  });
}
defineExpose({ validate, getValues, resetFields });
</script>
