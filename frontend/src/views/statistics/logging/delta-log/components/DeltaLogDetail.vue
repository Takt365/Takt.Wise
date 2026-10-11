<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/statistics/logging/delta-log/components -->
<!-- 文件名称：DeltaLogDetail.vue -->
<!-- 功能描述：差异日志只读详情展示 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div v-if="detail" class="delta-log-detail">
    <a-form layout="horizontal" label-align="right">
      <a-tabs v-model:active-key="activeTab" class="delta-log-detail-tabs">
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
                <a-form-item :label="t('entity.deltalog.username')">
                  <a-input :value="text(detail.userName)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.deltalog.opertype')">
                  <TaktConstTag category="operType" :value="detail.operType" />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.deltalog.tablename')">
                  <a-input :value="text(detail.tableName)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.deltalog.primarykeyid')">
                  <a-input :value="text(detail.primaryKeyId)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.deltalog.beforedata')">
                  <a-textarea :value="text(detail.beforeData)" :rows="3" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.deltalog.afterdata')">
                  <a-textarea :value="text(detail.afterData)" :rows="3" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.deltalog.diffdata')">
                  <a-textarea :value="text(detail.diffData)" :rows="3" size="small" disabled />
                </a-form-item>
              </a-col>
            </a-row>
          </div>
        </a-tab-pane>
        <a-tab-pane key="tab-1" :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'" force-render>
          <div class="takt-form-content-rows-10">
            <a-row :gutter="24">
              <a-col :span="24">
                <a-form-item :label="t('entity.deltalog.sqlstatement')">
                  <a-textarea :value="text(detail.sqlStatement)" :rows="3" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.deltalog.operip')">
                  <a-input :value="text(detail.operIp)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.deltalog.operlocation')">
                  <a-input :value="text(detail.operLocation)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.deltalog.opertime')">
                  <a-input :value="text(detail.operTime)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.deltalog.elapsedtime')">
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
 * 差异日志只读详情
 * @module views/statistics/logging/delta-log/components
 */
import { useI18n } from 'vue-i18n'
import type { DeltaLog } from '@/types/statistics/logging/deltaLog.types'

/** i18n */
const { t } = useI18n()

defineProps<{
  detail?: DeltaLog | null
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
