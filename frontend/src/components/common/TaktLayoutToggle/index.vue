<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/components/common/TaktLayoutToggle -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：登录表单位置切换（默认图标收缩，悬停下拉；可选单选组） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-dropdown
    v-if="type === 'dropdown'"
    :trigger="trigger"
    placement="bottomRight"
    :mouse-enter-delay="0.15"
    v-bind="$attrs"
  >
    <a-button type="text" :size="size" :aria-label="t('common.page.layout.switch')">
      <template #icon>
        <ri-layout-left-line class="takt-remix-icon" v-if="currentPosition === 'left'" />
        <ri-layout-column-line class="takt-remix-icon" v-else-if="currentPosition === 'center'" />
        <ri-layout-right-line class="takt-remix-icon" v-else />
      </template>
    </a-button>

    <template #overlay>
      <a-menu :selected-keys="[currentPosition]" @click="handleMenuClick">
        <a-menu-item key="left">
          <span class="takt-toggle-menu-row">
            <ri-layout-left-line class="takt-remix-icon" />
            {{ t('common.page.layout.position.left') }}
          </span>
        </a-menu-item>
        <a-menu-item key="center">
          <span class="takt-toggle-menu-row">
            <ri-layout-column-line class="takt-remix-icon" />
            {{ t('common.page.layout.position.center') }}
          </span>
        </a-menu-item>
        <a-menu-item key="right">
          <span class="takt-toggle-menu-row">
            <ri-layout-right-line class="takt-remix-icon" />
            {{ t('common.page.layout.position.right') }}
          </span>
        </a-menu-item>
      </a-menu>
    </template>
  </a-dropdown>

  <a-radio-group
    v-else-if="type === 'radio'"
    :value="currentPosition"
    :size="radioSize"
    v-bind="$attrs"
    @change="handleRadioChange"
  >
    <a-radio-button value="left">
      <a-tooltip :title="t('common.page.layout.position.left')">
        <span class="takt-toggle-radio-item" :aria-label="t('common.page.layout.position.left')">
          <ri-layout-left-line class="takt-remix-icon" />
        </span>
      </a-tooltip>
    </a-radio-button>
    <a-radio-button value="center">
      <a-tooltip :title="t('common.page.layout.position.center')">
        <span class="takt-toggle-radio-item" :aria-label="t('common.page.layout.position.center')">
          <ri-layout-column-line class="takt-remix-icon" />
        </span>
      </a-tooltip>
    </a-radio-button>
    <a-radio-button value="right">
      <a-tooltip :title="t('common.page.layout.position.right')">
        <span class="takt-toggle-radio-item" :aria-label="t('common.page.layout.position.right')">
          <ri-layout-right-line class="takt-remix-icon" />
        </span>
      </a-tooltip>
    </a-radio-button>
  </a-radio-group>

  <template v-else-if="type === 'radio-item'">
    <a-radio-button value="left">
      <a-tooltip :title="t('common.page.layout.position.left')">
        <span class="takt-toggle-radio-item" :aria-label="t('common.page.layout.position.left')">
          <ri-layout-left-line class="takt-remix-icon" />
        </span>
      </a-tooltip>
    </a-radio-button>
    <a-radio-button value="center">
      <a-tooltip :title="t('common.page.layout.position.center')">
        <span class="takt-toggle-radio-item" :aria-label="t('common.page.layout.position.center')">
          <ri-layout-column-line class="takt-remix-icon" />
        </span>
      </a-tooltip>
    </a-radio-button>
    <a-radio-button value="right">
      <a-tooltip :title="t('common.page.layout.position.right')">
        <span class="takt-toggle-radio-item" :aria-label="t('common.page.layout.position.right')">
          <ri-layout-right-line class="takt-remix-icon" />
        </span>
      </a-tooltip>
    </a-radio-button>
  </template>
</template>

<script setup lang="ts">
/**
 * 登录表单位置切换
 */
import { useI18n } from 'vue-i18n';
import type { MenuProps } from 'ant-design-vue';
import type { RadioChangeEvent } from 'ant-design-vue/es/radio';
import { RiLayoutLeftLine, RiLayoutColumnLine, RiLayoutRightLine } from '@remixicon/vue';
import {
  readStoredLoginLayoutPosition,
  saveLoginLayoutPosition,
  type TaktLoginLayoutPosition,
} from '@/utils/taktLoginLayoutDom';

/** 展示形态 */
type TaktLayoutToggleType = 'dropdown' | 'radio' | 'radio-item';

interface Props {
  /** 展示形态 */
  type?: TaktLayoutToggleType;
  /** 下拉触发方式（仅 type=dropdown） */
  trigger?: ('click' | 'hover' | 'contextmenu')[];
  /** 默认位置（无 localStorage 时使用） */
  position?: TaktLoginLayoutPosition;
  /** 控件尺寸 */
  size?: 'small' | 'middle' | 'large';
}

const props = withDefaults(defineProps<Props>(), {
  type: 'dropdown',
  trigger: () => ['hover'],
  position: 'center',
  size: 'middle',
});

const emit = defineEmits<{
  'update:position': [value: TaktLoginLayoutPosition];
}>();

const { t } = useI18n();

const currentPosition = ref<TaktLoginLayoutPosition>(readStoredLoginLayoutPosition(props.position));

watch(
  () => props.position,
  (next) => {
    currentPosition.value = next;
  }
);

watch(currentPosition, (next) => {
  emit('update:position', next);
  saveLoginLayoutPosition(next);
});

/**
 * Radio 组尺寸映射
 */
const radioSize = computed<'default' | 'small' | 'large'>(() =>
  props.size === 'middle' ? 'default' : props.size
);

/**
 * 下拉菜单选中位置
 */
const handleMenuClick: MenuProps['onClick'] = ({ key }) => {
  if (key === 'left' || key === 'center' || key === 'right') {
    currentPosition.value = key;
  }
};

/**
 * 单选组变更
 * @param event Radio 变更事件
 */
function handleRadioChange(event: RadioChangeEvent): void {
  const value = event.target?.value;

  if (value === 'left' || value === 'center' || value === 'right') {
    currentPosition.value = value;
  }
}
</script>

<style scoped>
.takt-toggle-menu-row,
.takt-toggle-radio-item {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  line-height: 1;
}

:deep(.ant-btn),
:deep(.ant-dropdown) {
  display: inline-flex;
  align-items: center;
  justify-content: center;
}

:deep(svg) {
  color: var(--ant-color-text);
  fill: currentColor;
}
</style>
