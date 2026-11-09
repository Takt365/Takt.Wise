<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-qc/components -->
<!-- 文件名称：EcDeptViewForm.vue -->
<!-- 功能描述：设变品管部门表单；ProductionTeam/ProductionDate/ProductionBatch 冗余自制一只读；InspectionDate/InspectionBatch/IsImplemented 可编辑；ExecContent 按「设变-成品，检验批次已实施，判定：PASS。」自动填写并可改；defineExpose validate/getValues/resetFields -->
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
      <a-col :span="12">
        <a-form-item :label="pi.label('productionTeam')">
          <TaktSelect
            v-model:value="productionTeamCodes"
            api-url="TaktProductionTeams/options"
            :api-params="productionTeamApiParams"
            multiple
            disabled
          />
        </a-form-item>
      </a-col>
      <a-col :span="12"><a-form-item :label="pi.label('productionDate')"><a-date-picker v-model:value="formState.productionDate" value-format="YYYY-MM-DD" class="w-full" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('productionBatch')"><a-input v-model:value="formState.productionBatch" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="isImplemented" :label="pi.label('isImplemented')"><TaktSelect v-model:value="formState.isImplemented" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="inspectionDate" :label="pi.label('inspectionDate')"><a-date-picker v-model:value="formState.inspectionDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="inspectionBatch" :label="pi.label('inspectionBatch')"><a-input :value="formState.inspectionBatch" :maxlength="20" @update:value="(value) => { formState.inspectionBatch = taktDeptBatchInput(value) }" /></a-form-item></a-col>
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
      <a-col :span="24"><a-form-item :label="pi.label('ecOldMaterialCodeGroup')"><a-textarea v-model:value="formState.ecOldMaterialCodeGroup" :rows="2" disabled /></a-form-item></a-col>
      <a-col :span="24"><a-form-item :label="pi.label('ecNewMaterialCodeGroup')"><a-textarea v-model:value="formState.ecNewMaterialCodeGroup" :rows="2" disabled /></a-form-item></a-col>
        </a-row>
      </a-tab-pane>
      <a-tab-pane key="tab-2" :tab="pi.t('common.page.form.tabs.basicinfo') + ' (3/3)'" force-render>
        <a-row :gutter="24">
      <a-col :span="12"><a-form-item :label="pi.label('ecQcId')"><a-input v-model:value="formState.ecQcId" disabled /></a-form-item></a-col>
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
import type { EcQc, EcQcUpdate } from '@/types/logistics/manufacturing/engineering-change/ecQc.types'
import {
  ecDeptImplementedFormValue,
  taktDeptBatchInput,
  taktEcIsAutoExecContent,
  taktEcQcDefaultExecContent,
} from '@/constants/logistics/ecDeptExecLineFields.constants'
import {
  buildEcMfg1ProductionTeamOptionsParams,
  taktSplitEcProductionTeamCodes,
} from '../../composables/ecDeptProductionTeamMulti'
import { useEcDeptViewI18n } from '../../composables/useEcDeptViewI18n'
import { useEcDeptViewFormRules } from '../../composables/useEcDeptViewFormRules'

const props = defineProps<{ formData?: EcQc | null; loading?: boolean }>();
const pi = useEcDeptViewI18n('ecqc')
const { rules } = useEcDeptViewFormRules('ecqc', pi.label)
const formRef = ref();
const activeTab = ref('tab-0');
const formState = reactive<{
  ecQcId?: string;
  tenantCode?: string;
  companyCode?: string;
  cultureCode?: string;
  plantCode?: string;
  ecCode?: string;
  lineNumber?: number;
  ecModelCodeGroup?: string;
  ecRootMaterialCodeGroup?: string;
  ecOldMaterialCodeGroup?: string;
  ecNewMaterialCodeGroup?: string;
  discontinuedStatus?: string;
  productionTeam?: string;
  productionDate?: string;
  productionBatch?: string;
  isImplemented: number;
  inspectionDate?: string;
  inspectionBatch?: string;
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

/** 生产班组只读展示（冗余自制一；分号串 → 多选数组） */
const productionTeamCodes = ref<string[]>([])
/** 班组 options：与制一同源（组立 A + 工厂；只读展示） */
const productionTeamApiParams = computed(() => buildEcMfg1ProductionTeamOptionsParams(formState.plantCode))

watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
  const productionBatch = taktDeptBatchInput(val.productionBatch, 40)
  /** 检验批次空则回填生产批次，便于生成「设变-成品，批次已实施，判定：PASS。」 */
  const inspectionBatch = taktDeptBatchInput(val.inspectionBatch) || productionBatch
  Object.assign(formState, {
    ecQcId: val.ecQcId,
    tenantCode: val.tenantCode,
    companyCode: val.companyCode,
    cultureCode: val.cultureCode,
    plantCode: val.plantCode,
    ecCode: val.ecCode,
    lineNumber: val.lineNumber,
    ecModelCodeGroup: val.ecModelCodeGroup,
    ecRootMaterialCodeGroup: val.ecRootMaterialCodeGroup,
    ecOldMaterialCodeGroup: val.ecOldMaterialCodeGroup,
    ecNewMaterialCodeGroup: val.ecNewMaterialCodeGroup,
    discontinuedStatus: val.discontinuedStatus ?? 'Z0',
    isImplemented: ecDeptImplementedFormValue(val.isImplemented),
    productionTeam: val.productionTeam,
    productionDate: val.productionDate,
    productionBatch,
    inspectionDate: val.inspectionDate,
    inspectionBatch,
    execContent: taktEcIsAutoExecContent(val.execContent)
      ? taktEcQcDefaultExecContent(val.ecCode, val.ecRootMaterialCodeGroup, inspectionBatch)
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
  productionTeamCodes.value = taktSplitEcProductionTeamCodes(val.productionTeam)
}, { immediate: true });

/** 手改检验批次时，若执行内容仍是自动文案则联动为「设变-成品，批次已实施，判定：PASS。」 */
watch(
  () => formState.inspectionBatch,
  (batch) => {
    if (taktEcIsAutoExecContent(formState.execContent)) {
      formState.execContent = taktEcQcDefaultExecContent(
        formState.ecCode,
        formState.ecRootMaterialCodeGroup,
        batch,
      )
    }
  },
)

async function validate() { await formRef.value?.validate(); }
function getValues(): EcQcUpdate {
  // 冗余字段（ecCode/deptCode/ecModelCodeGroup/productionTeam/productionBatch 等）界面只读但须随提交带回
  return { ...formState } as EcQcUpdate;
}
function resetFields() {
  productionTeamCodes.value = []
  Object.assign(formState, {
    ecQcId: '',
    tenantCode: '', companyCode: '', cultureCode: '', plantCode: '',
    ecCode: '', lineNumber: undefined, ecModelCodeGroup: '',
    ecRootMaterialCodeGroup: '',
    ecOldMaterialCodeGroup: '', ecNewMaterialCodeGroup: '',
    discontinuedStatus: 'Z0', isImplemented: 1, execContent: '',
    productionTeam: undefined, productionDate: undefined, productionBatch: undefined,
    inspectionDate: undefined, inspectionBatch: undefined,
    ecGrainKey: '', ecDetailId: '', deptCode: '', deptName: '',
    ecImplScope: undefined, ecControlScope: undefined, isObsolete: 0, remark: undefined,
  });
}
defineExpose({ validate, getValues, resetFields });
</script>
