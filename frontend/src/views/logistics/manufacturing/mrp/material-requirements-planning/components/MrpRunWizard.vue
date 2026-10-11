<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/mrp/material-requirements-planning/components -->
<!-- 文件名称：MrpRunWizard.vue -->
<!-- 功能描述：MRP 运算向导（选 MPS → 参数 → 运算 → 预览 → 发布）；defineExpose: open -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <TaktModal
    v-model:open="visible"
    :title="t('logistics.manufacturing.mrp.page.wizard.title')"
    width="960px"
    :confirm-loading="submitLoading"
    :ok-text="okText"
    :ok-button-props="{ disabled: okDisabled }"
    @ok="handleOk"
    @cancel="handleCancel"
  >
    <a-steps :current="currentStep" class="mb-6" size="small">
      <a-step :title="t('logistics.manufacturing.mrp.page.wizard.step.mps')" />
      <a-step :title="t('logistics.manufacturing.mrp.page.wizard.step.options')" />
      <a-step :title="t('logistics.manufacturing.mrp.page.wizard.step.run')" />
      <a-step :title="t('logistics.manufacturing.mrp.page.wizard.step.preview')" />
      <a-step :title="t('logistics.manufacturing.mrp.page.wizard.step.publish')" />
    </a-steps>

    <div v-if="currentStep === 0">
      <a-form layout="horizontal" :label-col="{ span: 6 }" :wrapper-col="{ span: 16 }">
        <a-form-item :label="t('entity.materialrequirementsplanning.masterproductionscheduleid')">
          <TaktSelect
            v-model:value="wizardState.masterProductionScheduleId"
            api-url="TaktMasterProductionSchedules/options"
            :placeholder="t('common.page.form.placeholder.select')"
            allow-clear
          />
        </a-form-item>
      </a-form>
      <a-typography-text type="secondary">
        {{ t('logistics.manufacturing.mrp.page.wizard.mpsHint') }}
      </a-typography-text>
    </div>

    <div v-else-if="currentStep === 1">
      <a-form layout="horizontal" :label-col="{ span: 8 }" :wrapper-col="{ span: 14 }">
        <a-form-item :label="t('logistics.manufacturing.mrp.page.wizard.bomType')">
          <TaktSelect
            v-model:value="wizardState.options.bomType"
            dict-type="logistics_manufacturing_bom_type"
            :placeholder="t('common.page.form.placeholder.select')"
          />
        </a-form-item>
        <a-form-item :label="t('logistics.manufacturing.mrp.page.wizard.maxBomLevel')">
          <a-input-number v-model:value="wizardState.options.maxBomLevel" :min="1" :max="30" class="w-full" />
        </a-form-item>
        <a-form-item :label="t('logistics.manufacturing.mrp.page.wizard.includePo')">
          <a-switch v-model:checked="wizardState.options.includeOpenPurchaseOrders" />
        </a-form-item>
        <a-form-item :label="t('logistics.manufacturing.mrp.page.wizard.includePlanned')">
          <a-switch v-model:checked="wizardState.options.includePlannedOrders" />
        </a-form-item>
      </a-form>
    </div>

    <div v-else-if="currentStep === 2" class="py-8 text-center">
      <a-spin v-if="submitLoading" />
      <a-typography-text v-else>
        {{ t('logistics.manufacturing.mrp.page.wizard.runReady') }}
      </a-typography-text>
    </div>

    <div v-else-if="currentStep === 3">
      <a-table
        size="middle"
        :loading="previewLoading"
        :columns="previewColumns"
        :data-source="previewItems"
        :pagination="{ pageSize: 8 }"
        row-key="materialRequirementsPlanningItemId"
        :scroll="{ x: 900 }"
      />
    </div>

    <div v-else class="py-8 text-center">
      <a-typography-text>
        {{ t('logistics.manufacturing.mrp.page.wizard.publishHint') }}
      </a-typography-text>
    </div>
  </TaktModal>
</template>

<script setup lang="ts">
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { publishMrp, runMrp } from '@/api/logistics/manufacturing/mrp/manufacturingPlanningFlow.api'
import { getMaterialRequirementsPlanningById, updateMaterialRequirementsPlanning } from '@/api/logistics/manufacturing/mrp/materialRequirementsPlanning.api'
import { getMaterialRequirementsPlanningItemList } from '@/api/logistics/manufacturing/mrp/materialRequirementsPlanningItem.api'
import type { MaterialRequirementsPlanningItem } from '@/types/logistics/manufacturing/mrp/materialRequirementsPlanningItem.types'
import type { MrpRunOptions } from '@/types/logistics/manufacturing/mrp/manufacturingPlanningFlow.types'

const emit = defineEmits<{
  completed: []
}>()

