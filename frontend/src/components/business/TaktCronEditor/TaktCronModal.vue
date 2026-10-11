<!-- ======================================== -->
<!-- 项目名称：TaktWise -->
<!-- 命名空间：@/components/business/TaktCronEditor -->
<!-- 文件名称：TaktCronModal.vue -->
<!-- 创建时间：2026-06-28 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：Quartz Cron 可视化弹窗（Tab + 表达式预览 + 最近 5 次运行时间，参照博客园自定义 cron 组件） -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-modal
    v-model:open="internalOpen"
    :title="t('foundation.quartz-task.page.cron.modalTitle')"
    :width="820"
    :z-index="1100"
    :get-container="getCronModalContainer"
    wrap-class-name="takt-cron-modal-wrap"
    :destroy-on-close="false"
    :mask-closable="false"
    class="takt-cron-modal"
    @cancel="handleCancel"
  >
    <div class="takt-cron-modal-body border border-border rounded bg-container">
      <div
        v-if="sourceExpression"
        class="mx-4 mt-4 mb-2 px-3 py-2 rounded bg-page border border-border text-sm"
      >
        <div class="text-text-secondary mb-1">
          {{ t('foundation.quartz-task.page.cron.sourceExpression') }}
        </div>
        <div class="font-mono text-text break-all mb-2">
          {{ sourceExpression }}
        </div>
        <div class="text-text-secondary mb-1">
          {{ t('foundation.quartz-task.page.cron.sourceMeaning') }}
        </div>
        <div class="text-text">
          {{ sourceDescription }}
        </div>
      </div>
      <a-tabs
        v-model:active-key="activeTabKey"
        type="card"
        class="takt-cron-tabs"
      >
        <!-- 秒 -->
        <a-tab-pane
          key="second"
          :tab="t('foundation.quartz-task.page.cron.tab.second')"
        >
          <a-radio-group
            v-model:value="editorState.second.cronEvery"
            class="flex flex-col gap-3 w-full"
          >
            <a-radio value="1">
              {{ t('foundation.quartz-task.page.cron.wildcard', { unit: t('foundation.quartz-task.page.cron.field.second') }) }}
            </a-radio>
            <a-radio value="4">
              <span class="inline-flex flex-wrap items-center gap-2">
                <span>{{ t('foundation.quartz-task.page.cron.rangeFrom') }}</span>
                <a-input-number
                  v-model:value="editorState.second.rangeStart"
                  :min="0"
                  :max="59"
                  class="!w-20"
                />
                <span>-</span>
                <a-input-number
                  v-model:value="editorState.second.rangeEnd"
                  :min="0"
                  :max="59"
                  class="!w-20"
                />
                <span>{{ t('foundation.quartz-task.page.cron.field.second') }}</span>
              </span>
            </a-radio>
            <a-radio value="2">
              <span class="inline-flex flex-wrap items-center gap-2">
                <span>{{ t('foundation.quartz-task.page.cron.intervalFrom') }}</span>
                <a-input-number
                  v-model:value="editorState.second.incrementStart"
                  :min="0"
                  :max="59"
                  class="!w-20"
                />
                <span>{{ t('foundation.quartz-task.page.cron.field.second') }}{{ t('foundation.quartz-task.page.cron.intervalEvery') }}</span>
                <a-input-number
                  v-model:value="editorState.second.incrementIncrement"
                  :min="1"
                  :max="59"
                  class="!w-20"
                />
                <span>{{ t('foundation.quartz-task.page.cron.field.second') }}{{ t('foundation.quartz-task.page.cron.intervalExecute') }}</span>
              </span>
            </a-radio>
            <a-radio value="3">
              <div class="flex flex-col gap-2 w-full">
                <span>{{ t('foundation.quartz-task.page.cron.specify') }}</span>
                <a-select
                  v-model:value="editorState.second.specificSpecific"
                  mode="multiple"
                  class="!w-full max-w-xl"
                  :placeholder="t('foundation.quartz-task.page.cron.selectPlaceholder')"
                  :options="secondOptions"
                  :get-popup-container="getSelectPopupContainer"
                />
              </div>
            </a-radio>
          </a-radio-group>
        </a-tab-pane>
        <!-- 分钟 -->
        <a-tab-pane
          key="minute"
          :tab="t('foundation.quartz-task.page.cron.tab.minute')"
        >
          <a-radio-group
            v-model:value="editorState.minute.cronEvery"
            class="flex flex-col gap-3 w-full"
          >
            <a-radio value="1">
              {{ t('foundation.quartz-task.page.cron.wildcard', { unit: t('foundation.quartz-task.page.cron.field.minute') }) }}
            </a-radio>
            <a-radio value="4">
              <span class="inline-flex flex-wrap items-center gap-2">
                <span>{{ t('foundation.quartz-task.page.cron.rangeFrom') }}</span>
                <a-input-number
                  v-model:value="editorState.minute.rangeStart"
                  :min="0"
                  :max="59"
                  class="!w-20"
                />
                <span>-</span>
                <a-input-number
                  v-model:value="editorState.minute.rangeEnd"
                  :min="0"
                  :max="59"
                  class="!w-20"
                />
                <span>{{ t('foundation.quartz-task.page.cron.field.minute') }}</span>
              </span>
            </a-radio>
            <a-radio value="2">
              <span class="inline-flex flex-wrap items-center gap-2">
                <span>{{ t('foundation.quartz-task.page.cron.intervalFrom') }}</span>
                <a-input-number
                  v-model:value="editorState.minute.incrementStart"
                  :min="0"
                  :max="59"
                  class="!w-20"
                />
                <span>{{ t('foundation.quartz-task.page.cron.field.minute') }}{{ t('foundation.quartz-task.page.cron.intervalEvery') }}</span>
                <a-input-number
                  v-model:value="editorState.minute.incrementIncrement"
                  :min="1"
                  :max="59"
                  class="!w-20"
                />
                <span>{{ t('foundation.quartz-task.page.cron.field.minute') }}{{ t('foundation.quartz-task.page.cron.intervalExecute') }}</span>
              </span>
            </a-radio>
            <a-radio value="3">
              <div class="flex flex-col gap-2 w-full">
                <span>{{ t('foundation.quartz-task.page.cron.specify') }}</span>
                <a-select
                  v-model:value="editorState.minute.specificSpecific"
                  mode="multiple"
                  class="!w-full max-w-xl"
                  :placeholder="t('foundation.quartz-task.page.cron.selectPlaceholder')"
                  :options="minuteOptions"
                  :get-popup-container="getSelectPopupContainer"
                />
              </div>
            </a-radio>
          </a-radio-group>
        </a-tab-pane>
        <!-- 小时 -->
        <a-tab-pane
          key="hour"
          :tab="t('foundation.quartz-task.page.cron.tab.hour')"
        >
          <a-radio-group
            v-model:value="editorState.hour.cronEvery"
            class="flex flex-col gap-3 w-full"
          >
            <a-radio value="1">
              {{ t('foundation.quartz-task.page.cron.wildcard', { unit: t('foundation.quartz-task.page.cron.field.hour') }) }}
            </a-radio>
            <a-radio value="4">
              <span class="inline-flex flex-wrap items-center gap-2">
                <span>{{ t('foundation.quartz-task.page.cron.rangeFrom') }}</span>
                <a-input-number
                  v-model:value="editorState.hour.rangeStart"
                  :min="0"
                  :max="23"
                  class="!w-20"
                />
                <span>-</span>
                <a-input-number
                  v-model:value="editorState.hour.rangeEnd"
                  :min="0"
                  :max="23"
                  class="!w-20"
                />
                <span>{{ t('foundation.quartz-task.page.cron.field.hour') }}</span>
              </span>
            </a-radio>
            <a-radio value="2">
              <span class="inline-flex flex-wrap items-center gap-2">
                <span>{{ t('foundation.quartz-task.page.cron.intervalFrom') }}</span>
                <a-input-number
                  v-model:value="editorState.hour.incrementStart"
                  :min="0"
                  :max="23"
                  class="!w-20"
                />
                <span>{{ t('foundation.quartz-task.page.cron.field.hour') }}{{ t('foundation.quartz-task.page.cron.intervalEvery') }}</span>
                <a-input-number
                  v-model:value="editorState.hour.incrementIncrement"
                  :min="1"
                  :max="23"
                  class="!w-20"
                />
                <span>{{ t('foundation.quartz-task.page.cron.field.hour') }}{{ t('foundation.quartz-task.page.cron.intervalExecute') }}</span>
              </span>
            </a-radio>
            <a-radio value="3">
              <div class="flex flex-col gap-2 w-full">
                <span>{{ t('foundation.quartz-task.page.cron.specify') }}</span>
                <a-select
                  v-model:value="editorState.hour.specificSpecific"
                  mode="multiple"
                  class="!w-full max-w-xl"
                  :placeholder="t('foundation.quartz-task.page.cron.selectPlaceholder')"
                  :options="hourOptions"
                  :get-popup-container="getSelectPopupContainer"
                />
              </div>
            </a-radio>
          </a-radio-group>
        </a-tab-pane>
        <!-- 日 -->
        <a-tab-pane
          key="day"
          :tab="t('foundation.quartz-task.page.cron.tab.day')"
        >
          <a-radio-group
            v-model:value="editorState.day.cronEvery"
            class="flex flex-col gap-3 w-full"
          >
            <a-radio value="1">{{ t('foundation.quartz-task.page.cron.everyDay') }}</a-radio>
            <a-radio value="3">
              <span class="inline-flex flex-wrap items-center gap-2">
                <a-input-number
                  v-model:value="editorState.day.incrementIncrement"
                  :min="1"
                  :max="31"
                  class="!w-20"
                />
                <span>{{ t('foundation.quartz-task.page.cron.field.day') }}</span>
                <a-input-number
                  v-model:value="editorState.day.incrementStart"
                  :min="1"
                  :max="31"
                  class="!w-20"
                />
              </span>
            </a-radio>
            <a-radio value="5">
              <div class="flex flex-col gap-2 w-full">
                <span>{{ t('foundation.quartz-task.page.cron.specificDay') }}</span>
                <a-select
                  v-model:value="editorState.day.specificSpecific"
                  mode="multiple"
                  class="!w-full max-w-xl"
                  :placeholder="t('foundation.quartz-task.page.cron.selectPlaceholder')"
                  :options="dayOfMonthOptions"
                  :get-popup-container="getSelectPopupContainer"
                />
              </div>
            </a-radio>
            <a-radio value="6">{{ t('foundation.quartz-task.page.cron.lastDayOfMonth') }}</a-radio>
            <a-radio value="7">{{ t('foundation.quartz-task.page.cron.lastWorkdayOfMonth') }}</a-radio>
            <a-radio value="8">
              <span class="inline-flex flex-wrap items-center gap-2">
                <a-select
                  v-model:value="editorState.day.cronLastSpecificDomDay"
                  class="!w-28"
                  :options="weekOptions"
                  :get-popup-container="getSelectPopupContainer"
                />
              </span>
            </a-radio>
            <a-radio value="9">
              <span class="inline-flex flex-wrap items-center gap-2">
                <a-input-number
                  v-model:value="editorState.day.cronDaysBeforeEomMinus"
                  :min="1"
                  :max="31"
                  class="!w-20"
                />
              </span>
            </a-radio>
            <a-radio value="10">
              <span class="inline-flex flex-wrap items-center gap-2">
                <a-input-number
                  v-model:value="editorState.day.cronDaysNearestWeekday"
                  :min="1"
                  :max="31"
                  class="!w-20"
                />
              </span>
            </a-radio>
          </a-radio-group>
        </a-tab-pane>
        <!-- 月 -->
        <a-tab-pane
          key="month"
          :tab="t('foundation.quartz-task.page.cron.tab.month')"
        >
          <a-radio-group
            v-model:value="editorState.month.cronEvery"
            class="flex flex-col gap-3 w-full"
          >
            <a-radio value="1">{{ t('foundation.quartz-task.page.cron.everyMonth') }}</a-radio>
            <a-radio value="2">
              <span class="inline-flex flex-wrap items-center gap-2">
                <a-input-number
                  v-model:value="editorState.month.incrementIncrement"
                  :min="1"
                  :max="12"
                  class="!w-20"
                />
                <a-input-number
                  v-model:value="editorState.month.incrementStart"
                  :min="1"
                  :max="12"
                  class="!w-20"
                />
              </span>
            </a-radio>
            <a-radio value="3">
              <div class="flex flex-col gap-2 w-full">
                <a-select
                  v-model:value="editorState.month.specificSpecific"
                  mode="multiple"
                  class="!w-full max-w-xl"
                  :placeholder="t('foundation.quartz-task.page.cron.selectPlaceholder')"
                  :options="monthOptions"
                />
              </div>
            </a-radio>
            <a-radio value="4">
              <span class="inline-flex flex-wrap items-center gap-2">
                <a-input-number
                  v-model:value="editorState.month.rangeStart"
                  :min="1"
                  :max="12"
                  class="!w-20"
                />
                <span>-</span>
                <a-input-number
                  v-model:value="editorState.month.rangeEnd"
                  :min="1"
                  :max="12"
                  class="!w-20"
                />
              </span>
            </a-radio>
          </a-radio-group>
        </a-tab-pane>
        <!-- 周 -->
        <a-tab-pane
          key="week"
          :tab="t('foundation.quartz-task.page.cron.tab.week')"
        >
          <a-radio-group
            v-model:value="editorState.day.cronEvery"
            class="flex flex-col gap-3 w-full"
          >
            <a-radio value="2">
              <span class="inline-flex flex-wrap items-center gap-2">
                <span>{{ t('foundation.quartz-task.page.cron.everyWeekInterval') }}</span>
                <a-input-number
                  v-model:value="editorState.week.incrementIncrement"
                  :min="1"
                  :max="7"
                  class="!w-20"
                />
                <a-select
                  v-model:value="editorState.week.incrementStart"
                  class="!w-28"
                  :options="weekOptions"
                  :get-popup-container="getSelectPopupContainer"
                />
              </span>
            </a-radio>
            <a-radio value="4">
              <div class="flex flex-col gap-2 w-full">
                <span>{{ t('foundation.quartz-task.page.cron.specificWeek') }}</span>
                <a-select
                  v-model:value="editorState.week.specificSpecific"
                  mode="multiple"
                  class="!w-full max-w-xl"
                  :placeholder="t('foundation.quartz-task.page.cron.selectPlaceholder')"
                  :options="weekOptions"
                  :get-popup-container="getSelectPopupContainer"
                />
              </div>
            </a-radio>
            <a-radio value="11">
              <span class="inline-flex flex-wrap items-center gap-2">
                <span>{{ t('foundation.quartz-task.page.cron.nthWeekday') }}</span>
                <a-input-number
                  v-model:value="editorState.week.cronNthDayNth"
                  :min="1"
                  :max="5"
                  class="!w-20"
                />
                <a-select
                  v-model:value="editorState.week.cronNthDayDay"
                  class="!w-28"
                  :options="weekOptions"
                  :get-popup-container="getSelectPopupContainer"
                />
              </span>
            </a-radio>
          </a-radio-group>
        </a-tab-pane>
        <!-- 年 -->
        <a-tab-pane
          key="year"
          :tab="t('foundation.quartz-task.page.cron.tab.year')"
        >
          <a-radio-group
            v-model:value="editorState.year.cronEvery"
            class="flex flex-col gap-3 w-full"
          >
            <a-radio value="1">{{ t('foundation.quartz-task.page.cron.everyYear') }}</a-radio>
            <a-radio value="2">
              <span class="inline-flex flex-wrap items-center gap-2">
                <a-input-number
                  v-model:value="editorState.year.incrementIncrement"
                  :min="1"
                  :max="99"
                  class="!w-20"
                />
                <a-input-number
                  v-model:value="editorState.year.incrementStart"
                  :min="yearStart"
                  :max="yearEnd"
                  class="!w-24"
                />
              </span>
            </a-radio>
            <a-radio value="3">
              <a-select
                v-model:value="editorState.year.specificSpecific"
                mode="multiple"
                class="!w-full max-w-xl"
                :placeholder="t('foundation.quartz-task.page.cron.selectPlaceholder')"
                :options="yearOptions"
              />
            </a-radio>
            <a-radio value="4">
              <span class="inline-flex flex-wrap items-center gap-2">
                <a-input-number
                  v-model:value="editorState.year.rangeStart"
                  :min="yearStart"
                  :max="yearEnd"
                  class="!w-24"
                />
                <span>-</span>
                <a-input-number
                  v-model:value="editorState.year.rangeEnd"
                  :min="yearStart"
                  :max="yearEnd"
                  class="!w-24"
                />
              </span>
            </a-radio>
          </a-radio-group>
        </a-tab-pane>
      </a-tabs>
      <div class="border-t border-border px-4 py-4 mt-2">
        <div class="flex flex-col lg:flex-row gap-4">
          <div class="flex-1 min-w-0">
            <div class="text-center text-sm font-medium text-text mb-3 pb-2 border-b border-border">
              {{ t('foundation.quartz-task.page.cron.expressionTitle') }}
            </div>
            <div class="grid grid-cols-7 gap-1 text-center text-xs">
              <div
                v-for="head in segmentHeaders"
                :key="head.key"
                class="py-2 bg-page border border-border font-medium"
              >
                {{ head.label }}
              </div>
              <div
                v-for="head in segmentHeaders"
                :key="`${head.key}-value`"
                class="py-3 bg-container border border-border font-mono text-base break-all min-h-10 flex items-center justify-center"
              >
                {{ head.key === 'year' ? ' ' : cronSegments[head.key as keyof typeof cronSegments] }}
              </div>
            </div>
          </div>
          <div class="w-full lg:w-52 shrink-0 flex flex-col">
            <div class="text-center text-sm font-medium text-text mb-3 pb-2 border-b border-border">
              {{ t('foundation.quartz-task.page.cron.fullExpression') }}
            </div>
            <div class="flex-1 border border-border bg-page px-3 py-4 font-mono text-sm break-all text-center flex items-center justify-center min-h-24">
              {{ cronExpression }}
            </div>
          </div>
        </div>
        <div class="mt-4 pt-3 border-t border-border">
          <div class="text-center text-sm font-medium text-text mb-2">
            {{ t('foundation.quartz-task.page.cron.nextRuns') }}
          </div>
          <ul
            v-if="nextRunTimes.length"
            class="list-none pl-0 text-sm space-y-1 m-0 text-center"
          >
            <li
              v-for="(run, idx) in nextRunTimes"
              :key="`${run}-${idx}`"
            >
              {{ run }}
            </li>
          </ul>
          <div
            v-else
            class="text-sm text-text-secondary text-center"
          >
            {{ t('foundation.quartz-task.page.cron.noNextRuns') }}
          </div>
        </div>
      </div>
    </div>
    <template #footer>
      <div class="flex justify-center gap-3">
        <a-button
          type="primary"
          @click="handleConfirm"
        >
          {{ t('foundation.quartz-task.page.cron.ok') }}
        </a-button>
        <a-button
          class="takt-button-reset"
          @click="handleReset"
        >
          {{ t('common.page.button.reset') }}
        </a-button>
        <a-button @click="handleCancel">
          {{ t('common.page.button.cancel') }}
        </a-button>
      </div>
    </template>
  </a-modal>
