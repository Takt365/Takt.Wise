<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-mfg2/components -->
<!-- 文件名称：EcDeptViewForm.vue -->
<!-- 功能描述：设变制造二课表单；冗余字段 disabled；IsImplemented/ExecContent/课别填报可编辑；defineExpose validate/getValues/resetFields -->
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
      <a-tab-pane key="tab-0" :tab="pi.t('common.page.form.tabs.basicinfo') + ' (1/3)'" force-render>
        <a-row :gutter="24">
      <a-col :span="12"><a-form-item :label="pi.label('plantCode')"><a-input v-model:value="formState.plantCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecCode')"><a-input v-model:value="formState.ecCode" disabled /></a-form-item></a-col>
      <a-col :span="24"><a-form-item :label="pi.label('ecModelCodeGroup')"><a-textarea v-model:value="formState.ecModelCodeGroup" :rows="2" disabled /></a-form-item></a-col>
      <a-col :span="24"><a-form-item :label="pi.label('ecRootMaterialCodeGroup')"><a-textarea v-model:value="formState.ecRootMaterialCodeGroup" :rows="2" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('scheduledDate')"><a-date-picker v-model:value="formState.scheduledDate" value-format="YYYY-MM-DD" class="w-full" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('scheduledBatch')"><a-input v-model:value="formState.scheduledBatch" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="isImplemented" :label="pi.label('isImplemented')"><TaktSelect v-model:value="formState.isImplemented" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="12">
        <a-form-item name="productionTeam" :label="pi.label('productionTeam')">
          <TaktSelect
            v-model:value="productionTeamCodes"
            api-url="TaktProductionTeams/options"
            :api-params="productionTeamApiParams"
            multiple
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="12"><a-form-item name="productionDate" :label="pi.label('productionDate')"><a-date-picker v-model:value="formState.productionDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="productionBatch" :label="pi.label('productionBatch')"><a-input :value="formState.productionBatch" :maxlength="40" @update:value="(value) => { formState.productionBatch = taktDeptBatchInput(value, 40) }" /></a-form-item></a-col>
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
      <a-col :span="12"><a-form-item :label="pi.label('ecParentMaterialCode')"><a-input v-model:value="formState.ecParentMaterialCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecParentMaterialDescription')"><a-input v-model:value="formState.ecParentMaterialDescription" disabled /></a-form-item></a-col>
      <a-col :span="24"><a-form-item :label="pi.label('ecOldMaterialCodeGroup')"><a-textarea v-model:value="formState.ecOldMaterialCodeGroup" :rows="2" disabled /></a-form-item></a-col>
      <a-col :span="24"><a-form-item :label="pi.label('ecNewMaterialCodeGroup')"><a-textarea v-model:value="formState.ecNewMaterialCodeGroup" :rows="2" disabled /></a-form-item></a-col>
        </a-row>
      </a-tab-pane>
      <a-tab-pane key="tab-2" :tab="pi.t('common.page.form.tabs.basicinfo') + ' (3/3)'" force-render>
        <a-row :gutter="24">
      <a-col :span="12"><a-form-item :label="pi.label('ecMfg2Id')"><a-input v-model:value="formState.ecMfg2Id" disabled /></a-form-item></a-col>
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
import type { EcMfg2, EcMfg2Update } from '@/types/logistics/manufacturing/engineering-change/ecMfg2.types'
import {
  ecDeptImplementedFormValue,
  taktDeptBatchInput,
  taktEcBatchFromScheduled,
  taktEcDateFromScheduled,
  taktEcDefaultExecContent,
  taktEcIsAutoExecContent,
} from '@/constants/logistics/ecDeptExecLineFields.constants'
import {
  EC_MFG2_DEFAULT_PRODUCTION_TEAM_CODES,
  buildEcMfg2ProductionTeamOptionsParams,
  taktJoinEcProductionTeamCodes,
  taktResolveEcProductionTeamCodes,
} from '../../composables/ecDeptProductionTeamMulti'
import { useEcDeptViewI18n } from '../../composables/useEcDeptViewI18n'
import { useEcDeptViewFormRules } from '../../composables/useEcDeptViewFormRules'

