// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useTaktMasterDetailLrScrollY.ts
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：takt-master-detail-table-lr 内 provide 共享 scroll.y（仅按主表区实测，选中从表后不改变主表高度）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import {
  inject,
  onBeforeUnmount,
  onMounted,
  provide,
  ref,
  watch,
  type InjectionKey,
  type Ref,
} from 'vue';
import {
  TAKT_TABLE_HEADER_FALLBACK_PX,
  TAKT_TABLE_PAGINATION_HEIGHT_PX,
  TAKT_TABLE_SCROLL_Y_MIN,
  TAKT_TABLE_SUMMARY_ROW_HEIGHT_PX,
  computeMasterDetailLrSharedScrollYPx,
} from '@/utils/tableScroll';

/** 左右主子表共享 scroll.y 注入键（takt-single-table scrollLayout=masterDetailLr 时 inject） */
export const TAKT_MASTER_DETAIL_LR_SCROLL_Y_KEY: InjectionKey<Ref<number>> = Symbol('taktMasterDetailLrScrollY');

/** 左右主子表 scroll.y 测量所需 DOM 引用（仅以主表区为准） */
export interface TaktMasterDetailLrScrollMeasureRefs {
  /** 组件根节点 */
  rootRef: Ref<HTMLElement | null | undefined>;
  /** 左侧 master-toolbar 包裹层 */
  masterToolbarWrapRef: Ref<HTMLElement | null | undefined>;
  /** 左侧 __table-body */
  masterTableBodyRef: Ref<HTMLElement | null | undefined>;
  /** 主表是否显示底部分页（与 TaktSingleTable showPagination 一致） */
  includeMasterPagination: Ref<boolean>;
}

/** 左右主子表 scroll.y 测量选项 */
export interface MeasureMasterDetailLrTableScrollYOptions {
  /** 预留汇总行高度（px）；为 true 时用 DOM 实测或 TAKT_TABLE_SUMMARY_ROW_HEIGHT_PX */
  reserveSummaryRow?: boolean;
  /** 显式预留底部高度（px），优先于 reserveSummaryRow 的 DOM 实测 */
  reserveBottomPx?: number;
}

/**
 * 从 __table-wrap / __table-body 内 TaktSingleTable 实测 scroll.y
 * @param hostEl 表格外层容器
 * @param options 汇总行等底部预留
 * @returns scroll.y（px）
 */
export function measureMasterDetailLrTableScrollY(
  hostEl: HTMLElement,
  options: MeasureMasterDetailLrTableScrollYOptions = {},
): number {
  const tableBody = hostEl.querySelector('.takt-single-table__body') as HTMLElement | null;
  if (!tableBody || tableBody.clientHeight <= 0) {
    return TAKT_TABLE_SCROLL_Y_MIN;
  }
  const headerEl = tableBody.querySelector('.ant-table-header') as HTMLElement | null;
  const headerHeight = headerEl?.offsetHeight ?? TAKT_TABLE_HEADER_FALLBACK_PX;
  let bottomReserve = options.reserveBottomPx ?? 0;
  if (bottomReserve <= 0 && options.reserveSummaryRow) {
    const summaryEl = tableBody.querySelector('.ant-table-summary') as HTMLElement | null;
    bottomReserve = summaryEl?.offsetHeight ?? TAKT_TABLE_SUMMARY_ROW_HEIGHT_PX;
  }
  return Math.max(TAKT_TABLE_SCROLL_Y_MIN, tableBody.clientHeight - headerHeight - bottomReserve);
}

/**
 * 在 takt-master-detail-table-lr 内 provide 共享 scroll.y
 * @param refs 主表区测量 DOM 引用
 * @returns 共享 scroll.y（px）
 */
export function provideTaktMasterDetailLrScrollY(refs: TaktMasterDetailLrScrollMeasureRefs) {
  /** 主从表统一 scroll.y（px）；仅随主表区 / 视口变化，不因选中从表而重算 */
  const scrollYPx = ref(TAKT_TABLE_SCROLL_Y_MIN);

  let resizeObserver: ResizeObserver | null = null;

  /** 重算共享 scroll.y：只读主表 __table-body，从表沿用同一值 */
  function recalcSharedScrollY(): void {
    const masterEl = refs.masterTableBodyRef.value;
    if (masterEl && masterEl.clientHeight > 0) {
      const measured = measureMasterDetailLrTableScrollY(masterEl);
      if (measured !== scrollYPx.value) {
        scrollYPx.value = measured;
      }
      return;
    }
    const rootEl = refs.rootRef.value;
    if (!rootEl || rootEl.clientHeight <= 0) {
      return;
    }
    const masterChrome = refs.masterToolbarWrapRef.value?.offsetHeight ?? 0;
    const paginationPx = refs.includeMasterPagination.value ? TAKT_TABLE_PAGINATION_HEIGHT_PX : 0;
    const fallback = computeMasterDetailLrSharedScrollYPx(rootEl.clientHeight, {
      masterChromePx: masterChrome,
      detailChromePx: 0,
      paginationPx,
    });
    if (fallback !== scrollYPx.value) {
      scrollYPx.value = fallback;
    }
  }

  /** 开始监听（仅主表相关节点，避免从表 v-show 触发左侧高度变化） */
  function startObserve(): void {
    stopObserve();
    recalcSharedScrollY();
    resizeObserver = new ResizeObserver(() => {
      recalcSharedScrollY();
    });
    const targets = [
      refs.rootRef.value,
      refs.masterToolbarWrapRef.value,
      refs.masterTableBodyRef.value,
    ];
    for (const target of targets) {
      if (target) {
        resizeObserver.observe(target);
      }
    }
  }

  /** 停止监听 */
  function stopObserve(): void {
    resizeObserver?.disconnect();
    resizeObserver = null;
  }

  provide(TAKT_MASTER_DETAIL_LR_SCROLL_Y_KEY, scrollYPx);

  watch(
    () => [
      refs.rootRef.value,
      refs.masterToolbarWrapRef.value,
      refs.masterTableBodyRef.value,
      refs.includeMasterPagination.value,
    ],
    () => {
      startObserve();
    },
  );

  onMounted(() => {
    startObserve();
  });

  onBeforeUnmount(() => {
    stopObserve();
  });

  return scrollYPx;
}

/**
 * 注入左右主子表共享 scroll.y（仅 takt-master-detail-table-lr 子树内有效）
 * @returns 共享 scroll.y；不在主子表布局内时为 undefined
 */
export function useTaktMasterDetailLrScrollY(): Ref<number> | undefined {
  return inject(TAKT_MASTER_DETAIL_LR_SCROLL_Y_KEY, undefined);
}
