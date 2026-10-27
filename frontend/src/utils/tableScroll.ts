// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/utils/tableScroll
// 文件名称：tableScroll.ts
// 创建时间：2026-06-07
// 创建人：Takt365(Cursor AI)
// 功能描述：Ant Design Vue 表格 scroll / 列呈现 / 单元格溢出省略纯函数；scroll.y 为布局高度（与数据行数无关）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TableColumnsType } from 'ant-design-vue'
import type { ColumnGroupType, ColumnType } from 'ant-design-vue/es/table/interface'
import { TAKT_LARGE_DATA_AUTO_THRESHOLD } from '@/utils/taktLargeData'

/** 表格 / 树：与下拉共用大数据自动阈值（别名兼容旧引用） */
export const TAKT_TABLE_AUTO_VIRTUAL_ROW_THRESHOLD = TAKT_LARGE_DATA_AUTO_THRESHOLD

type TableColumnItem = ColumnType<Record<string, unknown>> | ColumnGroupType<Record<string, unknown>>;

/** 行选择列占用宽度（与 Ant Design Table selection 列对齐） */
export const TAKT_TABLE_ROW_SELECTION_WIDTH = 48;

/** 未显式配置 width 时的默认列宽 */
export const TAKT_TABLE_DEFAULT_COLUMN_WIDTH = 120;

/** 单元格溢出省略内层包裹 class（与 table-ellipsis-base.css 对齐） */
export const TAKT_TABLE_CELL_ELLIPSIS_INNER_CLASS = 'takt-table-cell-ellipsis-inner';

/** 表格默认 scroll.y 布局场景（全局唯一配置入口） */
export type TaktTableScrollLayout =
  | 'page'
  | 'pageWithInnerTabs'
  | 'treeRight'
  | 'treeLeft'
  | 'masterDetailLr'
  | 'masterDetailTbMaster'
  | 'masterDetailTbDetail'
  | 'editable';

/**
 * 固定顶栏 TaktHeader 默认高度（px），与 layouts/side|top|mix 及 takt-header 默认 height=40 对齐
 */
export const TAKT_LAYOUT_HEADER_HEIGHT_PX = 40;

/**
 * 固定底栏 TaktFooter 默认高度（px），与 layouts 中 TaktFooter :height="40" 对齐
 */
export const TAKT_LAYOUT_FOOTER_HEIGHT_PX = 40;

/**
 * 顶栏与底栏之外、表格主体之上的页面壳预留（px）：多标签 TaktTabs(40) + 查询栏 + 工具栏 + 外置分页 + p-4 等
 * 与 layouts contentMaxHeight 中除 header 外的 84px（footer+标签）及 CRUD 页壳分区对齐
 */
export const TAKT_TABLE_PAGE_SHELL_BELOW_HEADER_PX = 220;

/**
 * 整页列表 scroll.y 视口总预留 = 固定 header + 固定 footer + 页面壳（等价历史 calc(100vh - 300px)）
 */
export const TAKT_TABLE_VIEWPORT_CHROME_RESERVE =
  TAKT_LAYOUT_HEADER_HEIGHT_PX + TAKT_LAYOUT_FOOTER_HEIGHT_PX + TAKT_TABLE_PAGE_SHELL_BELOW_HEADER_PX;

/**
 * 内容区内嵌 a-tabs 标签栏额外预留（px）；在 page 预留之上再减。
 * 默认标签栏约 46px，加页内间距后须把表体压回内容区，外置分页才留在可视区域。
 */
export const TAKT_TABLE_INNER_TABS_CHROME_PX = 88;

/**
 * @deprecated 请使用 computeTableScrollYPx；仅保留给仍传 :scroll="{ y: TAKT_VIEWPORT_TABLE_SCROLL_Y }" 的旧页面
 */
export const TAKT_VIEWPORT_TABLE_SCROLL_Y = `calc(100vh - ${TAKT_TABLE_VIEWPORT_CHROME_RESERVE}px)`;

