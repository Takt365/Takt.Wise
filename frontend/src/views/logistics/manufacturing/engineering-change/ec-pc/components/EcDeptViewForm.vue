<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-pc/components -->
<!-- 文件名称：EcDeptViewForm.vue -->
<!-- 功能描述：设变生管部门表单；冗余字段 disabled；IsImplemented/ExecContent/课别填报可编辑；旧品结余独立 Tab（同一设变下旧物料去重只结余一次；旧物料-采购单10位:序号4位:数量;结余;处理方式; → ecOldPartBalance，保存后同步同设变全部生管行）；defineExpose validate/getValues/resetFields -->
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
      <a-col :span="12"><a-form-item name="isImplemented" :label="pi.label('isImplemented')"><TaktSelect v-model:value="formState.isImplemented" dict-type="sys_yes_no" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="scheduledDate" :label="pi.label('scheduledDate')"><a-date-picker v-model:value="formState.scheduledDate" value-format="YYYY-MM-DD" class="w-full" /></a-form-item></a-col>
      <a-col :span="12"><a-form-item name="scheduledBatch" :label="pi.label('scheduledBatch')"><a-input :value="formState.scheduledBatch" :maxlength="20" @update:value="(value) => { formState.scheduledBatch = taktDeptBatchInput(value) }" /></a-form-item></a-col>
      <a-col :span="24">
        <a-form-item name="execContent" :label="pi.label('execContent')">
          <a-textarea v-model:value="formState.execContent" :rows="3" :placeholder="pi.t('common.page.form.placeholder.input')" />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item :label="pi.label('remark')">
          <a-textarea v-model:value="formState.remark" :rows="2" />
        </a-form-item>
      </a-col>
        </a-row>
      </a-tab-pane>
      <a-tab-pane
        key="tab-balance"
        :tab="pi.t('logistics.manufacturing.engineering-change.dept-exec.page.tabs.balance')"
        force-render
      >
        <a-row :gutter="24">
          <a-col :span="12">
            <a-form-item :label="pi.t('common.validation.format.pc.oldpart.need')">
              <a-radio-group :value="needOldPartHandling" @update:value="onNeedOldPartHandlingChange">
                <a-radio :value="1">{{ pi.t('common.status.yes') }}</a-radio>
                <a-radio :value="0">{{ pi.t('common.status.no') }}</a-radio>
              </a-radio-group>
            </a-form-item>
          </a-col>
          <a-col v-if="needOldPartHandling === 1" :span="24">
            <a-form-item name="ecOldPartBalance" :label="pi.label('ecOldPartBalance')">
              <div v-if="oldPartLines.length === 0" class="text-text-secondary text-sm">
                {{ pi.t('common.validation.format.pc.oldpart.empty') }}
              </div>
              <div v-else class="flex flex-col gap-2 w-full">
                <a-input-group
                  v-for="(line, index) in oldPartLines"
                  :key="`old-part-${line.materialCode}-${index}`"
                  compact
                  class="w-full"
                >
                  <a-input :value="line.materialCode" class="!w-[16%] text-center" disabled />
                  <a-input class="!w-6 text-center" style="pointer-events: none" placeholder="-" disabled />
                  <a-input
                    :value="line.orderCode"
                    class="!w-[16%] text-center"
                    :maxlength="10"
                    :placeholder="pi.t('common.validation.format.pc.oldpart.order')"
                    inputmode="numeric"
                    @update:value="(value) => onOrderInput(index, value)"
                  />
                  <a-input class="!w-6 text-center" style="pointer-events: none" placeholder=":" disabled />
                  <a-input
                    :value="line.lineNumber"
                    class="!w-[10%] text-center"
                    :maxlength="4"
                    :placeholder="pi.t('common.validation.format.pc.oldpart.line')"
                    inputmode="numeric"
                    @update:value="(value) => onLineNumberInput(index, value)"
                  />
                  <a-input class="!w-6 text-center" style="pointer-events: none" placeholder=":" disabled />
                  <a-input
                    :value="line.quantity"
                    class="!w-[12%] text-center"
                    :maxlength="20"
                    :placeholder="pi.t('common.validation.format.pc.oldpart.poqty')"
                    inputmode="decimal"
                    @update:value="(value) => onQtyInput(index, value)"
                  />
                  <a-input class="!w-6 text-center" style="pointer-events: none" placeholder=";" disabled />
                  <a-input
                    :value="line.balanceQty"
                    class="!w-[12%] text-center"
                    :maxlength="20"
                    :placeholder="pi.t('common.validation.format.pc.oldpart.qty')"
                    inputmode="decimal"
                    @update:value="(value) => onBalanceInput(index, value)"
                  />
                  <a-input class="!w-6 text-center" style="pointer-events: none" placeholder=";" disabled />
                  <TaktSelect
                    :value="line.disposition"
                    dict-type="logistics_manufacturing_ec_old_part_disposition"
                    class="!w-[calc(16%-1.5rem)]"
                    :apply-dict-default="false"
                    @update:value="(value) => onDispositionInput(index, value)"
                  />
                </a-input-group>
              </div>
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
      <a-col :span="12"><a-form-item :label="pi.label('ecPcId')"><a-input v-model:value="formState.ecPcId" disabled /></a-form-item></a-col>
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
import type { EcPc, EcPcUpdate } from '@/types/logistics/manufacturing/engineering-change/ecPc.types'
import {
  EC_DEPT_PO_DEFAULT_LINE,
  EC_DEPT_PO_DEFAULT_ORDER,
  EC_PC_OLD_PART_DEFAULT_DISPOSITION,
  EC_PC_OLD_PART_DEFAULT_QTY,
  EC_PC_PO_DEFAULT_QTY,
  ecDeptImplementedFormValue,
  taktDeptBatchInput,
  taktEcDefaultExecContent,
  taktEcIsAutoExecContent,
} from '@/constants/logistics/ecDeptExecLineFields.constants'
import { useEcDeptViewI18n } from '../../composables/useEcDeptViewI18n'
import { useEcDeptViewFormRules } from '../../composables/useEcDeptViewFormRules'

