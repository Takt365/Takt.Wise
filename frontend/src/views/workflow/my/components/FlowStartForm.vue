<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/workflow/my/components -->
<!-- 文件名称：FlowStartForm.vue -->
<!-- 功能描述：发起流程表单，左侧选择模板列表、右侧填写审批内容/全景流程图（截图风格） -->
<!-- ======================================== -->

<template>
  <div class="flow-start-form">
    <div class="flow-start-form__layout">
      <!-- 左侧：选择模板 -->
      <aside class="flow-start-form__sidebar">
        <div class="flow-start-form__sidebar-title">
          {{ t('workflow.my.page.start.flow.form.template.list.title') }}
        </div>
        <div
          v-if="schemeLoading"
          class="flow-start-form__sidebar-loading"
        >
          <a-spin />
        </div>
        <ul
          v-else
          class="flow-start-form__list"
        >
          <li
            v-for="opt in schemeOptions"
            :key="opt.value"
            class="flow-start-form__list-item"
            :class="{ 'flow-start-form__list-item--active': form.processKey === opt.value }"
            @click="form.processKey = opt.value"
          >
            {{ opt.label }}
          </li>
          <li
            v-if="!schemeOptions.length"
            class="flow-start-form__list-empty"
          >
            {{ t('workflow.my.page.start.flow.form.process.placeholder') }}
          </li>
        </ul>
      </aside>
      <!-- 右侧：填写审批内容 / 全景流程图 -->
      <main class="flow-start-form__main">
        <a-tabs
          v-model:active-key="activeTab"
          class="flow-start-form__tabs"
        >
          <a-tab-pane
            :key="'fill'"
            :title="t('workflow.my.page.start.flow.form.fill.approval.content')"
          >
            <a-form
              ref="formRef"
              layout="vertical"
              :model="form"
              :rules="formRules"
              class="flow-start-form__form"
            >
              <a-form-item :label="t('workflow.my.page.start.flow.form.applicant.label')">
                <a-select
                  v-model:value="applicantEmployeeId"
                  :placeholder="t('workflow.my.page.start.flow.form.applicant.placeholder')"
                  :options="employeeSelectOptions"
                  :loading="employeeOptionsLoading"
                  allow-clear
                  show-search
                  :filter-option="filterEmployeeOption"
                  style="width: 100%"
                />
              </a-form-item>
              <a-form-item
                :label="t('entity.flowinstance.processtitle')"
                name="processTitle"
              >
                <a-input
                  v-model:value="form.processTitle"
                  :placeholder="t('workflow.my.page.start.flow.form.title.placeholder')"
                  allow-clear
                />
              </a-form-item>
              <template v-if="formConfigRule.length">
                <template v-if="formConfigLoading">
                  <a-form-item
                    :label="t('workflow.my.page.start.flow.form.form.data.label')"
                    class="flow-start-form__form-data-item"
                  >
                    <div class="flow-start-form-loading">
                      <a-spin />
                    </div>
                  </a-form-item>
                </template>
                <template v-else>
                  <a-form-item
                    v-for="item in formConfigRule"
                    :key="(item.field as string)"
                    :label="(item.title as string)"
                    class="flow-start-form__form-data-item"
                  >
                    <a-select
                      v-if="item.type === 'select'"
                      v-model:value="(frmDataModel as Record<string, string>)[(item.field as string)]"
                      :options="selectOptionsForFormRuleItem(item)"
                      :show-search="isEmployeeIdFormField(item)"
                      :filter-option="isEmployeeIdFormField(item) ? filterEmployeeOption : false"
                      allow-clear
                      style="width: 100%"
                    />
                    <a-date-picker
                      v-else-if="item.type === 'datePicker'"
                      :value="(frmDataModel as Record<string, string>)[(item.field as string)] ?? ''"
                      @update:value="(v: string | Dayjs) => { (frmDataModel as Record<string, string>)[(item.field as string)] = typeof v === 'string' ? v : v.format('YYYY-MM-DD') }"
                      value-format="YYYY-MM-DD"
                      style="width: 100%"
                    />
                    <a-textarea
                      v-else-if="item.type === 'textarea'"
                      :value="(frmDataModel as Record<string, string>)[(item.field as string)] ?? ''"
                      @update:value="(v: string | number) => { (frmDataModel as Record<string, string>)[(item.field as string)] = String(v ?? '') }"
                      :rows="((item.props as { rows?: number })?.rows) ?? 3"
                      style="width: 100%"
                    />
                    <a-input
                      v-else
                      :value="(frmDataModel as Record<string, string>)[(item.field as string)] ?? ''"
                      @update:value="(v: string | number) => { (frmDataModel as Record<string, string>)[(item.field as string)] = String(v ?? '') }"
                      allow-clear
                      style="width: 100%"
                    />
                  </a-form-item>
                </template>
              </template>
              <a-form-item
                v-else
                :label="t('entity.flowinstance.frmdata')"
                name="frmData"
              >
                <a-textarea
                  v-model:value="form.frmData"
                  :rows="4"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.flowinstance.frmdata') })"
                />
              </a-form-item>
            </a-form>
          </a-tab-pane>
          <a-tab-pane
            :key="'chart'"
            :title="t('workflow.my.page.start.flow.form.step3.flow.chart')"
          >
            <div class="flow-start-form__step-chart">
              <div
                v-if="!processContentForPreview?.trim()"
                class="flow-start-form-chart-empty"
              >
                {{ t('workflow.my.page.start.flow.form.flow.chart.empty') }}
              </div>
              <TaktFlowLogicDesigner
                v-else
                :key="'flow-preview-' + (form.processKey || '')"
                :model-value="processContentForPreview"
                :readonly="true"
              />
            </div>
          </a-tab-pane>
        </a-tabs>
      </main>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 发起流程表单：左侧选择模板列表（选中高亮+左蓝条），右侧 Tab「填写审批内容」/「全景流程图」；对外暴露 validate。
 */