/** 表单内嵌可编辑子表默认纵向滚动高度（px） */
export const TAKT_EDITABLE_TABLE_DEFAULT_SCROLL_Y = 240;

/** scroll.y 像素下限（避免视口过小时表格不可滚动） */
export const TAKT_TABLE_SCROLL_Y_MIN = 200;

/** 无 window 时的视口高度回退（SSR/单测） */
export const TAKT_TABLE_VIEWPORT_HEIGHT_FALLBACK = 800;

/** 左树虚拟滚动测量失败时的回退高度（px） */
export const TAKT_TREE_LEFT_VIRTUAL_HEIGHT_FALLBACK = 400;

/**
 * 是否启用 Ant Design Vue Table / Tree 虚拟滚动（默认开；超大数据强制开；❌ 不截断行数据）
 * @param rowCount 当前绑定行数（表格 dataSource.length 或树节点总数）
 * @param virtualProp 组件 virtual；true 强制开；false 未超阈值时可关；省略则默认开
 * @returns {boolean} 是否开启 virtual
 */
export function shouldUseTableVirtualScroll(
  rowCount: number,
  virtualProp?: boolean,
): boolean {
  const len = Number.isFinite(rowCount) && rowCount > 0 ? Math.floor(rowCount) : 0
  if (virtualProp === true) {
    return true
  }
  // 超大数据强制虚拟化（防呆）；不是「库里只能查这么多条」
  if (len >= TAKT_LARGE_DATA_AUTO_THRESHOLD) {
    return true
  }
  if (virtualProp === false) {
    return false
  }
  return true
}

/**
 * 统计树节点总数（含子孙；达到 stopAt 后提前结束，供虚拟化阈值判断）
 * @param nodes 树根列表
 * @param childrenField 子节点字段名
 * @param stopAt 达到后停止计数（默认阈值 + 1）
 * @returns {number} 节点数（可能截断为 stopAt）
 */
export function countTreeNodesForVirtualScroll(
  nodes: readonly Record<string, unknown>[] | null | undefined,
  childrenField = 'children',
  stopAt: number = TAKT_TABLE_AUTO_VIRTUAL_ROW_THRESHOLD + 1,
): number {
  if (!nodes?.length || stopAt <= 0) {
    return 0;
  }
  let count = 0;
  const stack: Record<string, unknown>[] = [...nodes];
  while (stack.length > 0 && count < stopAt) {
    const node = stack.pop();
    if (!node) {
      continue;
    }
    count += 1;
    const children = node[childrenField];
    if (Array.isArray(children) && children.length > 0) {
      for (let i = 0; i < children.length; i += 1) {
        const child = children[i];
        if (child && typeof child === 'object') {
          stack.push(child as Record<string, unknown>);
        }
      }
    }
  }
  return count;
}

/**
 * 解析用于计算的视口高度
 * @param viewportHeight 调用方传入的视口高度；缺省时读 window.innerHeight
 * @returns 有效视口像素高度
 */
export function resolveTableViewportHeight(viewportHeight?: number): number {
  if (typeof viewportHeight === 'number' && Number.isFinite(viewportHeight) && viewportHeight > 0) {
    return viewportHeight
  }
  if (typeof window !== 'undefined' && typeof window.innerHeight === 'number' && window.innerHeight > 0) {
    return window.innerHeight
  }
  return TAKT_TABLE_VIEWPORT_HEIGHT_FALLBACK
}

/**
 * 解析弹出窗体高度（仅 .ant-modal / .ant-modal-content / .ant-modal-body / host）
 * ❌ 禁止回落 window.innerHeight（浏览器视口 ≠ 弹出窗体）
 * @param hostEl 弹窗内任意宿主元素（如表格外包 div）
 * @returns 弹出窗体像素高度；尚未布局完成时为 0
 */