/** 旧品结余一行：旧物料-采购单:序号:数量;结余;处理方式; */
type OldPartLine = {
  materialCode: string
  orderCode: string
  lineNumber: string
  quantity: string
  balanceQty: string
  disposition: string
}

const props = defineProps<{ formData?: EcPc | null; loading?: boolean }>();
const pi = useEcDeptViewI18n('ecpc')
/** 旧品是否结余（1=是 0=否；默认需要结余；对应 EcOldPartBalance） */
const needOldPartHandling = ref(1)
const { rules } = useEcDeptViewFormRules('ecpc', pi.label, needOldPartHandling)
/** 表单 ref */
const formRef = ref();
/** 当前 Tab */
const activeTab = ref('tab-0');
/** 按旧物料编码组拆出的旧品结余行 */
const oldPartLines = ref<OldPartLine[]>([])
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
  ecOldMaterialCodeGroup?: string;
  ecNewMaterialCodeGroup?: string;
  discontinuedStatus?: string;
  isImplemented: number;
  execContent?: string;
  scheduledDate?: string;
  scheduledBatch?: string;
  ecOldPartBalance?: string;
  ecGrainKey?: string;
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
  ecOldPartBalance: undefined,
  isObsolete: 0,
});

/**
 * 旧物料编码组按分号拆成物料列表
 * @param group 旧物料编码组
 * @returns {string[]} 物料编码
 */
function splitOldMaterialCodes(group: string | null | undefined): string[] {
  return String(group ?? '')
    .split(/[;；]/)
    .map((item) => item.trim())
    .filter(Boolean)
}

/**
 * 解析已存旧品结余；支持现行「旧物料-采购单:序号:数量;结余;处理方式;」及历史单值字典
 * @param content 旧品结余文本
 * @returns {Map<string, Omit<OldPartLine, 'materialCode'>>} 映射
 */
function parseOldPartDisposition(
  content: string | null | undefined,
): Map<string, Omit<OldPartLine, 'materialCode'>> {
  const map = new Map<string, Omit<OldPartLine, 'materialCode'>>()
  const raw = String(content ?? '').trim()
  if (!raw) {
    return map
  }
  if (/^(?:[1-5]|9)$/.test(raw)) {
    map.set('__legacy_single__', {
      orderCode: EC_DEPT_PO_DEFAULT_ORDER,
      lineNumber: EC_DEPT_PO_DEFAULT_LINE,
      quantity: EC_PC_PO_DEFAULT_QTY,
      balanceQty: EC_PC_OLD_PART_DEFAULT_QTY,
      disposition: raw,
    })
    return map
  }
  const parts = raw
    .split(/[\n\r]+/)
    .map((item) => item.trim())
    .filter(Boolean)
  for (const part of parts) {
    const matched = /^([^-\s;]+)-([^:]+):([^:]+):([^;]+);([^;]+);([^;]+);?$/.exec(part)
    if (!matched) {
      continue
    }
    const materialCode = (matched[1] ?? '').trim()
    if (!materialCode) {
      continue
    }
    map.set(materialCode, {
      orderCode: (matched[2] ?? '').trim(),
      lineNumber: (matched[3] ?? '').trim(),
      quantity: (matched[4] ?? '').trim(),
      balanceQty: (matched[5] ?? '').trim(),
      disposition: (matched[6] ?? '').trim(),
    })
  }
  return map
}

