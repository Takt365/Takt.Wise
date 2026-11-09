<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-iqc/components -->
<!-- 文件名称：EcDeptViewForm.vue -->
<!-- 功能描述：设变受检部门表单；冗余字段 disabled；IsImplemented/检验日期可编辑；执行内容按新物料编码组拆成多行「物料:检验单10位:序号4位:PASS」（缺省 4300000000:0010），组合写入 execContent；defineExpose validate/getValues/resetFields -->
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
      <a-col :span="12"><a-form-item :label="pi.label('plantCode')"><a-input v-model:value="formState.plantCode" disabled /></a-form-item></a-col>
      <a-col :span="12"><a-form-item :label="pi.label('ecCode')"><a-input v-model:value="formState.ecCode" disabled /></a-form-item></a-col>
      <a-col :span="24"><a-form-item :label="pi.label('ecModelCodeGroup')"><a-textarea v-model:value="formState.ecModelCodeGroup" :rows="2" disabled /></a-form-item></a-col>
      <a-col :span="24"><a-form-item :label="pi.label('ecRootMaterialCodeGroup')"><a-textarea v-model:value="formState.ecRootMaterialCodeGroup" :rows="2" disabled /></a-form-item></a-col>
      <a-col :span="12">
        <a-form-item :label="pi.label('ecNewRequiresInspection')">
          <TaktSelect v-model:value="formState.ecNewRequiresInspection" dict-type="sys_yes_no" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="12"><a-form-item name="isImplemented" :label="pi.label('isImplemented')"><TaktSelect v-model:value="formState.isImplemented" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="inspectionDate" :label="pi.label('inspectionDate')"><a-date-picker v-model:value="formState.inspectionDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
      <a-col :span="24">
        <a-form-item name="execContent" :label="pi.label('execContent')">
          <div v-if="iqcExecLines.length === 0" class="text-text-secondary text-sm">
            {{ pi.t('common.validation.format.iqc.exec.empty') }}
          </div>
          <div v-else class="flex flex-col gap-2 w-full">
            <a-input-group
              v-for="(line, index) in iqcExecLines"
              :key="`${line.materialCode}-${index}`"
              compact
              class="w-full"
            >
              <a-input
                :value="line.materialCode"
                class="!w-[28%] text-center"
                disabled
              />
              <a-input
                class="!w-8 text-center"
                style="pointer-events: none"
                placeholder=":"
                disabled
              />
              <a-input
                :value="line.inspectionOrderCode"
                class="!w-[28%] text-center"
                :maxlength="10"
                :placeholder="pi.t('common.validation.format.iqc.exec.order')"
                inputmode="numeric"
                @update:value="(value) => onIqcExecOrderInput(index, value)"
              />
              <a-input
                class="!w-8 text-center"
                style="pointer-events: none"
                placeholder=":"
                disabled
              />
              <a-input
                :value="line.inspectionLineNumber"
                class="!w-[16%] text-center"
                :maxlength="4"
                :placeholder="pi.t('common.validation.format.iqc.exec.line')"
                inputmode="numeric"
                @update:value="(value) => onIqcExecLineInput(index, value)"
              />
              <a-input
                class="!w-8 text-center"
                style="pointer-events: none"
                placeholder=":"
                disabled
              />
              <a-input
                value="PASS"
                class="!w-[calc(28%-6rem)] text-center"
                disabled
              />
            </a-input-group>
          </div>
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
      <a-col :span="12"><a-form-item :label="pi.label('ecIqcId')"><a-input v-model:value="formState.ecIqcId" disabled /></a-form-item></a-col>
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
import type { EcIqc, EcIqcUpdate } from '@/types/logistics/manufacturing/engineering-change/ecIqc.types'
import {
  EC_DEPT_PO_DEFAULT_LINE,
  EC_DEPT_PO_DEFAULT_ORDER,
  ecDeptImplementedFormValue,
} from '@/constants/logistics/ecDeptExecLineFields.constants'
import { useEcDeptViewI18n } from '../../composables/useEcDeptViewI18n'
import { useEcDeptViewFormRules } from '../../composables/useEcDeptViewFormRules'

