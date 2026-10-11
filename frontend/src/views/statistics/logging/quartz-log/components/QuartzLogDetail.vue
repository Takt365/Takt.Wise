<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/statistics/logging/quartz-log/components -->
<!-- 文件名称：QuartzLogDetail.vue -->
<!-- 功能描述：调度日志只读详情展示 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div v-if="detail" class="quartz-log-detail">
    <a-form layout="horizontal" label-align="right">
      <a-tabs v-model:active-key="activeTab" class="quartz-log-detail-tabs">
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
                <a-form-item :label="t('entity.quartzlog.quartztaskid')">
                  <a-input :value="text(detail.quartzTaskId)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.quartzlog.taskname')">
                  <a-input :value="text(detail.taskName)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.quartzlog.jobgroup')">
                  <TaktDictTag dict-type="sys_quartz_job_group" :value="detail.jobGroup" />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.quartzlog.tasktype')">
                  <TaktDictTag dict-type="sys_quartz_task_type" :value="detail.taskType" />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.quartzlog.executetime')">
                  <a-input :value="text(detail.executeTime)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.quartzlog.executeduration')">
                  <a-input :value="text(detail.executeDuration)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.quartzlog.executeparams')">
                  <a-textarea :value="text(detail.executeParams)" :rows="3" size="small" disabled />
                </a-form-item>
              </a-col>
            </a-row>
          </div>
        </a-tab-pane>
        <a-tab-pane key="tab-1" :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'" force-render>
          <div class="takt-form-content-rows-10">
            <a-row :gutter="24">
              <a-col :span="24">
                <a-form-item :label="t('entity.quartzlog.executemessage')">
                  <a-textarea :value="text(detail.executeMessage)" :rows="3" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.quartzlog.errorinfo')">
                  <a-textarea :value="text(detail.errorInfo)" :rows="3" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.quartzlog.executeip')">
                  <a-input :value="text(detail.executeIp)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.quartzlog.executehost')">
                  <a-input :value="text(detail.executeHost)" size="small" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('entity.quartzlog.executestatus')">
                  <a-input :value="text(detail.executeStatus)" size="small" disabled />
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
 * 调度日志只读详情
 * @module views/statistics/logging/quartz-log/components
 */
import { useI18n } from 'vue-i18n'
import type { QuartzLog } from '@/types/statistics/logging/quartzLog.types'

/** i18n */
const { t } = useI18n()

defineProps<{
  detail?: QuartzLog | null
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
