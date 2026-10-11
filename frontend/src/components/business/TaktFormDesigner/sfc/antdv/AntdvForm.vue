<template>
  <div class="antdv-form">
    <form-create
      v-if="rule.length > 0"
      :rule="rule"
      :option="props.option"
    />
    <div
      v-else
      class="antdv-form__empty"
    >
      暂无表单项，请在设计器中添加
    </div>
  </div>
</template>

<script setup lang="ts">
import { FORM_CREATE_DEFAULT_OPTION } from '@/utils/constants/formCreate'
import type { FormDesignerRule } from './ruleToAntdVue'

const props = withDefaults(
  defineProps<{
    rule: FormDesignerRule
    option?: Record<string, unknown>
  }>(),
  { option: () => ({ ...FORM_CREATE_DEFAULT_OPTION }) }
)
</script>

<style scoped>
.antdv-form {
  min-height: 120px;
  &__empty {
    color: var(--ant-color-text-tertiary);
    font-size: 13px;
  }
}
</style>