const { t } = useI18n()

/** 弹窗可见 */
const visible = ref(false)
/** 当前步骤 */
const currentStep = ref(0)
/** 提交 loading */
const submitLoading = ref(false)
/** 预览 loading */
const previewLoading = ref(false)
/** 当前 MRP 头 ID */
const mrpId = ref<string>('')
/** 预览明细 */
const previewItems = ref<MaterialRequirementsPlanningItem[]>([])

/** 向导表单状态 */
const wizardState = reactive<{
  masterProductionScheduleId?: string
  options: Required<MrpRunOptions>
}>({
  masterProductionScheduleId: undefined,
  options: {
    bomType: 2,
    maxBomLevel: 20,
    includeOpenPurchaseOrders: true,
    includePlannedOrders: true,
  },
})

/** 预览列 */
const previewColumns = computed<TableColumnsType<MaterialRequirementsPlanningItem>>(() => [
  { title: t('entity.materialrequirementsplanningitem.materialcode'), dataIndex: 'materialCode', key: 'materialCode', width: 120 },
  { title: t('entity.materialrequirementsplanningitem.grossrequirement'), dataIndex: 'grossRequirement', key: 'grossRequirement', width: 100 },
  { title: t('entity.materialrequirementsplanningitem.onhandquantity'), dataIndex: 'onHandQuantity', key: 'onHandQuantity', width: 100 },
  { title: t('entity.materialrequirementsplanningitem.scheduledreceipts'), dataIndex: 'scheduledReceipts', key: 'scheduledReceipts', width: 100 },
  { title: t('entity.materialrequirementsplanningitem.netrequirement'), dataIndex: 'netRequirement', key: 'netRequirement', width: 100 },
  { title: t('entity.materialrequirementsplanningitem.projectedonhand'), dataIndex: 'projectedOnHand', key: 'projectedOnHand', width: 100 }])

/** 确认按钮文案 */
const okText = computed(() => {
  if (currentStep.value === 2) return t('common.page.button.run')
  if (currentStep.value === 4) return t('common.page.button.publish')
  return t('common.page.button.next')
})

/** 确认按钮禁用 */
const okDisabled = computed(() => {
  if (currentStep.value === 0) return !wizardState.masterProductionScheduleId
  return false
})

/**
 * 打开向导
 * @param record MRP 头记录
 */
function open(record: { materialRequirementsPlanningId: string; masterProductionScheduleId?: string }) {
  mrpId.value = record.materialRequirementsPlanningId
  wizardState.masterProductionScheduleId = record.masterProductionScheduleId
  wizardState.options = {
    bomType: 2,
    maxBomLevel: 20,
    includeOpenPurchaseOrders: true,
    includePlannedOrders: true,
  }
  currentStep.value = 0
  previewItems.value = []
  visible.value = true
}

/**
 * 加载预览明细
 */
async function loadPreviewItems() {
  if (!mrpId.value) return
  previewLoading.value = true
  try {
    const result = await getMaterialRequirementsPlanningItemList({
      materialRequirementsPlanningId: mrpId.value,
      pageIndex: 1,
      pageSize: 200,
    })
    previewItems.value = result.data ?? []
  } finally {
    previewLoading.value = false
  }
}

/**
 * 向导确认
 */
async function handleOk() {
  if (currentStep.value === 0) {
    if (!mrpId.value || !wizardState.masterProductionScheduleId) return
    submitLoading.value = true
    try {
      const detail = await getMaterialRequirementsPlanningById(mrpId.value)
      await updateMaterialRequirementsPlanning(mrpId.value, {
        ...detail,
        masterProductionScheduleId: wizardState.masterProductionScheduleId,
      } as any)
      currentStep.value = 1
    } finally {
      submitLoading.value = false
    }
    return
  }
  if (currentStep.value === 1) {
    currentStep.value = 2
    return
  }
  if (currentStep.value === 2) {
    submitLoading.value = true
    try {
      await runMrp({
        materialRequirementsPlanningId: mrpId.value,
        options: { ...wizardState.options },
      })
      message.success(t('logistics.manufacturing.mrp.page.wizard.runSuccess'))
      currentStep.value = 3
      await loadPreviewItems()
    } finally {
      submitLoading.value = false
    }
    return
  }
  if (currentStep.value === 3) {
    currentStep.value = 4
    return
  }
  submitLoading.value = true
  try {
    await publishMrp(mrpId.value)
    message.success(t('logistics.manufacturing.mrp.page.wizard.publishSuccess'))
    visible.value = false
    emit('completed')
  } finally {
    submitLoading.value = false
  }
}

/** 取消向导 */
function handleCancel() {
  visible.value = false
}

defineExpose({ open })
</script>
