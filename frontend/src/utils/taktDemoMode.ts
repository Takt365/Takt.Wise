// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktDemoMode.ts
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：演示模式（Demo）只读判定；偏好落库后再生效拦截（开/关与水印同步）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { getSetting } from '@/setting';
import { emitNotification } from '@/utils/eventBus';
import { translateLocaleMessage } from '@/utils/taktI18nMessage';

/** Demo 下禁止的工具栏/操作列 key 或权限末段 */
const DEMO_BLOCKED_OPS = new Set([
  'create',
  'createrow',
  'update',
  'edit',
  'delete',
  'deleterow',
  'import',
  'source',
  'empty',
  'approve',
  'reject',
  'refuse',
  'revoke',
  'initiate',
  'start',
  'send',
  'sendmessage',
  'execute',
  'batch',
  'specialaccept',
]);

/**
 * 显式只读门闸（null=跟持久化 setting.demo；true/false=偏好落库后的生效态）
 */
let demoReadonlyEnforced: boolean | null = null;

/**
 * 切换 Demo 后、偏好落库前：暂不拦截写请求（避免一开就提示不能保存配置）
 */
let demoReadonlyPendingPersist = false;

/**
 * 读取已持久化偏好中的 demo 开关（localStorage，避免与 setting Store 循环依赖）
 * @returns {boolean} demo 是否打开
 */
function readDemoFlag(): boolean {
  return !!getSetting().demo;
}

/**
 * 用户切换 Demo：落库前暂缓只读拦截
 */
export function beginDemoSettingTransition(): void {
  demoReadonlyPendingPersist = true;
}

/**
 * 偏好已落盘（本地或服务端）后按 demo 生效只读；开启时可提示一次
 * @param demo 是否 Demo
 * @param options.notify 为 true 且 demo 开启时弹出只读提示
 */
export function commitDemoReadonlyAfterPersist(
  demo: boolean,
  options?: { notify?: boolean },
): void {
  demoReadonlyPendingPersist = false;
  demoReadonlyEnforced = !!demo;
  if (options?.notify && demo) {
    emitNotification(
      'warning',
      translateLocaleMessage('components.navigation.page.systemsetting.demohint'),
    );
  }
}

/**
 * 当前是否演示只读（落库前过渡态返回 false；否则跟门闸 / 已持久化 setting）
 * @returns {boolean} true=只读
 */
export function isDemoReadonly(): boolean {
  if (demoReadonlyPendingPersist) {
    return false;
  }
  if (demoReadonlyEnforced != null) {
    return demoReadonlyEnforced;
  }
  return readDemoFlag();
}

/**
 * HTTP 方法是否为写操作
 * @param method Axios method
 */
export function isMutatingHttpMethod(method?: string): boolean {
  const m = (method || 'get').toLowerCase();
  return m === 'post' || m === 'put' || m === 'patch' || m === 'delete';
}

/**
 * Demo 只读下仍允许的写请求（登录/鉴权/验证码/SignalR negotiate / 偏好 me 等）
 * @param url 请求 url（相对或绝对）
 */
export function isDemoWriteExemptUrl(url?: string): boolean {
  const path = String(url ?? '').toLowerCase();
  if (!path) {
    return false;
  }
  const exempt = [
    '/connect/',
    'oauth',
    'token',
    'captcha',
    'session/signout',
    '/login',
    'negotiate',
    '/hubs/',
    'hub?',
    // 偏好 /me：打开/关闭 Demo 后须能落库，再进入全站只读
    'taktuserpreferences/me',
  ];
  return exempt.some((p) => path.includes(p));
}

/**
 * 操作按钮/权限在 Demo 下是否应隐藏或禁用
 * @param actionKey 操作 key（如 update）
 * @param permission 权限码（如 identity:user:update）
 */
export function isDemoBlockedAction(actionKey?: string, permission?: string): boolean {
  if (!isDemoReadonly()) {
    return false;
  }
  const key = String(actionKey ?? '')
    .trim()
    .toLowerCase();
  if (key && DEMO_BLOCKED_OPS.has(key)) {
    return true;
  }
  const op = String(permission ?? '')
    .split(':')
    .pop()
    ?.trim()
    .toLowerCase();
  if (op && DEMO_BLOCKED_OPS.has(op)) {
    return true;
  }
  return false;
}