/** 受检执行内容一行：物料:检验单10位:序号4位:PASS */
type IqcExecLine = {
  materialCode: string
  inspectionOrderCode: string
  inspectionLineNumber: string
}

const props = defineProps<{ formData?: EcIqc | null; loading?: boolean }>();
const pi = useEcDeptViewI18n('eciqc')
const { rules } = useEcDeptViewFormRules('eciqc', pi.label)
const formRef = ref();
const activeTab = ref('tab-0');
/** 按新物料编码组拆出的执行内容行 */
const iqcExecLines = ref<IqcExecLine[]>([])
const formState = reactive<{
  ecIqcId?: string;
  tenantCode?: string;
  companyCode?: string;
  cultureCode?: string;
  plantCode?: string;
  ecCode?: string;
  lineNumber?: number;
  discontinuedStatus?: string;
  ecNewMaterialCodeGroup?: string;
  ecModelCodeGroup?: string;
  ecRootMaterialCodeGroup?: string;
  ecNewRequiresInspection?: number;
  isImplemented: number;
  execContent?: string;
  inspectionDate?: string;
  ecGrainKey?: string;
  ecDetailId?: string;
  deptCode?: string;
  deptName?: string;
  ecImplScope?: number;
  ecControlScope?: number;
  isObsolete: number;
  remark?: string;
}>({ isImplemented: 1, execContent: '', discontinuedStatus: 'Z0', isObsolete: 0 });

/**
 * 新物料编码组按分号拆成物料列表
 * @param group 新物料编码组
 * @returns {string[]} 物料编码
 */
function splitNewMaterialCodes(group: string | null | undefined): string[] {
  return String(group ?? '')
    .split(/[;；]/)
    .map((item) => item.trim())
    .filter(Boolean)
}

/**
 * 解析已存执行内容为物料 → 检验单10位/序号4位（末段须为 PASS）
 * @param content 执行内容
 * @returns {Map<string, Omit<IqcExecLine, 'materialCode'>>} 物料到检验单段
 */
function parseIqcExecContent(
  content: string | null | undefined,
): Map<string, Omit<IqcExecLine, 'materialCode'>> {
  const map = new Map<string, Omit<IqcExecLine, 'materialCode'>>()
  const raw = String(content ?? '').trim()
  if (!raw) {
    return map
  }
  const parts = raw
    .split(/[;\n\r]+/)
    .map((item) => item.trim())
    .filter(Boolean)
  for (const part of parts) {
    const matched = /^([^:]+):(\d{1,10}):(\d{1,4}):PASS$/i.exec(part)
    if (!matched) {
      continue
    }
    const materialCode = (matched[1] ?? '').trim()
    if (!materialCode) {
      continue
    }
    map.set(materialCode, {
      inspectionOrderCode: (matched[2] ?? '').trim(),
      inspectionLineNumber: (matched[3] ?? '').trim(),
    })
  }
  return map
}

/**
 * 以新物料编码组为准重建行；缺检验单/序号时填默认 4300000000:0010
 * @param group 新物料编码组
 * @param content 已存执行内容
 * @returns {IqcExecLine[]} 行列表
 */
function rebuildIqcExecLines(
  group: string | null | undefined,
  content: string | null | undefined,
): IqcExecLine[] {
  const parsed = parseIqcExecContent(content)
  return splitNewMaterialCodes(group).map((materialCode) => {
    const hit = parsed.get(materialCode)
    return {
      materialCode,
      inspectionOrderCode: hit?.inspectionOrderCode?.trim() || EC_DEPT_PO_DEFAULT_ORDER,
      inspectionLineNumber: hit?.inspectionLineNumber?.trim() || EC_DEPT_PO_DEFAULT_LINE,
    }
  })
}

/**
 * 组合执行内容：每行「物料:检验单10位:序号4位:PASS;」，行间换行
 * @param lines 行列表
 * @returns {string} 执行内容
 */
