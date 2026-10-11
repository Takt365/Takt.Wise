// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/components/business/TaktTableBodyCellFallback
// 文件名称：index.ts
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：TaktSingleTable / TaktTreeRightTable 在已占用 #bodyCell 时回退渲染列 customRender
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { defineComponent, Fragment, h, isVNode, type PropType } from 'vue'
import {
  resolveTableBodyCellFallback,
  type TaktTableBodyCellSlotData,
} from '@/utils/tableScroll'

/**
 * 单元格默认内容：优先 customRender（操作列等），否则标量 text
 */
export default defineComponent({
  name: 'TaktTableBodyCellFallback',
  props: {
    /** a-table bodyCell 插槽参数 */
    slotData: {
      type: Object as PropType<TaktTableBodyCellSlotData>,
      required: true,
    },
  },
  setup(props) {
    return () => {
      const result = resolveTableBodyCellFallback(props.slotData)
      if (result == null || result === false) {
        return null
      }
      if (isVNode(result)) {
        return result
      }
      if (Array.isArray(result)) {
        return h(Fragment, null, result)
      }
      if (
        typeof result === 'string' ||
        typeof result === 'number' ||
        typeof result === 'boolean'
      ) {
        return String(result)
      }
      return null
    }
  },
})
