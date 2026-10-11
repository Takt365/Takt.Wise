<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/workflow/scheme/components -->
<!-- 文件名称：SchemeForm.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：流程方案新增/编辑表单组件，含步骤与 ProcessContent 设计 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    layout="vertical"
    :model="form"
    :rules="formRules"
  >
    <a-steps
      :current="currentStep"
      :items="stepItems"
      class="scheme-steps"
    />
    <div class="steps-content">
      <!-- 步骤1：流程信息（与 AntFlow BasicSetting 职责对齐，绑定 FlowSchemeCreateOrUpdate） -->
      <div
        v-show="currentStep === 0"
        class="step-content"
      >
        <TaktFlowBasicSetting v-model:form="form" />
      </div>
      <!-- 步骤2：单选 关联表单 / 新建表单，均用 TaktFormDesigner 展示与设计 -->
      <div
        v-show="currentStep === 1"
        class="step-content"
      >
        <a-form-item :label="t('workflow.scheme.page.link.form.title')">
          <a-radio-group
            v-model:value="linkFormMode"
            class="scheme-form__form-radio"
          >
            <a-radio value="link">
              {{ t('workflow.scheme.page.link.form.option.link') }}
            </a-radio>
            <a-radio value="new">
              {{ t('workflow.scheme.page.link.form.option.new') }}
            </a-radio>
          </a-radio-group>
        </a-form-item>
        <!-- 关联表单：表单选择器 -->
        <a-form-item
          v-if="linkFormMode === 'link'"
          :label="t('workflow.scheme.page.link.form.title')"
          name="formCode"
        >
          <a-spin :spinning="formListLoading">
            <template v-if="formList.length > 0">
              <a-select
                v-model:value="formSelectValue"
                style="width: 100%"
                :placeholder="t('workflow.scheme.page.select.form.placeholder')"
                allow-clear
                show-search
                :filter-option="filterFormOption"
                option-filter-prop="label"
                @change="onFormSelectChange"
              >
                <a-select-option
                  v-for="item in formList"
                  :key="item.flowFormId ?? item.formCode"
                  :value="item.formCode ?? ''"
                  :label="(item.formCode ?? '') + ' ' + (item.formName ?? '')"
                >
                  {{ item.formCode }} - {{ item.formName }}
                </a-select-option>
              </a-select>
            </template>
            <div
              v-else-if="!formListLoading"
              class="scheme-form__no-form"
            >
              <span class="scheme-form__hint">{{ t('workflow.scheme.page.no.form.hint') }}</span>
            </div>
          </a-spin>
        </a-form-item>
        <template v-if="linkFormMode === 'new'">
          <a-form-item
            :label="t('workflow.scheme.page.new.form.code.label')"
            required
          >
            <a-input
              v-model:value="newFormCode"
              :placeholder="t('workflow.scheme.page.new.form.code.placeholder')"
            />
          </a-form-item>
          <a-form-item
            :label="t('workflow.scheme.page.new.form.name.label')"
            required
          >
            <a-input
              v-model:value="newFormName"
              :placeholder="t('workflow.scheme.page.new.form.name.placeholder')"
            />
          </a-form-item>
        </template>
        <!-- 表单设计器：关联与新建均在此展示/设计 -->
        <div class="scheme-form__form-designer-section">
          <div class="scheme-form__form-designer-section__title">
            {{ linkFormMode === 'link' ? t('workflow.scheme.page.link.form.option.link') : t('workflow.scheme.page.link.form.option.new') }}
          </div>
          <div class="scheme-form__form-designer-section__body">
            <TaktFormDesigner
              ref="formDesignerRef"
              :key="'form-designer-' + linkFormMode + '-' + (form.formId ?? formSelectValue ?? newFormCode ?? 'new')"
              v-model="formDesignConfig"
              height="480px"
              :designer-config="formDesignerConfig"
            />
          </div>
        </div>
      </div>
      <!-- 步骤3：流程设计 -->
      <div
        v-show="currentStep === 2"
        class="step-content step-content-design"
      >
        <div class="scheme-designer-section">
          <div class="scheme-designer-section__title">
            {{ isEdit ? t('workflow.scheme.page.designer.label.edit') : t('workflow.scheme.page.designer.label.create') }}
          </div>
          <div class="scheme-designer-section__body">
            <TaktFlowAntDesigner
              ref="flowDesignerRef"
              :key="'flow-ant-designer-' + (form.flowSchemeId ?? 'new')"
              :model-value="form.processContent || ''"
              @update:model-value="(v: string) => { form.processContent = v }"
            />
          </div>
        </div>
      </div>
    </div>
    <div class="steps-action">
      <a-button
        v-if="currentStep > 0"
        style="margin-right: 8px"
        @click="prev"
      >
        {{ t('workflow.scheme.page.step.prev') }}
      </a-button>
      <a-button
        v-if="currentStep < steps.length - 1"
        type="primary"
        @click="next"
      >
        {{ t('workflow.scheme.page.step.next') }}
      </a-button>
      <a-button
        v-if="currentStep === steps.length - 1"
        type="primary"
        @click="handleDone"
      >
        {{ t('workflow.scheme.page.step.done') }}
      </a-button>
    </div>
  </a-form>