</template>

<script setup lang="ts">
/**
 * Quartz Cron 可视化配置弹窗
 * @module components/business/TaktCronEditor/takt-cron-modal
 */
import { computed, reactive, ref, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import {
  buildQuartzCronExpression,
  buildQuartzCronSegments,
  createDefaultQuartzCronEditorState,
  describeQuartzCronExpression,
  parseQuartzCronExpression,
  type QuartzCronEditorState,
} from '@/components/business/TaktCronEditor/quartzCronCore'
import { getQuartzCronNextRunTimes } from '@/components/business/TaktCronEditor/quartzCronNextRuns'

interface Props {
  /** 弹窗可见 */
  open?: boolean
  /** 当前 Cron 表达式 */
  expression?: string
}

interface Emits {
  (event: 'update:open', value: boolean): void
  (event: 'confirm', value: string): void
}

const props = withDefaults(defineProps<Props>(), {
  open: false,
  expression: '',
})

const emit = defineEmits<Emits>()

const { t } = useI18n()

/** 当前 Tab */
const activeTabKey = ref('second')
/** 打开弹窗时锁定的原表达式（说明用，编辑过程不随确认前变更覆盖） */
const sourceExpression = ref('')
/** 弹窗内编辑 state */
const editorState = reactive<QuartzCronEditorState>(createDefaultQuartzCronEditorState())

const yearStart = new Date().getFullYear()
const yearEnd = yearStart + 99

/**
 * Cron 弹窗挂载到 body，避免嵌套在任务表单 Modal 内被遮挡
 * @returns document.body
 */
function getCronModalContainer(): HTMLElement {
  return document.body
}

/**
 * Select 下拉挂载到 body，避免弹窗内裁剪
 * @param triggerNode 触发节点
 * @returns 挂载容器
 */
function getSelectPopupContainer(triggerNode: HTMLElement): HTMLElement {
  return triggerNode.parentElement ?? document.body
}

/** v-model:open 双向绑定 */
const internalOpen = computed({
  get: () => props.open,
  set: (value: boolean) => emit('update:open', value),
})

/** 0-59 选项 */
const secondOptions = computed(() => Array.from({ length: 60 }, (_, index) => ({ label: String(index), value: index })))
const minuteOptions = secondOptions
/** 0-23 选项 */
const hourOptions = computed(() => Array.from({ length: 24 }, (_, index) => ({ label: String(index), value: index })))
/** 1-31 日 */
const dayOfMonthOptions = computed(() => Array.from({ length: 31 }, (_, index) => ({ label: String(index + 1), value: index + 1 })))
/** 1-12 月 */
const monthOptions = computed(() => Array.from({ length: 12 }, (_, index) => ({ label: String(index + 1), value: index + 1 })))
/** 年份选项 */
const yearOptions = computed(() => Array.from({ length: 100 }, (_, index) => {
  const year = yearStart + index
  return { label: String(year), value: year }
}))

/** 星期选项（Quartz 1=SUN … 7=SAT，与博客园一致） */
const weekOptions = computed(() => [
  { label: t('foundation.quartz-task.page.cron.weekday.sun'), value: 1 },
  { label: t('foundation.quartz-task.page.cron.weekday.mon'), value: 2 },
  { label: t('foundation.quartz-task.page.cron.weekday.tue'), value: 3 },
  { label: t('foundation.quartz-task.page.cron.weekday.wed'), value: 4 },
  { label: t('foundation.quartz-task.page.cron.weekday.thu'), value: 5 },
  { label: t('foundation.quartz-task.page.cron.weekday.fri'), value: 6 },
  { label: t('foundation.quartz-task.page.cron.weekday.sat'), value: 7 },
])

/** 表达式分段表头 */
const segmentHeaders = computed(() => [
  { key: 'second' as const, label: t('foundation.quartz-task.page.cron.field.second') },
  { key: 'minute' as const, label: t('foundation.quartz-task.page.cron.field.minute') },
  { key: 'hour' as const, label: t('foundation.quartz-task.page.cron.field.hour') },
  { key: 'day' as const, label: t('foundation.quartz-task.page.cron.field.day') },
  { key: 'month' as const, label: t('foundation.quartz-task.page.cron.field.month') },
  { key: 'week' as const, label: t('foundation.quartz-task.page.cron.field.week') },
  { key: 'year' as const, label: t('foundation.quartz-task.page.cron.field.year') },
])

/** 当前六段 Cron 文本 */
const cronSegments = computed(() => buildQuartzCronSegments(editorState))
/** 完整 Cron 字符串 */
const cronExpression = computed(() => buildQuartzCronExpression(editorState))
/** 原表达式人类可读说明 */
const sourceDescription = computed(() => {
  const weekdayKeys = ['sun', 'mon', 'tue', 'wed', 'thu', 'fri', 'sat'] as const
  return describeQuartzCronExpression(sourceExpression.value, {
    everySecond: t('foundation.quartz-task.page.cron.everySecond'),
    everyMinute: t('foundation.quartz-task.page.cron.everyMinute'),
    everyHour: t('foundation.quartz-task.page.cron.everyHour'),
    everyDay: t('foundation.quartz-task.page.cron.everyDay'),
    everyMonth: t('foundation.quartz-task.page.cron.everyMonth'),
    atTime: (h, m, s) => t('foundation.quartz-task.page.cron.describe.atTime', { h, m, s }),
    intervalSeconds: (start, step) => t('foundation.quartz-task.page.cron.describe.intervalSeconds', { start, step }),
    intervalMinutes: (start, step) => t('foundation.quartz-task.page.cron.describe.intervalMinutes', { start, step }),
    intervalHours: (start, step) => t('foundation.quartz-task.page.cron.describe.intervalHours', { start, step }),
    specificSeconds: (values) => t('foundation.quartz-task.page.cron.describe.specificSeconds', { values }),
    specificMinutes: (values) => t('foundation.quartz-task.page.cron.describe.specificMinutes', { values }),
    specificHours: (values) => t('foundation.quartz-task.page.cron.describe.specificHours', { values }),
    specificDays: (values) => t('foundation.quartz-task.page.cron.describe.specificDays', { values }),
    specificMonths: (values) => t('foundation.quartz-task.page.cron.describe.specificMonths', { values }),
    specificWeeks: (values) => t('foundation.quartz-task.page.cron.describe.specificWeeks', { values }),
    weekday: (n) => {
      const idx = Math.min(7, Math.max(1, Math.trunc(n))) - 1
      return t(`foundation.quartz-task.page.cron.weekday.${weekdayKeys[idx]}`)
    },
    unknown: t('foundation.quartz-task.page.cron.describe.unknown'),
    join: t('foundation.quartz-task.page.cron.describe.join'),
  })
})
/** 最近 5 次运行时间（cron-parser 异步懒加载） */
const nextRunTimes = ref<string[]>([])

watch(
  [cronExpression, () => props.open],
  async ([expression, visible]) => {
    if (!visible) {
      nextRunTimes.value = []
      return
    }
    nextRunTimes.value = await getQuartzCronNextRunTimes(expression, 5)
  },
  { immediate: true },
)

/**
 * 将 props.expression 灌入 editorState，并记录原表达式说明
 * @param expression Cron 字符串
 */
function hydrateEditorState(expression: string) {
  const source = String(expression ?? '').trim()
  sourceExpression.value = source
  const parsed = parseQuartzCronExpression(source)
  Object.assign(editorState.second, parsed.second)
  Object.assign(editorState.minute, parsed.minute)
  Object.assign(editorState.hour, parsed.hour)
  Object.assign(editorState.day, parsed.day)
  Object.assign(editorState.week, parsed.week)
  Object.assign(editorState.month, parsed.month)
  Object.assign(editorState.year, parsed.year)
}

/** 打开弹窗时反解析表达式（含 v-if 首次挂载 open=true） */
watch(
  () => props.open,
  (visible) => {
    if (visible) {
      hydrateEditorState(props.expression)
      activeTabKey.value = 'second'
    }
  },
  { immediate: true },
)

/** 取消：关闭弹窗 */
function handleCancel() {
  emit('update:open', false)
}

/** 重置为默认 Tab 配置 */
function handleReset() {
  const defaults = createDefaultQuartzCronEditorState()
  Object.assign(editorState.second, defaults.second)
  Object.assign(editorState.minute, defaults.minute)
  Object.assign(editorState.hour, defaults.hour)
  Object.assign(editorState.day, defaults.day)
  Object.assign(editorState.week, defaults.week)
  Object.assign(editorState.month, defaults.month)
  Object.assign(editorState.year, defaults.year)
}

/** 确定：回写 Cron 并关闭 */
function handleConfirm() {
  emit('confirm', cronExpression.value)
  emit('update:open', false)
}
</script>
