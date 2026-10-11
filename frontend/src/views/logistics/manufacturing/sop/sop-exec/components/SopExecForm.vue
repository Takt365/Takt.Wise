<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/sop/sop-exec/components -->
<!-- 文件名称：SopExecForm.vue -->
<!-- 功能描述：SOP 工位执行追溯实体维护弹窗内嵌表单。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
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
    <a-tabs
      v-model:active-key="activeTab"
      class="sop-exec-form-tabs"
    >
      <!-- 主表 -->
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
              <a-col :span="12">
                <a-form-item
                  :label="t('common.page.entity.culturecode')"
                  name="cultureCode"
                >
                  <a-input
                    v-model:value="formState.cultureCode"
                    disabled
                    :placeholder="t('common.page.form.placeholder.input')"
                  />
                </a-form-item>
              </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.productionorderid')"
                name="productionOrderId"
              >
                <a-input
                  v-model:value="formState.productionOrderId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.productionorderid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.workorderCode')"
                name="workOrderCode"
              >
                <a-input
                  v-model:value="formState.workOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.workorderCode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.serialnumber')"
                name="serialNumber"
              >
                <a-input
                  v-model:value="formState.serialNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.serialnumber') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.materialcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.sopExecId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.routingitemid')"
                name="routingItemId"
              >
                <a-input
                  v-model:value="formState.routingItemId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.routingitemid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.processsegmenttype')"
                name="processSegmentType"
              >
                <TaktSelect
                  v-model:value="formState.processSegmentType"
                  dict-type="logistics_manufacturing_process_segment_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopexec.processsegmenttype') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.workstationid')"
                name="workstationId"
              >
                <a-input
                  v-model:value="formState.workstationId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.workstationid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.employeeid')"
                name="employeeId"
              >
                <a-input
                  v-model:value="formState.employeeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.employeeid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.sopid')"
                name="sopId"
              >
                <a-input
                  v-model:value="formState.sopId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.sopid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.revisionid')"
                name="revisionId"
              >
                <a-input
                  v-model:value="formState.revisionId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.revisionid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.revision')"
                name="revision"
              >
                <a-input
                  v-model:value="formState.revision"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.revision') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.startedat')"
                name="startedAt"
              >
                <a-input
                  v-model:value="formState.startedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.startedat') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.endedat')"
                name="endedAt"
              >
                <a-input
                  v-model:value="formState.endedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.endedat') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.selfcheckresult')"
                name="selfCheckResult"
              >
                <TaktSelect
                  v-model:value="formState.selfCheckResult"
                  dict-type="logistics_manufacturing_sop_check_result"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopexec.selfcheckresult') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.execstatus')"
                name="execStatus"
              >
                <TaktSelect
                  v-model:value="formState.execStatus"
                  dict-type="logistics_manufacturing_sop_exec_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopexec.execstatus') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.currentstepid')"
                name="currentStepId"
              >
                <a-input
                  v-model:value="formState.currentStepId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.currentstepid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                name="extField"
                class="takt-form-item-ext-field"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t('common.page.entity.extfieldhint')"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ t('common.page.entity.extfield') }}</span>
                  </span>
                </template>
                <a-textarea
                  v-model:value="formState.extField"
                  :placeholder="t('common.page.form.placeholder.extfield')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('common.page.entity.remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <!-- 子表：sopExecStep -->
      <a-tab-pane
        key="child-steps"
        :tab="t('entity.sopexecstep._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddSopExecStepRow">
            {{ t('common.page.button.create') }}{{ t('entity.sopexecstep._self') }}
          </a-button>
        </div>
        <a-table
          :columns="sopExecStepFormColumns"
          :data-source="childSopExecStepRows"
          :pagination="false"
          :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          size="small"
          bordered
        >
          <template #bodyCell="{ column, record, index }">
            <template v-if="column.key === 'tenantCode'">
              <a-input
                v-model:value="record.tenantCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                show-count
                :maxlength="20"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyCode'">
              <a-input
                v-model:value="record.companyCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                show-count
                :maxlength="20"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyDefaultCulture'">
              <a-input
                v-model:value="record.companyDefaultCulture"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                show-count
                :maxlength="20"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'execId'">
              <a-input
                v-model:value="record.execId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecstep.execid') })"
                show-count
                :maxlength="20"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'stepId'">
              <a-input
                v-model:value="record.stepId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecstep.stepid') })"
                show-count
                :maxlength="20"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'stepNo'">
              <a-input-number
                v-model:value="record.stepNo"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecstep.stepno') })"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'startedAt'">
              <a-input
                v-model:value="record.startedAt"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecstep.startedat') })"
                show-count
                :maxlength="20"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'endedAt'">
              <a-input
                v-model:value="record.endedAt"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecstep.endedat') })"
                show-count
                :maxlength="20"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'stepResult'">
              <TaktSelect
                v-model:value="record.stepResult"
                dict-type="logistics_manufacturing_sop_check_result"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopexecstep.stepresult') })"
              />
            </template>
            <template v-else-if="column.key === 'confirmedBy'">
              <a-input
                v-model:value="record.confirmedBy"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecstep.confirmedby') })"
                show-count
                :maxlength="20"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'confirmedAt'">
              <a-input
                v-model:value="record.confirmedAt"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecstep.confirmedat') })"
                show-count
                :maxlength="20"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveSopExecStepRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：sopExecScan -->
      <a-tab-pane
        key="child-scans"
        :tab="t('entity.sopexecscan._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddSopExecScanRow">
            {{ t('common.page.button.create') }}{{ t('entity.sopexecscan._self') }}
          </a-button>
        </div>
        <a-table
          :columns="sopExecScanFormColumns"
          :data-source="childSopExecScanRows"
          :pagination="false"
          :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          size="small"
          bordered
        >
          <template #bodyCell="{ column, record, index }">
            <template v-if="column.key === 'tenantCode'">
              <a-input
                v-model:value="record.tenantCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                show-count
                :maxlength="20"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyCode'">
              <a-input
                v-model:value="record.companyCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                show-count
                :maxlength="20"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyDefaultCulture'">
              <a-input
                v-model:value="record.companyDefaultCulture"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                show-count
                :maxlength="20"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'execId'">
              <a-input
                v-model:value="record.execId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecscan.execid') })"
                show-count
                :maxlength="20"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'execStepId'">
              <a-input
                v-model:value="record.execStepId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecscan.execstepid') })"
                show-count
                :maxlength="20"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'stepId'">
              <a-input
                v-model:value="record.stepId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecscan.stepid') })"
                show-count
                :maxlength="20"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'scannedBarcode'">
              <a-input
                v-model:value="record.scannedBarcode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecscan.scannedbarcode') })"
                show-count
                :maxlength="20"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'expectedMaterialCode'">
              <a-input
                v-model:value="record.expectedMaterialCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecscan.expectedmaterialcode') })"
                show-count
                :maxlength="20"
                allow-clear
                :disabled="!!record.sopExecScanId"
              />
            </template>
            <template v-else-if="column.key === 'scanResult'">
              <TaktSelect
                v-model:value="record.scanResult"
                dict-type="logistics_manufacturing_sop_scan_result"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopexecscan.scanresult') })"
              />
            </template>
            <template v-else-if="column.key === 'matchMessage'">
              <a-input
                v-model:value="record.matchMessage"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecscan.matchmessage') })"
                show-count
                :maxlength="20"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'scannedAt'">
              <a-input
                v-model:value="record.scannedAt"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexecscan.scannedat') })"
                show-count
                :maxlength="20"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveSopExecScanRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
      <!-- 子表：sopArgument -->
      <a-tab-pane
        key="child-arguments"
        :tab="t('entity.sopargument._self')"
        force-render
      >
        <div class="mb-2">
          <a-button type="primary" size="small" @click="handleAddSopArgumentRow">
            {{ t('common.page.button.create') }}{{ t('entity.sopargument._self') }}
          </a-button>
        </div>
        <a-table
          :columns="sopArgumentFormColumns"
          :data-source="childSopArgumentRows"
          :pagination="false"
          :row-key="(row: Record<string, unknown>, index?: number) => String(row.__rowKey ?? index ?? 0)"
          size="small"
          bordered
        >
          <template #bodyCell="{ column, record, index }">
            <template v-if="column.key === 'tenantCode'">
              <a-input
                v-model:value="record.tenantCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                show-count
                :maxlength="20"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyCode'">
              <a-input
                v-model:value="record.companyCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                show-count
                :maxlength="20"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'companyDefaultCulture'">
              <a-input
                v-model:value="record.companyDefaultCulture"
                :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                show-count
                :maxlength="20"
                readonly
              />
            </template>
            <template v-else-if="column.key === 'execId'">
              <a-input
                v-model:value="record.execId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopargument.execid') })"
                show-count
                :maxlength="20"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'execStepId'">
              <a-input
                v-model:value="record.execStepId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopargument.execstepid') })"
                show-count
                :maxlength="20"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'routingItemParameterId'">
              <a-input
                v-model:value="record.routingItemParameterId"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopargument.routingitemparameterid') })"
                show-count
                :maxlength="20"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'paramCode'">
              <a-input
                v-model:value="record.paramCode"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopargument.paramcode') })"
                show-count
                :maxlength="20"
                allow-clear
                :disabled="!!record.sopArgumentId"
              />
            </template>
            <template v-else-if="column.key === 'actualValue'">
              <a-input-number
                v-model:value="record.actualValue"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopargument.actualvalue') })"
                style="width: 100%"
              />
            </template>
            <template v-else-if="column.key === 'isOutOfRange'">
              <TaktSelect
                v-model:value="record.isOutOfRange"
                dict-type="sys_yes_no"
                :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopargument.isoutofrange') })"
              />
            </template>
            <template v-else-if="column.key === 'recordedAt'">
              <a-input
                v-model:value="record.recordedAt"
                :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopargument.recordedat') })"
                show-count
                :maxlength="20"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === 'extField'">
              <a-textarea
                v-model:value="record.extField"
                :placeholder="t('common.page.form.placeholder.extfield')"
                :rows="4"
                show-count
                :maxlength="400"
                allow-clear
              />
            </template>
            <template v-else-if="column.key === '__action'">
              <a-button type="link" danger size="small" @click="handleRemoveSopArgumentRow(index)">
                {{ t('common.page.button.delete') }}
              </a-button>
            </template>
          </template>
        </a-table>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * SOP 工位执行追溯实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/sop/sop-exec/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SopExecCreate, SopExecStepCreate, SopExecStep, SopExecScanCreate, SopExecScan, SopArgumentCreate, SopArgument } from '@/types/logistics/manufacturing/sop/sopExec.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或公司切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (formFields.includes('tenantCode') && (force || !target.tenantCode)) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (formFields.includes('companyCode') && (force || !target.companyCode)) {
    target.companyCode = tenantStore.companyCode
  }
  if (formFields.includes('cultureCode') && (force || !target.cultureCode)) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }

}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","productionOrderId","workOrderCode","serialNumber","materialCode","routingItemId","processSegmentType","workstationId","employeeId","sopId","revisionId","revision","startedAt","endedAt","selfCheckResult","execStatus","currentStepId","extField","remark"]