export function resolveFormHostViewportHeight(hostEl?: HTMLElement | null): number {
  if (hostEl == null) {
    return 0
  }
  const modalContent = hostEl.closest('.ant-modal-content') as HTMLElement | null
  if (modalContent != null && modalContent.clientHeight > 0) {
    return modalContent.clientHeight
  }
  const modal = hostEl.closest('.ant-modal') as HTMLElement | null
  if (modal != null && modal.clientHeight > 0) {
    return modal.clientHeight
  }
  const modalBody = hostEl.closest('.ant-modal-body') as HTMLElement | null
  if (modalBody != null && modalBody.clientHeight > 0) {
    return modalBody.clientHeight
  }
  if (hostEl.clientHeight > 0) {
    return hostEl.clientHeight
  }
  return 0
}

/**
 * 按视口高度 × 分子/分母计算表格 scroll.y（如窗体高 × 5/4）
 * @param numerator 分子（如 5）
 * @param denominator 分母（如 4）
 * @param viewportHeight 视口高度；缺省为 window
 * @returns scroll.y 像素值
 */
export function computeRatioScrollYPx(
  numerator: number,
  denominator: number,
  viewportHeight?: number,
): number {
  const vh = resolveTableViewportHeight(viewportHeight)
  const den = denominator > 0 ? denominator : 1
  const num = Number.isFinite(numerator) ? numerator : 1
  return Math.max(TAKT_TABLE_SCROLL_Y_MIN, Math.floor((vh * num) / den))
}

/**
 * 从弹出窗体实测高度计算 scroll.y = 窗体高度 × 分子/分母（如 5/4）
 * 窗体尚未布局时返回下限，❌ 不改用浏览器视口
 * @param hostEl 弹窗内宿主
 * @param numerator 分子
 * @param denominator 分母
 * @returns scroll.y 像素值
 */
export function computeFormHostRatioScrollYPx(
  hostEl: HTMLElement | null | undefined,
  numerator: number,
  denominator: number,
): number {
  const hostHeight = resolveFormHostViewportHeight(hostEl)
  if (hostHeight <= 0) {
    return TAKT_TABLE_SCROLL_Y_MIN
  }
  const den = denominator > 0 ? denominator : 1
  const num = Number.isFinite(numerator) ? numerator : 1
  return Math.max(TAKT_TABLE_SCROLL_Y_MIN, Math.floor((hostHeight * num) / den))
}

/** 左右主子表明细侧标题/工具栏/外置分页等占用（px，视口 fallback；组件内优先实测 chrome） */
export const TAKT_TABLE_MASTER_DETAIL_LR_DETAIL_CHROME_PX = 152;

/** TaktPagination 默认占用高度（px，与 takt-pagination size=middle 对齐） */
export const TAKT_TABLE_PAGINATION_HEIGHT_PX = 40;

/** Ant Design Table 表头高度回退（size=middle） */
export const TAKT_TABLE_HEADER_FALLBACK_PX = 56;

/** Ant Design Table 汇总行高度回退（size=middle，含 fixed summary） */
export const TAKT_TABLE_SUMMARY_ROW_HEIGHT_PX = 48;

/** 虚拟表横向滚动条高度（px）；scroll.y 须留出，避免被外壳 overflow:hidden 裁掉 */
export const TAKT_TABLE_HORIZONTAL_SCROLLBAR_PX = 12;

/**
 * 按已限高的表格外壳计算 scroll.y，扣掉表头、分页和横向滚动条
 * @param wrapEl 包住 TaktSingleTable 的容器
 * @param options.reservePagination 外壳内有分页但尚未渲染时仍预留分页高度
 * @returns 可放入容器的 scroll.y；容器尚未布局时为 0
 */
