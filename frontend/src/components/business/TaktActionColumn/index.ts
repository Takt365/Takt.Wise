// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/components/business/TaktActionColumn
// 文件名称：index.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：表格操作列配置（权限过滤、More 下拉、按钮渲染）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { h } from 'vue';
import type { Component, VNode } from 'vue';
import type { TableColumnsType } from 'ant-design-vue';
import { Button, Space, Tooltip, Dropdown, Menu, MenuItem } from 'ant-design-vue';
import { MoreOutlined } from '@ant-design/icons-vue';
import { usePermissionStore } from '@/stores/identity/usePermissionStore';
import { isDemoBlockedAction } from '@/utils/taktDemoMode';
import { createLogger } from '@/utils/logger';
import { translateLocaleMessage } from '@/utils/taktI18nMessage';
import { emitNotification } from '@/utils/eventBus';
import './index.vue';

const actionColumnLogger = createLogger('takt-action-column');

/**
 * 构建下拉菜单项标签（图标 + 文本，单行）
 * @param action 操作项配置
 * @returns {VNode} 菜单标签节点
 */
function buildActionMenuLabel<TRow>(action: ActionColumnItem<TRow>): VNode {
  const children: Array<VNode | string> = [];
  if (action.icon) {
    if (typeof action.icon === 'string') {
      children.push(h('i', { class: `${action.icon} takt-action-column-menu-icon` }));
    } else {
      children.push(h(action.icon, { class: 'takt-action-column-menu-icon' }));
    }
  }
  if (action.label) {
    children.push(h('span', { class: 'takt-action-column-menu-text' }, action.label));
  }
  return h('span', { class: 'takt-action-column-menu-label' }, children);
}

/** 表格 customRender 传入的 record 在 Ant Design Vue 侧为宽松结构；与旧版 ActionColumn 回调一致 */
export type ActionRecord = Record<string, unknown>;

export interface ActionColumnItem<TRow = ActionRecord> {
  key: string;
  label?: string;
  /** 按钮形状：standard（标准，图标+文本）、plain（透明背景，图标或图标+文本）、circle（圆形，只显示图标） */
  shape?: 'standard' | 'plain' | 'circle';
  size?: 'small' | 'middle' | 'large';
  disabled?: boolean | ((record: TRow, index: number) => boolean);
  disabledFn?: (record: TRow, index: number) => boolean;
  loading?: boolean | ((record: TRow, index: number) => boolean);
  loadingFn?: (record: TRow, index: number) => boolean;
  /** 是否显示按钮，可以是布尔值或函数（根据记录动态判断） */
  visible?: boolean | ((record: TRow, index: number) => boolean);
  /** 图标组件或 CSS 类名（如 'ri-edit-line'） */
  icon?: Component | string;
  permission?: string;
  /** 按钮样式类名（如：takt-button-detail） */
  buttonClass?: string;
  onClick?: (record: TRow, index: number) => void;
}

export interface ActionColumnOptions<TRow = ActionRecord> {
  title?: string;
  width?: number | string;
  fixed?: boolean | 'left' | 'right';
  align?: 'left' | 'right' | 'center';
  actions: ActionColumnItem<TRow>[];
}

/**
 * 创建操作列配置
 * @param options 操作列选项；泛型 `TRow` 与本页表格行类型一致，便于 `visible` / `onClick` 等回调入参类型准确
 * @returns 表格列配置
 */
