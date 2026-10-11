<!-- ======================================== -->
<!-- 项目名称：TaktWise -->
<!-- 命名空间：@/components/business/TaktFlowAntDesigner/node -->
<!-- 文件名称：TaktFlowAddNode.vue -->
<!-- 创建时间：2026-04-07 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：流程节点下方「+」添加操作（addType 1～6 与 AntFlow addNode 一致） -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->
<template>
  <div class="add-node-btn-box">
    <div class="add-node-btn">
      <a-popover
        v-model:open="visible"
        placement="rightTop"
        trigger="click"
        :get-popup-container="getWorkflowPopupContainer"
      >
        <template #content>
          <div class="add-node-popover-body">
            <a
              class="add-node-popover-item"
              @click="addType(1)"
            >
              <div class="item-wrapper">
                <span class="iconfont approver-icon"><RiShieldCheckLine class="takt-remix-icon" /></span>
                <p>{{ t('workflow.designer.page.approver') }}</p>
              </div>
            </a>
            <a
              class="add-node-popover-item"
              @click="addType(3)"
            >
              <div class="item-wrapper">
                <span class="item-icon parallel-icon"><RiSplitCellsHorizontal class="takt-remix-icon" /></span>
                <p>{{ t('workflow.designer.page.parallelapproval') }}</p>
              </div>
            </a>
            <a
              class="add-node-popover-item"
              @click="addType(2)"
            >
              <div class="item-wrapper">
                <span class="iconfont copyer-icon"><RiShareForwardLine class="takt-remix-icon" /></span>
                <p>{{ t('workflow.designer.page.copyer') }}</p>
              </div>
            </a>
          </div>
          <div class="add-node-popover-body">
            <a
              class="add-node-popover-item"
              @click="addType(4)"
            >
              <div class="item-wrapper">
                <span class="iconfont condition-icon"><RiGitBranchLine class="takt-remix-icon" /></span>
                <p>{{ t('workflow.designer.page.conditionbranch') }}</p>
              </div>
            </a>
            <a
              class="add-node-popover-item"
              @click="addType(5)"
            >
              <div class="item-wrapper">
                <span class="iconfont dynamic-icon"><RiPulseLine class="takt-remix-icon" /></span>
                <p>{{ t('workflow.designer.page.dynamiccondition') }}</p>
              </div>
            </a>
            <a
              class="add-node-popover-item"
              @click="addType(6)"
            >
              <div class="item-wrapper">
                <span class="iconfont parallel-cond-icon"><RiOrganizationChart class="takt-remix-icon" /></span>
                <p>{{ t('workflow.designer.page.conditionparallel') }}</p>
              </div>
            </a>
          </div>
        </template>
        <template #default>
          <a-button
            type="primary"
            shape="circle"
            class="takt-button takt-button-create add-node-popover-trigger"
            :aria-label="t('workflow.designer.page.addnodebutton')"
          >
            <template #icon>
              <RiAddLine class="takt-remix-icon" />
            </template>
          </a-button>
        </template>
      </a-popover>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import {
  RiAddLine,
  RiShieldCheckLine,
  RiSplitCellsHorizontal,
  RiShareForwardLine,
  RiGitBranchLine,
  RiPulseLine,
  RiOrganizationChart
} from '@remixicon/vue'
import {
  buildCreateNodeMap,
  type FlowTreeNode
} from '../config/taktFlowTree'

const { t } = useI18n()

const props = defineProps<{ childNodeP: FlowTreeNode | null }>()
const emit = defineEmits<{ 'update:childNodeP': [value: FlowTreeNode | null] }>()

const visible = ref(false)

/** 将浮层挂在流程设计器根节点下，便于 workflow.css 统一作用域生效 */
function getWorkflowPopupContainer(trigger: HTMLElement): HTMLElement {
  const root = trigger.closest('.takt-flow-ant-designer')
  return (root as HTMLElement | null) ?? document.body
}

/** 与 AntFlow addNode.vue createNodeMap 键 1~6 完全一致 */
const createNodeMap = computed(() => buildCreateNodeMap(t))

function addType(type: number) {
  visible.value = false
  const fn = createNodeMap.value.get(type)
  if (!fn) return
  const newNode = fn(props.childNodeP ?? null)
  emit('update:childNodeP', newNode)
}
</script>
