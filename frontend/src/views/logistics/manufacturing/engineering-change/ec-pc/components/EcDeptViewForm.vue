<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-pc/components -->
<!-- 文件名称：EcDeptViewForm.vue -->
<!-- 功能描述：设变生管部门表单；冗余字段 disabled；IsImplemented/ExecContent/课别填报可编辑；Po残用 a-input-group compact（采购单-序号:数量）；defineExpose validate/getValues/resetFields -->
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
      <a-tab-pane key="tab-0" :tab="pi.t('common.page.form.tabs.basicinfo') + ' (1/2)'" force-render>
        <a-row :gutter="24">
      <a-col :span="12"><a-form-item :label="pi.label('ecPcId')"><a-input v-model:value="formState.ecPcId" disabled /></a-form-item></a-col>
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
      <a-col :span="12"><a-form-item name="isImplemented" :label="pi.label('isImplemented')"><TaktSelect v-model:value="formState.isImplemented" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="scheduledDate" :label="pi.label('scheduledDate')"><a-date-picker v-model:value="formState.scheduledDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="scheduledBatch" :label="pi.label('scheduledBatch')"><a-input :value="formState.scheduledBatch" :maxlength="20" @update:value="(value) => { formState.scheduledBatch = taktDeptBatchInput(value) }" /></a-form-item></a-col>
      <a-col :span="24">
        <a-form-item name="execContent" :label="pi.label('execContent')">
          <a-textarea v-model:value="formState.execContent" :rows="3" :placeholder="pi.t('common.page.form.placeholder.input')" />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item name="poRemainder" :label="pi.label('poRemainder')">
          <a-input-group compact class="w-full">
            <a-input
              :value="poRemainderParts.orderCode"
              class="!w-[42%] text-center"
              :maxlength="10"
              :placeholder="pi.t('common.validation.format.po.remainder.order')"
              inputmode="numeric"
              @update:value="onPoRemainderOrderInput"
            />
            <a-input
              class="!w-8 text-center"
              style="pointer-events: none"
              placeholder="-"
              disabled
            />
            <a-input
              :value="poRemainderParts.lineNumber"
              class="!w-[18%] text-center"
              :maxlength="4"
              :placeholder="pi.t('common.validation.format.po.remainder.line')"
              inputmode="numeric"
              @update:value="onPoRemainderLineInput"
            />
            <a-input
              class="!w-8 text-center"
              style="pointer-events: none"
              placeholder=":"
              disabled
            />
            <a-input
              :value="poRemainderParts.quantity"
              class="!w-[calc(40%-4rem)] text-center"
              :maxlength="20"
              :placeholder="pi.t('common.validation.format.po.remainder.qty')"
              inputmode="decimal"
              @update:value="onPoRemainderQtyInput"
            />
          </a-input-group>
        </a-form-item>
      </a-col>
      <a-col :span="12"><a-form-item name="balanceQuantity" :label="pi.label('balanceQuantity')"><a-input-number v-model:value="formState.balanceQuantity" class="w-full" :precision="2" /></a-form-item></a-col>
      <a-col :span="12">
        <a-form-item name="ecOldPartDisposition" :label="pi.label('ecOldPartDisposition')">
          <TaktSelect
            v-model:value="formState.ecOldPartDisposition"
            dict-type="logistics_manufacturing_ec_old_part_disposition"
            class="w-full"
            :apply-dict-default="false"
          />
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
import type { EcPc, EcPcUpdate } from '@/types/logistics/manufacturing/engineering-change/ecPc.types'
import { ecDeptImplementedFormValue, taktDeptBatchInput } from '@/constants/logistics/ecDeptExecLineFields.constants'
import { taktShowEcRootMaterialDescription } from '@/constants/logistics/ecScope.constants'
import { useEcDeptViewI18n } from '../../composables/useEcDeptViewI18n'
import { useEcDeptViewFormRules } from '../../composables/useEcDeptViewFormRules'

const props = defineProps<{ formData?: EcPc | null; loading?: boolean }>();
const pi = useEcDeptViewI18n('ecpc')
const { rules } = useEcDeptViewFormRules('ecpc', pi.label)
/** 表单 ref */
const formRef = ref();
/** 当前 Tab */
const activeTab = ref('tab-0');
/** Po残三段：采购单10位 / 序号4位 / 数量 */
const poRemainderParts = reactive({
  orderCode: '',
  lineNumber: '',
  quantity: '',
})
/** 表单状态 */
const formState = reactive<{
  ecPcId?: string;
  tenantCode?: string;
  companyCode?: string;
  cultureCode?: string;
  plantCode?: string;
  ecCode?: string;
  lineNumber?: number;
  ecModelCodeGroup?: string;
  ecRootMaterialCodeGroup?: string;
  ecRootMaterialDescription?: string;
  discontinuedStatus?: string;
  isImplemented: number;
  execContent?: string;
  scheduledDate?: string;
  scheduledBatch?: string;
  poRemainder?: string;
  balanceQuantity?: number | null;
  ecOldPartDisposition?: string;
  ecDetailId?: string;
  deptCode?: string;
  deptName?: string;
  ecImplScope?: number;
  ecControlScope?: number;
  isObsolete: number;
  remark?: string;
}>({
  isImplemented: 1,
  execContent: '',
  discontinuedStatus: 'Z0',
  isObsolete: 0,
});

/** 仅全仕向且按成品显示根物料描述 */
const showRootMaterialDescription = computed(() => taktShowEcRootMaterialDescription(formState.ecImplScope, formState.ecControlScope))