</template>

<script setup lang="ts">
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import TaktFlowAntDesigner from '@/components/business/TaktFlowAntDesigner/index.vue'
import TaktFlowBasicSetting from '@/components/business/TaktFlowAntDesigner/BasicSetting/TaktFlowBasicSetting.vue'
import TaktFormDesigner from '@/components/business/TaktFormDesigner/index.vue'
import { getFlowFormList, getFlowFormById, createFlowForm, updateFlowForm } from '@/api/workflow/flowForm.api'
import type { FlowSchemeFormModel } from '@/types/workflow/flowScheme.types'
import type { FlowForm, FlowFormUpdate } from '@/types/workflow/flowForm.types'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

const { t } = useI18n()
const tenantStore = useTenantStore()
const userStore = useUserStore()

// 父组件传入的表单数据（含 flowSchemeId 表示编辑）
interface Props {
  form: FlowSchemeFormModel
}

const props = defineProps<Props>()
const form = props.form

// 是否编辑（有 flowSchemeId 为编辑，反之为新增）
const isEdit = computed(() => !!form.flowSchemeId)
// 当前步骤
const currentStep = ref(0)
// Ant Design 表单实例
const formRef = ref()
// 流程设计器实例
const flowDesignerRef = ref<InstanceType<typeof TaktFlowAntDesigner> | null>(null)
// 关联表单设计器实例
const formDesignerRef = ref<InstanceType<typeof TaktFormDesigner> | null>(null)
// 新建表单编码与名称
const newFormCode = ref('')
const newFormName = ref('')

// 步骤配置：1 流程信息、2 关联表单、3 流程设计
const steps = computed(() => [
  { title: t('workflow.scheme.page.step.step1.flow.info'), content: 0 },
  { title: t('workflow.scheme.page.step.step2.select.form'), content: 1 },
  { title: t('workflow.scheme.page.step.step3.flow.design'), content: 2 }
])
const stepItems = computed(() => steps.value.map(item => ({ key: item.title, title: item.title })))

// 步骤2 单选：'link' 关联表单，'new' 新建表单
const linkFormMode = ref<'link' | 'new'>('link')
// 步骤2 表单设计器绑定内容
const formDesignConfig = ref<string>('')
// 设计器 config，按文档配置显隐
const formDesignerConfig = {
  showSaveBtn: true,
  showPreviewBtn: true,
  showJsonPreview: true,
  showLanguage: true,
  showInputData: true
}

// 选择表单：列表与当前选中
const formList = ref<FlowForm[]>([])
const formListLoading = ref(false)
const formSelectValue = ref<string | undefined>(form.formCode ?? undefined)

watch(
  () => form.formCode,
  (v) => { formSelectValue.value = v ?? undefined },
  { immediate: true }
)

// 表单下拉搜索：按 label 模糊匹配
const filterFormOption = (input: string, option?: { label?: string }) => {
  const label = option?.label ?? ''
  return label.toLowerCase().includes((input ?? '').toLowerCase())
}

// 关联表单选择变化：同步 form.formCode 与 form.formId
const onFormSelectChange = (value: unknown) => {
  const formCode = typeof value === 'string' ? value : undefined
  if (formCode) {
    form.formCode = formCode
  } else {
    delete form.formCode
  }
  const found = formCode ? formList.value.find(f => (f.formCode ?? '') === formCode) : undefined
  if (found?.flowFormId != null) {
    form.formId = String(found.flowFormId)
  } else {
    delete form.formId
  }
  if (linkFormMode.value === 'link') loadFormDetailIntoDesigner()
  else formDesignConfig.value = ''
}