/** sopExecStep 子表行（表单 Tab 内嵌） */
const childSopExecStepRows = ref<Record<string, unknown>[]>([])
/** sopExecScan 子表行（表单 Tab 内嵌） */
const childSopExecScanRows = ref<Record<string, unknown>[]>([])
/** sopArgument 子表行（表单 Tab 内嵌） */
const childSopArgumentRows = ref<Record<string, unknown>[]>([])

/** 子表 sopExecStep 表单列定义 */
const sopExecStepFormColumns = computed(() => [
  {
    title: t('common.page.entity.tenantcode'),
    dataIndex: 'tenantCode',
    key: 'tenantCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companycode'),
    dataIndex: 'companyCode',
    key: 'companyCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companydefaultculture'),
    dataIndex: 'companyDefaultCulture',
    key: 'companyDefaultCulture',
    width: 140,
  },
  {
    title: t('entity.sopexecstep.execid'),
    dataIndex: 'execId',
    key: 'execId',
    width: 140,
  },
  {
    title: t('entity.sopexecstep.stepid'),
    dataIndex: 'stepId',
    key: 'stepId',
    width: 140,
  },
  {
    title: t('entity.sopexecstep.stepno'),
    dataIndex: 'stepNo',
    key: 'stepNo',
    width: 140,
  },
  {
    title: t('entity.sopexecstep.startedat'),
    dataIndex: 'startedAt',
    key: 'startedAt',
    width: 140,
  },
  {
    title: t('entity.sopexecstep.endedat'),
    dataIndex: 'endedAt',
    key: 'endedAt',
    width: 140,
  },
  {
    title: t('entity.sopexecstep.stepresult'),
    dataIndex: 'stepResult',
    key: 'stepResult',
    width: 140,
  },
  {
    title: t('entity.sopexecstep.confirmedby'),
    dataIndex: 'confirmedBy',
    key: 'confirmedBy',
    width: 140,
  },
  {
    title: t('entity.sopexecstep.confirmedat'),
    dataIndex: 'confirmedAt',
    key: 'confirmedAt',
    width: 140,
  },
  {
    title: t('common.action.operation'),
    key: '__action',
    width: 80,
    fixed: 'right',
  }])

