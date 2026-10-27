<template>

  <a-card

    class="takt-dashboard-module-card databoard-module-card"

    size="small"

    :body-style="bodyStyle"

  >

    <template #title>

      <a-tooltip

        :title="dragTooltip"

        placement="right"

      >

        <span class="takt-dashboard-module-card-drag-handle">

          <RiDraggable class="takt-dashboard-module-card__head-icon" />

        </span>

      </a-tooltip>

    </template>

    <template #extra>

      <div

        class="takt-dashboard-module-card-head-actions"

        :class="{ 'takt-dashboard-module-card-head-actions--visible': showHeadActions }"

      >

        <a-space :size="HEAD_CONTROL_GAP">

          <a-tooltip

            :title="displayTitle"

            placement="right"

          >

            <a-button

              type="text"

              class="takt-dashboard-module-card-head-btn takt-dashboard-module-card-title-trigger"

            >

              <template #icon>

                <RiInformationLine class="takt-dashboard-module-card__head-icon" />

              </template>

            </a-button>

          </a-tooltip>

          <a-tooltip

            v-if="showLayoutSwitch"

            :title="layoutTooltip"

            placement="right"

          >

            <a-dropdown

              trigger="click"

              @open-change="onLayoutDropdownOpenChange"

            >

              <a-button

                type="text"

                class="takt-dashboard-module-card-head-btn"

              >

                <template #icon>

                  <RiLayoutGridLine class="takt-dashboard-module-card__head-icon" />

                </template>

              </a-button>

              <template #overlay>

                <a-menu

                  :selecmed-keys="currentLayoutKey ? [currentLayoutKey] : []"

                  @click="onLayoutMenuClick"

                >

                  <a-menu-item

                    key="full"

                    :disabled="moduleSpan === 24"

                  >

                    <template #icon>

                      <RiLayoutRowLine />

                    </template>

                    {{ t('dashboard.data-board.page.layoutfullrow') }}

                  </a-menu-item>

                  <a-menu-item

                    key="half"

                    :disabled="moduleSpan === 12"

                  >

                    <template #icon>

                      <RiLayoutColumnLine />

                    </template>

                    {{ t('dashboard.data-board.page.layouthalfrow') }}

                  </a-menu-item>

                </a-menu>

              </template>

            </a-dropdown>

          </a-tooltip>

          <slot name="headActions" />

          <a-tooltip

            :title="t('dashboard.data-board.page.removemodule')"

            placement="right"

          >

            <a-button

              type="text"

              danger

              class="takt-dashboard-module-card-head-btn"

              @click="onRemove"

            >

              <template #icon>

                <RiDeleteBinLine class="takt-dashboard-module-card__head-icon" />

              </template>

            </a-button>

          </a-tooltip>

        </a-space>

      </div>

    </template>

    <slot />

  </a-card>

</template>

<script setup lang="ts">

import { useI18n } from 'vue-i18n'

import {

  RiDeleteBinLine,

  RiDraggable,

  RiInformationLine,

  RiLayoutColumnLine,

  RiLayoutGridLine,

  RiLayoutRowLine

} from '@remixicon/vue'

import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface'

import type { DataBoardModuleItem } from '@/types/dashboard/dataBoard.types'

import { DATA_BOARD_AVAILABLE_MODULES } from '../config'

/** 卡片头按钮间距（px） */

const HEAD_CONTROL_GAP = 4 as const

const props = defineProps<{

  module: DataBoardModuleItem

  /** 父级容器悬停（用于显隐头部按钮） */

  hovered?: boolean

}>()

const emit = defineEmits<{

  remove: []

  'change-span': [id: string, span: number]

}>()

const { t } = useI18n()

const displayTitle = computed(() => {

  const meta = DATA_BOARD_AVAILABLE_MODULES.find(m => m.key === props.module.moduleKey)

  return meta ? t(meta.titleKey) : props.module.moduleKey

})

const dragTooltip = computed(() => t('dashboard.data-board.page.dragtoreorder', { title: displayTitle.value }))

const moduleSpan = computed(() => props.module.span ?? 24)

const currentLayoutKey = computed(() => (moduleSpan.value === 24 ? 'full' : 'half'))

const currentLayoutLabel = computed(() =>

  moduleSpan.value === 24 ? t('dashboard.data-board.page.layoutfullrow') : t('dashboard.data-board.page.layouthalfrow')

)

const layoutTooltip = computed(() => `${t('dashboard.data-board.page.layoutlabel')}：${currentLayoutLabel.value}`)

const showLayoutSwitch = true

/** 鼠标悬停卡片 */

const isCardHovered = computed(() => props.hovered === true)

/** 布局下拉已打开（保持操作区可见） */

const isLayoutDropdownOpen = ref(false)

/** 是否显示头部操作按钮 */

const showHeadActions = computed(() => isCardHovered.value || isLayoutDropdownOpen.value)

/**

 * 布局下拉显隐

 * @param open 是否打开

 */

function onLayoutDropdownOpenChange(open: boolean): void {

  isLayoutDropdownOpen.value = open

}

function onLayoutMenuClick(info: MenuInfo) {

  const key = String(info.key)

  const span = key === 'full' ? 24 : 12

  if (span !== moduleSpan.value) emit('change-span', props.module.id, span)

}

const bodyStyle = computed(() => ({

  flex: '1 1 auto',

  minHeight: 0,

  padding: '12px 16px',

  overflow: 'auto',

  boxSizing: 'border-box',

}))

function onRemove() {

  emit('remove')

}

</script>