// 根据 form.formId 拉取表单详情并写入 formDesignConfig
const loadFormDetailIntoDesigner = async () => {
  const id = form.formId
  if (!id || linkFormMode.value !== 'link') return
  try {
    const detail = await getFlowFormById(String(id))
    const raw = detail.formConfig
    formDesignConfig.value = typeof raw === 'string' && raw?.trim() ? raw : ''
  } catch (error: any) {
    logger.error('[Scheme Form] 加载表单详情失败:', error)
    formDesignConfig.value = ''
  }
}

// 拉取流程表单列表
const loadFormList = async () => {
  formListLoading.value = true
  try {
    const res = await getFlowFormList({ pageIndex: 1, pageSize: 500 })
    const list = res.data ?? []
    formList.value = list
    if (list.length === 0) linkFormMode.value = 'new'
    else if (linkFormMode.value === 'link' && form.formId) await loadFormDetailIntoDesigner()
    else if (linkFormMode.value === 'new') formDesignConfig.value = ''
  } catch (error: any) {
    logger.error('[Scheme Form] 加载表单列表失败:', error)
  } finally {
    formListLoading.value = false
  }
}

watch(linkFormMode, mode => {
  if (mode === 'new') {
    formDesignConfig.value = ''
    newFormCode.value = form.processKey ? `${form.processKey}_form` : ''
    newFormName.value = form.processName ? `${form.processName}表单` : ''
  } else if (form.formId) loadFormDetailIntoDesigner()
  else formDesignConfig.value = ''
})

// 挂载时拉取表单列表
onMounted(() => {
  loadFormList()
})

// 当前步骤需要校验的字段名
const stepFieldNames: Record<number, string[]> = {
  0: ['processKey', 'processName'],
  1: [],
  2: []
}

const formRules = computed(() => ({
  processKey: [{ required: true, message: t('common.page.form.placeholder.required', { field: t('entity.flowscheme.processkey') }) }],
  processName: [{ required: true, message: t('common.page.form.placeholder.required', { field: t('entity.flowscheme.processname') }) }]
}))

// 校验当前步骤需校验的字段，通过返回 true
const validateCurrentStep = async (): Promise<boolean> => {
  const fields = stepFieldNames[currentStep.value]
  if (!fields?.length) return true
  try {
    await formRef.value?.validateFields(fields)
    return true
  } catch {
    return false
  }
}

// 下一步
const next = async () => {
  const ok = await validateCurrentStep()
  if (!ok) return
  currentStep.value++
}

// 上一步
const prev = () => {
  currentStep.value--
}

// 完成：校验当前步骤通过则提示成功
const handleDone = async () => {
  const ok = await validateCurrentStep()
  if (!ok) return
  message.success(t('workflow.scheme.page.step.done'))
}

/**
 * 校验步骤 2（关联/新建表单）
 * @returns 是否通过
 */
function validateFormLinkStep(): boolean {
  formDesignerRef.value?.syncToModel?.()
  const config = formDesignConfig.value?.trim()
  if (!config) {
    currentStep.value = 1
    message.warning(t('workflow.scheme.page.form.config.required'))
    return false
  }
  if (linkFormMode.value === 'link') {
    if (!form.formId || !form.formCode?.trim()) {
      currentStep.value = 1
      message.warning(t('workflow.scheme.page.link.form.required'))
      return false
    }
    return true
  }
  if (!newFormCode.value?.trim() || !newFormName.value?.trim()) {
    currentStep.value = 1
    message.warning(t('workflow.scheme.page.new.form.required'))
    return false
  }
  return true
}

/**
 * 将关联/新建表单设计器内容持久化到 FlowForm（保存方案前调用）
 * @returns 是否成功
 */