import { useI18n } from 'vue-i18n'
import { storeToRefs } from 'pinia'
import type { Dayjs } from 'dayjs'
import { useUserStore } from '@/stores/identity/useUserStore'
import { getFlowFormById, getFlowFormList } from '@/api/workflow/flowForm.api'
import { getFlowSchemeList } from '@/api/workflow/flowScheme.api'
import { getEmployeeOptions } from '@/api/human-resource/personnel/employee.api'
import TaktFlowLogicDesigner from '@/components/business/TaktFlowLogicDesigner/index.vue'
import type { FlowScheme } from '@/types/workflow/flowScheme.types'
import type { FlowForm } from '@/types/workflow/flowForm.types'
import type { UserInfoResponse } from '@/types/identity/login.types'

const { t } = useI18n()

/** 表单配置规则（与 TaktFlowForm.FormConfig / 设计器 rule 一致），纯 Antdv 根据规则渲染，不依赖 form-create */
type FormConfigRule = Record<string, unknown>[]

/** 发起流程表单数据：流程 Key、标题、表单数据 JSON */
export interface FlowStartFormModel {
  processKey: string
  processTitle: string
  frmData: string
}

/** 父组件传入的表单绑定、流程下拉选项及加载态 */
interface Props {
  form: FlowStartFormModel
  schemeOptions: { label: string; value: string }[]
  schemeLoading?: boolean
}

const props = defineProps<Props>()
const form = props.form
const formRef = ref()
const activeTab = ref('fill')
const { userInfo } = storeToRefs(useUserStore())

/** 从登录用户信息解析展示名（优先级：员工姓名 > 昵称 > 用户名） */
function resolveUserDisplayName(u: UserInfoResponse | null | undefined): string {
  if (!u) return ''
  return (u.employeeName?.trim() || u.nickName?.trim() || u.userName || '').trim() || ''
}

const currentUserDisplayName = computed(() => resolveUserDisplayName(userInfo.value))
const applicantEmployeeId = ref<string>('')
const employeeOptions = ref<{ dictLabel: string; dictValue: string | number }[]>([])
const employeeOptionsLoading = ref(false)
const employeeSelectOptions = computed(() =>
  employeeOptions.value.map((o) => ({ label: o.dictLabel, value: String(o.dictValue) }))
)
function filterEmployeeOption(input: string, option: unknown) {
  if (!input?.trim()) return true
  const s = input.trim().toLowerCase()
  const label = (option as { label?: string })?.label ?? ''
  return String(label).toLowerCase().includes(s)
}

/** 请假表单等：field=employeeId 或 props.optionsSource=employee 时走员工选项接口 */
function isEmployeeIdFormField(item: Record<string, unknown>): boolean {
  const field = item.field as string | undefined
  const src = (item.props as { optionsSource?: string } | undefined)?.optionsSource
  return field === 'employeeId' || src === 'employee'
}

function selectOptionsForFormRuleItem(item: Record<string, unknown>): { label: string; value: string }[] {
  if (isEmployeeIdFormField(item)) return employeeSelectOptions.value
  return ((item.props as { options?: { label: string; value: string }[] })?.options) ?? []
}
function normalizeFrmDataModel(input: unknown): Record<string, string> {
  if (!input || typeof input !== 'object') return {}
  return Object.entries(input as Record<string, unknown>).reduce<Record<string, string>>((acc, [k, v]) => {
    acc[k] = v == null ? '' : String(v)
    return acc
  }, {})
}
const formConfigRule = ref<FormConfigRule>([])
const formConfigLoading = ref(false)
/** 动态表单项数据（与 formConfigRule 的 field 对应），同步到 form.frmData JSON */
const frmDataModel = ref<Record<string, string>>({})
const processContentForPreview = ref('')

