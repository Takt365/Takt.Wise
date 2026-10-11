<template>
  <div class="element-form">
    <div class="element-form__empty">
      Element Plus 预览仅用于占位
    </div>
  </div>
</template>

<script setup lang="ts">
import type { FormDesignerRule } from './ruleToElementVue'

defineProps<{
  rule: FormDesignerRule
}>()
</script>

<style scoped>
.element-form {
  min-height: 120px;
  &__empty {
    color: rgba(0, 0, 0, 0.45);
    font-size: 13px;
  }
}
</style>