async function persistFormBeforeSchemeSave(): Promise<boolean> {
  if (!validateFormLinkStep()) return false
  formDesignerRef.value?.syncToModel?.()
  const config = formDesignConfig.value.trim()
  const companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  try {
    if (linkFormMode.value === 'new') {
      const created = await createFlowForm({
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        companyDefaultCulture,
        formCode: newFormCode.value.trim(),
        formName: newFormName.value.trim(),
        formCategory: 1,
        formType: 0,
        formConfig: config,
        formVersion: 'v1.0.0',
        isDatasource: 0,
        sortOrder: 0,
        formStatus: 1
      })
      form.formId = created.flowFormId
      form.formCode = created.formCode
      formSelectValue.value = created.formCode
      linkFormMode.value = 'link'
      await loadFormList()
      return true
    }
    if (!form.formId) {
      message.warning(t('workflow.scheme.page.link.form.required'))
      return false
    }
    const detail = await getFlowFormById(String(form.formId))
    const updateDto: FlowFormUpdate = {
      flowFormId: detail.flowFormId,
      tenantCode: detail.tenantCode,
      companyCode: detail.companyCode,
      companyDefaultCulture,
      formCode: detail.formCode,
      formName: detail.formName,
      formCategory: detail.formCategory,
      formType: detail.formType,
      formConfig: config,
      formTemplate: detail.formTemplate,
      formVersion: detail.formVersion,
      isDatasource: detail.isDatasource,
      relatedDataBaseName: detail.relatedDataBaseName,
      relatedTableName: detail.relatedTableName,
      relatedFormField: detail.relatedFormField,
      sortOrder: detail.sortOrder,
      formStatus: detail.formStatus
    }
    await updateFlowForm(detail.flowFormId, updateDto)
    return true
  } catch (error: unknown) {
    logger.error('[Scheme Form] 持久化关联表单失败:', error)
    message.error(t('common.feedback.failed'))
    return false
  }
}

// 校验所有步骤；未通过时切换到对应步骤并返回 false
const validateAllSteps = async (): Promise<boolean> => {
  for (let i = 0; i < steps.value.length; i++) {
    const fields = stepFieldNames[i]
    if (fields?.length) {
      try {
        await formRef.value?.validateFields(fields)
      } catch {
        currentStep.value = i
        message.warning(t('workflow.scheme.page.step.validate.fail', { step: i + 1 }))
        return false
      }
    }
    if (i === 1 && !validateFormLinkStep()) return false
  }
  const pc = form.processContent?.trim()
  if (pc && flowDesignerRef.value?.validateDesign) {
    const ok = flowDesignerRef.value.validateDesign({ silent: true })
    if (!ok) return false
  }
  return true
}

// 重置当前步骤为 0
const resetSteps = () => {
  currentStep.value = 0
  newFormCode.value = ''
  newFormName.value = ''
}

defineExpose({
  currentStep,
  validateAllSteps,
  resetSteps,
  persistFormBeforeSchemeSave
})
</script>

<style scoped lang="css">
.scheme-steps {
  margin-bottom: 16px;
}
.steps-content {
  margin-top: 16px;
  min-height: 320px;
}
.step-content {
  margin-top: 0;
}
.step-content-design {
  min-height: 80vh;
}
.scheme-designer-section {
  margin-bottom: 16px;
  border: 1px solid var(--ant-color-border);
  border-radius: var(--ant-border-radius);
  overflow: hidden;
}
.scheme-designer-section__title {
  padding: 8px 12px;
  font-weight: 600;
  color: var(--ant-color-text-heading);
  background: var(--ant-color-fill-quaternary);
  border-bottom: 1px solid var(--ant-color-border);
}
/* 流程设计器所在区域：高度 80vh，使内部画布能撑满 */
.scheme-designer-section__body {
  padding: 12px;
  height: 80vh;
  min-height: 360px;
  background: var(--ant-color-bg-container);
  display: flex;
  flex-direction: column;
}
.scheme-designer-section__body .takt-flow-logic-designer {
  flex: 1;
  min-height: 0;
}
.scheme-form__form-radio {
  display: flex;
  gap: 16px;
}
.scheme-form__no-form {
  color: var(--ant-color-text-tertiary);
  font-size: 13px;
}
.scheme-form__hint {
  color: var(--ant-color-text-tertiary);
}
.scheme-form__form-designer-section {
  margin-top: 16px;
  margin-bottom: 16px;
  border: 1px solid var(--ant-color-border);
  border-radius: var(--ant-border-radius);
  overflow: hidden;
}
.scheme-form__form-designer-section__title {
  padding: 8px 12px;
  font-weight: 600;
  color: var(--ant-color-text-heading);
  background: var(--ant-color-fill-quaternary);
  border-bottom: 1px solid var(--ant-color-border);
}
.scheme-form__form-designer-section__body {
  padding: 12px;
  min-height: 520px;
  background: var(--ant-color-bg-container);
}
.steps-action {
  margin-top: 24px;
}
</style>