const formRules = computed(() => ({
  processKey: [{ required: true, message: t('workflow.my.page.start.flow.form.process.required') }],
  processTitle: [],
  frmData: []
}))

/** 按 processKey 查已发布方案（list 服务端过滤 + isLatest） */
async function loadPublishedSchemeByProcessKey(processKey: string): Promise<FlowScheme | null> {
  const key = processKey.trim()
  const page = await getFlowSchemeList({
    pageIndex: 1,
    pageSize: 20,
    processKey: key,
    isLatest: 1,
    processStatus: 1,
    suspensionState: 1
  })
  const candidates = (page.data ?? []).filter((s) => s.processKey?.trim() === key)
  if (!candidates.length) return null
  return candidates[0]
}

/** 从方案关联 formId / formCode 拉取表单定义 */
async function loadFlowFormForScheme(scheme: FlowScheme): Promise<FlowForm | null> {
  if (scheme.formId) {
    return getFlowFormById(String(scheme.formId))
  }
  const formCode = scheme.formCode?.trim()
  if (!formCode) return null
  const page = await getFlowFormList({ pageIndex: 1, pageSize: 20, formCode })
  return page.data?.find((f) => f.formCode?.trim() === formCode) ?? null
}

/** 根据 processKey 拉取流程方案与表单配置，填充 formConfigRule 与 processContentForPreview */
async function loadFormConfig(processKey: string): Promise<void> {
  processContentForPreview.value = ''
  if (!processKey?.trim()) {
    formConfigRule.value = []
    return
  }
  formConfigLoading.value = true
  formConfigRule.value = []
  try {
    const key = processKey.trim()
    let flowForm: FlowForm | null = null
    const scheme = await loadPublishedSchemeByProcessKey(key)
    if (scheme?.processContent?.trim()) {
      processContentForPreview.value = scheme.processContent.trim()
    }
    if (scheme) {
      try {
        flowForm = await loadFlowFormForScheme(scheme)
      } catch {
        // ignore
      }
    }
    const configStr = flowForm?.formConfig?.trim()
    if (!configStr) return
    const rule = JSON.parse(configStr) as FormConfigRule
    if (!Array.isArray(rule) || !rule.length) return
    formConfigRule.value = rule
    try {
      frmDataModel.value = form.frmData?.trim() ? normalizeFrmDataModel(JSON.parse(form.frmData)) : {}
    } catch {
      frmDataModel.value = {}
    }
    const hasEmployeeField = rule.some((r) => (r as { field?: string }).field === 'employeeId')
    if (hasEmployeeField && applicantEmployeeId.value) {
      const cur = frmDataModel.value.employeeId
      if (cur === undefined || cur === null || String(cur).trim() === '') {
        frmDataModel.value = { ...frmDataModel.value, employeeId: applicantEmployeeId.value }
      }
    }
  } catch {
    formConfigRule.value = []
    frmDataModel.value = {}
  } finally {
    formConfigLoading.value = false
  }
}

watch(
  () => form.processKey,
  (key) => {
    if (!key?.trim()) {
      formConfigRule.value = []
      frmDataModel.value = {}
      form.frmData = ''
      processContentForPreview.value = ''
      return
    }
    loadFormConfig(key)
  }
)

watch(applicantEmployeeId, (id) => {
  if (!formConfigRule.value.some((r) => (r as { field?: string }).field === 'employeeId')) return
  frmDataModel.value = { ...frmDataModel.value, employeeId: id || '' }
})

/** 申请人默认当前登录用户：优先 userInfo.employeeId 与员工选项 dictValue 对齐；否则按姓名匹配 */
function applyDefaultApplicant(list: { dictLabel?: string; dictValue?: string | number }[] | null | undefined) {
  const opts = list ?? []
  const eid = userInfo.value?.employeeId?.trim()
  if (eid) {
    const byId = opts.find((o) => String(o.dictValue ?? '') === eid)
    if (byId) {
      applicantEmployeeId.value = eid
      return
    }
  }
  const u = userInfo.value
  const displayName = resolveUserDisplayName(u)
  if (!displayName) return
  const found = opts.find((o) => (o.dictLabel ?? '').trim() === displayName)
  if (found) applicantEmployeeId.value = String(found.dictValue ?? '')
}