const props = defineProps<{ formData?: EcMfg2 | null; loading?: boolean }>();
const pi = useEcDeptViewI18n('ecmfg2')
const { rules } = useEcDeptViewFormRules('ecmfg2', pi.label)
const formRef = ref();
const activeTab = ref('tab-0');
const formState = reactive<{
  ecMfg2Id?: string;
  tenantCode?: string;
  companyCode?: string;
  cultureCode?: string;
  plantCode?: string;
  ecCode?: string;
  lineNumber?: number;
  ecModelCodeGroup?: string;
  ecRootMaterialCodeGroup?: string;
  ecParentMaterialCode?: string;
  ecParentMaterialDescription?: string;
  ecOldMaterialCodeGroup?: string;
  ecNewMaterialCodeGroup?: string;
  discontinuedStatus?: string;
  isImplemented: number;
  scheduledDate?: string;
  scheduledBatch?: string;
  execContent?: string;
  productionTeam?: string;
  productionDate?: string;
  productionBatch?: string;
  ecGrainKey?: string;
  ecDetailId?: string;
  deptCode?: string;
  deptName?: string;
  ecImplScope?: number;
  ecControlScope?: number;
  isObsolete: number;
  remark?: string;
}>({ isImplemented: 1, execContent: '', discontinuedStatus: 'Z0', isObsolete: 0 });

/** 生产班组多选（UI）；提交时拼回 formState.productionTeam */
const productionTeamCodes = ref<string[]>([])
/** 班组 options：仅 PCBA 分类 P + 工厂 */
const productionTeamApiParams = computed(() => buildEcMfg2ProductionTeamOptionsParams(formState.plantCode))

watch(productionTeamCodes, (codes) => {
  formState.productionTeam = taktJoinEcProductionTeamCodes(codes) || undefined
})

watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
  // 预定非空 → 生产日期/生产批次=预定；预定空 → 清空；执行内容默认「从{生产批次}开始」
  const productionBatch = taktEcBatchFromScheduled(val.scheduledBatch)
  const productionDate = taktEcDateFromScheduled(val.scheduledDate)
  const productionTeamCodesResolved = taktResolveEcProductionTeamCodes(
    val.productionTeam,
    EC_MFG2_DEFAULT_PRODUCTION_TEAM_CODES,
  )
  Object.assign(formState, {
    ecMfg2Id: val.ecMfg2Id,
    tenantCode: val.tenantCode,
    companyCode: val.companyCode,
    cultureCode: val.cultureCode,
    plantCode: val.plantCode,
    ecCode: val.ecCode,
    lineNumber: val.lineNumber,
    ecModelCodeGroup: val.ecModelCodeGroup,
    ecRootMaterialCodeGroup: val.ecRootMaterialCodeGroup,
    ecParentMaterialCode: val.ecParentMaterialCode,
    ecParentMaterialDescription: val.ecParentMaterialDescription,
    ecOldMaterialCodeGroup: val.ecOldMaterialCodeGroup,
    ecNewMaterialCodeGroup: val.ecNewMaterialCodeGroup,
    discontinuedStatus: val.discontinuedStatus ?? 'Z0',
    isImplemented: ecDeptImplementedFormValue(val.isImplemented),
    scheduledDate: val.scheduledDate,
    scheduledBatch: val.scheduledBatch,
    productionTeam: taktJoinEcProductionTeamCodes(productionTeamCodesResolved) || undefined,
    productionDate,
    productionBatch,
    execContent: taktEcIsAutoExecContent(val.execContent)
      ? taktEcDefaultExecContent(val.ecCode, val.ecRootMaterialCodeGroup, productionBatch)
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
  productionTeamCodes.value = productionTeamCodesResolved
}, { immediate: true });

/** 手改生产批次时，若执行内容仍是自动文案则联动 */
watch(
  () => formState.productionBatch,
  (batch) => {
    if (taktEcIsAutoExecContent(formState.execContent)) {
      formState.execContent = taktEcDefaultExecContent(formState.ecCode, formState.ecRootMaterialCodeGroup, batch)
    }
  },
)

async function validate() { await formRef.value?.validate(); }
function getValues(): EcMfg2Update {
  // 冗余字段（ecCode/deptCode/ecModelCodeGroup 等）界面只读但须随提交带回，否则后端 [Required]/Adapt 会当成空
  formState.productionTeam = taktJoinEcProductionTeamCodes(productionTeamCodes.value) || undefined
  return { ...formState } as EcMfg2Update;
}
function resetFields() {
  productionTeamCodes.value = []
  Object.assign(formState, {
    ecMfg2Id: '',
    tenantCode: '', companyCode: '', cultureCode: '', plantCode: '',
    ecCode: '', lineNumber: undefined, ecModelCodeGroup: '',
    ecRootMaterialCodeGroup: '',
    ecParentMaterialCode: '', ecParentMaterialDescription: '',
    ecOldMaterialCodeGroup: '', ecNewMaterialCodeGroup: '',
    discontinuedStatus: 'Z0', isImplemented: 1, execContent: '',
    scheduledDate: undefined, scheduledBatch: undefined,
    productionTeam: undefined, productionDate: undefined, productionBatch: undefined,
    ecGrainKey: '',
    ecDetailId: '', deptCode: '', deptName: '',
    ecImplScope: undefined, ecControlScope: undefined, isObsolete: 0, remark: undefined,
  });
}
defineExpose({ validate, getValues, resetFields });
</script>
