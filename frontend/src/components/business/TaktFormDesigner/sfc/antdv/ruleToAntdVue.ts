/**
 * 将 form-create 设计器规则转换为纯 Ant Design Vue 的 .vue 单文件内容（不依赖 form-create）。
 * 表单控件映射：input -> a-input，select -> a-select，radio -> a-radio-group 等。
 */

export type FormDesignerRuleItem = {
  type?: string
  field?: string
  title?: string
  props?: Record<string, unknown>
  value?: unknown
  options?: Array<{ label?: string; value?: unknown }>
  validate?: Array<Record<string, unknown>>
  children?: FormDesignerRuleItem[]
}

export type FormDesignerRule = FormDesignerRuleItem[]

function escapeAttr(s: string): string {
  return String(s ?? '')
    .replace(/\\/g, '\\\\')
    .replace(/'/g, "\\'")
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
}

/** 根据 type 和 props 推断默认值 */
function defaultValue(item: FormDesignerRuleItem): unknown {
  if (item.value !== undefined && item.value !== null) return item.value
  const t = (item.type ?? '').toLowerCase()
  const props = item.props ?? {}
  if (t === 'checkbox' || (t === 'select' && props.mode === 'multiple') || t === 'upload') return []
  if (t === 'switch') return props.unCheckedValue ?? false
  if (t === 'inputnumber' || t === 'slider' || t === 'rate') return 0
  if (t === 'radio' || t === 'select' || t === 'cascader' || t === 'treeselect' || t === 'atreeelect') return undefined
  return ''
}

/** 将 form-create validate 转为 Ant Design Vue rules */
function toFormRules(validate: Array<Record<string, unknown>> | undefined): unknown[] {
  if (!validate?.length) return []
  return validate.map((v) => {
    const r: Record<string, unknown> = {}
    if (v.required) {
      r.required = true
      r.message = v.message ?? '请填写此项'
    }
    if (v.min !== undefined) {
      r.min = v.min
      r.message = v.message ?? `最少 ${v.min} 个字符`
    }
    if (v.max !== undefined) {
      r.max = v.max
      r.message = v.message ?? `最多 ${v.max} 个字符`
    }
    if (v.pattern !== undefined) {
      r.pattern = typeof v.pattern === 'string' ? new RegExp(v.pattern) : v.pattern
      r.message = v.message ?? '格式不正确'
    }
    if (v.type === 'email') {
      r.type = 'email'
      r.message = v.message ?? '请输入正确邮箱'
    }
    if (v.type === 'url') {
      r.type = 'url'
      r.message = v.message ?? '请输入正确链接'
    }
    return r
  }).filter((r) => Object.keys(r).length > 0)
}

/** 递归收集所有带 field 的项，用于 formState 和 formRules */
function collectFields(
  items: FormDesignerRuleItem[],
  formState: Record<string, unknown>,
  formRules: Record<string, unknown[]>
): void {
  for (const item of items) {
    if (item.field) {
      formState[item.field] = defaultValue(item)
      const rules = toFormRules(item.validate)
      if (rules.length) formRules[item.field] = rules
    }
    if (item.children?.length) collectFields(item.children, formState, formRules)
  }
}

/** 生成单个表单项的模板（a-form-item + 控件） */
function renderField(item: FormDesignerRuleItem): string {
  const field = item.field!
  const label = escapeAttr((item.title as string) ?? '')
  const props = item.props ?? {}
  const t = (item.type ?? '').toLowerCase()

  // input 系列（含 textarea、password）
  if (t === 'input' || t === 'textarea') {
    const type = (props.type as string) ?? (t === 'textarea' ? 'textarea' : 'text')
    if (type === 'textarea') {
      const placeholder = escapeAttr((props.placeholder as string) ?? '')
      const rows = (props.autoSize as boolean) ? '' : ` :rows="${Number(props.rows) || 3}"`
      return `<a-form-item label='${label}' name="${field}">
    <a-textarea v-model:value="formState.${field}" placeholder='${placeholder}'${rows} />
  </a-form-item>`
    }
    if (type === 'password') {
      const placeholder = escapeAttr((props.placeholder as string) ?? '')
      return `<a-form-item label='${label}' name="${field}">
    <a-input-password v-model:value="formState.${field}" placeholder='${placeholder}' />
  </a-form-item>`
    }
    const placeholder = escapeAttr((props.placeholder as string) ?? '')
    const extra = [
      props.disabled ? ' disabled' : '',
      props.allowClear ? ' allow-clear' : '',
      props.maxlength != null ? ` :maxlength="${Number(props.maxlength)}"` : '',
      props.showCount ? ' show-count' : ''
    ].filter(Boolean).join('')
    return `<a-form-item label='${label}' name="${field}">
    <a-input v-model:value="formState.${field}" placeholder='${placeholder}'${extra} />
  </a-form-item>`
  }

  if (t === 'inputnumber') {
    const placeholder = escapeAttr((props.placeholder as string) ?? '')
    const min = props.min != null ? ` :min="${Number(props.min)}"` : ''
    const max = props.max != null ? ` :max="${Number(props.max)}"` : ''
    const step = props.step != null ? ` :step="${Number(props.step)}"` : ''
    return `<a-form-item label='${label}' name="${field}">
    <a-input-number v-model:value="formState.${field}" placeholder='${placeholder}' style="width: 100%"${min}${max}${step} />
  </a-form-item>`
  }

  if (t === 'select') {
    const placeholder = escapeAttr((props.placeholder as string) ?? '请选择')
    const multi = props.mode === 'multiple' || props.mode === 'tags' ? ' mode="multiple"' : ''
    return `<a-form-item label='${label}' name="${field}">
    <a-select v-model:value="formState.${field}" placeholder='${placeholder}'${multi} style="width: 100%">
      <a-select-option v-for="(opt, i) in __selectOptions_${field}" :key="i" :value="opt.value">{{ opt.label }}</a-select-option>
    </a-select>
  </a-form-item>`
  }

  if (t === 'radio') {
    const isButton = props.optionType === 'button'
    const tag = isButton ? 'a-radio-button' : 'a-radio'
    return `<a-form-item label='${label}' name="${field}">
    <a-radio-group v-model:value="formState.${field}">
      <${tag} v-for="(opt, i) in __radioOptions_${field}" :key="i" :value="opt.value">{{ opt.label }}</${tag}>
    </a-radio-group>
  </a-form-item>`
  }

  if (t === 'checkbox') {
    return `<a-form-item label='${label}' name="${field}">
    <a-checkbox-group v-model:value="formState.${field}">
      <a-checkbox v-for="(opt, i) in __checkboxOptions_${field}" :key="i" :value="opt.value">{{ opt.label }}</a-checkbox>
    </a-checkbox-group>
  </a-form-item>`
  }

  if (t === 'switch') {
    const checkedChild = props.checkedChildren != null ? ` checked-children='${escapeAttr(String(props.checkedChildren))}'` : ''
    const unCheckedChild = props.unCheckedChildren != null ? ` un-checked-children='${escapeAttr(String(props.unCheckedChildren))}'` : ''
    return `<a-form-item label='${label}' name="${field}">
    <a-switch v-model:checked="formState.${field}" :checked-value="__switch_${field}_checked" :un-checked-value="__switch_${field}_unchecked"${checkedChild}${unCheckedChild} />
  </a-form-item>`
  }

  if (t === 'slider') {
    const min = props.min != null ? ` :min="${Number(props.min)}"` : ''
    const max = props.max != null ? ` :max="${Number(props.max)}"` : ''
    return `<a-form-item label='${label}' name="${field}">
    <a-slider v-model:value="formState.${field}"${min}${max} />
  </a-form-item>`
  }

  if (t === 'rate') {
    return `<a-form-item label='${label}' name="${field}">
    <a-rate v-model:value="formState.${field}" />
  </a-form-item>`
  }

  if (t === 'timepicker') {
    const placeholder = escapeAttr((props.placeholder as string) ?? '请选择时间')
    const format = (props.format as string) ?? 'HH:mm:ss'
    return `<a-form-item label='${label}' name="${field}">
    <a-time-picker v-model:value="formState.${field}" placeholder='${placeholder}' format='${escapeAttr(format)}' style="width: 100%" value-format="HH:mm:ss" />
  </a-form-item>`
  }

  if (t === 'timerange' || t === 'vg') {
    const placeholder = escapeAttr((props.placeholder as string) ?? '请选择时间')
    return `<a-form-item label='${label}' name="${field}">
    <a-time-picker v-model:value="formState.${field}" placeholder='${placeholder}' style="width: 100%" value-format="HH:mm:ss" />
  </a-form-item>`
  }

  if (t === 'datepicker') {
    const placeholder = escapeAttr((props.placeholder as string) ?? '请选择日期')
    const format = (props.format as string) ?? 'YYYY-MM-DD'
    const valueFormat = (props.valueFormat as string) ?? 'YYYY-MM-DD'
    const showTime = props.showTime ? ' show-time' : ''
    if (props.range) {
      return `<a-form-item label='${label}' name="${field}">
    <a-range-picker v-model:value="formState.${field}" placeholder="['${placeholder}', '${placeholder}']" format='${escapeAttr(format)}' value-format='${escapeAttr(valueFormat)}' style="width: 100%"${showTime} />
  </a-form-item>`
    }
    return `<a-form-item label='${label}' name="${field}">
    <a-date-picker v-model:value="formState.${field}" placeholder='${placeholder}' format='${escapeAttr(format)}' value-format='${escapeAttr(valueFormat)}' style="width: 100%"${showTime} />
  </a-form-item>`
  }

  if (t === 'daterange') {
    const placeholder = escapeAttr((props.placeholder as string) ?? '请选择日期')
    const format = (props.format as string) ?? 'YYYY-MM-DD'
    const valueFormat = (props.valueFormat as string) ?? 'YYYY-MM-DD'
    return `<a-form-item label='${label}' name="${field}">
    <a-range-picker v-model:value="formState.${field}" placeholder="['${placeholder}', '${placeholder}']" format='${escapeAttr(format)}' value-format='${escapeAttr(valueFormat)}' style="width: 100%" />
  </a-form-item>`
  }

  if (t === 'cascader') {
    const placeholder = escapeAttr((props.placeholder as string) ?? '请选择')
    return `<a-form-item label='${label}' name="${field}">
    <a-cascader v-model:value="formState.${field}" placeholder='${placeholder}' :options="__cascaderOptions_${field}" style="width: 100%" />
  </a-form-item>`
  }

  if (t === 'upload') {
    return `<a-form-item label='${label}' name="${field}">
    <a-upload v-model:file-list="formState.${field}" />
  </a-form-item>`
  }

  if (t === 'atreeelect' || t === 'treeselect') {
    const placeholder = escapeAttr((props.placeholder as string) ?? '请选择')
    return `<a-form-item label='${label}' name="${field}">
    <a-tree-select v-model:value="formState.${field}" placeholder='${placeholder}' style="width: 100%" :tree-data="__treeSelectData_${field}" />
  </a-form-item>`
  }

  // 未映射的输入类型：占位
  return `<a-form-item label='${label}' name="${field}">
    <a-input v-model:value="formState.${field}" placeholder='（未映射: ${escapeAttr(t)}）' disabled />
  </a-form-item>`
}

/** 递归生成模板片段 */
function renderRule(items: FormDesignerRuleItem[]): string {
  const parts: string[] = []
  for (const item of items) {
    const t = (item.type ?? '').toLowerCase()
    if (item.field) {
      parts.push(renderField(item))
      continue
    }
    if (t === 'fcrow') {
      const gutter = (item.props?.gutter as number) ?? 0
      const justify = (item.props?.justify as string) ?? 'start'
      const align = (item.props?.align as string) ?? 'top'
      const inner = item.children?.length ? renderRule(item.children) : ''
      parts.push(`  <a-row :gutter="${gutter}" justify="${justify}" align="${align}">\n${inner}\n  </a-row>`)
      continue
    }
    if (t === 'col' || t === 'ds') {
      const span = (item.props?.span as number) ?? 24
      const offset = (item.props?.offset as number) ?? 0
      const inner = item.children?.length ? renderRule(item.children) : ''
      parts.push(`  <a-col :span="${span}" :offset="${offset}">\n${inner}\n  </a-col>`)
      continue
    }
    if (t === 'adivider') {
      const orientation = (item.props?.orientation as string) ?? 'center'
      parts.push(`  <a-divider orientation="${orientation}" />`)
      continue
    }
    if (t === 'text' || t === 'ec') {
      const content = (item.title as string) ?? (item.children?.[0] as string) ?? ''
      parts.push(`  <div class="form-static-text">${escapeAttr(content)}</div>`)
      continue
    }
    if (t === 'aalert') {
      const msg = (item.props?.message as string) ?? (item.title as string) ?? ''
      const typ = (item.props?.type as string) ?? 'info'
      parts.push(`  <a-alert message='${escapeAttr(msg)}' type="${typ}" show-icon />`)
      continue
    }
    if (t === 'space') {
      const inner = item.children?.length ? renderRule(item.children) : ''
      parts.push(`  <a-space>\n${inner}\n  </a-space>`)
      continue
    }
    if (t === 'fctitle' || t === 'rc') {
      const content = (item.title as string) ?? (item.props?.title as string) ?? ''
      const level = (item.props?.level as number) ?? 5
      parts.push(`  <a-typography-title :level="${level}">${escapeAttr(content)}</a-typography-title>`)
      continue
    }
    if (t === 'acard') {
      const title = (item.props?.title as string) ?? ''
      const inner = item.children?.length ? renderRule(item.children) : ''
      parts.push(`  <a-card title='${escapeAttr(title)}'>\n${inner}\n  </a-card>`)
      continue
    }
    if (item.children?.length) {
      parts.push(renderRule(item.children))
    }
  }
  return parts.join('\n')
}

/** 生成 script 中需要注入的选项常量（select/radio/checkbox/switch/cascader/treeSelect） */
function buildOptionRefs(rule: FormDesignerRuleItem[]): { scriptLines: string[]; formStatePatch: Record<string, unknown> } {
  const scriptLines: string[] = []
  const formStatePatch: Record<string, unknown> = {}
  function walk(items: FormDesignerRuleItem[]) {
    for (const item of items) {
      const field = item.field
      const t = (item.type ?? '').toLowerCase()
      const opts = item.options ?? []
      const optArr = opts.map((o) => ({ label: String(o.label ?? o.value ?? ''), value: o.value ?? o.label }))
      if (t === 'select' && opts.length) {
        const key = field ?? 'anonymous'
        scriptLines.push(`const __selectOptions_${key} = ${JSON.stringify(optArr)}`)
        if (field) formStatePatch[field] = opts[0].value ?? opts[0].label
      }
      if (t === 'radio' && opts.length) {
        const key = field ?? 'anonymous'
        scriptLines.push(`const __radioOptions_${key} = ${JSON.stringify(optArr)}`)
        if (field) formStatePatch[field] = opts[0].value ?? opts[0].label
      }
      if (t === 'checkbox' && opts.length) {
        const key = field ?? 'anonymous'
        scriptLines.push(`const __checkboxOptions_${key} = ${JSON.stringify(optArr)}`)
        if (field) formStatePatch[field] = []
      }
      if (t === 'switch' && field) {
        const checked = item.props?.checkedValue ?? true
        const unchecked = item.props?.unCheckedValue ?? false
        scriptLines.push(`const __switch_${field}_checked = ${JSON.stringify(checked)}`)
        scriptLines.push(`const __switch_${field}_unchecked = ${JSON.stringify(unchecked)}`)
      }
      if (t === 'cascader' && field) {
        scriptLines.push(`const __cascaderOptions_${field} = ${JSON.stringify(item.options ?? [])}`)
      }
      if ((t === 'atreeelect' || t === 'treeselect') && field) {
        scriptLines.push(`const __treeSelectData_${field} = ${JSON.stringify(item.options ?? [])}`)
      }
      if (item.children?.length) walk(item.children)
    }
  }
  walk(rule)
  return { scriptLines, formStatePatch }
}

export interface RuleToAntdVueResult {
  template: string
  formState: Record<string, unknown>
  formRules: Record<string, unknown[]>
  scriptExtras: string[]
}

/**
 * 将 form-create 规则转为 Ant Design Vue 单文件组件所需的 template 与 script 数据。
 */
export function ruleToAntdVue(rule: FormDesignerRule): RuleToAntdVueResult {
  const items = Array.isArray(rule) ? rule : []
  const formState: Record<string, unknown> = {}
  const formRules: Record<string, unknown[]> = {}
  collectFields(items, formState, formRules)
  const { scriptLines, formStatePatch } = buildOptionRefs(items)
  Object.assign(formState, formStatePatch)
  const template = renderRule(items)
  return {
    template: template || '  <!-- 无表单项 -->',
    formState,
    formRules,
    scriptExtras: scriptLines
  }
}

/**
 * 生成完整 .vue 文件内容（template + script setup），不依赖 form-create。
 */
export function generateVueSfc(rule: FormDesignerRule): string {
  const { template, formState, formRules, scriptExtras } = ruleToAntdVue(rule)
  const formStateJson = JSON.stringify(formState, null, 2)
  const formRulesJson = JSON.stringify(formRules, null, 2)
  return `<template>
  <div class="generated-form">
    <a-form
      :model="formState"
      :rules="formRules"
      :label-col="{ span: 6 }"
      :wrapper-col="{ span: 18 }"
      layout="horizontal"
    >
${template}
    </a-form>
  </div>
</template>

<script setup lang="ts">
/** 由表单设计器导出，纯 Ant Design Vue 实现，不依赖 form-create */
import { reactive } from 'vue'

${scriptExtras.join('\n')}

const formState = reactive(${formStateJson})
const formRules = ${formRulesJson}
</script>

<style scoped>
.generated-form { padding: 16px; }
</style>
`
}

