<template>
  <div class="takt-form-designer">
    <FcDesigner
      ref="designerRef"
      :height="height"
      :config="designerConfig"
      :locale="designerLocale"
      v-bind="designerAttrsRest"
    />
  </div>
</template>

<script setup lang="ts">
import FcDesigner from '@form-create/antd-designer'
import fcDesignerLocaleEn from '@form-create/antd-designer/locale/en.js'
import fcDesignerLocaleZhCn from '@form-create/antd-designer/locale/zh-cn.js'
import { useI18n } from 'vue-i18n'

export type FormDesignerRule = Record<string, unknown>[]

const { locale } = useI18n()

/** FcDesigner 仅提供 zh-cn / en；zh-HK 与 zh-CN 共用简体包 */
const designerLocale = computed(() => {
  const code = String(locale.value)
  return code === 'zh-CN' || code === 'zh-HK' ? fcDesignerLocaleZhCn : fcDesignerLocaleEn
})

const props = withDefaults(
  defineProps<{
    height?: string
    modelValue?: string
    /** 设计器 config（FcDesigner config），透传给 @form-create/antd-designer */
    designerConfig?: Record<string, unknown>
    /** 额外透传给 fc-designer 的其他 props（不含 config/locale/handle） */
    designerAttrs?: Record<string, unknown>
  }>(),
  {
    height: '100vh',
    modelValue: '',
    designerConfig: () => ({}),
    designerAttrs: () => ({})
  }
)

const emit = defineEmits<{ 'update:modelValue': [value: string] }>()

const designerRef = ref<{
  getRule: () => FormDesignerRule
  getJson?: () => string
  setRule: (rule: FormDesignerRule) => void
} | null>(null)

function getRule(): FormDesignerRule {
  return designerRef.value?.getRule() ?? []
}

/** designerConfig：只做最小化可编辑注入，避免影响官方其他功能 */
const designerConfig = computed(() => {
  const input = props.designerConfig ?? {}
  return {
    // fc-designer 内部基于 fieldReadonly 决定表单属性控件是否可编辑（横向/竖向属于表单属性）
    fieldReadonly: false,
    nameReadonly: false,
    ...input
  }
})

/** 透传 menu、mask 等（剔除可能冲突的 config/handle/locale 字段） */
const designerAttrsRest = computed(() => {
  const { config: _c, handle: _h, locale: _l, ...rest } = (props.designerAttrs ?? {})
  return rest
})

/** 将 modelValue JSON 灌入设计器（须等 FcDesigner 挂载并完成内部初始化） */
function applyModelToDesigner(): void {
  const designer = designerRef.value
  if (!designer?.setRule) return

  const v = String(props.modelValue ?? '').trim()
  if (!v) {
    designer.setRule([])
    return
  }

  try {
    const rule = JSON.parse(v) as unknown
    if (Array.isArray(rule)) designer.setRule(rule as FormDesignerRule)
    else designer.setRule([])
  } catch {
    designer.setRule([])
  }
}

watch(
  () => props.modelValue,
  () => {
    nextTick(applyModelToDesigner)
  }
)

onMounted(() => {
  nextTick(applyModelToDesigner)
})

function syncToModel() {
  emit('update:modelValue', JSON.stringify(getRule()))
}

defineExpose({
  designerRef,
  getRule,
  getRuleJson: () => JSON.stringify(getRule()),
  setRule: (rule: FormDesignerRule) => designerRef.value?.setRule(rule),
  syncToModel
})
</script>

<style scoped>
.takt-form-designer {
  width: 100%;
  min-width: 900px;
}
</style>

<style src="./form-designer-theme.css"></style>