export function measureConstrainedTableScrollY(
  wrapEl: HTMLElement | null | undefined,
  options: { reservePagination?: boolean } = {},
): number {
  if (wrapEl == null || wrapEl.clientHeight <= 0) {
    return 0;
  }
  const headerEl = wrapEl.querySelector('.ant-table-header') as HTMLElement | null;
  const paginationEl = wrapEl.querySelector('.takt-pagination') as HTMLElement | null;
  const headerHeight =
    headerEl != null && headerEl.offsetHeight > 0
      ? headerEl.offsetHeight
      : TAKT_TABLE_HEADER_FALLBACK_PX;
  const paginationHeight =
    paginationEl != null && paginationEl.offsetHeight > 0
      ? paginationEl.offsetHeight
      : options.reservePagination
        ? TAKT_TABLE_PAGINATION_HEIGHT_PX
        : 0;
  const raw =
    wrapEl.clientHeight - headerHeight - paginationHeight - TAKT_TABLE_HORIZONTAL_SCROLLBAR_PX;
  return Math.max(1, Math.floor(raw));
}

/**
 * 左右主子表共享 scroll.y（以组件 pane 高度与两侧 chrome 实测为准，保证左右表体默认等高）
 * @param paneHeight 组件根或 pane 可用高度（px）
 * @param options.masterChromePx 左侧 master-toolbar 实测高度
 * @param options.detailChromePx 右侧 detailTitle + detail-toolbar 实测高度
 * @param options.paginationPx 表内分页占用（主表 showPagination 时）
 * @param options.tableHeaderPx 表头高度回退
 * @returns 共享 scroll.y（px）
 */
export function computeMasterDetailLrSharedScrollYPx(
  paneHeight: number,
  options: {
    masterChromePx?: number;
    detailChromePx?: number;
    paginationPx?: number;
    tableHeaderPx?: number;
  } = {},
): number {
  const masterChrome = options.masterChromePx ?? 0;
  const detailChrome = options.detailChromePx ?? 0;
  const chrome = Math.max(masterChrome, detailChrome);
  const pagination = options.paginationPx ?? 0;
  const header = options.tableHeaderPx ?? TAKT_TABLE_HEADER_FALLBACK_PX;
  const raw = paneHeight - chrome - pagination - header;
  return Math.max(TAKT_TABLE_SCROLL_Y_MIN, raw);
}

/**
 * 从已撑满父级的容器实测纵向滚动高度（左树整视口 / 右表扣除表头）
 * @param hostEl 左树 viewport 或右表 __body
 * @param options.subtractTableHeader 为 true 时扣除 .ant-table-header（右表）
 * @returns {number} a-tree.height / a-table scroll.y（px）
 */
export function measureFillHeightScrollYPx(
  hostEl: HTMLElement | null | undefined,
  options: { subtractTableHeader?: boolean } = {},
): number {
  if (hostEl == null || hostEl.clientHeight <= 0) {
    return TAKT_TABLE_SCROLL_Y_MIN;
  }
  let next = hostEl.clientHeight;
  if (options.subtractTableHeader === true) {
    const headerEl = hostEl.querySelector('.ant-table-header') as HTMLElement | null;
    const headerHeight =
      headerEl != null && headerEl.offsetHeight > 0
        ? headerEl.offsetHeight
        : TAKT_TABLE_HEADER_FALLBACK_PX;
    next -= headerHeight;
  }
  return Math.max(TAKT_TABLE_SCROLL_Y_MIN, Math.floor(next));
}

/**
 * 按布局场景与视口高度计算 scroll.y（像素，Ant Design Table 推荐写法）
 * @param layout 表格所在布局场景
 * @param viewportHeight 视口高度；缺省为 window.innerHeight
 * @returns scroll.y 像素值
 */
