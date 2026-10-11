<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/components/common/TaktDictTag -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：字典值标签（按 dictTypeCode / dictValue 解析样式与文案） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-tag :class="tagClass" v-bind="$attrs">
    <slot>{{ displayLabel }}</slot>
  </a-tag>
</template>

<script setup lang="ts">
/**
 * 字典标签
 */
import { useDictDataStore } from '@/stores/foundation/useDictDataStore';
import { createLogger } from '@/utils/logger';
import { translateLocaleMessage } from '@/utils/taktI18nMessage';
import type { TaktSelectOption } from '@/types/common.types';

const dictLogger = createLogger('takt-dict-tag');

interface Props {
  /** 字典选项（优先） */
  option?: TaktSelectOption | Record<string, unknown>;
  /** 字典值 */
  value?: string | number;
  /** 显式标签 */
  label?: string;
  /** 字典类型编码 */
  dictType?: string;
  /** 样式类索引（0-69，覆盖 option 内 cssClass / listClass） */
  color?: string;
  /** 标签尺寸 */
  size?: 'small' | 'middle' | 'large';
}

const props = withDefaults(defineProps<Props>(), {
  option: undefined,
  value: undefined,
  label: undefined,
  dictType: undefined,
  color: undefined,
  size: 'middle',
});

const dictDataStore = useDictDataStore();

/**
 * 当前匹配的字典项
 */
const currentOption = computed<TaktSelectOption | undefined>(() => {
  if (props.option) {
    return props.option as TaktSelectOption;
  }

  if (props.dictType && props.value !== undefined && props.value !== null) {
    return dictDataStore.getDictOption(props.value, props.dictType, false);
  }

  return undefined;
});

/**
 * 展示文案（sys_culture_code 用 DictLabel；其余走 i18nKey，数值段键用树解析避免 vue-i18n 把 .1/.3 当成列表下标）
 */
const displayLabel = computed(() => {
  if (props.label) {
    return props.label;
  }

  if (props.dictType === 'sys_culture_code' && currentOption.value?.dictLabel) {
    return currentOption.value.dictLabel;
  }

  if (currentOption.value?.i18nKey) {
    const translated = translateLocaleMessage(currentOption.value.i18nKey);
    if (translated && translated !== currentOption.value.i18nKey) {
      return translated;
    }
  }

  if (currentOption.value?.dictLabel) {
    return currentOption.value.dictLabel;
  }

  if (props.value !== undefined && props.value !== null && props.value !== '') {
    return String(props.value);
  }

  return '';
});

/**
 * 标签样式类（takt-dict-tag-{0-69} + 尺寸）
 */
const tagClass = computed(() => {
  const classMap: Record<string, boolean> = {
    'takt-dict-tag': true,
    [`takt-dict-tag-${props.size}`]: true,
  };

  let styleClassValue = 0;

  if (props.color) {
    const parsed = Number.parseInt(props.color, 10);

    if (!Number.isNaN(parsed) && parsed >= 0 && parsed <= 69) {
      styleClassValue = parsed;
    }
  } else {
    const rawClass = currentOption.value?.cssClass ?? currentOption.value?.listClass;
    const parsed = typeof rawClass === 'number' ? rawClass : Number.parseInt(String(rawClass ?? ''), 10);

    if (!Number.isNaN(parsed) && parsed >= 0 && parsed <= 69) {
      styleClassValue = parsed;
    }
  }

  classMap[`takt-dict-tag-${styleClassValue}`] = true;

  return classMap;
});

/**
 * 兜底加载字典（路由守卫未加载时）
 */
async function ensureDictLoaded(): Promise<void> {
  if (!props.dictType || props.value === undefined || props.value === null) {
    return;
  }

  if (dictDataStore.isCacheFresh || dictDataStore.loading) {
    return;
  }

  try {
    await dictDataStore.loadAllDictDataAsync();
    dictLogger.debug('兜底加载字典数据成功');
  } catch (error) {
    dictLogger.warn('兜底加载字典数据失败', { action: 'ensureDictLoaded' }, error);
  }
}

onMounted(() => {
  void ensureDictLoaded();
});

watch(
  () => [props.dictType, props.value] as const,
  () => {
    void ensureDictLoaded();
  }
);
</script>

<style scoped>
:deep(.ant-tag) {
  border: none !important;
}
</style>
