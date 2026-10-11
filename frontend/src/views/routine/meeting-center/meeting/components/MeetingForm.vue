<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/routine/meeting-center/meeting/components -->
<!-- 文件名称：MeetingForm.vue -->
<!-- 功能描述：会议中心主实体维护弹窗内嵌表单（上主下从级联保存）；会议内容使用 takt-rich-editor。defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form meeting-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="meeting-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cultureCode')"
                name="cultureCode"
              >
                <TaktSelect
                  v-model:value="formState.cultureCode"
                  dict-type="sys_culture_code"
                  :placeholder="pi.ph('cultureCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.form.numberingRule')"
                name="numberingRuleCode"
              >
                <TaktSelect
                  v-model:value="formState.numberingRuleCode"
                  api-url="TaktNumberings/options"
                  :api-params="{ documentType: '会议' }"
                  :placeholder="t('common.page.form.placeholder.selectonly')"
                  :disabled="!!formData?.meetingId || loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('meetingCode')"
                name="meetingCode"
              >
                <a-input
                  v-model:value="formState.meetingCode"
                  :placeholder="t('common.page.form.numberingCodePreview')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('meetingTitle')"
                name="meetingTitle"
              >
                <a-input
                  v-model:value="formState.meetingTitle"
                  :placeholder="pi.ph('meetingTitle')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('meetingType')"
                name="meetingType"
              >
                <TaktSelect
                  v-model:value="formState.meetingType"
                  dict-type="routine_meeting_center_type"
                  :placeholder="pi.ph('meetingType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('meetingStatus')"
                name="meetingStatus"
              >
                <TaktSelect
                  v-model:value="formState.meetingStatus"
                  dict-type="routine_meeting_center_status"
                  :placeholder="pi.ph('meetingStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('startTime')"
                name="startTime"
              >
                <a-date-picker
                  v-model:value="formState.startTime"
                  :placeholder="pi.ph('startTime')"
                  show-time
                  value-format="YYYY-MM-DD HH:mm:ss"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('endTime')"
                name="endTime"
              >
                <a-date-picker
                  v-model:value="formState.endTime"
                  :placeholder="pi.ph('endTime')"
                  show-time
                  value-format="YYYY-MM-DD HH:mm:ss"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('location')"
                name="location"
              >
                <a-input
                  v-model:value="formState.location"
                  :placeholder="pi.ph('location')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('meetingLink')"
                name="meetingLink"
              >
                <a-input
                  v-model:value="formState.meetingLink"
                  :placeholder="pi.ph('meetingLink')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('meetingAgenda')"
                name="meetingAgenda"
              >
                <TaktRichEditor
                  v-model:value="formState.meetingAgenda"
                  :placeholder="pi.ph('meetingAgenda')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('meetingTags')"
                name="meetingTags"
              >
                <a-input
                  v-model:value="formState.meetingTags"
                  :placeholder="pi.ph('meetingTags')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('organizerId')"
                name="organizerId"
              >
                <TaktSelect
                  v-model:value="formState.organizerId"
                  api-url="TaktUsers/options"
                  :placeholder="pi.ph('organizerId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('organizerName')"
                name="organizerName"
              >
                <a-input
                  v-model:value="formState.organizerName"
                  :placeholder="pi.ph('organizerName')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('deptId')"
                name="deptId"
              >
                <TaktSelect
                  v-model:value="formState.deptId"
                  api-url="TaktDepts/tree-options"
                  :placeholder="pi.ph('deptId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('deptName')"
                name="deptName"
              >
                <a-input
                  v-model:value="formState.deptName"
                  :placeholder="pi.ph('deptName')"
                  show-count
                  :maxlength="100"
                  disabled
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('maxAttendees')"
                name="maxAttendees"
              >
                <a-input-number
                  v-model:value="formState.maxAttendees"
                  :placeholder="pi.ph('maxAttendees')"
                  :min="0"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('reminderMinutes')"
                name="reminderMinutes"
              >
                <a-input-number
                  v-model:value="formState.reminderMinutes"
                  :placeholder="pi.ph('reminderMinutes')"
                  :min="0"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('meetingRoomId')"
                name="meetingRoomId"
              >
                <TaktSelect
                  v-model:value="formState.meetingRoomId"
                  api-url="TaktMeetingRooms/options"
                  :placeholder="pi.ph('meetingRoomId')"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('meetingRoomName')"
                name="meetingRoomName"
              >
                <a-input
                  v-model:value="formState.meetingRoomName"
                  :placeholder="pi.ph('meetingRoomName')"
                  show-count
                  :maxlength="100"
                  disabled
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="pi.ph('tenantCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
              >
                <TaktSelect
                  v-model:value="formState.companyCode"
                  api-url="TaktCompanies/options"
                  :placeholder="pi.ph('companyCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
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
                    <span>{{ pi.label('extField') }}</span>
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
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
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
    </a-tabs>
    <TaktEditableTable
      ref="meetingAttendeeTableRef"
      v-model="childMeetingAttendeeRows"
      :columns="meetingAttendeeFormColumns"
      :title="meetingAttendeePi.self()"
      :add-button-entity="meetingAttendeePi.self()"
      id-field="meetingAttendeeId"
      :default-row="createDefaultMeetingAttendeeRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-userId="{ record }">
        <TaktSelect
          v-model:value="record.userId"
          api-url="TaktUsers/options"
          class="w-full"
          :get-popup-container="resolveSelectPopupContainer"
          :placeholder="meetingAttendeePi.ph('userId')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-attendeeRole="{ record }">
        <TaktSelect
          v-model:value="record.attendeeRole"
          dict-type="routine_meeting_center_attendee_role"
          class="w-full"
          :get-popup-container="resolveSelectPopupContainer"
          :placeholder="meetingAttendeePi.ph('attendeeRole')"
          :disabled="loading"
        />
      </template>
      <template #cell-attendanceStatus="{ record }">
        <TaktSelect
          v-model:value="record.attendanceStatus"
          dict-type="routine_meeting_center_attendance_status"
          class="w-full"
          :get-popup-container="resolveSelectPopupContainer"
          :placeholder="meetingAttendeePi.ph('attendanceStatus')"
          :disabled="loading"
        />
      </template>
      <template #cell-checkInMethod="{ record }">
        <TaktSelect
          v-model:value="record.checkInMethod"
          dict-type="routine_meeting_center_check_in_method"
          class="w-full"
          :get-popup-container="resolveSelectPopupContainer"
          :placeholder="meetingAttendeePi.ph('checkInMethod')"
          :disabled="loading"
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 会议中心主实体维护表单；会议内容 / 议程使用 takt-rich-editor
 * @module views/routine/meeting-center/meeting/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { RiQuestionLine } from '@remixicon/vue'