export function computeTableScrollYPx(
  layout: TaktTableScrollLayout = 'page',
  viewportHeight?: number,
): number {
  const vh = resolveTableViewportHeight(viewportHeight);
  let raw: number;
  switch (layout) {
    case 'masterDetailTbMaster':
      raw = Math.floor(vh * 0.4 - 72);
      break;
    case 'masterDetailTbDetail':
      raw = Math.floor(vh * 0.55 - 96);
      break;
    case 'masterDetailLr':
      // 左右主子表：扣除页壳与右栏明细标题/工具栏/分页，避免 scroll.y 过大导致表头表体横向错位
      raw = vh - TAKT_TABLE_VIEWPORT_CHROME_RESERVE - TAKT_TABLE_MASTER_DETAIL_LR_DETAIL_CHROME_PX;
      break;
    case 'editable':
      raw = TAKT_EDITABLE_TABLE_DEFAULT_SCROLL_Y;
      break;
    case 'pageWithInnerTabs':
      // 整页壳 + 内容区内嵌 a-tabs（个人中心等）
      raw = vh - TAKT_TABLE_VIEWPORT_CHROME_RESERVE - TAKT_TABLE_INNER_TABS_CHROME_PX;
      break;
    default:
      // page / treeRight / treeLeft：同一垂直带，innerHeight 减 header/footer/页壳
      raw = vh - TAKT_TABLE_VIEWPORT_CHROME_RESERVE;
      break;
  }
  return Math.max(TAKT_TABLE_SCROLL_Y_MIN, raw);
}

/**
 * 合并页面显式 scroll.y 与视口动态计算结果
 * @param scrollYOverride 页面 :scroll.y 覆盖值
 * @param viewportScrollYPx useTaktTableViewportScrollY 输出
 * @returns 传给 a-table 的 scroll.y
 */
export function resolveVerticalScrollY(
  scrollYOverride: number | string | undefined | null,
  viewportScrollYPx: number,
): number | string {
  if (scrollYOverride != null && scrollYOverride !== '') {
    return scrollYOverride;
  }
  return viewportScrollYPx;
}

/**
 * 按布局场景解析默认 scroll.y（像素）
 * @param layout 表格所在布局场景
 * @returns 默认纵向滚动高度（px）
 */
export function resolveDefaultTableScrollY(layout: TaktTableScrollLayout = 'page'): number {
  return computeTableScrollYPx(layout);
}

/** 表格 scroll 配置 */
export type TaktTableScrollConfig = {
  x?: number | string | true;
  y?: number | string;
};

/**
 * 解析单列数值宽度
 * @param column 列配置
 * @param fallback 无有效 width 时的回退宽度
 * @returns 像素宽度
 */
export function getTableColumnPixelWidth(
  column: { width?: string | number | null | undefined },
  fallback: number = TAKT_TABLE_DEFAULT_COLUMN_WIDTH,
): number {
  const width = column.width;
  if (typeof width === 'number' && width > 0) {
    return width;
  }
  if (typeof width === 'string' && width.trim()) {
    const parsed = Number.parseInt(width, 10);
    if (!Number.isNaN(parsed) && parsed > 0) {
      return parsed;
    }
  }
  return fallback;
}

/**
 * 累计列宽（含分组列 children）
 * @param columns 展示列
 * @param fallbackWidth 叶子列无 width 时的回退宽度
 * @returns 总像素宽度
 */
export function sumTableColumnsPixelWidth(
  columns: TableColumnsType,
  fallbackWidth: number = TAKT_TABLE_DEFAULT_COLUMN_WIDTH,
): number {
  if (!columns?.length) {
    return 0;
  }
  return columns.reduce((sum, column) => {
    const item = column as TableColumnItem;
    if ('children' in item && item.children?.length) {
      return sum + sumTableColumnsPixelWidth(item.children as TableColumnsType, fallbackWidth);
    }
    return sum + getTableColumnPixelWidth(item, fallbackWidth);
  }, 0);
}

/**
 * 列是否启用溢出省略（操作列强制否）
 * @param column 列配置
 * @returns 是否省略
 */
export function isTableColumnEllipsisEnabled(
  column: { key?: unknown; ellipsis?: unknown; className?: unknown } | null | undefined,
): boolean {
  if (column == null) {
    return false;
  }
  const key = column.key != null ? String(column.key) : '';
  if (key === 'action') {
    return false;
  }
  const className = column.className != null ? String(column.className) : '';
  if (className.split(/\s+/).includes('takt-action-column')) {
    return false;
  }
  if (column.ellipsis === false) {
    return false;
  }
  return column.ellipsis != null && column.ellipsis !== false;
}