onMounted(() => {
  employeeOptionsLoading.value = true
  getEmployeeOptions()
    .then((list) => {
      employeeOptions.value = (list ?? []).map((o) => ({
        dictLabel: o.dictLabel ?? '',
        dictValue: o.dictValue ?? ''
      }))
      applyDefaultApplicant(list)
    })
    .finally(() => {
      employeeOptionsLoading.value = false
    })
  if (form.processKey?.trim()) loadFormConfig(form.processKey)
})

const APPLICANT_FIELD = '申请人'

function getApplicantDisplayName(): string {
  const id = applicantEmployeeId.value
  if (!id?.trim()) return ''
  const o = employeeOptions.value.find((e) => String(e.dictValue) === id)
  return (o?.dictLabel ?? '').trim() || ''
}

function syncFrmDataModelToFrmData(): void {
  if (formConfigRule.value.length) {
    const data = { ...frmDataModel.value, [APPLICANT_FIELD]: getApplicantDisplayName() || currentUserDisplayName.value }
    form.frmData = JSON.stringify(data)
  }
}

async function validate(): Promise<boolean> {
  if (!form.processKey?.trim()) {
    const { message } = await import('ant-design-vue')
    message.error(t('workflow.my.page.start.flow.form.process.required'))
    return false
  }
  const id = (applicantEmployeeId.value ?? '').trim()
  if (!id) {
    const { message } = await import('ant-design-vue')
    message.error(t('workflow.my.page.start.flow.form.applicant.required'))
    return false
  }
  syncFrmDataModelToFrmData()
  try {
    await formRef.value?.validate()
    return true
  } catch {
    return false
  }
}

defineExpose({
  validate
})
</script>

<style scoped lang="css">
.flow-start-form {
  min-height: 420px;
}

.flow-start-form__layout {
  display: flex;
  gap: 0;
  min-height: 420px;
}

.flow-start-form__sidebar {
  width: 260px;
  flex-shrink: 0;
  background: var(--ant-color-fill-quaternary);
  border-radius: var(--ant-border-radius);
  overflow: hidden;
  display: flex;
  flex-direction: column;
}

.flow-start-form__sidebar-title {
  padding: 12px 16px;
  font-weight: 600;
  color: var(--ant-color-text);
  border-bottom: 1px solid var(--ant-color-border-secondary);
}

.flow-start-form__sidebar-loading {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 120px;
}

.flow-start-form__list {
  list-style: none;
  margin: 0;
  padding: 8px 0;
  overflow-y: auto;
  flex: 1;
}

.flow-start-form__list-item {
  padding: 10px 16px 10px 14px;
  margin: 0 8px;
  border-radius: var(--ant-border-radius);
  cursor: pointer;
  color: var(--ant-color-text-secondary);
  font-size: 14px;
  line-height: 1.5;
  border-left: 3px solid transparent;
  transition: background 0.2s, color 0.2s;

  &:hover {
    background: rgba(var(--ant-primary-color-rgb), 0.08);
    color: var(--ant-color-primary);
  }

  &--active {
    background: rgba(var(--ant-primary-color-rgb), 0.08);
    color: var(--ant-color-primary);
    border-left-color: var(--ant-color-primary);
  }
}

.flow-start-form__list-empty {
  padding: 16px;
  color: var(--ant-color-text-tertiary);
  font-size: 14px;
}

.flow-start-form__main {
  flex: 1;
  min-width: 0;
  padding-left: 20px;
  background: var(--ant-color-bg-container);
  overflow: visible;
}

.flow-start-form__tabs {
  min-height: 380px;
  overflow: visible;

  :deep(.ant-tabs-content) {
    overflow: visible;
  }
  :deep(.ant-tabs-tabpane) {
    overflow: visible;
  }
  :deep(.ant-tabs-content-holder) {
    overflow: visible;
  }
}

.flow-start-form__form {
  padding-top: 16px;
  padding-bottom: 24px;
  overflow: visible;
}

/* 仅对本组件直接声明的表单项（申请人、流程标题、表单数据）做间距，不侵入 form-create 内部 */
.flow-start-form__form > :deep(.ant-form-item) {
  margin-bottom: 24px;
}
.flow-start-form__form > :deep(.ant-form-item:last-child) {
  margin-bottom: 0;
}

.flow-start-form__form-data-item {
  margin-bottom: 24px;
}

.flow-start-form__step-chart {
  min-height: 320px;
  padding-top: 8px;
}

.flow-start-form-chart-empty {
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 280px;
  color: var(--ant-color-text-tertiary);
}

.flow-start-form-loading {
  min-height: 80px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.flow-start-form__step-chart :deep(.takt-flow-logic-designer) {
  min-height: 320px;
}
</style>