import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'
import { useTaktFormNumbering } from '@/composables/useTaktFormNumbering'
import { useMeetingI18n } from '../composables/useMeetingI18n'
import { useMeetingAttendeeI18n } from '../composables/useMeetingAttendeeI18n'
import type { Meeting } from '@/types/routine/meeting-center/meeting.types'

/** 实体字段 i18n */
const pi = useMeetingI18n()
/** 子表字段 i18n */
const meetingAttendeePi = useMeetingAttendeeI18n()
/** i18n 翻译函数 */
const { t } = useI18n()
/** Pinia：租户上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()
/** Pinia：字典缓存 */
const dictDataStore = useDictDataStore()

/** Create 字段名列表（与 formState 键对齐） */
const formFields = [
  'tenantCode',
  'companyCode',
  'cultureCode',
  'plantCode',
  'meetingCode',
  'meetingTitle',
  'meetingType',
  'meetingStatus',
  'startTime',
  'endTime',
  'location',
  'meetingLink',
  'meetingAgenda',
  'meetingTags',
  'organizerId',
  'organizerName',
  'deptId',
  'deptName',
  'maxAttendees',
  'reminderMinutes',
  'meetingRoomId',
  'meetingRoomName',
  'extField',
  'remark',
]
/** 表单内容区高度 class */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')

/**
 * 上下文隔离字段注入
 * @param target 表单数据
 * @param force 新增态强制覆盖
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (force || !target.companyCode) {
    target.companyCode = tenantStore.companyCode
  }
  if (force || !target.cultureCode) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    const nextPlant = tenantStore.currentCompanyRelatedPlant || ''
    if (nextPlant) {
      target.plantCode = nextPlant
    }
  }
}

/** 弹窗内 TaktSelect 下拉挂载到 body */
function resolveSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childMeetingAttendeeRows = ref<Record<string, unknown>[]>([])
const meetingAttendeeTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/**
 * 子表行是否已持久化
 * @param row 行数据
 */
