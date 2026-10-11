<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/statistics/logging/oper-log/components -->
<!-- 文件名称：OperLogDetail.vue -->
<!-- 功能描述：操作日志只读详情展示 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div v-if="detail" class="oper-log-detail">
    <a-form layout="horizontal" label-align="right">
      <a-tabs v-model:active-key="activeTab" class="oper-log-detail-tabs">
        <a-tab-pane key="tab-0" :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'" force-render>
          <div class="takt-form-content-rows-10">
            <a-row :gutter="24">
              <a-col :span="24">
                <a-form-item :label="t('common.page.entity.tenantcode')">
                  <a-input :value="text(detail.tenantCode)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('common.page.entity.companycode')">
                  <a-input :value="text(detail.companyCode)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.operlog.username')">
                  <a-input :value="text(detail.userName)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.operlog.opermodule')">
                  <a-input :value="text(detail.operModule)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.operlog.opertype')">
                  <TaktConstTag category="operType" :value="detail.operType" />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.operlog.opermethod')">
                  <a-input :value="text(detail.operMethod)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.operlog.requestmethod')">
                  <a-input :value="text(detail.requestMethod)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.operlog.operurl')">
                  <a-input :value="text(detail.operUrl)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.operlog.requestparam')">
                  <a-textarea :value="text(detail.requestParam)" :rows="3" size="small" disabled />
                </a-form-item>
              </a-col>
            </a-row>
          </div>
        </a-tab-pane>
        <a-tab-pane key="tab-1" :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'" force-render>
          <div class="takt-form-content-rows-10">
            <a-row :gutter="24">
              <a-col :span="24">
                <a-form-item :label="t('entity.operlog.jsonresult')">
                  <a-textarea :value="text(detail.jsonResult)" :rows="3" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.operlog.operstatus')">
                  <a-input :value="text(detail.operStatus)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.operlog.errormsg')">
                  <a-textarea :value="text(detail.errorMsg)" :rows="2" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.operlog.operip')">
                  <a-input :value="text(detail.operIp)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.operlog.operlocation')">
                  <a-input :value="text(detail.operLocation)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.operlog.opertime')">
                  <a-input :value="text(detail.operTime)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.operlog.elapsedtime')">
                  <a-input :value="text(detail.elapsedTime)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('common.page.entity.createdat')">
                  <a-input :value="text(detail.createdAt)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('common.page.entity.extfield')">
                  <a-textarea :value="text(detail.ExtField)" :rows="2" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('common.page.entity.remark')">
                  <a-textarea :value="text(detail.remark)" :rows="2" size="small" disabled />
                </a-form-item>
              </a-col>
            </a-row>
          </div>
        </a-tab-pane>
      </a-tabs>
    </a-form>
  </div>
</template>

<script setup lang="ts">
/**
 * 操作日志只读详情
 * @module views/statistics/logging/oper-log/components
 */
import { useI18n } from 'vue-i18n'
import type { OperLog } from '@/types/statistics/logging/operLog.types'

/** i18n */
const { t } = useI18n()

defineProps<{
  detail?: OperLog | null
}>()

/** 当前 Tab */
const activeTab = ref('tab-0')

/**
 * 只读展示文本
 * @param value 字段值
 * @returns {string} 展示字符串
 */
function text(value: unknown): string {
  if (value == null || value === '') {
    return ''
  }
  return String(value)
}
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}
:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>