/**
 * 解析单元格溢出悬停文案（仅标量；复杂 VNode 不设 title）
 * @param text bodyCell / 列文本
 * @returns title 或 undefined
 */
export function resolveTableCellEllipsisTitle(text: unknown): string | undefined {
  if (text == null || text === '') {
    return undefined;
  }
  if (typeof text === 'string' || typeof text === 'number' || typeof text === 'boolean') {
    const title = String(text).trim();
    return title || undefined;
  }
  return undefined;
}

/** a-table #bodyCell 插槽参数（与 Ant Design Vue 对齐的最小形态） */
export type TaktTableBodyCellSlotData = {
  text?: unknown;
  value?: unknown;
  record?: Record<string, unknown>;
  index?: number;
  renderIndex?: number;
  column?: ColumnType<Record<string, unknown>> & {
    customRender?: (data: {
      text: unknown;
      value: unknown;
      record?: Record<string, unknown>;
      index?: number;
      renderIndex?: number;
      column?: unknown;
    }) => unknown;
  };
};

/**
 * 表格已占用 #bodyCell 时的回退渲染：优先列 customRender，否则标量 text（禁止把对象 JSON 渲进单元格）
 * @param slotData bodyCell 插槽参数
 * @returns customRender 结果、标量文本或 null
 */
export function resolveTableBodyCellFallback(slotData: TaktTableBodyCellSlotData): unknown {
  const column = slotData.column;
  const customRender = column?.customRender;
  if (typeof customRender === 'function') {
    return customRender({
      text: slotData.text,
      value: slotData.value ?? slotData.text,
      record: slotData.record,
      index: slotData.index,
      renderIndex: slotData.renderIndex ?? slotData.index,
      column,
    });
  }
  const text = slotData.text;
  if (text != null && typeof text === 'object') {
    return null;
  }
  return text ?? null;
}

/**
 * 分析/推移表对齐：表头强制居中；表体按 column.align（默认 left）写 inline style，避免被 Ant Design / CSS 特异性覆盖
 * @param columns 列配置
 * @returns 带 customHeaderCell / customCell 的新列
 */
export function applyAnalysisTableColumnAlign(columns: TableColumnsType): TableColumnsType {
  if (!columns?.length) {
    return []
  }
  return columns.map((column) => {
    const group = column as ColumnGroupType<Record<string, unknown>>
    if (Array.isArray(group.children) && group.children.length > 0) {
      return {
        ...group,
        children: applyAnalysisTableColumnAlign(group.children as TableColumnsType),
      }
    }
    const col = column as ColumnType<Record<string, unknown>>
    const bodyAlign = col.align === 'center' || col.align === 'right' ? col.align : 'left'
    const prevHeaderCell = col.customHeaderCell
    const prevCell = col.customCell
    return {
      ...col,
      customHeaderCell: (columnConfig) => {
        const prev =
          typeof prevHeaderCell === 'function'
            ? (prevHeaderCell(columnConfig) as Record<string, unknown> | undefined)
            : ((prevHeaderCell as Record<string, unknown> | undefined) ?? {})
        const prevStyle =
          prev?.style && typeof prev.style === 'object' ? (prev.style as Record<string, unknown>) : {}
        return {
          ...prev,
          style: {
            ...prevStyle,
            textAlign: 'center',
          },
        }
      },
      customCell: (record, rowIndex, columnConfig) => {
        const prev =
          typeof prevCell === 'function'
            ? (prevCell(record, rowIndex, columnConfig) as Record<string, unknown> | undefined)
            : ((prevCell as Record<string, unknown> | undefined) ?? {})
        const prevStyle =
          prev?.style && typeof prev.style === 'object' ? (prev.style as Record<string, unknown>) : {}
        return {
          ...prev,
          style: {
            ...prevStyle,
            textAlign: bodyAlign,
          },
        }
      },
    }
  }) as TableColumnsType
}

