/**
 * 将 form-create 规则转换为 Element Plus 的 .vue 单文件内容（占位实现）。
 * 当前工程主要使用 Ant Design Vue，Element 仅用于兼容预览展示。
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

export function generateVueSfc(_rule: FormDesignerRule): string {
  return `<template>
  <div class="generated-form">
    <div>Element Plus 代码生成占位</div>
  </div>
</template>

<script setup lang="ts">
// 由表单设计器导出（Element Plus 占位）
</script>
`
}