/**
 * 以旧物料编码组为准重建行
 * @param group 旧物料编码组
 * @param content 已存旧品结余
 * @returns {OldPartLine[]} 行列表
 */
function rebuildOldPartLines(
  group: string | null | undefined,
  content: string | null | undefined,
): OldPartLine[] {
  const parsed = parseOldPartDisposition(content)
  const legacy = parsed.get('__legacy_single__')
  return splitOldMaterialCodes(group).map((materialCode) => {
    const hit = parsed.get(materialCode) ?? legacy
    return {
      materialCode,
      orderCode: hit?.orderCode?.trim() || EC_DEPT_PO_DEFAULT_ORDER,
      lineNumber: hit?.lineNumber?.trim() || EC_DEPT_PO_DEFAULT_LINE,
      quantity: hit?.quantity?.trim() || EC_PC_PO_DEFAULT_QTY,
      balanceQty: hit?.balanceQty?.trim() || EC_PC_OLD_PART_DEFAULT_QTY,
      disposition: hit?.disposition?.trim() || EC_PC_OLD_PART_DEFAULT_DISPOSITION,
    }
  })
}

/**
 * 组合旧品结余：每行「旧物料-采购单:序号:数量;结余;处理方式;」
 * @param lines 行列表
 * @returns {string} 旧品结余
 */
function composeOldPartDisposition(lines: readonly OldPartLine[]): string {
  return lines
    .map((line) => {
      const materialCode = line.materialCode.trim()
      if (!materialCode) {
        return ''
      }
      const orderCode = line.orderCode.trim() || EC_DEPT_PO_DEFAULT_ORDER
      const lineNumber = line.lineNumber.trim() || EC_DEPT_PO_DEFAULT_LINE
      const quantity = line.quantity.trim() || EC_PC_PO_DEFAULT_QTY
      const balanceQty = line.balanceQty.trim() || EC_PC_OLD_PART_DEFAULT_QTY
      const disposition = line.disposition.trim() || EC_PC_OLD_PART_DEFAULT_DISPOSITION
      return `${materialCode}-${orderCode}:${lineNumber}:${quantity};${balanceQty};${disposition};`
    })
    .filter(Boolean)
    .join('\n')
}

/** 同步到 formState.ecOldPartBalance；否处理时清空 */
function syncOldPartToForm() {
  if (needOldPartHandling.value !== 1) {
    formState.ecOldPartBalance = undefined
    return
  }
  formState.ecOldPartBalance = composeOldPartDisposition(oldPartLines.value)
}

/**
 * 切换旧品是否结余
 * @param value 1=是 0=否
 */
function onNeedOldPartHandlingChange(value: number | string) {
  needOldPartHandling.value = Number(value) === 1 ? 1 : 0
  syncOldPartToForm()
  if (needOldPartHandling.value !== 1) {
    formRef.value?.clearValidate?.(['ecOldPartBalance'])
  }
}

/**
 * 规范化非负小数输入
 * @param value 输入值
 * @returns {string} 规范化文本
 */
function normalizeDecimalInput(value: string): string {
  let next = String(value ?? '').replace(/[^\d.]/g, '')
  const dot = next.indexOf('.')
  if (dot >= 0) {
    next = `${next.slice(0, dot + 1)}${next.slice(dot + 1).replace(/\./g, '')}`
    const [intPart, decPart = ''] = next.split('.')
    next = `${intPart}.${decPart.slice(0, 2)}`
  }
  return next.slice(0, 20)
}

/**
 * 采购单号仅数字、最多 10 位
 * @param index 行下标
 * @param value 输入值
 */
function onOrderInput(index: number, value: string) {
  const line = oldPartLines.value[index]
  if (!line) {
    return
  }
  line.orderCode = String(value ?? '').replace(/\D/g, '').slice(0, 10)
  syncOldPartToForm()
}

/**
 * 序号仅数字、最多 4 位
 * @param index 行下标
 * @param value 输入值
 */