export function CreateActionColumn<TRow = ActionRecord>(
  options: ActionColumnOptions<TRow>,
): TableColumnsType[0] {
  const {
    title = translateLocaleMessage('common.action.operation'),
    width = 148,
    fixed = 'right',
    align = 'center',
    actions,
  } = options;

  const permissionStore = usePermissionStore();

  const toRow = (record: ActionRecord): TRow => record as unknown as TRow;

  return {
    key: 'action',
    title,
    width,
    fixed,
    align,
    ellipsis: false,
    className: 'takt-action-column',
    customRender: ({ record, index }: { record: ActionRecord; index: number }) => {
      const row = toRow(record);
      const filteredActions = actions.filter((action) => {
        try {
          if (action.visible !== undefined) {
            const isVisible =
              typeof action.visible === 'function' ? action.visible(row, index) : action.visible;
            if (!isVisible) {
              return false;
            }
          }
          if (action.permission) {
            const hasPerm = permissionStore.hasPermission(action.permission);
            if (!hasPerm) {
              return false;
            }
          }
          if (isDemoBlockedAction(action.key, action.permission)) {
            return false;
          }
          return true;
        } catch (error) {
          actionColumnLogger.error(
            '操作按钮检查失败',
            { action: 'filterAction', actionKey: action.key, record },
            error,
          );
          return false;
        }
      });

      const createButton = (action: ActionColumnItem<TRow>) => {
        const disabledRaw =
          typeof action.disabled === 'function'
            ? action.disabled(row, index)
            : action.disabled || (action.disabledFn ? action.disabledFn(row, index) : false);
        const loadingRaw =
          typeof action.loading === 'function'
            ? action.loading(row, index)
            : action.loading || (action.loadingFn ? action.loadingFn(row, index) : false);
        const disabled =
          Boolean(disabledRaw) || isDemoBlockedAction(action.key, action.permission);
        const loading = Boolean(loadingRaw);
        const buttonClass = [
          action.buttonClass || (action.key ? `takt-button-${action.key}` : undefined),
          action.shape === 'plain' ? 'takt-button-plain-borderless' : undefined,
          action.shape === 'plain' && !action.label ? 'takt-button-plain-icon-only' : undefined,
          action.shape === 'circle' ? 'takt-button-circle' : undefined,
        ]
          .filter(Boolean)
          .join(' ');
        const buttonProps = {
          class: buttonClass,
          size: (action.size || 'small') as 'small' | 'middle' | 'large',
          disabled,
          loading,
          onClick: () => {
            if (isDemoBlockedAction(action.key, action.permission)) {
              emitNotification(
                'warning',
                translateLocaleMessage('components.navigation.page.systemsetting.demohint'),
              );
              return;
            }
            if (action.onClick) {
              action.onClick(row, index);
            }
          },
        };
        const buttonChildren: Array<VNode | string> = [];
        if (action.icon) {
          if (typeof action.icon === 'string') {
            buttonChildren.push(h('i', { class: action.icon }));
          } else {
            buttonChildren.push(h(action.icon));
          }
        }
        if (action.shape !== 'circle' && action.shape !== 'plain' && action.label) {
          buttonChildren.push(action.label);
        }
        const button = h(Button, buttonProps, { default: () => buttonChildren });
        if (action.shape === 'plain' && action.label && action.icon) {
          return h(
            Tooltip,
            {
              title: action.label,
              getPopupContainer: (triggerNode: HTMLElement) => triggerNode.parentElement || document.body,
            },
            { default: () => button },
          );
        }
        return button;
      };

      const maxVisibleButtons = 3;
      const shouldShowMore = filteredActions.length > maxVisibleButtons;
      const visibleActions = shouldShowMore
        ? filteredActions.slice(0, maxVisibleButtons)
        : filteredActions;
      const moreActions = shouldShowMore ? filteredActions.slice(maxVisibleButtons) : [];
      const buttons: VNode[] = [];

      visibleActions.forEach((action) => {
        buttons.push(createButton(action));
      });

      if (shouldShowMore && moreActions.length > 0) {
        const menuItems = moreActions.map((action) => {
          const disabledRaw =
            typeof action.disabled === 'function'
              ? action.disabled(row, index)
              : action.disabled || (action.disabledFn ? action.disabledFn(row, index) : false);
          const loadingRaw =
            typeof action.loading === 'function'
              ? action.loading(row, index)
              : action.loading || (action.loadingFn ? action.loadingFn(row, index) : false);
          const menuItemDisabled =
            Boolean(disabledRaw || loadingRaw)
            || isDemoBlockedAction(action.key, action.permission);
          const menuItemClass = action.buttonClass || (action.key ? `takt-button-${action.key}` : '');
          return h(
            MenuItem,
            {
              key: action.key,
              class: menuItemClass,
              disabled: menuItemDisabled,
              onClick: () => {
                if (isDemoBlockedAction(action.key, action.permission)) {
                  emitNotification(
                    'warning',
                    translateLocaleMessage('components.navigation.page.systemsetting.demohint'),
                  );
                  return;
                }
                if (action.onClick && !menuItemDisabled) {
                  action.onClick(row, index);
                }
              },
            },
            { default: () => buildActionMenuLabel(action) },
          );
        });
        const menu = h(
          Menu,
          {
            class: 'takt-action-column-dropdown-menu',
            theme: 'light',
            selectable: false,
          },
          { default: () => menuItems },
        );
        const moreButton = h(
          Button,
          {
            class: 'takt-button-more takt-button-plain-borderless takt-button-plain-icon-only',
            size: 'small',
            onClick: (e: Event) => {
              e.stopPropagation();
            },
          },
          { default: () => h(MoreOutlined) },
        );
        const moreButtonWithTooltip = h(
          Tooltip,
          {
            title: translateLocaleMessage('common.page.button.more'),
            getPopupContainer: (triggerNode: HTMLElement) => triggerNode.parentElement || document.body,
          },
          { default: () => moreButton },
        );
        buttons.push(
          h(
            Dropdown,
            {
              trigger: ['click'],
              placement: 'bottomRight',
              getPopupContainer: () => document.body,
              overlayStyle: {
                zIndex: 1060,
              },
              overlayClassName: 'takt-action-column-dropdown',
            },
            {
              default: () => moreButtonWithTooltip,
              overlay: () => menu,
            },
          ),
        );
      }

      return h(Space, { wrap: false, size: 4 }, { default: () => buttons });
    },
  };
}