function isPersistedMeetingAttendeeRow(row: Record<string, unknown>): boolean {
  const id = row.meetingAttendeeId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一子表行号 */
function allocateNextMeetingAttendeeLineNumber(): number {
  const rows = meetingAttendeeTableRef.value?.getRows?.() ?? childMeetingAttendeeRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 meetingAttendee 可编辑列 */
const meetingAttendeeFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: meetingAttendeePi.label('lineNumber'),
    editor: 'inputNumber',
    min: 0,
    width: 100,
  },
  {
    key: 'userId',
    title: meetingAttendeePi.label('userId'),
    width: 160,
    required: true,
  },
  {
    key: 'userName',
    title: meetingAttendeePi.label('userName'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'attendeeRole',
    title: meetingAttendeePi.label('attendeeRole'),
    width: 140,
  },
  {
    key: 'attendanceStatus',
    title: meetingAttendeePi.label('attendanceStatus'),
    width: 140,
  },
  {
    key: 'checkInTime',
    title: meetingAttendeePi.label('checkInTime'),
    editor: 'datePicker',
    showTime: true,
    valueFormat: 'YYYY-MM-DD HH:mm:ss',
    width: 180,
  },
  {
    key: 'checkOutTime',
    title: meetingAttendeePi.label('checkOutTime'),
    editor: 'datePicker',
    showTime: true,
    valueFormat: 'YYYY-MM-DD HH:mm:ss',
    width: 180,
  },
  {
    key: 'checkInMethod',
    title: meetingAttendeePi.label('checkInMethod'),
    width: 140,
  },
])

/**
 * 编辑态从 formData 同步子表行
 * @param val 主表 DTO
 */
function syncChildRowsFromFormData(val: Partial<Meeting & { meetingId?: string }> | null | undefined) {
  const rows = ((val as { attendees?: Record<string, unknown>[] })?.attendees ?? []) as Record<string, unknown>[]
  childMeetingAttendeeRows.value = rows
}

function createDefaultMeetingAttendeeRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextMeetingAttendeeLineNumber(),
    userId: '',
    userName: '',
    attendeeRole: 0,
    attendanceStatus: 0,
    checkInMethod: 0,
  }
}

/** 组装 Create/Update 载荷 */
function buildSubmitPayload() {
  const masterId = props.formData?.meetingId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    attendees: (meetingAttendeeTableRef.value?.getRows?.() ?? childMeetingAttendeeRows.value).map((row) => {
      const normalized: Record<string, unknown> = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        meetingId: masterId,
      }
      if (isUpdate && isPersistedMeetingAttendeeRow(row)) {
        normalized.meetingAttendeeId = row.meetingAttendeeId
      } else {
        delete normalized.meetingAttendeeId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO */
interface Props {
  formData?: Partial<Meeting & { meetingId?: string }> | null
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

const formRef = ref()
const formState = reactive<Record<string, unknown>>({})
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  meetingType: 0,
  meetingStatus: 0,
  maxAttendees: 0,
  reminderMinutes: 0,
}

function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

watch(
  () => props.formData,
  (val) => {
    if (val?.meetingId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
      delete (next as { attendees?: unknown }).attendees
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
      applyScopeDefaults(formState, true)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true },
)

watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture, tenantStore.currentCompanyRelatedPlant] as const,
  () => {
    if (!props.formData?.meetingId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 是否编辑态 */
const isEditMode = computed(() => !!props.formData?.meetingId)

useTaktFormNumbering({
  formState,
  isEdit: isEditMode,
  businessCodeField: 'meetingCode',
})

const rules = computed<Record<string, Rule[]>>(() => ({
  numberingRuleCode: [{
    validator: async (_rule, value) => {
      if (isEditMode.value) {
        return Promise.resolve()
      }
      if (!String(value ?? '').trim()) {
        return Promise.reject(t('common.page.form.numberingRuleRequired'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  meetingCode: [{
    validator: async (_rule, value) => {
      if (isEditMode.value) {
        return Promise.resolve()
      }
      if (!String(value ?? '').trim()) {
        return Promise.reject(t('common.page.form.numberingCodePreview'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  meetingTitle: [
    { required: true, message: pi.ph('meetingTitle'), trigger: 'blur' },
  ],
  meetingType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('meetingType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('meetingType'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  startTime: [
    { required: true, message: pi.ph('startTime'), trigger: 'change' },
  ],
  endTime: [
    { required: true, message: pi.ph('endTime'), trigger: 'change' },
  ],
}))

async function validate() {
  await formRef.value?.validate()
  await meetingAttendeeTableRef.value?.validate?.()
  return formState
}

function getValues(): Record<string, unknown> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    const scopedPlant = tenantStore.currentCompanyRelatedPlant || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }
  if (props.formData?.meetingId) {
    payload.meetingId = props.formData.meetingId
    delete payload.numberingRuleCode
  }
  return payload
}

function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState, !props.formData?.meetingId)
  childMeetingAttendeeRows.value = []
  meetingAttendeeTableRef.value?.resetRows?.()
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