function onLineNumberInput(index: number, value: string) {
  const line = oldPartLines.value[index]
  if (!line) {
    return
  }
  line.lineNumber = String(value ?? '').replace(/\D/g, '').slice(0, 4)
  syncOldPartToForm()
}

/**
 * Po 数量
 * @param index 行下标
 * @param value 输入值
 */
function onQtyInput(index: number, value: string) {
  const line = oldPartLines.value[index]
  if (!line) {
    return
  }
  line.quantity = normalizeDecimalInput(value)
  syncOldPartToForm()
}

/**
 * 结余数量
 * @param index 行下标
 * @param value 输入值
 */
function onBalanceInput(index: number, value: string) {
  const line = oldPartLines.value[index]
  if (!line) {
    return
  }
  line.balanceQty = normalizeDecimalInput(value)
  syncOldPartToForm()
}

/**
 * 处理方式
 * @param index 行下标
 * @param value 字典值
 */
function onDispositionInput(index: number, value: string | number | null | undefined) {
  const line = oldPartLines.value[index]
  if (!line) {
    return
  }
  line.disposition = String(value ?? '').trim() || EC_PC_OLD_PART_DEFAULT_DISPOSITION
  syncOldPartToForm()
}

watch(() => props.formData, (val) => {
  if (!val) {
    resetFields();
    return;
  }
  const scheduledBatch = taktDeptBatchInput(val.scheduledBatch)
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
    ecOldMaterialCodeGroup: val.ecOldMaterialCodeGroup,
    ecNewMaterialCodeGroup: val.ecNewMaterialCodeGroup,
    discontinuedStatus: val.discontinuedStatus ?? 'Z0',
    isImplemented: ecDeptImplementedFormValue(val.isImplemented),
    scheduledDate: val.scheduledDate,
    scheduledBatch,
    execContent: taktEcIsAutoExecContent(val.execContent)
      ? taktEcDefaultExecContent(val.ecCode, val.ecRootMaterialCodeGroup, scheduledBatch)
      : (val.execContent ?? ''),
    ecOldPartBalance: val.ecOldPartBalance,
    ecGrainKey: val.ecGrainKey ?? '',
    ecDetailId: val.ecDetailId,
    deptCode: val.deptCode,
    deptName: val.deptName,
    ecImplScope: val.ecImplScope,
    ecControlScope: val.ecControlScope,
    isObsolete: val.isObsolete ?? 0,
    remark: val.remark,
  });
  // 默认需要处理；有已存内容时保持是，空内容也默认是
  needOldPartHandling.value = 1
  oldPartLines.value = rebuildOldPartLines(val.ecOldMaterialCodeGroup, val.ecOldPartBalance)
  syncOldPartToForm()
}, { immediate: true });

/** 手改预定批次时，若执行内容仍是自动文案则联动 */
watch(
  () => formState.scheduledBatch,
  (batch) => {
    if (taktEcIsAutoExecContent(formState.execContent)) {
      formState.execContent = taktEcDefaultExecContent(formState.ecCode, formState.ecRootMaterialCodeGroup, batch)
    }
  },
)

/** 校验表单；结余失败时切到结余 Tab */
async function validate() {
  syncOldPartToForm()
  try {
    await formRef.value?.validate()
  } catch (err: unknown) {
    const fields = (err as { errorFields?: Array<{ name?: Array<string | number> }> })?.errorFields
    const names = new Set((fields ?? []).map((f) => String(f.name?.[0] ?? '')))
    if (names.has('ecOldPartBalance')) {
      activeTab.value = 'tab-balance'
    }
    throw err
  }
}

/**
 * 获取提交 DTO
 * @returns {EcPcUpdate} 更新 DTO
 */
function getValues(): EcPcUpdate {
  syncOldPartToForm()
  return {
    ...formState,
  } as EcPcUpdate;
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
    ecOldMaterialCodeGroup: '',
    ecNewMaterialCodeGroup: '',
    discontinuedStatus: 'Z0',
    isImplemented: 1,
    execContent: '',
    scheduledDate: undefined,
    scheduledBatch: undefined,
    ecOldPartBalance: undefined,
    ecGrainKey: '',
    ecDetailId: '',
    deptCode: '',
    deptName: '',
    ecImplScope: undefined,
    ecControlScope: undefined,
    isObsolete: 0,
    remark: undefined,
  });
  needOldPartHandling.value = 1
  oldPartLines.value = []
}

defineExpose({ validate, getValues, resetFields });
</script>