function composeIqcExecContent(lines: readonly IqcExecLine[]): string {
  return lines
    .map((line) => {
      const materialCode = line.materialCode.trim()
      const orderCode = line.inspectionOrderCode.trim()
      const lineNumber = line.inspectionLineNumber.trim()
      if (!materialCode) {
        return ''
      }
      return `${materialCode}:${orderCode}:${lineNumber}:PASS;`
    })
    .filter(Boolean)
    .join('\n')
}

/** 把多行组件同步到 formState.execContent */
function syncIqcExecContentToForm() {
  formState.execContent = composeIqcExecContent(iqcExecLines.value)
}

/**
 * 检验单号仅数字、最多 10 位
 * @param index 行下标
 * @param value 输入值
 */
function onIqcExecOrderInput(index: number, value: string) {
  const line = iqcExecLines.value[index]
  if (!line) {
    return
  }
  line.inspectionOrderCode = String(value ?? '').replace(/\D/g, '').slice(0, 10)
  syncIqcExecContentToForm()
}

/**
 * 检验序号仅数字、最多 4 位
 * @param index 行下标
 * @param value 输入值
 */
function onIqcExecLineInput(index: number, value: string) {
  const line = iqcExecLines.value[index]
  if (!line) {
    return
  }
  line.inspectionLineNumber = String(value ?? '').replace(/\D/g, '').slice(0, 4)
  syncIqcExecContentToForm()
}

watch(() => props.formData, (val) => {
  if (!val) { resetFields(); return; }
  Object.assign(formState, {
    ecIqcId: val.ecIqcId,
    tenantCode: val.tenantCode,
    companyCode: val.companyCode,
    cultureCode: val.cultureCode,
    plantCode: val.plantCode,
    ecCode: val.ecCode,
    lineNumber: val.lineNumber,
    discontinuedStatus: val.discontinuedStatus ?? 'Z0',
    ecNewMaterialCodeGroup: val.ecNewMaterialCodeGroup,
    ecModelCodeGroup: val.ecModelCodeGroup,
    ecRootMaterialCodeGroup: val.ecRootMaterialCodeGroup,
    ecNewRequiresInspection: val.ecNewRequiresInspection ?? 0,
    isImplemented: ecDeptImplementedFormValue(val.isImplemented),
    inspectionDate: val.inspectionDate,
    execContent: val.execContent ?? '',
    ecGrainKey: (val as { ecGrainKey?: string }).ecGrainKey ?? '',
    ecDetailId: val.ecDetailId,
    deptCode: val.deptCode,
    deptName: val.deptName,
    ecImplScope: val.ecImplScope,
    ecControlScope: val.ecControlScope,
    isObsolete: val.isObsolete ?? 0,
    remark: val.remark,
  });
  iqcExecLines.value = rebuildIqcExecLines(val.ecNewMaterialCodeGroup, val.execContent)
  syncIqcExecContentToForm()
}, { immediate: true });

async function validate() {
  syncIqcExecContentToForm()
  await formRef.value?.validate()
}
function getValues(): EcIqcUpdate {
  syncIqcExecContentToForm()
  // 冗余字段（ecCode/deptCode 等）界面只读但须随提交带回，否则后端 [Required]/Adapt 会当成空
  return { ...formState } as EcIqcUpdate;
}
function resetFields() {
  Object.assign(formState, {
    ecIqcId: '',
    tenantCode: '', companyCode: '', cultureCode: '', plantCode: '',
    ecCode: '', lineNumber: undefined, discontinuedStatus: 'Z0',
    ecModelCodeGroup: '', ecRootMaterialCodeGroup: '',
    ecNewMaterialCodeGroup: '',
    ecNewRequiresInspection: 0, isImplemented: 1, execContent: '',
    inspectionDate: undefined,
    ecGrainKey: '', ecDetailId: '', deptCode: '', deptName: '',
    ecImplScope: undefined, ecControlScope: undefined, isObsolete: 0, remark: undefined,
  });
  iqcExecLines.value = []
}
defineExpose({ validate, getValues, resetFields });
</script>