/** 子表 sopExecScan 表单列定义 */
const sopExecScanFormColumns = computed(() => [
  {
    title: t('common.page.entity.tenantcode'),
    dataIndex: 'tenantCode',
    key: 'tenantCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companycode'),
    dataIndex: 'companyCode',
    key: 'companyCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companydefaultculture'),
    dataIndex: 'companyDefaultCulture',
    key: 'companyDefaultCulture',
    width: 140,
  },
  {
    title: t('entity.sopexecscan.execid'),
    dataIndex: 'execId',
    key: 'execId',
    width: 140,
  },
  {
    title: t('entity.sopexecscan.execstepid'),
    dataIndex: 'execStepId',
    key: 'execStepId',
    width: 140,
  },
  {
    title: t('entity.sopexecscan.stepid'),
    dataIndex: 'stepId',
    key: 'stepId',
    width: 140,
  },
  {
    title: t('entity.sopexecscan.scannedbarcode'),
    dataIndex: 'scannedBarcode',
    key: 'scannedBarcode',
    width: 140,
  },
  {
    title: t('entity.sopexecscan.expectedmaterialcode'),
    dataIndex: 'expectedMaterialCode',
    key: 'expectedMaterialCode',
    width: 140,
  },
  {
    title: t('entity.sopexecscan.scanresult'),
    dataIndex: 'scanResult',
    key: 'scanResult',
    width: 140,
  },
  {
    title: t('entity.sopexecscan.matchmessage'),
    dataIndex: 'matchMessage',
    key: 'matchMessage',
    width: 140,
  },
  {
    title: t('entity.sopexecscan.scannedat'),
    dataIndex: 'scannedAt',
    key: 'scannedAt',
    width: 140,
  },
  {
    title: t('common.action.operation'),
    key: '__action',
    width: 80,
    fixed: 'right',
  }])

