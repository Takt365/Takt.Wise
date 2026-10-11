<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-koubai/components -->
<!-- 文件名称：EcDeptViewForm.vue -->
<!-- 功能描述：设变采购部门表单；冗余字段 disabled；IsImplemented/ExecContent/课别填报可编辑；defineExpose validate/getValues/resetFields -->
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
      <a-col :span="12"><a-form-item :label="pi.label('ecKoubaiId')"><a-input v-model:value="formState.ecKoubaiId" disabled /></a-form-item></a-col>
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
      <a-col :span="12"><a-form-item :label="pi.label('ecNewMaterialCode')"><a-input v-model:value="formState.ecNewMaterialCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecNewMaterialDescription')"><a-input v-model:value="formState.ecNewMaterialDescription" disabled /></a-form-item></a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('ecNewWarehouse')">
          <TaktSelect v-model:value="formState.ecNewWarehouse" api-url="TaktWarehouses/options" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('ecNewPurchaseType')">
          <TaktSelect v-model:value="formState.ecNewPurchaseType" dict-type="logistics_procurement_type" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="12"><a-form-item name="isImplemented" :label="pi.label('isImplemented')"><TaktSelect v-model:value="formState.isImplemented" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="purchaseOrderIssueDate" :label="pi.label('purchaseOrderIssueDate')"><a-date-picker v-model:value="formState.purchaseOrderIssueDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="supplier" :label="pi.label('supplier')"><a-input v-model:value="formState.supplier" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="purchaseOrderCode" :label="pi.label('purchaseOrderCode')"><a-input v-model:value="formState.purchaseOrderCode" /></a-form-item></a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('ecOldPartDisposition')">
          <TaktDictTag
            v-if="formState.ecOldPartDisposition != null && String(formState.ecOldPartDisposition).trim() !== ''"
            dict-type="logistics_manufacturing_ec_old_part_disposition"
            :value="formState.ecOldPartDisposition"
          />
          <span v-else class="text-text-secondary">—</span>
        </a-form-item>
      </a-col>
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
import type { EcKoubai, EcKoubaiUpdate } from '@/types/logistics/manufacturing/engineering-change/ecKoubai.types'
import { useEcDeptViewI18n } from '../../composables/useEcDeptViewI18n'
import { useEcDeptViewFormRules } from '../../composables/useEcDeptViewFormRules'

const props = defineProps<{ formData?: EcKoubai | null; loading?: boolean }>();
const pi = useEcDeptViewI18n('eckoubai')
const { rules } = useEcDeptViewFormRules('eckoubai', pi.label)
const formRef = ref();
const activeTab = ref('tab-0');
const formState = reactive<{
  ecKoubaiId?: string;
  tenantCode?: string;
  companyCode?: string;
  cultureCode?: string;
  plantCode?: string;
  ecCode?: string;
  lineNumber?: number;
  discontinuedStatus?: string;
  ecModelCodeGroup?: string;
  ecRootMaterialCodeGroup?: string;
  ecNewMaterialCode?: string;
  ecNewMaterialDescription?: string;
  ecNewWarehouse?: string;
  ecNewPurchaseType?: string;
  isImplemented: number;
  execContent?: string;
  purchaseOrderIssueDate?: string;
  supplier?: string;
  purchaseOrderCode?: string;
  ecOldPartDisposition?: string;
  ecDetailId?: string;
  deptCode?: string;
  deptName?: string;
  ecScope?: number;
  isObsolete: number;
  remark?: string;
}>({ isImplemented: 1, execContent: '', discontinuedStatus: 'Z0', isObsolete: 0 });

watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
  Object.assign(formState, {
    ecKoubaiId: val.ecKoubaiId,
    tenantCode: val.tenantCode,
    companyCode: val.companyCode,
    cultureCode: val.cultureCode,
    plantCode: val.plantCode,
    ecCode: val.ecCode,
    lineNumber: val.lineNumber,
    discontinuedStatus: val.discontinuedStatus ?? 'Z0',
    ecModelCodeGroup: val.ecModelCodeGroup,
    ecRootMaterialCodeGroup: val.ecRootMaterialCodeGroup,
    ecNewMaterialCode: val.ecNewMaterialCode,
    ecNewMaterialDescription: val.ecNewMaterialDescription,
    ecNewWarehouse: val.ecNewWarehouse,
    ecNewPurchaseType: val.ecNewPurchaseType,
    isImplemented: val.isImplemented ?? 1,
    execContent: val.execContent ?? '',
    purchaseOrderIssueDate: val.purchaseOrderIssueDate,
    supplier: val.supplier,
    purchaseOrderCode: val.purchaseOrderCode,
    ecOldPartDisposition: val.ecOldPartDisposition != null && String(val.ecOldPartDisposition).trim() !== ''
      ? String(val.ecOldPartDisposition).trim()
      : undefined,
    ecDetailId: val.ecDetailId,
    deptCode: val.deptCode,
    deptName: val.deptName,
    ecScope: val.ecScope,
    isObsolete: val.isObsolete ?? 0,
    remark: val.remark,
  });
}, { immediate: true });

async function validate() { await formRef.value?.validate(); }
function getValues(): EcKoubaiUpdate {
  // 冗余字段（ecCode/deptCode 等）界面只读但须随提交带回，否则后端 [Required]/Adapt 会当成空
  return { ...formState } as EcKoubaiUpdate;
}
function resetFields() {
  Object.assign(formState, {
    ecKoubaiId: '',
    tenantCode: '', companyCode: '', cultureCode: '', plantCode: '',
    ecCode: '', lineNumber: undefined, discontinuedStatus: 'Z0',
    ecModelCodeGroup: '', ecRootMaterialCodeGroup: '',
    ecNewMaterialCode: '', ecNewMaterialDescription: '',
    ecNewWarehouse: undefined, ecNewPurchaseType: undefined, isImplemented: 1, execContent: '',
    purchaseOrderIssueDate: undefined, supplier: undefined, purchaseOrderCode: undefined,
    ecOldPartDisposition: undefined, ecDetailId: '', deptCode: '', deptName: '',
    ecScope: undefined, isObsolete: 0, remark: undefined,
  });
}
defineExpose({ validate, getValues, resetFields });
</script>