/**
 * 由三段拼出 Po残；全空则 undefined
 * @returns {string | undefined} 规范串或空
 */
function composePoRemainder(): string | undefined {
  const orderCode = poRemainderParts.orderCode.trim()
  const lineNumber = poRemainderParts.lineNumber.trim()
  const quantity = poRemainderParts.quantity.trim()
  if (!orderCode && !lineNumber && !quantity) {
    return undefined
  }
  return `${orderCode}-${lineNumber}:${quantity}`
}

/**
 * 把已存 Po残拆进三段输入
 * @param value 库内/接口值
 */
function splitPoRemainder(value: string | null | undefined) {
  const raw = value?.trim() ?? ''
  const matched = /^(\d{0,10})-(\d{0,4}):(.*)$/.exec(raw)
  if (matched) {
    poRemainderParts.orderCode = matched[1] ?? ''
    poRemainderParts.lineNumber = matched[2] ?? ''
    poRemainderParts.quantity = matched[3] ?? ''
    return
  }
  poRemainderParts.orderCode = ''
  poRemainderParts.lineNumber = ''
  poRemainderParts.quantity = ''
}

/** 同步拼接到 formState.poRemainder（供 rules 校验） */
function syncPoRemainderToForm() {
  formState.poRemainder = composePoRemainder()
}

/**
 * 采购单号仅数字、最多 10 位
 * @param value 输入值
 */
function onPoRemainderOrderInput(value: string) {
  poRemainderParts.orderCode = String(value ?? '').replace(/\D/g, '').slice(0, 10)
  syncPoRemainderToForm()
}

/**
 * 序号仅数字、最多 4 位
 * @param value 输入值
 */
function onPoRemainderLineInput(value: string) {
  poRemainderParts.lineNumber = String(value ?? '').replace(/\D/g, '').slice(0, 4)
  syncPoRemainderToForm()
}

/**
 * 数量：数字与至多一位小数点、最多两位小数
 * @param value 输入值
 */
function onPoRemainderQtyInput(value: string) {
  let next = String(value ?? '').replace(/[^\d.]/g, '')
  const dot = next.indexOf('.')
  if (dot >= 0) {
    next = `${next.slice(0, dot + 1)}${next.slice(dot + 1).replace(/\./g, '')}`
    const [intPart, decPart = ''] = next.split('.')
    next = `${intPart}.${decPart.slice(0, 2)}`
  }
  poRemainderParts.quantity = next.slice(0, 20)
  syncPoRemainderToForm()
}

watch(() => props.formData, (val) => {
  if (!val) {
    resetFields();
    return;
  }
  Object.assign(formState, {
    ecPcId: val.ecPcId,
    tenantCode: val.tenantCode,
    companyCode: val.companyCode,
    cultureCode: val.cultureCode,
    plantCode: val.plantCode,
    ecCode: val.ecCode,
    lineNumber: val.lineNumber,
    ecModelCodeGroup: val.ecModelCodeGroup,
    ecRootMaterialCodeGroup: val.ecRootMaterialCodeGroup,
    ecRootMaterialDescription: val.ecRootMaterialDescription,
    discontinuedStatus: val.discontinuedStatus ?? 'Z0',
    isImplemented: ecDeptImplementedFormValue(val.isImplemented),
    execContent: val.execContent ?? '',
    scheduledDate: val.scheduledDate,
    scheduledBatch: taktDeptBatchInput(val.scheduledBatch),
    poRemainder: val.poRemainder,
    balanceQuantity: val.balanceQuantity,
    ecOldPartDisposition: val.ecOldPartDisposition != null && String(val.ecOldPartDisposition).trim() !== ''
      ? String(val.ecOldPartDisposition).trim()
      : undefined,
    ecDetailId: val.ecDetailId,
    deptCode: val.deptCode,
    deptName: val.deptName,
    ecImplScope: val.ecImplScope,
    ecControlScope: val.ecControlScope,
    isObsolete: val.isObsolete ?? 0,
    remark: val.remark,
  });
  splitPoRemainder(val.poRemainder)
}, { immediate: true });

/** 校验表单 */
async function validate() {
  syncPoRemainderToForm()
  await formRef.value?.validate();
}

/**
 * 获取提交 DTO（课别填报字段前端必填）
 * @returns {EcPcUpdate} 更新 DTO
 */
function getValues(): EcPcUpdate {
  syncPoRemainderToForm()
  // 冗余字段（ecCode/deptCode/ecModelCodeGroup 等）界面只读但须随提交带回，否则后端 [Required]/Adapt 会当成空
  return { ...formState } as EcPcUpdate;
}

/** 重置表单 */
function resetFields() {
  Object.assign(formState, {
    ecPcId: '',
    tenantCode: '',
    companyCode: '',
    cultureCode: '',
    plantCode: '',
    ecCode: '',
    lineNumber: undefined,
    ecModelCodeGroup: '',
    ecRootMaterialCodeGroup: '',
    ecRootMaterialDescription: '',
    discontinuedStatus: 'Z0',
    isImplemented: 1,
    execContent: '',
    scheduledDate: undefined,
    scheduledBatch: undefined,
    poRemainder: undefined,
    balanceQuantity: undefined,
    ecOldPartDisposition: undefined,
    ecDetailId: '',
    deptCode: '',
    deptName: '',
    ecImplScope: undefined,
    ecControlScope: undefined,
    isObsolete: 0,
    remark: undefined,
  });
  poRemainderParts.orderCode = ''
  poRemainderParts.lineNumber = ''
  poRemainderParts.quantity = ''
}

defineExpose({ validate, getValues, resetFields });
</script>