/** 子表 sopArgument 表单列定义 */
const sopArgumentFormColumns = computed(() => [
  {
    title: t('common.page.entity.tenantcode'),
    dataIndex: 'tenantCode',
    key: 'tenantCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companycode'),
    dataIndex: 'companyCode',
    key: 'companyCode',
    width: 140,
  },
  {
    title: t('common.page.entity.companydefaultculture'),
    dataIndex: 'companyDefaultCulture',
    key: 'companyDefaultCulture',
    width: 140,
  },
  {
    title: t('entity.sopargument.execid'),
    dataIndex: 'execId',
    key: 'execId',
    width: 140,
  },
  {
    title: t('entity.sopargument.execstepid'),
    dataIndex: 'execStepId',
    key: 'execStepId',
    width: 140,
  },
  {
    title: t('entity.sopargument.routingitemparameterid'),
    dataIndex: 'routingItemParameterId',
    key: 'routingItemParameterId',
    width: 140,
  },
  {
    title: t('entity.sopargument.paramcode'),
    dataIndex: 'paramCode',
    key: 'paramCode',
    width: 140,
  },
  {
    title: t('entity.sopargument.actualvalue'),
    dataIndex: 'actualValue',
    key: 'actualValue',
    width: 140,
  },
  {
    title: t('entity.sopargument.isoutofrange'),
    dataIndex: 'isOutOfRange',
    key: 'isOutOfRange',
    width: 140,
  },
  {
    title: t('entity.sopargument.recordedat'),
    dataIndex: 'recordedAt',
    key: 'recordedAt',
    width: 140,
  },
  {
    title: t('common.page.entity.extfield'),
    dataIndex: 'extField',
    key: 'extField',
    width: 140,
  },
  {
    title: t('common.action.operation'),
    key: '__action',
    width: 80,
    fixed: 'right',
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SopExecCreate & { sopExecId?: string }> | null | undefined) {
  childSopExecStepRows.value = ((val as any)?.steps ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.sopExecStepId ?? `new-${index}`,
  }))
  childSopExecScanRows.value = ((val as any)?.scans ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.sopExecScanId ?? `new-${index}`,
  }))
  childSopArgumentRows.value = ((val as any)?.arguments ?? []).map((item: Record<string, unknown>, index: number) => ({
    ...item,
    __rowKey: item.sopArgumentId ?? `new-${index}`,
  }))
}