/**
 * 为列补充默认 width 与 ellipsis（不修改原对象引用外的共享状态）
 * @param columns 列配置
 * @param defaultEllipsis 是否默认 ellipsis
 * @param defaultWidth 无 width 时的默认列宽
 * @returns 新列数组
 */
export function applyTableColumnPresentation(
  columns: TableColumnsType,
  defaultEllipsis: boolean,
  defaultWidth: number = TAKT_TABLE_DEFAULT_COLUMN_WIDTH,
): TableColumnsType {
  if (!columns?.length) {
    return [];
  }
  return columns.map((column) => {
    const processedColumn = { ...column } as ColumnType<Record<string, unknown>>;
    const key = processedColumn.key != null ? String(processedColumn.key) : '';
    const className = processedColumn.className != null ? String(processedColumn.className) : '';
    const isActionColumn = key === 'action' || className.split(/\s+/).includes('takt-action-column');
    if (!processedColumn.width) {
      processedColumn.width = defaultWidth;
    }
    if (isActionColumn) {
      processedColumn.ellipsis = false;
    } else if (defaultEllipsis && processedColumn.ellipsis === undefined) {
      // showTitle:false：由 TaktSingleTable 统一用 title + 内层 class 控制，避免双层原生 tip
      processedColumn.ellipsis = { showTitle: false };
    } else if (processedColumn.ellipsis === true) {
      processedColumn.ellipsis = { showTitle: false };
    }
    return processedColumn;
  });
}

/**
 * 解析 Ant Design Table 的 scroll 配置（横向必须为数值总和以启用固定列宽 + 横向滚动条）
 * @param options.columns 最终展示列（已含 width）
 * @param options.scroll 页面传入的 scroll 覆盖项
 * @param options.includeRowSelection 是否包含行选择列宽
 * @param options.enableVerticalScroll 是否配置 scroll.y（为 true 时与数据行数无关，空表亦保持固定高度）
 * @param options.scrollLayout 布局场景（未传 verticalScrollHeight 时用于默认 scroll.y）
 * @param options.verticalScrollHeight 显式覆盖默认 scroll.y（优先于 scrollLayout）
 * @returns scroll 对象
 */
export function resolveTableScrollConfig(options: {
  columns: TableColumnsType;
  scroll?: TaktTableScrollConfig;
  includeRowSelection?: boolean;
  enableVerticalScroll?: boolean;
  scrollLayout?: TaktTableScrollLayout;
  verticalScrollHeight?: number | string;
}): TaktTableScrollConfig {
  const {
    columns,
    scroll,
    includeRowSelection = false,
    enableVerticalScroll = false,
    scrollLayout = 'page',
    verticalScrollHeight,
  } = options;
  const resolvedVerticalHeight = verticalScrollHeight ?? resolveDefaultTableScrollY(scrollLayout);
  const config: TaktTableScrollConfig = { ...scroll };
  // max-content 会按单元格内容撑开列宽，破坏列 width/ellipsis；与未传 x 一样按列宽总和锁定横滚
  if (config.x == null || config.x === true || config.x === 'max-content') {
    let totalWidth = sumTableColumnsPixelWidth(columns);
    if (includeRowSelection) {
      totalWidth += TAKT_TABLE_ROW_SELECTION_WIDTH;
    }
    config.x = totalWidth > 0 ? totalWidth : TAKT_TABLE_DEFAULT_COLUMN_WIDTH;
  }
  if (enableVerticalScroll) {
    // 企业级规范：scroll.y 表示布局高度，与 dataSource 行数无关；空数据亦须固定高度避免页面抖动
    if (config.y == null || config.y === '') {
      config.y = resolvedVerticalHeight;
    } else if (typeof config.y === 'string') {
      const trimmed = config.y.trim();
      if (/^\d+(\.\d+)?$/.test(trimmed)) {
        config.y = Number.parseInt(trimmed, 10);
      } else if (/^\d+(\.\d+)?px$/i.test(trimmed)) {
        config.y = Number.parseInt(trimmed, 10);
      }
    }
  }
  return config;
}
