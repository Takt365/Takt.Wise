<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-kanban/components -->
<!-- 文件名称：EcKanbanStageStrip.vue -->
<!-- 功能描述：设变看板各部门实施路径条（Pc→…→Qa→Te） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex flex-wrap gap-1">
    <span
      v-for="stage in orderedStages"
      :key="stage.deptCode"
      class="inline-flex items-center rounded px-1.5 py-0.5 text-xs"
      :class="stageClass(stage)"
      :title="stageTitle(stage)"
    >
      {{ deptLabel(stage.deptCode) }}
      <span class="ml-0.5 opacity-80">{{ stage.implementedCount }}/{{ stage.totalCount }}</span>
    </span>
  </div>
</template>

<script setup lang="ts">
/**
 * 设变看板实施路径条
 */
import { useI18n } from 'vue-i18n';
import { taktOrgDeptI18nKey } from '@/utils/naming';
import { TaktEcKanbanOrder } from '@/constants/logistics/ecExecCodes.constants';
import type { EcKanbanDeptStage } from '@/types/logistics/manufacturing/engineering-change/ecKanban.types';

const props = defineProps<{
  /** 各部门汇总 */
  deptStages: EcKanbanDeptStage[];
  /** 当前卡点部门 */
  currentDeptCode?: string | null;
}>();

const { t } = useI18n();

/** 看板顺序阶段 */
const orderedStages = computed(() => {
  const map = new Map(props.deptStages.map((s) => [s.deptCode, s]));
  return TaktEcKanbanOrder.map((code) => map.get(code) ?? { deptCode: code, implementedCount: 0, totalCount: 0 });
});

/**
 * 部门显示名
 * @param code 部门编码
 * @returns {string} 文案
 */
function deptLabel(code: string): string {
  const key = taktOrgDeptI18nKey(code);
  return key ? t(key) : '';
}

/**
 * 阶段样式
 * @param stage 阶段
 * @returns {string} Tailwind 类
 */
function stageClass(stage: EcKanbanDeptStage): string {
  const total = stage.totalCount ?? 0;
  const done = stage.implementedCount ?? 0;
  if (total > 0 && done >= total) {
    return 'bg-green-100 text-green-800 dark:bg-green-900/40 dark:text-green-200';
  }
  if (props.currentDeptCode && stage.deptCode === props.currentDeptCode) {
    return 'bg-orange-100 text-orange-800 ring-1 ring-orange-400 dark:bg-orange-900/40 dark:text-orange-200';
  }
  return 'bg-container text-text-secondary border border-border';
}

/**
 * 阶段 tooltip
 * @param stage 阶段
 * @returns {string} 说明
 */
function stageTitle(stage: EcKanbanDeptStage): string {
  return `${deptLabel(stage.deptCode)} ${stage.implementedCount}/${stage.totalCount}`;
}
</script>