/** 表单 Tab 内新增 sopExecStep 行 */
function handleAddSopExecStepRow() {
  childSopExecStepRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      execId: '',
      stepId: '',
      stepNo: 0,
      startedAt: '',
      endedAt: '',
      stepResult: 0,
      confirmedBy: '',
      confirmedAt: '',
  })
}

/** 表单 Tab 内删除 sopExecStep 行 */
function handleRemoveSopExecStepRow(index: number) {
  childSopExecStepRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 sopExecScan 行 */
function handleAddSopExecScanRow() {
  childSopExecScanRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      execId: '',
      execStepId: '',
      stepId: '',
      scannedBarcode: '',
      expectedMaterialCode: '',
      scanResult: 0,
      matchMessage: '',
      scannedAt: '',
  })
}

/** 表单 Tab 内删除 sopExecScan 行 */
function handleRemoveSopExecScanRow(index: number) {
  childSopExecScanRows.value.splice(index, 1)
}

/** 表单 Tab 内新增 sopArgument 行 */
function handleAddSopArgumentRow() {
  childSopArgumentRows.value.push({
    __rowKey: `new-${Date.now()}`,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      execId: '',
      execStepId: '',
      routingItemParameterId: '',
      paramCode: '',
      actualValue: 0,
      isOutOfRange: 0,
      recordedAt: '',
      extField: '',
  })
}

/** 表单 Tab 内删除 sopArgument 行 */
function handleRemoveSopArgumentRow(index: number) {
  childSopArgumentRows.value.splice(index, 1)
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  return {
    ...formState,
    steps: childSopExecStepRows.value.map(({ __rowKey, ...rest }) => rest),
    scans: childSopExecScanRows.value.map(({ __rowKey, ...rest }) => rest),
    arguments: childSopArgumentRows.value.map(({ __rowKey, ...rest }) => rest),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SopExecCreate & { sopExecId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 sopExecId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.sopExecId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).steps
    delete (next as any).scans
    delete (next as any).arguments
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.sopExecId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  workOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.workorderCode') }),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.materialcode') }),
      trigger: 'blur'
    }
  ],
  routingItemId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.routingitemid') }),
      trigger: 'blur'
    }
  ],
  processSegmentType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sopexec.processsegmenttype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sopexec.processsegmenttype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  workstationId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.workstationid') }),
      trigger: 'blur'
    }
  ],
  employeeId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.employeeid') }),
      trigger: 'blur'
    }
  ],
  sopId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.sopid') }),
      trigger: 'blur'
    }
  ],
  revisionId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.revisionid') }),
      trigger: 'blur'
    }
  ],
  revision: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.revision') }),
      trigger: 'blur'
    }
  ],
  cultureCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.culturecode') }),
      trigger: 'blur'
    }
  ],
  startedAt: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.startedat') }),
      trigger: 'blur'
    }
  ],
  execStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sopexec.execstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sopexec.execstatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload()
  if ('processSegmentType' in payload) {
    const rawprocessSegmentType = payload.processSegmentType
    payload.processSegmentType = typeof rawprocessSegmentType === 'number' ? rawprocessSegmentType : Number(rawprocessSegmentType)
  }
  if ('selfCheckResult' in payload) {
    const rawselfCheckResult = payload.selfCheckResult
    payload.selfCheckResult = typeof rawselfCheckResult === 'number' ? rawselfCheckResult : Number(rawselfCheckResult)
  }
  if ('execStatus' in payload) {
    const rawexecStatus = payload.execStatus
    payload.execStatus = typeof rawexecStatus === 'number' ? rawexecStatus : Number(rawexecStatus)
  }
  delete payload.sortOrder
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  childSopExecStepRows.value = []
  childSopExecScanRows.value = []
  childSopArgumentRows.value = []
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>
