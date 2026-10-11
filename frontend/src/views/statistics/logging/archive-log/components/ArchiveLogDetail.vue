<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/statistics/logging/archive-log/components -->
<!-- 文件名称：ArchiveLogDetail.vue -->
<!-- 功能描述：归档日志只读详情展示 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div v-if="detail" class="archive-log-detail">
    <a-form layout="horizontal" label-align="right">
      <a-tabs v-model:active-key="activeTab" class="archive-log-detail-tabs">
        <a-tab-pane key="tab-0" :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'" force-render>
          <div class="takt-form-content-rows-10">
            <a-row :gutter="24">
              <a-col :span="24">
                <a-form-item :label="t('common.page.entity.tenantcode')">
                  <a-input :value="text(detail.tenantCode)" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('common.page.entity.companycode')">
                  <a-input :value="text(detail.companyCode)" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="pi.label('archiveKind')">
                  <a-input :value="text(detail.archiveKind)" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="pi.label('sourceId')">
                  <a-input :value="text(detail.sourceId)" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="pi.label('sourceName')">
                  <a-input :value="text(detail.sourceName)" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="pi.label('targetName')">
                  <a-input :value="text(detail.targetName)" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="pi.label('archiveYear')">
                  <a-input :value="text(detail.archiveYear)" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="pi.label('sourceCount')">
                  <a-input :value="text(detail.sourceCount)" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="pi.label('archivedCount')">
                  <a-input :value="text(detail.archivedCount)" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="pi.label('deletedCount')">
                  <a-input :value="text(detail.deletedCount)" disabled />
                </a-form-item>
              </a-col>
            </a-row>
          </div>
        </a-tab-pane>
        <a-tab-pane key="tab-1" :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'" force-render>
          <div class="takt-form-content-rows-10">
            <a-row :gutter="24">
              <a-col :span="24">
                <a-form-item :label="pi.label('runStatus')">
                  <a-input :value="text(detail.runStatus)" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="pi.label('errorMessage')">
                  <a-textarea :value="text(detail.errorMessage)" :rows="3" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="pi.label('startedAt')">
                  <a-input :value="text(detail.startedAt)" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="pi.label('finishedAt')">
                  <a-input :value="text(detail.finishedAt)" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('common.page.entity.createdat')">
                  <a-input :value="text(detail.createdAt)" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('common.page.entity.extfield')">
                  <a-textarea :value="text(detail.extField)" :rows="2" disabled />
                </a-form-item>
              </a-col>
              <a-col :span="24">
                <a-form-item :label="t('common.page.entity.remark')">
                  <a-textarea :value="text(detail.remark)" :rows="2" disabled />
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
 * 归档日志只读详情
 * @module views/statistics/logging/archive-log/components
 */
import { useI18n } from 'vue-i18n'
import type { ArchiveLog } from '@/types/statistics/logging/archiveLog.types'
import { useArchiveLogI18n } from '../composables/useArchiveLogI18n'

/** 实体字段 i18n */
const pi = useArchiveLogI18n()
/** i18n */
const { t } = useI18n()

defineProps<{
  detail?: ArchiveLog | null
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
